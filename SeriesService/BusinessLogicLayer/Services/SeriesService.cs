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
    public class ReturnSeries : Series
    {
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Studio> Studios { get; set; }
        public IEnumerable<Selection> Selections { get; set; }
    }
    public class SeriesService : ISeriesService
    {
        private readonly SeriesServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public SeriesService(SeriesServiceDbContext filmServiceDbContext, IPublishEndpoint publishEndpoint)
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
        public List<ReturnSeries> GetSeriesByFilterAndSorting(Dictionary<string, string[]> filters, int pageNumber, int pageSize, string sortByDate, string sortByPopularity)
        {
            var query = ApplyFiltersAndSorting(filters, sortByDate, sortByPopularity);

            var result = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(f => ToReturnSeries(f))
                .ToList();

            return result;
        }
        private IQueryable<Series> ApplyFiltersAndSorting(Dictionary<string, string[]> filters, string sortByDate, string sortByPopularity)
        {
            var query = _dbContext.Series.AsQueryable();

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    switch (filter.Key)
                    {
                        case "Genres":
                            query = query.Where(a => filter.Value.All(g => a.GenresSeries.Any(ga => ga.Genre.Name == g)));
                            break;
                        case "Categories":
                            query = query.Where(a => filter.Value.All(t => a.CategoriesSeries.Any(ta => ta.Category.Name == t)));
                            break;
                        case "Studios":
                            query = query.Where(a => filter.Value.All(s => a.StudiosSeries.Any(sa => sa.Studio.Name == s)));
                            break;
                        case "Selections":
                            query = query.Where(a => filter.Value.All(s => a.SelectionSeries.Any(sa => sa.Selection.Name == s)));
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
                .Include(a => a.GenresSeries).ThenInclude(ga => ga.Genre)
                .Include(a => a.CategoriesSeries).ThenInclude(cf => cf.Category)
                .Include(a => a.StudiosSeries).ThenInclude(sa => sa.Studio)
                .Include(a => a.SelectionSeries).ThenInclude(sa => sa.Selection);
        }

        public double GetCountPagesSeriesByFilter(Dictionary<string, string[]> filters, int pageSize, string sortByDate, string sortByPopularity)
        {
            var query = ApplyFiltersAndSorting(filters, sortByDate, sortByPopularity);

            var count = query.Count();

            if (count == 0)
            {
                return 0;
            }

            return Math.Ceiling((double)count / pageSize);
        }
        public static ReturnSeries ToReturnSeries(Series a)
        {
            return new ReturnSeries
            {
                Id = a.Id,
                Poster = a.Poster,
                PosterPartOne = a.PosterPartOne,
                PosterPartTwo = a.PosterPartTwo,
                PosterPartThree = a.PosterPartThree,
                Title = a.Title,
                Description = a.Description,
                CountSeasons = a.CountSeasons,
                CountParts = a.CountParts,
                DateOfPublish = a.DateOfPublish,
                Director = a.Director,
                Actors = a.Actors,
                Rating = a.Rating,
                Quality = a.Quality,
                Country = a.Country,
                TrailerUri = a.TrailerUri,
                AgeRestriction = a.AgeRestriction,
                Genres = a.GenresSeries.Select(ga => new Genre { Id = ga.GenreId, Name = ga.Genre.Name }),
                Categories = a.CategoriesSeries.Select(ta => new Category { Id = ta.CategoryId, Name = ta.Category.Name }),
                Studios = a.StudiosSeries.Select(sa => new Studio { Id = sa.StudioId, Name = sa.Studio.Name }),
                Selections = a.SelectionSeries?.Select(sa => new Selection { Id = sa.SelectionId, Name = sa.Selection.Name }),
            };
        }
    }
}
