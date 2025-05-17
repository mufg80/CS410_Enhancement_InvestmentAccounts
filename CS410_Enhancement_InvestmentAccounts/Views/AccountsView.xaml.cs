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
    /// </summary>
    public static class EnumValues
    {
        public static Array Options => Enum.GetValues(typeof(Option));
    }
    /// <summary>
    /// Interaction logic for AccountsView.xaml
    /// </summary>
    public partial class AccountsView : UserControl
    {
        public AccountsViewModel viewModel { get; set; }
        public AccountsView()
        {
            viewModel = new AccountsViewModel();
            this.DataContext = viewModel;

            InitializeComponent();
            var f = mydatagrid.DataContext;
            var g = mydatagrid.ItemsSource;
            mydatagrid.DataContext = viewModel;
            mydatagrid.ItemsSource = viewModel.Model.Models;

        }

    }

}
