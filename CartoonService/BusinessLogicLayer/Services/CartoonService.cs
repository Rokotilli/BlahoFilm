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
using System.Xml.Linq;

namespace BusinessLogicLayer.Services
{
    public class ReturnCartoon: Cartoon
    {
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Studio> Studios { get; set; }
    }
    public class CartoonService : ICartoonService
    {
        private readonly CartoonServiceDbContext _dbContext;

        public CartoonService(CartoonServiceDbContext filmServiceDbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = filmServiceDbContext;
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
        public List<ReturnCartoon> GetCartoonsByFilterAndSorting(Dictionary<string, string[]> filters, int pageNumber, int pageSize, string sortByDate, string sortByPopularity)
        {
            var query = ApplyFiltersAndSorting(filters, sortByDate, sortByPopularity);

            var result = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(f => ToReturnCartoon(f))
            .ToList();

            return result;
        }
        private IQueryable<Cartoon> ApplyFiltersAndSorting(Dictionary<string, string[]> filters, string sortByDate, string sortByPopularity)
        {
            var query = _dbContext.Cartoons.AsQueryable();

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    switch (filter.Key)
                    {
                        case "Genres":
                            query = query.Where(a => filter.Value.All(g => a.GenresCartoons.Any(ga => ga.Genre.Name == g)));
                            break;
                        case "Categories":
                            query = query.Where(a => filter.Value.All(t => a.CategoriesCartoons.Any(ta => ta.Category.Name == t)));
                            break;
                        case "Studios":
                            query = query.Where(a => filter.Value.All(s => a.StudiosCartoons.Any(sa => sa.Studio.Name == s)));
                            break;
                        case "Selections":
                            query = query.Where(a => filter.Value.All(s => a.SelectionCartoons.Any(sa => sa.Selection.Name == s)));
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
                .Include(a => a.GenresCartoons).ThenInclude(ga => ga.Genre)
                .Include(a => a.CategoriesCartoons).ThenInclude(cf => cf.Category)
                .Include(a => a.StudiosCartoons).ThenInclude(sa => sa.Studio)
                .Include(a => a.SelectionCartoons).ThenInclude(sa => sa.Selection);
        }

        public double GetCountPagesCartoonsByFilter(Dictionary<string, string[]> filters, int pageSize, string sortByDate, string sortByPopularity)
        {
            var query = ApplyFiltersAndSorting(filters, sortByDate, sortByPopularity);

            var count = query.Count();

            if (count == 0)
            {
                return 0;
            }

            return Math.Ceiling((double)count / pageSize);
        }
        public static ReturnCartoon ToReturnCartoon(Cartoon c)
        {
            return new ReturnCartoon
            {
                Poster = c.Poster,
                PosterPartOne = c.PosterPartOne,
                PosterPartTwo = c.PosterPartTwo,
                PosterPartThree = c.PosterPartThree,
                Title = c.Title,
                Description = c.Description,
                Duration = c.Duration,
                CountSeasons = c.CountSeasons,
                CountParts = c.CountParts,
                DateOfPublish = c.DateOfPublish,
                Director = c.Director,
                Rating = c.Rating,
                TrailerUri = c.TrailerUri,
                AgeRestriction = c.AgeRestriction,
                Genres = c.GenresCartoons.Select(gc => new Genre { Id = gc.GenreId, Name = gc.Genre.Name }),
                Categories = c.CategoriesCartoons.Select(tc => new Category { Id = tc.CategoryId, Name = tc.Category.Name }),
                Studios = c.StudiosCartoons.Select(sc => new Studio { Id = sc.StudioId, Name = sc.Studio.Name }),
            };
        }
    }
}
