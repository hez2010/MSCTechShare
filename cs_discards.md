# 弃元
从 C# 7 开始，C# 支持弃元，这是一种在应用程序代码中人为取消使用的临时虚拟变量。 弃元相当于未赋值的变量；它们没有值。 因为只有一个弃元变量，甚至不为该变量分配存储空间，所以弃元可减少内存分配。  
通过将 _ 赋给一个变量作为其变量名，指示该变量为一个弃元。
```cs
using System;
using System.Collections.Generic;

public class Example
{
   public static void Main()
   {
       var (_, _, _, pop1, _, pop2) = QueryCityDataForYears("New York City", 1960, 2010);

       Console.WriteLine($"Population change, 1960 to 2010: {pop2 - pop1:N0}");
   }
   
   private static (string, double, int, int, int, int) QueryCityDataForYears(string name, int year1, int year2)
   {
      int population1 = 0, population2 = 0;
      double area = 0;
      
      if (name == "New York City") {
         area = 468.48; 
         if (year1 == 1960) {
            population1 = 7781984;
         }
         if (year2 == 2010) {
            population2 = 8175133;
         }
      return (name, area, year1, population1, year2, population2);
      }

      return ("", 0, 0, 0, 0, 0);
   }
}
```