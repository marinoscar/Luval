using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Common
{
    public interface ILocalizationProvider : IDisposable
    {
        string CultureCode { get; }
        string GetResource(string resourceName);
        IEnumerable<KeyValuePair<string, string>> GetAll();
    }
}
