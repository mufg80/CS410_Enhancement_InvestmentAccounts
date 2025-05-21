using CS410_Enhancement_InvestmentAccounts.Enums;
using CS410_Enhancement_InvestmentAccounts.ViewModels;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CS410_Enhancement_InvestmentAccounts.Views
{
    /// <summary>
    /// Interaction logic for AccountsView.xaml
    /// Static enumValues used by view to hold options in the combo boxes.
    /// </summary>
    public static class EnumValues
    {
        public static Array Options => Enum.GetValues(typeof(Option));
    }
    /// <summary>
    /// Interaction logic for AccountsView.xaml
    /// Codebehind initializes the viewmodel and sets the datacontext to the viewmodel. This allows
    /// the datagrid and buttons to bind to the viewmodel properties and commands.
    /// </summary>
    public partial class AccountsView : UserControl
    {
        public AccountsViewModel ViewModel { get; set; }
        public AccountsView()
        {
            ViewModel = new AccountsViewModel();
            this.DataContext = ViewModel;

            InitializeComponent();
            mydatagrid.DataContext = ViewModel;
            mydatagrid.ItemsSource = ViewModel.Model.Models;

        }

    }

}
