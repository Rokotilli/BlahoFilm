using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
    public interface IFavoritesService
    {
        Task<string> Favorite(MediaWithType mediaWithType, string userid);
    }
}
