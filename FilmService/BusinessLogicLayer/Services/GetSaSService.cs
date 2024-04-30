using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using BusinessLogicLayer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BusinessLogicLayer.Services
{
    public class GetSaSService : IGetSaSService
    {
        private readonly IConfiguration _configuration;

        public GetSaSService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetSaS(string blobName, BlobSasPermissions permission)
        {
            var connectionString = _configuration["AzureStorageConnectionString"];
            var containerName = _configuration["AzureStorageContainerName"];
            var accountName = _configuration["AzureStorageAccountName"];
            var accountKey = _configuration["AzureStorageAccountKey"];

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

                sasBuilder.SetPermissions(permission);

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
