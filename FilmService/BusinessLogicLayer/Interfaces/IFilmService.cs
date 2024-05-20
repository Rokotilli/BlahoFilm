using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
    public interface IFilmService
    {
        Task<string> RegisterFilm(FilmRegisterModel filmRegisterModel);
        List<ReturnFilms> GetFilmsByFilterAndSorting(Dictionary<string, string[]> filters, int pageNumber, int pageSize, string sortByDate, string sortByPopularity);
        double GetCountPagesFilmsByFilter(Dictionary<string, string[]> filters, int pageSize, string sortByDate, string sortByPopularity);
    }
}
