using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyGames.API.Interfaces;
using MyGames.API.Models.Jogo;
using MyGames.Object.Jogo;

namespace MyGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogoController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogoController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        // GET: api/Jogo
        [HttpGet]
        public async Task<ActionResult<IList<JogoResult>>> Get()
        {
            var lsitJogo = await _jogoService.GetListJogoAsync();

            if (lsitJogo == null)
            {
                return NotFound();
            }

            return Ok(lsitJogo);
        }

        // GET: api/Jogo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JogoResult>> Get(int id)
        {
            var lsitJogo = await _jogoService.GetJogoAsync(id);

            if (lsitJogo == null)
            {
                return NotFound();
            }

            return Ok(lsitJogo);
        }

        // POST: api/Jogo
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody]JogoCreate model)
        {
            var result = await _jogoService.CreateJogoAsync(model);
            if (!result.Succeeded)
            {
                return NotFound(result.Message);
            }

            return StatusCode(201);
        }

        // PUT: api/Jogo/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, string nome)
        {
            var result = await _jogoService.UpdateJogoAsync(id, nome);
            if (!result.Succeeded)
            {
                return NotFound(result.Message);
            }

            return NoContent();
        }

        //PUT api/Jogo/Devolver/5
        [HttpPut("Devolver/{id}")]
        public async Task<ActionResult> Devolver(int id)
        {
            var result = await _jogoService.DevolverJogoAsync(id);
            if (!result.Succeeded)
            {
                return NotFound(result.Message);
            }

            return NoContent();
        }

        //PUT api/Jogo/Emprestar/5
        [HttpPut("Emprestar/{id}/{amigoId}")]
        public async Task<ActionResult> Devolver(int id, int amigoId)
        {
            var result = await _jogoService.EmprestarJogoAsync(id, amigoId);
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
            var result = await _jogoService.DeleteJogoAsync(id);
            if (!result.Succeeded)
            {
                return NotFound(result.Message);
            }

            return NoContent();
        }
    }
}
