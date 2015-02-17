using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Common
{
    public class ConsoleArguments
    {
        private List<string> _args;

        public ConsoleArguments(IEnumerable<string> args)
        {
            _args = new List<string>(args);
        }

        public bool ContainsSwitch(string name)
        {
            return _args.Contains(name);
        }

        public string GetSwitchValue(string name)
        {
            if (!ContainsSwitch(name)) return null;
            var switchIndex = _args.IndexOf(name);
            if (_args.Count < switchIndex + 1) return null;
            return _args[switchIndex + 1];
        }
    }
}
