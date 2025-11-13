---
title: "Axle-Centric Kinematics Plan"
version: 0.2.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering & Simulation Working Group
reviewers:
  - Dynamics & Controls Team
approvers:
  - Project Coordinator
created: 2025-11-09
last_reviewed:
review_cycle: Quarterly
notes: Conceptual and mathematical plan for implementing the Axle-Centric Kinematics system used by AgOpenNext for 6-DoF simulation, visualization, and control.
---

# Axle-Centric Kinematics Plan

> This document defines the conceptual design and mathematical foundation for **Axle-Centric Kinematics** — the system that governs how tractors, implements, and trailers are modeled, simulated, and visualized within AgOpenNext.
> 
> It describes how every piece of equipment is modeled around **axles** as the primary reference frames, connected through **drawbars** and other linkage elements that define **6-DoF kinematic constraints**.  
> The goal is a **unified, physically consistent model** that supports *any* realistic configuration — from simple rigid implements to complex systems with multiple actively or passively steered axles, sliding three-point hitches, steering drawbars, and other articulated or adaptive mechanisms.  
>  
> This framework ensures that all equipment — real or simulated — behaves predictably under the same mathematical rules, enabling consistent integration between sensor fusion, simulation playback, and predictive section control.

---

## 1. Document Control

- **Purpose:**  
  Define the unified kinematics system that allows every machine and implement to be represented as a tree of **axles** connected through constrained joints, enabling deterministic positioning, visualization, and section-level prediction.

- **Audience:**  
  - Simulation 
  - Guidance and control  
  - Visualization and physics 
  - Hardware sensors

- **References:**  
  - `docs/governance/GOVERNANCE.md`   

---

## 2. Overview & Core Concepts

### 2.1 Big Idea

Every physical object — tractor, implement, trailer, or attached tool — is represented as a **hierarchy of axles**.  
Each **axle** forms a coordinate frame. **Drawbars** connect these frames through constrained **6-DoF joints** that define how one can move relative to another.

An **Axle-Centric Model** therefore consists of:
- **Axles**: primary frames of reference 
- **Wheels**: Each axle will by default model two independantly steerable wheels, constrained by various steering functions 
- **Drawbars**: rigid links of known length  
- **Hitches / Joints**: connection points with constrained degrees of freedom  
- **Sensors**: attached frames (GPS, IMU, row followers, camera, etc.)  
- **Sections**: planar or linear elements used for coverage and section control 
- **Snouts**: Defined elements you want to between existing rows
- **Operator Camera**: a defined frame that the 2D/3D visualization follows

The same structure supports both:
- **Simulation mode**, where each joint is integrated numerically.
- **Real mode**, where positions are updated from GPS and IMU data but respect the same relationships.

---

### 2.2 Design Principles

1. **Deterministic:** Purely kinematic; no heavy physics solver required.  
2. **Composable:** Any equipment chain can be built by connecting axles and joints.  
3. **6-DoF Native:** Each joint defines translation and rotation limits.  
4. **Lightweight:** Runs comfortably on a Raspberry Pi or embedded host.  
5. **Unified Math:** All transformations use **SE(3) operators** — a standardized set of 3D position-and-rotation transformations (the math that describes how one object’s frame relates to another). These operators handle translation, rotation, composition, and inversion consistently across simulation, visualization, and sensor fusion.


---

## 3. Axle-Centric Kinematics Framework

> **Note:**  
> In this model, **axles are defined directly on the ground plane**.  
> Wheels do not have physical height or radius—they act only as **directional constraints** that define steering and motion.  
> **Drawbars** serve as generalized rigid links: they can connect two frames, attach to one frame only, or even exist standalone to form structural geometry (such as booms or toolbars).  
> This approach keeps the math simple, deterministic, and fully compatible with both real sensor fusion and 3D visualization.

---

### 3.1 Coordinate Model Overview

The system consists of only two frame-owning entity types:

| Entity | Owns Frame? | Typical Use | Description |
|---------|--------------|-------------|--------------|
| **Axle** | ✅ | 1–N | Defines motion, steering, and drive direction. Always lies on the ground plane. |
| **Drawbar** | ✅ | 0–N | Rigid link or structure element. Connects two frames, attaches to one, or exists standalone. |

Everything else—sensors, implements, sections, and operator views—are **offsets** relative to one of these frames.

Each frame is an SE(3) transform:
$$
T_p^{\,i} =
\begin{bmatrix}
R_p^{\,i} & \mathbf{t}_p^{\,i} \\
0 & 1
\end{bmatrix}
$$

where $R_p^{\,i}$ is a 3×3 rotation matrix (orientation) and $\mathbf{t}_p^{\,i}$ is a translation vector (position).  

World transform:
$$
T_w^{\,i} = T_w^{\,p} \cdot T_p^{\,i}
$$

Motion always propagates through these relationships.

---

### 3.2 Axles

An **axle** is the primary motion node.  
It defines:

