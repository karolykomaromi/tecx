using System.Xml.Linq;

using TecX.Agile.Builder;
using TecX.Agile.Data.File;

namespace TecX.Agile.Data.Xml
{
    public class XmlProjectToStringConverter : ProjectToStringConverter
    {
        #region Overrides of ProjectToStringConverter

        protected override Project DoConvertToProject(string projectString)
        {
            XElement xml = XElement.Parse(projectString);

            Project project = New.Project();

            project.FromXml(xml);

            return project;
        }

        protected override string DoConvertToString(Project project)
        {
            XElement xml = project.ToXml();

            return xml.ToString();
        }

        #endregion
    }
}
