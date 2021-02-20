using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CovidApp.Entity.DTOs;
using CovidApp.Entity.Models;
using CovidApp.Entity.Parameters;
using CovidApp.Persistence;
using CovidApp.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CovidApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICovidService _covidService;

        public HomeController(ILogger<HomeController> logger, ICovidService covidService)
        {
            _logger = logger;
            _covidService = covidService;
        }
        private async Task<List<CovidDTO>> GetCovidDataAsync(SearchModelParameters searchModelParameters)
        {
            return await _covidService.GetCovidDataAsync(searchModelParameters);
        }

        [HttpGet]
        public IActionResult Index(CovidModelDTO covidModel)
        {
            return View(covidModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SearchModelParameters searchModelParameters)
        {
            var covidData = await GetCovidDataAsync(searchModelParameters);

            var covidModel = new CovidModelDTO();
            covidModel.CovidData = covidData;
            covidModel.SearchModelParameters = searchModelParameters;

            return View(covidModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
