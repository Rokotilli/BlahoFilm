﻿using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly UserServiceDbContext _dbContext;

        public FavoritesService(UserServiceDbContext userServiceDbContext) 
        { 
            _dbContext = userServiceDbContext;
        }

        public async Task<string> Favorite(MediaWithType mediaWithType, string userid)
        {
            try
            {
                var media = _dbContext.MediaWithTypes.FirstOrDefault(mwt => mwt.MediaId == mediaWithType.MediaId && mwt.MediaTypeId == mediaWithType.MediaTypeId);                

                if (media == null)
                {
                    return "Media not found!";
                }                

                var existFavorite = _dbContext.Favorites.FirstOrDefault(f => f.UserId == userid && f.MediaWithTypeId == media.Id);

                if (existFavorite == null)
                {
                    var favorite = new Favorite()
                    {
                        UserId = userid,
                        MediaWithTypeId = media.Id
                    };

                    _dbContext.Favorites.Add(favorite);
                    await _dbContext.SaveChangesAsync();

                    return null;
                }

                _dbContext.Favorites.Remove(existFavorite);
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
