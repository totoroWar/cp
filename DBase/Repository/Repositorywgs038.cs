using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DBase
{
    public class Repositorywgs038 : RepositoryBase<DBModel.wgs038>
    {
        public Repositorywgs038(DBBase context)
            :base(context)
        { 
        }
        public void GameSessionOtherClose()
        {
            context.Database.ExecuteSqlCommand("EXECUTE CLR_USP_GameSessionOtherClose;");
        }
    }
}
