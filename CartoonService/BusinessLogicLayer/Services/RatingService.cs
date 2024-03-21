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
                var model = _dbContext.Rating.FirstOrDefault(r => r.CartoonId == cartoonId && r.UserId == userid);
                var cartoon = _dbContext.Cartoons.FirstOrDefault(f => f.Id == cartoonId);

                if (model == null)
                {
                    var rating = new Rating()
                    {
                        CartoonId = cartoonId,
                        UserId = userid,
                        Rate = rate
                    };

                    _dbContext.Rating.Add(rating);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    model.Rate = rate;

                    _dbContext.Rating.Update(model);
                    await _dbContext.SaveChangesAsync();
                }

                var averageRating = _dbContext.Rating
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


        public async Task<string> RateCartoonPart(int cartoonPartId, int rate, string userid)
        {
            try
            {
                var model = _dbContext.Rating.FirstOrDefault(r => r.CartoonId == cartoonPartId && r.UserId == userid);
                var cartoonPart = _dbContext.Cartoons.FirstOrDefault(f => f.Id == cartoonPartId);

                if (model == null)
                {
                    var rating = new Rating()
                    {
                        CartoonId = cartoonPartId,
                        UserId = userid,
                        Rate = rate
                    };

                    _dbContext.Rating.Add(rating);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    model.Rate = rate;

                    _dbContext.Rating.Update(model);
                    await _dbContext.SaveChangesAsync();
                }

                var averageRating = _dbContext.Rating
                    .Where(r => r.CartoonId == cartoonPartId)
                    .Average(r => r.Rate);

                var result = Math.Round(averageRating, 1);

                cartoonPart.Rating = result;

                _dbContext.Cartoons.Update(cartoonPart);
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
