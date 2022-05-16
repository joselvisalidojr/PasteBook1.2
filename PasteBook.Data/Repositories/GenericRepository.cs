using PasteBook.Data;
using PasteBook.Data.Exceptions;
using PasteBook.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PasteBook.Data.DataTransferObjects;

namespace PasteBook.Data.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> InfiniteScrollList(int pageNumber, int itemsPerScroll);
        Task<ScrollListDTO<T>> InfiniteScrollListWithInfo(int pageNumber, int itemsPerScroll);
        Task<IEnumerable<T>> FindAll();
        Task<T> FindByPrimaryKey(int id);
        Task<T> Insert(T entity);
        T Update(T entity);
        Task<T> Delete(int id);
        T Delete(T entity);

        PasteBookDbContext Context { get; set; }
    }

    public abstract class GenericRepository<T> where T : BaseEntity
    {
        public GenericRepository(PasteBookDbContext context)
        {
            this.Context = context;
        }

        public PasteBookDbContext Context { get; set; }

        public async Task<IEnumerable<T>> InfiniteScrollList(int pageNumber, int itemsPerScroll)
        {
            IQueryable<T> query = this.Context.Set<T>();
            return await query.Take(pageNumber * itemsPerScroll)
                              .ToListAsync();
        }

        public async Task<ScrollListDTO<T>> InfiniteScrollListWithInfo(int pageNumber, int itemsPerScroll)
        {
            IQueryable<T> query = this.Context.Set<T>();
            return await ScrollListDTO<T>.GetScrollListAsync(query, pageNumber, itemsPerScroll);
        }

        public async Task<IEnumerable<T>> FindAll()
        {
            IQueryable<T> query = this.Context.Set<T>();
            return await query.Select(e => e).ToListAsync();
        }

        public async Task<T> FindByPrimaryKey(int id)
        {
            var entity = await this.Context.FindAsync<T>(id);
            if (entity is object)
            {
                return entity;
            }
            
            throw new EntityNotFoundException($"Entity with ID: {id} does not exist.");
        }

        public async Task<T> Insert(T entity)
        {
            if (entity is null)
            {
                throw new EntityDataException("Entity argument cannot be null.");
            }

            await Context.AddAsync<T>(entity);

            return entity;
        }

        public T Update(T entity)
        {
            if (entity is null)
            {
                throw new EntityDataException("Entity argument cannot be null.");
            }

            this.Context.Attach<T>(entity);
            this.Context.Entry<T>(entity).State = EntityState.Modified;

            return entity;
        }

        public async Task<T> Delete(int id)
        {
            var entity = await this.Context.FindAsync<T>(id);
            if (entity is object)
            {
                this.Context.Remove<T>(entity);
                return entity;
            }

            throw new EntityNotFoundException($"Entity with ID: {id} does not exist.");
        }

        public T Delete(T entity)
        {
            if (entity is null)
            {
                throw new EntityDataException("Entity argument cannot be null.");
            }

            if (this.Context.Entry(entity).State == EntityState.Detached)
            {
                this.Context.Set<T>().Attach(entity);
            }

            this.Context.Set<T>().Remove(entity);

            return entity;
        }
    }
}
