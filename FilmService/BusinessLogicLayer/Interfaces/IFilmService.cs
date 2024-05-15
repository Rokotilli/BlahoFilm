using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;

namespace BusinessLogicLayer.Interfaces
{
    public interface IFilmService
    {
        Task<string> RegisterFilm(FilmRegisterModel filmRegisterModel);
        List<ReturnFilms> GetFilmsByFilter(Dictionary<string, string[]> filters, int pageNumber, int pageSize);
        double GetCountPagesFilmsByFilter(Dictionary<string, string[]> filters, int pageSize);
    }
}
