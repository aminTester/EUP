using BlazorWasmShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorWasmShared
{
    [JsonSerializable(typeof(List<Professor>))]
    public partial class ProfessorJsonContext : JsonSerializerContext
    {
    }
}
