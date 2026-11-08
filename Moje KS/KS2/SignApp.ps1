$cert = Get-ChildItem Cert:\CurrentUser\My | Where-Object { $_.Subject -eq 'CN=MojeKSLocalDev' } | Select-Object -First 1
if (-not $cert) {
    $cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject "CN=MojeKSLocalDev" -CertStoreLocation Cert:\CurrentUser\My
}
$targetFile = $args[0]
if ($targetFile -and (Test-Path $targetFile)) {
    Set-AuthenticodeSignature -FilePath $targetFile -Certificate $cert
}