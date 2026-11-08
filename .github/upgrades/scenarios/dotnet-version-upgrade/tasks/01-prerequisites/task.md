# 01-prerequisites: Verify .NET 8 migration prerequisites

Verify the installed .NET 8 SDK/toolchain, inspect repository configuration such as `global.json`, and establish the current build baseline for `Moje KS/KS2/Program Moje KS.csproj`. Confirm the Windows Forms desktop requirements and record any baseline errors that must be distinguished from migration errors.

## Research findings

- Target project: `Moje KS/KS2/Program Moje KS.csproj`; one classic WinForms project, currently `TargetFrameworkVersion` `v4.8`.
- The project is non-SDK-style (`Project ToolsVersion="15.0"`) and contains explicit WinForms/System.Drawing references, designer-generated code, resources, and `Properties/AssemblyInfo.cs`.
- No `global.json` exists under the repository root, so SDK selection is not pinned.
- A compatible .NET 8 SDK is installed.
- Baseline build used Visual Studio MSBuild because the project is legacy WinForms and contains designer/resource files. Debug build succeeded with 7 existing compiler warnings: unused locals in `Dane.cs`, `Nuta.cs`, `Rysowanie.cs`, and `OknoGlowne.cs`; an unassigned field in `Rysowanie.cs`; and an unused field in `OknoGlowne.cs`.
- The assessment reports 3,614 binary-incompatible Windows Forms findings, 434 source-incompatible findings, 432 System.Drawing findings, and 2 legacy configuration findings. No NuGet packages or project dependencies were detected.

## Build tool decision

- `Moje KS/KS2/Program Moje KS.csproj`: `msbuild.exe` for the legacy project and WinForms resource/designer build. Re-evaluate after SDK-style conversion.

**Done when**: The required SDK is available, the project baseline and configuration are documented in the task record, and the project is ready for conversion.
