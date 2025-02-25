using BlazorWasmAPI.Data;
using BlazorWasmShared.Enum;
using BlazorWasmShared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

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

        [HttpGet("country/{country}")]
        public async Task<ActionResult<IEnumerable<Professor>>> GetProfessorsByCountry(string country)
        {
            var professors = await _context.Professors.Where(p => p.Country == Enum.Parse<CountryCode>(country)).ToListAsync();
            return Ok(professors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Professor>> GetProfessorById(int id)
        {
            var professor = await _context.Professors.FindAsync(id);
            if (professor == null)
                return NotFound();
            return professor;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfessor(Professor professor)
        {
            var existingProfessor = await _context.Professors.FindAsync(professor.Id);
            if (existingProfessor == null)
                return NotFound();

            existingProfessor.Text = professor.Text;
            existingProfessor.Related = professor.Related;
            existingProfessor.EmailDate = professor.EmailDate;
            existingProfessor.Result = professor.Result;
            existingProfessor.UpdateDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportProfessorsToExcel()
        {
            var professors = await _context.Professors.ToListAsync();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Professors");
            var headers = typeof(Professor).GetProperties().Select(p => p.Name).ToArray();

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            for (int i = 0; i < professors.Count; i++)
            {
                var values = professors[i].GetType().GetProperties().Select(p => p.GetValue(professors[i])?.ToString()).ToArray();
                for (int j = 0; j < values.Length; j++)
                {
                    worksheet.Cells[i + 2, j + 1].Value = values[j];
                }
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Professors.xlsx");
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportProfessorsFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;
            var properties = typeof(Professor).GetProperties();

            var importedProfessors = new List<Professor>();
            for (int row = 2; row <= rowCount; row++) // Start from row 2 (skip headers)
            {
                var professor = new Professor();
                for (int col = 1; col <= properties.Length; col++)
                {
                    var property = properties[col - 1];
                    var value = worksheet.Cells[row, col].Value?.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        property.SetValue(professor, Convert.ChangeType(value, property.PropertyType));
                    }
                }
                importedProfessors.Add(professor);
            }

            _context.Professors.AddRange(importedProfessors);
            await _context.SaveChangesAsync();

            return Ok("File imported successfully.");
        }
    }
}
