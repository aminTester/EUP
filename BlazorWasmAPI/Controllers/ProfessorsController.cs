using BlazorWasmAPI.Data;
using BlazorWasmShared.Enum;
using BlazorWasmShared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorWasmAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfessorsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{country}")]
        public async Task<ActionResult<IEnumerable<Professor>>> GetProfessorsByCountry(string country)
        {
            var professors = await _context.Professors.Where(p => p.Country == Enum.Parse<CountryCode>(country)).ToListAsync();
            return Ok(professors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Professor>> GetProfessor(int id)
        {
            var professor = await _context.Professors.FindAsync(id);
            if (professor == null)
                return NotFound();
            return professor;
        }

        [HttpPost]
        public async Task<ActionResult<Professor>> CreateProfessor(Professor professor)
        {
            _context.Professors.Add(professor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProfessor), new { id = professor.Id }, professor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfessor(int id, Professor professor)
        {
            if (id != professor.Id)
                return BadRequest();

            _context.Entry(professor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfessor(int id)
        {
            var professor = await _context.Professors.FindAsync(id);
            if (professor == null)
                return NotFound();

            _context.Professors.Remove(professor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
