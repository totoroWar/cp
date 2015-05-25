using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace GameServices
{
    public struct MR
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string OtherMessage { get; set; }
        public Exception Exception { get; set; }
        public int IntData { get; set; }
        public long LongData { get;set;}
        public List<Object> ObjectData{get;set;}
    }
}
