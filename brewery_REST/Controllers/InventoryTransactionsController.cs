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
    public class InventoryTransactionsController : ControllerBase
    {
        private readonly bitsContext _context;

        public InventoryTransactionsController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/InventoryTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryTransaction>>> GetInventoryTransaction()
        {
            return await _context.InventoryTransaction.ToListAsync();
        }

        // GET: api/InventoryTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryTransaction>> GetInventoryTransaction(int id)
        {
            var inventoryTransaction = await _context.InventoryTransaction.FindAsync(id);

            if (inventoryTransaction == null)
            {
                return NotFound();
            }

            return inventoryTransaction;
        }

        // PUT: api/InventoryTransactions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryTransaction(int id, InventoryTransaction inventoryTransaction)
        {
            if (id != inventoryTransaction.InventoryTransactionId)
            {
                return BadRequest();
            }

            _context.Entry(inventoryTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryTransactionExists(id))
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

        // POST: api/InventoryTransactions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<InventoryTransaction>> PostInventoryTransaction(InventoryTransaction inventoryTransaction)
        {
            _context.InventoryTransaction.Add(inventoryTransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventoryTransaction", new { id = inventoryTransaction.InventoryTransactionId }, inventoryTransaction);
        }

        // DELETE: api/InventoryTransactions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InventoryTransaction>> DeleteInventoryTransaction(int id)
        {
            var inventoryTransaction = await _context.InventoryTransaction.FindAsync(id);
            if (inventoryTransaction == null)
            {
                return NotFound();
            }

            _context.InventoryTransaction.Remove(inventoryTransaction);
            await _context.SaveChangesAsync();

            return inventoryTransaction;
        }

        private bool InventoryTransactionExists(int id)
        {
            return _context.InventoryTransaction.Any(e => e.InventoryTransactionId == id);
        }
    }
}
