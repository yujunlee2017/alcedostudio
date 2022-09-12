namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    private readonly Workspace _workspace;
    private readonly OpenProjectCommand _openCommand;

    public GenerateCodeCommand(Workspace workspace)
    {
        _workspace = workspace;
        _openCommand = new OpenProjectCommand(workspace);
    }

    public override async void Execute(object? parameter)
    {
        if (_workspace.DirectoryHandle is not null)
        {
            var alcedostudio = await _workspace.DirectoryHandle
                .GetDirectoryHandleAsync(".alcedostudio", new() { Create = true });

            var template = await alcedostudio
                .GetFileHandleAsync(".template", new(){ Create = true});

            var reader = await template.GetFileAsync();

            var templateName = await reader.TextAsync();

            if (templateName is "abp_efcore_blazor")
            {
                await GenerateAbpEFCoreBlazorAsync(_workspace.DirectoryHandle);
            }
        }

        base.Execute(parameter);
    }

    private async Task GenerateAbpEFCoreBlazorAsync(FileSystemDirectoryHandle solutionDirectory)
    {
        foreach (var schema in _workspace.Schemas)
        {
            var entityName = $"{schema.Name}.cs";
            var contextName = $"{schema.Name}Context.cs";
            _ = $"{schema.Name}Controller.cs";

            var currentDirectory = await projectDirectory
                    .GetDirectoryHandleAsync($"{schema.Name}s", new() { Create = true });

            var entityHandle = await currentDirectory
                    .GetFileHandleAsync(entityName, new() { Create = true });
            var entityWriter = await entityHandle
                    .CreateWritableAsync(new() { KeepExistingData = false });
            var entityContent = GenerateEntity(projectDirectory.Name, schema);
            await entityWriter.WriteAsync(entityContent);
            await entityWriter.CloseAsync();

            var contextHandle = await currentDirectory
                    .GetFileHandleAsync(contextName, new() { Create = true });
            var contextWriter = await contextHandle
                    .CreateWritableAsync(new() { KeepExistingData = false });
            var contextContent = GenerateContext(projectDirectory.Name, schema);
            await contextWriter.WriteAsync(contextContent);
            await contextWriter.CloseAsync();

            //var controllerHandle = await currentDirectory
            //    .GetFileHandleAsync(controllerName, new() { Create = true });
            //var controllerWriter = await controllerHandle
            //    .CreateWritableAsync(new() { KeepExistingData = false });
            //var contollerContent = GenerateController(projectDirectory.Name, schema);
            //await controllerWriter.WriteAsync(contollerContent);
            //await controllerWriter.CloseAsync();
        }

        _ = _workspace.Snackbar.Add("Code Generated", Severity.Success);

        if (_workspace.DirectoryHandle is not null)
        {
            await _openCommand.LoadDirectory(_workspace.DirectoryHandle);
        }
    }
}
