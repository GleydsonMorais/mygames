using FluentAssertions;
using MyGames.API.Models.Jogo;
using MyGames.TestIntegration.Fixtures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyGames.TestIntegration.Scenarios
{
    public class JogoTest
    {
        private readonly TestContext _testContext;

        public JogoTest()
        {
            _testContext = new TestContext();
        }

        [Fact]
        public async Task Jogo_Get_ReturnsOkResponse()
        {
            var response = await _testContext.Client.GetAsync("api/jogo");
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Jogo_GetById_ReturnsNotFoundResponse()
        {
            var response = await _testContext.Client.GetAsync("api/jogo/0");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Jogo_GetById_ReturnsBadRequestResponse()
        {
            var response = await _testContext.Client.GetAsync("api/jogo/xx");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("Jogo - Test Integration", 1)]
        public async Task Jogo_Post_ReturnsCreatedResponse(string Nome, int TipoJogoId)
        {
            var jogo = new JogoCreate
            {
                Nome = Nome,
                TipoJogoId = TipoJogoId
            };

            var jsonJogo = JsonConvert.SerializeObject(jogo);
            HttpContent httpContent = new StringContent(jsonJogo, Encoding.UTF8, "application/json");

            var response = await _testContext.Client.PostAsync("api/jogo", httpContent);
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Theory]
        [InlineData("Jogo - Test Integration", 0)]
        public async Task Jogo_Post_ReturnsNotFoundResponse(string Nome, int TipoJogoId)
        {
            var jogo = new JogoCreate
            {
                Nome = Nome,
                TipoJogoId = TipoJogoId
            };

            var jsonJogo = JsonConvert.SerializeObject(jogo);
            HttpContent httpContent = new StringContent(jsonJogo, Encoding.UTF8, "application/json");

            var response = await _testContext.Client.PostAsync("api/jogo", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Tipo do Jogo não esta cadastrado!", result);
        }
    }
}
