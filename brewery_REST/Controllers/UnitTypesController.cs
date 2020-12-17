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
    public class UnitTypesController : ControllerBase
    {
        private readonly bitsContext _context;

        public UnitTypesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/UnitTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnitType>>> GetUnitType()
        {
            return await _context.UnitType.ToListAsync();
        }

        // GET: api/UnitTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UnitType>> GetUnitType(int id)
        {
            var unitType = await _context.UnitType.FindAsync(id);

            if (unitType == null)
            {
                return NotFound();
            }

            return unitType;
        }

        // PUT: api/UnitTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnitType(int id, UnitType unitType)
        {
            if (id != unitType.UnitTypeId)
            {
                return BadRequest();
            }

            _context.Entry(unitType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnitTypeExists(id))
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

        // POST: api/UnitTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UnitType>> PostUnitType(UnitType unitType)
        {
            _context.UnitType.Add(unitType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUnitType", new { id = unitType.UnitTypeId }, unitType);
        }

        // DELETE: api/UnitTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UnitType>> DeleteUnitType(int id)
        {
            var unitType = await _context.UnitType.FindAsync(id);
            if (unitType == null)
            {
                return NotFound();
            }

            _context.UnitType.Remove(unitType);
            await _context.SaveChangesAsync();

            return unitType;
        }

        private bool UnitTypeExists(int id)
        {
            return _context.UnitType.Any(e => e.UnitTypeId == id);
        }
    }
}
