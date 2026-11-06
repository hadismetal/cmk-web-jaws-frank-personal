using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page {
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

    protected int id { get { return App.QueryInt(this, "id"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
            fillTable();
            
		}
        
	}

    //Display Table
    protected void fillTable()
    {
        title.InnerText = "Audit ID : " + id.ToString();

        

        var d = db.StockAuditDJ.SingleOrDefault(p => p.ID == id && p.StatusFinish.Value == true);

        if (d != null)
        {
            string NamaLokasi = "";
            string TipeLokasi = DL.LocationType(d.TipeLokasi);
            if (Convert.ToInt32(d.IDLokasi) == 0)
                NamaLokasi = "ALL";
            else
            {
                if (d.TipeLokasi == 0)
                {
                    var w = db.LocWarehouse.SingleOrDefault(p => p.ID == d.IDLokasi);
                    NamaLokasi = w.Nama;
                }
                if (d.TipeLokasi == 1)
                {
                    var o = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                    NamaLokasi = o.Nama;
                }
                if (d.TipeLokasi == 2)
                {
                    var exh = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                    NamaLokasi = exh.Nama;
                }
            }

            title2.InnerText = TipeLokasi + " " + NamaLokasi;
            title3.InnerText = Str.TglJam(d.OperatorTgl) + " - " + Str.TglJam(d.FinishJam);
            if (tipe == 1)
                title4.InnerText = "Fisik Ada,Data Tidak Ada";
            else if(tipe==2)
                title4.InnerText = "Fisik Tidak Ada,Data Ada";
            else if(tipe==3)
                title4.InnerText = "Gambar Tidak Sesuai";
            else if(tipe==4)
                title4.InnerText = "Bandrol Tidak Sesuai";

            int index = 1;

            if (tipe == 1)
            {
                var err1 = from c in d.StockAuditDJDetail
                           where !(from o in d.StockAuditItemDJ
                                   select o.IDProduct)
                                  .Contains(c.IDProduct)
                           select c;

                foreach (var r in err1)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.ProtoNomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.StockProductDJ);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 2)
            {
                var rs = d.StockAuditItemDJ;

                if (rs != null)
                {
                    foreach (var r in rs)
                    {
                        var cek = db.StockAuditDJDetail.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.AuditID == r.AuditID);
                        if (cek == null)
                        {
                            var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProduct);
                            if (x != null)
                            {
                                TableRow tr = new TableRow();
                                TableCell td;

                                td = new TableCell();
                                td.Text = x.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = x.ProtoNomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = x.StockProductDJ_CharProduct.CharProductItem.Nama;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = x.Keterangan;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = TD_HyperLink(x);
                                tr.Cells.Add(td);

                                tb.Rows.Add(tr);
                            }
                        }
                    }
                }
            }
            else if (tipe == 3)
            {
                var err3 = from c in d.StockAuditDJDetail
                           where c.ImgCheck == true
                           select c;

                foreach (var r in err3)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.ProtoNomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.StockProductDJ);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 4)
            {
                var err4 = from c in d.StockAuditDJDetail
                           where c.BandrolCheck == true
                           select c;

                foreach (var r in err4)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.ProtoNomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.StockProductDJ);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 5)
            {
                var rs = d.StockAuditDJDetail;

                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.ProtoNomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.StockProductDJ);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 6)
            {
                var rs = d.StockAuditItemDJ;

                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.ProtoNomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.StockProductDJ);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
        }
        else
            Response.Redirect("StockAuditDJ.aspx");

    }

    protected string TD_HyperLink(StockProductDJ r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"/DataProduct/Page/Catalogue/DJ_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "StockAuditDJ_DetailExcel.aspx?id=" + id + "&tipe=" + tipe;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "StockAuditDJ_DetailPrint.aspx?id=" + id + "&tipe=" + tipe;

        Response.Redirect(href);
    }
}