// PowerShell Cmdlet base class namespace 
using System.Management.Automation;

// Dynamics CRM client namespace 
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;

namespace Hiyoko.Crm.PowerShell
{
    // Windows PowerShell uses a verb-and-noun name pair to name cmdlets. The couple defined here is used by PowerShell runtime. 
    [Cmdlet(VerbsCommon.Get, "OrganizationService")]

    // Derive from the Cmdlet base class. 
    public class Get_OrganizationService : Cmdlet
    {
        // Define parameter as mandatory. This check is performed by the PowerShell runtime. 
        [Parameter(Mandatory = true)]

        // Validate that the parameter is not null or empty. This check is performed by the PowerShell runtime. 
        [ValidateNotNullOrEmpty]
        public string CrmConnectionString { get; set; }

        /// <summary> 
        /// Impelment the ProcessRecord method to provide record-by-record processing functionality for the cmdlet. 
        /// </summary> 
        protected override void ProcessRecord()
        {
            // Parse CRM connection string. 
            var connection = CrmConnection.Parse(CrmConnectionString);

            // Instanciate Organization service. 
            var service = new OrganizationService(connection);

            // Deliver the Organization service object to the pipeline. 
            WriteObject(service);
        }
    }
}