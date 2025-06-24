using Microsoft.Win32.TaskScheduler;

public class TaskSchedulerManager
{
    public static void ApplyTasks(List<TaskDto> taskDtos, string parentFolderName)
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
        taskDtos.ForEach(taskDto =>
            TaskService.Instance.RootFolder.RegisterTaskDefinition(
                taskDto.TaskPath,
                taskDto.Definition,
                TaskCreation.CreateOrUpdate,
                null,
                null,
                taskDto.Definition.Principal.LogonType,
                null
            )
        );
    }
}
