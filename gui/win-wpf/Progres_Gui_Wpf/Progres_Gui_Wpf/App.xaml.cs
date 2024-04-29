using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace Progres_Gui_Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Something happened we don't know how to handle:\n\n {e.Exception}");
            e.Handled = true;
        }
    }

}
