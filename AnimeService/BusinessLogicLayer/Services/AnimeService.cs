using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ReturnAnime: Anime
    {
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Studio> Studios { get; set; }
    }
    public class AnimeService : IAnimeService
    {
        public Task<string> CreateSelection(SelectionAddModel selectionAddModel)
        {
            throw new NotImplementedException();
        }

        public List<ReturnAnime> GetAnimeByFilterAndSorting(Dictionary<string, string[]> filters, int pageNumber, int pageSize, string sortByDate, string sortByPopularity)
        {
            throw new NotImplementedException();
        }

        public double GetCountPagesAnimeByFilter(Dictionary<string, string[]> filters, int pageSize, string sortByDate, string sortByPopularity)
        {
            throw new NotImplementedException();
        }

        public Task<string> RegisterAnime(AnimeRegisterModel animeRegisterModel)
        {
            throw new NotImplementedException();
        }
    }
}
