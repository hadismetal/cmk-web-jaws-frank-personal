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
            //App.MenuPath(li2, "/Page/Register/");
            //App.MenuPath(li1, "/Page/Approval/");
            App.MenuPath(a2, "/Page/Report/");

            //Role Protection
            //li1.Visible = Role.Manager;
            //li2.Visible = Role.Operator;
            a2.Visible = Role.Manager;
		}
	}
}