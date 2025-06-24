public class TaskInput
{
    public string Name { get; set; } = string.Empty;
    public List<TriggerInput> Triggers { get; set; } = new();
    public List<ActionInput> Actions { get; set; } = new();
    public SettingsInput? Settings { get; set; }
}

public class TriggerInput
{
    public string Type { get; set; } = null!; // "cron" | "startup" | "boot" | "once" | "atNow"
    public string Value { get; set; } = null!;
}

public class ActionInput
{
    public string Command { get; set; } = null!;
    public string? Args { get; set; }
    public string? WorkingDirectory { get; set; }
}

public class SettingsInput
{
    public int? RunLevel { get; set; }
    public string? LogonType { get; set; } // "S4U" | "LogonType" | "InteractiveToken" | string
}
