using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Net.Http.Tests
{
    [TestClass()]
    public class UrlTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            string urlStr = "http://localhost:7001/api/employees?id=1&name=jack";

            Url url = Url.Parse(urlStr);

            Assert.AreEqual(1, Convert.ToInt32(url.Queries["id"]));
            Assert.AreEqual("jack", (string)url.Queries["name"]);
        }
    }
}