using System;

public partial class Menu : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
            //App.MenuPath(a1, "/Page/DJ/");
            //App.MenuPath(a3, "/Page/GJ/");
            //App.MenuPath(a2, "/Page/Report/");

			//Role Protection
			a1.Visible = Role.Operator;
            a2.Visible = Role.Operator;
            a3.Visible = (Role.DownloadExcel || Role.DownloadFile);
            
            if(!Lib.isOpenReval())
            {
                //A7.HRef = "Page/Summary.aspx";
                //A7.Style.Add("color","red");
                
                //A8.HRef = "Page/Summary.aspx";
                //A8.Style.Add("color", "red");
            }


            //a2.Visible = Lib.isOpenReval();
            
		}
	}
}