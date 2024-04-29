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

namespace Progres_Gui_Wpf
{
    /// <summary>
    /// Interaction logic for ProgresSearchControl.xaml
    /// </summary>
    public partial class ProgresSearchControl : UserControl
    {
        private MainWindowViewModel _vm;

        public ProgresSearchControl()
        {
            InitializeComponent();
            vm = new MainWindowViewModel();
        }

        public MainWindowViewModel vm
        {
            get => _vm;
            set
            {
                _vm = value;
                DataContext = vm;
            } 
        }

        private async void ButtonRun_OnClick(object sender, RoutedEventArgs e)
        {
            await vm.ActionButtonCallback();
        }
    }
}
