// Controllers/MenuController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Models;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly PosDbContext _context;

    public MenuController(PosDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetMenuItems()
    {
        return Ok(await _context.MenuItems.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> AddMenuItem(MenuItem menuItem)
    {
        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMenuItems), new { id = menuItem.MenuItemId }, menuItem);
    }
}

// Similar controllers for Customers, Orders, and OrderDetails.
