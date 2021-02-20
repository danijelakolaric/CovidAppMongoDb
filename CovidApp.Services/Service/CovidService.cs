using CovidApp.Entity.DTOs;
using CovidApp.Entity.Parameters;
using CovidApp.Persistence.ModelDb;
using CovidApp.Services.Interface;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CovidApp.Services.Service
{
    public class CovidService : ICovidService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IDatabaseService _databaseService;
        private const string uriDayOneTotal = "total/dayone/country/croatia";
        private const string uriByCountryTotal = "/country/croatia";

        public CovidService(IHttpClientService httpClientService, IDatabaseService databaseService)
        {
            _httpClientService = httpClientService;
            _databaseService = databaseService;
        }

        public async Task<List<CovidDTO>> GetCovidDataAsync(SearchModelParameters searchModelParameters)
        {
            var saved = await SaveCovidDataToDb();

            var covidData = await _databaseService.GetCovidDataAsync();

            covidData = covidData.Where(d => FilterDataBySearchModelParameters(d, searchModelParameters)).OrderByDescending(d => d.Date).ToList();

            return covidData;
        }

        public async Task<bool> SaveCovidDataToDb()
        {
            var covidParameters = new CovidParameters();
            var haveToSaveData = false;

            var dbIsEmpty = await _databaseService.CheckIfDbIsEmpty();

            if (dbIsEmpty)
            {
                covidParameters.Uri = uriDayOneTotal;
                haveToSaveData = true;
            }
            else
            {
                var lastSavedDate = await _databaseService.GetLastDate();

                if (lastSavedDate < DateTime.Now)
                {
                    covidParameters.Uri = uriByCountryTotal;
                    covidParameters.From = lastSavedDate.AddDays(1);
                    covidParameters.To = DateTime.Now;
                    haveToSaveData = true;
                }
            }

            if (haveToSaveData)
            {
                var covidData = await _httpClientService.GetCovidDataAsync(covidParameters);

                if (covidData.Count > 0)
                {                    
                    covidData.OrderByDescending(d => d.Date);
                    await _databaseService.SaveCovidDataAsync(covidData);
                    return true;
                }

                return false;
            }
            return false;
        }

        private bool FilterDataBySearchModelParameters(CovidDTO covidData, SearchModelParameters searchModelParameters)
        {
            var valid = true;

            if (searchModelParameters.FromDate.HasValue)
            {
                valid = valid && covidData.Date >= searchModelParameters.FromDate;
            }

            if (searchModelParameters.ToDate.HasValue)
            {
                valid = valid && covidData.Date <= searchModelParameters.ToDate;
            }

            if (searchModelParameters.ConfirmedMin.HasValue)
            {
                valid = valid && covidData.Confirmed >= searchModelParameters.ConfirmedMin;
            }

            if (searchModelParameters.ConfirmedMax.HasValue)
            {
                valid = valid && covidData.Confirmed <= searchModelParameters.ConfirmedMax;
            }

            return valid;
        }
    }
}
