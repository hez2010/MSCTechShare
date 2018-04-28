# Task
使用 Task 代替原始的多线程解决方案，Task 会自动管理并发数量。如果不想被限制并发数量可以使用 Thread 代替。
```cs
static void Main(string[] args)
{
    var t1 = new Task(() =>
    {
        //doing something
    });
    t1.Start();

    var t2 = new Task(a);
    t2.Start();

    var t3 = new Task<int>(b);
    t3.Start();
    Console.WriteLine(t3.Result); //output: 5

    Task.Run(a);
    Task.Run(() =>
    {
        //do something
    });

    var taskList = new List<Task>();
    for (int i = 0; i < 10; i++)
    {
        var t = new Task(a);
        taskList.Add(t);
        t.Start();
    }
    while (taskList.Any(i => i.Status == TaskStatus.Running))
    {
        Thread.Sleep(100);
    }
    Console.WriteLine("All finished");
}

public static void a()
{
    var r = new Random();
    Console.WriteLine(233);
    Thread.Sleep(r.Next(1000, 10000));
    Console.WriteLine(666);
}

public static int b()
{
    return 5;
}
```