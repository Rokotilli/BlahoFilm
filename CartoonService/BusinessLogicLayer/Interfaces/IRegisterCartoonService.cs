using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRegisterCartoonService
    {
        Task<string> RegisterCartoon(CartoonRegisterModel cartoonRegisterModel);
        Task<string> RegisterCartoonPart(CartoonPartRegisterModel cartoonPartRegisterModel);
    }
}
