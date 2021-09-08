using HepsiYemek.Products.Data.Abstract;
using HepsiYemek.Products.Entities;
using HepsiYemek.Products.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiYemek.Products.Data.Concrete
{
    public class SourcingContext : ISourcingContext
    {
        public SourcingContext(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Products = database.GetCollection<Product>(settings.ProductCollectionName);
            Categories = database.GetCollection<Category>(settings.CategoryCollectionName);
            ProductContextSeed.SeedData(Products);
            CategoryContextSeed.SeedData(Categories);
        }

        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<Category> Categories { get; }
    }
}
