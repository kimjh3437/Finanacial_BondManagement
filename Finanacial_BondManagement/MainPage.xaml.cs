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
    private void RatingEntry_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private async void AddBond_Button_Clicked(object sender, EventArgs e)
    {
        int n, t, s; 
        bool x = int.TryParse(InterestRateEntry.Text, out n);
        bool y = int.TryParse(MaturityRateEntry.Text, out t);
        bool q = int.TryParse(RatingEntry.Text, out s);
        if (x && y && q)
        {
            var result = await _bindingContext.AddBondType(n, t, s);
            if (result != null)
            {
                InterestRateEntry.Text = string.Empty;
                MaturityRateEntry.Text = string.Empty;
                RatingEntry.Text = string.Empty; 
            }
        }
        bool z = int.TryParse(InterestRateEntry.Text, out s);
    }

    private async void AverageMaturityRateEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var obj = sender as Entry;
        int n; 
        bool x = int.TryParse(obj.Text, out n);
        if (x)
        {
            await _bindingContext.ChangeAVGrates(1, n);
        }
    }

    private async void AverageInterestRateEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var obj = sender as Entry;
        int n;
        bool x = int.TryParse(obj.Text, out n);
        if (x)
        {
            await _bindingContext.ChangeAVGrates(0, n);
        }
    }

    private async void Calcuate_Button_Clicked(object sender, EventArgs e)
    {
        var result = await _bindingContext.CheckBeforeGo();
        if (result)
        {
            await _bindingContext.InitiateSimplexMethod();
        }

    }

  
}

