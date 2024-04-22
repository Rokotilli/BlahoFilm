using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmServiceAPI.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterFilmController : ControllerBase
    {
        private readonly IGetSaSService _getSaSService;
        private readonly IFilmService _filmService;
        private readonly IUploadedFilmService _uploadedFilmService;
        private readonly IConfiguration _configuration;

        public RegisterFilmController(
            IGetSaSService getSaSService,
            IConfiguration configuration,
            IFilmService filmService,
            IUploadedFilmService uploadedFilmService)
        {
            _getSaSService = getSaSService;
            _configuration = configuration;
            _filmService = filmService;
            _uploadedFilmService = uploadedFilmService;
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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterFilm(FilmRegisterModel filmRegisterModel)
        {
            var result = await _filmService.RegisterFilm(filmRegisterModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }

        [HttpPost("uploadedfilm")]
        public async Task<IActionResult> UploadedFilm(FilmUploadedModel filmUploadedModel)
        {
            var result = await _uploadedFilmService.UploadedFilm(filmUploadedModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }
    }
}
