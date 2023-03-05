﻿using Microsoft.EntityFrameworkCore;
using ServiceAppointmentSystem.Data;
using ServiceAppointmentSystem.Repositories.Interfaces;
using System.Linq.Expressions;

namespace ServiceAppointmentSystem.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        private DbSet<T> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public List<T> GetAll(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            if (includeProperties != null)
            {
                foreach (var includeprop in includeProperties.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeprop in includeProperties.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }

            return query.FirstOrDefault();
        }

        public T Get(int ID)
        {
            return _dbSet.Find(ID);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
