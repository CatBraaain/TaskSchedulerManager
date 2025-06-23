using System.Diagnostics;
using System.Security.Principal;

public static class PrivilegeManager
{
    public static bool IsAdmin
    {
        get
        {
            using var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }

    public static void RunAsAdmin(string[]? args = null)
    {
        var psi = new ProcessStartInfo
        {
            FileName = Process.GetCurrentProcess().MainModule!.FileName!,
            Verb = "runas",
            UseShellExecute = true,
            Arguments = args is null ? "" : string.Join(" ", args),
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
