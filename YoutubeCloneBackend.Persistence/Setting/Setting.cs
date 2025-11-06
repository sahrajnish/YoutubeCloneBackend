using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Persistence.Setting
{
    public class Setting : ISetting
    {
        public Setting(IConfiguration configuration)
        {
            var con = configuration.GetConnectionString("DefaultConnection");
            ConnectionString = !string.IsNullOrEmpty(con) ? con : "";
        }
        public string? ConnectionString { get; set; }
    }
}
