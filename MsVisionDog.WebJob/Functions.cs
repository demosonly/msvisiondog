using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using MsVisionDog.Common;

namespace MsVisionDog.WebJob
{
    public class Functions
    {
        public static void ProcessQueueMessage([QueueTrigger(StorageObjectNames.JobQueue)] JobQueueMessage jobMsg, [Table(StorageObjectNames.JobTable)] CloudTable jobTable, TextWriter log)
        {
            log.WriteLine(jobMsg.JobID);
        }
    }
}
