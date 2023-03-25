using System.Diagnostics;

namespace SchedulingTasks;

public static class Crontab
{
    /// <summary>
    /// Code to get the current task list in the crontab
    /// </summary>
    public static string GetCrontab()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = "-c \"crontab -l\"",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        var process = new Process
        {
            StartInfo = processStartInfo
        };

        process.Start();
        string result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return result;
    }

    /// <summary>
    /// Code to open the crontab editor and allow adding new tasks
    /// </summary>
    public static void EditCrontab()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = "-c \"crontab -e\"",
            UseShellExecute = false
        };

        var process = new Process
        {
            StartInfo = processStartInfo
        };

        process.Start();
        process.WaitForExit();
    }
}

