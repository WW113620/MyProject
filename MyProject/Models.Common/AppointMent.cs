using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Common
{
    public class AppointMent
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string SId { get; set; }
        [MaxLength(500)]
        public string Message { get; set; }
        public int AcceptanceID { get; set; }
    }
    public class AppointMents : AppointMent
    {
        public string SName { get; set; }
        public string AcceptanceName { get; set; }
    }
}
