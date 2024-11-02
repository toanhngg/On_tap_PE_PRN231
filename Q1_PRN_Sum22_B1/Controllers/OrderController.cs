using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Q1_PRN_Sum22_B1.Models;
using System.Reflection.Metadata.Ecma335;

namespace Q1_PRN_Sum22_B1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly PRN_Sum22_B1Context _context;
        public OrderController(PRN_Sum22_B1Context context) {
            _context = context;
         }
        [HttpGet("GetAllOrder")]
        public IActionResult Get() {
            try
            {
                var orderlist = (from o in _context.Orders 
                            join e in _context.Employees on o.EmployeeId equals e.EmployeeId
                            join c in _context.Customers on o.CustomerId equals c.CustomerId
                            join d in _context.Departments on e.DepartmentId equals d.DepartmentId
                            select new
                            {
                                orderId = o.OrderId,
                                customerId = o.CustomerId,
                                customerName = o.Customer.CompanyName,
                                employeeId = o.EmployeeId,
                                employeeName = o.Employee.FirstName + o.Employee.LastName,
                                employeeDepartmentId = o.Employee.DepartmentId,
                                employeeDepartmentName = o.Employee.Department.DepartmentName,
                                orderDate = o.OrderDate,
                                requiredDate = o.RequiredDate,
                                shippedDate = o.ShippedDate,
                                freight = o.Freight,
                                shipName = o.ShipName,
                                shipAddress = o.ShipAddress,
                                shipCity = o.ShipCity,
                                shipRegion = o.ShipRegion,
                                shipPostalCode = o.ShipPostalCode,
                                shipCountry = o.ShipCountry
                            }


                            ).ToList();
                            

                return Ok(orderlist);
            }catch (Exception ex)
            {
                return BadRequest(ex);
            }
        
        }
        [HttpGet("GetOrderByDate/{From}/{To}")]
        public IActionResult Get(DateTime From, DateTime To)
        {
            try
            {
                if (From == null || To == null) return  BadRequest("Vui lòng nhập ngày");
                if (From <= To)
                {
                    var orderlist = (from o in _context.Orders
                                 join e in _context.Employees on o.EmployeeId equals e.EmployeeId
                                 join c in _context.Customers on o.CustomerId equals c.CustomerId
                                 join d in _context.Departments on e.DepartmentId equals d.DepartmentId
                                 where o.OrderDate >= From && o.OrderDate < To
                                     select new
                                 {
                                     orderId = o.OrderId,
                                     customerId = o.CustomerId,
                                     customerName = o.Customer.CompanyName,
                                     employeeId = o.EmployeeId,
                                     employeeName = o.Employee.FirstName + o.Employee.LastName,
                                     employeeDepartmentId = o.Employee.DepartmentId,
                                     employeeDepartmentName = o.Employee.Department.DepartmentName,
                                     orderDate = o.OrderDate,
                                     requiredDate = o.RequiredDate,
                                     shippedDate = o.ShippedDate,
                                     freight = o.Freight,
                                     shipName = o.ShipName,
                                     shipAddress = o.ShipAddress,
                                     shipCity = o.ShipCity,
                                     shipRegion = o.ShipRegion,
                                     shipPostalCode = o.ShipPostalCode,
                                     shipCountry = o.ShipCountry
                                 }
                            ).ToList();
                    return Ok(orderlist);

                }
                else
                {
                    return BadRequest("Vui lòng nhập ngày phù hợp");

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
