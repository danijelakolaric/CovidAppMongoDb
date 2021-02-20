using System;
using System.Collections.Generic;
using System.Text;

namespace CovidApp.Persistence.ModelDb
{
    public interface ICovidDatabaseSettings
    {
        string CovidCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
