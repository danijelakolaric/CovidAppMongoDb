using CovidApp.Entity.DTOs;
using CovidApp.Entity.Parameters;
using CovidApp.Persistence.ModelDb;
using CovidApp.Services.Interface;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidApp.Services.Service
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IMongoCollection<CovidDTO> _covidData;

        public DatabaseService(ICovidDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _covidData = database.GetCollection<CovidDTO>(settings.CovidCollectionName);
        }

        public async Task<List<CovidDTO>> GetCovidDataAsync() => await _covidData.AsQueryable().ToListAsync();

        public async Task SaveCovidDataAsync(List<CovidDTO> covidData) => await _covidData.InsertManyAsync(covidData);

        public async Task<bool> CheckIfDbIsEmpty() => await _covidData.AsQueryable().CountAsync() == 0;

        public async Task<DateTime> GetLastDate()
        {
            var first = await _covidData.AsQueryable().OrderByDescending(x => x.Date).FirstAsync();

            var date = first.Date;

            return date;
        }
    }
}
