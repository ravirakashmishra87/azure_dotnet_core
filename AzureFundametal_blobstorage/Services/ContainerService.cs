using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureFundametal_blobstorage.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public ContainerService(BlobServiceClient blobServiceClient)
        {
              _blobServiceClient = blobServiceClient;

        }
        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient blobcontainerclient = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobcontainerclient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobcontainerclient = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobcontainerclient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainerAndBlobs()
        {
             List<string> Containerandblob = new List<string>();
            Containerandblob.Add("-----------All Containers and container blobs ------------");
            Containerandblob.Add("---------------------------------------------------------");
            await  foreach (BlobContainerItem blobcontaineritem in _blobServiceClient.GetBlobContainersAsync())
            {
                Containerandblob.Add("---"+blobcontaineritem.Name);

                BlobContainerClient blobContainerclinet = _blobServiceClient.GetBlobContainerClient(blobcontaineritem.Name);

                await foreach(BlobItem  blobitem in blobContainerclinet.GetBlobsAsync())
                {
                    var blobclient = blobContainerclinet.GetBlobClient(blobitem.Name);
                    BlobProperties blobprops =await blobclient.GetPropertiesAsync();
                    string blobToAdd = blobitem.Name;
                    if(blobprops.Metadata.ContainsKey("title")) {
                        blobToAdd += "  [" + blobprops.Metadata["title"] + "]"; 
                    }
                    Containerandblob.Add("------" + blobToAdd);
                }
            }
            return Containerandblob;
        }

        public async Task<List<string>> GetAllContainers()
        {
            List<string> ContainerNames = new();

            await foreach(BlobContainerItem blobcontaineritem in _blobServiceClient.GetBlobContainersAsync()) {
                ContainerNames.Add(blobcontaineritem.Name);
            }
            return ContainerNames;
        }
    }
}
