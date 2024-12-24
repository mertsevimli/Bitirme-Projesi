using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sarideniz.Core.Entities;
using Sarideniz.Data;
using Sarideniz.Service.Abstract;

namespace Sarideniz.Service.Concrete;

public class Service<T> : IService<T> where T : class, IEntity, new()
{
    public Service(DatabaseContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    internal DatabaseContext _context;
    internal DbSet<T> _dbSet;
    public List<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public List<T> GetAll(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression).ToList();
    }

    public IQueryable<T> GetQueryable()
    {
        return _dbSet;
    }

    public T Get(Expression<Func<T, bool>> expression)
    {
        return _dbSet.FirstOrDefault(expression);
    }

    public T Find(int id)
    {
        return _dbSet.Find(id);
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
       _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<T> FindAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.FirstOrDefaultAsync(expression);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.Where(expression).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
       await _dbSet.AddAsync(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}