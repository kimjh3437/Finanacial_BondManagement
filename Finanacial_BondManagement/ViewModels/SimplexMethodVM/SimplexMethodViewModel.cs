using Finanacial_BondManagement.Models.Interests;
using Finanacial_BondManagement.Models.Variable;
using Finanacial_BondManagement.ViewModels.BaseVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanacial_BondManagement.ViewModels.SimplexMethodVM
{
    public class SimplexMethodViewModel : BaseViewModel
    {
        public SimplexMethodViewModel()
        {

        }


        //______________________________________
        //
        // Initialize Methods 
        //______________________________________
        public async Task LoadData()
        {
            await InitializeCollections();
        }
        public async Task InitializeCollections()
        {

        }

        //______________________________________
        //
        // Method / Event handlers  
        //______________________________________


        //______________________________________
        //
        // Models / Observable Collections  
        //______________________________________
        public ObservableCollection<Bonds> Bonds { get; set; }

        //Constraints 
        public ObservableCollection<Variables> Constraints_One { get; set; }
        public ObservableCollection<Variables> Constraints_Two { get; set; }
    }


   
}
