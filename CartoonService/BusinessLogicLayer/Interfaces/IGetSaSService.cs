using Microsoft.Extensions.Configuration;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGetSaSService
    {
        Task<string> GetSaS(IConfiguration configuration, string containerName, string blobName);
    }
}
