using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_DataAgent : System.Web.UI.Page
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
	protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

            //DLC.CompanyBrand(brand);

			//Form
			fillForm();

			//Generate
			fillFilter();
			//if (App.Refresh(this)) 

            cancel.HRef += "?id=" + myID;
		}
        fillTable(sender, e);
	}

	//Exception
	protected void DataError() { Response.Redirect("Repair.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Repair_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Hidden-ID
			//idbrand.Text = d.DataRepair.IDBrand.ToString();
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;

		//Customize-Qstr
		href += "&kw=" + kw.Text;
        //href += "&brand=" + brand.SelectedValue;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
            //brand.SelectedValue = Request.QueryString["brand"];
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//SELECT

        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);

        if (d != null)
        {
            var rs = from p in db.CharProcessRepair
                     where p.Disable == false
                     select p;

            if (kw.Text != "")
                rs = rs.Where(p => p.Nama.Contains(kw.Text));

            int index = 1;

            //if (rs.Count() == 1)
            //{
            //    Response.Redirect("Repair_WorkAddSave.aspx?id=" + myID + "&item=" + rs.First().ID);
            //}
            //else
            //{

            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nama;
                tr.Cells.Add(td);

                var repaircost = db.PricingTableRepair.SingleOrDefault(p => p.IDRepair == r.ID);

                td = new TableCell();
                td.Text = Convert.ToDecimal(repaircost.Biaya).ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }

            //}
        }
	}
    protected string TD_HyperLink(CharProcessRepair r)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "Repair_WorkAddSave.aspx";
        href += "?id=" + myID;
        href += "&item=" + r.ID;

        x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
}