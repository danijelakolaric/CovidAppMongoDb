using System;
using System.Collections.Generic;
using System.Text;

namespace CovidApp.Persistence.ModelDb
{
    public class CovidDatabaseSettings : ICovidDatabaseSettings
    {
        public string CovidCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
