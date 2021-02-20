using CovidApp.Entity.DTOs;
using CovidApp.Entity.Parameters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CovidApp.Services.Interface
{
    public interface IHttpClientService
    {
        Task<List<CovidDTO>> GetCovidDataAsync(CovidParameters covidParameters);
    }
}
