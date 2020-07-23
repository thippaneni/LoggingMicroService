using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Logging.Api.Infrastructure;
using Logging.Api.Models;
using Logging.Api.Services;

namespace Logging.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ActionLogger]
    public class LogController : ControllerBase
    {
        private readonly ILoggingService _loggerService;
        private IHttpContextAccessor _accessor;
        public LogController(ILoggingService loggerService, IHttpContextAccessor accessor)
        {
            _loggerService = loggerService;
            _accessor = accessor;
        }
        // GET: api/Log
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var healthy = await _loggerService.HealthAsync();
            return StatusCode(StatusCodes.Status200OK, healthy);
        }
                
        // POST: api/Log
        [HttpPost]        
        public async Task<IActionResult> Post([FromBody] LoggingModel logModel)
        {
            var response = await _loggerService.LogAsync(logModel);
            return StatusCode(StatusCodes.Status200OK, response);
        }        
    }
}
