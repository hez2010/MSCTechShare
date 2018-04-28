using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Advanced_C__Tech_and_Q_
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic a;
            a = "123";
            a = 666;
            var t1 = new Task(() =>
            {
                //doing something
            });
            t1.Start();

            var t2 = new Task(a);
            t2.Start();

            var taskList = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var t = new Task(a);
                taskList.Add(t);
                t.Start();
            }
            while (taskList.Any(i => i.Status == TaskStatus.Running))
            {
                Thread.Sleep(100);
            }
            Console.WriteLine("All finished");
            return;
        }

        public static void a()
        {
            var r = new Random();
            Console.WriteLine(233);
            Thread.Sleep(r.Next(100, 500));
            Console.WriteLine(666);
        }
    }
}
