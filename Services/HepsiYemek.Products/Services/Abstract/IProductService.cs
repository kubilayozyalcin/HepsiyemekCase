using HepsiYemek.Core.Utilities.Response;
using HepsiYemek.Products.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HepsiYemek.Products.Services.Abstract
{
    public interface IProductService
    {

        Task<Response<IEnumerable<Product>>> GetProducts();

        Task<Response<Product>> GetProduct(string id);

        Task<Response<Product>> Create(Product product);

        Task<Response<NoContent>> Update(Product product);

        Task<Response<NoContent>> Delete(string id);
    }
}
