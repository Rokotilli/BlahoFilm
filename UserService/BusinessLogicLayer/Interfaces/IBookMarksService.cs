using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
    public interface IBookMarksService
    {
        Task<string> BookMark(MediaWithType mediaWithType, string userid);
    }
}
