class Program
{
    static void Main(string[] args)
    {
        PrivilegeManager.EnsureAsAdmin(args); // need admin right for editing admin's task
        var taskDtos = TaskDto.BuildTaskDtos("tasks.yaml");
        TaskSchedulerManager.SyncTasks(taskDtos, "MyTasks");
    }
}
