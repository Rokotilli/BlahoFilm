using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class UploadedVoiceoverService : IUploadedVoiceoverService
    {
        private readonly FilmServiceDbContext _dbContext;

        public UploadedVoiceoverService(FilmServiceDbContext filmServiceDbContext)
        {
            _dbContext = filmServiceDbContext;
        }

        public async Task<string> UploadedVoiceover(VoiceoversFilm uploadedVoiceover)
        {
            try
            {
                var model = await _dbContext.VoiceoversFilms.FirstOrDefaultAsync(vf => vf.FilmId == uploadedVoiceover.FilmId && vf.VoiceoverId == uploadedVoiceover.VoiceoverId);

                if (model == null)
                {
                    return "Voiceover for film not found!";
                }

                model.FileUri = uploadedVoiceover.FileUri;

                _dbContext.VoiceoversFilms.Update(model);
                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return "Adding voiceover failed!";
            }
        }
    }
}
