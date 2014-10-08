using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//using TecX.Agile.Data.Tfs;

namespace TecX.Agile.Data.Test
{
    [TestClass]
    public class TfsRepositoryFixture
    {
        [TestMethod]
        public void MustReimplement()
        {
            Assert.Fail("must reimplement");
        }

        //[TestMethod]
        //public void GivenRepositoryAndLocalTfs_WhenGettingAvailableProjects_GetsAllProjects()
        //{
        //    Uri tfsUri = new Uri("http://WKWEBERSE02:8080/TFS");

        //    TfsRepository repository = new TfsRepository(tfsUri);

        //    var projects = repository.GetExistingProjects();
        //}
    }
}
