using System.Collections.Generic;
using ContactBook.Performance;

namespace ContactBook.Models
{
    public class PerformanceResultViewModel
    {
        public int RecordsCount { get; set; }

        public IEnumerable<PerformanceResult> RepositoriesResults { get; set; }
    }
}