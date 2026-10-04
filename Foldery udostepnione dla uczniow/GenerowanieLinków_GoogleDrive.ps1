# ============================================================
# GenerowanieLinkowGoogleDriveDlaUczniow.ps1
#
# Przyklad:
# .\GenerowanieLinkowGoogleDriveDlaUczniow.ps1 -Klasa Klasa_5
# ============================================================

param(
    [string]$Klasa = "Klasa_5",
    [string]$AccessToken = "" 
)

# ============================================================
# 1. Sprawdzanie Tokenu Dostepnosci
# ============================================================

if ([string]::IsNullOrWhiteSpace($AccessToken)) {
    Write-Host ""
    Write-Host "Brak podanego Access Tokenu dla Google Drive API." -ForegroundColor Yellow
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
# 2. Nazwa i sciezka do folderu klasy
# ============================================================

$targetFolderName = $Klasa

Write-Host ""
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host "FOLDER KLASY" -ForegroundColor Cyan
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Szukanie folderu: $targetFolderName" -ForegroundColor White
Write-Host ""

# ============================================================
# 3. Pobranie informacji o folderze klasy
# ============================================================

try {
    $q = "name = '$targetFolderName' and mimeType = 'application/vnd.google-apps.folder' and trashed = false"
    $encodedQ = [System.Uri]::EscapeDataString($q)
    
    $uri = "https://www.googleapis.com/drive/v3/files?q=$encodedQ&fields=files(id,name)"
    
    $parentResponse = Invoke-RestMethod -Method Get -Uri $uri -Headers $headers

    if ($null -eq $parentResponse.files -or $parentResponse.files.Count -eq 0) {
        throw "Nie znaleziono folderu o nazwie: $targetFolderName"
    }

    $parentFolder = $parentResponse.files[0]
}
catch {
    Write-Host ""
    Write-Host "==============================================" -ForegroundColor Red
    Write-Host "BLED" -ForegroundColor Red
    Write-Host "==============================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Nie znaleziono folderu: $targetFolderName" -ForegroundColor Yellow
    Write-Host "Szczegoly bledu:" -ForegroundColor Yellow
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ""
    return
}

Write-Host "Znaleziono folder:" -ForegroundColor Green
Write-Host "$($parentFolder.name) (ID: $($parentFolder.id))" -ForegroundColor White
Write-Host ""

# ============================================================
# 4. Pobranie podfolderow znajdujacych sie w folderze klasy
# ============================================================

try {
    $subq = "'$($parentFolder.id)' in parents and mimeType = 'application/vnd.google-apps.folder' and trashed = false"
    $encodedSubQ = [System.Uri]::EscapeDataString($subq)
    
    $subUri = "https://www.googleapis.com/drive/v3/files?q=$encodedSubQ&fields=files(id,name,webViewLink)"
    
    $subfoldersResponse = Invoke-RestMethod -Method Get -Uri $subUri -Headers $headers
    $subfolders = $subfoldersResponse.files
}
catch {
    Write-Host ""
    Write-Host "Nie udalo sie pobrac zawartosci folderu." -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ""
    return
}

# ============================================================
# 5. Sprawdzenie, czy znaleziono podfoldery
# ============================================================

if ($null -eq $subfolders -or $subfolders.Count -eq 0) {
    Write-Host ""
    Write-Host "W folderze: $targetFolderName nie znaleziono zadnych podfolderow." -ForegroundColor Yellow
    Write-Host ""
    return
}

Write-Host "Znaleziono $($subfolders.Count) podfolderow." -ForegroundColor Cyan
Write-Host ""
Write-Host "Rozpoczynam generowanie linkow..." -ForegroundColor Cyan
Write-Host ""

# ============================================================
# 6. Udostepnianie podfolderow i pobranie linkow
# ============================================================

$results = foreach ($folder in $subfolders) {

    Write-Host "----------------------------------------------" -ForegroundColor DarkGray
    Write-Host "Folder: $($folder.name)" -ForegroundColor White

    try {
        $permBody = @{
            role = "reader"
            type = "anyone"
        } | ConvertTo-Json

        $permUri = "https://www.googleapis.com/drive/v3/files/$($folder.id)/permissions"

        $null = Invoke-RestMethod -Method Post -Uri $permUri -Headers $headers -Body $permBody

        $shareLink = $folder.webViewLink

        if ([string]::IsNullOrWhiteSpace($shareLink)) {
            Write-Host "BLED: Google Drive API nie zwrocil linku." -ForegroundColor Red

            [PSCustomObject]@{
                "Nazwa Folderu"     = $folder.name
                "Link Google Drive" = "BLED - brak linku"
            }
        }
        else {
            Write-Host "OK" -ForegroundColor Green
            Write-Host $shareLink -ForegroundColor DarkCyan

            [PSCustomObject]@{
                "Nazwa Folderu"     = $folder.name
                "Link Google Drive" = $shareLink
            }
        }
    }
    catch {
        Write-Host "BLED podczas tworzenia linku:" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red

        [PSCustomObject]@{
            "Nazwa Folderu"     = $folder.name
            "Link Google Drive" = "BLED"
        }
    }
}

# ============================================================
# 7. Wyswietlenie wynikow
# ============================================================

Write-Host ""
Write-Host ""
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host "WYNIKI" -ForegroundColor Cyan
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host ""

$results | Format-Table -AutoSize

# ============================================================
# 8. Ustalenie folderu skryptu (Uniwersalne dla ISE i VS Code)
# ============================================================

if (-not [string]::IsNullOrWhiteSpace($PSScriptRoot)) {
    $scriptFolder = $PSScriptRoot
}
elseif ($psISE -and $psISE.CurrentFile -and -not [string]::IsNullOrWhiteSpace($psISE.CurrentFile.FullPath)) {
    $scriptFolder = Split-Path -Parent $psISE.CurrentFile.FullPath
}
else {
    Write-Host ""
    Write-Host "Nie mozna ustalic folderu skryptu." -ForegroundColor Yellow
    Write-Host "Zapis powiazany zostanie z Pulpitem." -ForegroundColor Yellow

    $scriptFolder = [Environment]::GetFolderPath("Desktop")
}

# ============================================================
# 9. Podsumowanie
# ============================================================

$successCount = @(
    $results | Where-Object {
        $_."Link Google Drive" -ne "BLED" -and
        $_."Link Google Drive" -ne "BLED - brak linku"
    }
).Count

$errorCount = $results.Count - $successCount

Write-Host ""
Write-Host "Podsumowanie:" -ForegroundColor Cyan
Write-Host "  Wszystkich folderow : $($results.Count)" -ForegroundColor White
Write-Host "  Wygenerowanych linkow: $successCount" -ForegroundColor Green
Write-Host "  Bledow              : $errorCount" -ForegroundColor $(if ($errorCount -gt 0) { "Red" } else { "Green" })
Write-Host ""