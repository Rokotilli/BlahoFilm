using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnimeServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterAnimeController : ControllerBase
    {
        private readonly IGetSaSService _getSaSService;
        private readonly IRegisterAnimeService _registerAnimeService;
        private readonly IUploadedAnimeService _uploadedAnimeService;
        private readonly IConfiguration _configuration;

        public RegisterAnimeController(
            IGetSaSService getSaSService,
            IConfiguration configuration,
            IRegisterAnimeService registerAnimeService,
            IUploadedAnimeService uploadedAnimeService)
        {
            _getSaSService = getSaSService;
            _configuration = configuration;
            _registerAnimeService = registerAnimeService;
            _uploadedAnimeService = uploadedAnimeService;
        }

        [HttpGet("getsas")]
        public async Task<IActionResult> GetSaS([FromQuery] string blobName)
        {
            var result = await _getSaSService.GetSaS(_configuration, _configuration["AzureStorageContainerName"], blobName);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Can't get a SaS");
        }

        [HttpPost("registeranime")]
        public async Task<IActionResult> RegisterAnime(AnimeRegisterModel animeRegisterModel)
        {
            var result = await _registerAnimeService.RegisterAnime(animeRegisterModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }
        [HttpPost("registeranimepart")]
        public async Task<IActionResult> RegisterAnimePart(AnimePartRegisterModel animePartRegisterModel)
        {
            var result = await _registerAnimeService.RegisterAnimePart(animePartRegisterModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }

        [HttpPost("uploadedanime")]
        public async Task<IActionResult> UploadedAnime(AnimeUploadedModel animeUploadedModel)
        {
            var result = await _uploadedAnimeService.UploadedAnime(animeUploadedModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }
        [HttpPost("uploadedanimepart")]
        public async Task<IActionResult> UploadedAnimePart(AnimePartUploadedModel animePartUploadedModel)
        {
            var result = await _uploadedAnimeService.UploadedAnimePart(animePartUploadedModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }
    }
}
