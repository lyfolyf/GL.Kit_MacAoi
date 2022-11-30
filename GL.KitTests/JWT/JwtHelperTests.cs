using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GL.WebApiKit.JWT.Tests
{
    [TestClass()]
    public class JwtHelperTests
    {
        [TestMethod()]
        public void EncodeTest()
        {
            var jwt = JwtSingle.Instance.JWT.Encode(new JwtTest { ID = 1, Name = "张三" });
        }
    }

    public class JwtTest
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
}