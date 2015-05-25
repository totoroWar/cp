using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
namespace DBase
{
    public class UnitOfWork : IDisposable
    {
        internal DBBase context = null;
        protected bool disposed = false;
        protected DbTransaction tran = null;
        public UnitOfWork()
        {
            context = new DBBase();
        }
        public UnitOfWork(bool read)
        {
            context = new DBBase(read);
        }
        public int SaveChanges()
        {
            return context.SaveChanges();
        }
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (null != tran)
                {
                    tran.Dispose();
                }
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        #region TS
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            tran = context.Database.Connection.BeginTransaction(isolationLevel);
        }
        public void BeginTransaction()
        {
            tran = context.Database.Connection.BeginTransaction();
        }
        public void Commit()
        {
            tran.Commit();
        }
        #endregion
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public IEnumerable<T> SqlQuery<T>(string sql, params object[] values)
        {
            return context.Database.SqlQuery<T>(sql, values);
        }
        public int ExecuteSqlCommand(string sql, params object[] values)
        {
            return context.Database.ExecuteSqlCommand(sql, values);
        }
        #region 统一上下
        private Repositorywgs001 _Repositorywgs001 = null;
        private Repositorywgs002 _Repositorywgs002 = null;
        private Repositorywgs003 _Repositorywgs003 = null;
        private Repositorywgs004 _Repositorywgs004 = null;
        private Repositorywgs005 _Repositorywgs005 = null;
        private Repositorywgs006 _Repositorywgs006 = null;
        private Repositorywgs007 _Repositorywgs007 = null;
        private Repositorywgs008 _Repositorywgs008 = null;
        private Repositorywgs009 _Repositorywgs009 = null;
        private Repositorywgs010 _Repositorywgs010 = null;
        private Repositorywgs011 _Repositorywgs011 = null;
        private Repositorywgs012 _Repositorywgs012 = null;
        private Repositorywgs013 _Repositorywgs013 = null;
        private Repositorywgs014 _Repositorywgs014 = null;
        private Repositorywgs015 _Repositorywgs015 = null;
        private Repositorywgs016 _Repositorywgs016 = null;
        private Repositorywgs017 _Repositorywgs017 = null;
        private Repositorywgs018 _Repositorywgs018 = null;
        private Repositorywgs019 _Repositorywgs019 = null;
        private Repositorywgs020 _Repositorywgs020 = null;
        private Repositorywgs021 _Repositorywgs021 = null;
        private Repositorywgs022 _Repositorywgs022 = null;
        private Repositorywgs023 _Repositorywgs023 = null;
        private Repositorywgs024 _Repositorywgs024 = null;
        private Repositorywgs025 _Repositorywgs025 = null;
        private Repositorywgs026 _Repositorywgs026 = null;
        private Repositorywgs027 _Repositorywgs027 = null;
        private Repositorywgs028 _Repositorywgs028 = null;
        private Repositorywgs029 _Repositorywgs029 = null;
        private Repositorywgs030 _Repositorywgs030 = null;
        private Repositorywgs031 _Repositorywgs031 = null;
        private Repositorywgs032 _Repositorywgs032 = null;
        private Repositorywgs033 _Repositorywgs033 = null;
        private Repositorywgs034 _Repositorywgs034 = null;
        private Repositorywgs035 _Repositorywgs035 = null;
        private Repositorywgs036 _Repositorywgs036 = null;
        private Repositorywgs037 _Repositorywgs037 = null;
        private Repositorywgs038 _Repositorywgs038 = null;
        private Repositorywgs039 _Repositorywgs039 = null;
        private Repositorywgs040 _Repositorywgs040 = null;
        private Repositorywgs041 _Repositorywgs041 = null;
        private Repositorywgs042 _Repositorywgs042 = null;
        private Repositorywgs043 _Repositorywgs043 = null;
        private Repositorywgs044 _Repositorywgs044 = null;
        private Repositorywgs045 _Repositorywgs045 = null;
        private Repositorywgs046 _Repositorywgs046 = null;
        private Repositorywgs047 _Repositorywgs047 = null;
        private Repositorywgs048 _Repositorywgs048 = null;
        private Repositorywgs049 _Repositorywgs049 = null;
        private Repositorywgs050 _Repositorywgs050 = null;
        private Repositorywgs051 _Repositorywgs051 = null;
        private Repositorywgs052 _Repositorywgs052 = null;
        private Repositorywgs053 _Repositorywgs053 = null;
        private Repositorywgs054 _Repositorywgs054 = null;
        private Repositorywgs055 _Repositorywgs055 = null;
        private Repositorywgs056 _Repositorywgs056 = null;
        private Repositorywgs057 _Repositorywgs057 = null;
        public Repositorywgs001 Repositorywgs001
        {
            get
            {
                if (null == _Repositorywgs001)
                {
                    _Repositorywgs001 = new Repositorywgs001(context);
                }
                return _Repositorywgs001;
            }
        }
        public Repositorywgs002 Repositorywgs002
        {
            get
            {
                if (null == _Repositorywgs002)
                {
                    _Repositorywgs002 = new Repositorywgs002(context);
                }
                return _Repositorywgs002;
            }
        }
        public Repositorywgs003 Repositorywgs003
        {
            get
            {
                if (null == _Repositorywgs003)
                {
                    _Repositorywgs003 = new Repositorywgs003(context);
                }
                return _Repositorywgs003;
            }
        }
        public Repositorywgs004 Repositorywgs004
        {
            get 
            {
                if (null == _Repositorywgs004)
                {
                    _Repositorywgs004 = new Repositorywgs004(context);
                }
                return _Repositorywgs004;
            }
        }
        public Repositorywgs005 Repositorywgs005
        {
            get
            {
                if (null == _Repositorywgs005)
                {
                    _Repositorywgs005 = new Repositorywgs005(context);
                }
                return _Repositorywgs005;
            }
        }
        public Repositorywgs006 Repositorywgs006
        {
            get
            {
                if (null == _Repositorywgs006)
                {
                    _Repositorywgs006 = new Repositorywgs006(context);
                }
                return _Repositorywgs006;
            }
        }
        public Repositorywgs007 Repositorywgs007
        {
            get
            {
                if (null == _Repositorywgs007)
                {
                    _Repositorywgs007 = new Repositorywgs007(context);
                }
                return _Repositorywgs007;
            }
        }
        public Repositorywgs008 Repositorywgs008
        {
            get
            {
                if (null == _Repositorywgs008)
                {
                    _Repositorywgs008 = new Repositorywgs008(context);
                }
                return _Repositorywgs008;
            }
        }
        public Repositorywgs009 Repositorywgs009
        {
            get
            {
                if (null == _Repositorywgs009)
                {
                    _Repositorywgs009 = new Repositorywgs009(context);
                }
                return _Repositorywgs009;
            }
        }
        public Repositorywgs010 Repositorywgs010
        {
            get
            {
                if (null == _Repositorywgs010)
                {
                    _Repositorywgs010 = new Repositorywgs010(context);
                }
                return _Repositorywgs010;
            }
        }
        public Repositorywgs011 Repositorywgs011
        {
            get
            {
                if (null == _Repositorywgs011)
                {
                    _Repositorywgs011 = new Repositorywgs011(context);
                }
                return _Repositorywgs011;
            }
        }
        public Repositorywgs012 Repositorywgs012
        {
            get
            {
                if (null == _Repositorywgs012)
                {
                    _Repositorywgs012 = new Repositorywgs012(context);
                }
                return _Repositorywgs012;
            }
        }
        public Repositorywgs013 Repositorywgs013
        {
            get
            {
                if (null == _Repositorywgs013)
                {
                    _Repositorywgs013 = new Repositorywgs013(context);
                }
                return _Repositorywgs013;
            }
        }
        public Repositorywgs014 Repositorywgs014
        {
            get
            {
                if (null == _Repositorywgs014)
                {
                    _Repositorywgs014 = new Repositorywgs014(context);
                }
                return _Repositorywgs014;
            }
        }
        public Repositorywgs015 Repositorywgs015
        {
            get
            {
                if (null == _Repositorywgs015)
                {
                    _Repositorywgs015 = new Repositorywgs015(context);
                }
                return _Repositorywgs015;
            }
        }
        public Repositorywgs016 Repositorywgs016
        {
            get
            {
                if (null == _Repositorywgs016)
                {
                    _Repositorywgs016 = new Repositorywgs016(context);
                }
                return _Repositorywgs016;
            }
        }
        public Repositorywgs017 Repositorywgs017
        {
            get
            {
                if (null == _Repositorywgs017)
                {
                    _Repositorywgs017 = new Repositorywgs017(context);
                }
                return _Repositorywgs017;
            }
        }
        public Repositorywgs018 Repositorywgs018
        {
            get
            {
                if (null == _Repositorywgs018)
                {
                    _Repositorywgs018 = new Repositorywgs018(context);
                }
                return _Repositorywgs018;
            }
        }
        public Repositorywgs019 Repositorywgs019
        {
            get
            {
                if (null == _Repositorywgs019)
                {
                    _Repositorywgs019 = new Repositorywgs019(context);
                }
                return _Repositorywgs019;
            }
        }
        public Repositorywgs020 Repositorywgs020
        {
            get
            {
                if (null == _Repositorywgs020)
                {
                    _Repositorywgs020 = new Repositorywgs020(context);
                }
                return _Repositorywgs020;
            }
        }
        public Repositorywgs021 Repositorywgs021
        {
            get
            {
                if (null == _Repositorywgs021)
                {
                    _Repositorywgs021 = new Repositorywgs021(context);
                }
                return _Repositorywgs021;
            }
        }
        public Repositorywgs022 Repositorywgs022
        {
            get
            {
                if (null == _Repositorywgs022)
                {
                    _Repositorywgs022 = new Repositorywgs022(context);
                }
                return _Repositorywgs022;
            }
        }
        public Repositorywgs023 Repositorywgs023
        {
            get
            {
                if (null == _Repositorywgs023)
                {
                    _Repositorywgs023 = new Repositorywgs023(context);
                }
                return _Repositorywgs023;
            }
        }
        public Repositorywgs024 Repositorywgs024
        {
            get
            {
                if (null == _Repositorywgs024)
                {
                    _Repositorywgs024 = new Repositorywgs024(context);
                }
                return _Repositorywgs024;
            }
        }
        public Repositorywgs025 Repositorywgs025
        {
            get
            {
                if (null == _Repositorywgs025)
                {
                    _Repositorywgs025 = new Repositorywgs025(context);
                }
                return _Repositorywgs025;
            }
        }
        public Repositorywgs026 Repositorywgs026
        {
            get
            {
                if (null == _Repositorywgs026)
                {
                    _Repositorywgs026 = new Repositorywgs026(context);
                }
                return _Repositorywgs026;
            }
        }
        public Repositorywgs027 Repositorywgs027
        {
            get
            {
                if (null == _Repositorywgs027)
                {
                    _Repositorywgs027 = new Repositorywgs027(context);
                }
                return _Repositorywgs027;
            }
        }
        public Repositorywgs028 Repositorywgs028
        {
            get
            {
                if (null == _Repositorywgs028)
                {
                    _Repositorywgs028 = new Repositorywgs028(context);
                }
                return _Repositorywgs028;
            }
        }
        public Repositorywgs029 Repositorywgs029
        {
            get
            {
                if (null == _Repositorywgs029)
                {
                    _Repositorywgs029 = new Repositorywgs029(context);
                }
                return _Repositorywgs029;
            }
        }
        public Repositorywgs030 Repositorywgs030
        {
            get
            {
                if (null == _Repositorywgs030)
                {
                    _Repositorywgs030 = new Repositorywgs030(context);
                }
                return _Repositorywgs030;
            }
        }
        public Repositorywgs031 Repositorywgs031
        {
            get
            {
                if (null == _Repositorywgs031)
                {
                    _Repositorywgs031 = new Repositorywgs031(context);
                }
                return _Repositorywgs031;
            }
        }
        public Repositorywgs032 Repositorywgs032
        {
            get
            {
                if (null == _Repositorywgs032)
                {
                    _Repositorywgs032 = new Repositorywgs032(context);
                }
                return _Repositorywgs032;
            }
        }
        public Repositorywgs033 Repositorywgs033
        {
            get
            {
                if (null == _Repositorywgs033)
                {
                    _Repositorywgs033 = new Repositorywgs033(context);
                }
                return _Repositorywgs033;
            }
        }
        public Repositorywgs034 Repositorywgs034
        {
            get
            {
                if (null == _Repositorywgs034)
                {
                    _Repositorywgs034 = new Repositorywgs034(context);
                }
                return _Repositorywgs034;
            }
        }
        public Repositorywgs035 Repositorywgs035
        {
            get
            {
                if (null == _Repositorywgs035)
                {
                    _Repositorywgs035 = new Repositorywgs035(context);
                }
                return _Repositorywgs035;
            }
        }
        public Repositorywgs036 Repositorywgs036
        {
            get
            {
                if (null == _Repositorywgs036)
                {
                    _Repositorywgs036 = new Repositorywgs036(context);
                }
                return _Repositorywgs036;
            }
        }
        public Repositorywgs037 Repositorywgs037
        {
            get
            {
                if (null == _Repositorywgs037)
                {
                    _Repositorywgs037 = new Repositorywgs037(context);
                }
                return _Repositorywgs037;
            }
        }
        public Repositorywgs038 Repositorywgs038
        {
            get
            {
                if (null == _Repositorywgs038)
                {
                    _Repositorywgs038 = new Repositorywgs038(context);
                }
                return _Repositorywgs038;
            }
        }
        public Repositorywgs039 Repositorywgs039
        {
            get
            {
                if (null == _Repositorywgs039)
                {
                    _Repositorywgs039 = new Repositorywgs039(context);
                }
                return _Repositorywgs039;
            }
        }
        public Repositorywgs040 Repositorywgs040
        {
            get
            {
                if (null == _Repositorywgs040)
                {
                    _Repositorywgs040 = new Repositorywgs040(context);
                }
                return _Repositorywgs040;
            }
        }
        public Repositorywgs041 Repositorywgs041
        {
            get
            {
                if (null == _Repositorywgs041)
                {
                    _Repositorywgs041 = new Repositorywgs041(context);
                }
                return _Repositorywgs041;
            }
        }
        public Repositorywgs042 Repositorywgs042
        {
            get
            {
                if (null == _Repositorywgs042)
                {
                    _Repositorywgs042 = new Repositorywgs042(context);
                }
                return _Repositorywgs042;
            }
        }
        public Repositorywgs043 Repositorywgs043
        {
            get
            {
                if (null == _Repositorywgs043)
                {
                    _Repositorywgs043 = new Repositorywgs043(context);
                }
                return _Repositorywgs043;
            }
        }
        public Repositorywgs044 Repositorywgs044
        {
            get
            {
                if (null == _Repositorywgs044)
                {
                    _Repositorywgs044 = new Repositorywgs044(context);
                }
                return _Repositorywgs044;
            }
        }
        public Repositorywgs045 Repositorywgs045
        {
            get
            {
                if (null == _Repositorywgs045)
                {
                    _Repositorywgs045 = new Repositorywgs045(context);
                }
                return _Repositorywgs045;
            }
        }
        public Repositorywgs046 Repositorywgs046
        {
            get
            {
                if (null == _Repositorywgs046)
                {
                    _Repositorywgs046 = new Repositorywgs046(context);
                }
                return _Repositorywgs046;
            }
        }
        public Repositorywgs047 Repositorywgs047
        {
            get
            {
                if (null == _Repositorywgs047)
                {
                    _Repositorywgs047 = new Repositorywgs047(context);
                }
                return _Repositorywgs047;
            }
        }
        public Repositorywgs048 Repositorywgs048
        {
            get
            {
                if (null == _Repositorywgs048)
                {
                    _Repositorywgs048 = new Repositorywgs048(context);
                }
                return _Repositorywgs048;
            }
        }
        public Repositorywgs049 Repositorywgs049
        {
            get
            {
                if (null == _Repositorywgs049)
                {
                    _Repositorywgs049 = new Repositorywgs049(context);
                }
                return _Repositorywgs049;
            }
        }
        public Repositorywgs050 Repositorywgs050
        {
            get
            {
                if (null == _Repositorywgs050)
                {
                    _Repositorywgs050 = new Repositorywgs050(context);
                }
                return _Repositorywgs050;
            }
        }
        public Repositorywgs051 Repositorywgs051
        {
            get
            {
                if (null == _Repositorywgs051)
                {
                    _Repositorywgs051 = new Repositorywgs051(context);
                }
                return _Repositorywgs051;
            }
        }
        public Repositorywgs052 Repositorywgs052
        {
            get
            {
                if (null == _Repositorywgs052)
                {
                    _Repositorywgs052 = new Repositorywgs052(context);
                }
                return _Repositorywgs052;
            }
        }
        public Repositorywgs053 Repositorywgs053
        {
            get
            {
                if (null == _Repositorywgs053)
                {
                    _Repositorywgs053 = new Repositorywgs053(context);
                }
                return _Repositorywgs053;
            }
        }
        public Repositorywgs054 Repositorywgs054
        {
            get
            {
                if (null == _Repositorywgs054)
                {
                    _Repositorywgs054 = new Repositorywgs054(context);
                }
                return _Repositorywgs054;
            }
        }

        public Repositorywgs055 Repositorywgs055
        {
            get
            {
                if (null == _Repositorywgs055)
                {
                    _Repositorywgs055 = new Repositorywgs055(context);
                }
                return _Repositorywgs055;
            }
        }
        public Repositorywgs056 Repositorywgs056
        {
            get
            {
                if (null == _Repositorywgs056)
                {
                    _Repositorywgs056 = new Repositorywgs056(context);
                }
                return _Repositorywgs056;
            }
        }
        public Repositorywgs057 Repositorywgs057
        {
            get
            {
                if (null == _Repositorywgs057)
                {
                    _Repositorywgs057 = new Repositorywgs057(context);
                }
                return _Repositorywgs057;
            }
        }
        #endregion
    }
}
