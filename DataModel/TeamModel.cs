using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class TeamModel
    {
        public int MatchID { get; set; }
        public DateTime MatchDate { get; set; }
        public string TeamOne { get; set; }
        public string TeamTwo { get; set; }
        public int TeamOneID { get; set; }
        public int TeamTwoID { get; set; }
        public int TeamOneBet { get; set; }
        public int TeamTwoBet { get; set; }
        public int BettorID { get; set; }
    }
}
