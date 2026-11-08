# 03-net8-windows-migration: Upgrade the WinForms project to .NET 8

Change the converted project to `net8.0-windows` with Windows Forms enabled and resolve the assessment's Windows Forms and System.Drawing compatibility findings, including the 3,614 binary-incompatible and 434 source-incompatible API findings. Preserve application configuration and runtime behavior, using compatible .NET APIs or supported compatibility packages where required. No NuGet package upgrades are currently identified by the assessment.

## Research findings

- The project is now SDK-style and has a single `TargetFramework` property currently set to `net48`.
- `UseWindowsForms` is already enabled. The project also contains `ImportWindowsDesktopTargets`, which remains compatible with the current build.
- No `Directory.Build.props` or `Directory.Packages.props` files were found; dependency inspection reports only framework/assembly references and no NuGet packages.
- The project contains three `.resx` resources and many `System.Windows.Forms`/`System.Drawing` usages, matching the assessment's WinForms and GDI+ findings. Visual Studio MSBuild is required for reliable WinForms resource compilation.
- `App.config` is present and remains preserved; the .NET 8 build did not report configuration-related errors.
- The SDK conversion introduced a `SignBinaryAfterBuild` target invoking `Moje KS/KS2/SignApp.ps1`. The target runs after a successful build; its certificate status output is informational and does not fail the build.

## Changes completed

- Changed `TargetFramework` from `net48` to `net8.0-windows`.
- Kept Windows Forms enabled and declared `SupportedOSPlatformVersion` `6.1` plus an assembly-level Windows platform annotation.
- Removed redundant explicit references to framework assemblies that caused .NET 8 MSBuild conflict warnings.
- Removed seven pre-existing unused declarations/fields so the migrated project builds warning-free without suppressions.

**Done when**: The project targets `net8.0-windows`, restores successfully, all migration-related compilation errors and warnings are resolved without suppressions, and the application builds successfully.
