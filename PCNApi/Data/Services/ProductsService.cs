using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCNApi.Data.Pagination;
using PCNApi.Models;

namespace PCNApi.Data.Services
{
    public class ProductsService : IProductsService
    {
        private readonly PCNPublicDBContext _context;
        public ProductsService(PCNPublicDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var allProducts = await _context.Products.AsNoTracking().AsQueryable().Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(s => s.Stocks).OrderBy(n => n.ProductId).ToListAsync();
            return allProducts;
        }

        public async Task<int> GetAllProductsCount()
        {
            var data = await GetAllProducts();
            return data.Count();
        }


        public async Task<IEnumerable<Product>> GetPaginatedProducts(int? pageNumber, int? pageSize)
        {
            var allProducts = await GetAllProducts();
            // Paging
            allProducts = PaginatedList<Product>.Create(allProducts.AsQueryable(), pageNumber ?? 1, pageSize ?? 5);
            return allProducts;
        }
        public async Task<Product> GetProductById(int id) => await _context.Products.AsNoTracking().AsQueryable().Include(b => b.Brand)
                                                                    .Include(c => c.Category)
                                                                    .Include(s => s.Stocks).OrderBy(n => n.ProductId)
                                                                    .FirstOrDefaultAsync(n => n.ProductId == id);
        public async Task<Product> DeleteProductById(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(n => n.ProductId == id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"The Product with id: {id} does not exist");
            }

            return product;
        }

        public async Task<Product> UpdateProduct(Product p)
        {
            var product = await _context.Products.FirstOrDefaultAsync(n => n.ProductId == p.ProductId);

            if (product != null)
            {
                _context.Products.Update(p);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"The Product with id: {p.ProductId} does not exist");
            }

            return product;
        }



    }
}