using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TransactionServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService) 
        {
            _transactionService = transactionService;
        }

        [Authorize]
        [HttpPost("subscribe")]
        public async Task<IActionResult> AddSubscription(SubscriptionModel subscriptionModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _transactionService.AddSubscription(subscriptionModel, userId);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpPut("changestatus")]
        public async Task<IActionResult> ChangeStatus([FromQuery] string reason)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _transactionService.ChangeStatusSubscription(userId, reason);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
