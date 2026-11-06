using System;
using System.Linq;

public partial class Menu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Summary
            App.MenuPath(a0, "/Page/Summary.aspx");

            //Module Menu
            App.MenuPath(a3, "/Page/ReportSales/");
            App.MenuPath(a1, "/Page/Create/");
            App.MenuPath(a2, "/Page/Report/");

            //Role Protection
            a1.Visible = Role.Operator;
            a3.Visible = Role.Manager;
            A11.Visible = true;
            RegisterWhitelistTradeIn.Visible = false;
            FilterApprovalVoidSO();
            FilterApprovalPackaging();
        }
    }
    protected void FilterApprovalVoidSO()
    {
        using (Map_AC ac = new Map_AC())
        {
            UserAccount user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            if (user != null)
            {
                string jabatan = user.Keterangan.ToString();
                if (jabatan.Contains("STORE MANAGER") || jabatan.Contains("ACCOUNT")
                    || jabatan.StartsWith("IT") || jabatan.Contains("AREA MANAGER")
                    || jabatan.Contains("JEWELLERY COORDINATOR")
                    || jabatan.Contains("MANAGEMENT TRAINEE")
                    || jabatan.StartsWith("QUALITY ASSURANCE"))
                {
                    andriPriyana.Visible = true;
                }
                else
                {
                    andriPriyana.Visible = false;
                }
            }
            else
            {
                andriPriyana.Visible = false;
            }
        }
    }

    protected void FilterApprovalPackaging()
    {
        using (Map_AC ac = new Map_AC())
        {
            UserAccount user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            if (user != null)
            {
                string jabatan = user.Keterangan.ToString();
                if (jabatan.Contains("STORE MANAGER") || jabatan.Contains("ACCOUNT")
                    || jabatan.StartsWith("IT") || jabatan.Contains("AREA MANAGER")
                    || jabatan.Contains("JEWELLERY COORDINATOR")
                    || jabatan.Contains("MANAGEMENT TRAINEE")
                    || jabatan.StartsWith("QUALITY ASSURANCE"))
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
