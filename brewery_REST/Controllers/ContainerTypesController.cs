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
    public class ContainerTypesController : ControllerBase
    {
        private readonly bitsContext _context;

        public ContainerTypesController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/ContainerTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContainerType>>> GetContainerType()
        {
            return await _context.ContainerType.ToListAsync();
        }

        // GET: api/ContainerTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContainerType>> GetContainerType(int id)
        {
            var containerType = await _context.ContainerType.FindAsync(id);

            if (containerType == null)
            {
                return NotFound();
            }

            return containerType;
        }

        // PUT: api/ContainerTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContainerType(int id, ContainerType containerType)
        {
            if (id != containerType.ContainerTypeId)
            {
                return BadRequest();
            }

            _context.Entry(containerType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContainerTypeExists(id))
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

        // POST: api/ContainerTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ContainerType>> PostContainerType(ContainerType containerType)
        {
            _context.ContainerType.Add(containerType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContainerType", new { id = containerType.ContainerTypeId }, containerType);
        }

        // DELETE: api/ContainerTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContainerType>> DeleteContainerType(int id)
        {
            var containerType = await _context.ContainerType.FindAsync(id);
            if (containerType == null)
            {
                return NotFound();
            }

            _context.ContainerType.Remove(containerType);
            await _context.SaveChangesAsync();

            return containerType;
        }

        private bool ContainerTypeExists(int id)
        {
            return _context.ContainerType.Any(e => e.ContainerTypeId == id);
        }
    }
}
