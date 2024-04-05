﻿using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using MassTransit;
using MassTransit.Initializers;
using MessageBus.Enums;
using MessageBus.Messages;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BusinessLogicLayer.Services
{
    public class RegisterAnimeService : IRegisterAnimeService
    {
        private readonly AnimeServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegisterAnimeService(AnimeServiceDbContext animeServiceDbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = animeServiceDbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<string> RegisterAnime(AnimeRegisterModel animeRegisterModel)
        {
            try
            {
                byte[] posterBytes = null;
                var genres = animeRegisterModel.Genres.Split(",");
                var tags = animeRegisterModel.Genres.Split(",");

                using (var stream = new MemoryStream())
                {
                    await animeRegisterModel.Poster.CopyToAsync(stream);
                    posterBytes = stream.ToArray();
                }

                var model = new Anime()
                {
                    Poster = posterBytes,
                    Title = animeRegisterModel.Title,
                    Description = animeRegisterModel.Description,
                    CountSeasons = animeRegisterModel.CountSeasons,
                    CountParts = animeRegisterModel.CountParts,
                    Year = animeRegisterModel.Year,
                    Director = animeRegisterModel.Director,
                    Rating = animeRegisterModel.Rating,
                    StudioName = animeRegisterModel.StudioName,
                    TrailerUri = animeRegisterModel.TrailerUri,
                    AgeRestriction = animeRegisterModel.AgeRestriction
                };

                var anime = await _dbContext.Animes
                    .FirstOrDefaultAsync(a =>
                                    a.Title == model.Title &&
                    a.Description == model.Description &&
                    a.CountSeasons == model.CountSeasons &&
                    a.CountParts == model.CountParts &&
                    a.Year == model.Year &&
                    a.Director == model.Director &&
                    a.Rating == model.Rating &&
                    a.StudioName == model.StudioName &&
                    a.TrailerUri == model.TrailerUri &&
                    a.AgeRestriction == model.AgeRestriction);
                if (anime != null)
                {
                    return "This anime already exists!";
                }

                var existingGenres = _dbContext.Genres
                    .Select(g => g.Name)
                    .ToArray();

                var existingTags = _dbContext.Tags
                    .Select(t => t.Name)
                    .ToArray();

                var missingGenres = genres
                    .Except(existingGenres)
                    .ToArray();

                var missingTags = tags
                    .Except(existingTags)
                    .ToArray();

                foreach (var item in missingGenres)
                {
                    var newGenre = new Genre { Name = item };
                    _dbContext.Genres.Add(newGenre);
                }

                foreach (var item in missingTags)
                {
                    var newTag = new Tag { Name = item };
                    _dbContext.Tags.Add(newTag);
                }

                _dbContext.Animes.Add(model);

                await _dbContext.SaveChangesAsync();

                var animeid = _dbContext.Animes
                    .Where(a =>
                                                  a.Title == model.Title &&
                    a.Description == model.Description &&
                    a.CountSeasons == model.CountSeasons &&
                    a.CountParts == model.CountParts &&
                    a.Year == model.Year &&
                    a.Director == model.Director &&
                    a.Rating == model.Rating &&
                    a.StudioName == model.StudioName &&
                    a.TrailerUri == model.TrailerUri &&
                    a.AgeRestriction == model.AgeRestriction)
                    .Select(s => s.Id)
                    .First();

                foreach (var item in genres)
                {
                    var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == item);

                    _dbContext.GenresAnimes.Add(new GenresAnime() { AnimeId = animeid, GenreId = genre.Id });
                }

                foreach (var item in tags)
                {
                    var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == item);
                    _dbContext.TagsAnimes.Add(new TagsAnime() { AnimeId = animeid, TagId = tag.Id });
                }

                await _dbContext.SaveChangesAsync();

                await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = animeid, MediaType = MediaTypes.Anime });

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> RegisterAnimePart(AnimePartRegisterModel animePartRegisterModel)
        {
            try
            {
                var model = new AnimePart()
                {
                    AnimeId = animePartRegisterModel.AnimeId,
                    SeasonNumber = animePartRegisterModel.SeasonNumber,
                    PartNumber = animePartRegisterModel.PartNumber,
                    Duration = animePartRegisterModel.Duration
                };

                var animePart = await _dbContext.AnimeParts
                    .FirstOrDefaultAsync(cp =>
                       cp.AnimeId == model.AnimeId &&
                    cp.SeasonNumber == model.SeasonNumber &&
                    cp.PartNumber == model.PartNumber &&
                    cp.Duration == model.Duration
                                    );
                if (animePart != null)
                {
                    return "This anime already exists!";
                }



                _dbContext.AnimeParts.Add(model);

                await _dbContext.SaveChangesAsync();

                var animeid = _dbContext.AnimeParts
                    .Where(cp =>
                       cp.AnimeId == model.AnimeId &&
                    cp.SeasonNumber == model.SeasonNumber &&
                    cp.PartNumber == model.PartNumber &&
                    cp.Duration == model.Duration
                                  )
                    .Select(cp => cp.Id)
                    .First();
                await _dbContext.SaveChangesAsync();

                await _publishEndpoint.Publish(new MediaRegisteredMessage() { Id = animeid, MediaType = MediaTypes.Anime });

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
