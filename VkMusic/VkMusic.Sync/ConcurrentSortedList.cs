using System.Collections.Generic;

namespace VkMusicSync
{
    // TODO think of optimizations
    public class ConcurrentSortedList<TKey, TValue>
    {
        private readonly SortedList<TKey, TValue> sortedList = new SortedList<TKey, TValue>();

        public void Add(TKey key, TValue value)
        {
            lock (sortedList)
                sortedList.Add(key, value);
        }

        public void AddRange(SortedList<TKey,TValue> list)
        {
            lock (sortedList)
            {
                foreach (var item in list)
                {
                    sortedList.Add(item.Key, item.Value);
                }     
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (sortedList)
                    return sortedList[key];
            }
            set
            {
                lock (sortedList)
                    sortedList[key] = value;
            }
        }

        // TODO change to enumerator or something else, cause Values could be changed by user
        public IList<TValue> Values
            => sortedList.Values;

        public int Count
            => sortedList.Count;






    }
}
