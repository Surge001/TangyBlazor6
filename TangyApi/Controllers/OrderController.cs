using Microsoft.AspNetCore.Mvc;
using Tangy.Business.Repository.IRepository;
using Tangy.Models.Dto;

namespace TangyApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        #region Private Fields

        private readonly IOrderRepository orderRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="repository">Instance of the repository</param>
        public OrderController(IOrderRepository repository)
        {
            this.orderRepository = repository;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await this.orderRepository.GetAll());
        }

        [HttpGet("orderHeaderId")]
        public async Task<IActionResult> Get(int? orderHeaderId)
        {
            if (orderHeaderId == null || orderHeaderId <= 0)
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var orderHeader = await this.orderRepository.Get(orderHeaderId.Value);
            if (orderHeader == null)
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = $"Order is not found by order Header Id {orderHeaderId.Value}",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(orderHeader);
        }
    }
}
