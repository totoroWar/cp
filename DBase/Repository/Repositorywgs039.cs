using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DBase
{
    public class Repositorywgs039 : RepositoryBase<DBModel.wgs039>
    {
        public Repositorywgs039(DBBase context)
            :base(context)
        { 
        }
        public void GameSessionOtherClose()
        {
            context.Database.ExecuteSqlCommand("EXECUTE CLR_USP_GameSessionOtherClose;");
        }
    }
}
