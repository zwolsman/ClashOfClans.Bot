using System;
using System.Collections.Concurrent;

namespace ClashOfClans.Util
{
    internal sealed class Pool<T>
    {
        private readonly Func<T> _generator;
        private readonly ConcurrentBag<T> _internalBag;

        public Pool() : this(Activator.CreateInstance<T>)
        {
        }

        public Pool(Func<T> generator)
        {
            _internalBag = new ConcurrentBag<T>();
            _generator = generator;
        }

        public T Get()
        {
            T item;
            return _internalBag.TryTake(out item) ? item : _generator();
        }

        public void Put(T item)
        {
            _internalBag.Add(item);
        }
    }
}