using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var deserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();
var yamlText = File.ReadAllText("tasks.yaml");
var tasks = deserializer.Deserialize<List<Task>>(yamlText);

TaskSchedulerManager.RemoveTasks("MyTasks");
