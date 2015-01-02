using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luval.Common;

namespace Luval.Orm
{
    public interface IDbExceptionHandler
    {
        DbException Handle(Exception dataException);
        DbException Handle(string message, Exception dataException);
    }

    public class DbExceptionHandlerFactory
    {
        public static IDbExceptionHandler Create(DatabaseProviderType provider)
        {
            var key = "DbExceptionHandler.{0}".Fi(provider);
            var assemblyInfo = ConfigurationManager.AppSettings[key];
            if(string.IsNullOrWhiteSpace(assemblyInfo)) return new AnsiDbExceptionHandler();
            return CreateFromAssembly(assemblyInfo);
        }

        public static IDbExceptionHandler CreateFromAssembly(string qualifiedName)
        {
            var provider = ObjectCacheProvider.GetProvider<string, IDbExceptionHandler>("DbExceptionHandlerFactory");
            return provider.GetCacheItem(qualifiedName, CreateFromReflection);
        }

        private static IDbExceptionHandler CreateFromReflection(string qualifiedName)
        {
            var nameParts = qualifiedName.Split(",".ToCharArray()).Select(i => i.Trim()).ToArray();
            var instanceHandle = Activator.CreateInstance(nameParts[1], nameParts[0]);
            return (IDbExceptionHandler)instanceHandle.Unwrap();
        }
    }

    public class AnsiDbExceptionHandler : IDbExceptionHandler
    {
        public DbException Handle(Exception dataException)
        {
            return new DbException(dataException.Message, dataException);
        }

        public DbException Handle(string message, Exception dataException)
        {
            return new DbException(message, dataException);
        }
    }

    public class SqlServerExceptionHandler : IDbExceptionHandler
    {
        public DbException Handle(Exception dataException)
        {
            return new DbException(dataException.Message, dataException);
        }

        public DbException Handle(string message, Exception dataException)
        {
            return new DbException(message, dataException);
        }
    }
}
