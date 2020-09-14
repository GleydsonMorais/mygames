using FluentAssertions;
using MyGames.Object.Amigo;
using MyGames.TestIntegration.Fixtures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MyGames.TestIntegration.Scenarios
{
    public class AmigoTest
    {
        private readonly TestContext _testContext;

        public AmigoTest()
        {
            _testContext = new TestContext();
        }

        [Fact]
        public async Task Amigo_Get_ReturnsOkResponse()
        {
            var response = await _testContext.Client.GetAsync("api/amigo");
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Amigo_GetById_ReturnsNotFoundResponse()
        {
            var response = await _testContext.Client.GetAsync("api/amigo/0");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Amigo_GetById_ReturnsBadRequestResponse()
        {
            var response = await _testContext.Client.GetAsync("api/amigo/xx");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
