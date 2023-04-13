using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class TimeSheetTableDto
    {
        public TimeSheetEntryDto[] Entries { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
    }
}
