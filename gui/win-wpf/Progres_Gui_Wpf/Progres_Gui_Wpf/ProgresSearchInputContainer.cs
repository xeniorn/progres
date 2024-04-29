using System.IO;
using PropertyChanged;

namespace Progres_Gui_Wpf;

[AddINotifyPropertyChangedInterface]
public class ProgresSearchInputContainer
{
    public string Path { get; set; } = null!;
    public bool Exists => !string.IsNullOrWhiteSpace(Path) && File.Exists(Path);
}