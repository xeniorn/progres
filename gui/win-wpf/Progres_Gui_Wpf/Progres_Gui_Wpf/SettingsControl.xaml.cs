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
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        private MainWindowViewModel _vm;

        public SettingsControl()
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
    }
}
