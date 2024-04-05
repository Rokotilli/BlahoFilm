using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class UploadedAnimeService : IUploadedAnimeService
    {
        private readonly AnimeServiceDbContext _dbContext;

        public UploadedAnimeService(AnimeServiceDbContext animeServiceDbContext)
        {
            _dbContext = animeServiceDbContext;
        }

        public async Task<string> UploadedAnime(AnimeUploadedModel animeUploadedModel)
        {
            try
            {
                var model = await _dbContext.Animes.FirstOrDefaultAsync(a => a.Id == animeUploadedModel.Id);

                model.FileName = animeUploadedModel.FileName;
                model.FileUri = animeUploadedModel.FileUri; 

                _dbContext.Animes.Update(model);
                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UploadedAnimePart(AnimePartUploadedModel animePartUploadedModel)
        {
            try
            {
                var model = _dbContext.AnimeParts
                .Where(a => a.Id == animePartUploadedModel.Id)
                .ToArray().First();
                model.FileName = animePartUploadedModel.FileName;
                model.FileUri = animePartUploadedModel.FileUri;

                _dbContext.AnimeParts.Update(model);
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
