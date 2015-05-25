using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
namespace DBase
{
    public class _GCS
    {
        public static string GetMain()
        {
            return "name=WebGameYJF";
        }
        public static string GetRead()
        {
            return "name=WebGameYJFR1";
        }
    }
    public class DBBase : DbContext
    {
        private bool isRead = false;
        void FirstSet()
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ValidateOnSaveEnabled = true;
        }
        public DBBase()
            : base(_GCS.GetMain())
        {
            FirstSet();
        }
        public DBBase(bool read)
            : base( _GCS.GetRead() )
        {
            //FirstSet();
            isRead = read;
        }
        private string GetReadDB(string connectionString)
        {
            return connectionString;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new ApplicationException();
        }
        public DbSet<DBModel.wgs001> wgs001 { get; set; }
        public DbSet<DBModel.wgs002> wgs002 { get; set; }
        public DbSet<DBModel.wgs003> wgs003 { get; set; }
        public DbSet<DBModel.wgs004> wgs004 { get; set; }
        public DbSet<DBModel.wgs005> wgs005 { get; set; }
        public DbSet<DBModel.wgs006> wgs006 { get; set; }
        public DbSet<DBModel.wgs007> wgs007 { get; set; }
        public DbSet<DBModel.wgs008> wgs008 { get; set; }
        public DbSet<DBModel.wgs009> wgs009 { get; set; }
        public DbSet<DBModel.wgs010> wgs010 { get; set; }
        public DbSet<DBModel.wgs011> wgs011 { get; set; }
        public DbSet<DBModel.wgs012> wgs012 { get; set; }
        public DbSet<DBModel.wgs013> wgs013 { get; set; }
        public DbSet<DBModel.wgs014> wgs014 { get; set; }
        public DbSet<DBModel.wgs015> wgs015 { get; set; }
        public DbSet<DBModel.wgs016> wgs016 { get; set; }
        public DbSet<DBModel.wgs017> wgs017 { get; set; }
        public DbSet<DBModel.wgs018> wgs018 { get; set; }
        public DbSet<DBModel.wgs019> wgs019 { get; set; }
        public DbSet<DBModel.wgs020> wgs020 { get; set; }
        public DbSet<DBModel.wgs021> wgs021 { get; set; }
        public DbSet<DBModel.wgs022> wgs022 { get; set; }
        public DbSet<DBModel.wgs023> wgs023 { get; set; }
        public DbSet<DBModel.wgs024> wgs024 { get; set; }
        public DbSet<DBModel.wgs025> wgs025 { get; set; }
        public DbSet<DBModel.wgs026> wgs026 { get; set; }
        public DbSet<DBModel.wgs027> wgs027 { get; set; }
        public DbSet<DBModel.wgs028> wgs028 { get; set; }
        public DbSet<DBModel.wgs029> wgs029 { get; set; }
    }
}
