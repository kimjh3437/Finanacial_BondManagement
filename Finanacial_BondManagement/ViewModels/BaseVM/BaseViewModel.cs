using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Finanacial_BondManagement.ViewModels.BaseVM
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        //______________________________________
        //
        // Initialize Methods 
        //______________________________________
        public async Task LoadData()
        {

        }

        //______________________________________
        //
        // Content Methods 
        //______________________________________


        //______________________________________
        //
        // Binding  Models 
        //______________________________________
    }
}
