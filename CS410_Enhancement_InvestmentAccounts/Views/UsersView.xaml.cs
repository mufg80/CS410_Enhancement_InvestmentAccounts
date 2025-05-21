using CS410_Enhancement_InvestmentAccounts.ViewModels;
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
    /// Interaction logic for UsersView.xaml
    /// Codebehind initializes the viewmodel and sets the datacontext to the viewmodel. This allows
    /// the datagrid and buttons to bind to the viewmodel properties and commands.
    /// </summary>
    public partial class UsersView : UserControl
    {
        public UsersViewModel ViewModel { get; set; }
        public UsersView()
        {
            InitializeComponent();
            ViewModel = new UsersViewModel();
            this.DataContext = ViewModel;
            mydatagrid2.ItemsSource = ViewModel.Model.Models;
        }
    }
}
