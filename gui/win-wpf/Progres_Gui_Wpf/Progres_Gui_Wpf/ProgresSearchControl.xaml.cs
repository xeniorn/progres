using System.Windows;
using System.Windows.Controls;

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
