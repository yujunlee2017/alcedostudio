namespace Alcedosoft.AlcedoStudio;

public class GenerateCodeCommand : Command
{
    private readonly Workspace _workspace;

    public GenerateCodeCommand(Workspace workspace)
    {
        _workspace = workspace;
    }

    public override void Execute(object? parameter)
    {

        base.Execute(parameter);
    }
}
