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
                var model = _dbContext.SeriesRating.FirstOrDefault(r => r.SeriesId == seriesId && r.UserId == userid);
                var series = _dbContext.Seriess.FirstOrDefault(f => f.Id == seriesId);

                if (model == null)
                {
                    var rating = new SeriesRating()
                    {
                        SeriesId = seriesId,
                        UserId = userid,
                        Rate = rate
                    };

                    _dbContext.SeriesRating.Add(rating);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    model.Rate = rate;

                    _dbContext.SeriesRating.Update(model);
                    await _dbContext.SaveChangesAsync();
                }

                var averageRating = _dbContext.SeriesRating
                    .Where(r => r.SeriesId == seriesId)
                    .Average(r => r.Rate);

                var result = Math.Round(averageRating, 1);

                series.Rating = result;

                _dbContext.Seriess.Update(series);
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
                var seriesPart = _dbContext.Seriess.FirstOrDefault(f => f.Id == seriesPartId);

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

                _dbContext.Seriess.Update(seriesPart);
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
