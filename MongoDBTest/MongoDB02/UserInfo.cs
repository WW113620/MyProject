using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Common
{
    [BsonIgnoreExtraElements]
    public class UserInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string UserGuid { get; set; }
        public int UserType { get; set; }
        public DateTime DataTime { get; set; }
    }
}
