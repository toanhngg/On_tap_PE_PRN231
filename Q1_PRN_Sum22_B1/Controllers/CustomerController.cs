using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Q1_PRN_Sum22_B1.Models;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Q1_PRN_Sum22_B1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public PRN_Sum22_B1Context _context;
        public CustomerController(PRN_Sum22_B1Context context) {
            _context = context;
        }
        [HttpPost("Delete/{CustomerId}")]
        public IActionResult DeleteCustomer(string CustomerId)
        {
            try
            {
                int _customerDeleteCount = 0;
                   int _orderDeleteCount = 0;
                   int _orderDetailDeleteCount = 0;
    
                if (CustomerId == null) return NotFound("Error: Not Found");
                var cus = _context.Customers.Where(x => x.CustomerId.Equals(CustomerId)).FirstOrDefault();
                _customerDeleteCount = 1;

                if (cus == null) return NotFound();
                var orders = _context.Orders.Where(x => x.CustomerId == CustomerId).ToList();
                _orderDeleteCount = orders.Count;

                if (orders.Any())
                {
                    var orderIds = orders.Select(o => o.OrderId).ToList();
                    var orderDetails = _context.OrderDetails.Where(x => orderIds.Contains(x.OrderId)).ToList();
                    _orderDetailDeleteCount = orderDetails.Count();

                    _context.OrderDetails.RemoveRange(orderDetails);

                    _context.Orders.RemoveRange(orders);
                }
                _context.Customers.Remove(cus);
                _context.SaveChanges();
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

                Console.WriteLine(ex.ToString());
                return NotFound(ex.ToString());
            }

        }

    }
}
