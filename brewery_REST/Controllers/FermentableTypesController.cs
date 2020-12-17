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
    public class FermentableTypesController : ControllerBase
    {
        private readonly bitsContext _context;

        public FermentableTypesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/FermentableTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FermentableType>>> GetFermentableType()
        {
            return await _context.FermentableType.ToListAsync();
        }

        // GET: api/FermentableTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FermentableType>> GetFermentableType(int id)
        {
            var fermentableType = await _context.FermentableType.FindAsync(id);

            if (fermentableType == null)
            {
                return NotFound();
            }

            return fermentableType;
        }

        // PUT: api/FermentableTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFermentableType(int id, FermentableType fermentableType)
        {
            if (id != fermentableType.FermentableTypeId)
            {
                return BadRequest();
            }

            _context.Entry(fermentableType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FermentableTypeExists(id))
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

        // POST: api/FermentableTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FermentableType>> PostFermentableType(FermentableType fermentableType)
        {
            _context.FermentableType.Add(fermentableType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFermentableType", new { id = fermentableType.FermentableTypeId }, fermentableType);
        }

        // DELETE: api/FermentableTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FermentableType>> DeleteFermentableType(int id)
        {
            var fermentableType = await _context.FermentableType.FindAsync(id);
            if (fermentableType == null)
            {
                return NotFound();
            }

            _context.FermentableType.Remove(fermentableType);
            await _context.SaveChangesAsync();

            return fermentableType;
        }

        private bool FermentableTypeExists(int id)
        {
            return _context.FermentableType.Any(e => e.FermentableTypeId == id);
        }
    }
}
