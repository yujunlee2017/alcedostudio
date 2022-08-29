namespace Alcedosoft.AlcedoStudio;

public class GenerateCodeCommand : Command
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
                string name = schema.Name;
                string namePlural = $"{name}s";

                string entityName = $"{name}.cs";
                string dataContext = $"{name}Context.cs";
                string controller = $"{name}Controller.cs";

                var currentDirectory = await projectDirectory
                    .GetDirectoryHandleAsync(namePlural, new() { Create = true });

                var entityHandle = await currentDirectory.GetFileHandleAsync(entityName, new() { Create = true });
                var dataContextHandle = await currentDirectory.GetFileHandleAsync(dataContext, new() { Create = true });
                var controllerHandle = await currentDirectory.GetFileHandleAsync(controller, new() { Create = true });


            }
        }

        base.Execute(parameter);
    }
}
