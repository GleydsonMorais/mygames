using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyGames.Data.Constants;
using MyGames.Web.Interfaces;
using MyGames.Web.Models;
using MyGames.Web.Models.Amigo;
using MyGames.Web.Models.Helper;

namespace MyGames.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AmigoController : Controller
    {
        private readonly IAmigoService _amigoService;
        private readonly ILogger<AmigoController> _logger;

        public AmigoController(IAmigoService amigoService,
            ILogger<AmigoController> logger)
        {
            _amigoService = amigoService;
            _logger = logger;
        }

        public async Task<IActionResult> Index([FromQuery]AmigoFilterViewModel filter)
        {
            ViewData["CurrentFilter"] = filter;

            var model = await _amigoService.ListAmigoAsync(filter);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetListJogosEmprestadosAsync(int id)
        {
            var model = await _amigoService.GetListJogosEmprestadosAsync(id);
            if (null == model)
            {
                return NotFound();
            }
            return PartialView("_ListJogosEmprestados", model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AmigoCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _amigoService.CreateAmigoAsync(model);
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
            var model = await _amigoService.GetAmigoEditAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AmigoEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _amigoService.EditAmigoAsync(model);
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

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _amigoService.GetAmigoDeleteAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AmigoDeleteViewModel model)
        {
            try
            {
                var result = await _amigoService.DeleteAmigoAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.alerts = new AlertViewModel { Type = GeneralConstants.ERROR, Text = result.Message };

                    model = await _amigoService.GetAmigoDeleteAsync(model.Id);
                    return View(model);
                }                
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}