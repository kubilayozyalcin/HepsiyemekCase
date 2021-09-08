using HepsiYemek.Products.Entities;
using MongoDB.Driver;

namespace HepsiYemek.Products.Data.Abstract
{
    public interface ISourcingContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<Category> Categories { get; }
    }
}
