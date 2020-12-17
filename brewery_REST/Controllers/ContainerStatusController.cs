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
    public class ContainerStatusController : ControllerBase
    {
        private readonly bitsContext _context;

        public ContainerStatusController(bitsContext context)
        {
            _context = context;
        }

        // GET: api/ContainerStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContainerStatus>>> GetContainerStatus()
        {
            return await _context.ContainerStatus.ToListAsync();
        }

        // GET: api/ContainerStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContainerStatus>> GetContainerStatus(int id)
        {
            var containerStatus = await _context.ContainerStatus.FindAsync(id);

            if (containerStatus == null)
            {
                return NotFound();
            }

            return containerStatus;
        }

        // PUT: api/ContainerStatus/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContainerStatus(int id, ContainerStatus containerStatus)
        {
            if (id != containerStatus.ContainerStatusId)
            {
                return BadRequest();
            }

            _context.Entry(containerStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContainerStatusExists(id))
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

        // POST: api/ContainerStatus
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ContainerStatus>> PostContainerStatus(ContainerStatus containerStatus)
        {
            _context.ContainerStatus.Add(containerStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContainerStatus", new { id = containerStatus.ContainerStatusId }, containerStatus);
        }

        // DELETE: api/ContainerStatus/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContainerStatus>> DeleteContainerStatus(int id)
        {
            var containerStatus = await _context.ContainerStatus.FindAsync(id);
            if (containerStatus == null)
            {
                return NotFound();
            }

            _context.ContainerStatus.Remove(containerStatus);
            await _context.SaveChangesAsync();

            return containerStatus;
        }

        private bool ContainerStatusExists(int id)
        {
            return _context.ContainerStatus.Any(e => e.ContainerStatusId == id);
        }
    }
}
