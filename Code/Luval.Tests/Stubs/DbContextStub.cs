using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Luval.Orm;

namespace Luval.Tests.Stubs
{
    public class DbContextStub : IDataContext
    {
        public DbContextStub()
        {
            Database = new DatabaseStub();
            _internalData = new Dictionary<Type, List<Tuple<object, DataListItemStatus>>>();
        }
        
        private readonly Dictionary<Type, List<Tuple<object, DataListItemStatus>>> _internalData; 

        public IDatabase Database { get; private set; }

        public int SaveChanges()
        {
            _internalData.Clear();
            return 1;
        }

        public void Add<T>(T item)
        {
            if (ContainsItem<T>(item)) return;
            Get<T>().Add(new Tuple<object, DataListItemStatus>(item, DataListItemStatus.Added));
        }

        public void Update<T>(T item)
        {
            if (ContainsItem<T>(item)) return;
            Get<T>().Add(new Tuple<object, DataListItemStatus>(item, DataListItemStatus.Updated));
        }

        public void Remove<T>(T item)
        {
            var storedItem = Find<T>(item);
            if (storedItem == null) return;
            var list = Get<T>();
            list.Add(new Tuple<object, DataListItemStatus>(item, DataListItemStatus.Deleted));
            list.Remove(storedItem);
        }

        private Tuple<object, DataListItemStatus> Find<T>(object item)
        {
            var list = Get<T>();
            return list.SingleOrDefault(i => i.Item1 == item);
        }
 
        private bool ContainsItem<T>(object item)
        {
            return Find<T>(item) != null;
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, bool orderByDescending, uint skip, uint take,
                                     bool lazyLoading)
        {
            return Get<T>().Select(i => (T) i.Item1).ToList().Where(expression.Compile());
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, bool orderByDescending, uint take, bool lazyLoading)
        {
            return Select<T>(expression, orderBy, orderByDescending, 0, take, lazyLoading);
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, bool orderByDescending)
        {
            return Select<T>(expression, orderBy, orderByDescending, 0, 0, false);
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, bool orderByDescending, bool lazyLoading)
        {
            return Select<T>(expression, orderBy, orderByDescending, 0, 0, lazyLoading);
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression)
        {
            return Select<T>(expression, null, false, 0, 0, true);
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, bool lazyLoading)
        {
            return Select<T>(expression, null, false, 0, 0, lazyLoading);
        }

        private List<Tuple<object, DataListItemStatus>> Get<T>()
        {
            var type = typeof (T);
            if(!_internalData.ContainsKey(type))
                _internalData[type] = new List<Tuple<object, DataListItemStatus>>();
            return _internalData[type];
        }
    }
}
