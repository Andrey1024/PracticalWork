using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fractal;

namespace Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestSerial()
        {
            IFractal obj = new Serial();
            int[,] res = obj.Process(1000);
        }

        [TestMethod]
        public void TestParallel()
        {
            IFractal obj = new FractalP();
            int[,] res = obj.Process(1000);
        }

        public void TestInterface()
        {
            IFractal obj = new FractalP();
        }
    }
}
