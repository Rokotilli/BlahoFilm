using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRegisterSeriesService
    {
        Task<string> RegisterSeries(SeriesRegisterModel seriesRegisterModel);
        Task<string> RegisterSeriesPart(SeriesPartRegisterModel seriesPartRegisterModel);
    }
}
