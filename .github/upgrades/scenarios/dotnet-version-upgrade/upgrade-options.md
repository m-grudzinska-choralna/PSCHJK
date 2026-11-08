# Upgrade Options

## Upgrade Strategy

| Option | Description |
|---|---|
| **All-At-Once** (selected) | Upgrade the single project in one atomic operation, with conversion and framework/code migration tasks followed by full validation. |
| Bottom-Up | Upgrade dependency tiers independently; not applicable because the scope contains one project. |
| Top-Down | Upgrade applications before dependencies; not applicable because the scope contains one project. |

**Why**: The scope contains one .NET Framework WinForms project, so the framework-migration rules select All-At-Once. The assessment also requires separate SDK-style conversion and TFM/API migration work.
