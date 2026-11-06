using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Resell : System.Web.UI.Page 
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
    protected int myItemID { get { return App.QueryInt(this,"itemid"); } }
    protected int myResellID { get { return App.QueryInt(this, "rid"); } }
    protected int myTipeID { get { return App.QueryInt(this,"tipe"); } }
    protected int myIDSO { get { return App.QueryInt(this, "so"); } }
    protected int myIDRO { get { return App.QueryInt(this, "ro"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {

			
			
		}
        fillTable(sender, e);
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {

        if (myTipeID == 1)
        {
            /*var rs = from p in db.DocQCDJ
                     where p.IDProduct == myItemID
                     select p;*/
            var rs = db.ExecuteQuery<DocQCDJ>("SELECT * FROM DocQCDJ WHERE IDProduct = " + myItemID + " AND ApprovalNama IS NOT NULL AND RejectNama IS NULL");

            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r.ID);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.QCNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Keterangan;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }
        }
        else if (myTipeID == 2)
        {
            /*var rs = from p in db.DocQCPG
                     where p.IDProduct == myItemID
                     select p;*/
            var rs = db.ExecuteQuery<DocQCPG>("SELECT * FROM DocQCPG WHERE IDProduct = " + myItemID + " AND ApprovalNama IS NOT NULL AND RejectNama IS NULL");

            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r.ID);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.QCNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Keterangan;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }
        }
        else if (myTipeID == 3)
        {
            /*var rs = from p in db.DocQCGJ
                     where p.IDProduct == myItemID
                     select p;*/
            var rs = db.ExecuteQuery<DocQCGJ>("SELECT * FROM DocQCGJ WHERE IDProduct = " + myItemID);

            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r.ID);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.QCNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Keterangan;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }
        }
        else if (myTipeID == 4)
        {
            /*var rs = from p in db.DocQCLD
                     where p.IDProduct == myItemID
                     select p;*/
			var rs = db.ExecuteQuery<DocQCLD>("SELECT * FROM DocQCLD WHERE IDProduct = " + myItemID + " AND ApprovalProcurementFinal IS NOT NULL");

            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r.ID);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.QCNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Keterangan;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }
        }
	}

    protected string TD_HyperLink(int IDDoc)
    {
        string x = string.Empty;

        x += "<div class=\"btn-group\">";
        x += "  <a class=\"btn btn-info\" href=\"Resell_AddDocQCSave.aspx?id=" + myID + "&so=" +  myIDSO + "&ro=" + myIDRO + "&docid=" + IDDoc + "&rid=" + myResellID + "&itemid=" + myItemID + "&tipe=" + myTipeID + "\">";
        x += "      <i class=\"icon-search\"></i> ";
        x += "      Select";
        x += "  </a>";
        x += "</div>";

        return x;
    }

}