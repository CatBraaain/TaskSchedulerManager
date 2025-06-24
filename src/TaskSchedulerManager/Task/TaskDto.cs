using Microsoft.Win32.TaskScheduler;

public class TaskDto
{
    public TaskInput Input { get; set; }
    public TaskDefinition Definition { get; set; }

    public TaskDto(TaskInput input, TaskDefinition definition)
    {
        Input = input;
        Definition = definition;
    }

    public string TaskPath => $@"MyTasks\{Input.Triggers[0].Type}\{Input.Name}";
}
