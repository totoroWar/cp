using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
namespace DBase
{
    public abstract class RepositoryBase<T> where T : class
    {
        protected DBBase context = null;
        protected DbSet<T> dbset = null;
        public RepositoryBase(DBBase context)
        {
            this.context = context;
            this.dbset = this.context.Set<T>();
        }
        public virtual void Add(T entity)
        {
            this.dbset.Add(entity);
        }
        public virtual void AddList(List<T> entityList)
        {
            foreach (T item in entityList)
            {
                this.Add(item);
            }
        }
        public virtual List<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, string includeProperties = "")
        {
            IQueryable<T> query = this.dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (order != null)
            {
                return order(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public virtual IEnumerable<T> Get(string include)
        {
            IQueryable<T> query = this.dbset;
            foreach (var includeProperty in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query.ToList();
        }
        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> where)
        {
            return this.dbset.Where(where);
        }
        public virtual T GetOne(Expression<Func<T, bool>> where)
        {
            return this.dbset.Where(where).FirstOrDefault();
        }
        public virtual IEnumerable<T> GetAll()
        {
            return this.dbset.ToList();
        }
        public virtual T GetByPrimaryKey(int key)
        {
            return this.dbset.Find(key);
        }
        public virtual T GetByPrimaryKey(long key)
        {
            return this.dbset.Find(key);
        }
        public virtual T GetByPrimaryKey(string key)
        {
           return this.dbset.Find(key);
        }
        public virtual void Delete(T entity)
        {
            if (context.Entry(entity).State == System.Data.Entity.EntityState.Detached)
            {
                dbset.Attach(entity);
            }
            dbset.Remove(entity);
        }
        public virtual void DeleteList(List<T> entityList)
        {
            foreach (T item in entityList)
            {
                this.Delete(item);
            }
        }
        public virtual void Update(T entity)
        {
            dbset.Attach(entity);
            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }
        public virtual void UpdateList(List<T> entityList)
        {
            foreach (T item in entityList)
            {
                this.Update(item);
            }
        }
        public virtual int ExecuteSqlCommand(string sql)
        {
            return context.Database.ExecuteSqlCommand(sql);
        }
        public virtual int Count(Expression<Func<T, bool>> where)
        {
            return this.dbset.Count(where);
        }
        public virtual IQueryable<T> IQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> order = null, string includeProperties = "")
        {
            IQueryable<T> query = this.dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (order != null)
            {
                return order(query);
            }
            else
            {
                return query;
            }
        }
        public virtual int ExecuteSqlCommand(string sql, params object[] values)
        {
            return context.Database.ExecuteSqlCommand(sql, values);
        }
        public virtual IEnumerable<T> SqlQuery(string sql, params object[] values)
        {
            return context.Database.SqlQuery<T>(sql, values);
        }
        public IEnumerable<T> GetPage<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> sortKeySelector, int pageIndex, int pageSize,out int recordCount, bool isAsc = true )
        {
            try
            {
               recordCount= this.dbset.Count(filter);
                if (isAsc)
                {
                    return this.dbset
                        .Where(filter)
                        .OrderBy(sortKeySelector)
                        .Skip(pageSize * (pageIndex))
                        .Take(pageSize).AsQueryable();
                }
                else
                {
                    return this.dbset
                        .Where(filter)
                        .OrderByDescending(sortKeySelector)
                        .Skip(pageSize * (pageIndex))
                        .Take(pageSize).AsQueryable();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
