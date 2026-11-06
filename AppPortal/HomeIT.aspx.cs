using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Configuration;

public partial class HomeIT : System.Web.UI.Page {
	private Map_AC _ac;
	private Map_DB _db;

	private Map_AC ac
	{
	    get
	    {
	        if (_ac == null)
	            _ac = new Map_AC();
	        return _ac;
	    }
	}

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
	    if (ac != null)
	        ac.Dispose();
	    if (db != null)
	        db.Dispose();
	    base.Dispose();
	}
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			
			fillForm();
		}
	}

	protected void fillForm() 
    {
        lit_issue.Text = "";
        string result = "";
        string mode = "success";
        string icon = "ok";
        string color = "green";

        var d = from p in db.IT_SP_Audit where p.StatusActive == true select p;

        if (d.Count() > 0)
        {
            foreach (var data in d)
            {
                if (data.TotalIssue > 0)
                {
                    mode = "danger"; icon = "warning-sign"; color = "red";
                }
                else
                {
                    mode = "success"; icon = "ok"; color = "green";
                }

                result += "<div class='col-xs-12 col-sm-4 col-md-3'>";
                result += "<div class='panel panel-" + mode + "'><div class='panel-body'><a style='display:block; text-decoration:none;' href='WarningSystem.aspx?id=" + data.ID.ToString() + "'>";
                result += "<div><div class='col-xs-2 col-sm-4 col-md-3'><span style='font-size:38px; color:" + color + ";' class='glyphicon glyphicon-" + icon + "' aria-hidden='true'></span></div>";
                result += "<div class='col-xs-10 col-sm-8 col-md-9'>";
                result += "<span class=''>" + data.Nama + "</span><br />";
                result += "<span class='text-lg text-" + mode + "'>" + Str.Number0(data.TotalIssue) + "</span>";
                result += "<span class='text-muted'> Issue</span>";
                result += "</div></div>";
                result += "</a></div></div>";
                result += "</div>";
                /*
                <div class="col-xs-12 col-sm-2">
                    <span class="text-muted">Sebelum Discount</span><br />
                    <span class="text-lg" id="summary_nilaiso" runat="server" />
                </div>
                */
            }
        }

        lit_issue.Text = result;
        
	}

}