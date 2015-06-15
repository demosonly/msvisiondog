using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsVisionDog.Common
{
    public class MyEntity
    {
        [Key]
        public string MyEntityID { get; set; }
        public string UserID { get; set; }
        public string EntityType { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