- A local frame $\mathcal{F}_a$ anchored to the terrain height.  
- Wheel offsets $\pm\mathbf{r}_w$ (purely lateral spacing).  
- Independent steering angles $\delta_L,\delta_R$.

Each axle can be **driven**, **passive**, or **steer-only** depending on its role.

#### Steering Geometry
Ackermann relation:
$$
\tan(\delta_L)=\frac{L}{R-\tfrac{W}{2}},\qquad
\tan(\delta_R)=\frac{L}{R+\tfrac{W}{2}},
$$
where $L$ = wheelbase, $W$ = track width, and $R$ = turning radius.

---

### 3.3 Drawbars and Structural Links

A **drawbar** is a rigid link or structure element with optional motion limits.  
Unlike a simple hitch, it may connect:

| Type | A | B | Example |
|------|---|---|----------|
| **Linked** | Frame | Frame | Tractor → Implement |
| **Anchored** | Frame | None | Toolbar or boom arm extending from an axle |
| **Independent** | None | None | Structural members used to define geometry (e.g., boom lattice) |

Each drawbar owns a local frame $\mathcal{F}_d$ and may optionally define **attachment points** at either end.  
Either or both may reference other frames.

Transform between endpoints:
$$
T_{A}^{\,B} =
\begin{bmatrix}
R_{A}^{\,B} & \mathbf{t}_{A}^{\,B} \\
0 & 1
\end{bmatrix}
$$

Translation and rotation limits:
$$
T_i \in [T_i^{min},T_i^{max}],\qquad
R_i \in [R_i^{min},R_i^{max}]
$$

If $T_i^{min}=T_i^{max}=0$, the axis is locked.

Optional variable length (telescoping drawbar):
$$
T_{base}^{\,tip} =
\begin{bmatrix}
I & [\,0,0,L(t)\,]^T\\
0 & 1
\end{bmatrix}.
$$

---

### 3.4 Wheel Constraints and Body Motion

Wheels define directional constraints for an axle.  
They don’t change height—they only control direction and speed in the ground plane.

Each wheel \(i\) defines:
- Heading $\hat{\mathbf{t}}_i$ (based on steering angle $\delta_i$)  
- Lateral $\hat{\boldsymbol{\ell}}_i$ (perpendicular)  
- Ground normal $\hat{\mathbf{n}} = [0,0,1]$

Let the axle origin be $\mathbf{p}_a$ and angular velocity $\boldsymbol{\omega}$.  
Contact point velocity:
$$
\mathbf{u}_i = \mathbf{v} + \boldsymbol{\omega}\times(\mathbf{q}_i-\mathbf{p}_a)
$$

**Constraints:**
- No lateral slip:
  $$
  \mathbf{u}_i\cdot\hat{\boldsymbol{\ell}}_i=0
  $$
- Drive speed (if powered):
  $$
  \mathbf{u}_i\cdot\hat{\mathbf{t}}_i=v_{drive,i}
  $$

All wheel constraints are combined into $A\boldsymbol{\xi}=\mathbf{b}$ and solved (weighted least squares) for twist $\boldsymbol{\xi}=[\mathbf{v};\boldsymbol{\omega}]$.

---

### 3.5 Steering Models

| Mode | Description | Example |
|-------|--------------|---------|
| **Commanded** | Input steering via Ackermann or rack calibration | Tractor front axle |
| **Passive (self-steer)** | Wheels align with velocity vector or hitch yaw | Wagon, trailing implement |
| **Active (controlled)** | Implement actuators steer to minimize path/yaw error | Steerable toolbar or boom |

Passive steering alignment:
$$
\delta_i = \mathrm{atan2}(v_y,v_x)
$$
or via hitch yaw:
$$
\delta_i = \mathrm{clamp}(k_\psi \psi_{hitch},\pm\delta_{max})
$$

Active implement steering:
$$
\delta_i = \mathrm{clamp}\!\left(\arctan\frac{2L_{pp} e_y}{L^2},\pm\delta_{max}\right)
$$

---

### 3.6 Integration and Propagation

Each axle integrates its solved velocities over $\Delta t$:

$$
\mathbf{p}(t+\Delta t)=\mathbf{p}(t)+\mathbf{v}(t)\,\Delta t
$$
$$
R(t+\Delta t)=\exp(\boldsymbol{\omega}(t)\Delta t)\,R(t)
$$

After integration:
1. Apply joint limits to drawbars.  
2. Recompute dependent drawbar or axle poses.  
3. Update any free drawbars (anchored or independent) with local constraints or control inputs.

---

### 3.7 Trailer and Implement Following

Implements without active steering follow through drawbar geometry:
$$
\mathbf{p}_t = \mathbf{p}_h - L_t
\begin{bmatrix}
\cos\theta_t\\\sin\theta_t\\0
\end{bmatrix},
\qquad
\theta_t = \mathrm{atan2}(y_h - y_t, x_h - x_t)
$$
For 3D terrain, replace $z=0$ with height $h(x,y)$.

---

### 3.8 Sensors and Fusion

