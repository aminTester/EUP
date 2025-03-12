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
            var requestKey = Request.Headers["X-Access-Key"].ToString();
            if (string.IsNullOrWhiteSpace(requestKey) || requestKey != "EA6664")
            {
                return Unauthorized("Invalid access key.");
            }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfessor(int id, Professor professor)
        {
            if (id != professor.Id)
            {
                return BadRequest("Professor ID mismatch.");
            }

            // Ensure DateTime fields are stored in UTC
            professor.UpdateDate = DateTime.UtcNow;

            if (professor.EmailDate != null)  // Only convert if it's not null
            {
                professor.EmailDate = professor.EmailDate.ToUniversalTime();
            }

            _context.Entry(professor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfessorExists(id))
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

        private bool ProfessorExists(int id)
        {
            // Check if the professor exists in the database
            return _context.Professors.Any(p => p.Id == id);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportProfessorsToExcel()
        {
            var requestKey = Request.Headers["X-Access-Key"].ToString();
            if (string.IsNullOrWhiteSpace(requestKey) || requestKey != "EA6664")
            {
                return Unauthorized("Invalid access key.");
            }

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
        public async Task<IActionResult> ImportProfessorsFromExcel([FromForm] IFormFile file)
        {
            var requestKey = Request.Headers["X-Access-Key"].ToString();
            if (string.IsNullOrWhiteSpace(requestKey) || requestKey != "EA6664")
            {
                return Unauthorized("Invalid access key.");
            }


            if (file == null || file.Length == 0)
            {
                return BadRequest("No file selected or invalid file type.");
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];

            if (worksheet == null)
            {
                return BadRequest("Invalid file format.");
            }

            var rowCount = worksheet.Dimension.Rows;
            var importedProfessors = new List<Professor>();

            for (int row = 2; row <= rowCount; row++) // Skip header row
            {
                if (worksheet.Cells[row, 1].Value == null) continue; // Skip empty rows

                var professor = new Professor
                {
                    FullName = worksheet.Cells[row, 1].Value?.ToString(),
                    Keywords = worksheet.Cells[row, 2].Value?.ToString(),
                    Wos = worksheet.Cells[row, 3].Value?.ToString(),
                    Web = worksheet.Cells[row, 4].Value?.ToString(),
                    Email = worksheet.Cells[row, 5].Value?.ToString(),
                    Text = worksheet.Cells[row, 6].Value?.ToString(),
                    University = worksheet.Cells[row, 7].Value?.ToString(),
                    Papers = int.TryParse(worksheet.Cells[row, 8].Value?.ToString(), out int papers) ? papers : 0,
                    Related = bool.TryParse(worksheet.Cells[row, 9].Value?.ToString(), out bool related) ? related : false,
                    Result = Enum.TryParse<ResultType>(worksheet.Cells[row, 10].Value?.ToString(), out var result) ? result : null,
                    Country = Enum.TryParse<CountryCode>(worksheet.Cells[row, 11].Value?.ToString(), out var country) ? country : null,
                    EmailDate = DateTime.TryParse(worksheet.Cells[row, 12].Value?.ToString(), out var emailDate) ? emailDate : DateTime.UtcNow,
                    UpdateDate = DateTime.TryParse(worksheet.Cells[row, 13].Value?.ToString(), out var updateDate) ? updateDate : DateTime.UtcNow
                };

                importedProfessors.Add(professor);
            }

            await _context.Professors.AddRangeAsync(importedProfessors);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "File imported successfully.", Count = importedProfessors.Count });
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchProfessors([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Search term cannot be empty.");

            string lowerName = name.ToLower();

            var professors = await _context.Professors
                .Where(p => EF.Functions.Like(p.FullName.ToLower(), $"%{lowerName}%"))
                .ToListAsync();

            return Ok(professors);
        }
    }
}
