using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class RatingService : IRatingService
    {
        private readonly AnimeServiceDbContext _dbContext;

        public RatingService(AnimeServiceDbContext animeServiceDbContext)
        {
            _dbContext = animeServiceDbContext;
        }

        public async Task<string> RateAnime(int animeId, int rate, string userid)
        {
            try
            {
                var model = _dbContext.AnimeRating.FirstOrDefault(r => r.AnimeId == animeId && r.UserId == userid);
                var anime = _dbContext.Animes.FirstOrDefault(f => f.Id == animeId);

                if (model == null)
                {
                    var rating = new AnimeRating()
                    {
                        AnimeId = animeId,
                        UserId = userid,
                        Rate = rate
                    };

                    _dbContext.AnimeRating.Add(rating);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    model.Rate = rate;

                    _dbContext.AnimeRating.Update(model);
                    await _dbContext.SaveChangesAsync();
                }

                var averageRating = _dbContext.AnimeRating
                    .Where(r => r.AnimeId == animeId)
                    .Average(r => r.Rate);

                var result = Math.Round(averageRating, 1);

                anime.Rating = result;

                _dbContext.Animes.Update(anime);
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
