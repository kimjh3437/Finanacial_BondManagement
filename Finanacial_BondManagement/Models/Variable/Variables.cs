using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanacial_BondManagement.Models.Variable
{
    public class Variables
    {
        // Main Contents 
        public double Variable { get; set; }
        public int VarNum { get; set; }
        public int Sign { get; set; }
        // 0 = equality =
        // 1 = greater than  > 
        // -1 = less than  < 
        // 2 = null 

        // Utility 
        public bool IsBasic { get; set; }
        public bool IsRegular { get; set; }
        public bool IsSlack { get; set; }
        public bool IsArtificial { get; set; }

        // only applicable for RHS variables 
    }
}
