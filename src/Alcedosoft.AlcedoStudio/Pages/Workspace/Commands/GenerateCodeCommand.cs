namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    private readonly Workspace _workspace;
    private readonly SolutionHandler _handler;

    public GenerateCodeCommand(Workspace workspace)
    {
        _workspace = workspace;
        _handler = new(workspace);
    }

    public override async void Execute(object? parameter)
    {
        var projectDirectory = await _handler.GetProjectDirectoryAsync();

        if (projectDirectory is not null)
        {
            foreach (var schema in _workspace.Schemas)
            {
                string entityName = $"{schema.Name}.cs";
                string contextName = $"{schema.Name}Context.cs";
                string controllerName = $"{schema.Name}Controller.cs";

                var currentDirectory = await projectDirectory
                    .GetDirectoryHandleAsync($"{schema.Name}s", new() { Create = true });

                var entityHandle = await currentDirectory
                    .GetFileHandleAsync(entityName, new() { Create = true });
                var entityWriter = await entityHandle
                    .CreateWritableAsync(new() { KeepExistingData = false });
                string entityContent = this.GenerateEntity(projectDirectory.Name, schema);
                await entityWriter.WriteAsync(entityContent);
                await entityWriter.CloseAsync();

                var contextHandle = await currentDirectory
                    .GetFileHandleAsync(contextName, new() { Create = true });
                var contextWriter = await contextHandle
                    .CreateWritableAsync(new() { KeepExistingData = false });
                string contextContent = this.GenerateContext(projectDirectory.Name, schema);
                await contextWriter.WriteAsync(contextContent);
                await contextWriter.CloseAsync();

                var controllerHandle = await currentDirectory
                    .GetFileHandleAsync(controllerName, new() { Create = true });
                var controllerWriter = await controllerHandle
                    .CreateWritableAsync(new() { KeepExistingData = false });
                string contollerContent = this.GenerateController(projectDirectory.Name, schema);
                await controllerWriter.WriteAsync(contollerContent);
                await controllerWriter.CloseAsync();

            }
        }

        base.Execute(parameter);
    }
}
