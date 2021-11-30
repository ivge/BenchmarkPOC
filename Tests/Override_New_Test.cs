using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace POCTests.OverrideOrNewTests
{
    [TestClass]
    public class OverrideNewTest
    {
        private class A
        {
            public virtual string GetString()
            {
                return "Base";
            }
        }

        private class B : A
        {
            public new string GetString()
            {
                return "new";
            }
        }

        private class C : A
        {
            public override string GetString()
            {
                return "Override";
            }
        }

        private string GetString(A instance) => instance.GetString();

        [TestMethod]
        public void TestMethodBase()
        {
            Assert.AreEqual("Base", GetString(new A()));
        }

        [TestMethod]
        public void TestMethodNew()
        {
            Assert.AreEqual("Base", GetString(new B()));
        }

        [TestMethod]
        public void TestMethodOverride()
        {
            Assert.AreEqual("Override", GetString(new C()));
        }
    }
}
