using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class RatingService : IRatingService
    {
        private readonly FilmServiceDbContext _dbContext;

        public RatingService(FilmServiceDbContext filmServiceDbContext)
        {
            _dbContext = filmServiceDbContext;
        }

        public async Task<double> GetRating(int filmId)
        {
            var model = _dbContext.Rating.Where(r => r.FilmId == filmId).ToArray();

            if (!model.Any())
            {
                return 0;
            }

            var sum = model
                .Where(r => r.FilmId == filmId)
                .Select(r => r.Rate)
                .Sum();

            var count = model
                .Where(r => r.FilmId == filmId)
                .Count();

            var result = Math.Round((double)sum / count, 1);

            return result;
        }

        public async Task<string> Rate(int filmId, int rate, string userid)
        {
            try
            {
                var model = _dbContext.Rating.FirstOrDefault(r => r.FilmId == filmId && r.UserId == userid);

                if (model == null)
                {
                    var rating = new Rating()
                    {
                        FilmId = filmId,
                        UserId = userid,
                        Rate = rate
                    };

                    _dbContext.Rating.Add(rating);
                    await _dbContext.SaveChangesAsync();

                    return null;
                }

                model.Rate = rate;

                _dbContext.Rating.Update(model);
                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
