using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Common
{
    public class ModelCommon<T> : ResultEntity<T>
    {
    }
    public class SearchEntity<T>
    {
        public int errorCode { get; set; }
        public string errorMsg { get; set; }
        public T Data { get; set; }
    }

    public class ResultEntity<T> : SearchEntity<T>
    {
        public int pageIndex { get; set; }
        public int pageCount { get; set; }
        public int totalCount { get; set; }
    } 

}
