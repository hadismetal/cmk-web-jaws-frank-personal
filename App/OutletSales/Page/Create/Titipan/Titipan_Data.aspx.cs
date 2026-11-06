using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Titipan_Item : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("Titipan.aspx"); }

	//Form
	protected void fillForm() {
        var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
        if (d == null) DataError();
        else
        {
            //Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/Titipan_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";
        }
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
        decimal GT = 0m;

        TextBox txtbox = new TextBox();

        string txt = Request.QueryString["tb"];
        if (txt != "")
        {
            Page.RegisterStartupScript("af", "<script>$(function() {$(\"#" + txt + "\").focus();});</script>");
        }

		//Item
		fillItem(ref GT);

        tb.Focus();
	}

	//Item
	protected void fillItem(ref decimal GT) {
        var rs = from p in db.DocTitipan_Product
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            int itemindex = 0;
            if (r.IDProduct.HasValue)
                itemindex = 0;
            else
                itemindex = 1;

            td = new TableCell();
            td.Text = TD_HyperLink(itemindex,r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.IDProduct.HasValue ? r.StockProductDJ.Nomor : r.StockProductDJCustomer.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.IDProduct.HasValue ? r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama : r.StockProductDJCustomer.StockProductDJCustomer_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DocQC(r);
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "dj_" + index.ToString();
            //txt.Attributes["style"] = "width: 200px;";
            txt.Text = "";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //txt.TextMode = TextBoxMode.MultiLine;

            HiddenField hf = new HiddenField();
            hf.ID = "hfdj" + index.ToString();
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
	protected string TD_HyperLink(int ItemIndex, int ID) {
		string x = string.Empty;
		string href = string.Empty;

		x += "<div class=\"btn-group\">";
		x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + myID + "', '" + ItemIndex + "','" + ID + "')\">";
		x += "      <i class=\"icon-trash\"></i> ";
		x += "      Delete";
		x += "  </a>";
		x += "</div>";

		return x;
	}
	protected string TD_Nama(string Nama) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";

		return x;
	}
	protected string TD_Nama(string Nama, string GIA) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";
		if (GIA != "")
			x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
        var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
        if (d == null) DataError();
        else
        {
            bool check = false;
            var dj = from p in d.DocTitipan_Product
                     where p.IDDocQC == null && p.IDProduct.HasValue
                     select p;
            if (dj.Count() > 0) // revisi coding (2016-02-02) : 1 dokumen titipan hanya untuk 1 barang
            {
                check = true;
            }
            
            if (check == true)
                ErrDocQC();
            else
            {
                int sumprod = d.DocTitipan_Product.Count();

                if (sumprod == 1)
                {
                    int index = 1;

                    TextBox txt = new TextBox();
                    string hf = "";

                    foreach (TableRow row in tb.Rows)
                    {
                        try
                        {
                            hf = ((HiddenField)row.FindControl("hfdj" + index.ToString())).Value;
                            txt = (TextBox)row.FindControl("dj_" + index.ToString());
                            var x = db.DocTitipan_Product.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                            if (x != null)
                            {
                                try
                                {
                                    x.Keterangan = txt.Text;

                                    db.SubmitChanges();


                                    if (x.IDProductTitipan != null)
                                    {
                                        var z = x.StockProductDJCustomer;

                                        // hitung ulang netto weight untuk harga emas pada table char product
                                        RecalWeight.RecalWeightTitipan(db, z, z.StockProductDJCustomer_CharProduct);
                                        db.SubmitChanges();
                                    }
			
                                }
                                catch (Exception ex) { }
                            }
                        }
                        catch (Exception ex) { }
                        index++;
                    }
                    d.Nomor = Generate_Nomor(d);
                    //d.Draft = false;
                    //d.DraftDate = null;
                    //d.StatusTitipan = 1;
                    db.SubmitChanges();
                    Response.Redirect("Titipan_Submit.aspx?id=" + myID);
                }
                else
                {
                    ErrInvalid();
                }
            }
        }
	}
    protected void ErrDocQC()
    {
        err.InnerText = "Dokumen QC belum dipilih";
    }
    protected void ErrInvalid()
    {
        err.InnerText = "Barang titipan belum ada / tidak boleh lebih dari 1 barang";
    }
    protected string Generate_Nomor(DocTitipan j)
    {
        string x = string.Empty;

        int Tahun = DateTime.Now.Year;
        int Bulan = DateTime.Now.Month;

        //int Nomor = db.Titipan.Count(p => p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan );
        //Nomor = Nomor + 1;

        int Nomor = 1;

        try
        {
            var s = (from p in db.DocTitipan
                     where p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan
                     && p.Nomor.Substring(3, 1) == "" + j.TipeLokasi.ToString() + ""
                     && p.Nomor.Substring(5, 3) == "" + j.IDLokasi.ToString().PadLeft(3, '0') + ""
                     orderby p.ID descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(15, 4)) + 1;
        }
        catch (Exception ex) { }

        return "TK/" + j.TipeLokasi.ToString() + "/" + j.IDLokasi.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
    }

    protected string DocQC(DocTitipan_Product r)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (r.IDDocQC == null && r.IDProduct.HasValue)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-info\" href=\"Titipan_AddDocQC.aspx?id=" + myID + "&itemid=" + r.IDProduct + "\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Add Doc QC";
            x += "  </a>";
            x += "</div>";
        }
        else if (r.IDDocQC != null && r.IDProduct.HasValue)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-info\" href=\"Titipan_AddDocQC.aspx?id=" + myID + "&itemid=" + r.IDProduct + "\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += r.DocQCDJ.Nomor;
            x += "  </a>";
            x += "</div>";
        }
        else
            x += "";

        return x;
    }
}