using HepsiYemek.Core.Utilities.Messages;
using HepsiYemek.Core.Utilities.Response;
using HepsiYemek.Products.Data.Abstract;
using HepsiYemek.Products.Entities;
using HepsiYemek.Products.Services.Abstract;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiYemek.Products.Services.Concrete
{
    public class ProductService : IProductService
    {

        private readonly ISourcingContext _sorucingcontext;

        public ProductService(ISourcingContext sorucingcontext)
        {
            _sorucingcontext = sorucingcontext;
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

        public async Task<Response<Product>> GetProduct(string id)
        {
            var product = await _sorucingcontext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

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
