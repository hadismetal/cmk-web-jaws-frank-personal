using System;
using System.Linq;

public partial class Menu : System.Web.UI.UserControl {
    private const string StockLedgerOnlyNik = "260875";

	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
			App.MenuPath(a1, "/Page/Register/");
			//App.MenuPath(a2, "/Page/PG/");
            App.MenuPath(a3, "/Page/Report/");

			//Role Protection
			a1.Visible = Role.Operator;
            //a2.Visible = Role.Operator;
            a3.Visible = Role.Manager;
			a3.Visible = (Role.DownloadExcel || Role.DownloadFile);
            applyStockLedgerOnlyMenu();
			consignment();
		}
	}
    protected void applyStockLedgerOnlyMenu()
    {
        using (Map_AC ac = new Map_AC())
        {
            UserAccount user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            if (user == null || user.UserNumber != StockLedgerOnlyNik)
                return;

            a1.Visible = false;
            Li1.Visible = false;
            Li2.Visible = false;
            Li3.Visible = false;
            Li4.Visible = false;
            Li5.Visible = false;
            Li6.Visible = false;
            Li7.Visible = false;
            Li8.Visible = false;
            Li9.Visible = false;
            Li10.Visible = false;
            Li11.Visible = false;
            Li12.Visible = true;
            Li13.Visible = false;
        }
    }
	protected void consignment()
    {
        using (Map_AC ac = new Map_AC())
        {
            UserAccount user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            if (user != null)
            {
                string jabatan = user.Keterangan.ToString();
                if(jabatan.StartsWith("IT"))
                {
                    A14.Visible = true;
                    A19.Visible = true;
                }
                else
                {
                    A14.Visible = false;
                    A19.Visible = false;
                }
            }
            else 
            {
                A14.Visible = false;
                A19.Visible = false;
            }
        }  
    }
}