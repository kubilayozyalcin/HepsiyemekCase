using HepsiYemek.Products.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiYemek.Products.Data
{
    public class ProductContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetConfigureProducts());
            }
        }

        private static IEnumerable<Product> GetConfigureProducts()
        {
            return new List<Product>()
            {
                  new Product()
                {
                    Name = "Döner",
                    Description = "1 Porsiyon Yaprak Döner",
                    CategoryId = "1",
                    Price = 25.90M,
                    Currency= "TL"
                },
                  new Product()
                {
                   Name = "Adana Kebap",
                    Description = "1 Porsiyon Adana Kebap",
                    CategoryId = "2",
                    Price = 39.90M,
                    Currency= "TL"
                },
                  new Product()
                {
                    Name = "İskender",
                    Description = "1 Porsiyon İskender",
                    CategoryId = "1",
                    Price = 50.00M,
                    Currency= "TL"
                },

            };
        }
    }
}
