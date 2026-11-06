using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Repair_Item : System.Web.UI.Page {
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
			//HyperLink
			add.HRef += "?id=" + myID;

			//Form
			fillForm();

			//Generate
			
		}
        fillTable(sender, e);
	}

	//Exception
	protected void DataError() { Response.Redirect("Repair.aspx"); }

	//Form
	protected void fillForm() {
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            //Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/Repair_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

            lit_plu.Text = d.StockProductDJ.Nomor;
            lit_tgl.Text = Str.FullDate(d.Tgl);
            lit_tgl2.Text = Str.FullDate(d.TglSelesai);
            lit_keterangan.Text = Str.HtmlBreak(d.Keterangan);
        }
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//Item
		fillItem();
	}

	//Item
    protected void fillItem()
    {
        var rs = from p in db.DocRepair_Repair
                 where p.IDForm == myID
                 select p;

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
            td.Text = r.CharProcessRepair.Nama;
            tr.Cells.Add(td);

            var repaircost = db.PricingTableRepair.SingleOrDefault(p => p.IDRepair == r.CharProcessRepair.ID);

            td = new TableCell();
            td.Text = Convert.ToDecimal(repaircost.Biaya).ToString("N0");
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "rp_" + index.ToString();
            txt.Attributes["style"] = "width: 100%;";
            txt.Attributes["class"] = "form-control input-sm";
            //txt.TextMode = TextBoxMode.MultiLine;
            txt.Text = r.Keterangan != null ? r.Keterangan : "";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            HiddenField hf = new HiddenField();
            hf.ID = "hfrp" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            index++;

        }
    }
	//Display Column
	protected string TD_HyperLink(int ID) {
		string x = string.Empty;
		string href = string.Empty;

		x += "<div class=\"btn-group\">";
		x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + myID + "','" + ID + "')\">";
		x += "      <i class=\"icon-trash\"></i> ";
		x += "      ";
		x += "  </a>";
		x += "</div>";

		return x;
	}
	protected string TD_Nama(string Nama) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {

        TextBox txt = new TextBox();
        string hf = "";

        int index = 1;

        foreach (TableRow row in tb.Rows)
        {
            string ket = "";
            try
            {
                hf = ((HiddenField)row.FindControl("hfrp" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("rp_" + index.ToString());
                ket = txt.Text;

                var d = db.DocRepair_Repair.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    d.Keterangan = ket;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) { }
            index++;
        }

		Response.Redirect("Repair_CharProduct02.aspx?id=" + myID);
	}
}