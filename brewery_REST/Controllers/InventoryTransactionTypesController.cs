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
    public class InventoryTransactionTypesController : ControllerBase
    {
        private readonly bitsContext _context;

        public InventoryTransactionTypesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/InventoryTransactionTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryTransactionType>>> GetInventoryTransactionType()
        {
            return await _context.InventoryTransactionType.ToListAsync();
        }

        // GET: api/InventoryTransactionTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryTransactionType>> GetInventoryTransactionType(int id)
        {
            var inventoryTransactionType = await _context.InventoryTransactionType.FindAsync(id);

            if (inventoryTransactionType == null)
            {
                return NotFound();
            }

            return inventoryTransactionType;
        }

        // PUT: api/InventoryTransactionTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryTransactionType(int id, InventoryTransactionType inventoryTransactionType)
        {
            if (id != inventoryTransactionType.InventoryTransactionTypeId)
            {
                return BadRequest();
            }

            _context.Entry(inventoryTransactionType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryTransactionTypeExists(id))
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

        // POST: api/InventoryTransactionTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<InventoryTransactionType>> PostInventoryTransactionType(InventoryTransactionType inventoryTransactionType)
        {
            _context.InventoryTransactionType.Add(inventoryTransactionType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventoryTransactionType", new { id = inventoryTransactionType.InventoryTransactionTypeId }, inventoryTransactionType);
        }

        // DELETE: api/InventoryTransactionTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InventoryTransactionType>> DeleteInventoryTransactionType(int id)
        {
            var inventoryTransactionType = await _context.InventoryTransactionType.FindAsync(id);
            if (inventoryTransactionType == null)
            {
                return NotFound();
            }

            _context.InventoryTransactionType.Remove(inventoryTransactionType);
            await _context.SaveChangesAsync();

            return inventoryTransactionType;
        }

        private bool InventoryTransactionTypeExists(int id)
        {
            return _context.InventoryTransactionType.Any(e => e.InventoryTransactionTypeId == id);
        }
    }
}
