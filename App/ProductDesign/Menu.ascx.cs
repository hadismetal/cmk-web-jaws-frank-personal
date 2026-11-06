using System;
using System.Linq;
using System.Linq.Dynamic;

public partial class Menu : System.Web.UI.UserControl
{
    private Map_DB _db;

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
        if (db != null)
            db.Dispose();
        base.Dispose();
    }

    protected string ProductType1
    {
        get
        {
            string xProductType = "";
            var d = db.CompanyBrand.Where(p => p.Disable == false).FirstOrDefault();

            if(d != null)
            {
                xProductType = (d.NamaKode == "Z") ? "Cubic Zirconia" : "Diamond Jewellery";
                
            }

            return xProductType;
        }
    }

    protected string ProductType2 { get { return "Gold Jewellery"; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Summary
            App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
			App.MenuPath(a1, "/Page/Register/");
            App.MenuPath(a4, "/Page/Update/");
			App.MenuPath(a2, "/Page/Approval/");
			App.MenuPath(a3, "/Page/History/");

			//Role Protection
			a1.Visible = Role.Operator;
			a2.Visible = Role.Manager;
            
        }
	}
}
