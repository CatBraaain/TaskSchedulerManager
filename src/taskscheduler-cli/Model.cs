public class Task
{
    public string Name { get; set; } = string.Empty;
    public List<Trigger> Triggers { get; set; } = new();
    public List<Action> Actions { get; set; } = new();
    public Settings? Settings { get; set; }
}

public class Trigger
{
    public string Type { get; set; } = null!; // "cron" | "startup" | "boot" | "once" | "atNow"
    public string Value { get; set; } = null!;
}

public class Action
{
    public string Command { get; set; } = null!;
    public string? Args { get; set; }
    public string? WorkingDirectory { get; set; }
}

public class Settings
{
    public int? RunLevel { get; set; }
    public string? LogonType { get; set; } // "S4U" | "LogonType" | "InteractiveToken" | string
}
