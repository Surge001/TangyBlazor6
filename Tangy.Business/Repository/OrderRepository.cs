using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy.Business.Repository.IRepository;
using Tangy.Common;
using Tangy.DataAccess;
using Tangy.DataAccess.Data;
using Tangy.DataAccess.ViewModel;
using Tangy.Models.Dto;

namespace Tangy.Business.Repository
{
    public class OrderRepository : IOrderRepository
    {

        #region Private Fields

        private readonly ApplicationDbContext dbContext;

        private readonly IMapper mapper;
        #endregion

        #region Constructors

        public OrderRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        #endregion
        public async Task<OrderDto> Create(OrderDto objDto)
        {
            try
            {
                OrderViewModel obj = this.mapper.Map<OrderDto, OrderViewModel>(objDto);
                this.dbContext.OrderHeaders.Add(obj.OrderHeader);

                foreach (var details in obj.OrderDetails)
                {
                    details.OrderHeaderId = obj.OrderHeader.Id;
                    dbContext.OrderDetails.Add(details);
                }
                dbContext.OrderDetails.AddRange(obj.OrderDetails);
                await dbContext.SaveChangesAsync();

                return new OrderDto()
                {
                    OrderHeader = mapper.Map<OrderHeader, OrderHeaderDto>(obj.OrderHeader),
                    OrderDetails = mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDto>>(
                        obj.OrderDetails).ToList()
                };

            }
            catch (Exception ex)
            {
                throw;
            }
            return objDto;
        }

        public async Task<int> Delete(int id)
        {
            var objHeader = await dbContext.OrderHeaders.FirstOrDefaultAsync(u => u.Id == id);
            if (objHeader != null)
            {
                IEnumerable<OrderDetail> objDetails = dbContext.OrderDetails.Where(
                    u => u.OrderHeaderId == id);

                dbContext.OrderDetails.RemoveRange(objDetails);
                dbContext.OrderHeaders.Remove(objHeader);
                dbContext.SaveChanges();
            }
            return 0;
        }

        public async Task<OrderDto> Get(int id)
        {
            OrderViewModel order = new()
            {
                OrderHeader = await dbContext.OrderHeaders.FirstOrDefaultAsync(u => u.Id == id),
                OrderDetails = dbContext.OrderDetails.Where(u => u.OrderHeaderId == id)
            };
            if (order != null)
            {
                return mapper.Map<OrderViewModel, OrderDto>(order);
            }
            return new OrderDto();
        }

        public async Task<IEnumerable<OrderDto>> GetAll(string? userId = null, string? status = null)
        {
            List<OrderViewModel> ordersFromDb = new List<OrderViewModel>();
            IEnumerable<OrderHeader> orderHeaderList = dbContext.OrderHeaders;
            IEnumerable<OrderDetail> orderDetailList = dbContext.OrderDetails;
            foreach (OrderHeader orderHeader in orderHeaderList)
            {
                OrderViewModel order = new OrderViewModel()
                {
                    OrderHeader = orderHeader,
                    OrderDetails = orderDetailList.Where(i => i.OrderHeaderId == orderHeader.Id).ToList()
                };
                ordersFromDb.Add(order);
            }
            return mapper.Map<List<OrderViewModel>, List<OrderDto>>(ordersFromDb);
        }

        public async Task<OrderHeaderDto> MarkPaymentSuccess(int id)
        {
            var data = await dbContext.OrderHeaders.FindAsync(id);
            if (data == null)
            {
                return new OrderHeaderDto();
            }
            if (data.Status == SD.Status_Pending)
            {
                data.Status = SD.Status_Confirmed;
                await dbContext.SaveChangesAsync();
                return mapper.Map<OrderHeader, OrderHeaderDto>(data);
            }
            return new OrderHeaderDto();
        }

        public async Task<OrderHeaderDto> UpdateHeader(OrderHeaderDto objDto)
        {
            if (objDto != null)
            {
                var orderHeader = mapper.Map<OrderHeaderDto, OrderHeader>(objDto);
                dbContext.OrderHeaders.Update(orderHeader);
                await dbContext.SaveChangesAsync();
                return mapper.Map<OrderHeader, OrderHeaderDto>(orderHeader);
            }
            return new OrderHeaderDto();
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            var data = await dbContext.OrderHeaders.FindAsync(orderId);
            if (data == null)
            {
                return false;
            }
            data.Status = status;
            if (status == SD.Status_Shipped)
            {
                data.ShippingDate = DateTime.Now;
            }
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
