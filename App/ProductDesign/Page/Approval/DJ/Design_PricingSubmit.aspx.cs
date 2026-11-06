using System;
using System.Linq;
using System.Web.UI.WebControls;
using Library.SAP_ZMMFM_NEWDESIGN_INTRFC;

public partial class Page_Approval_Design_PricingSubmit : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            fillForm();
        }
    }

    //Form
    protected void fillForm()
    {
        var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/Design_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            fillProduct(d);
            fillStone();
            fillBiaya(d);
            fillFinishing();
            fillMU(d);
            fillSegmen(d);
        }
    }

    protected void fillProduct(ProductDesign ms)
    {
        var d = ms.ProductDesign_PricingProduct;
        if (d != null)
        {
            //goldrate.InnerHtml = Str.Number2(d.GoldRate);
            netto.InnerHtml = Str.Number2(d.Netto);
            kadar.InnerHtml = Str.Number2(d.Kadar);


            //efektifgold.InnerHtml = Str.Tgl(d.EfektifGold);
        }
    }
    protected void fillBiaya(ProductDesign ms)
    {
        var d = ms.ProductDesign_PricingBiaya;
        if (d != null)
        {
            brand1.InnerHtml = d.NamaBrand;
            item1.InnerHtml = d.NamaItem;
            cons1.InnerHtml = d.NamaCons;


        }
    }
    protected void fillMU(ProductDesign ms)
    {
        var d = ms.ProductDesign_PricingMU;
        if (d != null)
        {
            cat1.InnerHtml = d.NamaCategory;
            lvl1.InnerHtml = d.NamaLevel;
            dist1.InnerHtml = d.NamaDist;


            hargaidr.InnerHtml = Str.Number0(d.HargaRupiahRound);
            hargausd.InnerHtml = Str.Number2(d.HargaUSD);
        }
    }
    protected void fillSegmen(ProductDesign ms)
    {
        var d = ms.ProductDesign_PricingSegmen;
        if (d != null)
        {
            cat2.InnerHtml = d.NamaCategory;
            lvl2.InnerHtml = d.NamaLevel;
            dist2.InnerHtml = d.NamaDist;



            segmen.InnerHtml = d.CharProductSegmen.Nama;
        }
    }

    //Stone
    protected void fillStone()
    {
        int GT_Butir = 0;
        decimal GT_Harga = 0;

        //Stone
        fillStone1A(0, ref GT_Butir, ref GT_Harga);
        fillStone1B(1, ref GT_Butir, ref GT_Harga);
        fillStone2(2, ref GT_Butir, ref GT_Harga);
        fillStone3(3, ref GT_Butir, ref GT_Harga);
        fillStone4(4, ref GT_Butir, ref GT_Harga);
        fillStone5(5, ref GT_Butir, ref GT_Harga);

        //Grand-Total
        //Sub_Total(GT_Butir, GT_Harga, true);
    }

    //Stone
    protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.ProductDesign_Stone1A
                 where p.IDForm == myID
                 orderby p.Stone1A.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;

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
            ST_Harga += r.HargaTotal.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;

            //Sub_Total(ST_Butir, ST_Harga, false);
        }
    }
    protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.ProductDesign_Stone1B
                 where p.IDForm == myID
                 orderby p.Stone1B.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;

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

            string Status = "";
            if (r.StatusTerpasang == 1)
                Status = "<br /><span class=\"text-sm text-silver\">Terpasang</span>";
            else
                Status = "<br /><span class=\"text-sm text-silver\">Tidak Terpasang</span>";

            td = new TableCell();
            td.Text = r.Stone1B.Nama + Status;
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
            ST_Harga += r.HargaTotal.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;

            //Sub_Total(ST_Butir, ST_Harga, false);
        }
    }
    protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.ProductDesign_Stone2
                 where p.IDForm == myID
                 orderby p.Stone2.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;

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
            ST_Harga += r.HargaTotal.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;

            //Sub_Total(ST_Butir, ST_Harga, false);
        }
    }
    protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.ProductDesign_Stone3
                 where p.IDForm == myID
                 orderby p.Stone3.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;

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
            ST_Harga += r.HargaTotal.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;

            //Sub_Total(ST_Butir, ST_Harga, false);
        }
    }
    protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.ProductDesign_Stone4
                 where p.IDForm == myID
                 orderby p.Stone4.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;

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
            ST_Harga += r.HargaTotal.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;

            //Sub_Total(ST_Butir, ST_Harga, false);
        }
    }
    protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.ProductDesign_Stone5
                 where p.IDForm == myID
                 orderby p.Stone5.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;

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
            ST_Harga += r.HargaTotal.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;

            //Sub_Total(ST_Butir, ST_Harga, false);
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
    protected void Sub_Total(int TotalButir, decimal TotalHarga, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 3;
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void fillFinishing()
    {
        //SELECT
        var rs = from p in db.ProductDesign_Finishing
                 where p.IDForm == myID
                 orderby p.CharProcessFinishing.SN
                 select p;

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
            td.Text = r.CharProcessFinishing.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.Biaya);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            //tbff.Rows.Add(tr);
        }
    }


    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            // Generate Document Number --> Sales Order
            if (d.Nomor == null || d.Nomor == "")
            {
                d.Nomor = Generate_Nomor(d);
                db.SubmitChanges();
            }

            ////Uncomment for hit wsdl Debug and get result
            //var result = new SAP.ValidationResponse();
            //ZmmfmNewdesignIntrfcResponse _output = new ZmmfmNewdesignIntrfcResponse();
            //result = SAP.WsdlNewDesignIntrfcResponse(DocumentNumber, myID, out _output);
            //string msg2 = "Status : " + result.Success.ToString() + " Info : " + result.Info.ToString();
            //lit_msg.Text = msg2;
            //return;

            //get sap integration value
            bool SAPModuleDesignActivate = false;

            SAPModuleDesignActivate = true; // 2019-02-11

            string status_SAP = "";

            try
            {
                if (SAP.xActivate || SAPModuleDesignActivate)
                {
                    lit_test_msg.Text = "";
                    lit_test_msg.Text += "d.Nomor = " + d.Nomor + "<br />";
                    lit_test_msg.Text += "myID = " + myID + "<br />";
                    lit_test_msg.Text += "d.DataHuman.CompanyBrand.ID = " + d.DataHuman.CompanyBrand.ID + "<br />";
                    lit_test_msg.Text += "d.DataHuman.CompanyBrand.Nama = " + d.DataHuman.CompanyBrand.Nama + "<br />";


                    var validationResponse = new SAP.ValidationResponse();
                    validationResponse = SAP.GetResponseWsdlNewDesign(lit_test_msg, d.Nomor, myID, d.DataHuman.CompanyBrand.ID, d.DataHuman.CompanyBrand.Nama);

                    //if result failed insert to product design invalid and get result
                    if (!validationResponse.Success)
                    {
                        status_SAP = "Status : " + validationResponse.Success.ToString() + " Info : " + validationResponse.Info.ToString();

                        SAP.InsertProductDesignInvalid(myID, status_SAP);

                        //return;
                    }

                }
            }
            catch (Exception ex)
            {
                status_SAP = "Terjadi kesalahan teknis. Silahkan hubungi IT Administrator. (Error message : "
                    + ex.Message
                    //+ ex.StackTrace 
                    //+ ex.InnerException 
                    + ")";
            }

            lit_test_msg.Text = "";

            if (status_SAP == "")
            {
                //User-Info
                d.DesignKode = d.Nomor;
                d.Approval = App.UserNama;
                d.ApprovalTgl = DateTime.Now;

                //Commit
                db.SubmitChanges();

                Response.Redirect("/ProductDesign/Page/Report/DJ/Design.aspx?refreshPage=1&kw=" + d.Nomor);

            }
            else
            {
                lit_msg.Text = "<div class=\"alert alert-danger\">SAP Post New Design : " + status_SAP + " </div>";
            }

        }
    }

    protected string Generate_Nomor(ProductDesign d)
    {
        string x = string.Empty;

        int Tahun = d.Tgl.Year;
        int Bulan = d.Tgl.Month;

        /*int Nomor = db.ProductDesign.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
        Nomor = Nomor + 1;*/

        int Nomor = 1;

        try
        {
            var s = (from p in db.ProductDesign
                     where p.Tgl.Year == Tahun && p.Tgl.Month == Bulan
                     && p.Nomor != null
                     orderby (p.Nomor.Substring(12, 4)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(12, 4)) + 1;
        }
        catch (Exception ex) { }

        return "DSG/" + d.DataHuman.CompanyBrand.NamaKode + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
    }
}