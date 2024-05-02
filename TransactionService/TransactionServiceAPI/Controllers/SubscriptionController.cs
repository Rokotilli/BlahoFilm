using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TransactionServiceAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly TransactionServiceDbContext _dbContext;
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService, TransactionServiceDbContext transactionServiceDbContext) 
        {
            _subscriptionService = subscriptionService;
            _dbContext = transactionServiceDbContext;
        }

        [HttpGet("subscriptions")]
        public async Task<IActionResult> GetSubscriptions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _dbContext.Subscriptions.Where(s => s.UserId == userId).ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }
        
        [HttpPost("subscribe")]
        public async Task<IActionResult> AddSubscription(SubscriptionModel subscriptionModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _subscriptionService.AddSubscription(subscriptionModel, userId);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [HttpPut("changestatus")]
        public async Task<IActionResult> ChangeStatus([FromQuery] string reason)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _subscriptionService.ChangeStatusSubscription(userId, reason);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
