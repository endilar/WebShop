﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebShop_DataAccess.Repository.IRepository
{
     public interface IRepository<T> where T : class
    {
        T Find(int id);

        IEnumerable<T> GetAll(

            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            bool isTreacking = true
            );

        T FirstOrDefault(
           Expression<Func<T, bool>> filter = null,
            string includeProperties = null,
            bool isTreacking = true);


        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

        void Save();
    }
}
