using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace SeriesServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterSeriesController : ControllerBase
    {
        private readonly IGetSaSService _getSaSService;
        private readonly IRegisterSeriesService _registerSeriesService;
        private readonly IUploadedSeriesPartService _uploadedSeriesService;
        private readonly IConfiguration _configuration;

        public RegisterSeriesController(
            IGetSaSService getSaSService,
            IConfiguration configuration,
            IRegisterSeriesService registerSeriesService,
            IUploadedSeriesPartService uploadedSeriesService)
        {
            _getSaSService = getSaSService;
            _configuration = configuration;
            _registerSeriesService = registerSeriesService;
            _uploadedSeriesService = uploadedSeriesService;
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

        [HttpPost("registerseries")]
        public async Task<IActionResult> RegisterSeries(SeriesRegisterModel seriesRegisterModel)
        {
            var result = await _registerSeriesService.RegisterSeries(seriesRegisterModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }
        [HttpPost("registerseriespart")]
        public async Task<IActionResult> RegisterSeriesPart(SeriesPartRegisterModel seriesPartRegisterModel)
        {
            var result = await _registerSeriesService.RegisterSeriesPart(seriesPartRegisterModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }
        [HttpPost("uploadedseriespart")]
        public async Task<IActionResult> UploadedSeriesPart(SeriesPartUploadedModel seriesPartUploadedModel)
        {
            var result = await _uploadedSeriesService.UploadedSeriesPart(seriesPartUploadedModel);

            if (result == null)
            {
                return Ok();
            }

            return BadRequest(result);
        }
    }
}
