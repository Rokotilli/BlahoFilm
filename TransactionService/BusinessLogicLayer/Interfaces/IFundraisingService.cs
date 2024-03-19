using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IFundraisingService
    {
        Task<string> CreateFundraising(FundraisingModel fundraisingModel);
        Task<string> ChangeFundraising(int fundraisingId, FundraisingModel fundraisingModel);
    }
}
