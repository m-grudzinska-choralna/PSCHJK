param(
    [string]$OutputPath = ""
)

$ErrorActionPreference = "Stop"

$repositoryFolder = Split-Path -Parent $PSScriptRoot
$linksFolder = Join-Path $repositoryFolder "LInki"
$linkFiles = @(
    Get-ChildItem -LiteralPath $linksFolder -Filter "Klasa_*.txt" -File |
    Sort-Object Name
)

if ($linkFiles.Count -eq 0) {
    throw "Nie znaleziono plikow z linkami do klas w folderze: $linksFolder"
}

$availableClasses = @($linkFiles | ForEach-Object { $_.BaseName -replace '(?i)^klasa_', 'Klasa_' })
Write-Host "Dostepne klasy: $($availableClasses -join ', ')" -ForegroundColor Cyan
$selectedClass = (Read-Host "Podaj klase do przetworzenia").Trim()
$selectedFile = @(
    $linkFiles | Where-Object {
        ($_.BaseName -replace '(?i)^klasa_', 'Klasa_') -eq $selectedClass
    }
)
if ($selectedFile.Count -ne 1) {
    throw "Nieprawidlowa klasa '$selectedClass'. Dostepne klasy: $($availableClasses -join ', ')"
}

$linkFile = $selectedFile[0]
$letters = @(
    $lineNumber = 0
    foreach ($line in Get-Content -LiteralPath $linkFile.FullName -Encoding UTF8) {
        $lineNumber++
        if ([string]::IsNullOrWhiteSpace($line)) {
            continue
        }

        if ($line -notmatch '^\s*(\d+)\s*\|\s*(.+?)\s*\|\s*(\S+)\s*$') {
            throw "Nieprawidlowy wpis w pliku '$($linkFile.Name)', wiersz $lineNumber."
        }

        $studentName = $matches[2].Trim()
        $link = $matches[3].Trim()
        $parsedUri = $null
        if ($link -like "BLAD:*" -or
            -not [System.Uri]::TryCreate($link, [System.UriKind]::Absolute, [ref]$parsedUri) -or
            $parsedUri.Scheme -notin @("http", "https")) {
            throw "Brak poprawnego linku dla ucznia '$studentName' w pliku '$($linkFile.Name)', wiersz $lineNumber."
        }

        [PSCustomObject]@{
            ImieINazwisko = $studentName
            Link          = $link
        }
    }
)

if ($letters.Count -eq 0) {
    throw "Pliki z linkami nie zawieraja zadnych uczniow."
}

if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path $linksFolder "Wiadomosci_dla_uczniow_$selectedClass.docx"
}
else {
    $OutputPath = [System.IO.Path]::GetFullPath($OutputPath)
}

$word = $null
$document = $null
try {
    $word = New-Object -ComObject Word.Application
    $word.Visible = $false
    $word.DisplayAlerts = 0
    $document = $word.Documents.Add()
    $selection = $word.Selection
    $selection.Font.Name = "Arial"
    $selection.Font.Size = 11

    for ($index = 0; $index -lt $letters.Count; $index++) {
        if ($index -gt 0) {
            $selection.InsertBreak(7)
        }

        $selection.Font.Bold = $true
        $selection.TypeText($letters[$index].ImieINazwisko)
        $selection.TypeParagraph()
        $selection.TypeParagraph()

        $selection.Font.Bold = $false
        $selection.TypeText("Szanowni Państwo,")
        $selection.TypeParagraph()
        $selection.TypeText("przesyłam indywidualny link do folderu w chmurze:")
        $selection.TypeParagraph()
        $selection.Hyperlinks.Add($selection.Range, $letters[$index].Link, "", "", $letters[$index].Link) | Out-Null
        $selection.Collapse(0)
        $selection.TypeParagraph()
        $selection.TypeParagraph()
        $selection.TypeText("W folderze będą pojawiać się nagrania MP3 (dyktanda rytmiczne) kolejnych serii zadań. Każdemu uczniowi przydzielane będą każdorazowo inne zestawy.")
        $selection.TypeParagraph()
        $selection.TypeParagraph()
        $selection.TypeText("Pozdrawiam serdecznie")
        $selection.TypeParagraph()
        $selection.TypeText("Marta Grudzińska")
        $selection.TypeParagraph()
    }

    $document.SaveAs($OutputPath, 16)
    Write-Host "Zapisano dokument dla $($letters.Count) uczniow: $OutputPath" -ForegroundColor Green
}
finally {
    if ($null -ne $document) {
        $document.Close(0)
        [void][System.Runtime.InteropServices.Marshal]::ReleaseComObject($document)
    }
    if ($null -ne $word) {
        $word.Quit()
        [void][System.Runtime.InteropServices.Marshal]::ReleaseComObject($word)
    }
}
