using System.Diagnostics;

namespace SchedulingTasks
{
    /// <summary>
    /// Provides methods to interact with the system crontab for task scheduling.
    /// </summary>
    public static class Crontab
    {
        /// <summary>
        /// Executes a shell command and optionally captures the output and error streams.
        /// </summary>
        /// <param name="command">The shell command to execute.</param>
        /// <param name="redirectOutput">Indicates whether to capture the standard output stream.</param>
        /// <param name="redirectError">Indicates whether to capture the standard error stream.</param>
        /// <returns>The standard output of the command if captured; otherwise, an empty string.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the command execution fails and error capturing is enabled.</exception>
        private static string ExecuteCommand(string command, bool redirectOutput = true, bool redirectError = false)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = redirectOutput,
                RedirectStandardError = redirectError,
                UseShellExecute = false
            };

            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();

                string result = redirectOutput ? process.StandardOutput.ReadToEnd() : string.Empty;
                string error = redirectError ? process.StandardError.ReadToEnd() : string.Empty;

                process.WaitForExit();

                if (process.ExitCode != 0 && redirectError)
                {
                    throw new InvalidOperationException($"Command failed: {error}");
                }

                return result;
            }
        }

        /// <summary>
        /// Retrieves the current crontab configuration for the user.
        /// </summary>
        /// <returns>The crontab entries as a string.</returns>
        /// <exception cref="InvalidOperationException">Thrown if retrieving the crontab configuration fails.</exception>
        public static string GetCrontab()
        {
            try
            {
                return ExecuteCommand("crontab -l");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while getting the crontab list", ex);
            }
        }

        /// <summary>
        /// Opens the crontab editor for manual editing.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if opening the crontab editor fails.</exception>
        public static void EditCrontab()
        {
            try
            {
                ExecuteCommand("crontab -e", redirectOutput: false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while editing the crontab", ex);
            }
        }

        /// <summary>
        /// Adds a new task to the crontab.
        /// </summary>
        /// <param name="task">The crontab task entry to add.</param>
        /// <exception cref="ArgumentException">Thrown if the task is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if adding the task fails.</exception>
        public static void AddTask(string task)
        {
            if (string.IsNullOrWhiteSpace(task))
                throw new ArgumentException("Task cannot be null or empty", nameof(task));

            try
            {
                ExecuteCommand($"echo '{task}' | crontab -e", redirectOutput: false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while adding the task to crontab", ex);
            }
        }

        /// <summary>
        /// Removes a task from the crontab based on a unique identifier.
        /// </summary>
        /// <param name="taskIdentifier">The unique identifier of the task to remove.</param>
        /// <exception cref="ArgumentException">Thrown if the task identifier is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if removing the task fails.</exception>
        public static void RemoveTask(string taskIdentifier)
        {
            if (string.IsNullOrWhiteSpace(taskIdentifier))
                throw new ArgumentException("Task identifier cannot be null or empty", nameof(taskIdentifier));

            try
            {
                ExecuteCommand($"crontab -l | grep -v '{taskIdentifier}' | crontab -", redirectError: true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while removing the task from crontab", ex);
            }
        }

        /// <summary>
        /// Enables or disables a crontab task by commenting or uncommenting its entry.
        /// </summary>
        /// <param name="taskIdentifier">The unique identifier of the task to enable or disable.</param>
        /// <param name="enable">True to enable the task, false to disable it.</param>
        /// <exception cref="ArgumentException">Thrown if the task identifier is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if modifying the task fails.</exception>
        public static void EnableDisableTask(string taskIdentifier, bool enable)
        {
            if (string.IsNullOrWhiteSpace(taskIdentifier))
                throw new ArgumentException("Task identifier cannot be null or empty", nameof(taskIdentifier));

            try
            {
                string command = enable ? 
                    $"crontab -l | sed '/# {taskIdentifier}/ s/^# //g' | crontab -" :
                    $"crontab -l | sed '/{taskIdentifier}/ s/^/# /g' | crontab -";

                ExecuteCommand(command, redirectOutput: false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while enabling/disabling the task in crontab", ex);
            }
        }

        /// <summary>
        /// Lists all tasks in the crontab along with their next execution times.
        /// </summary>
        /// <returns>A detailed list of crontab tasks and their next run times.</returns>
        /// <exception cref="InvalidOperationException">Thrown if listing tasks fails.</exception>
        public static string ListTasksWithDetails()
        {
            try
            {
                string command = "crontab -l | while read line; do next_run=$(echo $line | awk '{print $1,$2,$3,$4,$5}' | crontab -n); echo $line - Next Run: $next_run; done";
                return ExecuteCommand(command);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while listing tasks with details from crontab", ex);
            }
        }

        /// <summary>
        /// Exports the current crontab configuration to a specified file.
        /// </summary>
        /// <param name="filePath">The file path where the crontab configuration will be saved.</param>
        /// <exception cref="ArgumentException">Thrown if the file path is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if exporting the crontab fails.</exception>
        public static void ExportTasks(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            try
            {
                ExecuteCommand($"crontab -l > {filePath}", redirectOutput: false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while exporting tasks from crontab", ex);
            }
        }

        /// <summary>
        /// Imports a crontab configuration from a specified file.
        /// </summary>
        /// <param name="filePath">The file path containing the crontab configuration to import.</param>
        /// <exception cref="ArgumentException">Thrown if the file path is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if importing the crontab fails.</exception>
        public static void ImportTasks(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            try
            {
                ExecuteCommand($"crontab {filePath}", redirectOutput: false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while importing tasks to crontab", ex);
            }
        }

        /// <summary>
        /// Tracks changes made to the crontab by creating a timestamped backup of the current configuration.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if tracking changes fails.</exception>
        public static void TrackCrontabChanges()
        {
            try
            {
                string historyDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".crontab_history");
                if (!Directory.Exists(historyDir))
                {
                    Directory.CreateDirectory(historyDir);
                }

                ExecuteCommand("command -v crontab", redirectError: true);

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                ExecuteCommand($"crontab -l > {historyDir}/{timestamp}_crontab.bak", redirectOutput: false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while tracking crontab changes", ex);
            }
        }
    }
}

