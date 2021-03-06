﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ApiGateway.Controllers
{

    [Route("api/[controller]")]
    [AllowAnonymous]
    public class HealthController : Controller
    {
        private IConfiguration _configuration;

        public HealthController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("Index")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            Console.WriteLine($" This is HealthController  {this._configuration["Service:Port"]} Invoke");

            return Ok();
        }
    }
}
