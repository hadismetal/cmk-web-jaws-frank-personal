using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_UserUpdateRoleAdd : System.Web.UI.Page {
	private Map_AC _db;

	private Map_AC db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_AC();
	        return _db;
	    }
	}

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    base.Dispose();
	}
	protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			back.HRef += "?id=" + myID;

			fillForm();
			fillForm(select_all);
		}

		fillModule(); //Module Place-Holder
	}
	protected void fillForm() {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);
		}
	}
	protected void fillForm(DropDownList dl) {
		dl.Items.Add("---");
		dl.Items.Add("Limited Access / Visitor");
		dl.Items.Add("Standard Access / User");
		dl.Items.Add("Full Access / Manager");
	}

	//Module Place-Holder
	protected void fillModule() {
		//SELECT
		var rs = from p in db.ModuleIndex
				 orderby p.ID
				 select p;

		//RESULT
		int prev = 0;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			int count = db.UserRole.Count(p => p.ID == myID && p.Module == r.Kode);
			if (count == 0) {
				//Sub-Header
				if (prev != r.IDGroup) {
					Sub_Header(r);
					prev = r.IDGroup;
				}

				TableRow tr = new TableRow();
				TableCell td;

				Add_Select(tr, r);

				td = new TableCell();
                td.Text = TD_Nama(r);
                td.Style.Add("text-align", "left");
                td.Style.Add("vertical-align", "middle");
				tr.Cells.Add(td);

				tb.Rows.Add(tr);
			}
		}
	}
	protected void Add_Select(TableRow tr, ModuleIndex r) {
		DropDownList dl = new DropDownList();
		dl.ID = r.Kode;
		dl.CssClass = select_all.CssClass;
		dl.Width = select_all.Width;

		fillForm(dl); //List-Item

		TableCell td = new TableCell();
		td.Controls.Add(dl);
		tr.Cells.Add(td);
	}

    #region roleGroupSetting
    /// Penambahan role harus ditambahkan di modul Create dan Update

    //Role Superadmin
    protected void select_Superadmin(object sender, EventArgs e)
    {
        select_all.SelectedValue = "Full Access / Manager";

        var rs = from p in db.ModuleIndex
                 orderby p.ID
                 select p;

        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            DropDownList dl = (DropDownList)tb.FindControl(r.Kode);
            if (dl != null)
                dl.SelectedValue = "Full Access / Manager";
        }
    }
    //Role JR
    protected void select_JR(object sender, EventArgs e)
    {
        select_all.SelectedValue = "---";

        var rs = from p in db.ModuleIndex
                 orderby p.ID
                 select p;

        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            DropDownList dl = (DropDownList)tb.FindControl(r.Kode);
            if (dl != null)
            {
                if (r.Kode == "DataProduct"
                    || r.Kode == "DataCustomer"
                    || r.Kode == "OutletSales"
                    || r.Kode == "OutletInv"
                    )
                {
                    dl.SelectedValue = "Full Access / Manager";
                }
                else
                {
                    dl.SelectedValue = "---";
                }
            }
        }
    }
    //Role AE/SM
    protected void select_AE_SM(object sender, EventArgs e)
    {
        select_all.SelectedValue = "---";

        var rs = from p in db.ModuleIndex
                 orderby p.ID
                 select p;

        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            DropDownList dl = (DropDownList)tb.FindControl(r.Kode);
            if (dl != null)
            {
                if (r.Kode == "DataProduct"
                    || r.Kode == "DataCustomer"
                    || r.Kode == "OutletSales"
                    || r.Kode == "OutletInv"
                || r.Kode == "TransOutgoing"
                || r.Kode == "TransIncoming"
                    )
                {
                    dl.SelectedValue = "Full Access / Manager";
                }
                else
                {
                    dl.SelectedValue = "---";
                }
            }

        }
    }
    //Role Corporate Sales
    protected void select_CorpSales(object sender, EventArgs e)
    {
        select_all.SelectedValue = "---";

        var rs = from p in db.ModuleIndex
                 orderby p.ID
                 select p;

        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            DropDownList dl = (DropDownList)tb.FindControl(r.Kode);
            if (dl != null)
            {
                if (r.Kode == "DataProduct"
                    || r.Kode == "DataCustomer"
                    || r.Kode == "OutletSales"
                    || r.Kode == "OutletInv"
                    || r.Kode == "TransOutgoing"
                    || r.Kode == "TransIncoming"
                    || r.Kode == "OutletCorporate"
                    )
                {
                    dl.SelectedValue = "Full Access / Manager";
                }
                else
                {
                    dl.SelectedValue = "---";
                }
            }

        }
    }
    //Role Area Manager
    protected void select_AM(object sender, EventArgs e)
    {
        select_all.SelectedValue = "---";

        var rs = from p in db.ModuleIndex
                 orderby p.ID
                 select p;

        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            DropDownList dl = (DropDownList)tb.FindControl(r.Kode);
            if (dl != null)
            {
                if (r.Kode == "DataProduct"
                    || r.Kode == "DataCustomer"
                    || r.Kode == "OutletSales"
                    || r.Kode == "OutletInv"
                    || r.Kode == "TransOutgoing"
                    || r.Kode == "TransIncoming"
                    || r.Kode == "AreaManager"
                    )
                {
                    dl.SelectedValue = "Full Access / Manager";
                }
                else
                {
                    dl.SelectedValue = "---";
                }
            }

        }
    }
    //Role Accounting
    protected void select_ACC(object sender, EventArgs e)
    {
        select_all.SelectedValue = "---";

        var rs = from p in db.ModuleIndex
                 orderby p.ID
                 select p;

        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            DropDownList dl = (DropDownList)tb.FindControl(r.Kode);
            if (dl != null)
            {
                if (r.Kode == "Accounting"
                    || r.Kode == "StockInv"
                    || r.Kode == "StockAudit"
                    || r.Kode == "StockReceive"
                    || r.Kode == "DataProduct"
                    || r.Kode == "TransOutgoing"
                    || r.Kode == "TransIncoming"
                    || r.Kode == "OutletInv"
                    || r.Kode == "OutletSales"
                    || r.Kode == "ManagementReport"
                    )
                {
                    dl.SelectedValue = "Full Access / Manager";
                }
                else
                {
                    dl.SelectedValue = "---";
                }
            }

        }
    }
    #endregion
    
	protected void Sub_Header(ModuleIndex r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += r.ModuleGroup.Nama;

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_Nama(ModuleIndex r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";

		return x;
	}

	//Selection Changes
	protected void select_All(object sender, EventArgs e) {
		var rs = from p in db.ModuleIndex
				 orderby p.ID
				 select p;

		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			DropDownList dl = (DropDownList) tb.FindControl(r.Kode);
			if (dl != null)
				dl.SelectedIndex = select_all.SelectedIndex;
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//SELECT
			var rs = from p in db.ModuleIndex
					 orderby p.ID
					 select p;

			foreach (var r in rs) {
				if (!Response.IsClientConnected) break;

				DropDownList dl = (DropDownList) tb.FindControl(r.Kode);
				if (dl != null) {
					deleteModule(r); //Delete First
					saveModule(r, dl);
				}
			}

			//Redirect
			Response.Redirect("UserUpdateRole.aspx?d=1&id=" + myID);
		}
	}
	protected void deleteModule(ModuleIndex r) {
		var d = db.UserRole.SingleOrDefault(p => p.ID == myID && p.Module == r.Kode);
		if (d != null) {
			//Commit
			db.UserRole.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
	protected void saveModule(ModuleIndex r, DropDownList dl) {
		if (dl.SelectedIndex > 0) {
			UserRole n = new UserRole();

			n.ID = myID;
			n.Module = r.Kode;

			//Control-Field
			switch (dl.SelectedIndex) {
				case 2:
					n.Operator = true;
					n.DownloadExcel = true;
					n.DownloadFile = true;
					break;
				case 3:
					n.Manager = true;
					n.Operator = true;
					n.DownloadExcel = true;
					n.DownloadFile = true;
					break;
			}

			//Commit
			db.UserRole.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
}