using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;

namespace BusinessLogicLayer.Services
{
    public class UploadedCartoonService : IUploadedCartoonService
    {
        private readonly CartoonServiceDbContext _dbContext;

        public UploadedCartoonService(CartoonServiceDbContext cartoonServiceDbContext)
        {
            _dbContext = cartoonServiceDbContext;
        }

        public async Task<string> UploadedCartoon(CartoonUploadedModel cartoonUploadedModel)
        {
            try
            {
                var model = _dbContext.Cartoons
                .Where(c => c.Id == cartoonUploadedModel.Id)
                .ToArray().First();

                model.FileUri = cartoonUploadedModel.FileUri;

                _dbContext.Cartoons.Update(model);
                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> UploadedCartoonPart(CartoonPartUploadedModel cartoonPartUploadedModel)
        {
            try
            {
                var model = _dbContext.CartoonParts
                .Where(c => c.Id == cartoonPartUploadedModel.Id)
                .ToArray().First();

                model.FileUri = cartoonPartUploadedModel.FileUri;

                _dbContext.CartoonParts.Update(model);
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
