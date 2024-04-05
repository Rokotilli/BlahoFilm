using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class RatingService : IRatingService
    {
        private readonly CartoonServiceDbContext _dbContext;

        public RatingService(CartoonServiceDbContext cartoonServiceDbContext)
        {
            _dbContext = cartoonServiceDbContext;
        }

        public async Task<string> RateCartoon(int cartoonId, int rate, string userid)
        {
            try
            {
                var model = _dbContext.CartoonRating.FirstOrDefault(r => r.CartoonId == cartoonId && r.UserId == userid);
                var cartoon = _dbContext.Cartoons.FirstOrDefault(f => f.Id == cartoonId);

                if (model == null)
                {
                    var rating = new CartoonRating()
                    {
                        CartoonId = cartoonId,
                        UserId = userid,
                        Rate = rate
                    };

                    _dbContext.CartoonRating.Add(rating);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    model.Rate = rate;

                    _dbContext.CartoonRating.Update(model);
                    await _dbContext.SaveChangesAsync();
                }

                var averageRating = _dbContext.CartoonRating
                    .Where(r => r.CartoonId == cartoonId)
                    .Average(r => r.Rate);

                var result = Math.Round(averageRating, 1);

                cartoon.Rating = result;

                _dbContext.Cartoons.Update(cartoon);
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
