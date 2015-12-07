using System.Collections.Generic;

namespace ClashOfClans.Util.Parsers
{
    public abstract class DataRowParser : IParser
    {
        protected abstract int RowCount { get; }

        public IEnumerable<T> Parse<T>() where T : new()
        {
            var list = new List<T>();
            for (var row = 0; row < RowCount; ++row)
                list.Add(ParseRow<T>(row));

            return list;
        }

        protected abstract T ParseRow<T>(int row) where T : new();
    }
}