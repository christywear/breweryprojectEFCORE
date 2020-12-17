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
    public class BarrelsController : ControllerBase
    {
        private readonly bitsContext _context;

        public BarrelsController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/Barrels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Barrel>>> GetBarrel()
        {
            return await _context.Barrel.ToListAsync();
        }

        // GET: api/Barrels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Barrel>> GetBarrel(int id)
        {
            var barrel = await _context.Barrel.FindAsync(id);

            if (barrel == null)
            {
                return NotFound();
            }

            return barrel;
        }

        // PUT: api/Barrels/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBarrel(int id, Barrel barrel)
        {
            if (id != barrel.BrewContainerId)
            {
                return BadRequest();
            }

            _context.Entry(barrel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BarrelExists(id))
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

        // POST: api/Barrels
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Barrel>> PostBarrel(Barrel barrel)
        {
            _context.Barrel.Add(barrel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BarrelExists(barrel.BrewContainerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBarrel", new { id = barrel.BrewContainerId }, barrel);
        }

        // DELETE: api/Barrels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Barrel>> DeleteBarrel(int id)
        {
            var barrel = await _context.Barrel.FindAsync(id);
            if (barrel == null)
            {
                return NotFound();
            }

            _context.Barrel.Remove(barrel);
            await _context.SaveChangesAsync();

            return barrel;
        }

        private bool BarrelExists(int id)
        {
            return _context.Barrel.Any(e => e.BrewContainerId == id);
        }
    }
}
