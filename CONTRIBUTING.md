# Contributing to UtilityPDF

## Coding Standards

### Language Requirements
- **All code comments must be written in English (US)**

### Code Style
- **Use explicit types**: Do not use `var`, always declare the explicit type
- **Always use braces**: Use curly braces `{}` even for single-line `if`, `else`, `for`, `foreach`, `while` statements
- **Naming conventions**: Follow standard C# PascalCase for public members, camelCase for private fields

### Example

```csharp
// Correct
string filePath = GetFilePath();
if (filePath != null)
{
    ProcessFile(filePath);
}

// Incorrect
var filePath = GetFilePath();
if (filePath != null)
    ProcessFile(filePath);
```

## Pull Request Guidelines
1. Ensure all code follows the coding standards above
2. Test your changes thoroughly before submitting
3. Update documentation if necessary