using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class RatingService : IRatingService
    {
        private readonly SeriesServiceDbContext _dbContext;

        public RatingService(SeriesServiceDbContext seriesServiceDbContext)
        {
            _dbContext = seriesServiceDbContext;
        }

        public async Task<string> RateSeries(int seriesId, int rate, string userid)
        {
            try
            {
                var model = _dbContext.Ratings.FirstOrDefault(r => r.SeriesId == seriesId && r.UserId == userid);
                var series = _dbContext.Series.FirstOrDefault(f => f.Id == seriesId);

                if (model == null)
                {
                    var rating = new Rating()
                    {
                        SeriesId = seriesId,
                        UserId = userid,
                        Rate = rate
                    };

                    _dbContext.Ratings.Add(rating);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    model.Rate = rate;

                    _dbContext.Ratings.Update(model);
                    await _dbContext.SaveChangesAsync();
                }

                var averageRating = _dbContext.Ratings
                    .Where(r => r.SeriesId == seriesId)
                    .Average(r => r.Rate);

                var result = Math.Round(averageRating, 1);

                series.Rating = result;

                _dbContext.Series.Update(series);
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
