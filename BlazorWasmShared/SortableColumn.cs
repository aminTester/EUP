using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWasmShared
{

    public class SortableColumn
    {
        public string PropertyName { get; }
        public string DisplayName { get; }
        public SortDirection? SortOrder { get; set; }

        public SortableColumn(string propertyName)
        {
            PropertyName = propertyName;
            DisplayName = propertyName;
        }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}

