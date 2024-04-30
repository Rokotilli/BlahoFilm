using Azure.Storage.Sas;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Entities;
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
        private readonly IUploadedVoiceoverService _uploadedVoiceoverService;

        public RegisterFilmController(
            IGetSaSService getSaSService,
            IFilmService filmService,
            IUploadedVoiceoverService uploadedFilmService)
        {
            _getSaSService = getSaSService;
            _filmService = filmService;
            _uploadedVoiceoverService = uploadedFilmService;
        }

        [HttpGet("getsas")]
        public async Task<IActionResult> GetSaS([FromQuery] string blobName)
        {
            var result = await _getSaSService.GetSaS(blobName, BlobSasPermissions.Write);

            if (result != null)
            {
                return Ok();
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

        [HttpPost("uploadedvoiceover")]
        public async Task<IActionResult> UploadedVoiceover(VoiceoversFilm uploadedVoiceover)
        {
            var result = await _uploadedVoiceoverService.UploadedVoiceover(uploadedVoiceover);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }
    }
}
