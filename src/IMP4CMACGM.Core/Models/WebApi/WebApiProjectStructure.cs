using Newtonsoft.Json;
using System.Collections.Generic;

namespace IMP4CMACGM.Core.WebApi.Models
{
    public class WebApiProjectStructure : WebApiProjectInformation
    {
        [JsonProperty("webApiPackages")]
        public List<Package> WebApiPackages { get; set; }

        [JsonProperty("pollyWebApiPackage")]
        public Package PollyWebApiPackage { get; set; }

        [JsonProperty("steeltoeWebApiPackage")]
        public Package SteeltoeWebApiPackage { get; set; }

        [JsonProperty("mongoDBDriver")]
        public Package MongoDBDriver { get; set; }


        [JsonProperty("mongoDBDriverCore")]
        public Package MongoDBDriverCore { get; set; }

        [JsonProperty("mongoHealthCheck")]
        public Package MongoHealthCheck { get; set; }


        [JsonProperty("mongoBSON")]
        public Package MongoBSON { get; set; }

        [JsonProperty("subProjects")]
        public List<SubProject> SubProjects { get; set; }

        [JsonProperty("testProject")]
        public TestProject TestProject { get; set; }

    }

    public class SubProject
    {
        [JsonProperty("subProjectName")]
        public string SubProjectName { get; set; }

        [JsonProperty("subProjectPath")]
        public string SubProjectPath { get; set; }

        [JsonProperty("subProjectType")]
        public string SubProjectType { get; set; }

        [JsonProperty("projectDependencies")]
        public List<string> ProjectDependencies { get; set; }

        [JsonProperty("packages")]
        public List<Package> ProjectPackages { get; set; }

    }

    public class TestProject
    {
        [JsonProperty("rootTestDirectory")]
        public string RootTestDirectory { get; set; }

        [JsonProperty("testProjectNameSuffix")]
        public string TestProjectNameSuffix { get; set; }

        [JsonProperty("testProjectType")]
        public string TestProjectType { get; set; }

        [JsonProperty("projectDependencies")]
        public List<string> ProjectDependencies { get; set; }

        [JsonProperty("packages")]
        public List<Package> ProjectPackages { get; set; }
    }

    public class Package
    {
        [JsonProperty("packageName")]
        public string PackageName { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }


}
