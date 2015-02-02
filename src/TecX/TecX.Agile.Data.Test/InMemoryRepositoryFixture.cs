using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.TestTools;

namespace TecX.Agile.Data.Test
{
    public abstract class Given_AnInMemoryRepository : GivenWhenThen
    {
        protected InMemoryRepository<Project> _repository;

        protected override void Given()
        {
            _repository = new InMemoryRepository<Project>();
        }
    }

    [TestClass]
    public class When_AddingAnItem : Given_AnInMemoryRepository
    {
        protected Project _project;

        protected override void When()
        {
            _project = new Project { Id = Guid.NewGuid(), Name = "1" };

            _repository.Add(_project);
        }

        [TestMethod]
        public void Then_ItemCanBeFoundById()
        {
            var project = _repository.FindById(_project.Id);

            Assert.AreSame(_project, project);
        }

        [TestMethod]
        public void Then_ItemCanBeFoundByQueryExpression()
        {
            var project = _repository.FindWhere(p => p.Name == "1").Single();

            Assert.AreSame(_project, project);
        }

        [TestMethod]
        public void Then_ItemIsIncludedInFindAll()
        {
            var projects = _repository.FindAll();

            Assert.AreEqual(1, projects.Count());
            Assert.AreSame(_project, projects.Single());
        }
    }

    public abstract class Given_ARepositoryWithAnItem : Given_AnInMemoryRepository
    {
        protected Project _project;

        protected override void Given()
        {
            base.Given();

            _project = new Project { Id = Guid.NewGuid(), Name = "1" };

            _repository.Add(_project);
        }
    }

    [TestClass]
    public class When_RemovingAnItem : Given_ARepositoryWithAnItem
    {
        protected override void When()
        {
            _repository.Remove(_project);
        }

        [TestMethod]
        public void Then_ItemIsNoLongerInRepository()
        {
            Assert.IsNull(_repository.FindById(_project.Id));
        }
    }
}
