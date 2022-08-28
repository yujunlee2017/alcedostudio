namespace Alcedosoft.AlcedoStudio;

public class SchemaHandler
{
    public async Task SaveAsync(FileSchema schema)
    {
        if (schema.Handle is not null)
        {
            var writer = await schema.Handle
                .CreateWritableAsync(new() { KeepExistingData = false });

            await writer.WriteAsync(JsonSerializer.Serialize(schema));

            await writer.CloseAsync();
        }
    }

    public async Task SaveAsync(
        FileSystemDirectoryHandle directory, FileSchema schema)
    {
        if (schema.Handle is null)
        {
            var schemaDirectory = await this.GetDirectoryAsync(directory);

            if (schemaDirectory is not null)
            {
                var file = await schemaDirectory.GetFileHandleAsync(
                    $"{schema.Name}.schema", new() { Create = true });

                schema.Handle = file;
            }
        }

        await this.SaveAsync(schema);
    }

    public async Task<FileSchema[]> GetSchemasAsync(FileSystemDirectoryHandle directory)
    {
        var schemas = new List<FileSchema>();

        var schemaDirectory = await this.GetDirectoryAsync(directory);

        if (schemaDirectory is not null)
        {
            foreach (var handle in await schemaDirectory.ValuesAsync())
            {
                if (handle.Kind is FileSystemHandleKind.File && handle.Name.EndsWith(".schema"))
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

    private async Task<FileSystemDirectoryHandle?> GetDirectoryAsync(FileSystemDirectoryHandle directory)
    {
        var fileSystems = await directory.ValuesAsync();

        var projectDirectory = fileSystems
            .FirstOrDefault(x => x.Kind is FileSystemHandleKind.Directory);

        if (projectDirectory is not null)
        {
            var projectHandle = await directory.GetDirectoryHandleAsync(projectDirectory.Name);

            var propertiesHandle = await projectHandle
                .GetDirectoryHandleAsync("Properties", new() { Create = true });

            var schemasHandle = await propertiesHandle
                .GetDirectoryHandleAsync("Schemas", new() { Create = true });

            return schemasHandle;
        }

        return null;
    }
}
