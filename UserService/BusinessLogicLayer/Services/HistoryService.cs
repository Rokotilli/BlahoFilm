using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly UserServiceDbContext _dbContext;

        public HistoryService(UserServiceDbContext userServiceDbContext)
        {
            _dbContext = userServiceDbContext;
        }

        public async Task AddHistory(string userId, int FilmId, TimeSpan TimeCode)
        {
            var existingHistory = _dbContext.Histories
                .FirstOrDefault(h => h.UserId == userId && h.MediaWithTypeId == FilmId);

            if (existingHistory != null)
            {
                existingHistory.TimeCode = TimeCode;
            }
            else
            {
                var model = new History()
                {
                    UserId = userId,
                    MediaWithTypeId = FilmId,
                    TimeCode = TimeCode
                };

                _dbContext.Histories.Add(model);
            }

            await _dbContext.SaveChangesAsync();

        }
    }
}
