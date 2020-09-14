using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MyGames.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyGames.TestIntegration.Fixtures
{
    public class TestContext
    {
        public HttpClient Client { get; set; }
        private TestServer _server;

        public TestContext()
        {
            SetupClient();
        }

        private void SetupClient()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>()
                .UseSetting("ConnectionStrings:DefaultConnection", "Server=GGUEDESM10\\SQLEXPRESS;Database=mygames;Trusted_Connection=True;MultipleActiveResultSets=true"));

            Client = _server.CreateClient();
        }
    }
}
