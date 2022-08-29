namespace Alcedosoft.AlcedoStudio;

public class SolutionHandler
{
    private readonly Workspace _workspace;

    public SolutionHandler(Workspace workspace)
    {
        _workspace = workspace;
    }

    public async Task<FileSystemDirectoryHandle?> GetProjectDirectoryAsync()
    {
        if (_workspace.DirectoryHandle is FileSystemDirectoryHandle directory)
        {
            var fileSystems = await directory.ValuesAsync();

            var solutionFile = fileSystems.FirstOrDefault(x => x.Name.EndsWith(".sln"));

            if (solutionFile is not null)
            {
                string projectName = Path.GetFileNameWithoutExtension(solutionFile.Name);

                if (fileSystems.Any(x => x is { Kind: FileSystemHandleKind.Directory } && x.Name == projectName))
                {
                    var projectDirectory = await directory.GetDirectoryHandleAsync(projectName);

                    return projectDirectory;
                }
            }
        }

        return null;
    }
}