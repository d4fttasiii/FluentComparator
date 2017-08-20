namespace FluentComparator.Interfaces
{
    public interface IComparatorFirstAdded<T>
        where T : class
    {
        /// <summary>
        /// Sets the second (B) object
        /// </summary>
        /// <param name="objectB"></param>
        /// <returns></returns>
        IComparatorSecondAdded<T> To(T objectB);
    }
}
