# lambda
```cs
Func<int, int> square = x => x * x;
Func<int, int, int> sum = (a, b) => a + b;
Func<string, string> fuck = a => 
{
    Fuck(a);
    return FuckString;
}

Action<string> show = a => Console.WriteLine(a);

square.Invoke(4); //return 16
```

