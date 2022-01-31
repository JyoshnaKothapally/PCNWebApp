using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PCNApi.Models;

namespace PCNApi.Data.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<int> GetAllProductsCount();
        Task<IEnumerable<Product>> GetPaginatedProducts(int? pageNumber, int? pageSize);
        Task<Product> GetProductById(int id);
        Task<Product> DeleteProductById(int id);
        Task<Product> UpdateProduct(Product p);
    }
}
