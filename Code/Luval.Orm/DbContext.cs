using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm
{
    public class DbContext : IDataContext
    {

        #region Variable Declaration

        private readonly Dictionary<Type, IDataListItems> _items;
        protected internal ISqlLanguageProvider LanguageProvider { get; private set; }

        #endregion

        #region Constructors

        public DbContext()
            : this(DbConfiguration.CreateDatabase(), DbConfiguration.Get<ISqlLanguageProvider>())
        {

        }

        public DbContext(string connectionString)
            : this(DbConfiguration.CreateDatabase(connectionString), DbConfiguration.Get<ISqlLanguageProvider>())
        {
        }

        public DbContext(Database db)
            : this(db, DbConfiguration.Get<ISqlLanguageProvider>())
        {
        }

        public DbContext(Database db, ISqlLanguageProvider languageProvider)
        {
            _items = new Dictionary<Type, IDataListItems>();
            LanguageProvider = languageProvider;
            Database = db;
        }

        #endregion

        #region Property Implementation

        public IDatabase Database { get; private set; }

        #endregion

        #region Private Methods

        private DataList<T> GetDataList<T>()
        {
            var type = typeof(T);
            if (!_items.ContainsKey(type))
                _items[type] = new DataList<T>();
            return (DataList<T>)_items[type];
        }

        #endregion

        #region Persist Methods

        public void Add<T>(T item)
        {
            GetDataList<T>().Add(item);
        }

        public void Update<T>(T item)
        {
            GetDataList<T>().Update(item);
        }

        public void Remove<T>(T item)
        {
            GetDataList<T>().Remove(item);
        }

        public virtual int SaveChanges()
        {
            var result = 0;
            using (var transProvider = new DbTransactionProvider(DbConfiguration.Get<IDbConnectionProvider>()))
            {
                result = SaveChanges(transProvider);
                if (transProvider.ProvideTransaction)
                    transProvider.Commit();
            }
            return result;
        }

        public virtual int SaveChanges(DbTransactionProvider transaction)
        {
            var count = 0;
            count += DeleteRecords(transaction);
            count += InsertRecords(transaction);
            count += UpdateRecords(transaction);
            foreach (var item in _items)
            {
                foreach (var valueItem in item.Value.GetItems())
                {
                    valueItem.Status = DataListItemStatus.Unchanged;
                }
            }
            return count;
        }

        #endregion

        #region Select Methods

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression)
        {
            return Select(expression, null, false, 0, 0, true);
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, bool lazyLoading)
        {
            return Select(expression, null, false, 0, 0, lazyLoading);
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, bool orderByDescending)
        {
            return Select(expression, orderBy, orderByDescending, 0, 0, true);
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, bool orderByDescending, bool lazyLoading)
        {
            return Select(expression, orderBy, orderByDescending, 0, 0, lazyLoading);
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, bool orderByDescending, uint take, bool lazyLoading)
        {
            return Select(expression, orderBy, orderByDescending, 0, take, lazyLoading);
        }

        public IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, bool orderByDescending, uint skip, uint take, bool lazyLoading)
        {
            return
                Database.ExecuteToList<T>(LanguageProvider.Select<T>(expression, orderBy, orderByDescending, skip, take,
                                                                      lazyLoading));
        }

        #endregion

        #region Helper Methods

        private int UpsertRecords(IDbTransactionProvider transactionProvider)
        {
            Database.TransactionProvider = transactionProvider;
            var sb = new StringBuilder();
            foreach (var modelType in _items)
            {
                var list = modelType.Value.GetItems();
                var items = list.Where(i => i.Status != DataListItemStatus.Deleted);
                var sql = LanguageProvider.Upsert(items.Select(i => i.Value));
                if (string.IsNullOrWhiteSpace(sql)) continue;
                sb.AppendFormat("{0};\n", sql);
            }
            return sb.Length <= 0 ? 0 : Database.ExecuteNonQuery(sb.ToString());
        }

        private int InsertRecords(IDbTransactionProvider transactionProvider)
        {
            return ProcessRecord(transactionProvider, (i => i.Status == DataListItemStatus.Added),
                                 LanguageProvider.Insert, "Insert");
        }

        private int UpdateRecords(IDbTransactionProvider transactionProvider)
        {
            return ProcessRecord(transactionProvider, (i => i.Status == DataListItemStatus.Updated),
                                 LanguageProvider.Update, "Update");
        }

        private int DeleteRecords(IDbTransactionProvider transactionProvider)
        {
            return ProcessRecord(transactionProvider, (i => i.Status == DataListItemStatus.Deleted),
                                 LanguageProvider.Delete, "Delete");
        }

        private int ProcessRecord(IDbTransactionProvider transactionProvider, Func<IDataListItem, bool> filter, Func<object, string> action, string transType)
        {
            IAutoIncrement identity = null;
            var resultCount = 0;
            Database.TransactionProvider = transactionProvider;
            foreach (var modelType in _items)
            {
                var list = modelType.Value.GetItems();
                var items = list.Where(filter);
                foreach (var item in items)
                {
                    var sql = new StringBuilder();
                    sql.AppendFormat("{0};\n", action(item.Value));
                    if (transType == "Insert" && item.Value is IAutoIncrement)
                    {
                        sql.AppendFormat("{0};\n", LanguageProvider.GetLastIdentityInsert());
                        identity = ((IAutoIncrement)item.Value);
                    }
                    var result = Database.ExecuteScalarOr<object>(sql.ToString(), 0);
                    if (identity != null) identity.Id = Convert.ToInt32(result);
                    resultCount++;
                }
            }
            return resultCount;
        }

        #endregion
    }
}
