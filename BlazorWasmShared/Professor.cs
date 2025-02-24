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
        public DateTime? EmailDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public ResultType? Result { get; set; }
        public CountryCode? Country { get; set; }

    }
}
