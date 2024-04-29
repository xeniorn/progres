using System.Windows;
using System.Windows.Controls;

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

        private void InstallDocker_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"You should run in powershell: \n" +
                            $"winget install --id=Docker.DockerDesktop\n" +
                            $"And probably add yourself to docker-users local group (in lusrmgr.msc)");
        }
    }
}
