using TecX.Common;

namespace TecX.Agile.Data.File
{
    public abstract class ProjectToStringConverter
    {
        public string ConvertToString(Project project)
        {
            Guard.AssertNotNull(project, "project");

            string projectAsString = DoConvertToString(project);

            return projectAsString;
        }

        public Project ConvertToProject(string projectString)
        {
            Guard.AssertNotEmpty(projectString, "projectString");

            Project project = DoConvertToProject(projectString);

            return project;
        }

        protected abstract Project DoConvertToProject(string projectString);

        protected abstract string DoConvertToString(Project project);
    }
}