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
}

