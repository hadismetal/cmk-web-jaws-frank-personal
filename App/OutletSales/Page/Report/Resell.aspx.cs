using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            status.SelectedValue = "1";
            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.Resell
                 where !(from z in db.TradeIn
                         where !z.Draft
                         select z.IDResell).Contains(p.ID)
                 select p;

        string Status = "";
        Status = Request.QueryString["status"];
        if (Status == "1")
            rs = rs.Where(p => p.Draft == false && (p.StatusVoid == 0 || p.StatusVoid == null));
        else if (Status == "2")
            rs = rs.Where(p => p.Draft == true);
		else if (Status == "3")
            rs = rs.Where(p => (p.Draft == false && p.StatusVoid == 1));

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi) && p.TipeLokasi == 1);
        }

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.Nomor == "" + keyword + "" || p.CustomerNama.Contains(kw.Text) || p.ResellDJ.Any(z => z.StockProductDJ.Nomor == "" + keyword + "") || p.ResellGJ.Any(z => z.StockProductGJ.Nomor == "" + keyword + "") || p.ResellPG.Any(z => z.StockProductPG.Nomor == "" + keyword + "") || p.ResellLD.Any(z => z.StockProductLD_Stone1B.Nomor == "" + keyword + ""));

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal Total = 0m;
            decimal TotalQtyTradeIn = 0m;
            decimal TotalBerat = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;

            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                decimal Harga = 0m;
                int QtyTradeIn = 0;


                QtyTradeIn = r.ResellDJ.Count() + r.ResellPG.Count() + r.ResellLD.Count() + r.ResellGJ.Count();

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CustomerNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = QtyTradeIn.ToString();
                tr.Cells.Add(td);

                decimal NetWeight = 0m;

                NetWeight = r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight) + r.ResellGJ.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight) + r.ResellPG.Sum(p => p.StockProductPG.GrossWeight.Value);

                td = new TableCell();
                //td.Text = r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
                td.Text = NetWeight.ToString("N2");
                tr.Cells.Add(td);

                decimal StoneQty = 0m;
                StoneQty = Convert.ToDecimal(r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value)) + Convert.ToDecimal(r.ResellLD.Count());

                td = new TableCell();
                //td.Text = Convert.ToDecimal(r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty)).ToString("N0");
                td.Text = StoneQty.ToString("N0");
                tr.Cells.Add(td);

                decimal StoneWeight = 0m;
                StoneWeight = r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneWeight) + r.ResellLD.Sum(p => p.StockProductLD_Stone1B.TotalCarat);

                td = new TableCell();
                td.Text = StoneWeight.ToString("N3");
                tr.Cells.Add(td);

                try
                {
                    Harga = Convert.ToDecimal(r.ResellDJ.Sum(p => p.HargaRupiah)) + Convert.ToDecimal(r.ResellPG.Sum(p => p.HargaRupiah)) + Convert.ToDecimal(r.ResellLD.Sum(p=>p.HargaRupiah)) + Convert.ToDecimal(r.ResellGJ.Sum(p=>p.HargaRupiah));
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = Harga.ToString("N0");
                tr.Cells.Add(td);
				
				td = new TableCell();
				string GroupResell = Db.SingleString("SELECT TOP 1 ISNULL(GroupResell, 'CMK') FROM Resell WHERE ID = " + r.ID ,Db.Cnn_DB);
				td.Text = (GroupResell == "CMK") ? "<p style=\"color:blue\">Resell CMK</p>" : ((GroupResell == "LE") ? "<p style=\"color:green\">Resell Butik Emas</p>" : "<p style=\"color:orange\">Resell Gold Tunai</p>");
				tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.Keterangan;
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.VoidKeterangan;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Total += Harga;
                TotalQtyTradeIn += QtyTradeIn;
                //TotalBerat += r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight);
                //TotalBerat += NetWeight;
                //TotalButir += Convert.ToDecimal(r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty));
                TotalButir += StoneQty;
                //TotalCarat += r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneWeight);
                TotalCarat += StoneWeight;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 5;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBerat).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(Total).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            status.Text = Request.QueryString["status"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.Text;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(Resell r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info btn-sm\" target=\"_blank\" href=\"Resell_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";
        if (r.Draft == true)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger btn-sm\" onClick=\"return confirm('Are you Sure');\" href=\"Resell_Delete.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Delete";
            x += "</a>";
        }
        if ((r.StatusVoid == null || r.StatusVoid == 0) && r.Draft == false && r.Tgl == DateTime.Today)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger btn-sm\" onClick=\"return confirm('Are you Sure');\" href=\"Resell_FormVoid.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Void";
            x += "</a>";
        }
        if (r.Draft == false && r.StatusVoid != 1)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-info btn-sm\" target=\"_blank\" href=\"/OutletSales/Page/Create/Resell/Print_Receipt.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-print\"></i> ";
            x += "  Print";
            x += "</a>";
        }
        return x;
    }
}