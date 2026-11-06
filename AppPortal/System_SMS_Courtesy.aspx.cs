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

public partial class System_SMS_Courtesy : System.Web.UI.Page
{
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
	protected void Page_Load(object sender, EventArgs e) 
    {
		//App.Protect();

		if (!IsPostBack) {
			
			fillForm();
		}
	}

	protected void fillForm() 
    {
        string Request_URL = "";
        string Response_String = "";
        string Response_Result = "";

        bool xStatus = false;

        var d = from p in db.IT_SMS_Blast 
                where p.ExecuteStatus == false 
                    && p.ExecuteDateTime != null 
                    && p.ExecuteDateTime < System.DateTime.Now
                    && p.ExecuteDateTime > System.DateTime.Now.AddDays(-3)
                    && p.VoidStatus == false
                    && p.Module.Substring(0,3) == "SCO"
                select p;

        if (d.Count() > 0)
        {
            foreach (var data in d)
            {
                xStatus = false;
                Request_URL = data.RequestURL;
                
                WebClient webpage = new WebClient();
                Response_String = webpage.DownloadString(Request_URL);
                
                if (Response_String.Trim() == "") { Response_Result = "Fail"; xStatus = false; }
                else { Response_Result = "Sent"; xStatus = true; }

                if (xStatus)
                {
                    data.ResponseURL = Response_String;
                    data.ResponseDateTime = System.DateTime.Now;
                }

                data.ExecuteAttempt = data.ExecuteAttempt + 1;
                data.ExecuteStatus = xStatus;
                data.Result = Response_Result;

                db.SubmitChanges();

                if (data.ExecuteAttempt == 2)
                {
                    data.VoidStatus = true;
                    data.VoidDateTime = System.DateTime.Now;
                    data.VoidDescription = "Auto void by System. Maximum attempt reached.";
                    db.SubmitChanges();
                }

                System.Threading.Thread.Sleep(1000);

            }
        }


        
	}

}