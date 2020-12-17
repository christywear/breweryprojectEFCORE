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
    public class BrewContainersController : ControllerBase
    {
        private readonly bitsContext _context;

        public BrewContainersController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/BrewContainers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrewContainer>>> GetBrewContainer()
        {
            return await _context.BrewContainer.ToListAsync();
        }

        // GET: api/BrewContainers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BrewContainer>> GetBrewContainer(int id)
        {
            var brewContainer = await _context.BrewContainer.FindAsync(id);

            if (brewContainer == null)
            {
                return NotFound();
            }

            return brewContainer;
        }

        // PUT: api/BrewContainers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrewContainer(int id, BrewContainer brewContainer)
        {
            if (id != brewContainer.BrewContainerId)
            {
                return BadRequest();
            }

            _context.Entry(brewContainer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrewContainerExists(id))
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

        // POST: api/BrewContainers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BrewContainer>> PostBrewContainer(BrewContainer brewContainer)
        {
            _context.BrewContainer.Add(brewContainer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBrewContainer", new { id = brewContainer.BrewContainerId }, brewContainer);
        }

        // DELETE: api/BrewContainers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BrewContainer>> DeleteBrewContainer(int id)
        {
            var brewContainer = await _context.BrewContainer.FindAsync(id);
            if (brewContainer == null)
            {
                return NotFound();
            }

            _context.BrewContainer.Remove(brewContainer);
            await _context.SaveChangesAsync();

            return brewContainer;
        }

        private bool BrewContainerExists(int id)
        {
            return _context.BrewContainer.Any(e => e.BrewContainerId == id);
        }
    }
}
