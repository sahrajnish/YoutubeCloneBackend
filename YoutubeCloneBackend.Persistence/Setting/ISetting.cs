using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Persistence.Setting
{
    public interface ISetting
    {
        string ConnectionString { get; }
    }
}
