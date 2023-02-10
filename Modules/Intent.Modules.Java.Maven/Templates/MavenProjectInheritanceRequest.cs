using System;
using System.Collections.Generic;
using System.Text;
using Intent.Modules.Common.Java.Templates;

namespace Intent.Modules.Java.Maven.Templates
{
    public class MavenProjectInheritanceRequest : JavaDependency
    {
        public MavenProjectInheritanceRequest(string groupId, string artifactId, string version = null) : base(groupId, artifactId, version)
        {
        }
    }
}
