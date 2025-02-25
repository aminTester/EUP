using System.Net.Http.Json;
using BlazorWasmShared.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorWasmClient.Services
{
    public class ProfessorService
    {
        private readonly HttpClient _http;

        public ProfessorService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Professor>> GetProfessorsByCountry(string? country)
        {
            string url = string.IsNullOrEmpty(country) ? "api/professors" : $"api/professors/country/{country}";
            return await _http.GetFromJsonAsync<List<Professor>>(url) ?? new List<Professor>();
        }

        public async Task<Professor?> GetProfessorById(int id)
        {
            return await _http.GetFromJsonAsync<Professor>($"api/professors/{id}");
        }

        public async Task UpdateProfessor(Professor professor)
        {
            await _http.PutAsJsonAsync("api/professors", professor);
        }

        public async Task<string> ExportProfessorsToExcel()
        {
            var response = await _http.GetAsync("api/professors/export");
            if (response.IsSuccessStatusCode)
            {
                var fileName = "Professors.xlsx";
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var fileUrl = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{Convert.ToBase64String(fileBytes)}";

                return fileUrl;
            }
            return string.Empty;
        }

        public async Task ImportProfessorsFromExcel(IBrowserFile file)
        {
            var content = new MultipartFormDataContent();
            var fileStream = file.OpenReadStream(10485760); // 10MB max
            var fileContent = new StreamContent(fileStream);
            content.Add(fileContent, "file", file.Name);

            await _http.PostAsync("api/professors/import", content);
        }
    }
}
