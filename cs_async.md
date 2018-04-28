# 异步
异步编程的核心是 Task 和 Task\<T> 对象，这两个对象对异步操作建模。 它们受关键字 async 和 await 的支持。 在大多数情况下模型十分简单： 
对于 I/O 绑定代码，当你 await 一个操作，它将返回 async 方法中的一个 Task 或 Task\<T>。
对于 CPU 绑定代码，当你 await 一个操作，它将在后台线程通过 Task.Run 方法启动。
await 关键字有这奇妙的作用。 它控制执行 await 的方法的调用方，且它最终允许 UI 具有响应性或服务具有灵活性。

I/O 绑定示例：你可能需要在按下按钮时从 Web 服务下载某些数据，但不希望阻止 UI 线程。 只需执行如下操作即可轻松实现：
```cs
private readonly HttpClient _httpClient = new HttpClient();

downloadButton.Clicked += async (o, e) =>
{
    var stringData = await _httpClient.GetStringAsync(URL);
    DoSomethingWithData(stringData);
};
```

CPU 绑定示例：假设你正在编写一个游戏，在该游戏中，按下某个按钮将会对屏幕中的许多敌人造成伤害。 执行伤害计算的开销可能极大，而且在 UI 线程中执行计算有可能使游戏在计算执行过程中暂停！
此问题的最佳解决方法是启动一个后台线程，它使用 Task.Run 执行工作，并 await 其结果。 这可确保在执行工作时 UI 能流畅运行。
```cs
private DamageResult CalculateDamageDone()
{
    //do something
}


calculateButton.Clicked += async (o, e) =>
{
    var damageResult = await Task.Run(() => CalculateDamageDone());
    DisplayDamage(damageResult);
};
```

选择合适的方案：

需要等待某些内容或数据 —— I/O 绑定。  
需要执行开销巨大的计算 —— CPU 绑定。  
如果是 I/O 绑定，请使用 async 和 await（而不使用 Task.Run），不应使用任务并行库。  
如果是 CPU 绑定，并且你重视响应能力，请使用 async 和 await，并在另一个线程上使用 Task.Run 生成工作。 如果该工作同时适用于并发和并行，则应考虑使用任务并行库。  
| 使用 | 代替 | 作用 |
| --------   | -----   | ---- |
await | Task.Wait 或 Task.Result | 检索后台任务的结果
await Task.WhenAny | Task.WaitAny | 等待任何任务完成
await Task.WhenAll | Task.WaitAll | 等待所有任务完成
await Task.Delay | Thread.Sleep | 等待一段时间

深入了解：https://docs.microsoft.com/zh-cn/dotnet/standard/async-in-depth