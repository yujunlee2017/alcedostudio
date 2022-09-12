namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    public string GenerateInterface(string projectName, FileSchema schema)
    {
        _ = schema.Name;

        _ = schema.Name.ToLower();

        return String.Empty;
    }

    public string GenerateImplement(string projectName, FileSchema schema)
    {
        return String.Empty;
    }

    public string GenerateAutoMapper(string projectName, FileSchema schema)
    {
        return String.Empty;
    }
}
