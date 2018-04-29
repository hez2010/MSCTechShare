# using
* using namespace
    ```cs
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    ```

* static members of a type

    before
    ```cs
    class Program   
    {   
        static void Main()   
        {   
            Console.WriteLine(Math.Sqrt(3*3 + 4*4));   
        }   
    } 
    ```
    after
    ```cs
    using static System.Console;   
    using static System.Math;  
    class Program   
    {   
        static void Main()   
        {   
            WriteLine(Sqrt(3*3 + 4*4));   
        }   
    } 
    ```
* alias
    ```cs
    using Project = PC.MyCompany.Project;
    using Msgbox = MessageBox.Show;
    ```
* IDisposable
    ```cs
    class A : IDisposable
    {
        public void Dispose()
        {
            ...
        }
    }
    ...

    class Program
    {
        static void Main(string[] arg)
        {
            using (var a = new A())
            {
                ...
            }
        }
    }
   
    ```

examples
```cs
namespace PC
{
    // Define an alias
    using Project = PC.MyCompany.Project;
    class A
    {
        void M()
        {
            // Use the alias
            Project.MyClass mc = new Project.MyClass();
        }
    }
    namespace MyCompany
    {
        namespace Project
        {
            public class MyClass { }
        }
    }
}
```

```cs
using System;

// Using alias directive for a class.
using AliasToMyClass = NameSpace1.MyClass;

// Using alias directive for a generic class.
using UsingAlias = NameSpace2.MyClass<int>;

namespace NameSpace1
{
    public class MyClass
    {
        public override string ToString()
        {
            return "You are in NameSpace1.MyClass.";
        }
    }

}

namespace NameSpace2
{
    class MyClass<T>
    {
        public override string ToString()
        {
            return "You are in NameSpace2.MyClass.";
        }
    }
}

namespace NameSpace3
{
    // Using directive:
    using NameSpace1;
    // Using directive:
    using NameSpace2;

    class MainClass
    {
        static void Main()
        {
            AliasToMyClass instance1 = new AliasToMyClass();
            Console.WriteLine(instance1);

            UsingAlias instance2 = new UsingAlias();
            Console.WriteLine(instance2);

        }
    }
}
// Output: 
//    You are in NameSpace1.MyClass.
//    You are in NameSpace2.MyClass.
```