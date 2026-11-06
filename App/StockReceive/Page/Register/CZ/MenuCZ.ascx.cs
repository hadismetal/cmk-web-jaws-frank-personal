using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_CZ_MenuCZ : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            App.MenuURL(l1, "CZ.aspx");
            App.MenuURL(l11, "CZ_Form1.aspx");
            App.MenuURL(l2, "CZ_Form.aspx");
        }
    }
}