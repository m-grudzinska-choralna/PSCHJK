# ============================================================
# GenerowanieLinkowGoogleDriveDlaUczniow.ps1
#
# Przyklad:
# .\GenerowanieLinkowGoogleDriveDlaUczniow.ps1 -FolderPath "PSCHJK-dla uczniow\Klasa_5"
# ============================================================

param(
    [string]$FolderPath = "PSCHJK-dla uczniow\Klasa_5",
    [string]$StudentListPath = "",
    [string]$AccessToken = "" 
)

if (-not [string]::IsNullOrWhiteSpace($PSScriptRoot)) {
    $scriptFolder = $PSScriptRoot
}
elseif ($psISE -and $psISE.CurrentFile -and -not [string]::IsNullOrWhiteSpace($psISE.CurrentFile.FullPath)) {
    $scriptFolder = Split-Path -Parent $psISE.CurrentFile.FullPath
}
else {
    $scriptFolder = (Get-Location).Path
}

$repositoryFolder = Split-Path -Parent $scriptFolder
$studentListFolder = Join-Path $repositoryFolder "Listy uczniow"
$availableClasses = @(
    Get-ChildItem -LiteralPath $studentListFolder -Filter "*.txt" -File -ErrorAction SilentlyContinue |
    ForEach-Object { [System.IO.Path]::GetFileNameWithoutExtension($_.Name) } |
    Sort-Object
)

if ($availableClasses.Count -eq 0) {
    Write-Host "Nie znaleziono plikow klasowych w folderze: $studentListFolder" -ForegroundColor Red
    return
}

Write-Host "Dostepne klasy: $($availableClasses -join ', ')" -ForegroundColor Cyan
$selectedClass = (Read-Host "Podaj klase do przetworzenia").Trim()
if ($availableClasses -notcontains $selectedClass) {
    Write-Host "Nieprawidlowa klasa '$selectedClass'. Dostepne klasy: $($availableClasses -join ', ')" -ForegroundColor Red
    return
}

