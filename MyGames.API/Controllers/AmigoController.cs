using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGames.API.Interfaces;
using MyGames.API.Models.Amigo;
using MyGames.Data.Helpers;
using MyGames.Object.Amigo;

namespace MyGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmigoController : ControllerBase
    {
        private readonly IAmigoService _amigoService;

        public AmigoController(IAmigoService amigoService)
        {
            _amigoService = amigoService;
        }

        // GET: api/Amigo
        [HttpGet]
        public async Task<IList<Amigo>> Get() => await _amigoService.GetListAmigoAsync();

        // GET: api/Amigo/5
        [HttpGet("{id}")]
        public async Task<Amigo> Get(int id) => await _amigoService.GetAmigoAsync(id);

        // POST: api/Amigo
        [HttpPost]
        public async Task<QueryResult<string>> Post([FromBody]AmigoCreate model) => await _amigoService.CreateAmigoAsync(model);

        // PUT: api/Amigo/5
        [HttpPut("{id}")]
        public async Task<QueryResult<string>> Put(int id, [FromBody]AmigoEdit model) => await _amigoService.EditAmigoAsync(id, model);

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<QueryResult<string>> Delete(int id) => await _amigoService.DeleteAmigoAsync(id);
    }
}
