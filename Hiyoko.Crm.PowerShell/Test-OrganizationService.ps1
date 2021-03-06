﻿# Get the current script directory path
$ScriptPath = $(Split-Path -Path $script:MyInvocation.MyCommand.Path)

# Add the definition of the objects contained in the Microsoft.Xrm.Client namespace.
Add-Type -Path $($ScriptPath + "\microsoft.xrm.sdk.dll")

# Import the Get_OrganizationService class.
Import-Module -Name $($ScriptPath + "\Hiyoko.Crm.PowerShell.dll")

# Define the CRM connection string
$CrmConnectionString = "Url=https://crm.company.org/organization;"

# Retrieve the corresponding OrganizationService object.
$organizationService = Get-OrganizationService -CrmConnectionString $CrmConnectionString

# Test connection
if($organizationService.InnerService.IsAuthenticated)
{
    Write-Host "User is authenticated!"
}