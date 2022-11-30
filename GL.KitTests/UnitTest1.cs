using System;
using GL.Kit.Log;
using GL.Kit.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GL.KitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            LogLevel level = LogLevel.Debug;

            Assert.IsTrue(level >= LogLevel.Debug);
        }
    }
}
