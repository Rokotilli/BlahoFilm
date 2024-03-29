using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;

namespace BusinessLogicLayer.Services
{
    public class UploadedSeriesService : IUploadedSeriesService
    {
        private readonly SeriesServiceDbContext _dbContext;

        public UploadedSeriesService(SeriesServiceDbContext seriesServiceDbContext)
        {
            _dbContext = seriesServiceDbContext;
        }

        public async Task<string> UploadedSeries(SeriesUploadedModel seriesUploadedModel)
        {
            try
            {
                var model = _dbContext.Seriess
                .Where(c => c.Id == seriesUploadedModel.Id)
                .ToArray().First();

                model.FileUri = seriesUploadedModel.FileUri;

                _dbContext.Seriess.Update(model);
                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UploadedSeriesPart(SeriesPartUploadedModel seriesPartUploadedModel)
        {
            try
            {
                var model = _dbContext.SeriesParts
                .Where(c => c.Id == seriesPartUploadedModel.Id)
                .ToArray().First();

                model.FileUri = seriesPartUploadedModel.FileUri;

                _dbContext.SeriesParts.Update(model);
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
