using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Net6.Products;

namespace WebApi.Net6.Controllers;

[Route("/tools/db")]
public class DbUpdateController : ControllerBase
{
    private readonly DataContext _context;

    public DbUpdateController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        await this._context.Database.MigrateAsync();

        return this.Ok();
    }
}