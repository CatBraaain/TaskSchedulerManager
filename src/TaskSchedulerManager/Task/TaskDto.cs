using Microsoft.Win32.TaskScheduler;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class TaskDto
{
    public TaskInput Input { get; set; }
    public TaskDefinition Definition { get; set; }

    public string TaskPath => $@"MyTasks\{Input.Triggers[0].Type}\{Input.Name}";

    public TaskDto(TaskInput input, TaskDefinition definition)
    {
        Input = input;
        Definition = definition;
    }

    public static List<TaskDto> BuildTaskDtos(string yamlPath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        var yamlText = File.ReadAllText(yamlPath);
        var taskInputs = deserializer.Deserialize<List<TaskInput>>(yamlText) ?? [];

        return taskInputs
            .Select(input => new TaskDto(input, TaskBuilder.BuildTask(input)))
            .ToList();
    }
}
