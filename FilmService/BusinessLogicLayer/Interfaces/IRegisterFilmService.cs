using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRegisterFilmService
    {
        Task<string> RegisterFilm(FilmRegisterModel filmRegisterModel);
    }
}
