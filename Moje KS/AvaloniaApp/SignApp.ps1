param(
	[Parameter(Mandatory = $true)]
	[string] $TargetFile
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $TargetFile -PathType Leaf)) {
	throw "The application binary was not found: $TargetFile"
}

$cert = Get-ChildItem Cert:\CurrentUser\My |
	Where-Object { $_.Subject -eq 'CN=MojeKSLocalDev' -and $_.HasPrivateKey } |
	Sort-Object NotAfter -Descending |
	Select-Object -First 1

if (-not $cert) {
	$cert = New-SelfSignedCertificate `
		-Type CodeSigningCert `
		-Subject 'CN=MojeKSLocalDev' `
		-CertStoreLocation Cert:\CurrentUser\My `
		-NotAfter (Get-Date).AddYears(1)
}

$result = Set-AuthenticodeSignature -FilePath $TargetFile -Certificate $cert
if ($result.Status -ne 'Valid') {
	throw "Signing failed for '$TargetFile'. Status: $($result.Status); message: $($result.StatusMessage)"
}

Write-Host "Signed $TargetFile with $($cert.Thumbprint)"
