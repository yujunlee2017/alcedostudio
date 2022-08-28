namespace Alcedosoft.AlcedoStudio;

public class RemoveSchemaCommand : Command
{
    private readonly Workspace _workspace;
    private readonly SchemaHandler _handler;

    public RemoveSchemaCommand(Workspace workspace)
    {
        _workspace = workspace;
        _handler = new(workspace);
    }

    public override async void Execute(object? parameter)
    {
        bool? result = await _workspace.DialogService.ShowMessageBox(
            "Warning",
            "Deleting can not be undone!",
            yesText: "Delete",
            cancelText: "Cancel");

        if (result is true && _workspace.SelectedSchema is not null)
        {
            _workspace.Schemas.Remove(_workspace.SelectedSchema);

            _workspace.StateHasChanged();

            await _handler.DeleteAsync(_workspace.SelectedSchema);

            _workspace.Snackbar.Add("Schema Deleted", Severity.Success);
        }

        base.Execute(parameter);
    }
}
