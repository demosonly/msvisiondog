using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsVisionDog.Common
{
    public class Job : TableEntity
    {
        public string JobType { get; set; }
        public DateTime? TimeStarted { get; set; }
        public DateTime? TimeEnded { get; set; }
        public double ElapsedSeconds { get; set; }
        public string JobStatus { get; set; }
        public string JobRequestJson { get; set; }
        public string JobResponseJson { get; set; }

        public Job(string jobID)
        {
            this.PartitionKey = jobID;
            this.RowKey = jobID;
        }
    }
}
