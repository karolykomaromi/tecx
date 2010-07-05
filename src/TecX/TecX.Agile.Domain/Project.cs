using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Agile.Domain
{
    public class Project
    {
        #region Properties

        public string Vision { get; set; }

        public string Name { get; set; }

        public List<UserStory> Backlog { get; private set; }

        #endregion Properties

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class
        /// </summary>
        public Project()
        {
            Vision = string.Empty;

            Name = string.Empty;

            Backlog = new List<UserStory>();
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////
    }
}
