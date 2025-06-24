using Cocona;

CoconaApp.Run<CLI>();

public class CLI
{
    public record GlobalParameters(
        [Option("path", Description = "yaml file path to read")] string Path = "tasks.yaml",
        [Option("mount", Description = "task folder path to mount")]
            string Mount = "TaskSchedulerManager"
    ) : ICommandParameterSet;

    public void Apply(GlobalParameters globalParams)
    {
        PrivilegeManager.EnsureAsAdmin(); // need admin right for editing admin's task
        var taskDtos = TaskDto.BuildTaskDtos(globalParams.Path);
        TaskSchedulerManager.ApplyTasks(taskDtos, globalParams.Mount);
    }

    public void Diff(GlobalParameters globalParams)
    {
        Console.WriteLine("diff command not supported yet");
    }
}
