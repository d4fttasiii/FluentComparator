namespace FluentComparator.Interfaces
{
    public interface IComparatorInit<T>
        where T : class
    {
        /// <summary>
        /// Sets the first (A) object
        /// </summary>
        /// <param name="objectA"></param>
        /// <returns></returns>
        IComparatorFirstAdded<T> Compare(T objectA);
    }
}
