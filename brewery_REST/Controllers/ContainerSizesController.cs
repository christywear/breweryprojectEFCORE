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
    public class ContainerSizesController : ControllerBase
    {
        private readonly bitsContext _context;

        public ContainerSizesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/ContainerSizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContainerSize>>> GetContainerSize()
        {
            return await _context.ContainerSize.ToListAsync();
        }

        // GET: api/ContainerSizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContainerSize>> GetContainerSize(int id)
        {
            var containerSize = await _context.ContainerSize.FindAsync(id);

            if (containerSize == null)
            {
                return NotFound();
            }

            return containerSize;
        }

        // PUT: api/ContainerSizes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContainerSize(int id, ContainerSize containerSize)
        {
            if (id != containerSize.ContainerSizeId)
            {
                return BadRequest();
            }

            _context.Entry(containerSize).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContainerSizeExists(id))
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

        // POST: api/ContainerSizes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ContainerSize>> PostContainerSize(ContainerSize containerSize)
        {
            _context.ContainerSize.Add(containerSize);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContainerSize", new { id = containerSize.ContainerSizeId }, containerSize);
        }

        // DELETE: api/ContainerSizes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContainerSize>> DeleteContainerSize(int id)
        {
            var containerSize = await _context.ContainerSize.FindAsync(id);
            if (containerSize == null)
            {
                return NotFound();
            }

            _context.ContainerSize.Remove(containerSize);
            await _context.SaveChangesAsync();

            return containerSize;
        }

        private bool ContainerSizeExists(int id)
        {
            return _context.ContainerSize.Any(e => e.ContainerSizeId == id);
        }
    }
}
