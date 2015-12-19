using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;

/// <summary>
/// Summary description for MobileElement
/// </summary>
public class MobileElement : ConfigurationElement
{
    [ConfigurationProperty("System")]
    public string System
    {
        get { return this["system"].ToString(); }
        set { this["system"] = value; }
    }

    public MobileElement()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}