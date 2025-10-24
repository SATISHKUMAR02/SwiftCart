using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinesslogicLayer.DTO;
using BusinesslogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;

namespace BusinesslogicLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IValidator<ProductAddRequest> _productAddValidator;
        private readonly IValidator<ProductUpdateRequest> _productUpdateValidator;
        private readonly IMapper _mapper;
        private readonly IProductsRepository _productsRepository;
        public ProductService(IValidator<ProductAddRequest> productAddValidator, IValidator<ProductUpdateRequest> productUpdateValidator, IMapper mapper, IProductsRepository productsRepository)
        {
            _mapper = mapper;
            _productAddValidator = productAddValidator;
            _productUpdateValidator = productUpdateValidator;
            _productsRepository = productsRepository;
        }

        public async Task<ProductResponse?> AddProducts(ProductAddRequest request)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));
            // perform validation
            ValidationResult result = await _productAddValidator.ValidateAsync(request);
            if (result.IsValid)
            {
               Product product =  _mapper.Map<Product>(request);
                Product newproduct = await _productsRepository.AddProduct(product);
               ProductResponse response = _mapper.Map<ProductResponse>(newproduct);
                return response;





            }
            else
            {
                string errors = string.Join(", ",result.Errors.Select(temp => temp.ErrorMessage));
                throw new Exception(errors);
            }

        }

        public async Task<bool?> DeleteProducts(Guid productID)
        {
            if(productID == null)
            {
                throw new ArgumentNullException(nameof(productID));
            }
            Product exproduct = await _productsRepository.GetSingleProductByCondition(temp=>temp.ProductID == productID);
            if (exproduct == null) {
            
                return false;
            }
            bool isDeleted = await _productsRepository.DeleteProduct(productID);
            return isDeleted;
        }

        public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> condition)
        {
            Product? product = await _productsRepository.GetSingleProductByCondition(condition);
            if (product == null) { 
                throw new ArgumentNullException(nameof(condition));
            }
            ProductResponse? response = _mapper?.Map<ProductResponse?>(product);
            return response;
        }

        public async Task<List<ProductResponse?>> GetProducts()
        {
            IEnumerable<Product?> products = await _productsRepository.GetProducts();
            IEnumerable<ProductResponse?> response = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return response.ToList();


        }

        public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> condition)
        {
            IEnumerable<Product?> products = await _productsRepository.GetProductsByCondition(condition);
            IEnumerable<ProductResponse> responses = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return responses.ToList();

        }

        public async Task<ProductResponse?> UpdateProducts(ProductUpdateRequest request)
        {
            Product? exproduct = await _productsRepository.GetSingleProductByCondition(temp=>temp.ProductID == request.ProductID);
            if (exproduct == null) {
            return null;
            }
            ValidationResult result = await _productUpdateValidator.ValidateAsync(request);
            if (result.IsValid)
            {
                Product product = _mapper.Map<Product>(request);
                Product newproduct = await _productsRepository.UpdateProduct(product);
                ProductResponse response = _mapper.Map<ProductResponse>(newproduct);
                return response;





            }
            else
            {
                string errors = string.Join(", ", result.Errors.Select(temp => temp.ErrorMessage));
                throw new Exception(errors);
            }



        }
    }
}
