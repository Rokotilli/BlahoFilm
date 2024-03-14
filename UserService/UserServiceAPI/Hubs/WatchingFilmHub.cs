using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MessageBus.Enums;
using Microsoft.AspNetCore.SignalR;

namespace UserServiceAPI.Hubs
{
    public class WatchingFilmHub : Hub
    {
        private readonly UserServiceDbContext _dbContext;
        private readonly IHistoryService _historyService;
        private Dictionary<string, (int FilmId, TimeSpan TimeCode)> _userTimeCodes;

        public WatchingFilmHub(UserServiceDbContext userServiceDbContext, IHistoryService historyService)
        {
            _dbContext = userServiceDbContext;
            _historyService = historyService;
            _userTimeCodes = new Dictionary<string, (int FilmId, TimeSpan TimeCode)>();
        }

        public async Task History(int filmId, TimeSpan timeCode)
        {
            var mediaWithType = _dbContext.MediaWithTypes.FirstOrDefault(mwt => mwt.MediaId == filmId && mwt.MediaTypeId == (int)MediaTypes.Film);

            if (mediaWithType == null)
            {
                Context.Abort();
                await base.OnDisconnectedAsync(new Exception("Media was not found!"));
                return;
            }

            //UserId must be from jwt
            var userId = "user1";
            _userTimeCodes[userId] = (mediaWithType.Id, timeCode);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //UserId must be from jwt
            var userId = "user1";

            if (_userTimeCodes.TryGetValue(userId, out var timeCodeInfo))
            {
                await _historyService.AddHistory(userId, timeCodeInfo.FilmId, timeCodeInfo.TimeCode);

                _userTimeCodes.Remove(userId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
