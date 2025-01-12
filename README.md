## ☕ ScheduleYourTask

Crontab is a task scheduling tool on Unix and Linux operating systems that allows for programming the execution of commands at specific times, either repeatedly or at a one-time occurrence.

The basic syntax of a Crontab command consists of five fields separated by spaces that define the time of execution:
- **Minute** (0-59)
- **Hour** (0-23)
- **Day of the Month** (1-31)
- **Month** (1-12)
- **Day of the Week** (0-6, where 0 is Sunday)

For example:
```bash
0 12 * * *
```
This command will execute a task every day at 12:00 PM.

### ⏰ Common Usage Patterns
- **Repeating tasks**: Execute tasks at regular intervals (e.g., every minute, hour, day, week, or month).
- **Specific days and times**: Run tasks at precise schedules (e.g., weekdays at 9:00 AM).
- **Complex schedules**: Combine fields for advanced timing (e.g., every weekday at 9:00 AM and 5:00 PM).

Crontab simplifies system administration and automates routine tasks, making workflows more efficient.

## ☕ BinaryCoffee.ScheduleYourTask

`BinaryCoffee.ScheduleYourTask` is a .NET package that brings the power of Crontab scheduling to the .NET platform. It enables developers to interact with crontab configurations programmatically.

### Features ✨
- Retrieve existing crontab entries.
- Add or remove tasks with unique identifiers.
- Enable or disable tasks dynamically.
- Export or import crontab configurations.
- Track changes and maintain backups of crontab files.

## 🚀 How to Use the Package

1. **Include the package in your project**:
   Install from NuGet: [BinaryCoffee.ScheduleYourTask](https://www.nuget.org/packages/BinaryCoffee.SchedulingTask)

2. **Declare the namespace**:
   ```csharp
   using SchedulingTasks;
   ```

3. **Access the functionality**:
   Use methods like `Crontab.GetCrontab()` to interact with crontab entries programmatically.

### Example Usage 💻

#### Retrieve Existing Tasks
```csharp
using SchedulingTasks;

// Retrieve existing tasks
var tasks = Crontab.GetCrontab();
Console.WriteLine("Current Crontab Entries:");
Console.WriteLine(tasks);
```

#### Add a New Task
```csharp
using SchedulingTasks;

// Add a new task to run a script daily at 2 AM
string task = "0 2 * * * /path/to/your/script.sh";
Crontab.AddTask(task);
Console.WriteLine("Task added successfully.");
```

#### Remove a Task
```csharp
using SchedulingTasks;

// Remove a task using its unique identifier
string taskIdentifier = "your-unique-identifier";
Crontab.RemoveTask(taskIdentifier);
Console.WriteLine("Task removed successfully.");
```

#### Enable or Disable a Task
```csharp
using SchedulingTasks;

// Enable a task
string taskIdentifier = "your-unique-identifier";
Crontab.EnableDisableTask(taskIdentifier, true);
Console.WriteLine("Task enabled successfully.");

// Disable a task
Crontab.EnableDisableTask(taskIdentifier, false);
Console.WriteLine("Task disabled successfully.");
```

#### Export Tasks to a File
```csharp
using SchedulingTasks;

// Export crontab configuration to a file
string filePath = "/path/to/exported_crontab.txt";
Crontab.ExportTasks(filePath);
Console.WriteLine($"Crontab exported to {filePath}.");
```

#### Import Tasks from a File
```csharp
using SchedulingTasks;

// Import tasks from a file
string filePath = "/path/to/imported_crontab.txt";
Crontab.ImportTasks(filePath);
Console.WriteLine("Crontab imported successfully.");
```

#### Track Changes and Create Backups
```csharp
using SchedulingTasks;

// Track changes to crontab
Crontab.TrackCrontabChanges();
Console.WriteLine("Crontab changes tracked and backup created.");
```

## 📖 Documentation

### Methods Available

- **`Crontab.GetCrontab()`**:
  Retrieves the current crontab configuration.

- **`Crontab.AddTask(string task)`**:
  Adds a new task to the crontab.

- **`Crontab.RemoveTask(string taskIdentifier)`**:
  Removes a task by its unique identifier.

- **`Crontab.EnableDisableTask(string taskIdentifier, bool enable)`**:
  Enables or disables a task by toggling comments.

- **`Crontab.ExportTasks(string filePath)`**:
  Exports the crontab configuration to a file.

- **`Crontab.ImportTasks(string filePath)`**:
  Imports tasks from a crontab file.

- **`Crontab.TrackCrontabChanges()`**:
  Tracks changes to crontab by creating timestamped backups.

## 📌 Stickers for Enrichment
- ⏰ Scheduling made simple.
- 💻 .NET Integration for Crontab.
- 🚀 Automate your workflows effortlessly.
- 📂 Manage your tasks programmatically.

## 🛠️ Contributing
Feel free to contribute by submitting issues or pull requests to the [GitHub Repository](https://github.com/stescobedo92/ScheduleYourTask).

---

Made with ☕ by BinaryCoffee.


