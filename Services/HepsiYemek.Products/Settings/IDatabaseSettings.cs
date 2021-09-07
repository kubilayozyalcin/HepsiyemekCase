using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiYemek.Products.Settings
{
    public interface IDatabaseSettings
    {
        public string ProductCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
