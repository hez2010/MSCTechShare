# 泛型
在 C# 1.0 中，ArrayList 元素的类型为 object。 这意味着，添加的任何元素都将以静默方式转换为 object；从列表中读取元素时也会发生相同的情况。这样的装箱和取消装箱会给性能造成影响。  
不仅如此，在编译时无法知道列表中的实际数据类型是什么。 这就使得某些代码不太可靠。 泛型解决了此问题，它可以提供有关每个列表实例将要包含的数据类型的附加信息。 简单而言，只能将整数添加到 List\<int>，只能将人员添加到 List<Person>，等等。

```cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenericsExample 
{
    class Program 
    {
        static void Main(string[] args) 
        {
            //generic list
            List<int> ListGeneric = new List<int> { 5, 9, 1, 4 };
            //non-generic list
            ArrayList ListNonGeneric = new ArrayList { 5, 9, 1, 4 };
            // timer for generic list sort
            Stopwatch s = Stopwatch.StartNew();
            ListGeneric.Sort();
            s.Stop();
            Console.WriteLine($"Generic Sort: {ListGeneric}  \n Time taken: {s.Elapsed.TotalMilliseconds}ms");

            //timer for non-generic list sort
            Stopwatch s2 = Stopwatch.StartNew();
            ListNonGeneric.Sort();
            s2.Stop();
            Console.WriteLine($"Non-Generic Sort: {ListNonGeneric}  \n Time taken: {s2.Elapsed.TotalMilliseconds}ms");
            Console.ReadLine();
        }
    }
}
```
> 输出：  
> Generic Sort: System.Collections.Generic.List\`1[System.Int32] Time taken: 0.0789ms  
Non-Generic Sort: System.Collections.ArrayList Time taken: 2.4324ms

在此处可以看到的第一个优点是，泛型列表的排序比非泛型列表要快得多。 还可以看到，泛型列表的类型是不同的 ([System.Int32])，而非泛型列表的类型已通用化。 由于运行时知道泛型 List<int> 的类型是 int，因此可以将列表元素存储在内存中的基础整数数组内；而非泛型 ArrayList 必须将每个列表元素强制转换为对象并将其存储在内存中的对象数组内。 如本示例中所示，多余的强制转换会占用时间，降低列表排序的速度。

```cs
// Declare the generic class.
public class GenericList<T>
{
    public void Add(T input) { }
}
class TestGenericList
{
    private class ExampleClass { }
    static void Main()
    {
        // Declare a list of type int.
        GenericList<int> list1 = new GenericList<int>();
        list1.Add(1);

        // Declare a list of type string.
        GenericList<string> list2 = new GenericList<string>();
        list2.Add("");

        // Declare a list of type ExampleClass.
        GenericList<ExampleClass> list3 = new GenericList<ExampleClass>();
        list3.Add(new ExampleClass());
    }
}
```

定义泛型类时，可以对客户端代码能够在实例化类时用于类型参数的几种类型施加限制。 如果客户端代码尝试使用约束所不允许的类型来实例化类，则会产生编译时错误。 这些限制称为约束。 通过使用 where 上下文关键字指定约束。 下表列出了六种类型的约束： 
| 约束 | 解释 |
| -- | -- |
where T: struct | 类型参数必须是值类型。 可以指定除 Nullable 以外的任何值类型。 有关详细信息，请参阅使用可以为 null 的类型。
where T : class | 类型参数必须是引用类型；这同样适用于所有类、接口、委托或数组类型。
where T : new() | 类型参数必须具有公共无参数构造函数。 与其他约束一起使用时，new() 约束必须最后指定。
where T :\<基类名> | 类型参数必须是指定的基类或派生自指定的基类。
where T :\<接口名称> | 类型参数必须是指定的接口或实现指定的接口。 可指定多个接口约束。 约束接口也可以是泛型。
where T : U | 为 T 提供的类型参数必须是为 U 提供的参数或派生自为 U 提供的参数。

```cs
public class Employee
{
    private string name;
    private int id;

    public Employee(string s, int i)
    {
        name = s;
        id = i;
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }
}

public class GenericList<T> where T : Employee
{
    private class Node
    {
        private Node next;
        private T data;

        public Node(T t)
        {
            next = null;
            data = t;
        }

        public Node Next
        {
            get { return next; }
            set { next = value; }
        }

        public T Data
        {
            get { return data; }
            set { data = value; }
        }
    }

    private Node head;

    public GenericList() //constructor
    {
        head = null;
    }

    public void AddHead(T t)
    {
        Node n = new Node(t);
        n.Next = head;
        head = n;
    }

    public IEnumerator<T> GetEnumerator()
    {
        Node current = head;

        while (current != null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    public T FindFirstOccurrence(string s)
    {
        Node current = head;
        T t = null;

        while (current != null)
        {
            //The constraint enables access to the Name property.
            if (current.Data.Name == s)
            {
                t = current.Data;
                break;
            }
            else
            {
                current = current.Next;
            }
        }
        return t;
    }
}
```
约束使得泛型类能够使用 Employee.Name 属性，因为类型 T 的所有项都保证是 Employee 对象或是从 Employee 继承的对象。 

可以对同一类型参数应用多个约束，并且约束自身可以是泛型类型：
```cs
class EmployeeList<T> where T : Employee, IEmployee, System.IComparable<T>, new()
{
    // ...
}
```

在应用 where T : class 约束时，请避免对类型参数使用 == 和 != 运算符，因为这些运算符仅测试引用标识而不测试值相等性。 即使在用作参数的类型中重载这些运算符也是如此。
```cs
public static void OpTest<T>(T s, T t) where T : class
{
    System.Console.WriteLine(s == t);
}
static void Main()
{
    string s1 = "target";
    System.Text.StringBuilder sb = new System.Text.StringBuilder("target");
    string s2 = sb.ToString();
    OpTest<string>(s1, s2);
}
```
> 输出： false

编译器在编译时仅知道 T 是引用类型，因此必须使用对所有引用类型都有效的默认运算符。 如果必须测试值相等性，建议的方法是同时应用 where T : IComparable\<T> 约束，并在将用于构造泛型类的任何类中实现该接口。 
#### 约束多个参数
可以对多个参数应用多个约束，对一个参数应用多个约束：
```cs
class Base { }
class Test<T, U>
    where U : struct
    where T : Base, new() { }
```
#### 未绑定的类型参数
没有约束的类型参数（如公共类 SampleClass\<T>{} 中的 T）称为未绑定的类型参数。  
未绑定的类型参数具有以下规则：   
不能使用 != 和 == 运算符，因为无法保证具体的类型参数能支持这些运算符。 
可以在它们与 System.Object 之间来回转换，或将它们显式转换为任何接口类型。 
可以将它们与 null 进行比较。 将未绑定的参数与 null 进行比较时，如果类型参数为值类型，则该比较将始终返回 false。   
#### 类型参数作为约束
在具有自己类型参数的成员函数必须将该参数约束为包含类型的类型参数时，将泛型类型参数用作约束非常有用。
```cs
class List<T>
{
    void Add<U>(List<U> items) where U : T {/*...*/}
}
```

* 泛型可以用于类、接口、方法、数组、委托等等
* 协变/逆变此处不做介绍