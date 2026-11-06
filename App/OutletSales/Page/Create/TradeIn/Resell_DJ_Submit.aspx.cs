using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_DJ_Submit : System.Web.UI.Page
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
    protected int myItemID { get { return App.QueryInt(this, "itemid"); } }
    protected int myIDSO { get { return App.QueryInt(this, "so"); } }
    protected int myIDRO { get { return App.QueryInt(this, "ro"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("TradeIn.aspx?id=" + myID); }

    //Form
    protected void fillForm()
    {
        var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myItemID);
        if (d == null) DataError();
        else
        {
            //Header

            tglcetak.Text = DateTime.Now.ToString();

            ////Product Char.
            var product = d.StockProductDJ_CharProduct;
            if (product == null) save.Disabled = true;
            else
            {
                productitem.InnerHtml = product.CharProductItem.Nama + " (" + product.CharProductItem.NamaKode + ")";
                productcategory.InnerHtml = product.CharProductCategory.Nama + " (" + product.CharProductCategory.NamaKode + ")";
                productlevel.InnerHtml = product.CharProductLevel.Nama + " (" + product.CharProductLevel.NamaKode + ")";
                stonedist.InnerHtml = product.CharStoneDist.Nama + " (" + product.CharStoneDist.NamaKode + ")";
                framematerial.InnerHtml = product.CharFrameMaterial.Nama + " (" + product.CharFrameMaterial.NamaKode + ")";
                framefinishing.InnerHtml = product.CharFrameFinishing.Nama + " (" + product.CharFrameFinishing.NamaKode + ")";
                framecolor.InnerHtml = product.CharFrameColor.Nama + " (" + product.CharFrameColor.NamaKode + ")";
                processcons.InnerHtml = product.CharProcessCons.Nama + " (" + product.CharProcessCons.NamaKode + ")";

                //Weight
                grossweight.InnerHtml = Str.Number3(product.GrossWeight);
                stonecarat.InnerHtml = Str.Number3(product.StoneCarat);
                stoneweight.InnerHtml = Str.Number3(product.StoneWeight);
                nettoweight.InnerHtml = Str.Number3(product.NettoWeight);

                //Dimensi
                dimensid.InnerHtml = Str.Number2(product.DimensiD);
                dimensip.InnerHtml = Str.Number2(product.DimensiP);
                dimensil.InnerHtml = Str.Number2(product.DimensiL);
                dimensir.InnerHtml = Str.Number2(product.DimensiR);
            }

            finishing.InnerHtml = designFinishing(d);

            price.InnerText = Convert.ToDecimal(d.StockProductDJ_PricingMU.HargaRupiahRound.Value).ToString("N0");


            //Stone
            fillStone();


        }
    }


    protected string designFinishing(StockProductDJ d)
    {
        string x = string.Empty;

        var rs = from p in d.StockProductDJ_Finishing select p;
        foreach (var r in rs)
        {
            if (x != "") x += "<br />";
            x += r.CharProcessFinishing.Nama + " (" + r.CharProcessFinishing.NamaKode + ")";
        }

        return x;
    }

    //Stone
    protected void fillStone()
    {
        int GT_Butir = 0;
        decimal GT_Carat = 0;

        //Stone
        fillStone1A(0, ref GT_Butir, ref GT_Carat);
        fillStone1B(1, ref GT_Butir, ref GT_Carat);
        fillStone2(2, ref GT_Butir, ref GT_Carat);
        fillStone3(3, ref GT_Butir, ref GT_Carat);
        fillStone4(4, ref GT_Butir, ref GT_Carat);
        fillStone5(5, ref GT_Butir, ref GT_Carat);

        //Grand-Total
        Sub_Total(GT_Butir, GT_Carat, true);
    }

    //Stone
    protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone1A
                 where p.IDForm == myItemID
                 orderby p.Stone1A.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            string nama = "";
            nama = r.Stone1A.Parcel101.Nama + " " + r.Stone1A.Parcel102.Nama;

            td = new TableCell();
            td.Text = TD_Nama(nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone1B
                 where p.IDForm == myItemID
                 orderby p.Stone1B.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            string nama = "";
            nama = r.Stone1B.Parcel101.Nama + " " + r.Stone1B.Parcel102.Nama;

            td = new TableCell();
            td.Text = TD_Nama(nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone2
                 where p.IDForm == myItemID
                 orderby p.Stone2.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            string nama = "";
            nama = r.Stone2.Parcel201.Nama + " " + r.Stone2.Parcel202.Nama;

            td = new TableCell();
            td.Text = TD_Nama(nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone3
                 where p.IDForm == myItemID
                 orderby p.Stone3.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            string nama = "";
            nama = r.Stone3.Parcel301.Nama + " " + r.Stone3.Parcel302.Nama;

            td = new TableCell();
            td.Text = TD_Nama(nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone4
                 where p.IDForm == myItemID
                 orderby p.Stone4.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            string nama = "";
            nama = r.Stone4.Parcel401.Nama + " " + r.Stone4.Parcel402.Nama;

            td = new TableCell();
            td.Text = TD_Nama(nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone5
                 where p.IDForm == myItemID
                 orderby p.Stone5.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            string nama = "";
            nama = r.Stone5.Parcel501.Nama + " " + r.Stone5.Parcel502.Nama;

            td = new TableCell();
            td.Text = TD_Nama(nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }

    //Custom Row
    protected void Sub_Header(int StoneIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.StoneIndex(StoneIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_Total(int TotalButir, decimal TotalCarat, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 6;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Integer(TotalButir) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number3(TotalCarat) + "</p>";
        td.CssClass = "text-right";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    //Display Column
    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        return x;
    }
    protected string TD_Nama(string Nama, string GIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

        return x;
    }
    protected string TD_Dimensi(decimal? Dimensi)
    {
        string x = string.Empty;

        x += "<p>" + Str.Number2(Dimensi) + "</p>";

        return x;
    }
    protected string TD_CaratButir(decimal? CaratButir)
    {
        string x = string.Empty;

        x += "<p class=\"text-muted\">" + Str.Number3(CaratButir) + "</p>";

        return x;
    }
    protected string TD_TotalButir(int? TotalButir, ref int ST)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-success\">" + Str.Integer(TotalButir) + "</p>";

        //Sub-Total
        if (TotalButir.HasValue) ST += TotalButir.Value;

        return x;
    }
    protected string TD_TotalCarat(decimal? TotalCarat, ref decimal ST)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-info\">" + Str.Number3(TotalCarat) + "</p>";

        //Sub-Total
        if (TotalCarat.HasValue) ST += TotalCarat.Value;

        return x;
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myItemID);
        if (d == null) DataError();
        else
        {
            



            var res = db.Resell.SingleOrDefault(p => p.ID == myIDRO);

            if (res != null)
            {
                /*TradeIn v = new TradeIn();

                //Location
                v.IDLokasi = res.IDLokasi;
                v.TipeLokasi = res.TipeLokasi;

                //Status
                v.Draft = false;
                v.DraftDate = null;

                //User-Info
                v.OperatorNama = App.UserNama;
                v.OperatorTgl = DateTime.Now;
                v.IDSalesOrder = myIDSO;
                v.IDResell = myIDRO;

                //Commit
                db.TradeIn.InsertOnSubmit(v);
                db.SubmitChanges();*/

                // Ledger 
                StockLedgerDJ l = new StockLedgerDJ();

                // Value (See Library DB - Ledger for Value and Remark)
                decimal Value = 1; // 1 : in , -1 : out
                int Remark = 4; // See Library DB - Ledger
                string Keterangan = d.Nomor; // Commonly filled by document number or id 
                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                string Lokasi = "";
                int IDBrand = 1;
                if (res.TipeLokasi == 0)
                {
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
                    IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocWarehouse WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
                }
                else if (res.TipeLokasi == 1)
                {
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
                    IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocOutlet WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
                }
                else if (res.TipeLokasi == 2)
                {
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
                    IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocExhibition WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
                }

                //Input
                l.IDProduct = d.ID;
                l.TipeLokasi = res.TipeLokasi;
                l.IDLokasi = res.IDLokasi;
                l.NamaLokasi = Lokasi;
                l.Value = Value;
                l.Remark = Remark;
                l.Keterangan = Keterangan;

                //User-Info
                l.Operator = App.UserNama;
                l.OperatorTgl = DateTime.Now;

                //Commit
                db.StockLedgerDJ.InsertOnSubmit(l);
                db.SubmitChanges();

                StockActualDJ stock = new StockActualDJ();
                stock.IDProduct = d.ID;
                stock.NamaLokasi = Lokasi;
                stock.IDBrand = IDBrand;
                stock.TipeLokasi = res.TipeLokasi;
                stock.IDLokasi = res.IDLokasi;
                stock.Substorage = 1;
                stock.Keterangan = "Barang Non JAWS";
                stock.Tgl = d.Tgl.Value;
                stock.Nomor = d.Nomor;
                stock.InHand = 1;
                stock.Operator = d.Operator;
                stock.OperatorTgl = d.OperatorTgl.Value;
                db.StockActualDJ.InsertOnSubmit(stock);
                db.SubmitChanges();

                SalesOrder n = new SalesOrder();

                //Location
                n.IDLokasi = res.IDLokasi;
                n.TipeLokasi = res.TipeLokasi;
                n.IDCustomer = res.IDCustomer;
                n.CustomerNama = res.CustomerNama;
                n.Tgl = Convert.ToDateTime(tgl.Text);

                //Status
                n.Draft = false;
                n.DraftDate = null;

                //User-Info
                n.Operator = App.UserNama;
                n.OperatorTgl = DateTime.Now;
                n.StatusVoid = false;
                n.StatusPembayaran = true;
                n.TotalBayar = Convert.ToDecimal(harga.Text);

                //Commit
                db.SalesOrder.InsertOnSubmit(n);
                db.SubmitChanges();

                var pr = db.SalesOrder.SingleOrDefault(p => p.ID == n.ID);
                pr.Nomor = Document.Generate_Nomor_SalesOrder(pr);
                db.SubmitChanges();

                SalesOrderDJ dj = new SalesOrderDJ();

                dj.IDForm = n.ID;
                dj.IDProduct = myItemID;
                dj.ModalRupiah = 0m;
                dj.TotalBayar = 0m;
                dj.TotalRupiah = 0m;
                dj.HargaRupiah = 0m;
                dj.Discount = 0m;
                dj.DiscountGift = 0m;
                dj.DiscountNominal = 0m;
                dj.DiscountProgram = 0m;
                dj.DiscountProgramNominal = 0m;
                dj.DiscountPromo = 0m;
                dj.DiscountRound = 0m;
                dj.MENominal = 0m;
                dj.MEPeriodeMax = 0;
                dj.MEPeriodeMin = 0;
                dj.MERumus = 0m;
                dj.R1_Nominal = 0m;
                dj.R1_PeriodeMax = 0;
                dj.R1_PeriodeMin = 0;
                dj.R1_Rumus = 0m;
                dj.R2_Nominal = 0m;
                dj.R2_PeriodeMax = 0;
                dj.R2_PeriodeMin = 0;
                dj.R2_Rumus = 0m;
                dj.TI1_Nominal = 0m;
                dj.TI1_PeriodeMax = 0;
                dj.TI1_PeriodeMin = 0;
                dj.TI1_Rumus = 0;
                dj.TI2_Nominal = 0m;
                dj.TI2_PeriodeMax = 0;
                dj.TI2_PeriodeMin = 0;
                dj.TI2_Rumus = 0;
                dj.TI3_Nominal = 0m;
                dj.TI3_PeriodeMax = 0;
                dj.TI3_PeriodeMin = 0;
                dj.TI3_Rumus = 0;
                dj.StatusResell = 0;
                dj.TotalBayar = Convert.ToDecimal(harga.Text);

                db.SalesOrderDJ.InsertOnSubmit(dj);
                db.SubmitChanges();

                var prdj = db.SalesOrderDJ.SingleOrDefault(p => p.ID == dj.ID);
                prdj.Invoice = Document.Generate_Nomor_SalesOrder_DJ(prdj);
                db.SubmitChanges();

                int DesignCategory = 1;
                int DesignConcept = 1;
                int DesignProcess = 1;
                int TargetAge = 1;
                int TargetGender = 1;
                DesignCategory = Db.SingleInteger("SELECT TOP 1 ID FROM CharDesignCategory WHERE NamaKode='N'", Db.Cnn_DB);
                DesignConcept = Db.SingleInteger("SELECT TOP 1 ID FROM CharDesignConcept WHERE NamaKode='N'", Db.Cnn_DB);
                DesignProcess = Db.SingleInteger("SELECT TOP 1 ID FROM CharDesignProcess WHERE NamaKode='N'", Db.Cnn_DB);
                TargetAge = Db.SingleInteger("SELECT TOP 1 ID FROM CharTargetAge WHERE NamaKode='N'", Db.Cnn_DB);
                TargetGender = Db.SingleInteger("SELECT TOP 1 ID FROM CharTargetGender WHERE NamaKode='N'", Db.Cnn_DB);

                StockProductDJ_CharDesign u = new StockProductDJ_CharDesign();

                u.ID = d.ID;
                u.DesignCategory = DesignCategory;
                u.DesignConcept = DesignConcept;
                u.DesignProcess = DesignProcess;
                u.TargetAge = TargetAge;
                u.TargetGender = TargetGender;

                db.StockProductDJ_CharDesign.InsertOnSubmit(u);
                db.SubmitChanges();

                StockProductDJ_Costing w = new StockProductDJ_Costing();

                w.ID = d.ID;
                w.MountingNO = 0m;
                w.MountingRO = 0m;
                w.OngkosLain = 0m;
                w.OngkosLainCOGS = 0m;
                w.Pembagi = 1;
                w.Setting = 0m;
                w.TotalBiaya = 0m;
                w.TotalRate = 1m;
                w.TotalRupiah = 0m;
                w.UnitPrice = 0m;
                w.Finishing = 0m;
                w.Frame = 0m;

                db.StockProductDJ_Costing.InsertOnSubmit(w);
                db.SubmitChanges();

            }

            d.Draft = false;
            d.DraftDate = null;
            db.SubmitChanges();

            Response.Redirect("Resell_ItemAddSave.aspx?id=" + myID + "&item=1&iditem=" + myItemID + "&ro=" + myIDRO + "&so=" + myIDSO);
        }

    }

    protected void saveStone(DocQCDJ d)
    {
        saveStone1A(d);
        saveStone1B(d);
        saveStone2(d);
        saveStone3(d);
        saveStone4(d);
        saveStone5(d);
    }

    protected void saveStone1A(DocQCDJ s)
    {
        TextBox txt = new TextBox();

        var rs = from p in db.StockProductDJ_Stone1A
                 where p.IDForm == myItemID
                 select p;

        foreach (var r in rs)
        {
            DocQCDJ_Stone1A j = new DocQCDJ_Stone1A();

            j.IDForm = s.ID;
            j.IDStone = r.ID;
            j.Keterangan = "";

            db.DocQCDJ_Stone1A.InsertOnSubmit(j);
            db.SubmitChanges();
        }
    }
    protected void saveStone1B(DocQCDJ s)
    {
        TextBox txt = new TextBox();
        
        var rs = from p in db.StockProductDJ_Stone1B
                 where p.IDForm == myItemID
                 select p;

        foreach (var r in rs)
        {
            DocQCDJ_Stone1B j = new DocQCDJ_Stone1B();

            j.IDForm = s.ID;
            j.IDStone = r.ID;
            j.Keterangan = "";

            db.DocQCDJ_Stone1B.InsertOnSubmit(j);
            db.SubmitChanges();
        }
    }
    protected void saveStone2(DocQCDJ s)
    {
        TextBox txt = new TextBox();
        
        var rs = from p in db.StockProductDJ_Stone2
                 where p.IDForm == myItemID
                 select p;

        foreach (var r in rs)
        {
            DocQCDJ_Stone2 j = new DocQCDJ_Stone2();

            j.IDForm = s.ID;
            j.IDStone = r.ID;
            j.Keterangan = "";

            db.DocQCDJ_Stone2.InsertOnSubmit(j);
            db.SubmitChanges();
        }
    }

    protected void saveStone3(DocQCDJ s)
    {
        TextBox txt = new TextBox();
        
        var rs = from p in db.StockProductDJ_Stone1B
                 where p.IDForm == myItemID
                 select p;

        foreach (var r in rs)
        {
            DocQCDJ_Stone3 j = new DocQCDJ_Stone3();

            j.IDForm = s.ID;
            j.IDStone = r.ID;
            j.Keterangan = "";

            db.DocQCDJ_Stone3.InsertOnSubmit(j);
            db.SubmitChanges();
        }
    }
    protected void saveStone4(DocQCDJ s)
    {
        TextBox txt = new TextBox();
        
        var rs = from p in db.StockProductDJ_Stone4
                 where p.IDForm == myItemID
                 select p;

        foreach (var r in rs)
        {
            DocQCDJ_Stone4 j = new DocQCDJ_Stone4();

            j.IDForm = s.ID;
            j.IDStone = r.ID;
            j.Keterangan = "";

            db.DocQCDJ_Stone4.InsertOnSubmit(j);
            db.SubmitChanges();
        }
    }
    protected void saveStone5(DocQCDJ s)
    {
        TextBox txt = new TextBox();
        
        var rs = from p in db.StockProductDJ_Stone5
                 where p.IDForm == myItemID
                 select p;

        foreach (var r in rs)
        {
            DocQCDJ_Stone5 j = new DocQCDJ_Stone5();

            j.IDForm = s.ID;
            j.IDStone = r.ID;
            j.Keterangan = "";

            db.DocQCDJ_Stone5.InsertOnSubmit(j);
            db.SubmitChanges();
        }
    }
}