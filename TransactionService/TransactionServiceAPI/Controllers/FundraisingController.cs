﻿using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TransactionServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundraisingController : ControllerBase
    {
        private readonly TransactionServiceDbContext _dbContext;
        private readonly IFundraisingService _fundraisingService;

        public FundraisingController(TransactionServiceDbContext transactionServiceDbContext, IFundraisingService fundraisingService) 
        {
            _dbContext = transactionServiceDbContext;
            _fundraisingService = fundraisingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaggedFundraisings([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var model = _dbContext.Fundraisings
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArray();

            if (!model.Any())
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("countpages")]
        public async Task<IActionResult> GetCountPaggesFundraisings([FromQuery] int pageSize)
        {
            var count = _dbContext.Fundraisings.Count();

            if (count == 0)
            {
                return NotFound();
            }

            var pages = Math.Ceiling((double)count / pageSize);

            return Ok(pages);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> CreateFundraising(FundraisingModel fundraisingModel)
        {
            var result = await _fundraisingService.CreateFundraising(fundraisingModel);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut]
        public async Task<IActionResult> ChangeStatusFundraising([FromQuery] int fundraisingId)
        {
            var result = await _fundraisingService.ChangeStatus(fundraisingId);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}
