using MyShop.Core.Models;
using MyShop.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace MyShop.DataAccess.InMemory
{
    // this is going to be a generic class
    // the T could be anything 

    // when ever we are passing an object to T, it must be of type BaseEntity
    // a generic clas can inherit generic interfaces
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public InMemoryRepository()
        {
            // The typeof is an operator keyword which is used to get a type at the compile-time
            // it is getting the actual name of our class
            // if we pass Product class
            // the classname will become Product
            className = typeof(T).Name;
            items = cache[className] as List<T>;

            if (items == null)
            {
                items = new List<T>();
            }
        }

        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

            if (tToUpdate != null)
            {
                tToUpdate = t;
            }

            else
            {
                throw new Exception(className + " Not found");
            }
        }

        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);

            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(className + "Not Found");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {
            T tToDelete = items.Find(i => i.Id == Id);

            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }

            else
            {
                throw new Exception(className + " Not found");
            }
        }
    }
}
