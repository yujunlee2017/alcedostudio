namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    public async Task GenerateModelAsync(
        ProjectName projectName, FileSystemDirectoryHandle src, FileSchema schema)
    {
        var schemaName  = new SchemaName(schema.Name);

        var domain = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Domain", new(){ Create = true});
        var contracts = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Application.Contracts", new(){ Create = true});

        var entityDir = await domain.GetDirectoryHandleAsync(
            schemaName.PluralPascalName, new(){ Create = true});
        var entityDtoDir = await contracts.GetDirectoryHandleAsync(
            schemaName.PluralPascalName, new(){ Create = true});

        var entityFile = await entityDir.GetFileHandleAsync(
            $"{schemaName.PascalName}.cs", new(){ Create = true });
        var queryDtoFile = await entityDtoDir.GetFileHandleAsync(
            $"{schemaName.PascalName}QueryDto.cs", new(){ Create = true });
        var createDtoFile = await entityDtoDir.GetFileHandleAsync(
            $"{schemaName.PascalName}CreateDto.cs", new(){ Create = true });
        var updateDtoFile = await entityDtoDir.GetFileHandleAsync(
            $"{schemaName.PascalName}UpdateDto.cs", new(){ Create = true });

        var entityContent = GenerateEntity(projectName, schema);
        var queryDtoContent = GenerateQueryDto(projectName, schema);
        var createDtoContent = GenerateCreateDto(projectName, schema);
        var updateDtoContent = GenerateUpdateDto(projectName, schema);

        await WriteTextAsync(entityFile, entityContent);
        await WriteTextAsync(queryDtoFile, queryDtoContent);
        await WriteTextAsync(createDtoFile, createDtoContent);
        await WriteTextAsync(updateDtoFile, updateDtoContent);
    }

    private string GenerateEntity(ProjectName projectName, FileSchema schema)
    {
        var builder = new StringBuilder();

        _ = builder.AppendLine($"namespace {projectName.Value};");
        _ = builder.AppendLine($"");
        _ = builder.AppendLine($"public class {schema.Name} : AuditedAggregateRoot<Guid>");
        _ = builder.AppendLine($"{{");

        foreach (var item in schema.Items)
        {
            _ = builder.AppendLine($"    public {item.DataType} {item.Name} {{ get; set; }}");
        }

        _ = builder.AppendLine($"}}");

        return builder.ToString();
    }

    private string GenerateQueryDto(ProjectName projectName, FileSchema schema)
    {
        var builder = new StringBuilder();

        _ = builder.AppendLine($"namespace {projectName.Value};");
        _ = builder.AppendLine($"");
        _ = builder.AppendLine($"public class {schema.Name}QueryDto : AuditedEntityDto<Guid>");
        _ = builder.AppendLine($"{{");

        foreach (var item in schema.Items)
        {
            _ = builder.AppendLine($"    [DisplayName(\"{item.DisplayName}\")]");
            _ = builder.AppendLine($"    public {item.DataType} {item.Name} {{ get; set; }}");
        }

        _ = builder.AppendLine($"}}");

        return builder.ToString();
    }

    private string GenerateCreateDto(ProjectName projectName, FileSchema schema)
    {
        var builder = new StringBuilder();

        _ = builder.AppendLine($"namespace {projectName.Value};");
        _ = builder.AppendLine($"");
        _ = builder.AppendLine($"public class {schema.Name}CreateDto : AuditedEntityDto<Guid>");
        _ = builder.AppendLine($"{{");

        foreach (var item in schema.Items)
        {
            _ = builder.AppendLine($"    [DisplayName(\"{item.DisplayName}\")]");
            _ = builder.AppendLine($"    public {item.DataType} {item.Name} {{ get; set; }}");
        }

        _ = builder.AppendLine($"}}");

        return builder.ToString();
    }

    private string GenerateUpdateDto(ProjectName projectName, FileSchema schema)
    {
        var builder = new StringBuilder();

        _ = builder.AppendLine($"namespace {projectName.Value};");
        _ = builder.AppendLine($"");
        _ = builder.AppendLine($"public class {schema.Name}UpdateDto : AuditedEntityDto<Guid>");
        _ = builder.AppendLine($"{{");

        foreach (var item in schema.Items)
        {
            _ = builder.AppendLine($"    [DisplayName(\"{item.DisplayName}\")]");
            _ = builder.AppendLine($"    public {item.DataType} {item.Name} {{ get; set; }}");
        }

        _ = builder.AppendLine($"}}");

        return builder.ToString();
    }
}
