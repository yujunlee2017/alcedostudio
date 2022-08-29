namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    public string GenerateEntity(string projectName, FileSchema schema)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"namespace {projectName};");
        builder.AppendLine($"");
        builder.AppendLine($"public class {schema.Name}");
        builder.AppendLine($"{{");

        foreach (var item in schema.Items)
        {
            builder.AppendLine($"    [DisplayName(\"{item.DisplayName}\")]");
            builder.AppendLine($"    public {item.Type} {item.Name} {{ get; set; }}");
        }

        builder.AppendLine($"}}");

        return builder.ToString();
    }
}
