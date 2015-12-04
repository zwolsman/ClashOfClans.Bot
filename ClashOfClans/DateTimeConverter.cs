using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans
{
    internal class DateTimeConverter
    {
        private static DateTime UnixTime = new DateTime(1970, 1, 1);

        public static DateTime FromUnixTimestamp(double unixTimestamp)
        {
            return UnixTime.AddSeconds(unixTimestamp);
        }

        public static double ToUnixTimestamp(DateTime time)
        {
            return (time - UnixTime).TotalSeconds;
        }

        public static DateTime FromJavaTimestamp(double javaTimestamp)
        {
            return UnixTime.AddSeconds(javaTimestamp / 1000);
        }

        public static double ToJavaTimestamp(DateTime time)
        {
            return (time - UnixTime).TotalSeconds * 1000;
        }
    }
}
