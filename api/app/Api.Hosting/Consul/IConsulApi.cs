using NetCoreStack.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Hosting.Consul
{
    /// <summary>
    /// Consul API Interface [https://www.consul.io/]
    /// </summary>
    [ApiRoute("v1", regionKey: "Consul")]
    public interface IConsulApi : IApiContract
    {
        [HttpPutMarker(Template = "/agent/service/register")]
        Task RegisterAsync(AgentRegistry registry);

        [HttpPutMarker(Template = "kv/{key}")]
        Task<bool> CreateOrUpdateKey(string key, string body);
    }
}