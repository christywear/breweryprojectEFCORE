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
    public class FermentablesController : ControllerBase
    {
        private readonly bitsContext _context;

        public FermentablesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/Fermentables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fermentable>>> GetFermentable()
        {
            return await _context.Fermentable.ToListAsync();
        }

        // GET: api/Fermentables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fermentable>> GetFermentable(int id)
        {
            var fermentable = await _context.Fermentable.FindAsync(id);

            if (fermentable == null)
            {
                return NotFound();
            }

            return fermentable;
        }

        // PUT: api/Fermentables/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFermentable(int id, Fermentable fermentable)
        {
            if (id != fermentable.IngredientId)
            {
                return BadRequest();
            }

            _context.Entry(fermentable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FermentableExists(id))
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

        // POST: api/Fermentables
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Fermentable>> PostFermentable(Fermentable fermentable)
        {
            _context.Fermentable.Add(fermentable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FermentableExists(fermentable.IngredientId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFermentable", new { id = fermentable.IngredientId }, fermentable);
        }

        // DELETE: api/Fermentables/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Fermentable>> DeleteFermentable(int id)
        {
            var fermentable = await _context.Fermentable.FindAsync(id);
            if (fermentable == null)
            {
                return NotFound();
            }

            _context.Fermentable.Remove(fermentable);
            await _context.SaveChangesAsync();

            return fermentable;
        }

        private bool FermentableExists(int id)
        {
            return _context.Fermentable.Any(e => e.IngredientId == id);
        }
    }
}
