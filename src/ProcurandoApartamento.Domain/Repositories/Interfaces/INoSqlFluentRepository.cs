// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using JHipsterNet.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProcurandoApartamento.Domain.Repositories.Interfaces
{
    public interface INoSqlFluentRepository<TEntity> where TEntity : class
    {
        INoSqlFluentRepository<TEntity> Filter(Expression<Func<TEntity, bool>> filter);
        INoSqlFluentRepository<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> GetFirstAsync();
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IPage<TEntity>> GetPageAsync(IPageable pageable);
    }
}
