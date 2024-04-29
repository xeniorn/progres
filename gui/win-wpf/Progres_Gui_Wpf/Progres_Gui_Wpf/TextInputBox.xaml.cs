using System.Windows;
using System.Windows.Controls;

namespace Progres_Gui_Wpf
{
    /// <summary>
    ///     Interaction logic for TextInputBox.xaml
    /// </summary>
    public partial class TextInputBox : UserControl
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string),
            typeof(TextInputBox),
            new FrameworkPropertyMetadata { BindsTwoWayByDefault = false, DefaultValue = default(string) });

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string),
            typeof(TextInputBox),
            new FrameworkPropertyMetadata { BindsTwoWayByDefault = true, DefaultValue = default(string) });


        public TextInputBox()
        {
            InitializeComponent();
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}