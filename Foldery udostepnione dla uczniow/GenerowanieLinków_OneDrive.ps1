Uwaga trzeba to uruchomic w Powershell ISE

# Użycie: .\GenerowanieLinkówOneDriveDlaUczniów.ps1 -Klasa Klasa_5
param(
    [string]$Klasa = "Klasa_5"
)

# 1. Połączenie z z kontem (jeśli sesja jeszcze trwa, przejdzie od razu)
Connect-MgGraph -Scopes "Files.ReadWrite.All" -UseDeviceAuthentication

# 2. Ścieżka do docelowego folderu
$folderPath = "Udostępnione dla innych w chmurze/PSCHJK-dla uczniow/$Klasa"

# 3. Pobranie informacji o folderze klasy
try {
    $parentFolder = Invoke-MgGraphRequest -Method GET -Uri "https://graph.microsoft.com/v1.0/me/drive/root:/$($folderPath)"
} catch {
    Write-Error "Nie znaleziono folderu pod ścieżką: $folderPath"
    return
}

# 4. Pobranie podfolderów znajdujących się wewnątrz klasy
$subfoldersResponse = Invoke-MgGraphRequest -Method GET -Uri "https://graph.microsoft.com/v1.0/me/drive/items/$($parentFolder.id)/children?`$select=id,name,folder"
$subfolders = $subfoldersResponse.value | Where-Object { $_.folder -ne $null }

if ($subfolders.Count -eq 0) {
    Write-Host "W folderze '$folderPath' nie znaleziono żadnych podfolderów." -ForegroundColor Yellow
    return
}

Write-Host "Znaleziono $($subfolders.Count) podfolderów. Generowanie linków..." -ForegroundColor Cyan

# 5. Generowanie linków udostępniania do odczytu (View / Anonymous)
$results = foreach ($folder in $subfolders) {
    $body = @{
        type  = "view"
        scope = "anonymous"
    } | ConvertTo-Json

    # Tworzenie linku przez Microsoft Graph API
    $linkResponse = Invoke-MgGraphRequest -Method POST -Uri "https://graph.microsoft.com/v1.0/me/drive/items/$($folder.id)/createLink" -Body $body -ContentType "application/json"

    [PSCustomObject]@{
        "Nazwa Folderu" = $folder.name
        "Link OneDrive" = $linkResponse.link.webUrl
    }
}

# 6. Wyświetlenie wyników w konsoli
$results | Format-Table -AutoSize