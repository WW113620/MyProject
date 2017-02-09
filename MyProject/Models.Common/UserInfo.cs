using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Common
{
    [Serializable]
    public class UserInfo
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        public string UserGuid { get; set; }
        public int UserType { get; set; }
    }
}
