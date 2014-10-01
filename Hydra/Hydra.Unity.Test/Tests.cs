namespace Hydra.Unity.Test
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using Xunit;

    public class Tests
    {
        [Fact]
        public void Should_Do_What_I_Want_Not_What_I_Say()
        {
            DateTimeFormatInfo format = CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat;

            string shortDatePattern = format.ShortDatePattern;
        }
    }
}
