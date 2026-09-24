using NUnit.Framework;
using WTFGames.Hephaestus.VersionsSystem.Editor;

namespace WTFGames.Hephaestus.VersionsSystem.Tests
{
    public class BuildNumberTests
    {
        [TestCase("0", "1")]
        [TestCase("41", "42")]
        [TestCase("9", "10")]
        [TestCase("1.0.7", "1.0.8")]
        [TestCase("2.9", "2.10")]
        [TestCase(" 5 ", "6")]
        [TestCase("", BuildNumber.Initial)]
        [TestCase(null, BuildNumber.Initial)]
        public void TryIncrement_ValidValue_IncrementsLastPart(string value, string expected)
        {
            Assert.IsTrue(BuildNumber.TryIncrement(value, out var result));
            Assert.AreEqual(expected, result);
        }

        [TestCase("1.2.3.4")]
        [TestCase("1.a")]
        [TestCase("1.")]
        [TestCase("-1")]
        [TestCase("2147483647")]
        public void TryIncrement_InvalidValue_ReturnsFalse(string value)
        {
            Assert.IsFalse(BuildNumber.TryIncrement(value, out _));
        }
    }
}
