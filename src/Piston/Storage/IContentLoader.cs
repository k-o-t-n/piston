using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Storage
{
    public interface IContentLoader
    {
        RawContent ReadFile(string filePath);
    }
}
