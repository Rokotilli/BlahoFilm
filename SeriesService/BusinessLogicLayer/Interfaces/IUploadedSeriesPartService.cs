using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUploadedSeriesPartService
    {
        Task<string> UploadedSeriesPart(SeriesPartUploadedModel seriesUploadedModel);
    }
}
