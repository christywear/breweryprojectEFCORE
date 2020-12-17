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
    public class HopsController : ControllerBase
    {
        private readonly bitsContext _context;

        public HopsController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/Hops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hop>>> GetHop()
        {
            return await _context.Hop.ToListAsync();
        }

        // GET: api/Hops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hop>> GetHop(int id)
        {
            var hop = await _context.Hop.FindAsync(id);

            if (hop == null)
            {
                return NotFound();
            }

            return hop;
        }

        // PUT: api/Hops/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHop(int id, Hop hop)
        {
            if (id != hop.IngredientId)
            {
                return BadRequest();
            }

            _context.Entry(hop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HopExists(id))
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

        // POST: api/Hops
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Hop>> PostHop(Hop hop)
        {
            _context.Hop.Add(hop);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HopExists(hop.IngredientId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHop", new { id = hop.IngredientId }, hop);
        }

        // DELETE: api/Hops/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Hop>> DeleteHop(int id)
        {
            var hop = await _context.Hop.FindAsync(id);
            if (hop == null)
            {
                return NotFound();
            }

            _context.Hop.Remove(hop);
            await _context.SaveChangesAsync();

            return hop;
        }

        private bool HopExists(int id)
        {
            return _context.Hop.Any(e => e.IngredientId == id);
        }
    }
}
