using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["so"] == null)
            {
                //Auth. Parameter
                int UserID = App.QueryInt(this, "uid");
                string SID = Request.QueryString["sid"];

                //Redirect
                if (AC.Auth(UserID, SID))
                    Response.Redirect("~/Page/Summary.aspx");
                else
                    Response.Redirect("/"); //Error
            }
            else
            {
                //Sign-Out
                Session.Abandon();

                //Redirect
                if (Request.QueryString["so"] == "1")
                    Response.Redirect("/Home.aspx");
                else if (Request.QueryString["so"] == "3")
                    Response.Redirect("~/Page/FAQ.aspx");

                else if (Request.QueryString["so"] == "2")
                    Response.Redirect("/?so=1"); //Portal Sign-Out
            }
        }
    }
}