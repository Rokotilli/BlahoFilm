namespace BusinessLogicLayer.Interfaces
{
    public interface IRatingService
    {
        Task<double> GetRating(int filmId);
        Task<string> Rate(int filmId, int rate, string userid);
    }
}
