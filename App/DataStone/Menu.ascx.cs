using System;
using System.Linq;

public partial class Menu : System.Web.UI.UserControl {
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
    
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
            
            App.MenuPath(a1, "/Page/Create/");
			App.MenuPath(a2, "/Page/Account/");

            //// pembatasan hak akses create stone hanya untuk JAWS Mondial
            var rsCB = (from p in db.CompanyBrand
                        where p.Draft == false && p.Disable == false
                        select p.Nama).First();

            string CB = ""; /// mondial, franknco, missmondial, thepalace
            if (rsCB.Count() > 0) { CB = rsCB.ToLower(); }

            if (CB == "mondial" || CB == "miss mondial")
            {
                //Role Protection
                a1.Visible = Role.Manager;
            }
            else
            {
                a1.Visible = false;
            }

            a2.Visible = Role.Operator;
		}
	}
}