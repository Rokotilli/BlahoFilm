using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MessageBus.Enums;

namespace BusinessLogicLayer.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly UserServiceDbContext _dbContext;

        public HistoryService(UserServiceDbContext userServiceDbContext)
        {
            _dbContext = userServiceDbContext;
        }

        public async Task<string> AddHistoryForFilm(string userId, HistoryModel historyModel)
        {
            try
            {
                var media = _dbContext.MediaWithTypes
                    .FirstOrDefault(mwt => mwt.MediaId == historyModel.MediaWithType.MediaId && mwt.MediaTypeId == historyModel.MediaWithType.MediaTypeId);

                if (media == null)
                {
                    return "Media was not found!";
                }

                var existingHistory = _dbContext.Histories
                    .FirstOrDefault(h => h.UserId == userId && h.MediaWithTypeId == media.Id);

                if (existingHistory != null)
                {
                    existingHistory.TimeCode = historyModel.TimeCode;
                }
                else
                {
                    var model = new History()
                    {
                        UserId = userId,
                        MediaWithTypeId = media.Id,
                        TimeCode = historyModel.TimeCode
                    };

                    _dbContext.Histories.Add(model);
                }

                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return "Adding film history failed!";
            }
        }

        public async Task<string> AddHistoryForSeries(string userId, HistoryModel historyModel)
        {
            try
            {
                var media = _dbContext.MediaWithTypes
                    .FirstOrDefault(mwt => mwt.MediaId == historyModel.MediaWithType.MediaId && mwt.MediaTypeId == historyModel.MediaWithType.MediaTypeId);

                if (media == null)
                {
                    return "Media was not found!";
                }

                var existingHistory = _dbContext.Histories
                        .FirstOrDefault(h => h.UserId == userId && h.MediaWithTypeId == media.Id && h.PartNumber == historyModel.PartNumber && h.SeasonNumber == historyModel.SeasonNumber);

                if (existingHistory != null)
                {
                    existingHistory.TimeCode = historyModel.TimeCode;
                }
                else
                {
                    var model = new History()
                    {
                        UserId = userId,
                        MediaWithTypeId = media.Id,
                        PartNumber = historyModel.PartNumber,
                        SeasonNumber = historyModel.SeasonNumber,
                        TimeCode = historyModel.TimeCode
                    };

                    _dbContext.Histories.Add(model);
                }

                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return "Adding series history failed!";
            }            
        }
    }
}
