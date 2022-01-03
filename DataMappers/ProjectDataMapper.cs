using System;
using GreenOnion.DomainModels;

namespace GreenOnion.DataMappers
{
    public class ProjectDataMapper
    {
        public ProjectDataMapper()
        {
        }

        public bool insert(Project project)
        {
            return true;
        }

        public bool deleteBy(string projID)
        {
            return true;
        }

        public Project select(string projID)
        {
            return new Project();
        }

        public bool update(Project project)
        {
            return true;
        }
    }
}
