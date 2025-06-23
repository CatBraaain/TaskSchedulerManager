using Microsoft.Win32.TaskScheduler;

public class TaskSchedulerManager
{
    public static void SyncTasks(List<TaskDto> taskDtos, string parentFolderName)
    {
        RemoveTasks(parentFolderName);
        AddTasks(taskDtos);
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

    public static void AddTasks(List<TaskDto> taskDtos)
    {
        TaskService.Instance.RootFolder.RegisterTaskDefinition(
            taskDtos[0].TaskPath,
            taskDtos[0].Definition,
            TaskCreation.CreateOrUpdate,
            null,
            null,
            taskDtos[0].Definition.Principal.LogonType,
            null
        );
    }
}
