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
    public class ProductContainerSizesController : ControllerBase
    {
        private readonly bitsContext _context;

        public ProductContainerSizesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/ProductContainerSizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductContainerSize>>> GetProductContainerSize()
        {
            return await _context.ProductContainerSize.ToListAsync();
        }

        // GET: api/ProductContainerSizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductContainerSize>> GetProductContainerSize(int id)
        {
            var productContainerSize = await _context.ProductContainerSize.FindAsync(id);

            if (productContainerSize == null)
            {
                return NotFound();
            }

            return productContainerSize;
        }

        // PUT: api/ProductContainerSizes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductContainerSize(int id, ProductContainerSize productContainerSize)
        {
            if (id != productContainerSize.ContainerSizeId)
            {
                return BadRequest();
            }

            _context.Entry(productContainerSize).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductContainerSizeExists(id))
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

        // POST: api/ProductContainerSizes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProductContainerSize>> PostProductContainerSize(ProductContainerSize productContainerSize)
        {
            _context.ProductContainerSize.Add(productContainerSize);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductContainerSize", new { id = productContainerSize.ContainerSizeId }, productContainerSize);
        }

        // DELETE: api/ProductContainerSizes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductContainerSize>> DeleteProductContainerSize(int id)
        {
            var productContainerSize = await _context.ProductContainerSize.FindAsync(id);
            if (productContainerSize == null)
            {
                return NotFound();
            }

            _context.ProductContainerSize.Remove(productContainerSize);
            await _context.SaveChangesAsync();

            return productContainerSize;
        }

        private bool ProductContainerSizeExists(int id)
        {
            return _context.ProductContainerSize.Any(e => e.ContainerSizeId == id);
        }
    }
}
