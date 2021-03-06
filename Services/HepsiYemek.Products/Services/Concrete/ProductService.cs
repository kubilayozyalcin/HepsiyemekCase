using HepsiYemek.Core.Utilities.Messages;
using HepsiYemek.Core.Utilities.Response;
using HepsiYemek.Products.Data.Abstract;
using HepsiYemek.Products.Entities;
using HepsiYemek.Products.Services.Abstract;
using HepsiYemek.Products.Services.Redis;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HepsiYemek.Products.Services.Concrete
{
    public class ProductService : IProductService
    {

        private readonly ISourcingContext _sorucingcontext;
        private readonly RedisService _redisService;

        public ProductService(ISourcingContext sorucingcontext, RedisService redisService)
        {
            _sorucingcontext = sorucingcontext;
            _redisService = redisService;
        }

        public async Task<Response<IEnumerable<Product>>> GetProducts()
        {           
            var products = await _sorucingcontext.Products.Find(p => true).ToListAsync();

            if (products.Any())
            {
                foreach (var product in products)
                {
                    product.Category = await _sorucingcontext.Categories.Find(x => x.Id == product.CategoryId).FirstAsync();
                }
            }
            else
            {
                products = new List<Product>();
            }

            return Response<IEnumerable<Product>>.Success((products), StatusCodes.Status200OK);
        }
        // Add Redis Cache Product
        public async Task<Response<Product>> GetProduct(string id)
        {
            var existProduct = await _redisService.GetDb().StringGetAsync(id);

            if (!String.IsNullOrEmpty(existProduct))
                return Response<Product>.Success(JsonSerializer.Deserialize<Product>(existProduct), StatusCodes.Status200OK);
           

            var product = await _sorucingcontext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

            await _redisService.GetDb().StringSetAsync(product.Id, JsonSerializer.Serialize(product));


            if (product == null)
                return Response<Product>.Fail(Messages.ProductNotFound, StatusCodes.Status404NotFound);

            product.Category = await _sorucingcontext.Categories.Find<Category>(x => x.Id == product.CategoryId).FirstAsync();

            return Response<Product>.Success((product), StatusCodes.Status200OK);
        }

        public async Task<Response<Product>> Create(Product product)
        {
            await _sorucingcontext.Products.InsertOneAsync(product);

            return Response<Product>.Success(StatusCodes.Status200OK);
        }

        public async Task<Response<NoContent>> Update(Product product)
        {
            var updateResult = await _sorucingcontext.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            if (updateResult == null)
                return Response<NoContent>.Fail(Messages.ProductNotFound, StatusCodes.Status200OK);
            else
                return Response<NoContent>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<Response<NoContent>> Delete(string id)
        {
            var filter = Builders<Product>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult = await _sorucingcontext.Products.DeleteOneAsync(filter);

            if (deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0)
                return Response<NoContent>.Success(StatusCodes.Status204NoContent);
            else
                return Response<NoContent>.Fail(Messages.ProductNotFound, StatusCodes.Status404NotFound);
        }
    }
}
