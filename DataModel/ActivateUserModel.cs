using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class ActivateUserModel
    {
        public string UserEmail { get; set; }
        public string ActivationCode { get; set; }
        public bool IsUserActive { get; set; }
    }
}
