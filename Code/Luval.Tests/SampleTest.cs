using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Luval.Common;
using Luval.Security.Model;
using NUnit.Framework;

namespace Luval.Tests
{
    [TestFixture]
    public class SampleTest
    {
        [Test]
        public void SampleCache1()
        {
            GetItemFromCache();
        }

        [Test]
        public void SampleCache2()
        {
            GetItemFromCache();
        }

        public void GetItemFromCache()
        {
            var cacheProvider = ObjectCacheProvider.GetProvider<int, List<string>>("cache.storage");

            var value = cacheProvider.GetCacheItem(5, GetItems);

            foreach (var item in value)
            {
                Debug.WriteLine(item);
            }
        }

        public List<string> GetItems(int key)
        {
            var result = new List<string>();
            key.Times(i =>
                {
                    result.Add(Guid.NewGuid().ToString());
                    Thread.Sleep(500);
                });
            return result;
        }
    }
}
