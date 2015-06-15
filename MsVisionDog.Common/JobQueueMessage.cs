using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsVisionDog.Common
{
    public class JobQueueMessage
    {
        public string JobID { get; set; }
        public string JobType { get; set; }
    }
}
