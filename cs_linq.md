# LINQ
所有 LINQ 查询操作都由以下三个不同的操作组成： 
* 获取数据源。 
* 创建查询。 
* 执行查询。

关键字： from where select group into orderby join let ascending descending on equals by in
```cs
class IntroToLINQ
{        
    static void Main()
    {
        // The Three Parts of a LINQ Query:
        //  1. Data source.
        int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

        // 2. Query creation.
        // numQuery is an IEnumerable<int>
        var numQuery =
            from num in numbers
            where (num % 2) == 0
            select num;

        // 3. Query execution.
        foreach (int num in numQuery)
        {
            Console.Write("{0,1} ", num);
        }
    }
}
```
查询变量本身只存储查询命令。 查询的实际执行将推迟到在 foreach 语句中循环访问查询变量之后进行。 要强制立即执行任何查询并缓存其结果，可调用 ToList 或 ToArray 方法。

#### 基本查询
```cs
//queryAllCustomers is an IEnumerable<Customer>
var queryAllCustomers = from cust in customers
                        select cust;
```

#### 筛选
```cs
var queryLondonCustomers = from cust in customers
                           where cust.City == "London"
                           select cust;
```

#### 排序
```cs
var queryLondonCustomers3 = 
    from cust in customers
    where cust.City == "London"
    orderby cust.Name ascending
    select cust;
```

#### 分组
group 子句用于对根据您指定的键所获得的结果进行分组。 例如，可指定按 City 对结果进行分组，使来自 London 或 Paris 的所有客户位于单独的组内。 在这种情况下，cust.City 是键。
```cs
// queryCustomersByCity is an IEnumerable<IGrouping<string, Customer>>
  var queryCustomersByCity =
      from cust in customers
      group cust by cust.City;

  // customerGroup is an IGrouping<string, Customer>
  foreach (var customerGroup in queryCustomersByCity)
  {
      Console.WriteLine(customerGroup.Key);
      foreach (Customer customer in customerGroup)
      {
          Console.WriteLine("    {0}", customer.Name);
      }
  }
```
如果必须引用某个组操作的结果，可使用 into 关键字创建能被进一步查询的标识符。 
```cs
// custQuery is an IEnumerable<IGrouping<string, Customer>>
var custQuery =
    from cust in customers
    group cust by cust.City into custGroup
    where custGroup.Count() > 2
    orderby custGroup.Key
    select custGroup;
```

#### 联接
联接操作在不同序列间创建关联，这些序列在数据源中未被显式模块化。 例如，可通过执行联接来查找所有位置相同的客户和分销商。 在 LINQ 中，join 子句始终作用于对象集合，而非直接作用于数据库表。 
```cs
var innerJoinQuery =
    from cust in customers
    join dist in distributors on cust.City equals dist.City
    select new { CustomerName = cust.Name, DistributorName = dist.Name };
```
在 LINQ 中，不必像在 SQL 中那样频繁使用 join，因为 LINQ 中的外键在对象模型中表示为包含项集合的属性。 例如 Customer 对象包含 Order 对象的集合。 不必执行联接，只需使用点表示法访问订单： 
```cs
from order in Customer.Orders...  
```

除查询之外，LINQ 还可用于数据转换等等。