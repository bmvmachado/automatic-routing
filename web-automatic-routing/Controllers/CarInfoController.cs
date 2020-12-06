using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_automatic_routing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarInfoController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "BMW", "Mercedes", "Audi", "Ford", "Toyota", "Tesla", "Peageout", "Hyunday", "Honda"
        };

        private readonly ILogger<CarInfoController> _logger;

        public CarInfoController(ILogger<CarInfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{numberOfItems}")]
        public IEnumerable<WeatherForecast> Get(int numberOfItems)
        {
            var rng = new Random();
            return Enumerable.Range(1, numberOfItems).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
