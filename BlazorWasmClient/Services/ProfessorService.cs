using System.Net.Http.Json;
using BlazorWasmShared.Models;

namespace BlazorWasmClient.Services
{
    public class ProfessorService
    {
        private readonly HttpClient _http;

        public ProfessorService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Professor>> GetProfessors()
        {
            return await _http.GetFromJsonAsync<List<Professor>>("api/professors");
        }

        public async Task<Professor> GetProfessor(int id)
        {
            return await _http.GetFromJsonAsync<Professor>($"api/professors/{id}");
        }

        public async Task<bool> AddProfessor(Professor professor)
        {
            var response = await _http.PostAsJsonAsync("api/professors", professor);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProfessor(int id, Professor professor)
        {
            var response = await _http.PutAsJsonAsync($"api/professors/{id}", professor);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProfessor(int id)
        {
            var response = await _http.DeleteAsync($"api/professors/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
