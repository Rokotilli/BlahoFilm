using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        public IEnumerable<Selection>? Selections { get; set; }
    }
    public class AnimeService : IAnimeService
    {
        private readonly AnimeServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public AnimeService(AnimeServiceDbContext filmServiceDbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = filmServiceDbContext;
            _publishEndpoint = publishEndpoint;
        }
        private async Task<byte[]> ReadBytesAsync(IFormFile file)
        {
            if (file == null)
                return null;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                return stream.ToArray();
            }
        }
        public async Task<string> CreateSelection(SelectionAddModel selectionAddModel)
        {
            try
            {
                var existSelection = await _dbContext.Selections.FirstOrDefaultAsync(s => s.Name == selectionAddModel.Name);

                if (existSelection != null)
                {
                    return "This selection already exists!";
                }

                byte[] imageBytes = await ReadBytesAsync(selectionAddModel.Image);

                var model = new Selection
                {
                    Name = selectionAddModel.Name,
                    Image = imageBytes
                };

                await _dbContext.Selections.AddAsync(model);
                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch
            {
                return "Adding selection failed!";
            }
        }
        public List<ReturnAnime> GetAnimeByFilterAndSorting(Dictionary<string, string[]> filters, int pageNumber, int pageSize, string sortByDate, string sortByPopularity)
        {
            var query = ApplyFiltersAndSorting(filters, sortByDate, sortByPopularity);

            var result = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(f => ToReturnAnime(f))
                .ToList();

            return result;
        }
        private IQueryable<Anime> ApplyFiltersAndSorting(Dictionary<string, string[]> filters, string sortByDate, string sortByPopularity)
        {
            var query = _dbContext.Animes.AsQueryable();

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    switch (filter.Key)
                    {
                        case "Genres":
                            query = query.Where(a => filter.Value.All(g => a.GenresAnimes.Any(ga => ga.Genre.Name == g)));
                            break;
                        case "Categories":
                            query = query.Where(a => filter.Value.All(t => a.CategoriesAnimes.Any(ta => ta.Category.Name == t)));
                            break;
                        case "Studios":
                            query = query.Where(a => filter.Value.All(s => a.StudiosAnime.Any(sa => sa.Studio.Name == s)));
                            break;
                        case "Selections":
                            query = query.Where(a => filter.Value.All(s => a.SelectionAnimes.Any(sa => sa.Selection.Name == s)));
                            break;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(sortByDate))
            {
                query = sortByDate == "desc" ? query.OrderByDescending(a => a.DateOfPublish) : query.OrderBy(a => a.DateOfPublish);
            }

            if (!string.IsNullOrWhiteSpace(sortByPopularity))
            {
                query = sortByPopularity switch
                {
                    "rating" => query.OrderByDescending(a => a.Rating),
                    "discussing" => query.OrderByDescending(a => a.Comments.Count)
                };
            }

            return query
                .Include(a => a.GenresAnimes).ThenInclude(ga => ga.Genre)
                .Include(a => a.CategoriesAnimes).ThenInclude(cf => cf.Category)
                .Include(a => a.StudiosAnime).ThenInclude(sa => sa.Studio)
                .Include(a => a.SelectionAnimes).ThenInclude(sa => sa.Selection);
        }

        public double GetCountPagesAnimeByFilter(Dictionary<string, string[]> filters, int pageSize, string sortByDate, string sortByPopularity)
        {
            var query = ApplyFiltersAndSorting(filters, sortByDate, sortByPopularity);

            var count = query.Count();

            if (count == 0)
            {
                return 0;
            }

            return Math.Ceiling((double)count / pageSize);
        }
        public static ReturnAnime ToReturnAnime(Anime a)
        {
            return new ReturnAnime
            {
                Id = a.Id,
                Poster = a.Poster,
                PosterPartOne = a.PosterPartOne,
                PosterPartTwo = a.PosterPartTwo,
                PosterPartThree = a.PosterPartThree,
                Title = a.Title,
                Quality = a.Quality,
                Description = a.Description,
                CountSeasons = a.CountSeasons,
                CountParts = a.CountParts,
                DateOfPublish = a.DateOfPublish,
                Director = a.Director,
                Actors = a.Actors,
                Rating = a.Rating,
                TrailerUri = a.TrailerUri,
                AgeRestriction = a.AgeRestriction,
                FileName = a.FileName,
                FileUri = a.FileUri,
                Country = a.Country,
                Genres = a.GenresAnimes.Select(ga => new Genre { Id = ga.GenreId, Name = ga.Genre.Name }),
                Categories = a.CategoriesAnimes.Select(ta => new Category { Id = ta.CategoryId, Name = ta.Category.Name }),
                Studios = a.StudiosAnime.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),
                Selections = a.SelectionAnimes?.Select(sa => new Selection { Id = sa.SelectionId, Name = sa.Selection.Name }),
            };
        }
    }      
}
