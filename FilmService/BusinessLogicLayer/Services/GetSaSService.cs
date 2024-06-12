using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BusinessLogicLayer.Services
{
    public class GetSaSService : IGetSaSService
    {
        private readonly AppSettings _appSettings;

        public GetSaSService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<string> GetSaS(string blobName, BlobSasPermissions permission)
        {
            var connectionString = _appSettings.AzureStorage.ConnectionString;
            var containerName = _appSettings.AzureStorage.FilmsContainerName;
            var accountName = _appSettings.AzureStorage.AccountName;
            var accountKey = _appSettings.AzureStorage.AccountKey;

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
