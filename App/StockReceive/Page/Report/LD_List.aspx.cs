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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {

            DLC.LocationList(cabang, tipe);

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);

            DLC.DDLSupplier(supplier, 0);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
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

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.StockReceiveLD_Stone1B
                     //where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;

        rs = rs.OrderByDescending(p => p.StockReceiveLD.ApprovalTgl);

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.StockReceiveLD.ApprovalTgl >= start && p.StockReceiveLD.ApprovalTgl < end.AddDays(1));
        }

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "" || p.GIA.Equals(kw.Text));
        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
        if (Cabang > 0) rs = rs.Where(p => p.StockReceiveLD.IDWarehouse == Cabang);
        //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

        string Supplier = supplier.SelectedValue;
        if (Supplier != "0")
            rs = rs.Where(p => p.StockReceiveLD.SupplierNama.Equals(Supplier));

        if (rs != null)
        {
            int index = 1;
            //decimal TotalStock = 0m;
            //decimal TotalResell = 0m;
            //decimal TotalExchange = 0m;
            //decimal TotalHarga = 0m;
            //decimal TotalModal = 0m;
            //decimal TotalCarat = 0m;

            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalHargaTotalM = 0m;
            decimal TotalHargaTotalR = 0m;
            decimal TotalRappaport = 0m;

            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockReceiveLD.SupplierNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Stone1B.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.StockReceiveLD.ApprovalTgl);
                tr.Cells.Add(td);

                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                /*td = new TableCell();
                td.Text = Jual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0");
                tr.Cells.Add(td);*/



                td = new TableCell();
                td.Text = "<a href=\"http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + r.GIA + "\" target=\"_blank\">" + r.GIA + "</a>";
                tr.Cells.Add(td);

                /// Nama SUpplier
                td = new TableCell();
                td.Text = r.StockReceiveLD.SupplierNama;
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.TotalButir).ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.TotalCarat).ToString("N3");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.HargaTotalM).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.HargaTotal).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r.HargaInputP).ToString("N2");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = r.StockActualLD_Stone1B.InHand.ToString("N0");
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
                //td.Text = Convert.ToDecimal(r.HargaTotal.Value).ToString("N2");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Convert.ToDecimal(r.HargaTotalM.Value).ToString("N2");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = r.StockReceiveLD.SupplierNama;
                //tr.Cells.Add(td);

                tb.Rows.Add(tr);

                //TotalStock += Jual;
                //TotalResell += Resell;
                //TotalExchange += Exchange;
                //TotalModal += Convert.ToDecimal(r.HargaTotalM.Value);
                //TotalHarga += Convert.ToDecimal(r.HargaTotal.Value);
                //TotalCarat += Convert.ToDecimal(r.TotalCarat);

                TotalButir += p_ForceConvertToDecimal(r.TotalButir);
                TotalCarat += p_ForceConvertToDecimal(r.TotalCarat);
                TotalHargaTotalM += p_ForceConvertToDecimal(r.HargaTotalM);
                TotalHargaTotalR += p_ForceConvertToDecimal(r.HargaTotal);
                TotalRappaport += p_ForceConvertToDecimal(r.HargaInputP);

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
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalButir.ToString("N0") + "</b>";
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaTotalM.ToString("N2") + "</b>";
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaTotalR.ToString("N2") + "</b>";
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalRappaport.ToString("N2") + "</b>";
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            kw.Text = Request.QueryString["kw"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            supplier.SelectedValue = Request.QueryString["supplier"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&tipe=" + tipe;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&supplier=" + supplier.SelectedValue;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "LD_ListExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&tipe=" + tipe;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&supplier=" + supplier.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "LD_ListPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&tipe=" + tipe;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&supplier=" + supplier.SelectedValue;

        Response.Redirect(href);
    }
}