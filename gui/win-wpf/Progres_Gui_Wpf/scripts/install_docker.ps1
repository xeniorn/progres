## Ensure running as Administrator
#if (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator"))
#{
#    Write-Warning "Please run PowerShell as Administrator"
#    break
#}

# Check if Docker is installed
$dockerInstalled = $null -ne (Get-Command docker -ErrorAction SilentlyContinue)

if (-not $dockerInstalled) {
    # Attempt to install Docker using winget
    try {
        winget install --id=Docker.DockerDesktop --source=winget --accept-package-agreements --accept-source-agreements
        Write-Host "Docker has been installed successfully."
    } catch {
        Write-Error "Failed to install Docker. Error: $_"
    }
} else {
    Write-Host "Docker is already installed."
}