$folderPathParts = @($FolderPath -split '[\\/]' | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
if ($folderPathParts.Count -gt 1) {
    $folderParentPath = $folderPathParts[0..($folderPathParts.Count - 2)] -join "\"
    $FolderPath = Join-Path $folderParentPath $selectedClass
}
else {
    $FolderPath = $selectedClass
}

# ============================================================
# 1. Sprawdzanie Tokenu Dostepnosci
# ============================================================

if ([string]::IsNullOrWhiteSpace($AccessToken)) {
    Write-Host ""
    Write-Host "Brak podanego Access Tokenu dla Google Drive API." -ForegroundColor Yellow
    Write-Host "Otworz Google OAuth 2.0 Playground: https://developers.google.com/oauthplayground/" -ForegroundColor Cyan
    Write-Host "W kroku 1 wybierz zakres: https://www.googleapis.com/auth/drive" -ForegroundColor White
    Write-Host "Zaloguj sie na konto Google, zatwierdz dostep, a w kroku 2 kliknij 'Exchange authorization code for tokens'." -ForegroundColor White
    Write-Host "Skopiuj wartosc Access token (token wygasa po okolo godzinie)." -ForegroundColor White
    $AccessToken = Read-Host "Wklej swoj Google Drive Access Token (OAuth 2.0)"
    
    if ([string]::IsNullOrWhiteSpace($AccessToken)) {
        Write-Host "Bled: Brak Access Tokena. Anulowano." -ForegroundColor Red
        return
    }
}

$headers = @{
    "Authorization" = "Bearer $AccessToken"
    "Content-Type"  = "application/json"
}

# ============================================================
# 2. Sciezka do folderu klasy
# ============================================================

Write-Host ""
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host "FOLDER KLASY" -ForegroundColor Cyan
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Sciezka folderu: $FolderPath" -ForegroundColor White
Write-Host ""

$pathParts = @($FolderPath -split '[\\/]' | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
if ($pathParts.Count -eq 0) {
    Write-Host "Sciezka folderu jest pusta." -ForegroundColor Red
    return
}

$className = $pathParts[-1]
$linksFolder = Join-Path $repositoryFolder "LInki"
$outputPath = Join-Path $linksFolder "$className.txt"
if ([string]::IsNullOrWhiteSpace($StudentListPath)) {
    $StudentListPath = Join-Path $studentListFolder "$className.txt"
}

try {
    if (-not (Test-Path -LiteralPath $StudentListPath -PathType Leaf)) {
        throw "Nie znaleziono listy uczniow: $StudentListPath"
    }

    $studentLines = @(
        Get-Content -LiteralPath $StudentListPath -Encoding UTF8 |
        Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    )
    if ($studentLines.Count -eq 0) {
        throw "Lista uczniow jest pusta: $StudentListPath"
    }

    $studentEntries = for ($index = 0; $index -lt $studentLines.Count; $index++) {
        $line = $studentLines[$index].Trim()
        $number = $index + 1
        $studentName = $line
        if ($line -match '^(\d+)\s*\.\s*(.+)$') {
            $number = [int]$matches[1]
            $studentName = $matches[2].Trim()
        }

        if ([string]::IsNullOrWhiteSpace($studentName) -or $number -lt 1) {
            throw "Nieprawidlowy wpis na liscie uczniow (wiersz $($index + 1))."
        }

        [PSCustomObject]@{
            Numer         = $number
            ImieINazwisko = $studentName
        }
    }

    $duplicateNumbers = @($studentEntries | Group-Object Numer | Where-Object { $_.Count -gt 1 })
    if ($duplicateNumbers.Count -gt 0) {
        throw "Lista uczniow zawiera powtorzone numery: $(($duplicateNumbers.Name) -join ', ')."
    }
}
catch {
    Write-Host $_.Exception.Message -ForegroundColor Red
    return
}

# ============================================================
# 3. Odszukanie folderu po sciezce
# ============================================================

try {
    $parentId = "root"
    foreach ($pathPart in $pathParts) {
        $escapedName = $pathPart.Replace('\', '\\').Replace("'", "\'")
        $q = "'$parentId' in parents and name = '$escapedName' and mimeType = 'application/vnd.google-apps.folder' and trashed = false"
        $encodedQ = [System.Uri]::EscapeDataString($q)
        $uri = "https://www.googleapis.com/drive/v3/files?q=$encodedQ&fields=files(id,name)&pageSize=100"
        $folderResponse = Invoke-RestMethod -Method Get -Uri $uri -Headers $headers

        if ($null -eq $folderResponse.files -or $folderResponse.files.Count -eq 0) {
            $folderBody = @{
                name     = $pathPart
                mimeType = "application/vnd.google-apps.folder"
                parents  = @($parentId)
            } | ConvertTo-Json
            $createUri = "https://www.googleapis.com/drive/v3/files?fields=id,name"
            $parentFolder = Invoke-RestMethod -Method Post -Uri $createUri -Headers $headers -Body $folderBody

            if ([string]::IsNullOrWhiteSpace($parentFolder.id)) {
                throw "Google Drive API nie zwrocilo ID utworzonego folderu '$pathPart'."
            }

            Write-Host "Utworzono brakujacy folder: $pathPart" -ForegroundColor Green
            $parentId = $parentFolder.id
            continue
        }
        if ($folderResponse.files.Count -gt 1) {
            throw "Sciezka jest niejednoznaczna: znaleziono kilka folderow o nazwie '$pathPart'."
        }

        $parentFolder = $folderResponse.files[0]
        $parentId = $parentFolder.id
    }
}
catch {
    Write-Host ""
    Write-Host "==============================================" -ForegroundColor Red
    Write-Host "BLED" -ForegroundColor Red
    Write-Host "==============================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Nie udalo sie odnalezc ani utworzyc folderu ze sciezki: $FolderPath" -ForegroundColor Yellow
    Write-Host "Szczegoly bledu:" -ForegroundColor Yellow
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ""
    return
}

Write-Host "Znaleziono folder:" -ForegroundColor Green
Write-Host "$($parentFolder.name) (ID: $($parentFolder.id))" -ForegroundColor White
Write-Host ""

# ============================================================
# 4. Utworzenie folderow uczniow i generowanie linkow
# ============================================================

$results = foreach ($studentEntry in $studentEntries) {
    $number = $studentEntry.Numer
    $studentName = $studentEntry.ImieINazwisko
    Write-Host "Folder $number - $studentName" -ForegroundColor White

    try {
        $escapedName = "$number".Replace('\', '\\').Replace("'", "\'")
        $q = "'$($parentFolder.id)' in parents and name = '$escapedName' and mimeType = 'application/vnd.google-apps.folder' and trashed = false"
        $encodedQ = [System.Uri]::EscapeDataString($q)
        $folderUri = "https://www.googleapis.com/drive/v3/files?q=$encodedQ&fields=files(id,name,webViewLink)&pageSize=100"
        $folderResponse = Invoke-RestMethod -Method Get -Uri $folderUri -Headers $headers

        if ($folderResponse.files.Count -gt 1) {
            throw "Znaleziono kilka folderow o numerze '$number' w folderze klasy."
        }

        if ($null -eq $folderResponse.files -or $folderResponse.files.Count -eq 0) {
            $folderBody = @{
                name     = "$number"
                mimeType = "application/vnd.google-apps.folder"
                parents  = @($parentFolder.id)
            } | ConvertTo-Json
            $folder = Invoke-RestMethod -Method Post -Uri "https://www.googleapis.com/drive/v3/files?fields=id,name,webViewLink" -Headers $headers -Body $folderBody

            if ([string]::IsNullOrWhiteSpace($folder.id)) {
                throw "Google Drive API nie zwrocilo ID folderu '$number'."
            }

            Write-Host "Utworzono folder: $number" -ForegroundColor Green
        }
        else {
            $folder = $folderResponse.files[0]
            Write-Host "Folder $number juz istnieje - zostanie uzyty ponownie." -ForegroundColor DarkGray
        }

        $permissionsUri = "https://www.googleapis.com/drive/v3/files/$($folder.id)/permissions?fields=permissions(id,type,role)&pageSize=100"
        $permissionsResponse = Invoke-RestMethod -Method Get -Uri $permissionsUri -Headers $headers
        $anyonePermission = @($permissionsResponse.permissions | Where-Object { $_.type -eq "anyone" })
        if ($anyonePermission.Count -eq 0) {
            $permBody = @{
                role = "reader"
                type = "anyone"
            } | ConvertTo-Json
            $permUri = "https://www.googleapis.com/drive/v3/files/$($folder.id)/permissions"
            $null = Invoke-RestMethod -Method Post -Uri $permUri -Headers $headers -Body $permBody
        }

        $shareLink = $folder.webViewLink
        if ([string]::IsNullOrWhiteSpace($shareLink)) {
            $fileUri = "https://www.googleapis.com/drive/v3/files/$($folder.id)?fields=id,name,webViewLink"
            $folderDetails = Invoke-RestMethod -Method Get -Uri $fileUri -Headers $headers
            $shareLink = $folderDetails.webViewLink
        }

        if ([string]::IsNullOrWhiteSpace($shareLink)) {
            throw "Google Drive API nie zwrocilo linku do folderu '$number'."
        }

        Write-Host "OK: $shareLink" -ForegroundColor Green
        [PSCustomObject]@{
            "Numer"           = $number
            "Imie i nazwisko" = $studentName
            "Link do folderu" = $shareLink
        }
    }
    catch {
        Write-Host "BLAD dla ucznia '$studentName' (folder $number):" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
        [PSCustomObject]@{
            "Numer"           = $number
            "Imie i nazwisko" = $studentName
            "Link do folderu" = "BLAD: $($_.Exception.Message)"
        }
    }
}

# ============================================================
# 5. Zapisanie zestawienia linkow w folderze lokalnym LInki
# ============================================================

try {
    if (-not (Test-Path -LiteralPath $linksFolder -PathType Container)) {
        $null = New-Item -ItemType Directory -Path $linksFolder -Force
    }
    $resultLines = @(
        $results | ForEach-Object {
            "$($_.Numer) | $($_.'Imie i nazwisko') | $($_.'Link do folderu')"
        }
    )
    Set-Content -LiteralPath $outputPath -Value $resultLines -Encoding UTF8
    Write-Host ""
    Write-Host "Zapisano zestawienie linkow: $outputPath" -ForegroundColor Green
}
catch {
    Write-Host "Nie udalo sie zapisac zestawienia linkow: $outputPath" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    return
}

$results | Format-Table -AutoSize

$successCount = @(
    $results | Where-Object {
        $_."Link do folderu" -notlike "BLAD:*"
    }
).Count

$errorCount = $results.Count - $successCount

Write-Host ""
Write-Host "Podsumowanie:" -ForegroundColor Cyan
Write-Host "  Wszystkich folderow : $($results.Count)" -ForegroundColor White
Write-Host "  Wygenerowanych linkow: $successCount" -ForegroundColor Green
Write-Host "  Bledow              : $errorCount" -ForegroundColor $(if ($errorCount -gt 0) { "Red" } else { "Green" })
Write-Host ""