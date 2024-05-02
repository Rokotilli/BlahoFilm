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

        public async Task<string> Rate(int filmId, int rate, string userid)
        {
            try
            {
                var model = _dbContext.Rating.FirstOrDefault(r => r.FilmId == filmId && r.UserId == userid);
                var film = _dbContext.Films.FirstOrDefault(f => f.Id == filmId);

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
                }
                else
                {
                    model.Rate = rate;

                    _dbContext.Rating.Update(model);
                    await _dbContext.SaveChangesAsync();
                }

                var averageRating = _dbContext.Rating
                    .Where(r => r.FilmId == filmId)
                    .Average(r => r.Rate);

                var result = Math.Round(averageRating, 1);

                film.Rating = result;

                _dbContext.Films.Update(film);
                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch
            {
                return "Rating film failed!";
            }
        }
    }
}
