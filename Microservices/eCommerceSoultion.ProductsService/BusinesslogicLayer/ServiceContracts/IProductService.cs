using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinesslogicLayer.DTO;
using DataAccessLayer.Entities;

namespace BusinesslogicLayer.ServiceContracts
{
    public interface IProductService
    {
        Task<List<ProductResponse?>> GetProducts();
        Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product,bool>> condition);
        Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> condition);
        Task<ProductResponse?> AddProducts(ProductAddRequest request);
        Task<ProductResponse?> UpdateProducts(ProductUpdateRequest request);
        Task<bool?> DeleteProducts(Guid productID);


    }
}
