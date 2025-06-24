using System.Diagnostics;
using System.Security.Principal;

public static class PrivilegeManager
{
    public static void EnsureAsAdmin()
    {
        if (!IsAdmin)
        {
            RunAsAdmin();
            Environment.Exit(0);
        }
    }

    public static bool IsAdmin
    {
        get
        {
            using var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }

    public static void RunAsAdmin()
    {
        var psi = new ProcessStartInfo
        {
            FileName = Process.GetCurrentProcess().MainModule!.FileName!,
            Verb = "runas",
            UseShellExecute = true,
            Arguments = Environment.GetCommandLineArgs().Skip(1).Aggregate((a, b) => a + " " + b),
        };

        try
        {
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to elevate: {ex.Message}");
        }
    }
}
