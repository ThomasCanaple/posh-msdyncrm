using System;
using System.Net;

// PowerShell Cmdlet base class namespace 
using System.Management.Automation;

// Dynamics CRM deployment namespace 
using Microsoft.Xrm.Sdk.Deployment.Proxy;

namespace Hiyoko.Crm.PowerShell
{
    // Windows PowerShell uses a verb-and-noun name pair to name cmdlets. 
    [Cmdlet(VerbsCommon.Get, "DeploymentService")]

    // Derive from the Cmdlet base class. 
    public class Get_DeploymentService : Cmdlet
    {
        // Define parameter as mandatory. This check is performed by the PowerShell runtime. 
        [Parameter(Mandatory = true)]

        // Validate that the parameter is not null or empty. This check is performed by the PowerShell runtime. 
        [ValidateNotNullOrEmpty]
        public Uri DeploymentUri { get; set; }

        /// <summary> 
        /// Impelment the method used to provide record-by-record processing functionality for the cmdlet. 
        /// </summary> 
        protected override void ProcessRecord()
        {
            // Create the deployment service from the deployment URI. 
            var service = ProxyClientHelper.CreateClient(DeploymentUri);

            // Use default network credentials if necessary. 
            if (service.ClientCredentials != null)
            {
                service.ClientCredentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            }

            // Deliver the Deployment service object to the pipeline. 
            WriteObject(service);
        }
    }
}