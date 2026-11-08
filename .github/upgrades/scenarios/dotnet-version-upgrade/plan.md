# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade `Moje KS/KS2/Program Moje KS.csproj` from .NET Framework 4.8 (`net48`) to .NET 8 for Windows (`net8.0-windows`).
**Scope**: One classic WinForms project with 47 project files, approximately 10,503 lines of code, no NuGet packages, and significant Windows Forms/System.Drawing API compatibility findings.

### Selected Strategy
**All-At-Once** — Upgrade the single project in one atomic operation.
**Rationale**: The scope contains one .NET Framework project, and the framework-migration rules require All-At-Once for a single-project migration. The old project format must be converted separately before changing the target framework.

## Tasks

### 01-prerequisites: Verify .NET 8 migration prerequisites

Verify the installed .NET 8 SDK/toolchain, inspect repository configuration such as `global.json`, and establish the current build baseline for `Moje KS/KS2/Program Moje KS.csproj`. Confirm the Windows Forms desktop requirements and record any baseline errors that must be distinguished from migration errors.

**Done when**: The required SDK is available, the project baseline and configuration are documented in the task record, and the project is ready for conversion.

---

### 02-sdk-style-conversion: Convert the WinForms project to SDK-style

Convert the classic `Moje KS/KS2/Program Moje KS.csproj` project to SDK-style while keeping its current .NET Framework 4.8 target. Preserve source files, embedded resources, application settings, WinForms designer metadata, startup behavior, and configuration files. This is intentionally separate from the target framework/API migration because the assessment reports a non-SDK-style project.

**Done when**: The project is SDK-style, retains its current application behavior and required resources, restores successfully, and builds against its current target framework or has only documented migration-specific baseline issues.

---

### 03-net8-windows-migration: Upgrade the WinForms project to .NET 8

Change the converted project to `net8.0-windows` with Windows Forms enabled and resolve the assessment's Windows Forms and System.Drawing compatibility findings, including the 3,614 binary-incompatible and 434 source-incompatible API findings. Preserve application configuration and runtime behavior, using compatible .NET APIs or supported compatibility packages where required. No NuGet package upgrades are currently identified by the assessment.

**Done when**: The project targets `net8.0-windows`, restores successfully, all migration-related compilation errors and warnings are resolved without suppressions, and the application builds successfully.

---

### 04-final-validation: Validate the upgraded solution

Build the complete `Moje KS/KS7.sln`, run all discovered tests or applicable validation, and review the resulting project/runtime configuration for remaining framework, package, configuration, and dependency issues. Document any runtime checks or deferred recommendations that cannot be automated for the WinForms application.

**Done when**: The solution builds with zero errors and warnings, all available tests pass, and remaining non-automated validation items are documented.
