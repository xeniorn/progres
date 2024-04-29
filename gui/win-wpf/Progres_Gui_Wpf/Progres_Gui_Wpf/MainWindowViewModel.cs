using System.CodeDom;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Windows;
using PropertyChanged;

namespace Progres_Gui_Wpf;


[AddINotifyPropertyChangedInterface]
public class MainWindowViewModel
{
    public MainWindowViewModel()
    {
        SelectedSearchDbOption = SearchDbOptions.Last();
        SwitchToState(State.WaitingToRun);

#if DEBUG
        InitDebugData();
#endif

    }

#if DEBUG
    private void InitDebugData()
    {
        SelectedSearchDbOption = SearchDbOptions.First();
        SearchInputs.Add(new ProgresSearchInputContainer(){Path = @"C:\\temp\\progres\\1ubq.pdb" });
    }
#endif

    private void SwitchToState(State state)
    {
        switch (state)
        {
            case State.WaitingToRun:
                ActionButtonText = "Run";
                ActionButtonCallback = async () =>
                {
                    SwitchToState(State.Running);
                    await RunSearch();
                    SwitchToState(State.WaitingToRun);
                };
                break;
            case State.Running:
                ActionButtonText = "Cancel";
                ActionButtonCallback = () =>
                {
                    SwitchToState(State.Cancelling);
                    return Task.CompletedTask;
                };
                break;
            case State.Cancelling:
                ActionButtonCallback = () => Task.CompletedTask;
                CancellationSource?.Cancel();
                ActionButtonText = "Run";
                break;
            case State.Unknown:
            default:
                throw new NotImplementedException();
        }
    }

    public Func<Task> ActionButtonCallback { get; set; }

    private CancellationTokenSource? CancellationSource { get; set; }


    public string DownloadedModelsPath { get; set; } = Path.Join(Path.GetTempPath(), "ProgresData");
    public List<string> SearchDbOptions { get; set; } = ["scope95", "scope40", "cath40", "ecod70", "af21org", "afted"];
    public string SelectedSearchDbOption { get; set; }
    public string InputPath { get; set; }
    
    public async Task RunSearch()
    {
        var existingPaths = SearchInputs
            .Where(x => x.Exists)
            .Select(x => x.Path)
            .ToList();

        if (existingPaths.Count == 0)
        {
            MessageBox.Show("None of the input paths exist");
            return;
        }

        CancellationSource = new CancellationTokenSource();
        var token = CancellationSource.Token;

        //TODO: make it just queue jobs
        var runner = new ProgresRunner();

        var exitCode = int.MinValue;

        SearchOutput = string.Empty;

        await foreach (var chunk in runner.RunSearchProcessAsync(existingPaths,
                           SelectedSearchDbOption,
                           DownloadedModelsPath,
                           DockerImageName,
                           ProgresSearchExtraArgs,
                           token).WithCancellation(token))
        {
            switch (chunk.Type)
            {
                case RunProcessFullAsyncChunkType.ErrorStream or RunProcessFullAsyncChunkType.OutputStream:
                    SearchOutput += chunk.Content;
                    break;
                case RunProcessFullAsyncChunkType.ExitCode:
                    exitCode = int.Parse(chunk.Content);
                    if (exitCode == ProcessRunner.CancelledProcessExitCode)
                    {
                        SearchOutput += "\n\n########### Task was cancelled! ###########\n\n";
                    }
                    break;
                default:
                    throw new NotImplementedException($"Chunk type {chunk.Type} is not implemented.");
            }
        }
    }

    public string SearchOutput { get; set; } = string.Empty;

    public string DockerImageName { get; set; } = "xeniorn/progres";
    public string ProgresSearchExtraArgs { get; set; } = string.Empty;
    public ObservableCollection<ProgresSearchInputContainer> SearchInputs { get; } = [];
    public string ActionButtonText { get; set; } = "Uninitialized";
    public ProgresWpfToolTips ToolTips { get; } = new ();
    public string Title { get; } = $"Progres UI";
}