using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace DataAccessLayer.RepositoryContracts
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>>GetProducts();
        Task<IEnumerable<Product>>GetProductsByCondition(Expression<Func<Product,bool>>condition);
        Task<Product?>GetSingleProductByCondition(Expression<Func<Product,bool>>condition);

        Task<Product?>AddProduct(Product product);
        Task<Product?>UpdateProduct(Product product);
        Task<bool>DeleteProduct(Guid productId);

        

    }
}
