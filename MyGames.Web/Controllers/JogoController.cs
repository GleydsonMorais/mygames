using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyGames.Data.Constants;
using MyGames.Data.Helpers;
using MyGames.Web.Interfaces;
using MyGames.Web.Models;
using MyGames.Web.Models.Helper;
using MyGames.Web.Models.Jogo;

namespace MyGames.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class JogoController : Controller
    {
        private readonly IJogoService _jogoService;
        private readonly ILogger<JogoController> _logger;

        public JogoController(IJogoService jogoService,
            ILogger<JogoController> logger)
        {
            _jogoService = jogoService;
            _logger = logger;
        }

        public async Task<IActionResult> Index([FromQuery]JogoFilterViewModel filter)
        {
            ViewData["CurrentFilter"] = filter;

            var model = await _jogoService.ListJogoAsync(filter);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetJogoHistoricoAsync(int id)
        {
            var model = await _jogoService.GetJogoHistoricoAsync(id);

            if (null == model)
            {
                return NotFound();
            }

            return PartialView("_ListHistoricoEmprestimo", model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(JogoCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _jogoService.CreateJogoAsync(model);
                    if (result.Succeeded)
                    {
                        return Redirect(nameof(Index));
                    }
                    else
                    {
                        ViewBag.alerts = new AlertViewModel { Type = GeneralConstants.ERROR, Text = result.Message };
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _jogoService.GetJogoEditAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(JogoEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _jogoService.EditJogoAsync(model);
                    if (result.Succeeded)
                    {
                        ViewBag.alerts = new AlertViewModel { Type = GeneralConstants.SUCCESS, Text = result.Message };
                    }
                    else
                    {
                        ViewBag.alerts = new AlertViewModel { Type = GeneralConstants.ERROR, Text = result.Message };
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpPost]
        [ActionName("DevolverJogo")]
        public async Task<QueryResult<string>> DevolverJogoAsync(int id) => await _jogoService.DevolverJogoAsync(id);
    }
}