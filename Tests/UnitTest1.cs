using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fractal;
using System.Drawing;
using System.Numerics;
using System;

namespace Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestSerial()
        {
            Serial obj = new Serial();
            obj.StepsX = obj.StepsY = 500;
            int[,] res = obj.Process();
        }

        [TestMethod]
        public void TestParallel()
        {
            FractalP obj = new FractalP();
            obj.StepsX = obj.StepsY = 500;
            int[,] res = obj.Process();
        }

        [TestMethod]
        public void TestInterfaceParallel()
        {
            FractalP obj = new FractalP(2, 150, z => z, (z1, z2) => z1 * z1 + z2, new Complex(-2, -2), new Complex(2, 2), Color.Red, k => Color.FromArgb(k));
            Assert.AreEqual(obj.MaxConstant, 2);
            Assert.AreEqual(obj.MaxIteration, 150);
            Assert.AreEqual(obj.F(new Complex(1, 2)), new Complex(1, 2));
            Assert.AreEqual(obj.G(new Complex(1, 2), new Complex(2, 2)), new Complex(-1, 6));
            Assert.AreEqual(obj.C1, Color.Red);
            Assert.AreEqual(obj.C2(10), Color.FromArgb(10));
        }


        [TestMethod]
        public void TestInterfaceSerial()
        {
            Serial obj = new Serial(2, 150, z => z, (z1, z2) => z1 * z1 + z2, new Complex(-2, -2), new Complex(2, 2), Color.Red, k => Color.FromArgb(k));
            Assert.AreEqual(obj.MaxConstant, 2);
            Assert.AreEqual(obj.MaxIteration, 150);
            Assert.AreEqual(obj.F(new Complex(1, 2)), new Complex(1, 2));
            Assert.AreEqual(obj.G(new Complex(1, 2), new Complex(2, 2)), new Complex(-1, 6));
            Assert.AreEqual(obj.C1, Color.Red);
            Assert.AreEqual(obj.C2(10), Color.FromArgb(10));
        }
    }
}
