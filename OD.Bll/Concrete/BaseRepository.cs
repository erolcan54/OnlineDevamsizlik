using OD.Bll.Abstract;
using OD.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OD.Bll.Concrete
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        readonly OnlineDevDbEntities dbcontext = new OnlineDevDbEntities();
        private DbSet<T> _objset;

        public BaseRepository()
        {
            _objset = dbcontext.Set<T>();
        }

        public bool Add(T model)
        {
            _objset.Add(model);
            return Save();
        }

        public bool Delete(T model)
        {
            _objset.Remove(model);
            return Save();
        }

        public T Get(int id)
        {
            return _objset.Find(id);
        }

        public List<T> GetAll()
        {
            return _objset.ToList();
        }

        public T GetByFilter(Expression<Func<T, bool>> filter)
        {
            return _objset.Where(filter).FirstOrDefault();
        }

        public List<T> GetByFilterList(Expression<Func<T, bool>> filter)
        {
            return _objset.Where(filter).ToList();
        }

        public bool Save()
        {
            int result = dbcontext.SaveChanges();
            return result > 0 ? true : false;
        }

        public bool Update(T model)
        {
            dbcontext.Entry<T>(model).State = EntityState.Modified;
            return Save();
        }
    }
}
