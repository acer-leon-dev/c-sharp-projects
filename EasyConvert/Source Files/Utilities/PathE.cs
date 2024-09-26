using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class PathE
    {
        public static string ChangeDirectory(string directory, string file)
        {
            return Path.Join(directory, Path.GetFileName(file));
        }
    }
}
