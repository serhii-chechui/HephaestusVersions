using System;
using NUnit.Framework;
using WTFGames.Hephaestus.VersionsSystem.Editor;

namespace WTFGames.Hephaestus.VersionsSystem.Tests
{
    public class SemanticVersionTests
    {
        [TestCase("1.2.3", 1, 2, 3)]
        [TestCase("1.2", 1, 2, 0)]
        [TestCase("7", 7, 0, 0)]
        [TestCase(" 0.1.0 ", 0, 1, 0)]
        [TestCase("1.2.0-beta.1", 1, 2, 0)]
        [TestCase("1.2.0+42", 1, 2, 0)]
        public void TryParse_ValidValue_ReturnsParts(string value, int major, int minor, int patch)
        {
            Assert.IsTrue(SemanticVersion.TryParse(value, out var version));
            Assert.AreEqual(new SemanticVersion(major, minor, patch), version);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("1.2.3.4")]
        [TestCase("1..2")]
        [TestCase("a.b.c")]
        [TestCase("-1.0.0")]
        [TestCase("v1.0.0")]
        public void TryParse_InvalidValue_ReturnsFalse(string value)
        {
            Assert.IsFalse(SemanticVersion.TryParse(value, out _));
        }

        [Test]
        public void Parse_InvalidValue_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => SemanticVersion.Parse("abc"));
        }

        [TestCase(VersionPart.Major, "2.0.0")]
        [TestCase(VersionPart.Minor, "1.3.0")]
        [TestCase(VersionPart.Patch, "1.2.4")]
        public void Bump_ResetsLowerParts(VersionPart part, string expected)
        {
            var version = new SemanticVersion(1, 2, 3);

            Assert.AreEqual(expected, version.Bump(part).ToString());
        }

        [TestCase(VersionPart.Major, "0.2.3")]
        [TestCase(VersionPart.Minor, "1.0.3")]
        [TestCase(VersionPart.Patch, "1.2.0")]
        public void Reset_SetsOnlyThatPartToZero(VersionPart part, string expected)
        {
            var version = new SemanticVersion(1, 2, 3);

            Assert.AreEqual(expected, version.Reset(part).ToString());
        }

        [Test]
        public void Constructor_NegativePart_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SemanticVersion(1, -1, 0));
        }
    }
}
