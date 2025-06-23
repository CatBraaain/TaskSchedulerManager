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

    public static void AddTasks(List<TaskDefinition> tasks)
    {
        using var taskService = new TaskService();
        taskService.RootFolder.RegisterTaskDefinition(
            "MyTasks\\test",
            tasks[0],
            TaskCreation.CreateOrUpdate,
            null,
            null,
            // task.Principal.LogonType,
            TaskLogonType.InteractiveToken,
            null
        );
    }
}
