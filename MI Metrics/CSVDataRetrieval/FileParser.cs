using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public abstract class FileParser
    {
        public abstract List<IntermediateTestResult> RetrieveContentsFromFile();
    }
}
