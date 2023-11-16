using World;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private TestContext _testContext;

        public TestContext TestContext
        {
            get => _testContext;
            set => _testContext = value;
        }

        [TestMethod]
        public void TestMethod1()
        {
            TestContext.WriteLine(NoiseSampler.GetHeight(0, 0).ToString());
        }
    }
}