using System;
// ALso needs a reference on System.RUntime.Serialization assembly

// PowerShell Cmdlet base class namespace 
using System.Management.Automation;

// Dynamics CRM client namespace 
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace Hiyoko.Crm.PowerShell
{
    // Windows PowerShell uses a verb-and-noun name pair to name cmdlets. The couple defined here is used by PowerShell runtime. 
    [Cmdlet(VerbsCommon.Rename, "Attribute")]

    // Derive from the Cmdlet base class.
    public class Rename_Attribute : Cmdlet
    {
        // Define parameter as mandatory. This check is performed by the PowerShell runtime.
        [Parameter(Mandatory = true, HelpMessage = "CRM Organization service")]

        // Validate that the parameter is not null or empty. This check is performed by the PowerShell runtime.
        [ValidateNotNullOrEmpty]
        public OrganizationService Service { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Entity Logical Name")]
        [ValidateNotNullOrEmpty]
        public string EntityLogicalName { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Attribute Logical Name")]
        [ValidateNotNullOrEmpty]
        public string AttributeLogicalName { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Display Name")]
        [ValidateNotNullOrEmpty]
        public string DisplayName { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Language Code")]

        // Validate that the parameter's value is in a possible range of values.
        // Source: https://msdn.microsoft.com/en-us/library/ms912047%28WinEmbedded.10%29.aspx
        [ValidateRange(1025, 20490)]

        public int LanguageCode { get; set; }

        /// <summary> 
        /// Implement the ProcessRecord method to provide record-by-record processing functionality for the cmdlet. 
        /// </summary>
        protected override void ProcessRecord()
        {
            // Retrieve CRM attribute metadata
            var attributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = EntityLogicalName,
                LogicalName = AttributeLogicalName,
                RetrieveAsIfPublished = false
            };
            var attributeResponse = (RetrieveAttributeResponse)Service.Execute(attributeRequest);

            if (attributeResponse == null)
            {
                var exception = new Exception("An error occured while updating attribute metadata: response is null.");
                var errorRecord = new ErrorRecord(exception, exception.Message, ErrorCategory.InvalidResult, attributeResponse);

                /*
                 * When a cmdlet encounters a terminating error, call this method rather than simply throwing an exception.
                 * Calling this method allows the cmdlet to attach additional error record information that describes 
                 * the condition that caused the terminating error. 
                 * When this method is called, the Windows PowerShell runtime catches the error record and 
                 * then starts shutting down the pipeline.
                 */
                ThrowTerminatingError(errorRecord);
            }

            if (attributeResponse.AttributeMetadata == null)
            {
                var exception = new Exception("An error occured while updating attribute metadata: attribute metadata is null.");
                var errorRecord = new ErrorRecord(exception, exception.Message, ErrorCategory.InvalidResult, attributeResponse);

                ThrowTerminatingError(errorRecord);
            }

            var retrievedAttributeMetadata = attributeResponse.AttributeMetadata;

            // Update the retrieved attribute metadata.
            retrievedAttributeMetadata.DisplayName = new Label(DisplayName, LanguageCode);

            // Send the updated attribute metadata to the CRM server.
            var updateRequest = new UpdateAttributeRequest
            {
                Attribute = retrievedAttributeMetadata,
                EntityName = EntityLogicalName,
                MergeLabels = false
            };
            Service.Execute(updateRequest);

            // Write a verbose output to the PowerShell prompt.
            WriteVerbose(string.Format("Attribute: {0} of entity: {1} updated with new display name: {2} for language code: {3}",
                AttributeLogicalName,
                EntityLogicalName,
                DisplayName,
                LanguageCode));
        }
    }
}
