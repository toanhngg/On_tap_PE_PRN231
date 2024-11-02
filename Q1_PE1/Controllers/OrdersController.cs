using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_PE1.Models;

namespace Q1_PE1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public readonly PRN_Sum22_B1Context _context;

        public OrdersController(PRN_Sum22_B1Context context)
        {
            _context = context;
        }
        [HttpGet("GetAllOrder")]
        public ActionResult Get()
        {
            try
            {
                var listOrder = (from order in _context.Orders
                                join emp in _context.Employees on order.EmployeeId equals emp.EmployeeId
                                join cus in _context.Customers on order.CustomerId equals cus.CustomerId
                                join dep in _context.Departments on emp.DepartmentId equals dep.DepartmentId              
                                select new {
                                    orderId = order.OrderId,
                                    customerId = order.CustomerId,
                                    customerName = cus.CompanyName,
                                    employeeId = order.EmployeeId,
                                    employeeName = emp.FirstName + emp.LastName,
                                    employeeDepartmentId = dep.DepartmentId,
                                    employeeDepartmentName = dep.DepartmentName,
                                    orderDate = order.OrderDate,
                                    requiredDate = order.RequiredDate,
                                    shippedDate = order.ShippedDate,
                                    freight = order.Freight,
                                    shipName = order.ShipName,
                                    shipAddress = order.ShipAddress,
                                    shipCity = order.ShipCity,
                                    shipRegion = order.ShipRegion,
                                    shipPostalCode = order.ShipPostalCode,
                                    shipCountry = order.ShipCountry
                                }).ToList();
                return Ok(listOrder);
            }catch(Exception ex) { 
            return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllOrderByDate/{From}/{To}")]
        public ActionResult GetByDate(DateTime From , DateTime To)
        {
            try
            {
                var listOrder = (from order in _context.Orders
                                 join emp in _context.Employees on order.EmployeeId equals emp.EmployeeId
                                 join cus in _context.Customers on order.CustomerId equals cus.CustomerId
                                 join dep in _context.Departments on emp.DepartmentId equals dep.DepartmentId
                                 where order.OrderDate > From && order.OrderDate < To
                                 select new
                                 {
                                     orderId = order.OrderId,
                                     customerId = order.CustomerId,
                                     customerName = cus.CompanyName,
                                     employeeId = order.EmployeeId,
                                     employeeName = emp.FirstName + emp.LastName,
                                     employeeDepartmentId = dep.DepartmentId,
                                     employeeDepartmentName = dep.DepartmentName,
                                     orderDate = order.OrderDate,
                                     requiredDate = order.RequiredDate,
                                     shippedDate = order.ShippedDate,
                                     freight = order.Freight,
                                     shipName = order.ShipName,
                                     shipAddress = order.ShipAddress,
                                     shipCity = order.ShipCity,
                                     shipRegion = order.ShipRegion,
                                     shipPostalCode = order.ShipPostalCode,
                                     shipCountry = order.ShipCountry
                                 }).ToList();
                return Ok(listOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
