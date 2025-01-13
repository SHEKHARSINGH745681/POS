using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Models;

namespace POS.Controller
{
    // Controllers/OrdersController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly PosDbContext _context;

        public OrdersController(PosDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await _context.Orders.Include(o => o.CustomerId).Include(o => o.OrderDetails).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders.Include(o => o.CustomerId).Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // Controllers/OrdersController.cs
        [HttpPost]
        public async Task<IActionResult> AddOrder(Order order)
        {
            // Calculate the total amount based on order details
            order.TotalAmount = order.OrderDetails.Sum(od => od.Quantity * od.MenuItem.Price);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            // Calculate the total amount again in case of updates
            order.TotalAmount = order.OrderDetails.Sum(od => od.Quantity * od.MenuItem.Price);

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(e => e.OrderId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("CompletePayment")]
        public async Task<IActionResult> CompletePayment(int orderId, PaymentDetails paymentDetails)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.MenuItem)
                                              .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Here, integrate with a payment gateway to process the payment
            // For example, using a hypothetical payment service:
            //var paymentResult = await ProcessPayment(order.TotalAmount, paymentDetails);

            //if (!paymentResult.IsSuccess)
            //{
            //    return BadRequest("Payment failed.");
            //}

            order.IsPaid = true;  // Add an IsPaid field to the Order entity to track payment status
            await _context.SaveChangesAsync();

            return Ok("Payment successful.");
        }
        [HttpPost("ConfirmOrder")]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails)
                                             .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Calculate the total number of people (assuming Quantity represents the number of people per item, adjust as needed)
            int totalPeople = order.OrderDetails.Sum(od => od.Quantity);

            // Find an available table with sufficient capacity
            var table = await _context.Tables
                                      .Where(t => t.IsAvailable && t.Capacity >= totalPeople)
                                      .FirstOrDefaultAsync();

            if (table == null)
            {
                return BadRequest("No available table with sufficient capacity.");
            }

            // Book the table
            table.IsAvailable = false;
            order.TableId = table.TableId;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Order confirmed and table booked.", tableId = table.TableId });
        }
        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var order = await _context.Orders.Include(o => o.Table)
                                             .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Table.IsAvailable = true;  // Free the table
            await _context.SaveChangesAsync();

            return Ok("Order completed and table freed.");
        }

    }
}

