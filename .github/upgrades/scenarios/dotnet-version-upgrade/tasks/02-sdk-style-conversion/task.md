# 02-sdk-style-conversion: Convert the WinForms project to SDK-style

Convert the classic `Moje KS/KS2/Program Moje KS.csproj` project to SDK-style while keeping its current .NET Framework 4.8 target. Preserve source files, embedded resources, application settings, WinForms designer metadata, startup behavior, and configuration files. This is intentionally separate from the target framework/API migration because the assessment reports a non-SDK-style project.

## Research findings

- Topological project ordering reports one project only: `Moje KS/KS2/Program Moje KS.csproj`.
- The project uses legacy MSBuild XML (`ToolsVersion="15.0"`), explicit compile/reference items, `Properties/AssemblyInfo.cs`, and `TargetFrameworkVersion` `v4.8`.
- WinForms resources requiring preservation: `OknoGlowne.resx`, `ZakresNutDialog.resx`, and `Properties/Resources.resx`.
- No `packages.config` was found and the assessment reports zero NuGet packages.
- The conversion must use the dedicated SDK-style conversion tool, must not change the target framework, and must be validated with Visual Studio MSBuild because this is a WinForms project with designer-generated resources.
- No `// STUB:` markers were found in the KS2 C# source tree.

**Done when**: The project is SDK-style, retains its current application behavior and required resources, restores successfully, and builds against its current target framework or has only documented migration-specific baseline issues.
