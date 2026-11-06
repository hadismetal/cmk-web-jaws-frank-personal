using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_DJ_PricingSubmit : System.Web.UI.Page
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
    decimal GT_HargaM = 0;

    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            fillForm();
            fillAlasan(ddalasan);
        }
    }

    protected void fillAlasan(DropDownList ddl)
    {
        var rs = from p in db.MasterMeltingReason orderby p.Nama select p;
        foreach (var r in rs)
        {
            string t = r.Kode + " - " + r.Nama;
            string v = r.ID.ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }
    //Form
    protected void fillForm()
    {
        petugaslebur.Text = App.UserNama;

        var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml = d.Nomor;

            //Trans. Data
            protonomor.InnerHtml = d.ProtoNomor;
            productordernomor.InnerHtml = d.ProductOrderNomor;
            suppliernama.InnerHtml = d.SupplierNama;
            suppliernomor.InnerHtml = d.SupplierNomor;
            requestoutletnama.InnerHtml = d.RequestOutletNama;
            requestcustomernama.InnerHtml = d.RequestCustomerNama;
            humanstocknama.InnerHtml = d.HumanStockNama;
            expiredproduct.InnerHtml = Str.Tgl(d.ExpiredProduct);
            expiredrequest.InnerHtml = Str.Tgl(d.ExpiredRequest);

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

            //Design Char.
            var design = d.StockProductDJ_CharDesign;
            if (design == null) save.Disabled = true;
            else
            {
                designcategory.InnerHtml = design.CharDesignCategory.Nama + " (" + design.CharDesignCategory.NamaKode + ")";
                designconcept.InnerHtml = design.CharDesignConcept.Nama + " (" + design.CharDesignConcept.NamaKode + ")";
                designprocess.InnerHtml = design.CharDesignProcess.Nama + " (" + design.CharDesignProcess.NamaKode + ")";
                targetage.InnerHtml = design.CharTargetAge.Nama + " (" + design.CharTargetAge.NamaKode + ")";
                targetgender.InnerHtml = design.CharTargetGender.Nama + " (" + design.CharTargetGender.NamaKode + ")";

                naming.InnerHtml = design.Naming;
                designket.InnerHtml = designKet(design);
                grafir.InnerHtml = design.Grafir;
                finishing.InnerHtml = designFinishing(d);
            }

            //Picture
            Upload.Image(imgpicture, d.ImgPicture);

            fillStone();
            fillProduct(d);

            fillMU(d);
        }
    }

    protected void fillProduct(StockProductDJ ms)
    {
        var d = ms.StockProductDJ_PricingProduct;
        var x = ms.StockProductDJ_CostingProduct;
        if (d != null && x != null)
        {
            netto.InnerHtml = Str.Number2(x.Netto);
            //kadar.InnerHtml = Str.Number2(Math.Ceiling(x.Kadar));
            // kadar.Text = Str.Number2(x.Kadar);
            kadar.Text = "75,00";
        }
    }
    protected void fillMU(StockProductDJ ms)
    {
        decimal AllM = 0m;

        var d = ms.StockProductDJ_PricingMU;
        if (d != null)
        {

            hargaidr.InnerHtml = Str.Number0(d.HargaRupiahRound);
        }
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
                 where p.IDForm == myID
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
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone1A.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;
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
                 where p.IDForm == myID
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
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone1B.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;
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
                 where p.IDForm == myID
                 orderby p.Stone2.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0m;

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
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone2.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;
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
                 where p.IDForm == myID
                 orderby p.Stone3.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0m;

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
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone3.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;
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
                 where p.IDForm == myID
                 orderby p.Stone4.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0m;

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
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone4.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;
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
                 where p.IDForm == myID
                 orderby p.Stone5.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0m;

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
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone5.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Carat += r.TotalCarat;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
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
        td.ColumnSpan = 3;
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Integer(TotalButir);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number3(TotalCarat);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    //Design Info
    protected string designKet(StockProductDJ_CharDesign d)
    {
        string x = string.Empty;

        if (d.SetItem)
        {
            if (x != "") x += ", ";
            x += "Set Item";
        }
        if (d.Special)
        {
            if (x != "") x += ", ";
            x += "Special Design";
        }

        return x;
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

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var cek = db.StockRetireDJ.OrderByDescending(p=> p.ID).FirstOrDefault(p => p.IDProduct == myID);

        if (cek == null)
        {
            if (kadar.Text != null && kadar.Text != "")
            {
                insert();
            }
            else
            {
                valkadar.Visible = true;
            }
        }
        else
        {
            Response.Write("<script>alert('no PLU sudah terdaftar!');</script>");
            save.Visible = false;
            bback.Visible = true;
            try
            {
                if (cek.Status == 2)
                {
                    insert();
                }
                else if (cek.Approval == null && cek.Draft == true)
                {
                    update(cek.ID);
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('no PLU sudah terdaftar!');</script>");
            }
        }
    }

    protected void insert()
    {
        var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            StockRetireDJ x = new StockRetireDJ();

            x.Nama = petugaslebur.Text;
            string nodokumen = G_Nomor(d);
            x.Nomor = nodokumen;
            x.IDProduct = d.ID;
            x.Keterangan = keterangan.Text;
            x.Reason = Convert.ToInt32(ddalasan.SelectedValue);
            x.Alasan = ddalasan.SelectedItem.Text;
            x.Tgl = DateTime.Now;
            x.Operator = App.UserNama;
            x.OperatorTgl = DateTime.Now;
            x.Draft = false;
            x.DraftDate = null;
            x.Status = 3; // after regis status must be 3
            x.KadarTukaran = Convert.ToDecimal(kadar.Text);
            db.StockRetireDJ.InsertOnSubmit(x);
            db.SubmitChanges();

            for (int i = 0; i < cbl_Product.Items.Count; i++)
            {
                if (cbl_Product.Items[i].Selected)
                {

                    Db.Execute(@"INSERT INTO StockRetireDJLD 
                                SELECT " + x.ID + @", A.ID, B.ID, B.Nama, A.Nomor, A.CaratButir, A.GIA from StockProductLD_Stone1B A
                                JOIN Stone1B B ON A.IDStone = B.ID
                                WHERE A.ID = " + cbl_Product.Items[i].Value , Db.Cnn_DB);

                    var ld = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == Convert.ToInt32(cbl_Product.Items[i].Value));

                    ld.ReturnProduct = true;
                    ld.StockActualLD_Stone1B.InHand = 0;

                    db.SubmitChanges();

                    StockLedgerLD ledger = new StockLedgerLD();
                    ledger.IDProduct = ld.ID;
                    ledger.IDLokasi = ld.StockActualLD_Stone1B.IDLokasi;
                    ledger.TipeLokasi = ld.StockActualLD_Stone1B.TipeLokasi;
                    ledger.Value = 0;
                    ledger.Keterangan = "Register " + x.Nomor;
                    ledger.NamaLokasi = ld.StockActualLD_Stone1B.NamaLokasi;
                    ledger.Remark = 20;
                    ledger.Operator = App.UserNama;
                    ledger.OperatorTgl = DateTime.Now;

                    db.StockLedgerLD.InsertOnSubmit(ledger);
                    db.SubmitChanges();
                }
            }

            StockLedgerDJ l = new StockLedgerDJ();

            // Value (See Library DB - Ledger for Value and Remark)
            decimal Value = 0; // 1 : in , -1 : out , 0 for register retire
            int Remark = 20; // See Library DB - Ledger
            string Keterangan = x.Nomor; // Commonly filled by document number or id 
            if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

            string Lokasi = "";
            if (d.StockActualDJ.TipeLokasi == 1)
                Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockActualDJ.IDLokasi, Db.Cnn_DB);
            else if (d.StockActualDJ.TipeLokasi == 2)
                Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockActualDJ.IDLokasi, Db.Cnn_DB);

            //Input
            l.IDProduct = d.ID;
            l.TipeLokasi = d.StockActualDJ.TipeLokasi;
            l.IDLokasi = d.StockActualDJ.IDLokasi;
            l.NamaLokasi = d.StockActualDJ.NamaLokasi;
            l.Value = Value;
            l.Remark = Remark;
            l.Keterangan = "Register " + x.Nomor;

            //User-Info
            l.Operator = App.UserNama;
            l.OperatorTgl = DateTime.Now;

            //Commit
            db.StockLedgerDJ.InsertOnSubmit(l);
            db.SubmitChanges();


            //Redirect
            //App.ReloadClose(this);
            Response.Redirect("SPL_DJ.aspx?id=" + x.ID + "&status=1");
        }
    }

    protected void update(int id)
    {
        string query = string.Empty;
        int IDHeader = Convert.ToInt32(Request.QueryString["idheader"]);
        if (IDHeader == 0)
        {
            query = @"declare @ID table( ID int )
                    INSERT INTO StockRetireDJ_Doc (Keterangan, Tgl, Operator, OperatorTgl)
                    OUTPUT inserted.ID
                    INTO @ID
                    VALUES ('', GETDATE(), '" + App.UserNama + @"', GETDATE())
                    SELECT * FROM @ID";

            IDHeader = Db.SingleInteger(query, Db.Cnn_DB);
        }

        query = @"Update StockRetireDJ
            SET IDForm = " + IDHeader.ToString() + @"
            WHERE ID = " + id;

        Db.Execute(query, Db.Cnn_DB);

        Response.Redirect("DJ.aspx?idheader=" + IDHeader);
    }
    protected string Generate_Nomor(StockRetireDJ j)
    {
        string x = string.Empty;

        int Tahun = DateTime.Now.Year;
        int Bulan = DateTime.Now.Month;

        //int Nomor = db.SalesOrder.Count(p => p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan );
        //Nomor = Nomor + 1;

        int Nomor = 1;

        try
        {
            var s = (from p in db.StockRetireDJ
                     where p.OperatorTgl.Value.Year == Tahun && p.OperatorTgl.Value.Month == Bulan
                     orderby (p.Nomor.Substring(14, 4)) descending
                     select p).FirstOrDefault();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(14, 4)) + 1;
        }
        catch (Exception ex) { }

        return "SPL/"+ j.StockProductDJ.StockActualDJ.CompanyBrand.NamaKode +"/D/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
    }

    protected string G_Nomor(StockProductDJ j)
    {
        string x = string.Empty;

        int Tahun = DateTime.Now.Year;
        int Bulan = DateTime.Now.Month;

        //int Nomor = db.SalesOrder.Count(p => p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan );
        //Nomor = Nomor + 1;
     
        int Nomor = Db.SingleInteger("SELECT COUNT(*) FROM StockRetireDJ  WHERE SUBSTRING(Nomor,9,2) = CONVERT(VARCHAR(2), GETDATE(), 2) AND SUBSTRING(Nomor,12,2) = CONVERT(VARCHAR(2), GETDATE(), 1)", Db.Cnn_DB);

        if (Nomor == 0)
        {
            Nomor = 1;
        }
        else
        {
            Nomor = Nomor + 1;
        }
        //try
        //{
        //    var s = (from p in db.StockRetireDJ
        //             where p.OperatorTgl.Value.Year == Tahun && p.OperatorTgl.Value.Month == Bulan
        //             orderby (p.Nomor.Substring(15, 4)) descending
        //             select p).FirstOrDefault();

        //    if (s.Nomor != null)
        //        Nomor = Convert.ToInt32(s.Nomor.Substring(15, 4)) + 1;
        //}
        //catch (Exception ex) { }



        return "SPL/" + j.StockActualDJ.CompanyBrand.NamaKode + "/D/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
    }

    protected void bback_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("DJ.aspx");
    }

    protected void addLD_ServerClick(object sender, EventArgs e)
    {
        Literal plit_Message = lit_Message;
        TextBox pinputproduct = inputproduct;
        CheckBoxList pcbl_Product = cbl_Product;
        AddLD(plit_Message, pinputproduct, pcbl_Product);
        fillStone();
    }

    private void AddLD(Literal lit_Message, TextBox inputproduct, CheckBoxList cbl_Product)
    {
        using (Map_DB d = new Map_DB())
        {

        /// Master Brand
       

        lit_Message.Text = "";
        string input = inputproduct.Text.Trim();

        try
        {
            int exist = 0;
            exist = Db.SingleInteger("SELECT COUNT(A.ID) FROM StockRetireDJLD A JOIN StockRetireDJ B ON A.IDForm = B.ID WHERE B.Status != 2 AND A.Nomor = '" + input + "'", Db.Cnn_DB);
            var rs = d.StockActualLD_Stone1B.SingleOrDefault(p => p.StockProductLD_Stone1B.Nomor == input
                && p.InHand >= 1 && p.StockProductLD_Stone1B.ReturnProduct == false && exist == 0
                && !(from z in d.StockReturnLD_Product
                     where z.StockReturnLD.Approval != null
                     select z.IDProduct).Contains(p.IDProduct)
                ); ;

            if (rs != null)
            {
                /// Check Apakah produk sudah pernah diinput (tidak bisa input barang yang sama 2x pada dokumen yang sama)
                string err_msg = validLD(input, cbl_Product);

                var djld = d.StockProductDJLD.Where(p => p.IDProductDJ == myID && p.IDProductLD == rs.IDProduct);

                if(djld.Count() == 0 && err_msg == "")
                {
                    string pluDJ = d.StockProductDJ.SingleOrDefault(p => p.ID == myID).Nomor;
                    err_msg = "LD " + rs.Nomor + " bukan merupakan kombinasi dari DJ " + pluDJ ;
                }

                if (err_msg == "")
                {
                    lit_Message.Text = ""; /// tidak ada error message

                    string v = rs.StockProductLD_Stone1B.ID.ToString();
                    string t = ProductList_LD(
                            rs.StockProductLD_Stone1B.Nomor,
                            rs.StockProductLD_Stone1B.ImgPicture,
                            DL.SubStorage(rs.Substorage),
                            Str.Number3(rs.StockProductLD_Stone1B.TotalCarat)
                            );

                    int jlh = cbl_Product.Items.Count;
                    string[] item = new string[1000];
                    string[] itemvalue = new string[1000];

                    for (int i = 0; i < jlh; i++)
                    {
                        item[i] = cbl_Product.Items[i].Text;
                        itemvalue[i] = cbl_Product.Items[i].Value;
                    }

                    cbl_Product.Items.Clear();

                    cbl_Product.Items.Add(new ListItem(t, v));
                    cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                    for (int i = 0; i < jlh; i++)
                    {
                        cbl_Product.Items.Add(new ListItem(item[i], itemvalue[i]));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                    }

                    for (int i = 0; i < jlh; i++)
                    {
                        cbl_Product.Items[i].Selected = true;
                    }

                    //upl.Update();

                    inputproduct.Text = "";
                    inputproduct.Focus();
                }
                else
                {
                    // error message
                    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                }

            }
            else
            {
                lit_Message.Text = "<span style='color:red;'>Barang sudah terinput / tidak terdapat di cabang ini</span>";
            }

        }
        catch (Exception ex)
        {
            lit_Message.Text = "<span style='color:red;'>Barang sudah ter-register, cek menu Approval</span>";
        }
        }
    }

    private string validLD(string input, CheckBoxList cbl)
    {
        using (Map_DB d = new Map_DB())
        {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.StockProductLD_Stone1B.SingleOrDefault(p => p.Nomor == input);

        if (rs1 == null) { message = "Invalid Product"; }
        else
        {
            // 2. Check Apakah Product Item sudah pernah terinput
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + input + " sudah terinput"; }
            }
        }

        // 3. Check Apakah product valid status substoragenya untuk dikirim ke gudang

        return message;
        }
    }

    public static string ProductList_LD(string Nomor, string PathImage, string Status, string Carat)
    {
        string x;
        x = "<div style='width: 1100px; margin-left:20px;'>"
            + "<div style='padding:5px; display: table-cell;'>"
            + "<div style='float: left; width: 200px;'>" + Nomor + "</div>"
            + "<div style='float: left; width: 120px;'>" + Status + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + 1 + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Carat + "<div style='clear:both;'></div></div>"
            + "</div>"
            + "</div>"
            + "<div style='clear: both; font-size:1px; height: 1px;'>&nbsp;</div>"
            ;
        return x;
    }
}