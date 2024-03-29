using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUploadedSeriesService
    {
        Task<string> UploadedSeries(SeriesUploadedModel seriesUploadedModel);
        Task<string> UploadedSeriesPart(SeriesPartUploadedModel seriesUploadedModel);
    }
}
