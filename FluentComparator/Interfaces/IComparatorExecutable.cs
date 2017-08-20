using FluentComparator.Models;

namespace FluentComparator.Interfaces
{
    public interface IComparatorExecutable<T>
        where T: class
    {
        /// <summary>
        /// Returns the comparison result with the list of differences if there are any.
        /// </summary>
        /// <returns></returns>
        ComparisonResult Evaluate();
    }
}
