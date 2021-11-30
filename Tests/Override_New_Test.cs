using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace POCTests.OverrideOrNewTests
{
    [TestClass]
    public class OverrideNewTest
    {
        private class BaseClass
        {
            public virtual string GetString()
            {
                return "Base";
            }
        }

        private class ClassThatHidesViaNew : BaseClass
        {
            public new string GetString()
            {
                return "New";
            }
        }

        private class ClassThatExtendViaOverride : BaseClass
        {
            public override string GetString()
            {
                return "Override";
            }
        }

        private string GetString(BaseClass instance) => instance.GetString();

        [TestMethod]
        public void TestMethodBase()
        {
            Assert.AreEqual("Base", GetString(new BaseClass()));
        }

        [TestMethod]
        public void TestMethodNew()
        {
            Assert.AreEqual("Base", GetString(new ClassThatHidesViaNew()));
        }

        [TestMethod]
        public void TestMethodOverride()
        {
            Assert.AreEqual("Override", GetString(new ClassThatExtendViaOverride()));
        }
    }
}
