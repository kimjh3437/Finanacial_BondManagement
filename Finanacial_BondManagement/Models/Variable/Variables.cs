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
        public double Variable { get; set; } // indicates the value of the variable e.g 0.2, 5, 6.1, etc
        public int VarNum { get; set; } // indicates the number of variables e.g x_2, x_3, x_4, etc 
        public int Sign { get; set; }
        public string VariableName { get; set; }
        // 0 = equality =
        // 1 = greater than  > 
        // -1 = less than  < 
        // 2 = null 

        public string bondID { get; set; }
        // Utility 
        public bool IsBasic { get; set; }
        public bool IsRegular { get; set; }
        public bool IsSlack { get; set; }
        public bool IsArtificial { get; set; }

        // only applicable for RHS variables 
    }
}
