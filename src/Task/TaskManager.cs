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
        var targetFolder = TaskService.Instance.GetFolder(parentFolderName);
        if (targetFolder == null)
        {
            return;
        }

        var tasks = targetFolder.EnumerateTasks(null, true).ToList();
        tasks.ForEach(task => task.Folder.DeleteTask(task.Name));

        var folders = targetFolder
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
