using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using AzureFundametal_blobstorage.Models;

namespace AzureFundametal_blobstorage.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        public async Task<bool> DeleteBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobclient = blobContainerClient.GetBlobClient(name);
            return await blobclient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();

            List<string> result = new List<string>();
            await foreach (var blob in blobs)
            {
                result.Add(blob.Name);
            }
            return result;
        }

        public async Task<List<Blob>> GetAllBlobswithUri(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            string containerSASToken = string.Empty;
            if(blobContainerClient.CanGenerateSasUri)
            {
                containerSASToken = GenerateSASToken_container(blobContainerClient);
            }
            var blobs = blobContainerClient.GetBlobsAsync();
            List<Blob> blobList = new List<Blob>();
            await foreach (var blob in blobs)
            {
                var blobclient = blobContainerClient.GetBlobClient(blob.Name);
                Blob blobindividual = new Blob()
                {
                    //Uri = blobclient.Uri.AbsoluteUri
                    //if uri need to be with SAS token
                    Uri = blobclient.Uri.AbsoluteUri + "?" + containerSASToken
                };
                /*
                if (blobclient.CanGenerateSasUri)
                {
                    blobindividual.Uri = GenerateSASToken_blob(blobclient);
                }
                */

                BlobProperties blobProperties = await blobclient.GetPropertiesAsync();
                if (blobProperties.Metadata.ContainsKey("Title"))
                {
                    blobindividual.Title = blobProperties.Metadata["Title"];
                }
                if (blobProperties.Metadata.ContainsKey("Comment"))
                {
                    blobindividual.Comment = blobProperties.Metadata["Comment"];
                }
                blobList.Add(blobindividual);
            }
            return blobList;
        }

        //GENERATE SAS TOKEN AT BLOB LEVEL
        private string GenerateSASToken_blob(BlobClient blobclient)
        {
            string token = string.Empty;

            BlobSasBuilder sasbuilder = new()
            {
                BlobContainerName = blobclient.GetParentBlobContainerClient().Name,
                BlobName = blobclient.Name,
                Resource = "b"
            };
            sasbuilder.ExpiresOn = DateTime.UtcNow.AddMinutes(2);
            sasbuilder.SetPermissions(BlobAccountSasPermissions.Read);
            token = blobclient.GenerateSasUri(sasbuilder).AbsoluteUri;

            return token;
        }

        //GENERATE SAS TOKEN AT CONTAINER LEVEL
        private string GenerateSASToken_container(BlobContainerClient blobContainerClient)
        {
            string token = string.Empty;

            BlobSasBuilder sasbuilder = new()
            {
                BlobContainerName = blobContainerClient.Name,                
                Resource = "c"
            };
            sasbuilder.ExpiresOn = DateTime.UtcNow.AddMinutes(2);
            sasbuilder.SetPermissions(BlobAccountSasPermissions.Read);
            token = blobContainerClient.GenerateSasUri(sasbuilder).AbsoluteUri.Split("?")[1].ToString();

            return token;
        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobclient = blobContainerClient.GetBlobClient(name);
            return blobclient.Uri.AbsoluteUri;
        }


        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName, Blob blob)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobclient = blobContainerClient.GetBlobClient(name);

            var httpHeader = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            IDictionary<string, string> metadata = new Dictionary<string, string>();
            metadata["Title"] = blob.Title;
            metadata.Add("Comment", blob.Comment);

            var result = await blobclient.UploadAsync(file.OpenReadStream(), httpHeader, metadata);
            /***** CODE TO UPDATE OR REMOVE METADATA 
            metadata.Remove("Title");
            await blobclient.SetMetadataAsync(metadata);
            *******/

            if (result != null)
                return true;
            return false;
        }
    }
}
