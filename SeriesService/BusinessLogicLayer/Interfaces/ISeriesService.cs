using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ISeriesService
    {
        List<ReturnSeries> GetSeriesByFilterAndSorting(Dictionary<string, string[]> filters, int pageNumber, int pageSize, string sortByDate, string sortByPopularity);
        double GetCountPagesSeriesByFilter(Dictionary<string, string[]> filters, int pageSize, string sortByDate, string sortByPopularity);
        Task<string> CreateSelection(SelectionAddModel selectionAddModel);
    }
}
