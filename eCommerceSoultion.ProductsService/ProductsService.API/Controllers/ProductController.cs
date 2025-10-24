using BusinesslogicLayer.DTO;
using BusinesslogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductsService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<ActionResult<ProductResponse>> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {


                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetSinlgeProducts")]
        public async Task<ActionResult<ProductResponse>> GetSingleProduct(Guid productID)
        {
            try
            {
                var product = await _productService.GetProductByCondition(temp => temp.ProductID == productID);
                return Ok(product);
            }
            catch (Exception ex)
            {


                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<ActionResult<ProductResponse>> CreateNewProduct(ProductAddRequest request)
        {
            try
            {
                var newproduct = await _productService.AddProducts(request);
                return Ok(newproduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [Route("UpdateProducts/{productId:guid}")]
        public async Task<ActionResult<ProductResponse>> UpdateProductDetails(ProductUpdateRequest request)
        {
            try
            {
                var exprod = _productService.UpdateProducts(request);
                return Ok(exprod);
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteProducts/{productId:guid}")]
        public async Task<ActionResult<ProductResponse>> DeleteProductDetails(Guid productId)
        {
            try
            {
                var exprod = _productService.DeleteProducts(productId);
                return Ok(exprod);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
