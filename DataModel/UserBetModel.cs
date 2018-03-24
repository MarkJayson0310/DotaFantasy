using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class UserBetModel
    {        
        public int BettorID { get; set; }
        public int MatchID { get; set; }
        public int TeamID { get; set; }
        public int TournamentID { get; set; }
        public int PlaceBet { get; set; }
    }
}
