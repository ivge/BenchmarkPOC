using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POCTests.StaticThreadsafeTest
{
    [TestClass]
    public class StaticThreadsafeTest
    {
        private const int iterations = 10000000;
        private delegate void InstanceOperation(InstanceClass instance);
        private delegate void StaticOperation();

        private class InstanceClass
        {
            public readonly object Lock = new object();
            public long Field { get; set; } = 0;

            public void Add(InstanceClass instance)
            {
                for (long i = 0; i < iterations; i++)
                {
                    instance.Field += 1;
                }
            }

            public void Subtract(InstanceClass instance)
            {
                for (long i = 0; i < iterations; i++)
                {
                    instance.Field -= 1;
                }
            }

            public void AddLocked(InstanceClass instance)
            {
                for (long i = 0; i < iterations; i++)
                {
                    lock (instance.Lock)
                    {
                        instance.Field += 1;
                    }
                }
            }

            public void SubtractLocked(InstanceClass instance)
            {
                for (long i = 0; i < iterations; i++)
                {
                    lock (instance.Lock)
                    {
                        instance.Field -= 1;
                    }
                }
            }
        }

        private static class StaticClass
        {
            private static readonly object Lock = new object();
            public static long Field { get; set; } = 0;

            public static void Add()
            {
                for (long i = 0; i < iterations; i++)
                {
                    StaticClass.Field += 1;
                }
            }

            public static void Subtract()
            {
                for (long i = 0; i < iterations; i++)
                {
                    StaticClass.Field -= 1;
                }
            }

            public static void AddLocked()
            {
                for (long i = 0; i < iterations; i++)
                {
                    lock (Lock)
                    {
                        StaticClass.Field += 1;
                    }
                }
            }

            public static void SubtractLocked()
            {
                for (long i = 0; i < iterations; i++)
                {
                    lock (Lock)
                    {
                        StaticClass.Field -= 1;
                    }
                }
            }
        }


        [TestMethod]
        public void TestMethodInstance()
        {
            var tasks = new List<Task>();


            var instance = new InstanceClass();
            StartTasksInstance(tasks, instance, instance.Add, instance.Subtract);

            WaitAllTasks(tasks);

            Assert.AreEqual(0, instance.Field);
        }

        [TestMethod]
        public void TestMethodInstanceLocked()
        {
            var tasks = new List<Task>();

            var instance = new InstanceClass();
            StartTasksInstance(tasks, instance, instance.AddLocked, instance.SubtractLocked);

            WaitAllTasks(tasks);

            Assert.AreEqual(0, instance.Field);
        }

        [TestMethod]
        public void TestMethodStatic()
        {
            var tasks = new List<Task>();

            StartTasksStatic(tasks, StaticClass.Add, StaticClass.Subtract);
            WaitAllTasks(tasks);


            Assert.AreEqual(0, StaticClass.Field);
        }


        [TestMethod]
        public void TestMethodStaticLocked()
        {
            var tasks = new List<Task>();

            StartTasksStatic(tasks, StaticClass.AddLocked, StaticClass.SubtractLocked);
            WaitAllTasks(tasks);

            Assert.AreEqual(0, StaticClass.Field);
        }

        private static void WaitAllTasks(List<Task> tasks)
        {
            var taskAll = Task.WhenAll(tasks.ToArray());

            int wt, pt;
            while (!taskAll.IsCompleted)
            {
                ThreadPool.GetAvailableThreads(out wt, out pt);
                var tasksGrouped = tasks.GroupBy(t => t.Status);
                string s = "";
                foreach (var record in tasksGrouped)
                    s = s + $"{Enum.GetName(typeof(TaskStatus), record.Key)} - {record.Count()};";

                Console.WriteLine($"wt = {wt}, pt = {pt}, {s}");
                Thread.Sleep(100);
            }
        }

        private void StartTasksInstance(List<Task> tasks, InstanceClass instance, InstanceOperation add, InstanceOperation subtract)
        {
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() => add(instance)));
                tasks.Add(Task.Run(() => subtract(instance)));
            }
        }

        private void StartTasksStatic(List<Task> tasks, StaticOperation add, StaticOperation subtract)
        {
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() => add()));
                tasks.Add(Task.Run(() => subtract()));
            }
        }


    }
}
