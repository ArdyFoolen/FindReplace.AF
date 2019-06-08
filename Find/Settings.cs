using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find
{
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static readonly Settings DefaultInstance = (Settings)Synchronized(new Settings());

        public static Settings Default
        {
            get { return DefaultInstance; }
        }

        [UserScopedSetting]
        public string RecentFolder
        {
            get { return (string)this["RecentFolder"]; }
            set
            {
                this["RecentFolder"] = value;
                Save();
            }
        }
    }
}
