using System.Collections.Generic;

namespace FluentComparator.Models
{
    public class ComparisonResult
    {
        public bool IsEquivalent { get; set; }
        public IEnumerable<Difference> Differences { get; set; }
    }
}
