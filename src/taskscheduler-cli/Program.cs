using Microsoft.Win32.TaskScheduler;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

class Program
{
    static void Main(string[] args)
    {
        PrivilegeManager.EnsureAsAdmin(args);
        var tasks = BuildTasks("tasks.yaml");
        TaskSchedulerManager.SyncTasks(tasks, "MyTasks");
        // Console.ReadLine();
    }

    public static IEnumerable<TaskDefinition> BuildTasks(string yamlPath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        var yamlText = File.ReadAllText(yamlPath);
        var taskInputs = deserializer.Deserialize<List<TaskInput>>(yamlText);

        return taskInputs.Select(taskInput => TaskBuilder.BuildTask(taskInput));
    }
}
