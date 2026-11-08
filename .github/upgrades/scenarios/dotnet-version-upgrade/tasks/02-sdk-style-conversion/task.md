# 02-sdk-style-conversion: Convert the WinForms project to SDK-style

Convert the classic `Moje KS/KS2/Program Moje KS.csproj` project to SDK-style while keeping its current .NET Framework 4.8 target. Preserve source files, embedded resources, application settings, WinForms designer metadata, startup behavior, and configuration files. This is intentionally separate from the target framework/API migration because the assessment reports a non-SDK-style project.

**Done when**: The project is SDK-style, retains its current application behavior and required resources, restores successfully, and builds against its current target framework or has only documented migration-specific baseline issues.
