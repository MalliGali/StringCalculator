using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using StringCalculatorConsole;

namespace Calc.Tests
{
    [TestFixture]
    public class StringCalcTests
    {
        [Test]
        public void WhenEmptyString_ThenGiveZero()
        {
            string value = string.Empty;
            Assert.AreEqual(StringCalculator.Add(value), 0);
        }

        [Test]
        public void WhenIGiveValidStringWhichHasHigherThanThousand_ThenGetSumExcludingBiggerValues()
        {
            string value = "1,2,1002";
            Assert.AreEqual(StringCalculator.Add(value), 3);
        }

        [Test]
        public void WhenIGiveMultiliveValidString_ThenGetSumValue()
        {
            string value = "1\n2,3";
            Assert.AreEqual(StringCalculator.Add(value), 6);
        }

        [Test]
        public void WhenIGiveMultipleValueString_ThenGetSumValue()
        {
            string value = "3,3,5,23,78,223,89";
            Assert.AreEqual(StringCalculator.Add(value), 424);
        }

        [Test]
        public void WhenIGiveMultilineWithMultipleDelimiters_ThenGetSumValue()
        {
            string value = "//*$\n45*56\n34$32$4\n1*1999";
            Assert.AreEqual(StringCalculator.Add(value), 172);
        }

        [Test]
        public void WhenIGiveNegativeValues_ThenIGetException()
        {
            string value = "//*$\n45*56\n34$32$4\n1*1999$-34";
            var exception = Assert.Throws<Exception>(() => StringCalculator.Add(value));
            Assert.That(exception.Message.Contains("Negatives not allowed"));
        }

        [Test]
        public void WhenIGiveInvalidInputString_ThenIGetException()
        {
            string value = "jsdfh,asdla, 3423, 5, 643";
            var exception = Assert.Throws<Exception>(() => StringCalculator.Add(value));
            Assert.That(exception.Message.Contains("Input string has invlid characters"));
        }
        
        [Test]
        public void WhenIGiveDelimiterAtTheEndOfTheLine_ThenIGetException()
        {
            string value = "//*$\n45*56$\n34$32$4\n1*1999$2";
            var exception = Assert.Throws<Exception>(() => StringCalculator.Add(value));
            Assert.That(exception.Message.Contains("Line should be ended with number."));
        }
    }
}
