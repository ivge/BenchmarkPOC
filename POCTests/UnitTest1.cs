using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace POCTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] colors =  { "green", "blue", "brown", "red" };
            var q = colors.Where(x => x.Contains("e"));
            q = q.Where(x => x.Contains("n"));

            Console.WriteLine(q.Count());

        }
    }
}
