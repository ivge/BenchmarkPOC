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
        private const int TasksCount = 10;
        private const int AvailableThreads = 5;

        private delegate void InstanceOperation();
        private delegate void StaticOperation();

        public StaticThreadsafeTest()
        {
            bool result;
            //int wt, pt;
            ThreadPool.GetAvailableThreads(out int wt, out int pt);
            result = ThreadPool.SetMinThreads(AvailableThreads, pt);
            Assert.AreEqual(true, result);

            result = ThreadPool.SetMaxThreads(AvailableThreads, pt);
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
        public async Task TestMethodInstanceAsync()
        {
            var instance = new InstanceClass();
            var tasks = CreateTasksInstance(instance.Add, instance.Subtract);

            await ExecuteAllTasksAsync(tasks);

            Assert.AreEqual(0, instance.Field);
        }

        [TestMethod]
        public void TestMethodInstance()
        {
            var instance = new InstanceClass();
            var tasks = CreateTasksInstance(instance.Add, instance.Subtract).ToArray();

            ExecuteAllTasksAsync(tasks).Wait();

            Assert.AreEqual(0, instance.Field);
        }

        [TestMethod]
        public void TestMethodInstanceOneThread()
        {
            var instance = new InstanceClass();
            var tasks = CreateTasksInstance(instance.Add, instance.Subtract).ToArray();

            StartAllTasks(tasks);

            Assert.AreEqual(0, instance.Field);
        }


        [TestMethod]
        public async Task TestMethodInstanceLockedAsync()
        {
            var instance = new InstanceClass();
            var tasks = CreateTasksInstance(instance.AddLocked, instance.SubtractLocked).ToArray();

            await ExecuteAllTasksAsync(tasks);

            Assert.AreEqual(0, instance.Field);
        }

        [TestMethod]
        public void TestMethodInstanceLocked()
        {
            var instance = new InstanceClass();
            var tasks = CreateTasksInstance(instance.AddLocked, instance.SubtractLocked).ToArray();

            ExecuteAllTasksAsync(tasks).Wait();

            Assert.AreEqual(0, instance.Field);
        }

        [TestMethod]
        public async Task TestMethodStaticAsync()
        {
            StaticClass.Field = 0;

            var tasks = CreateTasksStatic(StaticClass.Add, StaticClass.Subtract).ToArray();
            await ExecuteAllTasksAsync(tasks);

            Assert.AreEqual(0, StaticClass.Field);
        }

        [TestMethod]
        public void TestMethodStatic()
        {
            StaticClass.Field = 0;

            var tasks = CreateTasksStatic(StaticClass.Add, StaticClass.Subtract).ToArray();
            ExecuteAllTasksAsync(tasks).Wait();

            Assert.AreEqual(0, StaticClass.Field);
        }

        [TestMethod]
        public void TestMethodStaticOneThread()
        {
            StaticClass.Field = 0;

            var tasks = CreateTasksStatic(StaticClass.Add, StaticClass.Subtract).ToArray();

            StartAllTasks(tasks);

            Assert.AreEqual(0, StaticClass.Field);
        }


        [TestMethod]
        public async Task TestMethodStaticLockedAsync()
        {
            StaticClass.Field = 0;
            var tasks = CreateTasksStatic(StaticClass.AddLocked, StaticClass.SubtractLocked).ToArray();
            await ExecuteAllTasksAsync(tasks);
            Assert.AreEqual(0, StaticClass.Field);
        }


        [TestMethod]
        public void TestMethodStaticLocked()
        {
            StaticClass.Field = 0;
            var tasks = CreateTasksStatic(StaticClass.AddLocked, StaticClass.SubtractLocked).ToArray();
            ExecuteAllTasksAsync(tasks).Wait();
            Assert.AreEqual(0, StaticClass.Field);
        }

        private async Task ExecuteAllTasksAsync(IEnumerable<Task> tasks)
        {
            foreach (var task in tasks)
                task.Start();

            do
            {
                await GetThreadPoolStatistics(tasks);
            }
            while (!tasks.All(task => task.IsCompleted));
        }

        private void StartAllTasks(IEnumerable<Task> tasks)
        {
            Task.Run(async () =>
            {
                do
                {
                    await GetThreadPoolStatistics(tasks);
                }
                while (!tasks.All(task => task.IsCompleted));
            }
            );

            foreach (var task in tasks)
                task.RunSynchronously();
        }

        private async Task GetThreadPoolStatistics(IEnumerable<Task> tasks)
        {
            await Task.Delay(100);
            ThreadPool.GetAvailableThreads(out int wt, out int pt);
            var tasksGrouped = tasks.GroupBy(t => t.Status);
            string s = "";
            foreach (var record in tasksGrouped)
                s = s + $"{Enum.GetName(typeof(TaskStatus), record.Key)} - {record.Count()};";

            Console.WriteLine($"wt = {wt}, pt = {pt}, pending = {ThreadPool.PendingWorkItemCount}, tc = {ThreadPool.ThreadCount}" +
                $" {s}");

        }

        private Task[] CreateTasksInstance(InstanceOperation add, InstanceOperation subtract)
        {
            var result = new List<Task>(StaticThreadsafeTest.TasksCount * 2);
            for (int i = 0; i < StaticThreadsafeTest.TasksCount; i++)
            {
                result.Add(new Task(() => add()));
                result.Add(new Task(() => subtract()));
            }
            return result.ToArray();
        }

        private IEnumerable<Task> CreateTasksInstanceEnumerable(InstanceOperation add, InstanceOperation subtract)
        {
            for (int i = 0; i < StaticThreadsafeTest.TasksCount; i++)
            {
                yield return new Task(() => add());
                yield return new Task(() => subtract());
            }
        }

        private Task[] CreateTasksStatic(StaticOperation add, StaticOperation subtract)
        {
            var result = new List<Task>(StaticThreadsafeTest.TasksCount * 2);
            for (int i = 0; i < StaticThreadsafeTest.TasksCount; i++)
            {
                result.Add(new Task(() => add()));
                result.Add(new Task(() => subtract()));
            }
            return result.ToArray();
        }
    }
}
