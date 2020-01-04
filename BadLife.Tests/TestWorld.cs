using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BadLife.Tests
{
    [TestClass]
    public class TestWorld
    {
        // TODO: Add all test cases for (general case + corners + borders):
        // Any live cell with fewer than two live neighbors dies, as if by underpopulation.
        // Any live cell with two or three live neighbors lives on to the next generation.
        // Any live cell with more than three live neighbors dies, as if by overpopulation.
        // Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.

        // TODO: Add all test cases to catch wrong input
        // Insufficient number of rows or cols
        // Unexpected characters

        [TestMethod]
        public void TestCaseAllAlive()
        {
            string[] Start =
            {
                "***",
                "***",
                "***",
            };

            string[] End =
            {
                "___",
                "___",
                "___",
            };

            Check(Start, End);
        }

        [TestMethod]
        public void TestCaseAllAliveApartFromOne()
        {
            string[] Start =
            {
                "***",
                "*_*",
                "***",
            };

            string[] End =
            {
                "___",
                "___",
                "___",
            };

            Check(Start, End);
        }

        [TestMethod]
        public void TestCaseAllDead()
        {
            string[] Start =
            {
                "___",
                "___",
                "___",
            };

            string[] End =
            {
                "___",
                "___",
                "___",
            };

            Check(Start, End);
        }

        [TestMethod]
        public void TestCaseAllDeadApartFromOne()
        {
            string[] Start =
            {
                "___",
                "_*_",
                "___",
            };

            string[] End =
            {
                "___",
                "___",
                "___",
            };

            Check(Start, End);
        }

        [TestMethod]
        public void TestStaticVerticalBeehive()
        {
            string[] Start =
            {
                "_____",
                "__*__",
                "_*_*_",
                "_*_*_",
                "__*__",
            };

            Check(Start, Start);
        }

        [TestMethod]
        public void TestOscillator()
        {
            string[] Start =
            {
                "_____",
                "_____",
                "_***_",
                "_____",
                "_____",
            };

            string[] End =
            {
                "_____",
                "__*__",
                "__*__",
                "__*__",
                "_____",
            };

            Check(Start, End);
            Check(End, Start);
        }

        [TestMethod]
        public void TestLoading()
        {
            World world = World.LoadFromTextFile(@".\..\..\..\BadLife\sample_input.txt");
        }

        // Using string representation in order to have an easy way to represent test cases
        private static void Check(string[] start, string[] end)
        {
            string startString = string.Join(Environment.NewLine, start);
            World world = World.LoadFromString(startString);
            
            // Check we load correctly the initial configuration
            Assert.AreEqual(startString, world.AsString());
            
            world.Evolve();
            string endString = string.Join(Environment.NewLine, end);
            
            // Check that the evolution worked as expected
            Assert.AreEqual(endString, world.AsString());
        }
    }
}
