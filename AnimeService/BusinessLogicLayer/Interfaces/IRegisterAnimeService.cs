using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRegisterAnimeService
    {
        Task<string> RegisterAnime(AnimeRegisterModel animeRegisterModel);
        Task<string> RegisterAnimePart(AnimePartRegisterModel animePartRegisterModel);
    }
}
