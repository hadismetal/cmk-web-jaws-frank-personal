using System;
using System.Data;
using System.Linq;

public partial class Menu : System.Web.UI.UserControl {
	private Map_AC _ac;

	private Map_AC ac
	{
	    get
	    {
	        if (_ac == null)
	            _ac = new Map_AC();
	        return _ac;
	    }
	}

	public override void Dispose()
	{
	    if (ac != null)
	        ac.Dispose();
	    base.Dispose();
	}
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
            App.MenuPath(Li1, "/Page/Register/");
            App.MenuPath(Li2, "/Page/ReportSales/");
            App.MenuPath(Li3, "/Page/Report/");
            App.MenuPath(Li4, "/Page/ReportOutgoing/");
            App.MenuPath(Li5, "/Page/ReportIncoming/");
            App.MenuPath(Li6, "/Page/Approval/");

			//Role Protection
            Li1.Visible = Role.Operator;
            Li6.Visible = Role.Manager;
            Li2.Visible = Role.Operator;
            Li3.Visible = Role.Operator;
            Li4.Visible = Role.Operator;
            Li5.Visible = Role.Operator;
			
			var user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            if (user != null)
            {
                string query = "SELECT TOP 1 NIK, Nama, IsFinance, IsAccounting, Disable FROM DataPackagingApproval WHERE NIK = " + user.UserNumber;
                DataTable dt = Db.Rs(query, Db.Cnn_DB);
                if(dt.Rows.Count == 1) 
                {
                    bool IsFinance = Convert.ToBoolean(dt.Rows[0]["IsFinance"]);
                    bool IsAccounting = Convert.ToBoolean(dt.Rows[0]["IsAccounting"]);
                    if (IsFinance && !IsAccounting)
                    {
                        A3X.Visible = true;
                        A2X.Visible = false;
                    }
                    else if (!IsFinance && IsAccounting) 
                    {
                        A3X.Visible = false;
                        A2X.Visible = true;
                    }
                    else if(IsFinance && IsAccounting) 
                    {
                        A3X.Visible = true;
                        A2X.Visible = true;
                    }
                    else 
                    {
                        A3X.Visible = false;
                        A2X.Visible = false;
                    }
                }
                else
                {
                    A3X.Visible = false;
                    A2X.Visible = false;
                }

                if(user.Email == "april@lakuemas.com" || user.Email == "noviani@lakuemas.com")
                {
                    Li1.Visible = false;
                    Li6.Visible = false;
                    Li2.Visible = false;
                    Li3.Visible = Role.Operator;
                    Li4.Visible = false;
                    Li5.Visible = false;
                }
            }
            else 
            {
                A3X.Visible = false;
                A2X.Visible = false;
            }
		}
	}
}