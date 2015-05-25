using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DBase
{
    public class Repositorywgs001 : RepositoryBase<DBModel.wgs001>
    {
        public Repositorywgs001(DBBase context)
            : base(context)
        {
        }
    }
}
