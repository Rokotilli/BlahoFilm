using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

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
                var model = await _dbContext.Cartoons.FirstOrDefaultAsync(c => c.Id == cartoonUploadedModel.Id);

                model.FileName = cartoonUploadedModel.FileName;
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
                model.FileName = cartoonPartUploadedModel.FileName;
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
