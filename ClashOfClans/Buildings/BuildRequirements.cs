using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClashOfClans.Logic;
using ClashOfClans.Util.Parsers;

namespace ClashOfClans.Buildings
{
    internal sealed class BuildRequirements
    {
        [SpreadSheetParse]
        public string Name { get; set; }

        //[SpreadSheetParse]
        //public TimeSpan BuildTime { get; set; }

        //[SpreadSheetParse]
        //public Resource BuildResource { get; set; }

        //[SpreadSheetParse]
        //public int BuildCost { get; set; }

        [SpreadSheetParse(ColumnNames = new [] { "BuildTimeD", "BuildTimeH", "BuildTime" })]
        public TimeSpan BuildTime { get; set; }
    }
}
