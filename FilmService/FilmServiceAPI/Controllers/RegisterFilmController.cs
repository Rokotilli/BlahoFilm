using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterFilmController : ControllerBase
    {
        private readonly IGetSaSService _getSaSService;
        private readonly IRegisterFilmService _registerFilmService;
        private readonly IUploadedFilmService _uploadedFilmService;
        private readonly IConfiguration _configuration;

        public RegisterFilmController(
            IGetSaSService getSaSService,
            IConfiguration configuration,
            IRegisterFilmService registerFilmService,
            IUploadedFilmService uploadedFilmService)
        {
            _getSaSService = getSaSService;
            _configuration = configuration;
            _registerFilmService = registerFilmService;
            _uploadedFilmService = uploadedFilmService;
        }

        [Authorize(Roles = "ADMIN")]
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

        [Authorize(Roles = "ADMIN")]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterFilm(FilmRegisterModel filmRegisterModel)
        {
            var result = await _registerFilmService.RegisterFilm(filmRegisterModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }

        [Authorize(Roles = "ADMIN")]
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
