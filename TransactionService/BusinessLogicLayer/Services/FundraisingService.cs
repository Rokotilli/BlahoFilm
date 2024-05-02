using BusinessLogicLayer.Interfaces;
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

        public async Task<string> CreateFundraising(string fundraisingUrl)
        {
            try
            {
                var model = new Fundraising()
                {
                    FundraisingUrl = fundraisingUrl
                };

                _dbContext.Fundraisings.Add(model);
                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return "Creating fundrsising failed!";
            }
        }

        public async Task<string> ChangeStatus(int fundraisingId)
        {
            try
            {
                var model = _dbContext.Fundraisings.FirstOrDefault(f => f.Id == fundraisingId);

                if (model == null)
                {
                    return "Fundraising was not found!";
                }

                model.IsClosed = !model.IsClosed;

                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return "Changing fundraising status failed!";
            }
        }
    }
}
