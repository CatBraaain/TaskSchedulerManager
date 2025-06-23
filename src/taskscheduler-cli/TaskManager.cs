using Microsoft.Win32.TaskScheduler;

public class TaskSchedulerManager
{
    public static void SyncTasks(List<TaskDefinition> tasks, string parentFolderName)
    {
        RemoveTasks(parentFolderName);
        AddTasks([tasks[0]]);
    }

    public static void RemoveTasks(string parentFolderName)
    {
        var parentFolder = TaskService.Instance.GetFolder(parentFolderName);

        var tasks = parentFolder.EnumerateTasks(null, true).ToList();
        tasks.ForEach(task => task.Folder.DeleteTask(task.Name));

        var folders = parentFolder
            .EnumerateFolders(null)
            .OrderByDescending(f => f.Path, StringComparer.Ordinal)
            .ToList();
        folders.ForEach(folder =>
            (folder.Parent ?? TaskService.Instance.RootFolder).DeleteFolder(folder.Name)
        );
    }

    public static void AddTasks(List<TaskDefinition> tasks)
    {
        TaskService.Instance.RootFolder.RegisterTaskDefinition(
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
