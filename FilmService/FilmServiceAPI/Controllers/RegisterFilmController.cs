using Azure.Storage.Sas;
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

        public RegisterFilmController(
            IGetSaSService getSaSService,
            IFilmService filmService,
            IUploadedFilmService uploadedFilmService)
        {
            _getSaSService = getSaSService;
            _filmService = filmService;
            _uploadedFilmService = uploadedFilmService;
        }

        [HttpGet("getsas")]
        public async Task<IActionResult> GetSaS([FromQuery] string blobName)
        {
            var result = await _getSaSService.GetSaS(blobName, BlobSasPermissions.Write);

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
        public async Task<IActionResult> UploadedVoiceover(FilmUploadedModel uploadedModel)
        {
            var result = await _uploadedFilmService.UploadedFilm(uploadedModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }

        [HttpPost("createselection")]
        public async Task<IActionResult> CreateSelection(SelectionAddModel selectionAddModel)
        {
            var result = await _filmService.CreateSelection(selectionAddModel);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
