namespace BlazorWasmShared.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
