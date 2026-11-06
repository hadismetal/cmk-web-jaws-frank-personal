using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPageDP : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Init(this);

        if (!IsPostBack)
            module.InnerHtml = App.NamaModule;

        //Sistem Callout
        if (Request.QueryString["d"] != null)
            callout.Visible = callout_script.Visible = !IsPostBack;
    }
}
