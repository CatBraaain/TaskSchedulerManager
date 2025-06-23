class Program
{
    static void Main(string[] args)
    {
        PrivilegeManager.EnsureAsAdmin(args);
        TaskSchedulerManager.SyncTasks("MyTasks");
        // Console.ReadLine();
    }
}
