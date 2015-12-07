using System.Collections.Generic;

namespace Util.Parsers
{
    public interface IParser
    {
        IEnumerable<T> Parse<T>() where T : new();
    }
}
