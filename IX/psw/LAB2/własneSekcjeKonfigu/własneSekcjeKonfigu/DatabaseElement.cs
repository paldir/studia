using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;

namespace własneSekcjeKonfigu
{
    public class DatabaseElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }
    }
}