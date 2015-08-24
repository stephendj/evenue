using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using Evenue.BackEndAPI.DataObjects;
using Evenue.BackEndAPI.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Evenue.BackEndAPI.Controllers
{
    // Give anonymous access to the event controller class
    [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class EventController : TableController<Event>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            EvenueBackEndAPIContext context = new EvenueBackEndAPIContext();
            DomainManager = new EntityDomainManager<Event>(context, Request, Services);
        }

        // GET tables/Event
        public IQueryable<Event> GetAllEvent()
        {
            return Query(); 
        }

        // GET tables/Event/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Event> GetEvent(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Event/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Event> PatchEvent(string id, Delta<Event> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Event
        public async Task<IHttpActionResult> PostEvent(Event item)
        {
            string storageAccountName;
            string storageAccountKey;

            // Try to get the Azure storage account token from app settings.  
            if (!(Services.Settings.TryGetValue("STORAGE_ACCOUNT_NAME", out storageAccountName) |
            Services.Settings.TryGetValue("STORAGE_ACCOUNT_ACCESS_KEY", out storageAccountKey)))
            {
                Services.Log.Error("Could not retrieve storage account settings.");
            }

            // Set the URI for the Blob Storage service.
            Uri blobEndpoint = new Uri(string.Format("https://{0}.blob.core.windows.net", storageAccountName));

            // Create the BLOB service client.
            CloudBlobClient blobClient = new CloudBlobClient(blobEndpoint,
                new StorageCredentials(storageAccountName, storageAccountKey));

            if (item.containerName != null)
            {
                // Set the BLOB store container name on the item, which must be lowercase.
                item.containerName = item.containerName.ToLower();

                // Create a container, if it doesn't already exist.
                CloudBlobContainer container = blobClient.GetContainerReference(item.containerName);
                await container.CreateIfNotExistsAsync();

                // Create a shared access permission policy. 
                BlobContainerPermissions containerPermissions = new BlobContainerPermissions();

                // Enable anonymous read access to BLOBs.
                containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                container.SetPermissions(containerPermissions);

                // Define a policy that gives write access to the container for 5 minutes.                                   
                SharedAccessBlobPolicy sasPolicy = new SharedAccessBlobPolicy()
                {
                    SharedAccessStartTime = DateTime.UtcNow,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(5),
                    Permissions = SharedAccessBlobPermissions.Write
                };

                // Get the SAS as a string.
                item.sasQueryString = container.GetSharedAccessSignature(sasPolicy);

                // Set the URL used to store the image.
                item.imageUri = string.Format("{0}{1}/{2}", blobEndpoint.ToString(),
                    item.containerName, item.resourceName);
            }

            Event current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Event/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteEvent(string id)
        {
             return DeleteAsync(id);
        }

    }
}