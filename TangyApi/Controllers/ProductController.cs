using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tangy.Business.Repository.IRepository;
using Tangy.Common;
using Tangy.Models.Dto;

namespace TangyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region Private Fields

        private readonly IProductRepository productRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="repository">Instance of the repository</param>
        public ProductController(IProductRepository repository)
        {
            this.productRepository = repository;
        }

        #endregion

        [HttpGet]
        [Authorize(AuthenticationSchemes ="Bearer", Roles = SD.Role_Customer)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await this.productRepository.GetAll());
        }

        [HttpGet("productId")]
        public async Task<IActionResult> Get(int? productId)
        {
            if (productId == null || productId <= 0)
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var product = await this.productRepository.Get(productId.Value);
            if (product == null)
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = $"Product is not found by product Id {productId.Value}",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(product);
        }
    }
}
