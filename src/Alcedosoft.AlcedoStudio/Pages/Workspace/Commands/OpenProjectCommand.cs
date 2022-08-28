namespace Alcedosoft.AlcedoStudio;

public class OpenProjectCommand : Command
{
    private readonly Workspace _workspace;
    private readonly SchemaHandler _handler = new();

    public OpenProjectCommand(Workspace workspace)
    {
        _workspace = workspace;
    }

    public async override void Execute(object? parameter)
    {
        var options = new DirectoryPickerOptionsStartInWellKnownDirectory
        {
            StartIn = WellKnownDirectory.Desktop,
        };

        var directory = await _workspace.FileSystemService.ShowDirectoryPickerAsync(options);

        var state = await directory.RequestPermission(
            new() { Mode = FileSystemPermissionMode.ReadWrite });

        if (state is PermissionState.Granted)
        {
            try
            {
                _workspace.IsLoading = true;

                _workspace.DirectoryHandle = directory;

                _workspace.StateHasChanged();

                await ResolveSchema(directory, _workspace.Schemas);

                await ResolveSolution(directory, _workspace.FileSystemItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                _workspace.IsLoading = false;

                _workspace.StateHasChanged();
            }
        }
    }

    public async Task LoadDirectory(FileSystemDirectoryHandle directory)
    {
        await ResolveSchema(directory, _workspace.Schemas);

        await ResolveSolution(directory, _workspace.FileSystemItems);
    }

    private async Task ResolveSchema(FileSystemDirectoryHandle directory, HashSet<FileSchema> schemas)
    {
        foreach (var schema in await _handler.GetSchemasAsync(directory))
        {
            schemas.Add(schema);
        }
    }

    private async Task ResolveSolution(FileSystemDirectoryHandle directory, HashSet<FileSystemItem> items)
    {
        var handles = await directory.ValuesAsync();

        foreach (var handle in handles)
        {
            FileSystemItem? item;

            if (handle.Kind == FileSystemHandleKind.File)
            {
                var file = await directory.GetFileHandleAsync(handle.Name);

                item = new FileSystemFileItem(file);
            }
            else
            {
                if (handle.Name is ".vs" or ".vscode" or "obj" or "bin" or "node_modules")
                {
                    continue;
                }

                var subDirectory = await directory.GetDirectoryHandleAsync(handle.Name);

                item = new FileSystemFolderItem(subDirectory);

                await ResolveSolution(subDirectory, item.Items);
            }

            _ = items.Add(item);
        }
    }
}
