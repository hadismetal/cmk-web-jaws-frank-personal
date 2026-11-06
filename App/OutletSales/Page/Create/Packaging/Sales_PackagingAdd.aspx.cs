using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_ItemAdd : System.Web.UI.Page {
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
    App gp = new App();
	protected int myID { get { return App.QueryInt(this); } }
    protected int tipe { get { return App.QueryInt(this,"tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

            //DLC.POSListItem(myID, ddl_item);

			//Form
			fillForm();

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Sales.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Sales_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Cancel-Link
			cancel.HRef += "?id=" + myID;
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;

		href += "&kw=" + kw.Text;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			
			kw.Text = Request.QueryString["kw"];
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
        fillPackaging();
	}
	protected void fillPackaging() 
    {
        var d = db.SalesOrder.SingleOrDefault(p=>p.ID == myID);

        if(d!=null)
        {
            var rs = from p in db.StockActualPackaging
                     where p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.InHand > 0
						&& p.Packaging.Draft == false && p.Packaging.Disable == false
						&& !db.SalesOrderPackaging.Any(so => so.IDForm == myID && so.IDProduct == p.IDProduct)
                     select p;

            if (kw.Text != "")
            {
                rs = from p in rs
                     where p.Packaging.Nama.Contains(kw.Text)
                     || p.Packaging.Kode.Contains(kw.Text)
                     select p;
            }

            int index = 1;

            //int itemindex = 1;
            //int PLU = 0;
            //string[] data = (ddl_item.SelectedValue).Split(',');
            //if (data[1] != "")
            //{
            //    itemindex = Convert.ToInt32(data[0]);
            //    PLU = Convert.ToInt32(data[1]);
            //}

            if (rs.Count() == 1)
            {
                Response.Redirect("Sales_PackagingAddSave.aspx?id=" + myID + "&item=1&iditem=" + rs.First().IDProduct);
            }
            else
            {
                

                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.IDProduct);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Packaging.Kode;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Packaging.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.InHand.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    index++;
                }
            }
        }
	}

	//Display Column
	protected string TD_HyperLink(int ID) {
		string x = string.Empty;
		string href = string.Empty;

		href += "Sales_PackagingAddSave.aspx";
		href += "?id=" + myID;
		//href += "&item=" + itemindex;
		href += "&iditem=" + ID;
        //href += "&plu=" + PLU;

		x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  Select";
		x += "</a>";

		return x;
	}
}