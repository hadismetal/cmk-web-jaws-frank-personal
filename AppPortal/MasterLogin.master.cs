using System;

public partial class MasterLogin : System.Web.UI.MasterPage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Favicon
        string CB = ""; /// mondial, franknco, missmondial, thepalace
        CB = System.Configuration.ConfigurationManager.AppSettings["LicensedTo"];
    }
}
