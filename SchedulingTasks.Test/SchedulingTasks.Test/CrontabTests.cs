namespace SchedulingTasks.Test;

public class CrontabTests
{
    [Test]
    public void GetCrontab_ReturnsEmptyString()
    {
        string crontab = Crontab.GetCrontab();
        Assert.That(crontab, Is.Empty);
    }

    [Test]
    public void GetCrontab_ReturnsNonEmptyString()
    {
        string crontab = Crontab.GetCrontab();
        Assert.That(crontab, Is.Not.Empty);
    }

    [Test]
    public void AddTask_AddsTaskToCrontab()
    {
        string task = "* * * * * echo 'Hello, world!'";
        Crontab.AddTask(task);
        string crontab = Crontab.GetCrontab();
        Assert.That(crontab, Contains.Substring(task));
    }

    [Test]
    public void EditCrontab_UpdatesCrontab()
    {
        string originalCrontab = Crontab.GetCrontab();
        Crontab.EditCrontab();
        string editedCrontab = Crontab.GetCrontab();
        Assert.That(editedCrontab, Is.Not.EqualTo(originalCrontab));
    }
}
