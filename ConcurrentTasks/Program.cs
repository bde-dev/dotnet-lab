namespace ConcurrentTasks;

class Program
{
    static async Task Main(string[] args)
    {
        //await RunParallelTasks();

        var lTopLevelTasks = new List<Task<bool>>();

        lTopLevelTasks.Add(TopLevelTask("1"));
        //lTopLevelTasks.Add(TopLevelTask("2"));

        await Task.WhenAll(lTopLevelTasks);
        
        Console.ReadLine();
    }

    static async Task<bool> TopLevelTask(string topIndex)
    {
        Console.WriteLine($"{topIndex}: Top Level Task Started");

        var src = new CancellationTokenSource();
        var token = src.Token;
        
        var lMidLevelTasks = new List<Task<bool>>();
        
        lMidLevelTasks.Add(MidLevelTask(topIndex, "1", src, token));
        lMidLevelTasks.Add(MidLevelTask(topIndex, "2", src, token));

        await Task.WhenAll(lMidLevelTasks);
        
        Console.WriteLine($"{topIndex}: Top Level Task Finished");

        return true;
    }

    static async Task<bool> MidLevelTask(string topIndex, string middleIndex, CancellationTokenSource src, CancellationToken token)
    {
        bool flag = false;
        
        try
        {
            Console.WriteLine($"{topIndex} | {middleIndex}: Mid Level Task Started");

            Random r = new Random();

            await Task.Delay(r.Next(1000, 5000), token);

            if (r.Next(1,1) == 1)
            {
                flag = true;
                src.Cancel();
            }

            Console.WriteLine($"{topIndex} | {middleIndex}: Mid Level Task Finished");
            return flag;
        }
        catch (OperationCanceledException lException)
        {
            Console.WriteLine($"{topIndex} | {middleIndex}: Task cancelled");
            return flag;
        }
    }

    static async Task RunParallelTasks()
    {
        // Create a list to store functions returning Task<List<MyTaskResult>>
        List<Func<int, string, Task<List<MyTaskResult>>>> lTasks = new();

        Console.WriteLine("Tasks initialized. Waiting 3 seconds...");
        await Task.Delay(3000);

        Console.WriteLine("Waited 3 seconds, starting tasks.");

        // Add tasks to the list as deferred functions
        lTasks.Add((delay, message) => Task1(delay, message));
        lTasks.Add((delay, message) => Task2(delay, message));
        lTasks.Add((delay, message) => Task3(delay, message));

        // Execute tasks concurrently using Task.WhenAll
        int lDelayDuration = 2000; // Example delay duration in milliseconds
        string lStartMessage = "Subtask started: ";
        List<MyTaskResult>[] lResults = await Task.WhenAll(lTasks.Select(taskFunc => taskFunc(lDelayDuration, lStartMessage)));

        // All tasks have completed
        Console.WriteLine("All tasks have completed.");

        // Handle the results if needed
        foreach (var lResult in lResults.SelectMany(r => r))
        {
            Console.WriteLine($"Task result: {lResult.Result}");
        }
    }

    static async Task<List<MyTaskResult>> Task1(int delay, string message)
    {
        Console.WriteLine(message + "Task 1 Subtask 1 started");
        await Task.Delay(delay); // Simulate some work based on delay duration
        Console.WriteLine(message + "Task 1 Subtask 1 completed");

        Console.WriteLine(message + "Task 1 Subtask 2 started");
        await Task.Delay(delay); // Simulate some work based on delay duration
        Console.WriteLine(message + "Task 1 Subtask 2 completed");

        return new List<MyTaskResult> { new("Task 1 result data") };
    }

    static async Task<List<MyTaskResult>> Task2(int delay, string message)
    {
        Console.WriteLine(message + "Task 2 Subtask 1 started");
        await Task.Delay(delay); // Simulate some work based on delay duration
        Console.WriteLine(message + "Task 2 Subtask 1 completed");

        Console.WriteLine(message + "Task 2 Subtask 2 started");
        await Task.Delay(delay); // Simulate some work based on delay duration
        Console.WriteLine(message + "Task 2 Subtask 2 completed");

        return new List<MyTaskResult> { new("Task 2 result data") };
    }

    static async Task<List<MyTaskResult>> Task3(int delay, string message)
    {
        Console.WriteLine(message + "Task 3 Subtask 1 started");
        await Task.Delay(delay); // Simulate some work based on delay duration
        Console.WriteLine(message + "Task 3 Subtask 1 completed");

        Console.WriteLine(message + "Task 3 Subtask 2 started");
        await Task.Delay(delay); // Simulate some work based on delay duration
        Console.WriteLine(message + "Task 3 Subtask 2 completed");

        return new List<MyTaskResult> { new("Task 3 result data") };
    }
}

// Custom class for task result data
class MyTaskResult
{
    public string Result { get; }

    public MyTaskResult(string result)
    {
        Result = result;
    }
}