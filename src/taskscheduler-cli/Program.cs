using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

class Program
{
    static void Main(string[] args)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        var yamlText = File.ReadAllText("tasks.yaml");
        var taskInputs = deserializer.Deserialize<List<TaskInput>>(yamlText);

        PrivilegeManager.EnsureAsAdmin(args);

        TaskSchedulerManager.RemoveTasks("MyTasks");
        // Console.ReadLine();
    }
}
