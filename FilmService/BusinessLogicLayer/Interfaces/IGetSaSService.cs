
using Azure.Storage.Sas;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGetSaSService
    {
        Task<string> GetSaS(string blobName, BlobSasPermissions permission);
    }
}
