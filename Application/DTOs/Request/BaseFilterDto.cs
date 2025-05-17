using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class BaseFilterDto
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string? SearchTerm { get; set; }
        public List<SortOption>? SortOptions { get; set; }
    }

    public class SortOption
    {
        public string Column { get; set; } = string.Empty;
        public bool Descending { get; set; }
    }
}
