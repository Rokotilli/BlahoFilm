namespace BusinessLogicLayer.Interfaces
{
    public interface IHistoryService
    {
        Task AddHistory(string userId, int FilmId, TimeSpan TimeCode);
    }
}
