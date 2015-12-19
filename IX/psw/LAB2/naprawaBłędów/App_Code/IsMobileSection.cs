using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;

/// <summary>
/// Summary description for IsMobileSection
/// </summary>
public class IsMobileSection : ConfigurationSection
{
    [ConfigurationProperty("Mobile")]
    public MobileElement Mobile
    {
        get { return this["mobile"] as MobileElement; }
        set { this["mobile"] = value; }
    }
    
    public IsMobileSection()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}