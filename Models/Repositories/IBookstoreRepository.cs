﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public interface IBookstoreRepository<TEntity>
    {
        IList<TEntity> List();
        TEntity Find(int Id);
        void Add(TEntity entity);
        void Update(int id,TEntity entity);
        void Delete(int id);
        IList<TEntity> Search(Func<TEntity, bool> predicate);

    }
}
