using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class TournamentMatchModel
    {
        public int TournamentID { get; set; }
        public string TournamentName { get; set; }
        public DateTime TournamentDate { get; set; }
       public List<MatchModel> Matches { get; set; }
    }
}
