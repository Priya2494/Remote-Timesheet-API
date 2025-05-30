using Microsoft.AspNetCore.Mvc;
using TimesheetAPI.Models;
using TimesheetAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TimesheetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimesheetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TimesheetController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimesheetEntry>>> GetAll() =>
            await _context.TimesheetEntries.ToListAsync();

        [HttpPost]
        public async Task<ActionResult<TimesheetEntry>> Create(TimesheetEntry entry)
        {
            _context.TimesheetEntries.Add(entry);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, entry);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TimesheetEntry>> GetById(int id)
        {
            var entry = await _context.TimesheetEntries.FindAsync(id);
            return entry is null ? NotFound() : Ok(entry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TimesheetEntry updated)
        {
            if (id != updated.Id) return BadRequest();
            _context.Entry(updated).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entry = await _context.TimesheetEntries.FindAsync(id);
            if (entry is null) return NotFound();
            _context.TimesheetEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("paged-deferred")]
        public IActionResult GetPagedDeferred(int page = 1, int pageSize = 5)
        {
            var query = _context.TimesheetEntries
                .OrderByDescending(t => t.Date);  // IQueryable → IEnumerable

            var paged = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return Ok(paged); // LINQ query is executed here
        }
        [HttpGet("paged-immediate")]
        public IActionResult GetPagedImmediate(int page = 1, int pageSize = 5)
        {
            var allEntries = _context.TimesheetEntries
                .OrderByDescending(t => t.Date)
                .ToList(); // executes immediately

            var paged = allEntries
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return Ok(paged);
        }

    }
}
