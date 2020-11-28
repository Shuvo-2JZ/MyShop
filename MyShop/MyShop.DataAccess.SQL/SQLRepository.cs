using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext context;
        internal DbSet<T> dbSet; // DbSet represents a table in the database

        public SQLRepository(DataContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>(); // DbContext.Set method
            // Set<T>() Returns a DbSet<TEntity> instance for access to entities of the given type in the context and the underlying store.
        }

        public IQueryable<T> Collection()
        {
            return dbSet;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var t = Find(Id);

            if (context.Entry(t).State == EntityState.Detached)
                dbSet.Attach(t);

            dbSet.Remove(t);

            /*
             Entry()

             It just attaches the entity to the dataContext. 
             Otherwise you will have to search for the entity using the primary key 
             and then edit the value and save it.
            */

            /*
             You have to attach your entity in the context because if you don't do that,
             you will receive an error while removing. EF can remove entities in this context only.

             the entity must exist in the context before the Remove operation can be performed.
            */
        }

        public T Find(string Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

        public void Update(T t)
        {
            dbSet.Attach(t);
            context.Entry(t).State = EntityState.Modified;

            /*
             When you do context.Entry(entity).State = EntityState.Modified;, 
             you are not only attaching the entity to the DbContext, you are also marking the whole entity as dirty. 
             This means that when you do context.SaveChanges(), 
             EF will generate an update statement that will update all the fields of the entity.
             */
        }
    }
}