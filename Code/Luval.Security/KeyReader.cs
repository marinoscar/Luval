using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Luval.Common;

namespace Luval.Security
{
    public class KeyReader
    {

        #region Variable Declaration
        
        private readonly string _keysFileName;
        private bool _fileLoaded;

        #endregion

        #region Constructors
        
        public KeyReader()
            : this(HttpContext.Current.Server.MapPath(@"\Keys\authentication.keys"))
        {

        }

        internal KeyReader(string keyFileFullPath)
        {
            Keys = new List<KeyInformation>(4);
            _keysFileName = keyFileFullPath;
        } 

        #endregion

        #region Property Implementation
        
        public List<KeyInformation> Keys { get; private set; } 

        #endregion

        #region Method Implementation
        
        public KeyInformation GetKey(string provider)
        {
            if (!_fileLoaded)
                LoadFileContent();
            return Keys.SingleOrDefault(i => i.Provider.ToLowerInvariant() == provider.ToLowerInvariant());
        } 

        public void SaveFile()
        {
            var content = JsonSerializer.ToJson(Keys);
            using (var stream = new StreamWriter(_keysFileName, false))
            {
                stream.Write(content);
            }
        }

        #endregion

        #region Helper Methods

        private string ReadFileContent()
        {
            var content = string.Empty;
            using (var stream = new StreamReader(_keysFileName))
            {
                content = stream.ReadToEnd();
            }
            return content;
        }

        private void LoadFileContent()
        {
            var content = ReadFileContent();
            List<KeyInformation> fileKeys;
            try
            {
                fileKeys = JsonSerializer.FromJson<List<KeyInformation>>(content);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(GetSerializationErrorMessage(), ex);
            }
            Keys.AddRange(fileKeys);
            _fileLoaded = true;
        }

        private string GetSerializationErrorMessage()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Unable to read file {0}\n", _keysFileName);
            sb.AppendFormat("Please make sure the file is created and has the proper format\n");
            sb.Append(@"
[{Provider:'Google', Public:'PublicKeyValue', Private:'PrivateKeyValue'},
 {Provider:'Facebook', Public:'PublicKeyValue', Private:'PrivateKeyValue'},
 {Provider:'Twitter', Public:'PublicKeyValue', Private:'PrivateKeyValue'}]\n");
            return sb.ToString();
        } 

        #endregion
    }

    public class KeyInformation
    {
        public string Provider { get; set; }
        public string Public { get; set; }
        public string Private { get; set; }
    }
}
