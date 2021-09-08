using HepsiYemek.Core.Utilities.Response;
using HepsiYemek.Products.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HepsiYemek.Products.Services.Abstract
{
    public interface ICategoryService
    {
        Task<Response<IEnumerable<Category>>> GetCategories();

        Task<Response<Category>> GetCategory(string id);

    }
}
