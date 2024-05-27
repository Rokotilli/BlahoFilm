using BusinessLogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface ICartoonService
    {
        List<ReturnCartoon> GetCartoonsByFilterAndSorting(Dictionary<string, string[]> filters, int pageNumber, int pageSize, string sortByDate, string sortByPopularity);
        double GetCountPagesCartoonsByFilter(Dictionary<string, string[]> filters, int pageSize, string sortByDate, string sortByPopularity);
        Task<string> CreateSelection(SelectionAddModel selectionAddModel);
    }
}
