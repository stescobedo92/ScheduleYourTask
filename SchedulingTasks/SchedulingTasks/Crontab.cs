using System.Diagnostics;

namespace SchedulingTasks
{
    public static class Crontab
    {
        public static string GetCrontab()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"crontab -l\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    string result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while getting the crontab list", ex);
            }
        }

        public static void EditCrontab()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"crontab -e\"",
                    UseShellExecute = false
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while editing the crontab", ex);
            }
        }

        public static void AddTask(string task)
        {
            if (string.IsNullOrWhiteSpace(task))
                throw new ArgumentException("Task cannot be null or empty", nameof(task));

            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"echo '{task}' | crontab -e\"",
                    UseShellExecute = false
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the task to crontab", ex);
            }
        }

        public static void RemoveTask(string taskIdentifier)
        {
            if (string.IsNullOrWhiteSpace(taskIdentifier))
                throw new ArgumentException("Task identifier cannot be null or empty", nameof(taskIdentifier));

            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"crontab -l | grep -v '{taskIdentifier}' | crontab -\"",
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

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

        public static void EnableDisableTask(string taskIdentifier, bool enable)
        {
            if (string.IsNullOrWhiteSpace(taskIdentifier))
                throw new ArgumentException("Task identifier cannot be null or empty", nameof(taskIdentifier));

            try
            {
                string command = enable ? $"crontab -l | sed '/# {taskIdentifier}/ s/^# //g' | crontab -" :
                    $"crontab -l | sed '/{taskIdentifier}/ s/^/# /g' | crontab -";

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command}\"",
                    UseShellExecute = false
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while enabling/disabling the task in crontab", ex);
            }
        }

        public static string ListTasksWithDetails()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"crontab -l | while read line; do next_run=$(echo $line | awk '{print $1,$2,$3,$4,$5}' | crontab -n); echo $line - Next Run: $next_run; done\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    string result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while listing tasks with details from crontab", ex);
            }
        }

        public static void ExportTasks(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"crontab -l > {filePath}\"",
                    UseShellExecute = false
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while exporting tasks from crontab", ex);
            }
        }

        public static void ImportTasks(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"crontab {filePath}\"",
                    UseShellExecute = false
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while importing tasks to crontab", ex);
            }
        }
        
        public static void TrackCrontabChanges()
        {
            try
            {
                // Validate if the directory exists, if not create it
                string historyDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".crontab_history");
                if (!Directory.Exists(historyDir))
                {
                    Directory.CreateDirectory(historyDir);
                }

                // Validate if the crontab command is available
                var checkCrontab = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"command -v crontab\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

                using (var process = new Process { StartInfo = checkCrontab })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0 || string.IsNullOrWhiteSpace(output))
                    {
                        throw new InvalidOperationException($"Crontab command not found: {error}");
                    }
                }

                // Execute the crontab backup command
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"crontab -l > {historyDir}/$(date +\\%Y\\%m\\%d\\%H\\%M\\%S)_crontab.bak\"",
                    UseShellExecute = false
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new InvalidOperationException("An error occurred while backing up the crontab list");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while tracking crontab changes", ex);
            }
        }
    }
}

