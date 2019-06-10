using Microsoft.VisualStudio.TestTools.UnitTesting;
using MuzikaClasses;
using MuzikaClasses.Rules;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestCellularAutomata
    {
        [TestMethod]
        public void TestLangtonIterate()
        {
            bool[,] StartingContent = new bool[8, 8]
                {
                    { false, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false },
                    { true, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false }
                };

            CircularArray2D<bool> StartingArray = new CircularArray2D<bool>(StartingContent);
            CellularAutomata<LangtonsAnt> _CellularAutomata = new CellularAutomata<LangtonsAnt>(StartingArray);

            bool[,] IteratedContent = _CellularAutomata.Iterate();

            bool[,] ModelContent = new bool[8, 8]
                 {
                    { false, false, false, false, true, false, false, false },
                    { false, false, false, false, false, false, false, false },
                    { false, true, false, false, false, true, false, false },
                    { false, false, false, false, false, false, false, false },
                    { false, false, true, false, false, false, true, false },
                    { false, false, false, false, false, false, false, false },
                    { false, false, false, true, false, false, false, true },
                    { false, false, false, false, false, false, false, false }
                 };

            Assert.AreEqual(IteratedContent, ModelContent);
        }

        [TestMethod]
        public void TestFrequencyCalculator()
        {
            int Keys = 9;
            int aKeyDifference = 11;

            float frequency = Frequency.CalculateFrequencyBySemitone((short)Numbers.AllMaj[Keys][aKeyDifference]) / 440;

            Assert.AreEqual(0.25f, frequency);
        }
    }
}
