using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAnimeService
    {
        List<ReturnAnime> GetAnimeByFilterAndSorting(Dictionary<string, string[]> filters, int pageNumber, int pageSize, string sortByDate, string sortByPopularity);
        double GetCountPagesAnimeByFilter(Dictionary<string, string[]> filters, int pageSize, string sortByDate, string sortByPopularity);
        Task<string> CreateSelection(SelectionAddModel selectionAddModel);
    }
}
