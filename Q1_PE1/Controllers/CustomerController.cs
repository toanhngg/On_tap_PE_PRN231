using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_PE1.Models;

namespace Q1_PE1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public readonly PRN_Sum22_B1Context _context;

        public CustomerController(PRN_Sum22_B1Context context)
        {
            _context = context;
        }
        [HttpPost("Delete/{CustomerId}")]
        public IActionResult Delete(string CustomerId)
        {
            try
            {
                int _customerDeleteCount = 0;
                int _orderDeleteCount = 0;
                int _orderDetailDeleteCount = 0;
                //C1 var cus = _context.Customers
                //    .Where(x=> x.CustomerId == CustomerId).FirstOrDefault();
                //_customerDeleteCount = 1;
                //var order = _context.Orders.Where(x=> x.CustomerId == CustomerId).ToList();
                //_orderDeleteCount = order.Count();  
                //if (cus == null)
                //{
                //    return NotFound();
                //}
                //if (order.Any())
                //{
                //    var orderIds = order.Select(o => o.OrderId).ToList();
                //    var orderDetails = _context.OrderDetails.Where(x => orderIds.Contains(x.OrderId)).ToList();
                //    _orderDetailDeleteCount = orderDetails.Count();

                //    _context.OrderDetails.RemoveRange(orderDetails);

                //    _context.Orders.RemoveRange(order);
                //}
                //_context.Customers.Remove(cus);
                //_context.SaveChanges(); 


                //C2
                var cus = _context.Customers
                   .Where(x=> x.CustomerId == CustomerId).FirstOrDefault();

                if (cus == null)
                {
                    return NotFound();
                }
                else
                {
                    _customerDeleteCount = 1;
                    var orderCount = _context.Orders.Where(x => x.CustomerId == cus.CustomerId).ToList();

                    _orderDeleteCount = orderCount.Count();
                    List<OrderDetail> orders = new List<OrderDetail>();
                    for (int i = 0; i < orderCount.Count; i++)
                    {
                        List<OrderDetail> orderDetail = _context.OrderDetails.Where(x => x.OrderId == (orderCount[i]).OrderId).ToList();

                        orders.AddRange(orderDetail);
                    }
                    _orderDetailDeleteCount = orders.Count;

                    _context.RemoveRange(orders);
                    _context.RemoveRange(orderCount);
                    _context.Remove(cus);
                    _context.SaveChanges();

                }


                var response = new
                {
                    customerDeleteCount = _customerDeleteCount,
                    orderDeleteCount = _orderDeleteCount,
                    orderDetailDeleteCount = _orderDetailDeleteCount
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
