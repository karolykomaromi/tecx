namespace BankOcrKata
{
    using System;

    using TecX.Katas.BankOcr;

    using Xunit;

    public class DigitFixture
    {
        [Fact]
        public void CanCastZero()
        {
            Assert.Equal((Digit)0, Digit.Zero);
        }

        [Fact]
        public void CanCastOne()
        {
            Assert.Equal((Digit)1, Digit.One);
        }

        [Fact]
        public void CanCastTwo()
        {
            Assert.Equal((Digit)2, Digit.Two);
        }

        [Fact]
        public void CanCastThree()
        {
            Assert.Equal((Digit)3, Digit.Three);
        }

        [Fact]
        public void CanCastFour()
        {
            Assert.Equal((Digit)4, Digit.Four);
        }

        [Fact]
        public void CanCastFive()
        {
            Assert.Equal((Digit)5, Digit.Five);
        }

        [Fact]
        public void CanCastSix()
        {
            Assert.Equal((Digit)6, Digit.Six);
        }

        [Fact]
        public void CanCastSeven()
        {
            Assert.Equal((Digit)7, Digit.Seven);
        }

        [Fact]
        public void CanCastEight()
        {
            Assert.Equal((Digit)8, Digit.Eight);
        }

        [Fact]
        public void CanCastNine()
        {
            Assert.Equal((Digit)9, Digit.Nine);
        }

        [Fact]
        public void CastingTooSmallNumbersThrows()
        {
            Assert.ThrowsDelegate action = () =>
                {
                    Digit d = -1;
                };

            Assert.Throws<InvalidCastException>(action);
        }

        [Fact]
        public void CastingTooLargeNumbersThrows()
        {
            Assert.ThrowsDelegate action = () =>
                {
                    Digit d = 10;
                };

            Assert.Throws<InvalidCastException>(action);
        }
    }
}