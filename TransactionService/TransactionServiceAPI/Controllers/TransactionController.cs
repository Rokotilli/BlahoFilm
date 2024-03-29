using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("subscribe")]
        public async Task<IActionResult> AddSubscription(SubscriptionModel subscriptionModel)
        {
            //UserId must be from jwt
            var result = await _transactionService.AddSubscription(subscriptionModel, "user1");

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [HttpPut("changestatus")]
        public async Task<IActionResult> ChangeStatus([FromQuery] string reason)
        {
            //UserId must be from jwt
            var result = await _transactionService.ChangeStatusSubscription("user1", reason);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
