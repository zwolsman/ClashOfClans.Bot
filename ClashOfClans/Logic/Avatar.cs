using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashOfClans.Logic
{
    public class Avatar
    {
        public long ID;
        public int TownHallLevel { get; set; }
        public string Username { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Gems { get; set; }
        public int Trophies { get; set; }
        public int AttacksWon { get; set; }
        public int AttacksLost { get; set; }
        public int DefensesWon { get; set; }
        public int DefensesLost { get; set; }

        public Clan Clan { get; set; }
    }
}
