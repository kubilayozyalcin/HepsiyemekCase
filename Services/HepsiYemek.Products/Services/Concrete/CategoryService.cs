using HepsiYemek.Core.Utilities.Messages;
using HepsiYemek.Core.Utilities.Response;
using HepsiYemek.Products.Data.Abstract;
using HepsiYemek.Products.Entities;
using HepsiYemek.Products.Services.Abstract;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HepsiYemek.Products.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ISourcingContext _sorucingcontext;

        public CategoryService(ISourcingContext sorucingcontext)
        {
            _sorucingcontext = sorucingcontext;
        }


        public async Task<Response<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _sorucingcontext.Categories.Find(p => true).ToListAsync();
       
            return Response<IEnumerable<Category>>.Success((categories), StatusCodes.Status200OK);
        }

        public async Task<Response<Category>> GetCategory(string id)
        {
            var category = await _sorucingcontext.Categories.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (category == null)
                return Response<Category>.Fail(Messages.CatogoryNotFound, StatusCodes.Status404NotFound);

            return Response<Category>.Success((category), StatusCodes.Status200OK);
        }

    }
}
