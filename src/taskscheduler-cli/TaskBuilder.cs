using Microsoft.Win32.TaskScheduler;
using TaskAction = Microsoft.Win32.TaskScheduler.Action;

public class TaskBuilder
{
    public static TaskDefinition BuildTask(TaskInput taskInput)
    {
        var task = TaskService.Instance.NewTask();

        task.Actions.AddRange(BuildActions(taskInput));
        task.Triggers.AddRange(BuildTriggers(taskInput));

        return task;
    }

    private static IEnumerable<TaskAction> BuildActions(TaskInput taskInput)
    {
        return taskInput.Actions.Select(actionInput => new ExecAction(
            actionInput.Command,
            actionInput.Args,
            actionInput.WorkingDirectory ?? Path.GetDirectoryName(actionInput.Command)
        ));
    }

    private static IEnumerable<Trigger> BuildTriggers(TaskInput taskInput)
    {
        return taskInput.Triggers.SelectMany(triggerInput =>
            triggerInput.Type switch
            {
                "cron" => Trigger
                    .FromCronFormat(triggerInput.Value)
                    .Select(trigger =>
                    {
                        var now = DateTime.Now;
                        var start = trigger.StartBoundary;
                        if (start <= now)
                        {
                            trigger.StartBoundary = start.AddDays(1);
                        }
                        return trigger;
                    }),
                "startup" => [new LogonTrigger { Delay = TimeSpan.Parse(triggerInput.Value) }],
                "boot" => [new BootTrigger { Delay = TimeSpan.Parse(triggerInput.Value) }],
                "once" => [new TimeTrigger { StartBoundary = DateTime.Parse(triggerInput.Value) }],
                "instant" => [new RegistrationTrigger()],
                _ => throw new ArgumentException($"Unknown trigger type: {triggerInput.Type}"),
            }
        );
    }
}
