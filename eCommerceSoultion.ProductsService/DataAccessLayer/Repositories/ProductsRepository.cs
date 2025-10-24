using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public ProductsRepository(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public async Task<Product?> AddProduct(Product product)
        {
            _dbcontext.Products.Add(product);
            await _dbcontext.SaveChangesAsync();
            return product;
        }
        public async Task<bool> DeleteProduct(Guid productId)
        {
            Product? exprod = await _dbcontext.Products.FirstOrDefaultAsync(temp => temp.ProductID == productId);
            if (exprod != null)
            {
                _dbcontext.Products.Remove(exprod);
                _dbcontext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }


        }

        public async Task<Product?> GetSingleProductByCondition(Expression<Func<Product, bool>> condition)
        {
            return await _dbcontext.Products.FirstOrDefaultAsync(condition);
        }


        public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> condition)
        {
            return await _dbcontext.Products.Where(condition).ToListAsync();

        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _dbcontext.Products.ToListAsync();
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            Product? exprod = await _dbcontext.Products.FirstOrDefaultAsync(temp => temp.ProductID == product.ProductID);
            if (exprod == null)
            {
                return null;

            }
            else
            {
                //exprod.ProductID = product.ProductID;
                exprod.ProductName = product.ProductName;
                exprod.UnitPrice    = product.UnitPrice;
                exprod.QuantityInStock = product.QuantityInStock;
                exprod.Category = product.Category;
                await _dbcontext.SaveChangesAsync();
                return exprod;
            }
        }
    }
}