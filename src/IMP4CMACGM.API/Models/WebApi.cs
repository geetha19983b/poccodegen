using System.Collections.Generic;

namespace IMP4CMACGM.API.Models
{
    public class WebAPI
    {
        public string projectName { get; set; }

        public string preset { get; set; }

        public bool saveToGit { get; set; }

        public string namespaceName { get; set; }

        public string swaggerPath { get; set; }

        public List<string> databases { get; set; }

        public List<string> messaging { get; set; }

        public bool useCircuitBreaker { get; set; }

        public string gitUsername { get; set; }

        public string gitPassword { get; set; }

        public string gitURL { get; set; }

    }
}
