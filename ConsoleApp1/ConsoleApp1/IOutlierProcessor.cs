using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoLibrary
{
        public interface IOutlierProcessor
        {
            // handle the parts of contents             
            void Request( int movingIdx,int lookupRng);
            
            // for unit test
            List<string> GetTestResult();
        }
}
