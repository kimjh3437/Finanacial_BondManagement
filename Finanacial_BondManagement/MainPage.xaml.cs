using Finanacial_BondManagement.ViewModels.CalculationsVM;

namespace Finanacial_BondManagement;

public partial class MainPage : ContentPage
{

    public CalculationViewModel _bindingContext; 
	public MainPage()
	{
		InitializeComponent();
        var vm = new CalculationViewModel();
        vm.LoadData();
        _bindingContext = vm;
        this.BindingContext = _bindingContext; 
	}

    private void TargetAverageInterestRateEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var model = sender as Entry;
        int numericValue;
        bool isNumber =  int.TryParse(model.Text, out numericValue);
        if (isNumber)
        {

        }
    }

    private void MaturityRateEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var model = sender as Entry;
        int numericValue;
        bool isNumber = int.TryParse(model.Text, out numericValue);
        if (isNumber)
        {

        }
    }

    private void InterestRateEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var model = sender as Entry;
        int numericValue;
        bool isNumber = int.TryParse(model.Text, out numericValue);
        if (isNumber)
        {

        }
    }

    private async void AddBond_Button_Clicked(object sender, EventArgs e)
    {
        int n, t, s; 
        bool x = int.TryParse(InterestRateEntry.Text, out n);
        bool y = int.TryParse(MaturityRateEntry.Text, out t);
        if(x && y)
        {
            var result = await _bindingContext.AddBondType(n, t);
            if (result != null)
            {
                InterestRateEntry.Text = string.Empty;
                MaturityRateEntry.Text = string.Empty;
            }
        }
        bool z = int.TryParse(InterestRateEntry.Text, out s);
    }
}

