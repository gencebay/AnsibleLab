using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetCoreStack.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Hosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscoveryController : ControllerBase
    {
        private readonly IOptions<ProxyOptions> _options;
        private readonly IHostingEnvironment _hosting;
        public DiscoveryController(IOptions<ProxyOptions> options, IHostingEnvironment hosting)
        {
            _options = options;
            _hosting = hosting;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var host = new 
            {
                MachineName = Environment.MachineName,
                DateTime = DateTime.Now.ToString(),
                Options = _options,
                IpAddress = Startup.GetLocalIPAddress(),
                EnvironmentName = _hosting.EnvironmentName
            };

            return new JsonResult(host);
        }

        [HttpGet(nameof(Regions))]
        public IActionResult Regions()
        {
            var roundRobinManager = HttpContext.RequestServices.GetService<RoundRobinManager>();
            return new JsonResult(roundRobinManager.ProxyRegionDict);
        }
    }
}
