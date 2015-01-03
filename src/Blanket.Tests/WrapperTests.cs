using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blanket.Tests
{
    [TestClass]
    public class WrapperTests
    {
        [TestMethod]
        public void UrlCreation1()
        {
            var wrapper = Wrapper.Wrap("https://api.github.com");

            Assert.AreEqual("https://api.github.com/Users/RCBDev", wrapper.Users("RCBDev").Url);
        }

        [TestMethod]
        public void UrlCreation2()
        {
            var wrapper = Wrapper.Wrap("https://api.github.com");

            Assert.AreEqual("https://api.github.com/Users/RCBDev/Repos", wrapper.Users("RCBDev").Repos.Url);
        }

        [TestMethod]
        public void WrapperNotModified()
        {
            var wrapper = Wrapper.Wrap("https://api.github.com");

            wrapper.Users("RCBDev");

            Assert.AreEqual("https://api.github.com/", wrapper.Url);
        }

        [TestMethod]
        public void SlashAtEndOfBaseUrlSupported()
        {
            var wrapper = Wrapper.Wrap("https://api.github.com/");

            Assert.AreEqual("https://api.github.com/Users/RCBDev", wrapper.Users("RCBDev").Url);
        }

        [TestMethod]
        public void NoSlashAtEndOfBaseUrlSupported()
        {
            var wrapper = Wrapper.Wrap("https://api.github.com");

            Assert.AreEqual("https://api.github.com/Users/RCBDev", wrapper.Users("RCBDev").Url);
        }
    }
}
