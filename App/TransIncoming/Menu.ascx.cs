using System;
using System.Linq;

public partial class Menu : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            //Summary
            App.MenuPath(a0, "/Page/Summary");

            //Module Menu
            App.MenuPath(a1, "/Page/Register/");
            App.MenuPath(a2, "/Page/Approval/");
            //App.MenuPath(a3, "/Page/History/");
            App.MenuPath(a4, "/Page/Report/");

            //Role Protection
            a1.Visible = Role.Operator;
            a2.Visible = Role.Manager;
			a4.Visible = (Role.DownloadExcel || Role.DownloadFile);
			
			FilterApprovalDoKRM();
		}
	}
	
	protected void FilterApprovalDoKRM()
    {
        using (Map_AC ac = new Map_AC())
        {
            UserAccount user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            if (user != null)
            {
                string jabatan = user.Keterangan.ToString();
                if(jabatan.Contains("STORE MANAGER") || jabatan.Contains("ACCOUNT") 
                    || jabatan.StartsWith("IT") || jabatan.Contains("AREA MANAGER") ||jabatan.Contains("JEWELLERY COORDINATOR")
					|| jabatan.Contains("QUALITY ASSURANCE") || jabatan.Contains("STOCK KEEPER") || jabatan.Contains("INVENTORY CONTROL"))
                {
                    ANJ4Y.Visible = true;
                }
                else
                {
                    ANJ4Y.Visible = false;
                }
            }
            else 
            {
                ANJ4Y.Visible = false;
            }
        }
    }
}