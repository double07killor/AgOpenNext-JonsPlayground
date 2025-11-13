# Contributing to AgNext

Thank you for your interest in contributing to AgNext! This document provides guidelines for contributing to the project.

## Getting Started

### Prerequisites
- .NET SDK (version TBD based on project requirements)
- Git
- A GitHub account

### Setting Up Your Development Environment

1. Fork the repository on GitHub
2. Clone your fork locally:
   ```bash
   git clone https://github.com/YOUR-USERNAME/AgNext.git
   cd AgNext
   ```

3. Add the upstream repository as a remote:
   ```bash
   git remote add upstream https://github.com/AgOpenGPS-Official/AgNext.git
   ```

4. Keep your fork synchronized:
   ```bash
   git fetch upstream
   git checkout develop
   git merge upstream/develop
   ```

## Contribution Workflow

### Creating a Branch

Always create a new branch for your work:

```bash
git checkout develop
git pull upstream develop
git checkout -b feature/your-feature-name
# or
git checkout -b fix/your-bug-fix
```

Branch naming conventions:
- `feature/` - for new features
- `fix/` - for bug fixes
- `docs/` - for documentation changes
- `refactor/` - for code refactoring

### Making Changes

1. Make your changes in your feature branch
2. Write clear, concise commit messages
3. Keep commits focused on a single logical change
4. Test your changes thoroughly

### Submitting a Pull Request

1. Push your changes to your fork:
   ```bash
   git push origin feature/your-feature-name
   ```

2. Create a Pull Request from your fork to the upstream repository
3. Target the `develop` branch (not `main` or `master`)
4. Provide a clear description of:
   - What the PR does
   - Why the change is needed
   - Any relevant issue numbers (e.g., "Fixes #123")

### Code Review Process

- Maintainers will review your PR
- Address any feedback or requested changes
- Once approved, a maintainer will merge your PR

## Coding Standards

### General Guidelines

- Write clean, readable, and maintainable code
- Follow existing code style and conventions
- Add comments where necessary to explain complex logic
- Avoid unnecessary complexity

### C# Specific

- Follow standard C# naming conventions
- Use meaningful variable and method names
- Keep methods focused and single-purpose
- Handle exceptions appropriately

## Testing

- Write tests for new features
- Ensure existing tests pass before submitting PR
- Test your changes on different platforms if applicable

## Documentation

- Update documentation for any changed functionality
- Add XML documentation comments to public APIs
- Update README.md if adding new features or changing setup

## Reporting Issues

When reporting bugs or suggesting features:

1. Check if the issue already exists
2. Use the issue templates if available
3. Provide detailed information:
   - Steps to reproduce (for bugs)
   - Expected vs actual behavior
   - System information (OS, .NET version, etc.)
   - Screenshots if applicable

## Code of Conduct

- Be respectful and inclusive
- Welcome newcomers and help them get started
- Focus on constructive feedback
- Respect differing viewpoints and experiences

## Questions?

If you have questions about contributing, feel free to:
- Open an issue for discussion
- Reach out to the maintainers
- Check existing documentation and issues

## License

By contributing to AgNext, you agree that your contributions will be licensed under the same license as the project.
