﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using MsVisionDog.Common;

namespace MsVisionDog.WebJob
{
    class Program
    {
        static void Main()
        {
            var host = new JobHost();
            host.RunAndBlock();
        }
    }
}
