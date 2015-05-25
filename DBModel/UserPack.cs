using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel
{
    public class UserPack
    {
        public wgs012 User { get; set; }
        public int ChildCount { get; set; }
        public List<wgs017> PointList { get; set; }
    }
}
