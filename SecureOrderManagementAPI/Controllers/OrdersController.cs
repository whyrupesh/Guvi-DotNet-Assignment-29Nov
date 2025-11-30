using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Data;
using OrderManagementApi.Models;
using System.Security.Claims;

namespace OrderManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public OrdersController(AppDbContext db) { _db = db; }
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var orders = await _db.Orders.Where(o => o.OwnerId == userId).ToListAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = GetUserId();
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id && o.OwnerId == userId);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            order.OwnerId = GetUserId();
            order.TotalAmount = order.UnitPrice * order.Quantity;
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order updated)
        {
            var userId = GetUserId();
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id && o.OwnerId == userId);
            if (order == null) return NotFound();

            order.ProductName = updated.ProductName;
            order.Quantity = updated.Quantity;
            order.UnitPrice = updated.UnitPrice;
            order.TotalAmount = updated.UnitPrice * updated.Quantity;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id && o.OwnerId == userId);
            if (order == null) return NotFound();
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
