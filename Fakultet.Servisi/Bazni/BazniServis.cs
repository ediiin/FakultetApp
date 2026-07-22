using Microsoft.EntityFrameworkCore;

namespace Fakultet.Servisi.Bazni
{
    public class BazniServis<T>: IServis<T> where T: class
    {
        protected readonly FakultetAppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public BazniServis(FakultetAppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual T? GetById(int id) 
        {
            return _dbSet.Find(id);
        }

        public virtual void Add(T obj)
        {
            _dbSet.Add(obj);
            _dbContext.SaveChanges();
        }

        public virtual void Update(T obj)
        {
            _dbSet.Update(obj);
            _dbContext.SaveChanges();
        }

        public virtual void Remove(int id)
        {
            var zapis = GetById(id);
            if(zapis != null) 
            {
                _dbSet.Remove(zapis);
                _dbContext.SaveChanges();
            }
        }
    }
}
