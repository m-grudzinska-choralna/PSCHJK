# Copilot Instructions

## Project Guidelines
- Po zakończeniu migracji .NET 8 użytkownik wybrał jako następny etap naprawę podpisywania aplikacji i weryfikację jej uruchomienia.
- Fix for Avalonia .NET 8: Do NOT include XAML files as AvaloniaResource with wildcard patterns (e.g., `<AvaloniaResource Include="**/*.axaml" />`). This causes "Duplicate x:Class directive" errors in Avalonia 11.2.0+ with .NET 8. Solution: Only include static assets (`<AvaloniaResource Include="Assets/**" />`). XAML files are automatically loaded at runtime via AvaloniaXamlLoader.Load(this) in code-behind files.