using Microsoft.Win32.TaskScheduler;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class TaskSchedulerManager
{
    public static void SyncTasks(string parentFolderName)
    {
        RemoveTasks(parentFolderName);

        using var taskService = new TaskService();
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        var yamlText = File.ReadAllText("tasks.yaml");
        var taskInputs = deserializer.Deserialize<List<TaskInput>>(yamlText);
        var taskBuilder = new TaskBuilder(taskService);
        var task = taskBuilder.BuildTask(taskInputs[0]);
        AddTasks([task]);
    }

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
