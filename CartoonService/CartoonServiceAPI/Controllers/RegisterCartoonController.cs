using Azure.Storage.Sas;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartoonServiceAPI.Controllers
{
    //[Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterCartoonController : ControllerBase
    {
        private readonly IGetSaSService _getSaSService;
        private readonly IRegisterCartoonService _registerCartoonService;
        private readonly IUploadedCartoonService _uploadedCartoonService;
        private readonly IConfiguration _configuration;

        public RegisterCartoonController(
            IGetSaSService getSaSService,
            IConfiguration configuration,
            IRegisterCartoonService registerCartoonService,
            IUploadedCartoonService uploadedCartoonService)
        {
            _getSaSService = getSaSService;
            _configuration = configuration;
            _registerCartoonService = registerCartoonService;
            _uploadedCartoonService = uploadedCartoonService;
        }

        [HttpGet("getsas")]
        public async Task<IActionResult> GetSaS([FromQuery] string blobName)
        {
            var result = await _getSaSService.GetSaS( blobName, BlobSasPermissions.Write);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Can't get c SaS");
        }

        [HttpPost("registercartoon")]
        public async Task<IActionResult> RegisterCartoon(CartoonRegisterModel cartoonRegisterModel)
        {
            var result = await _registerCartoonService.RegisterCartoon(cartoonRegisterModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }
        [HttpPost("registercartoonpart")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RegisterCartoonPart(CartoonPartRegisterModel cartoonPartRegisterModel)
        {
            var result = await _registerCartoonService.RegisterCartoonPart(cartoonPartRegisterModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }

        [HttpPost("uploadedcartoon")]
        public async Task<IActionResult> UploadedCartoon(CartoonUploadedModel cartoonUploadedModel)
        {
            var result = await _uploadedCartoonService.UploadedCartoon(cartoonUploadedModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }
        [HttpPost("uploadedcartoonpart")]
        public async Task<IActionResult> UploadedCartoonPart(CartoonPartUploadedModel cartoonPartUploadedModel)
        {
            var result = await _uploadedCartoonService.UploadedCartoonPart(cartoonPartUploadedModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }
    }
}
