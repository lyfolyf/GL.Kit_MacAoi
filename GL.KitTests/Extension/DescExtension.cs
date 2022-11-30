using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GL.Kit.Extension.Tests
{
    [TestClass]
    public class EnumExtensionTests
    {
        [TestMethod]
        public void ToDescriptionTest()
        {
            Assert.AreEqual("春季", Season.Spring.ToDescription());

            Assert.AreEqual("Winter", Season.Winter.ToDescription());
        }

        public enum Season
        {
            [System.ComponentModel.Description("春季")]
            Spring,

            [System.ComponentModel.Description("夏季")]
            Summer,

            [System.ComponentModel.Description("秋季")]
            Autumn,

            Winter
        }
    }
}