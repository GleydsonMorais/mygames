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
        public async Task<ActionResult<IList<AmigoResult>>> Get()
        {
            var lsitAmigo = await _amigoService.GetListAmigoAsync();

            if (lsitAmigo == null)
            {
                return NotFound();
            }

            return Ok(lsitAmigo);
        }

        // GET: api/Amigo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AmigoResult>> Get(int id)
        {
            var amigo = await _amigoService.GetAmigoAsync(id);

            if (amigo == null)
            {
                return NotFound();
            }

            return Ok(amigo);
        }

        // POST: api/Amigo
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody]AmigoCreate model)
        {
            var result = await _amigoService.CreateAmigoAsync(model);
            if (!result.Succeeded)
            {
                return NotFound(result.Message);
            }

            return StatusCode(201);
        }

        // PUT: api/Amigo/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]AmigoEdit model)
        {
            var result = await _amigoService.UpdateAmigoAsync(id, model);
            if (!result.Succeeded)
            {
                return NotFound(result.Message);
            }

            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _amigoService.DeleteAmigoAsync(id);
            if (!result.Succeeded)
            {
                return NotFound(result.Message);
            }

            return NoContent();
        }
    }
}
