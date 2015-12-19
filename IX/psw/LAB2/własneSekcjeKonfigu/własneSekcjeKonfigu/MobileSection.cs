using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;

namespace własneSekcjeKonfigu
{
    public class MobileSection : ConfigurationSection
    {
        [ConfigurationProperty("Database")]
        public DatabaseElement Database
        {
            get { return this["Database"] as DatabaseElement; }
            set { this["Database"] = value; }
        }
    }
}