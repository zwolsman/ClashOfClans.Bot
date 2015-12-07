using System.Collections.Generic;

namespace ClashOfClans.Util.Parsers
{
    public interface IParser
    {
        IEnumerable<T> Parse<T>() where T : new();
    }
}
