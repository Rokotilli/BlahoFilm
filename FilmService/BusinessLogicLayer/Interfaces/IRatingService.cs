namespace BusinessLogicLayer.Interfaces
{
    public interface IRatingService
    {
        Task<string> Rate(int filmId, int rate, string userid);
    }
}
