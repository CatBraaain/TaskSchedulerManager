using Microsoft.Win32.TaskScheduler;

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
}
