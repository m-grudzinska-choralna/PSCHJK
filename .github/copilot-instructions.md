# Copilot Instructions

## Project Guidelines
- Po zakończeniu migracji .NET 8 użytkownik wybrał jako następny etap naprawę podpisywania aplikacji i weryfikację jej uruchomienia.
- Fix for Avalonia .NET 8: Do NOT include XAML files as AvaloniaResource with wildcard patterns (e.g., `<AvaloniaResource Include="**/*.axaml" />`). This causes "Duplicate x:Class directive" errors in Avalonia 11.2.0+ with .NET 8. Solution: Only include static assets (`<AvaloniaResource Include="Assets/**" />`). XAML files are automatically loaded at runtime via AvaloniaXamlLoader.Load(this) in code-behind files.
- Document the Avalonia startup issue caused by local Debug code-signing being blocked by Windows Application Control (0x800711C7). Fix: disable `EnableLocalSigning` by default in `AvaloniaApp.csproj`, keeping optional signing via `/p:EnableLocalSigning=true`.
- Always build the application after making changes to verify there are no errors.

## Visual Design Preferences
- For the music-themed header, use cheerful, lively-looking wavy staff lines and musical notes, avoiding a subdued or overly formal treatment.

## General Instructions
- When the user has already provided screenshots as the specification, inspect those and the relevant project source instead of asking them to restate the ranges or inferring values from incomplete data.

## Avalonia App Guidelines
- When matching KS2 visually in the Avalonia app, omit headings/placeholders not present in KS2.
- Use symbolic note labels instead of text prefixes like “Nuta:”. 
- Correctly align the clef to the staff.
- Prevent noteheads from obscuring staff lines.
- Render ledger lines for notes such as middle C.

## Rendering Guidelines
- When rendering low ledger-line notes in Avalonia, keep the full notehead visible within the staff canvas; middle C (C4) must be visible and ledger lines must be shown rather than clipping the note below the canvas.

## Notation Guidelines
- Keep the music staff scale and position stable between questions; do not resize/recenter it for each note.
- Place the symbolic note label lower and farther right to give the notation more display space.

## Note Display Guidelines
- Keep staff scale fixed across all note changes by fitting to the full supported note span for the selected clef, ensuring that displayed notes outside the selected range (such as h³) remain visible.

## UI Design Guidelines
- When adapting UI to a reference image, ensure graphics are vivid and visually prominent, rather than understated or represented by simple text glyphs.
- The exercise-options card should have a decorative light-blue wave along its bottom-left corner, with small music notes near the lower-right end, matching the provided reference.