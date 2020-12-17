using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreweryEFCoreClasses.Models;

namespace brewery_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductContainerInventoriesController : ControllerBase
    {
        private readonly bitsContext _context;

        public ProductContainerInventoriesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/ProductContainerInventories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductContainerInventory>>> GetProductContainerInventory()
        {
            return await _context.ProductContainerInventory.ToListAsync();
        }

        // GET: api/ProductContainerInventories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductContainerInventory>> GetProductContainerInventory(int id)
        {
            var productContainerInventory = await _context.ProductContainerInventory.FindAsync(id);

            if (productContainerInventory == null)
            {
                return NotFound();
            }

            return productContainerInventory;
        }

        // PUT: api/ProductContainerInventories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductContainerInventory(int id, ProductContainerInventory productContainerInventory)
        {
            if (id != productContainerInventory.ContainerSizeId)
            {
                return BadRequest();
            }

            _context.Entry(productContainerInventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductContainerInventoryExists(id))
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

        // POST: api/ProductContainerInventories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProductContainerInventory>> PostProductContainerInventory(ProductContainerInventory productContainerInventory)
        {
            _context.ProductContainerInventory.Add(productContainerInventory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductContainerInventoryExists(productContainerInventory.ContainerSizeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductContainerInventory", new { id = productContainerInventory.ContainerSizeId }, productContainerInventory);
        }

        // DELETE: api/ProductContainerInventories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductContainerInventory>> DeleteProductContainerInventory(int id)
        {
            var productContainerInventory = await _context.ProductContainerInventory.FindAsync(id);
            if (productContainerInventory == null)
            {
                return NotFound();
            }

            _context.ProductContainerInventory.Remove(productContainerInventory);
            await _context.SaveChangesAsync();

            return productContainerInventory;
        }

        private bool ProductContainerInventoryExists(int id)
        {
            return _context.ProductContainerInventory.Any(e => e.ContainerSizeId == id);
        }
    }
}