Sensors (GNSS, IMU, camera, etc.) attach to any frame:
$$
T_a^{\,s}=
\begin{bmatrix}
R_a^{\,s}&\mathbf{t}_a^{\,s}\\0&1
\end{bmatrix},
\quad
T_w^{\,s,pred}=T_w^{\,a}T_a^{\,s}.
$$

Compare to measurement:
$$
\Delta T=T_w^{\,s,meas}(T_w^{\,s,pred})^{-1}.
$$

The fusion filter adjusts the parent frame using $\Delta T$ with small gain $K_f$.  
Vertical state anchors to ground height $z=h(x,y)$ unless explicitly overridden.

---

### 3.9 Calibration and Geometry Consistency

Multi-sensor systems depend on correct lever arms and drawbar lengths.

#### Strategies
1. **Manual (Default):** User measures geometry; fusion assumes correct.  
2. **One-Time Auto-Cal:** Record motion, minimize GPS residuals to solve for offsets.  
3. **Continuous Bias Correction:** Slowly adapts geometry via  
   $$
   \mathbf{t}_{cal}(t+1)=\mathbf{t}_{cal}(t)+K_b[\mathbf{t}_{meas}-\mathbf{t}_{pred}],
   $$
   where $K_b$ is small.

If residuals exceed tolerance, flag a **Geometry Mismatch Warning** and optionally disable multi-sensor blending.

---

### 3.10 Sections and Look-Ahead

Sections define endpoints $\mathbf{p}_1,\mathbf{p}_2$ on a host frame:
$$
\mathbf{p}_{1w}=T_w^{\,n}\mathbf{p}_1,\quad
\mathbf{p}_{2w}=T_w^{\,n}\mathbf{p}_2.
$$

Predictive paths use lead-axle velocity integration:
$$
\mathbf{p}(t+\tau)=\mathbf{p}(t)+\int_0^{\tau}R(t)\mathbf{v}(t)\,d\tau.
$$

Intersect with field polygons to perform anticipatory section on/off logic.

---

### 3.11 Operator and Visualization Frames

Operator/camera frame $\mathcal{F}_c$:
$$
T_w^{\,c}=T_w^{\,a}T_a^{\,c}.
$$
Provides stable visual and debug viewpoints following the host axle.

---

### 3.12 Processing Order (Per Tick)

1. **Inputs:** Steering, speed, control updates.  
2. **Steering pass:** Compute per-wheel steer angles (§3.5).  
3. **Constraint solve:** Determine axle twist (§3.4).  
4. **Integrate:** Update axle poses (§3.6).  
5. **Apply joints:** Update drawbars and attached nodes (§3.3).  
6. **Fusion:** Correct with sensors (§3.8–3.9).  
7. **Sections:** Update section endpoints (§3.10).  
8. **Render:** Refresh operator/camera view (§3.11).

---

**Summary**

- Only **axles** and **drawbars** own frames.  
- Drawbars can connect any two entities—or none—to define physical or virtual structure.  
- Axles lie on the ground plane; wheels act as rolling and steering constraints only.  
- Sensor fusion keeps model and measurements synchronized.  
- The system supports flexible, efficient simulation and real-time operation on embedded hardware.



## 4. Implementation Roadmap (High-Level)

| Stage | Goal | Deliverable |
|--------|------|-------------|
| **1** | Define `Frame`, `Axle`, `Joint6DoF`, `Drawbar` data structures | Basic data model |
| **2** | Implement transform composition & joint constraint math | Deterministic 6-DoF solver |
| **3** | Add wheel & steering functions | Realistic axle behavior |
| **4** | Integrate with simulation loop | Stepwise chain propagation |
| **5** | Render in OpenGL/Vulkan | Visualization of axles/drawbars/sections |
| **6** | Fuse with GNSS/IMU sensors | Live validation & hybrid mode |

---

## 5. Collaboration Notes

- Review cycles will include **Simulation**, **Guidance**, and **Visualization** leads.  
- Changes affecting geometry, section logic, or rendering require joint approval.  
- Discussions take place in:
  - GitHub Discussions → *Kinematics & Simulation*
  - Telegram → *AgOpenNext Dynamics Channel*

---

## 6. Appendix

### Changelog

| Version | Date | Summary | Author | PR / Issue |
| --- | --- | --- | --- | --- |
| 0.2.0 | 2025-11-09 | Expanded conceptual intro; added implementation roadmap and cleaned math syntax for GitHub rendering. | @JonFortney | #0000 |

---

### Glossary

- **SE(3):** Special Euclidean group of 3D transformations (position + rotation).  
- **SO(3):** Special orthogonal group of 3D rotations.  
- **DoF:** Degree of Freedom.  
- **Ackermann Geometry:** Steering relation ensuring both front wheels share a turning center.  
- **Follow-the-Leader (FTL):** Simplified geometric trailer model.  
- **Joint6DoF:** Constraint allowing per-axis translation/rotation limits.  
- **Lever Arm:** Offset between sensor mount and reference frame.  
- **Look-Ahead Horizon:** Future path integration period for predictive control.

---
