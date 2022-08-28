namespace Alcedosoft.AlcedoStudio;

public class SolutionHandler
{
    public async Task<FileSystemDirectoryHandle?> GetProjectDirectoryAsync(FileSystemDirectoryHandle directory)
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

        return null;
    }
}