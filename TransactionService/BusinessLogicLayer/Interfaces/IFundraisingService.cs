namespace BusinessLogicLayer.Interfaces
{
    public interface IFundraisingService
    {
        Task<string> CreateFundraising(string fundraisingUrl);
        Task<string> ChangeStatus(int fundraisingId);
    }
}
