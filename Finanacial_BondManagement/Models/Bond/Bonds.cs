using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanacial_BondManagement.Models.Interests
{
    public class Bonds
    {
        public string bondID { get; set; }
        public double InterestRate { get; set; }
        public double MaturityRate { get; set; }
        public int Order { get; set; }
        // Numeric order 
    }
}
