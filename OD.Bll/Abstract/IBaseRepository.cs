using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OD.Bll.Abstract
{
    public interface IBaseRepository<T> where T : class, new()
    {
        List<T> GetAll();
        T Get(int id);
        bool Add(T model);
        bool Delete(T model);
        bool Update(T model);
        bool Save();
        List<T> GetByFilterList(Expression<Func<T, bool>> filter);
        T GetByFilter(Expression<Func<T, bool>> filter);
    }
}
