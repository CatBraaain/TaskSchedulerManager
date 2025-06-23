using Microsoft.Win32.TaskScheduler;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

class Program
{
    static void Main(string[] args)
    {
        PrivilegeManager.EnsureAsAdmin(args); // need admin right for editing admin's task
        var taskDtos = BuildTaskDtos("tasks.yaml");
        TaskSchedulerManager.SyncTasks(taskDtos, "MyTasks");
        // Console.ReadLine();
    }

    public static List<TaskDto> BuildTaskDtos(string yamlPath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        var yamlText = File.ReadAllText(yamlPath);
        var taskInputs = deserializer.Deserialize<List<TaskInput>>(yamlText);

        return taskInputs
            .Select(input => new TaskDto(input, TaskBuilder.BuildTask(input)))
            .ToList();
    }
}
