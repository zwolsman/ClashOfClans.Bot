using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Util
{
    internal static class Extensions
    {
        internal static byte[] Reversed(this byte[] b)
        {
            Array.Reverse(b);
            return b;
        }
    }
}
