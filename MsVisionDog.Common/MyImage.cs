using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsVisionDog.Common
{
    public class MyImage
    {
        [Key]
        public string MyImageID { get; set; }
        public string UserID { get; set;  }
        public string Url { get; set; }
        public DateTime DateCreated { get; set; }
        public string DetailsJson { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ComputerVisionApiResponse { get; set; }
    }
}
