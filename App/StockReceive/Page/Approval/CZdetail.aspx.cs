using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Approval_CZdetail : System.Web.UI.Page
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

        fillTable(sender, e);
        Label2.Text = Request.QueryString["ttb"].ToString();
        if (!IsPostBack)
        {
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }
    protected void fillTable(object sender, EventArgs e)
    {
       string ttb = Request.QueryString["ttb"].ToString();
        Label2.Text = ttb;
        var rs = from p in db.StockReceivePG
                     //where p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                 where p.NoKirimCZ != null && p.NoTTB == ttb
                 select p;

        int a = 1;
        decimal totalberat = 0;
        foreach (var r in rs)
        {

            TableRow tr = new TableRow();
            TableCell td;


            td = new TableCell();
            td.Text = a.ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            string cut = r.ImgPicture.ToString();
            string Cutted = System.IO.Path.GetFileName(cut);
            td.Text = "<img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"150\" />"+
                "<br/>"+ Cutted;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.KodeBarang;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DimensiR).ToString("N2");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.GrossWeight).ToString("N2");
            tr.Cells.Add(td);


            td = new TableCell();
            td.Text = (from p in db.CharGoldModel where p.ID == r.Model select p.Nama).Single();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (from p in db.CharProductItem where p.ID == r.ProductItem select p.Nama).Single();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (from p in db.CharFrameColor where p.ID == r.FrameColor select p.Nama).Single();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.KadarTukaranJual).ToString("N2");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.KadarTukaranBeli).ToString("N2");
            tr.Cells.Add(td);

            a++;
            totalberat += r.GrossWeight;
            tb.Rows.Add(tr);

        }
        if (rs.Count() > 0)
        {

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<b>Total</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = totalberat.ToString("N2");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }
}