using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;

namespace BusinessLogicLayer.Services
{
    public class UploadedSeriesPartService : IUploadedSeriesPartService
    {
        private readonly SeriesServiceDbContext _dbContext;

        public UploadedSeriesPartService(SeriesServiceDbContext seriesServiceDbContext)
        {
            _dbContext = seriesServiceDbContext;
        }
        public async Task<string> UploadedSeriesPart(SeriesPartUploadedModel seriesPartUploadedModel)
        {
            try
            {
                var model = _dbContext.SeriesParts
                .Where(c => c.Id == seriesPartUploadedModel.Id)
                .ToArray().First();

                model.FileUri = seriesPartUploadedModel.FileUri;
                model.FileName = seriesPartUploadedModel.FileName;
                model.Quality = seriesPartUploadedModel.Quality;

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
