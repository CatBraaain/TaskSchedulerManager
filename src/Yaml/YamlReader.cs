using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class YamlReader
{
    public static List<TaskInput> ReadYaml(string yamlPath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        var yamlText = File.ReadAllText(yamlPath);
        var taskInputs = deserializer.Deserialize<List<TaskInput>>(yamlText) ?? [];
        return taskInputs;
    }
}
