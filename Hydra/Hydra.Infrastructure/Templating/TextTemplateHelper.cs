namespace Hydra.Infrastructure.Templating
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public static class TextTemplateHelper
    {
        public static string GetConstantsForAllCultures()
        {
            StringBuilder sb = new StringBuilder(500);

            var culturesByPropertyName = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Select(c => new { Name = StringHelper.ToValidPropertyName(c.EnglishName), Culture = c })
                .GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase);

            foreach (var group in culturesByPropertyName)
            {
                if (group.Count() > 1)
                {
                    foreach (var info in group)
                    {
                        string cultureConstant =
                            string.Format(
                                "        public static readonly CultureInfo {0}_{1} = new CultureInfo({1});",
                                info.Name,
                                info.Culture.LCID);

                        sb.AppendLine(cultureConstant);
                        sb.AppendLine();
                    }
                }
                else
                {
                    var info = group.Single();

                    string cultureConstant =
                        string.Format(
                            "        public static readonly CultureInfo {0} = new CultureInfo({1});",
                            info.Name,
                            info.Culture.LCID);

                    sb.AppendLine(cultureConstant);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }
}
