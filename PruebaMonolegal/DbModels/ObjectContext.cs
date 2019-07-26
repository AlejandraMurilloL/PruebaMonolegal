using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PruebaMonolegal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaMonolegal.DbModels
{
    public class ObjectContext
    {
        public IConfigurationRoot Configuration { get; }
        private IMongoDatabase _database = null;

        public ObjectContext(IOptions<Settings> settings)
        {
            Configuration = settings.Value.iConfigurationRoot;
            settings.Value.connectionString = Configuration.GetSection("MongoConection:ConnectionString").Value;
            settings.Value.database = Configuration.GetSection("MongoConection:Database").Value;

            var client = new MongoClient(settings.Value.connectionString);

            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.database);
            }
        }

        public IMongoCollection<Factura> Facturas
        {
            get
            {
                return _database.GetCollection<Factura>("facturas");
            }
        }
    }
}
