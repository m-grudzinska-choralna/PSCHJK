# .NET Version Upgrade — Report

**Scenario:** Upgrade `Moje KS/KS2/Program Moje KS.csproj` from .NET Framework 4.8 to .NET 8 for Windows.
**Outcome:** ✅ Fully completed
**Projects affected:** 1
**Tasks:** 4/4 completed

---

## Summary

The classic WinForms project was converted to SDK-style and upgraded from `net48` to `net8.0-windows`. Windows Forms support and Windows platform metadata were preserved, redundant framework references were removed, and existing compiler warnings were resolved without suppressions. The complete solution builds successfully with zero compiler/MSBuild warnings and errors.

No test projects were found, so no automated tests were available. Manual application launch and certificate-signing verification remain recommended follow-up checks.

---

## What Changed

### Packages

No NuGet packages were present or changed.

### Code Modifications

- **Project format** — converted `Moje KS/KS2/Program Moje KS.csproj` from classic MSBuild format to SDK-style while initially retaining `net48`.
- **Target framework** — changed the project target to `net8.0-windows` and retained Windows Forms support.
- **Platform compatibility** — added Windows platform metadata through `SupportedOSPlatformVersion` and an assembly-level Windows platform annotation.
- **Framework references** — removed redundant explicit references to `System.Data.DataSetExtensions`, `Microsoft.CSharp`, and `System.Net.Http`, eliminating .NET 8 assembly conflict warnings.
- **Warning cleanup** — removed seven unused declarations/fields in `Dane.cs`, `Nuta.cs`, `Rysowanie.cs`, and `OknoGlowne.cs`.
- **Configuration** — removed the obsolete .NET Framework `<startup>` declaration from `Moje KS/KS2/App.config` while preserving the configuration file.
- **Build/signing** — preserved the post-build `SignApp.ps1` target and validated that it does not prevent a successful build.

### Git Commits

| SHA | Message |
|-----|---------|
| `6548fc2` | `upgrade(01-prerequisites): verify .NET 8 migration prerequisites` |
| `8d3c45b` | `upgrade(02-sdk-style-conversion): convert WinForms project` |
| `809234b` | `upgrade(03-net8-windows-migration): retarget WinForms app to .NET 8` |
| `3b9f0a2` | `upgrade(04-final-validation): validate .NET 8 solution` |

---

## Task Breakdown

- **01-prerequisites** — ✅ Verified the .NET 8 SDK, inspected configuration, and established the .NET Framework baseline. [Details](tasks/01-prerequisites/progress-details.md)
- **02-sdk-style-conversion** — ✅ Converted the WinForms project to SDK-style and validated the original target framework build. [Details](tasks/02-sdk-style-conversion/progress-details.md)
- **03-net8-windows-migration** — ✅ Retargeted the project to `net8.0-windows`, resolved build warnings, and validated the migrated project. [Details](tasks/03-net8-windows-migration/progress-details.md)
- **04-final-validation** — ✅ Performed clean full-solution validation and reviewed tests and runtime configuration. [Details](tasks/04-final-validation/progress-details.md)

---

## Decisions Made

- **Target framework: `net8.0-windows`** — required for this Windows Forms application.
- **All-At-Once strategy** — selected because the scope contained one .NET Framework project.
- **Separate SDK conversion task** — required before changing the target framework.
- **Commit after each task** — preserved the requested incremental source-control history.

---

## Build & Test Results

| Project | Build | Tests | Warnings |
|---------|-------|-------|----------|
| `Moje KS/KS2/Program Moje KS.csproj` | ✅ Full solution restore/build succeeded | No test project found | 0 compiler/MSBuild warnings |

The build output still displays the signing script's `SignerCertificate ... UnknownError` status, but the signing target completes and does not fail the build.

---

## Known Gaps & Follow-up Items

- **Manual runtime validation** — launch the WinForms application and verify the main forms, resources, graphics rendering, and user workflows on a Windows machine.
- **Certificate signing** — investigate the `EC01FB6574FD46A991B2D29BB64FA4ECA5365716` certificate status reported as `UnknownError` by `SignApp.ps1` if signed deployment is required.
- **Automated tests** — add test coverage if regression protection is needed; no test projects currently exist in the solution.
