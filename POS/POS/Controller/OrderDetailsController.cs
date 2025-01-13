using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Models;

namespace POS.Controller
{
    // Controllers/OrderDetailsController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly PosDbContext _context;

        public OrderDetailsController(PosDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetails()
        {
            return Ok(await _context.OrderDetails.Include(od => od.Order).Include(od => od.MenuItem).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetails.Include(od => od.Order).Include(od => od.MenuItem).FirstOrDefaultAsync(od => od.OrderDetailId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return Ok(orderDetail);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrderDetail), new { id = orderDetail.OrderDetailId }, orderDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailId)
            {
                return BadRequest();
            }

            _context.Entry(orderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.OrderDetails.Any(e => e.OrderDetailId == id))
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
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}

