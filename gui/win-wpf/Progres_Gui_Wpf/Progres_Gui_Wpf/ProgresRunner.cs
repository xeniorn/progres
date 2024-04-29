using System.IO;

namespace Progres_Gui_Wpf;

public class ProgresRunner
{

    private string Q(string input)
    {
        return $"\"{input}\"";
    }

    public async Task<(bool success, string res)> RunSearchProcess(
        string queryPdbPath, 
        string searchDb, 
        string progresDataPath, 
        string dockerImage, 
        CancellationToken? cancellationToken = null)
    {
        var program = "docker";

        const string dataInDocker = @"/persist/progres";

        if (!Directory.Exists(progresDataPath)) Directory.CreateDirectory(progresDataPath);

        var argsStringDocker = $"run -v {progresDataPath}:{dataInDocker} -t {Q(dockerImage)}";
        var progresProgram = "progres";
        var argsStringProgres = $"search -q {Q(queryPdbPath)} -t {Q(searchDb)}";

        var argsString = $"{argsStringDocker} {progresProgram} {argsStringProgres}";

        var (exitCode, outdata) = await ProcessRunner.RunProcessAsync("docker", argsString, cancellationTokenInput: cancellationToken);

        const int successCode = 0;

        return (exitCode == successCode, string.Join("\n", outdata));
    }

    public async IAsyncEnumerable<RunProcessFullAsyncChunk> RunSearchProcessAsync(
        IEnumerable<string> queryPaths,
        string searchDb,
        string progresDataPath,
        string dockerImage,
        string extraArguments = "",
        CancellationToken? cancellationToken = null)
    {
        var queryList = queryPaths
            .Where(x=>!string.IsNullOrWhiteSpace(x))
            .DistinctBy(x=>x.ToUpper())
            .ToList();
        if (queryList.Count == 0) yield break;

        var program = "docker";

        const string dataInDocker = @"/persist/progres";

        if (!Directory.Exists(progresDataPath)) Directory.CreateDirectory(progresDataPath);

        string querySpecifier = null!;
        string extraMountSpecifier = null!;

        // naaa, always use lists
        if (queryList.Count == -1)
        {
            querySpecifier = $"-q {Q(queryList.Single())}";
            extraMountSpecifier = string.Empty;
        }
        else //count >1, progres expects an input that's a file containing a list of paths
        {
            var guid = Guid.NewGuid().ToString();
            
            var hostDir = Path.Join(Path.GetTempPath(), guid);
            Directory.CreateDirectory(hostDir);

            var queriesFileName = "queries.txt";
            var queriesFilePathHost = Path.Join(hostDir, queriesFileName);

            var containerDir = $"/temp_input_{guid}";
            var queriesFilePathContainer = $"{containerDir}/{queriesFileName}";

            querySpecifier = $"-l {Q(queriesFilePathContainer)}";
            extraMountSpecifier = $"-v {Q(hostDir)}:{Q(containerDir)}";

            foreach (var queryPath in queryList)
            {
                var adaptedSubdir = PathHelper.AdaptPathForUnix(Path.GetDirectoryName(queryPath));
                var fullDirPath = Path.Join(hostDir, adaptedSubdir);
                Directory.CreateDirectory(fullDirPath);

                var fileName = Path.GetFileName(queryPath);
                var finalPathHost = Path.Join(fullDirPath, fileName);
                var finalPathContainer = $"{containerDir}/{adaptedSubdir}/{fileName}";

                File.Copy(queryPath, finalPathHost);

                await File.AppendAllLinesAsync(queriesFilePathHost, [finalPathContainer]);
            }
        }

        var argsStringDocker = $"run {extraMountSpecifier} -v {progresDataPath}:{dataInDocker} -t {Q(dockerImage)}";
        var progresProgram = "progres";
        var argsStringProgres = $"search {querySpecifier} -t {Q(searchDb)} {extraArguments}";

        var argsString = $"{argsStringDocker} {progresProgram} {argsStringProgres}";

        var asyncEnum = ProcessRunner.RunProcessFullAsync("docker", argsString, cancellationTokenInput: cancellationToken);

        await foreach (var runProcessFullAsyncChunk in asyncEnum)
        {
            yield return runProcessFullAsyncChunk;
        }
    }


}

public static class PathHelper
{
    public static string? AdaptPathForUnix(string? path)
    {
        var adaptedPath = path?.Replace('\\', '/').Replace(":","");
        return adaptedPath;
    }
}