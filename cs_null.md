# null 相关

#### 可空类型

```cs
int a; //a 初始为 0 且 a 不可能为 null
int? b; //b 初始为 null
```

当前最新的 C# 版本 (7.2) 仅支持可空值类型，在即将发布的 C# 8.0 版本中将可使用可空引用类型，如 string?

#### ? 和 ?? 操作符

```cs
public class A
{
    public B instance;
}

public class B
{
    public int? a;
}
```

```cs
public class Program
{
    static void Main(string[] args)
    {
        A x = new A();
        if (x.B != null && x.B.a != null)
        {
            Console.WriteLine(x.B.a);
        }
        else 
        {
            Console.WriteLine("null");
        }
    }
}
```

```cs
public class Program
{
    static void Main(string[] args)
    {
        A x = new A();
        Console.WriteLine(x.B?.a?.ToString() ?? "null");
    }
}
```
