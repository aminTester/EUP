using System.Net.Http.Json;
using BlazorWasmShared.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace BlazorWasmClient.Services
{
    public class ProfessorService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;
        private readonly IJSRuntime _js;


        public ProfessorService(HttpClient http, string baseUrl,IJSRuntime js)
        {
            _http = http;
            this._baseUrl = baseUrl;
            this._js = js;
        }
        private async Task<string> GetUserKey()
        {
            return await _js.InvokeAsync<string>("getCookie", "accessKey");
        }

        public async Task<List<Professor>> GetProfessorsByCountry(string? country)
        {
            var userKey = await GetUserKey();
            if (string.IsNullOrEmpty(userKey))
            {
                Console.WriteLine("No access key found!");
                return new List<Professor>();
            }

            _http.DefaultRequestHeaders.Remove("X-Access-Key");
            _http.DefaultRequestHeaders.Add("X-Access-Key", userKey);


            string url = string.IsNullOrEmpty(country) ? "api/professors" : $"api/professors/country/{country}";
            return await _http.GetFromJsonAsync<List<Professor>>(url) ?? new List<Professor>();
        }

        public async Task<Professor?> GetProfessorById(int id)
        {
            return await _http.GetFromJsonAsync<Professor>($"api/professors/{id}");
        }

        public async Task UpdateProfessor(Professor professor)
        {
            professor.UpdateDate = DateTime.UtcNow; // Ensure UpdateDate is always in UTC
            if (professor.EmailDate!=null)
            {
                professor.EmailDate = professor.EmailDate.ToUniversalTime(); // Convert to UTC
            }

            var response = await _http.PutAsJsonAsync($"{_baseUrl}api/professors/{professor.Id}", professor);
            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to update professor: {errorMsg}");
            }
        }

        public async Task<string> ExportProfessorsToExcel()
        {
            var userKey = await GetUserKey();
            if (string.IsNullOrEmpty(userKey))
            {
                Console.WriteLine("No access key found!");
                return "Not Authenticated";
            }

            _http.DefaultRequestHeaders.Remove("X-Access-Key");
            _http.DefaultRequestHeaders.Add("X-Access-Key", userKey);

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



        public async Task<bool> ImportProfessorsFromExcel(IBrowserFile file)
        {
            var userKey = await GetUserKey();
            if (string.IsNullOrEmpty(userKey))
            {
                Console.WriteLine("No access key found!");
                return false;
            }

            _http.DefaultRequestHeaders.Remove("X-Access-Key");
            _http.DefaultRequestHeaders.Add("X-Access-Key", userKey);

            if (file == null)
            {
                Console.WriteLine("No file selected.");
                return false;
            }

            try
            {
                var content = new MultipartFormDataContent();
                var fileStream = file.OpenReadStream(10485760); // 10MB max
                var fileContent = new StreamContent(fileStream);
                content.Add(fileContent, "file", file.Name);

                var response = await _http.PostAsync("api/professors/import", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("File imported successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error importing file: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during import: {ex.Message}");
                return false;
            }
        }


    }
}
