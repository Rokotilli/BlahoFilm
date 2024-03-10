using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;

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
                var model = _dbContext.Films
                .Where(f => f.Id == filmUploadedModel.Id)
                .ToArray().First();

                model.FileUri = filmUploadedModel.FileUri;

                _dbContext.Films.Update(model);
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
