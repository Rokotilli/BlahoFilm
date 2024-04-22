using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;

namespace BusinessLogicLayer.Interfaces
{
    public interface IFilmService
    {
        Task<string> RegisterFilm(FilmRegisterModel filmRegisterModel);
        List<ReturnFilms> GetFilmsByFilter(string[] items, string filter, int pageNumber, int pageSize);
        double GetCountPagesFilmsByFilter(string[] items, string filter, int pageSize);
    }
}
