using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POCTests
{
    [TestClass]
    public class ExceptionsTest
    {
        public delegate void TestExceptionThrowMethod();

        [TestMethod]
        public void TestMethod1()
        {
            var methods = new Dictionary<string, TestExceptionThrowMethod>();
            methods.Add("Throw", Throw);
            methods.Add("Throw Ex", ThrowEx);

            foreach (var item in methods)
                try
                {
                    item.Value();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{item.Key} stacktrace");
                    Console.WriteLine(e.StackTrace);
                }
        }

        private void Throw()
        {
            try
            {
                A();
            }
            catch
            {
                throw;
            }
        }

        private void ThrowEx()
        {
            try
            {
                A();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void A()
        {
            B();
        }

        private void B()
        {
            C();
        }

        private void C()
        {
            throw new Exception("SLOMALOS'");
        }
    }
}
