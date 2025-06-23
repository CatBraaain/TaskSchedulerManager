using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

class Program
{
    static void Main(string[] args)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var yamlText = File.ReadAllText("tasks.yaml");
        var tasks = deserializer.Deserialize<List<TaskInput>>(yamlText);

        if (!PrivilegeManager.IsAdmin)
        {
            PrivilegeManager.RunAsAdmin(args);
            Environment.Exit(0);
        }

        TaskSchedulerManager.RemoveTasks("MyTasks");
        Console.ReadLine();
    }
}
