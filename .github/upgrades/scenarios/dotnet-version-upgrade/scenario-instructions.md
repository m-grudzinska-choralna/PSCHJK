# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net8.0
- **Scope**: `Moje KS/KS2/Program Moje KS.csproj`

## Source Control
- **Source Branch**: Avalonia-UI
- **Working Branch**: upgrade-dotnet-8
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## Upgrade Options
- **Upgrade Strategy**: All-At-Once (confirmed)

## Strategy
**Selected**: All-At-Once
**Rationale**: Single .NET Framework WinForms project; framework-migration rules require All-At-Once for a one-project scope.

### Execution Constraints
- Convert the classic project to SDK-style as a separate task before changing its target framework.
- Upgrade the project to `net8.0-windows` with Windows Forms enabled.
- Resolve migration compilation errors and warnings without suppressions.
- Validate the complete solution and available tests after the migration.
