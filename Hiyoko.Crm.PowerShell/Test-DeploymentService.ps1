# Get the current script directory path
$ScriptPath = $(Split-Path -Path $script:MyInvocation.MyCommand.Path)

# Add the definition of the objects ontained in the Microsoft.Xrm.Sdk.Deployment namespace.
Add-Type -Path $($ScriptPath + "\microsoft.xrm.sdk.deployment.dll")

# Import the Get_DeploymentService class.
Import-Module -Name $($ScriptPath + "\Hiyoko.Crm.PowerShell.dll")

# Define the Deployment URI
$DeploymentUri = "https://crm.company.org/XRMDeployment/2011/Deployment.svc"

# Intanciate and retrieve the DeploymentServiceClient object.
$deploymentService = Get-DeploymentService -DeploymentUri $DeploymentUri

# Test connection
if($deploymentService.State -eq "Created")
{
    Write-Host "Deployment service created!"
}