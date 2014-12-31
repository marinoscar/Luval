using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Common
{
    public class FileLocalization : ILocalizationProvider
    {

        #region Variable Declaration
        
        private readonly string _fileLocation;
        private IDictionary<string, string> _data; 

        #endregion 

        #region Constructors

        public FileLocalization(string cultureCode, string resourceFolderLocation)
        {
            CultureCode = cultureCode;
            _fileLocation = resourceFolderLocation;
            if (!Directory.Exists(resourceFolderLocation)) throw new ArgumentException("{0} is not a valid folder location".Fi(resourceFolderLocation));
            _data = new ConcurrentDictionary<string, string>();
            LoadFile();
        } 

        #endregion

        #region Property Implementation

        public string CultureCode { get; private set; } 

        #endregion

        #region Method Implementation


        public static ILocalizationProvider GetLocalizationProvider()
        {
            return GetLocalizationProvider("es");
        }

        public static ILocalizationProvider GetLocalizationProvider(string cultureCode)
        {
            return GetLocalizationProvider(cultureCode, @"\Localization");
        }

        public static ILocalizationProvider GetLocalizationProvider(string cultureCode, string relativeFileName)
        {

            return  new FileLocalization(cultureCode, PathHelper.GetPathForFile(relativeFileName));
        }

        public void LoadFile()
        {
            _data = new ConcurrentDictionary<string, string>();
            var fileName = Path.Combine(_fileLocation, "localization.{0}.txt".Fi(CultureCode.ToLowerInvariant()));
            using (var stream = new StreamReader(fileName))
            {
                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine();
                    if (string.IsNullOrWhiteSpace(line) || line.Trim().Equals(string.Empty) ||
                        line.Trim().StartsWith("#")) continue;
                    var items = line.Split(";".ToCharArray());
                    if (!items.Any() || items.Count() != 2) continue;
                    _data.Add(items[0].Trim(), items[1].Trim());
                }
            }
        }

        public string GetResource(string resourceName)
        {
            return !_data.ContainsKey(resourceName) ? "!WRONG!" : _data[resourceName];
        }

        public IEnumerable<KeyValuePair<string, string>> GetAll()
        {
            return _data;
        }

        #endregion

        public void Dispose()
        {
            _data.Clear();
            _data = null;
        }
    }
}
