using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Update_MU : System.Web.UI.Page {
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
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {

			//Generate
			fillFilter();
			fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);


		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {

        ////SELECT
        //var rs = from p in db.PricingTableResellLDMU
        //         select p;

        ////RESULT
        //if (rs.Count() == 0) App.TableNoResult(tb);

        ////RESULT
        //string prev = "";
        //foreach (var r in rs) {
        //    if (!Response.IsClientConnected) break;

			

        //    TableRow tr = new TableRow();
        //    TableCell td;

        //    td = new TableCell();
        //    td.Text = TD_HyperLink(r);
        //    tr.Cells.Add(td);

        //    td = new TableCell();
        //    td.Text = TD_Persen(r);
        //    td.CssClass = "text-right";
        //    tr.Cells.Add(td);

        //    tb.Rows.Add(tr);
        //}
	}
    //protected string TD_HyperLink(PricingTableResellLDMU r) {
    //    string x = string.Empty;

    //    x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../UpdateProcess/MULD.aspx?id=" + r.ID + "\">";
    //    x += "  <i class=\"icon-pencil\"></i> ";
    //    x += "  Update";
    //    x += "</a>";

    //    return x;
    //}
    //protected string TD_Persen(PricingTableResellLDMU r) {
    //    string x = string.Empty;

    //    x += "<p class=\"text-success text-lg\">";
    //    x += Str.Number2(r.Persen);
    //    x += "</p>";

    //    return x;
    //}
}