using CovidApp.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CovidApp.Services.Interface
{
    public interface IDatabaseService
    {
        Task<List<CovidDTO>> GetCovidDataAsync();
        Task SaveCovidDataAsync(List<CovidDTO> covidData);
        Task<bool> CheckIfDbIsEmpty();
        Task<DateTime> GetLastDate();
    }
}
