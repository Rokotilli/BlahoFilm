using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using BusinessLogicLayer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BusinessLogicLayer.Services
{
    public class GetSaSService : IGetSaSService
    {
        public async Task<string> GetSaS(IConfiguration configuration, string containerName, string blobName)
        {
            var connectionString = configuration["AzureStorageConnectionString"];
            var accountName = configuration["AzureStorageAccountName"];
            var accountKey = configuration["AzureStorageAccountKey"];

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            if (await containerClient.ExistsAsync())
            {
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerClient.Name,
                    BlobName = blobClient.Name,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow,
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(10),
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Write);
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                StorageSharedKeyCredential storageSharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey);
                string sasToken = sasBuilder.ToSasQueryParameters(storageSharedKeyCredential).ToString();

                return sasToken;
            }
            else
            {
                return null;
            }
        }
    }
}
