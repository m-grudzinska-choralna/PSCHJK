# AvaloniaApp Debugging Instructions (SkiaSharp / libSkiaSharp)

## What was wrong before
The Visual Studio launch profile used:

- `commandName: "Executable"`
- hardcoded path to `Release\net8.0\win-x64\publish\AvaloniaApp.exe`

This can run a stale/incomplete publish output. Then `SkiaSharp.dll` may load, but native `libSkiaSharp` (or its dependencies) may be missing, causing:

- `System.DllNotFoundException: libSkiaSharp`
- wrapped by `System.TypeInitializationException` (`SkiaSharp.SKImageInfo`)

## What was changed (fix)
`Properties/launchSettings.json` was updated to:

- `commandName: "Project"`

This ensures debugging uses current project build output and proper runtime asset resolution.

## Standard workflow (always use this)
1. Use Visual Studio profile with `commandName: "Project"`.
2. Build and run from the project (not a hardcoded publish EXE).
3. After package/runtime changes, run:

```powershell
dotnet clean
dotnet restore
dotnet build -c Debug -r win-x64
```

## Publish workflow (when needed)
Only for release/manual verification:

```powershell
dotnet publish -c Release -r win-x64 --self-contained false
```

Then run the published EXE. Do **not** use publish path as default debug profile.

## Quick diagnostics when error returns
1. Verify output has Skia files:

```powershell
Get-ChildItem .\bin\Debug\ -Recurse *Skia*
```

2. Confirm architecture is consistent (`win-x64`, x64 process).
3. Clean/restore/build again.
4. If still failing, check native dependencies (e.g., VC++ runtime) on target machine.

## Team rule
Do not commit machine-specific or stale `Executable` launch profiles as the default developer debug profile.
