using System.Windows;

namespace Progres_Gui_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowViewModel();
            DataContext = vm;
            ProgresSearchControl.vm = vm;
            SettingsControl.vm = vm;
        }

        public MainWindowViewModel vm { get; set; }
    }
}