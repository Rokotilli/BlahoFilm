using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUploadedFilmService
    {
        Task<string> UploadedFilm(FilmUploadedModel filmUploadedModel);
    }
}
