using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_PG : System.Web.UI.Page {
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
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=StockAudit_DetailExcel.xls");

            fillTable();
            
		}
        
	}

    //Display Table
    protected void fillTable()
    {
        title.InnerText = "Audit ID : " + id.ToString();

        

        var d = db.StockAuditPG.SingleOrDefault(p => p.ID == id && p.StatusFinish.Value == true);

        if (d != null)
        {
            var sai = d.StockAuditItemPG; //StockAuditItemPG
            var sadtl = d.StockAuditPGDetail; //StockAuditPGDetail

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
            else if (tipe == 8)
                title4.InnerText = "Multiple Input";

            int index = 1;

            if (tipe == 1)
            {
                var err1 = from c in sadtl
                           where !(from o in sai
                                   select o.IDProduct)
                                  .Contains(c.IDProduct)
                           select c;

                foreach (var r in err1)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.NoMeja;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductPG.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 2)
            {
                var rs = from p in sai
                         where p.AuditID == id
                         && !(from z in sadtl
                              where z.AuditID == id
                              select z.IDProduct).Contains(p.IDProduct)
                         select p;

                if (rs != null)
                {
                    foreach (var r in rs)
                    {
                        
                            var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProduct);
                            if (x != null)
                            {
                                TableRow tr = new TableRow();
                                TableCell td;

                                td = new TableCell();
                                td.Text = "-";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = x.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = x.CharProductItem.Nama;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = x.Keterangan;
                                tr.Cells.Add(td);

                                tb.Rows.Add(tr);
                            }
                        
                    }
                }
            }
            else if (tipe == 3)
            {
                var err3 = from c in sadtl
                           where c.ImgCheck == true
                           select c;

                foreach (var r in err3)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.NoMeja;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductPG.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 4)
            {
                var err4 = from c in sadtl
                           where c.BandrolCheck == true
                           select c;

                foreach (var r in err4)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.NoMeja;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductPG.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 5)
            {
                var rs = sadtl;

                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.NoMeja;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductPG.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 6)
            {
                var rs = sai;

                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductPG.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 8)
            {
                var rs = from c in sadtl
                         where c.AuditID == id
                         select c;

                var src = rs.GroupBy(p => p.IDProduct);
                var ctr = src.Where(p => p.Count() > 1);

                foreach (var r in ctr)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = index++.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    var u = db.StockProductPG.SingleOrDefault(p => p.ID == r.Key);
                    var cek = rs.Where(p => p.IDProduct == r.Key);

                    td = new TableCell();
                    td.Text = u.Nomor + " (" + cek.Count().ToString() + ")";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = u.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
        }
        else
            Response.Redirect("StockAuditPG.aspx");

    }
}