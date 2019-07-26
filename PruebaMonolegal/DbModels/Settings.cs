using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaMonolegal.DbModels
{
    public class Settings
    {
        public string connectionString;
        public string database;
        public IConfigurationRoot iConfigurationRoot;
    }
}
