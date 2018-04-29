# yield
yield是C#为了简化遍历操作实现的语法糖，我们知道如果要要某个类型支持遍历就必须要实现系统接口IEnumerable，这个接口后续实现比较繁琐要写一大堆代码才能支持真正的遍历功能。举例说明

```cs
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace{
    class Program
    {
        static void Main(string[] args)
        {
            HelloCollection helloCollection = new HelloCollection();
            foreach (string s in helloCollection)
            {
                Console.WriteLine(s);
            }

            Console.ReadKey();
        }
    }

    public class HelloCollection : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            Enumerator enumerator = new Enumerator(0);
            return enumerator;
        }

        public class Enumerator : IEnumerator, IDisposable
        {
            private int state;
            private object current;

            public Enumerator(int state)
            {
                this.state = state;
            }

            public bool MoveNext()
            {
                switch (state)
                {
                    case 0:
                        current = "Hello";
                        state = 1;
                        return true;
                    case 1:
                        current = "World";
                        state = 2;
                        return true;
                    case 2:
                        break;
                }
                return false;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public object Current
            {
                get { return current; }
            }
            public void Dispose()
            {
            }
        }
    }
}
```

```cs
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace{
    class Program
    {
        static void Main(string[] args)
        {
            HelloCollection helloCollection = new HelloCollection();
            foreach (string s in helloCollection)
            {
                Console.WriteLine(s);
            }

            Console.ReadKey();
        }
    }

    public class HelloCollection : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return "Hello";
            yield return "World";
        }
    }
}
```

yield return 表示在迭代中下一个迭代时返回的数据，除此之外还有yield break, 其表示跳出迭代，为了理解二者的区别我们看下面的例子。比如：

```cs
class A : IEnumerable
{
    private int[] array = new int[10];

    public IEnumerator GetEnumerator()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return array[i];
        }
    }
}
```
如果你只想让用户访问ARRAY的前8个数据,则可做如下修改.这时将会用到yield break,修改函数如下
```cs
public IEnumerator GetEnumerator()
{
    for (int i = 0; i < 10; i++)
    {
        if (i < 8)
        {
            yield return array[i];
        }
        else
        {
            yield break;
        }
    }
}
```