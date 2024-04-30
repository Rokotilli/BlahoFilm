using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUploadedVoiceoverService
    {
        Task<string> UploadedVoiceover(VoiceoversFilm uploadedVoiceover);
    }
}
