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
    /// Code to open the crontab editor and allow editing tasks
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

    /// <summary>
    /// Code to open the crontab editor and allow adding new tasks
    /// </summary>
    public static void AddTask(string task)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"echo '{task}' | crontab -e\"",
            UseShellExecute = false
        };

        var process = new Process
        {
            StartInfo = processStartInfo
        };

        process.Start();
        process.WaitForExit();
    }
    
    public static void RemoveTask(string taskIdentifier)
    {
        if (string.IsNullOrWhiteSpace(taskIdentifier))
            throw new ArgumentException("Task identifier cannot be null or empty", nameof(taskIdentifier));

        var processStartInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"crontab -l | grep -v '{taskIdentifier}' | crontab -\"",
            RedirectStandardError = true,
            UseShellExecute = false
        };

        try
        {
            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"Failed to remove task: {error}");
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while removing the task from crontab", ex);
        }
    }
}

