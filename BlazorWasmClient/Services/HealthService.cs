namespace BlazorWasmClient.Services
{
    public class HealthService
    {
        private readonly HttpClient _httpClient;
        public HealthService (HttpClient httpClient)
        { _httpClient = httpClient; }
        public async Task PingApiAsync()
        {
            try
            {
                await _httpClient.GetAsync("https://eup-server.onrender.com/api/health");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"health check failed : { ex.Message }");
            }
        }
    }
}
