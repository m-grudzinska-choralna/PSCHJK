# 03-net8-windows-migration: Upgrade the WinForms project to .NET 8

Change the converted project to `net8.0-windows` with Windows Forms enabled and resolve the assessment's Windows Forms and System.Drawing compatibility findings, including the 3,614 binary-incompatible and 434 source-incompatible API findings. Preserve application configuration and runtime behavior, using compatible .NET APIs or supported compatibility packages where required. No NuGet package upgrades are currently identified by the assessment.

**Done when**: The project targets `net8.0-windows`, restores successfully, all migration-related compilation errors and warnings are resolved without suppressions, and the application builds successfully.
