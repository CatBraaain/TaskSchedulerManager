using Microsoft.Win32.TaskScheduler;

public class TaskSchedulerManager
{
    public static void RemoveTasks(string parentFolderName)
    {
        using var taskService = new TaskService();
        var parentFolder = taskService.GetFolder(parentFolderName);

        var folders = parentFolder
            .EnumerateFolders(null)
            .OrderByDescending(f => f.Path, StringComparer.Ordinal)
            .ToList();

        foreach (var folder in folders)
        {
            foreach (var task in folder.EnumerateTasks(null, false))
            {
                folder.DeleteTask(task.Name);
            }

            var parent = folder.Parent ?? taskService.RootFolder;
            parent.DeleteFolder(folder.Name);
        }
    }
}
