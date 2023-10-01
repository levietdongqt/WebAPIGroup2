using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Respository
{
    public class GenericRepository<T> where T : class
    {
        private readonly MyImageContext _context;
        private DbSet<T> _entities;
        public GenericRepository(MyImageContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }
        public async Task<IEnumerable<T>?> GetAllAsync()
        {
            try
            {
                var list = await _entities.ToListAsync();
                return (IEnumerable<T>?)list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }
        }
        public async Task<bool> InsertAsync(T entity)
        {
            try
            {
                _entities.Add(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }
        public async Task<bool> InsertAllAsync(List<T> list)
        {
            using (var trasaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _entities.AddRangeAsync(list);
                    _context.SaveChanges();
                    trasaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    trasaction.Rollback();
                    return false;
                }
            }
        }
        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _entities.Update(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }
        public async Task<bool> UpdateAllAsync(List<T> list)
        {
            using (var trasaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var entity in list)
                    {
                        _context.Entry(entity).State = EntityState.Modified;
                    }
                    await _context.SaveChangesAsync();
                    trasaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    trasaction.Rollback();
                    return false;
                }
            }
        }
        public async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                _entities.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }
        public async Task<bool> DeleteAllAsync(List<T> list)
        {
            using (var trasaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var entity in list)
                    {
                        _context.Entry(entity).State = EntityState.Deleted;
                    }
                    await _context.SaveChangesAsync();
                    trasaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    trasaction.Rollback();
                    return false;
                }
            }
        }

        internal Task<bool> UpdateAsync(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }
    }

}
