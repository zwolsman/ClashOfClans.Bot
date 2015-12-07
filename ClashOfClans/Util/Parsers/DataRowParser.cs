using System.Collections.Generic;

namespace Util.Parsers
{
    public abstract class DataRowParser : IParser
    {
        public IEnumerable<T> Parse<T>() where T : new()
        {
            var list = new List<T>();
            for (var row = 0; row < RowCount; ++row)
                list.Add(ParseRow<T>(row));

            return list;
        }

        protected abstract int RowCount { get; }

        protected abstract T ParseRow<T>(int row) where T : new();
    }
}
