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
                var model = _dbContext.SeriesRatings.FirstOrDefault(r => r.SeriesId == seriesId && r.UserId == userid);
                var series = _dbContext.Series.FirstOrDefault(f => f.Id == seriesId);

                if (model == null)
                {
                    var rating = new SeriesRating()
                    {
                        SeriesId = seriesId,
                        UserId = userid,
                        Rate = rate
                    };

                    _dbContext.SeriesRatings.Add(rating);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    model.Rate = rate;

                    _dbContext.SeriesRatings.Update(model);
                    await _dbContext.SaveChangesAsync();
                }

                var averageRating = _dbContext.SeriesRatings
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


        public async Task<string> RateSeriesPart(int seriesPartId, int rate, string userid)
        {
            try
            {
                var model = _dbContext.SeriesPartRating.FirstOrDefault(r => r.SeriesPartId == seriesPartId && r.UserId == userid);
                var seriesPart = _dbContext.Series.FirstOrDefault(f => f.Id == seriesPartId);

                if (model == null)
                {
                    var rating = new SeriesPartRating()
                    {
                        SeriesPartId = seriesPartId,
                        UserId = userid,
                        Rate = rate
                    };

                    _dbContext.SeriesPartRating.Add(rating);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    model.Rate = rate;

                    _dbContext.SeriesPartRating.Update(model);
                    await _dbContext.SaveChangesAsync();
                }

                var averageRating = _dbContext.SeriesPartRating
                    .Where(r => r.SeriesPartId == seriesPartId)
                    .Average(r => r.Rate);

                var result = Math.Round(averageRating, 1);

                seriesPart.Rating = result;

                _dbContext.Series.Update(seriesPart);
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
