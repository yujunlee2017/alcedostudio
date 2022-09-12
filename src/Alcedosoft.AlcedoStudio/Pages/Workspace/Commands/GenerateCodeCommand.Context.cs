namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    public string GenerateContext(string projectName, FileSchema schema)
    {
        return $@"namespace {projectName};

public partial class DataContext
{{
    public DbSet<{schema.Name}> {schema.Name} {{ get; set; }} = default!;
}}
";
    }

    public string GenerateDataSeed(string projectName, FileSchema schema)
    {
        return String.Empty;
    }
}
