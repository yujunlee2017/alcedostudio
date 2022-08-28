namespace Alcedosoft.AlcedoStudio;

public class NewSchemaCommand : Command
{
    private readonly Workspace _workspace;
    private readonly SchemaHandler _handler = new();

    public NewSchemaCommand(Workspace workspace)
    {
        _workspace = workspace;
    }

    public override async void Execute(object? parameter)
    {
        if (_workspace.DirectoryHandle is null)
        {
            return;
        }

        var dialog = _workspace.DialogService.Show<NewSchemaDialog>("Create New Schame");

        if (await dialog.Result is { Cancelled: false } reuslt && reuslt.Data is FileSchema schema)
        {
            await _handler.SaveAsync(_workspace.DirectoryHandle, schema);

            _workspace.Schemas.Add(schema);

            _workspace.SelectedSchema = schema;

            _workspace.StateHasChanged();
        }

        base.Execute(parameter);
    }
}
