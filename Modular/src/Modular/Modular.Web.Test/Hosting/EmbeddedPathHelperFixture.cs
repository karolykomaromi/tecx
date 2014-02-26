namespace Modular.Web.Test.Hosting
{
    using System;
    using System.Collections.Generic;
    using Modular.Web.Hosting;
    using Xunit;

    public class EmbeddedPathHelperFixture
    {
        [Fact]
        public void Should_Transform_ManifestResourceName_To_App_Relative_Path()
        {
            string manifestResourceName = "Modular.Web.Test.Assets.Texts.Long.VeryLong.txt";

            string expected = "~/Texts/Long/VeryLong.txt";

            string actual = EmbeddedPathHelper.ToAppRelative(manifestResourceName);

            Assert.Equal(expected, actual, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Should_Get_AppRelative_Directories_From_ManifestResourceName()
        {
            string manifestResourceName = "Modular.Web.Test.Assets.Texts.Long.VeryLong.txt";

            IEnumerable<string> expected = new[] { "~/Texts/", "~/Texts/Long/" };

            IEnumerable<string> actual = EmbeddedPathHelper.GetDirectories(manifestResourceName);

            Assert.Equal(expected, actual, StringComparer.OrdinalIgnoreCase);
        }
    }
}
