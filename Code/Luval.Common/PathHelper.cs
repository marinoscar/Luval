using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Luval.Common
{
    public static class PathHelper
    {
        public static string GetPath()
        {
            return HostingEnvironment.IsHosted ? HostingEnvironment.ApplicationPhysicalPath : Environment.CurrentDirectory;
        }

        public static string GetPathForFile(string fileName)
        {
            var currentPath = GetPath();
            if (!currentPath.EndsWith(@"\"))
                currentPath = currentPath + @"\";
            if (fileName.StartsWith(@"\"))
                fileName = fileName.Remove(0, 1);
            return @"{0}{1}".Fi(currentPath, fileName);
        }
    }
}
