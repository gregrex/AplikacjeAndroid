using Microsoft.Maui.Storage;

namespace AppFactory.Mobile.Services;

public sealed class ProjectToolStateService
{
    public int GetRowCount(string projectId) => Preferences.Default.Get(RowKey(projectId), 0);

    public void SetRowCount(string projectId, int value) =>
        Preferences.Default.Set(RowKey(projectId), Math.Max(0, value));

    public string GetNotes(string projectId) => Preferences.Default.Get(NotesKey(projectId), string.Empty);

    public void SetNotes(string projectId, string notes) =>
        Preferences.Default.Set(NotesKey(projectId), notes ?? string.Empty);

    public void Reset(string projectId)
    {
        Preferences.Default.Remove(RowKey(projectId));
        Preferences.Default.Remove(NotesKey(projectId));
    }

    private static string RowKey(string projectId) => $"project-tools:{projectId}:rows";
    private static string NotesKey(string projectId) => $"project-tools:{projectId}:notes";
}
