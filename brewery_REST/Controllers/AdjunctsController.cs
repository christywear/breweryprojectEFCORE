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
    public class AdjunctsController : ControllerBase
    {
        private readonly bitsContext _context;

        public AdjunctsController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/Adjuncts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Adjunct>>> GetAdjunct()
        {
            return await _context.Adjunct.ToListAsync();
        }

        // GET: api/Adjuncts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Adjunct>> GetAdjunct(int id)
        {
            var adjunct = await _context.Adjunct.FindAsync(id);

            if (adjunct == null)
            {
                return NotFound();
            }

            return adjunct;
        }

        // PUT: api/Adjuncts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdjunct(int id, Adjunct adjunct)
        {
            if (id != adjunct.IngredientId)
            {
                return BadRequest();
            }

            _context.Entry(adjunct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdjunctExists(id))
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

        // POST: api/Adjuncts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Adjunct>> PostAdjunct(Adjunct adjunct)
        {
            _context.Adjunct.Add(adjunct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdjunctExists(adjunct.IngredientId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAdjunct", new { id = adjunct.IngredientId }, adjunct);
        }

        // DELETE: api/Adjuncts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Adjunct>> DeleteAdjunct(int id)
        {
            var adjunct = await _context.Adjunct.FindAsync(id);
            if (adjunct == null)
            {
                return NotFound();
            }

            _context.Adjunct.Remove(adjunct);
            await _context.SaveChangesAsync();

            return adjunct;
        }

        private bool AdjunctExists(int id)
        {
            return _context.Adjunct.Any(e => e.IngredientId == id);
        }
    }
}
