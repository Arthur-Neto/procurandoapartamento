// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using JHipsterNet.Core.Pagination;
using JHipsterNet.Core.Pagination.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ProcurandoApartamento.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProcurandoApartamento.Infrastructure.Data.Repositories
{
    public class FluentRepository<TEntity> : IFluentRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> _include;
        private List<Expression<Func<TEntity, object>>> _includeProperties;
        private Expression<Func<TEntity, bool>> _filter;
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> _orderBy;
        private bool _disableTracking;

        public FluentRepository(DbSet<TEntity> dbset)
        {
            _dbSet = dbset;
            _include = new List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();
            _includeProperties = new List<Expression<Func<TEntity, object>>>();
        }

        public INoSqlFluentRepository<TEntity> Filter(Expression<Func<TEntity, bool>> filter)
        {
            _filter = filter;
            return this;
        }

        public INoSqlFluentRepository<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            _orderBy = orderBy;
            return this;
        }

        public IFluentRepository<TEntity> Include(Expression<Func<TEntity, object>> expression)
        {
            _includeProperties.Add(expression);
            return this;
        }

        public IFluentRepository<TEntity> Include(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            _include.Add(include);
            return this;
        }

        public IFluentRepository<TEntity> AsNoTracking()
        {
            _disableTracking = true;
            return this;
        }

        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter)
        {
            _filter = filter;
            IQueryable<TEntity> query = BuildQuery();
            return await query.SingleOrDefaultAsync();
        }

        public async Task<TEntity> GetFirstAsync()
        {
            IQueryable<TEntity> query = BuildQuery();
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            IQueryable<TEntity> query = BuildQuery();
            return await query.ToListAsync();
        }

        public async Task<IPage<TEntity>> GetPageAsync(IPageable pageable)
        {
            IQueryable<TEntity> query = BuildQuery();
            return await query.UsePageableAsync(pageable);
        }

        private IQueryable<TEntity> BuildQuery()
        {
            IQueryable<TEntity> query = _dbSet;

            if (_disableTracking)
            {
                query = query.AsNoTracking();
            }

            _includeProperties?.ForEach(i => { query = query.Include(i); });

            _include?.ForEach(i => { query = i(query); });

            if (_filter != null)
            {
                query = query.Where(_filter);
            }

            if (_orderBy != null)
            {
                query = _orderBy(query);
            }

            return query;
        }
    }
}
