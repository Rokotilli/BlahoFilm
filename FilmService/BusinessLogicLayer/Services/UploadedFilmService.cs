using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class UploadedFilmService : IUploadedFilmService
    {
        private readonly FilmServiceDbContext _dbContext;

        public UploadedFilmService(FilmServiceDbContext filmServiceDbContext)
        {
            _dbContext = filmServiceDbContext;
        }

        public async Task<string> UploadedFilm(FilmUploadedModel filmUploadedModel)
        {
            try
            {
                var model = await _dbContext.Films.FirstOrDefaultAsync(f => f.Id == filmUploadedModel.Id);

                if (model == null)
                {
                    return "Film not found!";
                }

                model.FileName = filmUploadedModel.FileName;
                model.FileUri = filmUploadedModel.FileUri;

                _dbContext.Films.Update(model);
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
