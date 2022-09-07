namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    public string GenerateEntity(string projectName, FileSchema schema)
    {
        var builder = new StringBuilder();

        _ = builder.AppendLine($"namespace {projectName};");
        _ = builder.AppendLine($"");
        _ = builder.AppendLine($"public class {schema.Name}");
        _ = builder.AppendLine($"{{");

        foreach (var item in schema.Items)
        {
            _ = builder.AppendLine($"    [DisplayName(\"{item.DisplayName}\")]");
            _ = builder.AppendLine($"    public {item.Type} {item.Name} {{ get; set; }}");
        }

        _ = builder.AppendLine($"}}");

        return builder.ToString();
    }
}
