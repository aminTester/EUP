using BlazorWasmShared.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlazorWasmShared.Models
{
    public class Professor
    {
        [Key]
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Keywords { get; set; }
        public string? Wos { get; set; }
        public string? Web { get; set; }
        public string? Email { get; set; }
        public string? Text { get; set; }
        public string? University { get; set; }
        public int Papers { get; set; }
        public bool Related { get; set; }
        public ResultType? Result { get; set; }
        public CountryCode? Country { get; set; }
        private DateTime _emailDate { get; set; }
        private DateTime _updateDate;
        public DateTime EmailDate
        {
            get => _emailDate;
            set => _emailDate = DateTime.SpecifyKind(value, DateTimeKind.Utc); // Ensure it's UTC
        }
        public DateTime UpdateDate
        {
            get => _updateDate;
            set => _updateDate = DateTime.SpecifyKind(value, DateTimeKind.Utc); // Ensure it's UTC
        }

    }
}
