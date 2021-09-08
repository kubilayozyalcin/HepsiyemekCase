using HepsiYemek.Products.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace HepsiYemek.Products.Data
{
    public class CategoryContextSeed
    {
        public static void SeedData(IMongoCollection<Category> categoryCollection)
        {
            bool existProduct = categoryCollection.Find(p => true).Any();
            if (!existProduct)
            {
                categoryCollection.InsertManyAsync(GetConfigureCategory());
            }
        }

        private static IEnumerable<Category> GetConfigureCategory()
        {
            return new List<Category>()
            {
                  new Category()
                {
                    Name = "Döner Çeşitleri",
                    Description = "Döner Kategorisi",
                },
                  new Category()
                {
                    Name = "Kebap Çeşitleri",
                    Description = "Kebap Kategorisi",
                },         

            };
        }
    }
}
