using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_LD : System.Web.UI.Page
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
    protected int ReturnID { get { return App.QueryInt(this, "id"); } }
    protected int ItemID { get { return App.QueryInt(this, "item"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=LD_Export.xls");
            Set();
        }
    }

    private decimal p_ForceConvertToDecimal(object objValue)
    {
        decimal decReturn = decimal.Zero;

        try
        {
            decReturn = Convert.ToDecimal(objValue);
        }
        catch
        {
            //  do nothing.
            //  return 0;
        }

        return decReturn;
    }

    protected void Set()
    {
        DateTime start;
        DateTime end;

        int Qty = 0;
        decimal hargam = 0m;
        decimal hargajual = 0m;

        var rs = from p in db.StockReturnLD_Product
                 where p.StockReturnLD.Approval != null
                 select p;

        if (Request.QueryString["kw"] != "")
        {
            rs = from p in rs
                 where p.Nomor.Contains(Request.QueryString["kw"]) || p.StockProductLD_Stone1B.Nomor.Contains(Request.QueryString["kw"])
                 select p;
        }

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            start = Convert.ToDateTime(Request.QueryString["dari"]);
            end = Convert.ToDateTime(Request.QueryString["sampai"]);
            rs = rs.Where(p => p.StockReturnLD.ApprovalTgl >= start && p.StockReturnLD.ApprovalTgl < end.AddDays(1));
        }

        string Supplier = Request.QueryString["supplier"];
        if (Supplier != "")
            rs = rs.Where(p => p.StockProductLD_Stone1B.StockProductLD.SupplierNama.Equals(Supplier));
        string SupplierTujuan = Request.QueryString["suppliertujuan"];
        if (SupplierTujuan != "")
            rs = rs.Where(p => p.StockReturnLD.SupplierNama.Equals(SupplierTujuan));

        rs = rs.OrderByDescending(p => p.StockReturnLD.ApprovalTgl);

        if (rs != null)
        {
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalHargaTotalM = 0m;
            decimal TotalHargaTotalR = 0m;
            decimal TotalRappaport = 0m;

            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockReturnLD.NoDokumen;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductLD_Stone1B.Stone1B.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.StockReturnLD.ApprovalTgl);
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.StockReturnLD.SupplierNama;
                tr.Cells.Add(td);

                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                if (r.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage == 1)
                    Jual = r.StockProductLD_Stone1B.StockActualLD_Stone1B.InHand;
                else if (r.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage == 3)
                    Exchange = r.StockProductLD_Stone1B.StockActualLD_Stone1B.InHand;
                else if (r.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage == 2)
                    Resell = r.StockProductLD_Stone1B.StockActualLD_Stone1B.InHand;

                /*td = new TableCell();
                td.Text = Jual.ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);*/



                td = new TableCell();
                td.Text = "<a href=\"http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + r.StockProductLD_Stone1B.GIA + "\" target=\"_blank\">" + r.StockProductLD_Stone1B.GIA + "</a>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.StockProductLD_Stone1B.TotalButir).ToString("N0").Replace(".","").Replace(",",".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.StockProductLD_Stone1B.TotalCarat).ToString("N3").Replace(".","").Replace(",",".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.StockProductLD_Stone1B.HargaTotalM).ToString("N2").Replace(".","").Replace(",",".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.StockProductLD_Stone1B.HargaTotal).ToString("N2").Replace(".","").Replace(",",".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.StockProductLD_Stone1B.HargaInputP).ToString("N2").Replace(".","").Replace(",",".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(r.StockReturnLD.Keterangan);
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = r.StockActualLD_Stone1B.InHand.ToString("N0").Replace(".","").Replace(",",".");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.SubStorage(r.StockActualLD_Stone1B.Substorage);
                //tr.Cells.Add(td);

                //string Lokasi = "";
                //if (r.StockActualLD_Stone1B.TipeLokasi == 0)
                //    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
                //else if (r.StockActualLD_Stone1B.TipeLokasi == 1)
                //    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
                //else if (r.StockActualLD_Stone1B.TipeLokasi == 2)
                //    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
                //td = new TableCell();
                //td.Text = Lokasi;
                //tr.Cells.Add(td);


                //td = new TableCell();
                //td.Text = Convert.ToDecimal(r.HargaTotal.Value).ToString("N2").Replace(".","").Replace(",",".");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Convert.ToDecimal(r.HargaTotalM.Value).ToString("N2").Replace(".","").Replace(",",".");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = r.StockProductLD.SupplierNama;
                //tr.Cells.Add(td);

                tb.Rows.Add(tr);

                //TotalStock += Jual;
                //TotalResell += Resell;
                //TotalExchange += Exchange;
                //TotalModal += Convert.ToDecimal(r.HargaTotalM.Value);
                //TotalHarga += Convert.ToDecimal(r.HargaTotal.Value);
                //TotalCarat += Convert.ToDecimal(r.TotalCarat);

                TotalButir += p_ForceConvertToDecimal(r.StockProductLD_Stone1B.TotalButir);
                TotalCarat += p_ForceConvertToDecimal(r.StockProductLD_Stone1B.TotalCarat);
                TotalHargaTotalM += p_ForceConvertToDecimal(r.StockProductLD_Stone1B.HargaTotalM);
                TotalHargaTotalR += p_ForceConvertToDecimal(r.StockProductLD_Stone1B.HargaTotal);
                TotalRappaport += p_ForceConvertToDecimal(r.StockProductLD_Stone1B.HargaInputP);

                index++;
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
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalButir.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalCarat.ToString("N3").Replace(".","").Replace(",",".") + "</b>";
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaTotalM.ToString("N2").Replace(".","").Replace(",",".") + "</b>";
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaTotalR.ToString("N2").Replace(".","").Replace(",",".") + "</b>";
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalRappaport.ToString("N2").Replace(".","").Replace(",",".") + "</b>";
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }
}