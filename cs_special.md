# 一些奇怪的东西

1 
```cs
public abstract class MyBase<T> where T :MyBase <T> 
{
    public static string DataForThisType {get;set;}
    public static T Instance{get;protected set;} //让父类操作子类出现了 
    public static readonly IReadOnlyDictionary<string,MemberInfo> Members =typeof(T) 
        .GetMembers() 
        .ToDictionary(x=>x.Name); 
} 
public class MyClass:MyBase<MyClass> 
{ 
    static MyClass() 
    { 
        DataForThisType =string.Format(
                 "MyClass got {0} members",Members.Count); 
        Instance = new MyClass(); 
    } 
} 

```

2 SMID支持

```cs
// 两个整数数组相加的常规方法 
for (int i = 0; i < size; i++) 
{ C[i] = A[i] + B[i]; } 
// SIMD 方法， 每次几个元素同时相加，Vector<int>.Count是每个SSE/AVX寄存器容纳int的个数 
using System.Numerics.Vectors; 
for (int i = 0; i < size; i += Vector<int>.Count) 
{
    Vector<int> v = new Vector<int>(A,i) + new Vector<int>(B,i); 
    v.CopyTo(C,i); 
} 
```

3 无 GC 模式  
调用GC.TryStartNoGCRegion(int64)函数，传入一个内存大小（比如1G）。CLR会开辟一个指定大小的内存区域，然后进入无GC模式。适用于critical path部分的业务逻辑。

4 用 LINQ 写 Parser  
参考 https://blogs.msdn.microsoft.com/lukeh/2007/08/19/monadic-parser-combinators-using-c-3-0/