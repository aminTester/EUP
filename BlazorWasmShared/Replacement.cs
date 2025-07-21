using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWasmShared
{
    public class EmailBatchUpdateDto
    {
        public Dictionary<string, string> Replacements { get; set; } = new();
    }
}
