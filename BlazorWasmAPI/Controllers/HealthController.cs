﻿using Microsoft.AspNetCore.Mvc;

namespace BlazorWasmAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok(new {status = "ok"});
        }
    }
}
