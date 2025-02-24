using System.Text.Json.Serialization;

namespace BlazorWasmShared.Models
{
    public class Professor
    {
        public int Id { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Department { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
