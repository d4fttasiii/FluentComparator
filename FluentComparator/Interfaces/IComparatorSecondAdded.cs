using System;
using System.Linq.Expressions;

namespace FluentComparator.Interfaces
{
    public interface IComparatorSecondAdded<T>
        where T : class
    {
        /// <summary>
        /// The given property will be ignored in the comparison process.
        /// Nested Object properties are currently not supported.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IComparatorSecondAdded<T> ExcludeProperty(Expression<Func<T, object>> expression);
        /// <summary>
        /// Differences between the objects will be stored and added to the comparison result.
        /// </summary>
        /// <returns></returns>
        IComparatorExecutable<T> EnableDifferences();
        /// <summary>
        /// The result set will not contain the differences even if there are any.
        /// </summary>
        /// <returns></returns>
        IComparatorExecutable<T> DisableDifferences();
    }
}
