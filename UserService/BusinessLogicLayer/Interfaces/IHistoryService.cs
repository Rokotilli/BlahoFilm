using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IHistoryService
    {
        Task<string> AddHistoryForFilm(string userId, HistoryModel historyModel);
        Task<string> AddHistoryForSeries(string userId, HistoryModel historyModel);
    }
}
