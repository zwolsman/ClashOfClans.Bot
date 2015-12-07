using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Data.Csv
{
    class CsvException : Exception
    {
        public CsvException()
        {
        }

        public CsvException(string message) : base(message)
        {
        }
    }
}
