# 表达式树

```cs
var sum = 1 + 2;
```
如果要将其作为一个表达式树进行分析，则该树包含多个节点。 最外面的节点是具有赋值 (var sum = 1 + 2;) 的变量声明语句，该节点包含若干子节点：变量声明、赋值运算符和一个表示等于号右侧的表达式。 该表达式被进一步细分为表示加法运算、该加法左操作数和右操作数的表达式。  
右侧表达式是 1 + 2。 这是一个二进制表达式。 更具体地说，它是一个二进制加法表达式。 二进制加法表达式有两个子表达式，表示加法表达式的左侧和右侧节点。 此处，这两个节点均为常数表达式：左操作数是值 1，右操作数是值 2。  
直观地看，整个语句是一棵树：应从根节点开始，浏览到树中的每个节点，以查看构成该语句的代码：  
* 具有赋值 (var sum = 1 + 2;) 的变量声明语句
    * 隐式变量类型声明 (var sum)
        * 隐式 var 关键字 (var)
        * 变量名称声明 (sum)
    * 赋值运算符 (=)
    * 二进制加法表达式 (1 + 2)
        * 左操作数 (1)
        * 加法运算符 (+)
        * 右操作数 (2)

#### 执行表达式
```cs
Expression<Func<int>> add = () => 1 + 2;
var func = add.Compile(); // Create Delegate
var answer = func(); // Invoke Delegate
Console.WriteLine(answer);
```

#### 解释表达式
```cs
Expression<Func<int, int, int>> addition = (a, b) => a + b;

Console.WriteLine($"This expression is a {addition.NodeType} expression type");
Console.WriteLine($"The name of the lambda is {((addition.Name == null) ? "<null>" : addition.Name)}");
Console.WriteLine($"The return type is {addition.ReturnType.ToString()}");
Console.WriteLine($"The expression has {addition.Parameters.Count} arguments. They are:");
foreach(var argumentExpression in addition.Parameters)
{
    Console.WriteLine($"\tParameter Type: {argumentExpression.Type.ToString()}, Name: {argumentExpression.Name}");
}

var additionBody = (BinaryExpression)addition.Body;
Console.WriteLine($"The body is a {additionBody.NodeType} expression");
Console.WriteLine($"The left side is a {additionBody.Left.NodeType} expression");
var left = (ParameterExpression)additionBody.Left;
Console.WriteLine($"\tParameter Type: {left.Type.ToString()}, Name: {left.Name}");
Console.WriteLine($"The right side is a {additionBody.Right.NodeType} expression");
var right= (ParameterExpression)additionBody.Right;
Console.WriteLine($"\tParameter Type: {right.Type.ToString()}, Name: {right.Name}");
```
> 输出：
> ```
> This expression is a/an Lambda expression type
> The name of the lambda is <null>
> The return type is System.Int32
> The expression has 2 arguments. They are:
>         Parameter Type: System.Int32, Name: a
>         Parameter Type: System.Int32, Name: b
> The body is a/an Add expression
> The left side is a Parameter expression
>         Parameter Type: System.Int32, Name: a
> The right side is a Parameter expression
>         Parameter Type: System.Int32, Name: b
> ```

#### 生成表达式
```cs
Expression<Func<int>> sum = () => 1 + 2;
```
若要构造该表达式树，必须构造叶节点。 叶节点是常量，因此可以使用 Expression.Constant 方法创建节点：
```cs
var one = Expression.Constant(1, typeof(int));
var two = Expression.Constant(2, typeof(int));
```
接下来，将生成加法表达式：
```cs
var addition = Expression.Add(one, two);
```
一旦获得了加法表达式，就可以创建 lambda 表达式：
```cs
var lamdba = Expression.Lambda(addition);
```

对于此类简单的表达式，可以将所有调用合并到单个语句中：
```cs
var lambda = Expression.Lambda(
    Expression.Add(
        Expression.Constant(1, typeof(int)),
        Expression.Constant(2, typeof(int))
    )
);
```

#### 生成树
这是在内存中生成表达式树的基础知识。 更复杂的树通常意味着更多的节点类型，并且树中有更多的节点。 让我们再浏览一个示例，了解通常在创建表达式树时创建的其他两个节点类型：参数节点和方法调用节点。  
生成一个表达式树以创建此表达式：
```cs
Expression<Func<double, double, double>> distanceCalc =
    (x, y) => Math.Sqrt(x * x + y * y);
```

首先，创建 x 和 y 的参数表达式：
```cs
var xParameter = Expression.Parameter(typeof(double), "x");
var yParameter = Expression.Parameter(typeof(double), "y");
```

按照你所看到的模式创建乘法和加法表达式：
```cs
var xSquared = Expression.Multiply(xParameter, xParameter);
var ySquared = Expression.Multiply(yParameter, yParameter);
var sum = Expression.Add(xSquared, ySquared);
```

接下来，需要为调用 Math.Sqrt 创建方法调用表达式。
```cs
var sqrtMethod = typeof(Math).GetMethod("Sqrt", new[] { typeof(double) });
var distance = Expression.Call(sqrtMethod, sum);
```
最后，将方法调用放入 lambda 表达式，并确保定义 lambda 表达式的参数：
```cs
var distanceLambda = Expression.Lambda(
    distance,
    xParameter,
    yParameter);
```

#### 更刺激一点的
生成：
```cs
Func<int, int> factorialFunc = (n) =>
{
    var res = 1;
    while (n > 1)
    {
        res = res * n;
        n--;
    }
    return res;
};
```
```cs
var nArgument = Expression.Parameter(typeof(int), "n");
var result = Expression.Variable(typeof(int), "result");

// Creating a label that represents the return value
LabelTarget label = Expression.Label(typeof(int));

var initializeResult = Expression.Assign(result, Expression.Constant(1));

// This is the inner block that performs the multiplication,
// and decrements the value of 'n'
var block = Expression.Block(
    Expression.Assign(result,
        Expression.Multiply(result, nArgument)),
    Expression.PostDecrementAssign(nArgument)
);

// Creating a method body.
BlockExpression body = Expression.Block(
    new[] { result },
    initializeResult,
    Expression.Loop(
        Expression.IfThenElse(
            Expression.GreaterThan(nArgument, Expression.Constant(1)),
            block,
            Expression.Break(label, result)
        ),
        label
    )
);
```

#### 转换表达式
参考 https://docs.microsoft.com/zh-cn/dotnet/csharp/expression-trees-translating