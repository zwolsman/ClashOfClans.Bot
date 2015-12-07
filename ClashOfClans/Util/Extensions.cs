using System;

namespace ClashOfClans.Util
{
    internal static class Extensions
    {
        internal static byte[] Reversed(this byte[] b)
        {
            Array.Reverse(b);
            return b;
        }

        internal static string RemoveSpaces(this string input)
            => input.Replace(" ", "");
    }
}