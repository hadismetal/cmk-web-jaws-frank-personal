using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class DJ : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Packaging_Outgoing.xls");
            fillTable(sender, e);

        }
    }


       //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;

        int tF = 0; // type from
        int tT = 0; // type to

        int brand = Convert.ToInt32(Request.QueryString["brand"]);
        int lokasiFrom = Convert.ToInt32(Request.QueryString["lokasifrom"]);
        int lokasiTo = Convert.ToInt32(Request.QueryString["lokasito"]);
        int status = Convert.ToInt32(Request.QueryString["status"]);
        int tipeLokasiFrom = Convert.ToInt32(Request.QueryString["tipelokasifrom"]);
        int tipeLokasiTo = Convert.ToInt32(Request.QueryString["tipelokasito"]);
        DateTime dateFrom = Convert.ToDateTime(Request.QueryString["pf"]);
        DateTime dateTo = Convert.ToDateTime(Request.QueryString["pt"]);
		
        
        //WHERE
        string w = "1=1";
        //if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;
        if (status > 0) w += " AND StatusTransOut = " + status;
        if (tipeLokasiFrom > 0) { tF = Convert.ToInt32(tipeLokasiFrom); }
        if (tipeLokasiTo > 0) { tT = Convert.ToInt32(tipeLokasiTo); }
        
        //SELECT
        var rs = (from p in db.StockOutgoingPackaging.Where(w)
                  select p);
        rs = rs.Where(p => (brand >= 0) ? p.IDBrand == brand : 1 == 1 && p.Tgl >= dateFrom && p.Tgl <= dateTo && p.ApprovalTgl != null);

        if (tF > 0) { rs = rs.Where(p => p.TipeAsal == tF); }
        if (tT > 0) { rs = rs.Where(p => p.TipeTujuan == tT); }

        if (lokasiFrom > 0)
        {
            if (tF == 0)
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.IDAsal equals q.ID
                     where q.ID == lokasiFrom
                     select p;
            }
            if (tF == 1)
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.IDAsal equals q.ID
                     where q.ID == lokasiFrom
                     select p;
            }
            if (tF == 2)
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.IDAsal equals q.ID
                     where q.ID == lokasiFrom
                     select p;
            }
        }

        if (lokasiTo > 0)
        {

            if (tT == 0)
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.IDTujuan equals q.ID
                     where q.ID == lokasiTo
                     select p;
            }
            if (tT == 1)
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.IDTujuan equals q.ID
                     where q.ID == lokasiTo
                     select p;
            }
            if (tT == 2)
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.IDTujuan equals q.ID
                     where q.ID == lokasiTo
                     select p;
            }
        }

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            /*rs = from p in rs
                 where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                 || (p.TipeTujuan == d.First().TipeLokasi && p.IDTujuan == d.First().IDLokasi)
                 select p;*/

            rs = from p in rs
                 where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                 select p;
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {
            rs = from p in rs
                 where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                 select p;
        }

        rs = rs.OrderByDescending(p => p.OperatorTgl);

        int vF = 0; // id from
        int vT = 0; // id to

        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            foreach (var item in rs)
            {
                var rsI = from p in db.StockOutgoingPackaging_Product where p.IDForm == item.ID select p;

                if (!Response.IsClientConnected) break;

                vF = item.IDAsal; // id from
                vT = item.IDTujuan; // id to
                tF = item.TipeAsal; // value from
                tT = item.TipeTujuan; // value to

                //Sub-Header
                //if (prev != r1.IDBrand)
                //{
                //    Sub_Header(vT, r1.ID);
                //    prev = r1.IDBrand;
                //}

                TableRow tr = new TableRow();
                TableCell td;


                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.FullDate(item.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Keterangan;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text =  TD_Nama(tF, vF) ;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nama(tT, vT) ;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rsI.Count().ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Operator;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.FullDate(item.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Approval;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.FullDate(item.ApprovalTgl);
                tr.Cells.Add(td);
                tb.Rows.Add(tr);
            }
        }
        bool isValidFromTo = true;
    }

    protected string TD_Nama(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        /*else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }*/

        return x;
    }
}