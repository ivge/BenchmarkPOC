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
        private const int TasksCount = 100;

        private delegate void InstanceOperation();
        private delegate void StaticOperation();

        public StaticThreadsafeTest()
        {
            bool result;
            //int wt, pt;
            ThreadPool.GetAvailableThreads(out int wt, out int pt);
            result = ThreadPool.SetMinThreads(3, pt);
            Assert.AreEqual(true, result);

            result = ThreadPool.SetMaxThreads(3, pt);
            Assert.AreEqual(true, result);
        }

        private class InstanceClass
        {
            private readonly object Lock = new object();
            public long Field { get; set; } = 0;

            public void Add()
            {
                for (long i = 0; i < iterations; i++)
                {
                    this.Field += 1;
                }
            }

            public void Subtract()
            {
                for (long i = 0; i < iterations; i++)
                {
                    this.Field -= 1;
                }
            }

            public void AddLocked()
            {
                for (long i = 0; i < iterations; i++)
                {
                    lock (this.Lock)
                    {
                        this.Field += 1;
                    }
                }
            }

            public void SubtractLocked()
            {
                for (long i = 0; i < iterations; i++)
                {
                    lock (this.Lock)
                    {
                        this.Field -= 1;
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
                    Field += 1;
                }
            }

            public static void Subtract()
            {
                for (long i = 0; i < iterations; i++)
                {
                    Field -= 1;
                }
            }

            public static void AddLocked()
            {
                for (long i = 0; i < iterations; i++)
                {
                    lock (Lock)
                    {
                        Field += 1;
                    }
                }
            }

            public static void SubtractLocked()
            {
                for (long i = 0; i < iterations; i++)
                {
                    lock (Lock)
                    {
                        Field -= 1;
                    }
                }
            }
        }


        [TestMethod]
        public void TestMethodInstance()
        {
            var tasks = new List<Task>();


            var instance = new InstanceClass();
            StartTasksInstance(tasks, instance.Add, instance.Subtract);

            WaitAllTasks(tasks);

            Assert.AreEqual(0, instance.Field);
        }

        [TestMethod]
        public void TestMethodInstanceLocked()
        {
            var tasks = new List<Task>();

            var instance = new InstanceClass();
            StartTasksInstance(tasks, instance.AddLocked, instance.SubtractLocked);

            WaitAllTasks(tasks);

            Assert.AreEqual(0, instance.Field);
        }

        [TestMethod]
        public void TestMethodStatic()
        {
            var tasks = new List<Task>();

            StaticClass.Field = 0;

            StartTasksStatic(tasks, StaticClass.Add, StaticClass.Subtract);
            WaitAllTasks(tasks);

            Assert.AreEqual(0, StaticClass.Field);
        }


        [TestMethod]
        public void TestMethodStaticLocked()
        {
            var tasks = new List<Task>();

            StaticClass.Field = 0;

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

                Console.WriteLine($"wt = {wt}, pt = {pt}, pending = {ThreadPool.PendingWorkItemCount}, tc = {ThreadPool.ThreadCount}" +
                    $" {s}");
                Thread.Sleep(100);
            }
        }

        private void StartTasksInstance(List<Task> tasks, InstanceOperation add, InstanceOperation subtract)
        {
            for (int i = 0; i < StaticThreadsafeTest.TasksCount; i++)
            {
                tasks.Add(Task.Run(() => add()));
                tasks.Add(Task.Run(() => subtract()));
            }
        }

        private void StartTasksStatic(List<Task> tasks, StaticOperation add, StaticOperation subtract)
        {
            for (int i = 0; i < StaticThreadsafeTest.TasksCount; i++)
            {
                tasks.Add(Task.Run(() => add()));
                tasks.Add(Task.Run(() => subtract()));
            }
        }
    }
}
