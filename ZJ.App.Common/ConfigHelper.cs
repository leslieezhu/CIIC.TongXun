using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJ.App.Common
{
    public static class ConfigHelper
    {
        public static string ReadConfig(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}
