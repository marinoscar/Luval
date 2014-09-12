using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm
{
    public interface IDataContext
    {
        IDatabase Database { get; }
        int SaveChanges();
        void Add<T>(T item);
        void Update<T>(T item);
        void Remove<T>(T item);
        IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, bool orderByDescending, uint skip, uint take, bool lazyLoading);
        IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, bool orderByDescending, uint take, bool lazyLoading);
        IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, bool orderByDescending);
        IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, bool orderByDescending, bool lazyLoading);
        IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression);
        IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, bool lazyLoading);
    }
}
