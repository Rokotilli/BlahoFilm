using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGetSaSService
    {
        Task<string> GetSaS(string blobName, BlobSasPermissions permission);
    }
}
