using System.Collections.Generic;

namespace Api.Hosting.Consul 
{
    public class AgentRegistry 
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public IDictionary<string, string> Meta { get; set; }
        public bool EnableTagOverride { get; set; }
        public Check Check { get; set; }
    }

    public class Check
    {
        public string DeregisterCriticalServiceAfter { get; set; }
        public string HTTP { get; set; }
        public string Interval { get; set; }
    }
}