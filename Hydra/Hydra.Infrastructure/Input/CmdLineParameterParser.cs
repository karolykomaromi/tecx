namespace Hydra.Infrastructure.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class CmdLineParameterParser
    {
        private readonly Regex regex;

        public CmdLineParameterParser()
        {
            this.regex = new Regex(@"/\w+(\s\w+(\s\w+)*)?", RegexOptions.Compiled);
        }

        public virtual IEnumerable<CmdLineParameter> Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                yield break;
            }

            var matches = this.regex.Matches(s);

            foreach (Match match in matches)
            {
                string[] m = match.Value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (m.Length < 2)
                {
                    yield return new NoValueCmdLineParameter(m[0].TrimStart('/'));
                }

                if (m.Length == 2)
                {
                    yield return new CmdLineParameter(m[0].TrimStart('/'), m[1]);
                }
                
                if (m.Length > 2)
                {
                    yield return new MultiValueCmdLineParameter(m[0].TrimStart('/'), m.Skip(1).ToArray());
                }
            }
        }
    }
}