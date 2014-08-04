using Newtonsoft.Json.Linq;

using TecX.Agile.Builder;
using TecX.Agile.Data.File;

namespace TecX.Agile.Data.Json
{
    class JsonProjectToStringConverter : ProjectToStringConverter
    {
        #region Overrides of ProjectToStringConverter

        protected override Project DoConvertToProject(string projectString)
        {
            JObject json = JObject.Parse(projectString);

            Project project = New.Project();

            project.FromJson(json);

            return project;
        }

        protected override string DoConvertToString(Project project)
        {
            string json = project.ToJson().ToString();

            return json;
        }

        #endregion
    }
}
