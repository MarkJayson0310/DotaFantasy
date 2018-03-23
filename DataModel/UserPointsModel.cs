using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class UserPointsModel
    {
        public int UserID { get; set; }
        public int TotalPoints { get; set; }
        public int AvailableMatchPoints { get; set; }
        public int MatchID { get; set; }
    }
}
