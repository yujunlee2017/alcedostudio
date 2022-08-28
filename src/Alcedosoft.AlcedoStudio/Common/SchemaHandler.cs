namespace Alcedosoft.AlcedoStudio;

public class SchemaHandler
{
    private const string EXTENSION = ".schema";
    private const string PROPETIES = "Properties";
    private const string SCHEMADIR = "Schemas";

    private readonly Workspace _workspace;

    public SchemaHandler(Workspace workspace)
    {
        _workspace = workspace;
    }

    public async Task SaveAsync(FileSchema schema)
    {
        if (schema.Handle is null && _workspace.DirectoryHandle is not null)
        {
            var schemaDirectory = await this.GetSchemaDirectoryAsync(_workspace.DirectoryHandle);

            if (schemaDirectory is not null)
            {
                var file = await schemaDirectory.GetFileHandleAsync(
                    $"{schema.Name}{EXTENSION}", new() { Create = true });

                schema.Handle = file;
            }
        }

        if (schema.Handle is not null)
        {
            var writer = await schema.Handle
                .CreateWritableAsync(new() { KeepExistingData = false });

            await writer.WriteAsync(JsonSerializer.Serialize(schema));

            await writer.CloseAsync();
        }
    }

    public async Task DeleteAsync(FileSchema schema)
    {
        if (_workspace.DirectoryHandle is not null)
        {
            var schemaDirectory = await this.GetSchemaDirectoryAsync(_workspace.DirectoryHandle);

            if (schemaDirectory is not null)
            {
                await schemaDirectory.RemoveEntryAsync($"{schema.Name}{EXTENSION}");
            }
        }
    }

    public async Task<FileSchema[]> GetSchemasAsync(FileSystemDirectoryHandle directory)
    {
        var schemas = new List<FileSchema>();

        var schemaDirectory = await this.GetSchemaDirectoryAsync(directory);

        if (schemaDirectory is not null)
        {
            foreach (var handle in await schemaDirectory.ValuesAsync())
            {
                if (handle.Kind is FileSystemHandleKind.File && handle.Name.EndsWith(EXTENSION))
                {
                    var fileHandle = await schemaDirectory.GetFileHandleAsync(handle.Name);

                    var reader = await fileHandle.GetFileAsync();

                    string schemaText = await reader.TextAsync();

                    var schema = JsonSerializer.Deserialize<FileSchema>(schemaText);

                    if (schema is not null)
                    {
                        schema.Handle = fileHandle;

                        schemas.Add(schema);
                    }
                }
            }
        }

        return schemas.ToArray();
    }

    private async Task<FileSystemDirectoryHandle?> GetSchemaDirectoryAsync(FileSystemDirectoryHandle directory)
    {
        var handler = new SolutionHandler();

        var projectDirectory = await handler.GetProjectDirectoryAsync(directory);

        if (projectDirectory is not null)
        {
            var propertiesHandle = await projectDirectory
                .GetDirectoryHandleAsync(PROPETIES, new() { Create = true });

            var schemasHandle = await propertiesHandle
                .GetDirectoryHandleAsync(SCHEMADIR, new() { Create = true });

            return schemasHandle;
        }

        return null;
    }
}
