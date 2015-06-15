using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsVisionDog.Common
{
    public class MyEntityImage
    {
        [Key]
        public int MyEntityImageID { get; set; }
        public string MyEntityID { get; set; }
        public string MyImageID { get; set; }
        public string UserID { get; set; }
        public DateTime DateCreated { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
