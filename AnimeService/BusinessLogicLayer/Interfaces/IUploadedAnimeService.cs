using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUploadedAnimeService
    {
        Task<string> UploadedAnime(AnimeUploadedModel animeUploadedModel);
        Task<string> UploadedAnimePart(AnimePartUploadedModel animeUploadedModel);
    }
}
