using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class MasterLogin : System.Web.UI.MasterPage 
{
    private Map_DB _db;

    private Map_DB db
    {
        get
        {
            if (_db == null)
                _db = new Map_DB();
            return _db;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        base.Dispose();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Event.generate_css_event(lit_css_event);

        // Favicon
        string CB = ""; /// mondial, franknco, missmondial, thepalace
        CB = System.Configuration.ConfigurationManager.AppSettings["LicensedTo"];

        string path = "cmk";
        string brand = "PT Central Mega Kencana";
        string random = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(System.DateTime.Now.Date.ToString()));
        string color = "#ffffff";

        if (CB.ToLower().Contains("WHOLESALE".ToLower()))
        {
            path = "wholesale";
            brand = "Wholesale";
            color = "#FBEB36";
        }
        else if (CB.ToLower().Contains("MONDIAL RARE JEWELS".ToLower()))
        {
            path = "mondial";
            brand = "Mondial";
            color = "#BB935E";
        }
        else if (CB.ToLower().Contains("MISS MONDIAL".ToLower()))
        {
            path = "missmondial";
            brand = "Miss Mondial";
            color = "#1A2674";
        }
        else if (CB.ToLower().Contains("MONDIAL".ToLower()))
        {
            path = "mondial";
            brand = "Mondial";
            color = "#1A2674";
        }
        
        else if (CB.ToLower().Contains("THE PALACE".ToLower()))
        {
            path = "thepalace";
            brand = "The Palace";
            color = "#000000";
        }
        else if (CB.ToLower().Contains("FRANK".ToLower()))
        {
            path = "franknco";
            brand = "Frank & Co";
            color = "#7C0041";
        }
        else
        {
            path = "cmk";
            brand = "PT Central Mega Kencana";
            color = "#FBEB36";
        }

        // generated using http://realfavicongenerator.net/

        link1.Href = "/Images/favicon/" + path + "/apple-touch-icon.png?v=" + random;
        link2.Href = "/Images/favicon/" + path + "/favicon-32x32.png?v=" + random;
        link3.Href = "/Images/favicon/" + path + "/favicon-16x16.png?v=" + random;
        link4.Href = "/Images/favicon/" + path + "/manifest.json?v=" + random;
        link5.Href = "/Images/favicon/" + path + "/safari-pinned-tab.svg?v=" + random;
        link6.Href = "/Images/favicon/" + path + "/favicon.ico?v=" + random;

        meta1.Content = brand;
        meta2.Content = brand;
        meta3.Content = color;

        App.Init(this);
    }

}
