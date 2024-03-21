using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUploadedCartoonService
    {
        Task<string> UploadedCartoon(CartoonUploadedModel cartoonUploadedModel);
        Task<string> UploadedCartoonPart(CartoonPartUploadedModel cartoonUploadedModel);
    }
}
