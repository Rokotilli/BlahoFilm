using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class FundraisingService : IFundraisingService
    {
        private readonly TransactionServiceDbContext _dbContext;

        public FundraisingService(TransactionServiceDbContext transactionServiceDbContext)
        {
            _dbContext = transactionServiceDbContext;
        }

        public async Task<string> CreateFundraising(FundraisingModel fundraisingModel)
        {
            try
            {
                var model = new Fundraising()
                {
                    Title = fundraisingModel.Title,
                    Description = fundraisingModel.Description,
                    Total = fundraisingModel.TotalAmount
                };

                _dbContext.Fundraisings.Add(model);
                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> ChangeFundraising(int fundraisingId, FundraisingModel fundraisingModel)
        {
            try
            {
                var model = _dbContext.Fundraisings.FirstOrDefault(f => f.Id == fundraisingId);

                if (model == null)
                {
                    return "Fundraising was not found!";
                }

                model.Title = fundraisingModel.Title;
                model.Description = fundraisingModel.Description;
                model.Total = fundraisingModel.TotalAmount;

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
