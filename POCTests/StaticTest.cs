using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;

namespace POCTests
{
    [TestClass]
    public class StaticTest
    {

        public static class StaticFailedClass
        {
            public static int x = 1 / Int32.Parse("0");

        }

        [TestMethod]
        public void TestMethod1()
        {
            for (int i = 0; i < 2; i++)
            try
            {
                Console.WriteLine(StaticFailedClass.x);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
