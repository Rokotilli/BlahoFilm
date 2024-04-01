using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class BookMarksService : IBookMarksService
    {
        private readonly UserServiceDbContext _dbContext;

        public BookMarksService(UserServiceDbContext userServiceDbContext)
        {
            _dbContext = userServiceDbContext;
        }

        public async Task<string> BookMark(MediaWithType mediaWithType, string userid)
        {
            try
            {
                var media = _dbContext.MediaWithTypes.FirstOrDefault(mwt => mwt.MediaId == mediaWithType.MediaId && mwt.MediaTypeId == mediaWithType.MediaTypeId);

                if (media == null)
                {
                    return "Media not found!";
                }

                var existBookMark = _dbContext.BookMarks.FirstOrDefault(f => f.UserId == userid && f.MediaWithTypeId == media.Id);

                if (existBookMark == null)
                {
                    var bookMark = new BookMark()
                    {
                        UserId = userid,
                        MediaWithTypeId = media.Id
                    };

                    _dbContext.BookMarks.Add(bookMark);
                    await _dbContext.SaveChangesAsync();

                    return null;
                }

                _dbContext.BookMarks.Remove(existBookMark);
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
