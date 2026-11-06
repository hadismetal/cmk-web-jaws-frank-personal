using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

public partial class Page_Approval_SPK : System.Web.UI.Page
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
    App price = new App();

    string Fail_Reason = "";
    protected void PricingError(string sebab, int id = 0)
    {
        //Response.Redirect("SPK_Form.aspx?id=" + myID + "&prcerror=" + sebab);
        Fail_Reason = "<p class=\"text-lg text-red\">" + sebab + "</p>";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            //Generate
            fillTable(sender, e);
        }
        createRows();
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

    }

    private void createRows()
    {
        //SELECT
        var rs = from p in db.ProductOrder
                 where p.StatusPenerimaan == 0 && p.Draft == false && p.Approval == null
                 orderby p.Operator, p.KodeKaretLama, p.OperatorTgl
                 select p;

        //NO-RESULT
        zero.Visible = rs.Count() == 0;
        form_area.Visible = !zero.Visible;

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            //Sub-Header

            TableRow tr = new TableRow();
            TableCell td;

            bool ck = check_app(r.ID);

            td = new TableCell();
            td.Text = TD_HyperLink(r);
            td.Style.Add("vertical-align", "top");
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            CheckBox cb = new CheckBox();
            cb.ID = "cb_po" + index.ToString();
            cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            if (Fail_Reason != "") { cb.Enabled = false; }


            HiddenField hf = new HiddenField();
            hf.ID = "hf" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(cb);
            td.Controls.Add(hf);
            td.Style.Add("vertical-align", "top");
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = index++.ToString() + ".";
            td.Style.Add("vertical-align", "top");
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            //td.Text = "<p><img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" /></p>";
            td.Text = Img.ResizeImageToBase64Html(r.ImgPicture, 30, 30);
            td.Style.Add("vertical-align", "top");
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Operator(r);
            td.Style.Add("vertical-align", "top");
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Data(r);
            td.Style.Add("vertical-align", "top");
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Brand(r);
            td.Style.Add("vertical-align", "top");
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            if (r.ProductOrder_CharProduct != null)
            {
                td = new TableCell();
                td.Text = TD_Product01(r.ProductOrder_CharProduct);
                td.Style.Add("vertical-align", "top");
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Product02(r.ProductOrder_CharProduct);
                td.Style.Add("vertical-align", "top");
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Product04(r);
                td.Style.Add("vertical-align", "top");
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
            }
         
            td = new TableCell();
            //td.Text = (Fail_Reason == "") ? "ok" : "cek ulang";
            td.Text = (Fail_Reason == "") 
                ? "<a class='glyphicon glyphicon-ok-circle' style='color:green; font-size:25px;'> </a>"
                : "<a class='glyphicon glyphicon-remove-circle' style='color:red; font-size:25px; align:center;'> </a>"
                ;
            td.Style.Add("vertical-align", "top");
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Fail_Reason;
            td.Style.Add("vertical-align", "top");
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }
    protected string TD_HyperLink(ProductOrder r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"SPK_Pricing.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }
    protected string TD_Operator(ProductOrder r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

        return x;
    }
    protected string TD_Data(ProductOrder r)
    {
        string x = string.Empty;

        if (r.ProductDesign != null)
            x += "<p class=\"text-sm\">" + r.ProductDesign.Nomor + "</p>";
        if (r.KodeKaret != "" && r.KodeKaret != null && r.IDKodeKaret != null)
            x += "<p class=\"text-sm\">" + r.KodeKaret + "</p>";
        if (r.KodeKaretLama != "" && r.KodeKaret != "" && r.KodeKaret != null && r.IDKodeKaret != null)
            x += "<p class=\"text-muted\">" + r.KodeKaretLama 
                + ((r.KodeKaretRepeat != null && r.KodeKaretRepeat != 0) ? ("/" + r.KodeKaretRepeat.ToString().PadLeft(4,'0')) : "")
                +"</p>"
                ;

        return x;
    }
    protected string TD_Brand(ProductOrder r)
    {
        string x = string.Empty;

        if (r.ProductDesign != null)
            x += "<p class=\"text-sm\">" + r.CompanyBrand.Nama + "</p>";

        return x;
    }
    protected string TD_Product01(ProductOrder_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(ProductOrder_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductCategory.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(ProductOrder_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductLevel.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Product04(ProductOrder r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductSegmen.Nama;
        x += "</p>";

        return x;
    }

    protected bool check_app(int idprod)
    {
        Fail_Reason = "";
        try
        {
            var d = db.ProductOrder.SingleOrDefault(p => p.ID == idprod && p.Draft == false && p.Approval == null);
            if (d == null) App.DataNull(this);
            else
            {
                //Delete
                deleteProcess(d);

                string testDebug = "";

                //Process
                var product = d.ProductOrder_CharProduct;
                if (product == null) { PricingError("Product Char.", d.ID); }
                else
                {
                    saveStone(d);
                    saveFinishing(d, product);

                    saveProduct(d, product);
                    saveBiaya(d, product);

                    testDebug = saveMU(d, product);
                    saveSegmen(d, product);
                    saveTotalStone(d);
                }
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
        
    }
    protected void Approve_Click(object sender, EventArgs e)
    {
        //Response.Redirect(tb.Rows.Count.ToString());

        string Bulan = DateTime.Now.ToString("MM");
        string Tahun = DateTime.Now.ToString("yy");

        int index = 1;

        var rs = from p in db.ProductOrder
                 where p.StatusPenerimaan == 0 && p.Draft == false && p.Approval == null
                 orderby p.ID
                 select p;

        CheckBox cb = new CheckBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            if (tb.Rows.Count >= 1)
            {
                if (index < tb.Rows.Count)
                {
                    hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                    cb = (CheckBox)row.FindControl("cb_po" + index.ToString());

                    if (cb.Checked == true)
                    {
                        int LastKodeKaret = 1;

                        int LastSPK = 1;
                        int Counter = 1;
                        int LastSPKID = 1;
                        int lastadd = 1;
                        string KodeKaret = "";

                        var d = db.ProductOrder.SingleOrDefault(p => p.ID == Convert.ToInt32(hf) && p.Draft == false && p.StatusPenerimaan < 2 && p.Approval == null);
                        if (d == null) App.DataNull(this);
                        else
                        {
                            //Process
                            var product = d.ProductOrder_CharProduct;
                            if (product == null) PricingError("Product Char.",d.ID);
                            else
                            {
                                //edit suwandi
                                for (int i = 0; i < d.JlhCopy; i++)
                                {
                                    try
                                    {
                                        LastSPK = Db.SingleInteger("SELECT COUNT(*) FROM ProductOrder WHERE SUBSTRING(NoSPK,5,2) = CONVERT(VARCHAR(2), GETDATE(), 2) AND SUBSTRING(NoSPK,8,2) = CONVERT(VARCHAR(2), GETDATE(), 1)", Db.Cnn_DB);
                                        lastadd = Db.SingleInteger("SELECT COUNT(*) FROM ProductOrder WHERE SUBSTRING(NoSPK,7,2) = CONVERT(VARCHAR(2), GETDATE(), 2) AND SUBSTRING(NoSPK,10,2) = CONVERT(VARCHAR(2), GETDATE(), 1)", Db.Cnn_DB);
                                        LastSPK = LastSPK + lastadd;
                                    }
                                    catch (Exception)
                                    {
                                        LastSPK = 0;
                                    }

                                    string SPKNumber = Str.KodeFormat((LastSPK + 1), 4);
                                    string KodeBrand, KodeItem, KodeProductCategory, KodeSet;
                                    if (d.IDProductDesign != null)
                                        KodeBrand = d.ProductDesign.DataHuman.CompanyBrand.NamaKode;
                                    else
                                        KodeBrand = d.CompanyBrand.NamaKode;

                                    KodeItem = d.ProductOrder_CharProduct.CharProductItem.NamaKode;
                                    KodeProductCategory = d.ProductOrder_CharProduct.CharProductCategory.KodeProto.Substring(0, 1);
                                    KodeSet = (d.ProductOrder_CharDesign.SetItem == true) ? "S" : "T";

                                    int SeqProductCategory = 1;
                                    try
                                    {
                                        SeqProductCategory = Db.SingleInteger("SELECT COUNT(*) FROM KodeKaretDJ_CharProduct WHERE ProductCategory=" + d.ProductOrder_CharProduct.ProductCategory + " AND ProductItem=" + d.ProductOrder_CharProduct.ProductItem, Db.Cnn_DB);
                                        SeqProductCategory = SeqProductCategory + 1;
                                    }
                                    catch (Exception)
                                    {
                                        SeqProductCategory = 1;
                                    }

                                    if (Counter == 1 && d.IDKodeKaret == null)
                                    {
                                        KodeKaretDJ kd = new KodeKaretDJ();

                                        KodeKaret = KodeBrand + KodeItem + KodeProductCategory + KodeSet + Tahun + Str.KodeFormat(SeqProductCategory, 4);

                                        try
                                        {
                                            // revisi coding untuk antisipasi pengkodean kode karet jaws double untuk Kode Karet Lama yang berbeda
                                            // 2016-09-22, 23:26
                                            var check_kd = from p in db.KodeKaretDJ where p.Nomor == KodeKaret select p;
                                            if (check_kd.Count() == 1) { SeqProductCategory = SeqProductCategory + 1; }

                                            KodeKaret = KodeBrand + KodeItem + KodeProductCategory + KodeSet + Tahun + Str.KodeFormat(SeqProductCategory, 4);
                                        }
                                        catch (Exception) { }

                                        kd.Nomor = KodeKaret;
                                        kd.IDProductOrder = d.ID;
                                        kd.Tgl = d.Tgl;
                                        kd.Keterangan = d.Keterangan;
                                        kd.Operator = d.Operator;
                                        kd.OperatorTgl = d.OperatorTgl;
                                        kd.ImgPicture = d.ImgPicture;
                                        kd.IDBrand = d.ProductDesign.DataHuman.IDBrand;
                                        kd.IDSegmen = d.ProductDesign.ProductDesign_PricingSegmen.ProductSegmen;
                                        kd.IDDesigner = d.ProductDesign.DataHuman.ID;
                                        kd.KodeKaretLama = d.KodeKaretLama;

                                        db.KodeKaretDJ.InsertOnSubmit(kd);
                                        db.SubmitChanges();

                                        try
                                        {
                                            LastKodeKaret = Db.SingleInteger("SELECT TOP 1 ID FROM KodeKaretDJ ORDER BY ID DESC", Db.Cnn_DB);
                                        }
                                        catch (Exception)
                                        {
                                            LastKodeKaret = 1;
                                        }

                                        KodeKaretDJ_CharDesign dsg = new KodeKaretDJ_CharDesign();
                                        dsg.ID = LastKodeKaret;
                                        dsg.DesignCategory = d.ProductOrder_CharDesign.DesignCategory;
                                        dsg.DesignConcept = d.ProductOrder_CharDesign.DesignConcept;
                                        dsg.DesignProcess = d.ProductOrder_CharDesign.DesignProcess;
                                        dsg.TargetAge = d.ProductOrder_CharDesign.TargetAge;
                                        dsg.TargetGender = d.ProductOrder_CharDesign.TargetGender;
                                        dsg.Naming = d.ProductOrder_CharDesign.Naming;
                                        dsg.SetItem = d.ProductOrder_CharDesign.SetItem;
                                        dsg.Special = d.ProductOrder_CharDesign.Special;

                                        db.KodeKaretDJ_CharDesign.InsertOnSubmit(dsg);
                                        db.SubmitChanges();

                                        KodeKaretDJ_CharProduct prod = new KodeKaretDJ_CharProduct();
                                        prod.ID = LastKodeKaret;
                                        prod.FrameColor = d.ProductOrder_CharProduct.FrameColor;
                                        prod.FrameFinishing = d.ProductOrder_CharProduct.FrameFinishing;
                                        prod.FrameMaterial = d.ProductOrder_CharProduct.FrameMaterial;
                                        prod.ProcessCons = d.ProductOrder_CharProduct.ProcessCons;
                                        prod.ProductCategory = d.ProductOrder_CharProduct.ProductCategory;
                                        prod.ProductItem = d.ProductOrder_CharProduct.ProductItem;
                                        prod.ProductLevel = d.ProductOrder_CharProduct.ProductLevel;
                                        prod.StoneDist = d.ProductOrder_CharProduct.StoneDist;
                                        prod.GrossWeight = d.ProductOrder_CharProduct.GrossWeight;
                                        prod.StoneCarat = d.ProductOrder_CharProduct.StoneCarat;
                                        prod.StoneWeight = d.ProductOrder_CharProduct.StoneWeight;
                                        prod.NettoWeight = d.ProductOrder_CharProduct.NettoWeight;
                                        prod.KadarLogam = d.ProductOrder_CharProduct.KadarLogam;
                                        prod.KadarTukaran = d.ProductOrder_CharProduct.KadarTukaran;
                                        prod.DimensiD = d.ProductOrder_CharProduct.DimensiD;
                                        prod.DimensiL = d.ProductOrder_CharProduct.DimensiL;
                                        prod.DimensiP = d.ProductOrder_CharProduct.DimensiP;
                                        prod.DimensiR = d.ProductOrder_CharProduct.DimensiR;
                                        prod.KodeKomponen1 = d.ProductOrder_CharProduct.KodeKomponen1;
                                        prod.JlhKomponen1 = d.ProductOrder_CharProduct.JlhKomponen1;
                                        prod.KodeKomponen2 = d.ProductOrder_CharProduct.KodeKomponen2;
                                        prod.JlhKomponen2 = d.ProductOrder_CharProduct.JlhKomponen2;
                                        prod.KodeKomponen3 = d.ProductOrder_CharProduct.KodeKomponen3;
                                        prod.JlhKomponen3 = d.ProductOrder_CharProduct.JlhKomponen3;
                                        prod.KodeKomponen4 = d.ProductOrder_CharProduct.KodeKomponen4;
                                        prod.JlhKomponen4 = d.ProductOrder_CharProduct.JlhKomponen4;
                                        prod.KodeKomponen5 = d.ProductOrder_CharProduct.KodeKomponen5;
                                        prod.JlhKomponen5 = d.ProductOrder_CharProduct.JlhKomponen5;
                                        prod.KodeKomponen6 = d.ProductOrder_CharProduct.KodeKomponen6;
                                        prod.JlhKomponen6 = d.ProductOrder_CharProduct.JlhKomponen6;
                                        prod.Komponen = d.ProductOrder_CharProduct.Komponen;
                                        prod.PoinKomponen = d.ProductOrder_CharProduct.PoinKomponen;
                                        prod.BebanSusut = d.ProductOrder_CharProduct.BebanSusut;

                                        db.KodeKaretDJ_CharProduct.InsertOnSubmit(prod);
                                        db.SubmitChanges();



                                        var finishing = from p in db.ProductOrder_Finishing
                                                        where p.IDForm == d.ID
                                                        select p;
                                        if (finishing != null)
                                        {

                                            foreach (var r in finishing)
                                            {
                                                KodeKaretDJ_Finishing fns = new KodeKaretDJ_Finishing();
                                                fns.IDForm = LastKodeKaret;
                                                fns.IDFinishing = r.IDFinishing;
                                                fns.Biaya = r.Biaya;
                                                fns.Efektif = r.Efektif;
                                                db.KodeKaretDJ_Finishing.InsertOnSubmit(fns);
                                                db.SubmitChanges();
                                            }

                                        }
                                        saveStoneKaret(d, LastKodeKaret);
                                    }
                                    else
                                    {
                                        LastKodeKaret = Convert.ToInt32(d.IDKodeKaret);

                                        var dsg = db.KodeKaretDJ_CharDesign.SingleOrDefault(p => p.ID == LastKodeKaret);

                                        dsg.DesignCategory = d.ProductOrder_CharDesign.DesignCategory;
                                        dsg.DesignConcept = d.ProductOrder_CharDesign.DesignConcept;
                                        dsg.DesignProcess = d.ProductOrder_CharDesign.DesignProcess;
                                        dsg.TargetAge = d.ProductOrder_CharDesign.TargetAge;
                                        dsg.TargetGender = d.ProductOrder_CharDesign.TargetGender;
                                        dsg.Naming = d.ProductOrder_CharDesign.Naming;
                                        dsg.SetItem = d.ProductOrder_CharDesign.SetItem;
                                        dsg.Special = d.ProductOrder_CharDesign.Special;

                                        db.SubmitChanges();

                                        var prod = db.KodeKaretDJ_CharProduct.SingleOrDefault(p => p.ID == LastKodeKaret);
                                        prod.FrameColor = d.ProductOrder_CharProduct.FrameColor;
                                        prod.FrameFinishing = d.ProductOrder_CharProduct.FrameFinishing;
                                        prod.FrameMaterial = d.ProductOrder_CharProduct.FrameMaterial;
                                        prod.ProcessCons = d.ProductOrder_CharProduct.ProcessCons;
                                        prod.ProductCategory = d.ProductOrder_CharProduct.ProductCategory;
                                        prod.ProductItem = d.ProductOrder_CharProduct.ProductItem;
                                        prod.ProductLevel = d.ProductOrder_CharProduct.ProductLevel;
                                        prod.StoneDist = d.ProductOrder_CharProduct.StoneDist;
                                        prod.GrossWeight = d.ProductOrder_CharProduct.GrossWeight;
                                        prod.StoneCarat = d.ProductOrder_CharProduct.StoneCarat;
                                        prod.StoneWeight = d.ProductOrder_CharProduct.StoneWeight;
                                        prod.NettoWeight = d.ProductOrder_CharProduct.NettoWeight;
                                        prod.KadarLogam = d.ProductOrder_CharProduct.KadarLogam;
                                        prod.KadarTukaran = d.ProductOrder_CharProduct.KadarTukaran;
                                        prod.DimensiD = d.ProductOrder_CharProduct.DimensiD;
                                        prod.DimensiL = d.ProductOrder_CharProduct.DimensiL;
                                        prod.DimensiP = d.ProductOrder_CharProduct.DimensiP;
                                        prod.DimensiR = d.ProductOrder_CharProduct.DimensiR;
                                        prod.KodeKomponen1 = d.ProductOrder_CharProduct.KodeKomponen1;
                                        prod.JlhKomponen1 = d.ProductOrder_CharProduct.JlhKomponen1;
                                        prod.KodeKomponen2 = d.ProductOrder_CharProduct.KodeKomponen2;
                                        prod.JlhKomponen2 = d.ProductOrder_CharProduct.JlhKomponen2;
                                        prod.KodeKomponen3 = d.ProductOrder_CharProduct.KodeKomponen3;
                                        prod.JlhKomponen3 = d.ProductOrder_CharProduct.JlhKomponen3;
                                        prod.KodeKomponen4 = d.ProductOrder_CharProduct.KodeKomponen4;
                                        prod.JlhKomponen4 = d.ProductOrder_CharProduct.JlhKomponen4;
                                        prod.KodeKomponen5 = d.ProductOrder_CharProduct.KodeKomponen5;
                                        prod.JlhKomponen5 = d.ProductOrder_CharProduct.JlhKomponen5;
                                        prod.KodeKomponen6 = d.ProductOrder_CharProduct.KodeKomponen6;
                                        prod.JlhKomponen6 = d.ProductOrder_CharProduct.JlhKomponen6;
                                        prod.Komponen = d.ProductOrder_CharProduct.Komponen;
                                        prod.PoinKomponen = d.ProductOrder_CharProduct.PoinKomponen;
                                        prod.BebanSusut = d.ProductOrder_CharProduct.BebanSusut;

                                        db.SubmitChanges();

                                        var z = db.KodeKaretDJ_Finishing.Where(p => p.IDForm == LastKodeKaret);
                                        if (z != null)
                                        {
                                            db.KodeKaretDJ_Finishing.DeleteAllOnSubmit(z);
                                            db.SubmitChanges();
                                        }

                                        var finishing = from p in db.ProductOrder_Finishing
                                                        where p.IDForm == d.ID
                                                        select p;
                                        if (finishing != null)
                                        {

                                            foreach (var r in finishing)
                                            {
                                                KodeKaretDJ_Finishing fns = new KodeKaretDJ_Finishing();
                                                fns.IDForm = LastKodeKaret;
                                                fns.IDFinishing = r.IDFinishing;
                                                fns.Biaya = r.Biaya;
                                                fns.Efektif = r.Efektif;
                                                db.KodeKaretDJ_Finishing.InsertOnSubmit(fns);
                                                db.SubmitChanges();
                                            }

                                        }
                                        saveStoneKaret(d, LastKodeKaret);
                                    }

                                    string CB = ""; /// mondial, franknco, missmondial, thepalace
                                    CB = d.CompanyBrand.Nama.ToLower();

                                    string companycode = d.CompanyBrand.NamaKode;
                    
                                    if (Counter == 1)
                                    {
                                        d.NoSPK = "SPK/" + companycode + "/" + Tahun + "/" + Bulan + "/" + SPKNumber;
                                        d.IDKodeKaret = (d.IDKodeKaret != null) ? d.IDKodeKaret : LastKodeKaret;
                                        d.StatusFPS = 0;
                                        d.StatusPenerimaan = 0;
                                        d.Approval = App.UserNama;
                                        d.ApprovalTgl = System.DateTime.Now;
                                        if (d.JenisSPK == 2)
                                        {
                                            d.ApprovalGM = App.UserNama;
                                            d.ApprovalGMTanggal = DateTime.Today;
                                        }
                                        d.IDDesigner = d.IDDesigner;
                                        d.IDSegmen = d.IDSegmen;
                                        db.SubmitChanges();

                                        LastSPKID = d.ID;
                                    }
                                    else
                                    {
                                        ProductOrder kd = new ProductOrder();

                                        /*if (d.IDKodeKaret == null)
                                            KodeKaret = KodeBrand + KodeItem + KodeProductCategory + KodeSet + Tahun + Str.KodeFormat(SeqProductCategory, 4);
                                        else
                                        {
                                            var karet = db.KodeKaretDJ.SingleOrDefault(p => p.ID == d.IDKodeKaret);
                                            KodeKaret = karet.Nomor;
                                        }*/

                                        kd.IDBrand = d.IDBrand;
                                        kd.KodeKaret = KodeKaret;
                                        kd.IDKodeKaret = LastKodeKaret;
                                        kd.NoSPK = "SPK/" + companycode + "/" + Tahun + "/" + Bulan + "/" + SPKNumber;
                                        kd.Tgl = d.Tgl;
                                        kd.Keterangan = d.Keterangan;
                                        kd.Operator = d.Operator;
                                        kd.OperatorTgl = d.OperatorTgl;
                                        kd.ImgPicture = d.ImgPicture;
                                        kd.StatusFPS = 0;
                                        kd.StatusPenerimaan = 0;
                                        kd.Draft = false;
                                        kd.DraftDate = d.Tgl;
                                        kd.RequestCustomer = d.RequestCustomer;
                                        kd.HumanDesigner = d.HumanDesigner;
                                        kd.HumanDesignerNama = d.HumanDesignerNama;
                                        kd.TanggalJatuhTempoDJ = d.TanggalJatuhTempoDJ;
                                        kd.TanggalJatuhTempoKonsumen = d.TanggalJatuhTempoKonsumen;
                                        kd.TanggalJatuhTempoRangka = d.TanggalJatuhTempoRangka;
                                        kd.TanggalJatuhTempoSetting = d.TanggalJatuhTempoSetting;
                                        kd.Operator = d.Operator;
                                        kd.OperatorTgl = d.OperatorTgl;
                                        kd.Approval = d.Approval;
                                        kd.ApprovalTgl = d.ApprovalTgl;
                                        kd.JlhCopy = 1;
                                        kd.IDDesigner = d.IDDesigner;
                                        kd.IDSegmen = d.IDSegmen;

                                        kd.StatusBarang = d.StatusBarang;
                                        kd.StatusBarangID = d.StatusBarangID;
                                        kd.StatusBarangKeterangan = d.StatusBarangKeterangan;
                                        kd.StatusPesanan = d.StatusPesanan;

                                        db.ProductOrder.InsertOnSubmit(kd);
                                        db.SubmitChanges();

                                        try
                                        {
                                            LastSPKID = Db.SingleInteger("SELECT TOP 1 ID FROM ProductOrder ORDER BY ID DESC", Db.Cnn_DB);
                                        }
                                        catch (Exception)
                                        {
                                            LastSPKID = 1;
                                        }

                                        ProductOrder_CharDesign dsg = new ProductOrder_CharDesign();
                                        dsg.ID = LastSPKID;
                                        dsg.DesignCategory = d.ProductOrder_CharDesign.DesignCategory;
                                        dsg.DesignConcept = d.ProductOrder_CharDesign.DesignConcept;
                                        dsg.DesignProcess = d.ProductOrder_CharDesign.DesignProcess;
                                        dsg.TargetAge = d.ProductOrder_CharDesign.TargetAge;
                                        dsg.TargetGender = d.ProductOrder_CharDesign.TargetGender;
                                        dsg.Naming = d.ProductOrder_CharDesign.Naming;
                                        dsg.SetItem = d.ProductOrder_CharDesign.SetItem;
                                        dsg.Special = d.ProductOrder_CharDesign.Special;

                                        db.ProductOrder_CharDesign.InsertOnSubmit(dsg);
                                        db.SubmitChanges();

                                        ProductOrder_CharProduct prod = new ProductOrder_CharProduct();
                                        prod.ID = LastSPKID;
                                        prod.FrameColor = d.ProductOrder_CharProduct.FrameColor;
                                        prod.FrameFinishing = d.ProductOrder_CharProduct.FrameFinishing;
                                        prod.FrameMaterial = d.ProductOrder_CharProduct.FrameMaterial;
                                        prod.ProcessCons = d.ProductOrder_CharProduct.ProcessCons;
                                        prod.ProductCategory = d.ProductOrder_CharProduct.ProductCategory;
                                        prod.ProductItem = d.ProductOrder_CharProduct.ProductItem;
                                        prod.ProductLevel = d.ProductOrder_CharProduct.ProductLevel;
                                        prod.StoneDist = d.ProductOrder_CharProduct.StoneDist;
                                        prod.GrossWeight = d.ProductOrder_CharProduct.GrossWeight;
                                        prod.StoneCarat = d.ProductOrder_CharProduct.StoneCarat;
                                        prod.StoneWeight = d.ProductOrder_CharProduct.StoneWeight;
                                        prod.NettoWeight = d.ProductOrder_CharProduct.NettoWeight;
                                        prod.KadarLogam = d.ProductOrder_CharProduct.KadarLogam;
                                        prod.KadarTukaran = d.ProductOrder_CharProduct.KadarTukaran;
                                        prod.DimensiD = d.ProductOrder_CharProduct.DimensiD;
                                        prod.DimensiL = d.ProductOrder_CharProduct.DimensiL;
                                        prod.DimensiP = d.ProductOrder_CharProduct.DimensiP;
                                        prod.DimensiR = d.ProductOrder_CharProduct.DimensiR;

                                        db.ProductOrder_CharProduct.InsertOnSubmit(prod);
                                        db.SubmitChanges();



                                        var finishing = from p in db.ProductOrder_Finishing
                                                        where p.IDForm == d.ID
                                                        select p;
                                        if (finishing != null)
                                        {
                                            foreach (var r in finishing)
                                            {
                                                ProductOrder_Finishing fns = new ProductOrder_Finishing();

                                                fns.IDForm = LastSPKID;
                                                fns.IDFinishing = r.IDFinishing;
                                                fns.Biaya = r.Biaya;
                                                fns.Efektif = r.Efektif;
                                                db.ProductOrder_Finishing.InsertOnSubmit(fns);
                                                db.SubmitChanges();
                                            }

                                        }

                                        saveStone(d, LastSPKID);

                                        saveFinishing(d, prod);

                                        saveProduct(d, prod);
                                        saveBiaya(d, prod);

                                        saveMU(d, prod);
                                        saveSegmen(d, prod);
                                        saveTotalStone(d);
                                    }

                                    string KodeKaretDetail = "";
                                    int SeqKodeKaretDetail = 1;

                                    try
                                    {
                                        SeqKodeKaretDetail = Db.SingleInteger("SELECT MAX(Increment) FROM KodeKaret_Detail WHERE IDKodeKaret=" + LastKodeKaret, Db.Cnn_DB);
                                        SeqKodeKaretDetail = SeqKodeKaretDetail + 1;
                                    }
                                    catch (Exception)
                                    {
                                        SeqKodeKaretDetail = 1;
                                    }

                                    if (d.IDKodeKaret == null)
                                        KodeKaret = KodeBrand + KodeItem + KodeProductCategory + KodeSet + Tahun + Str.KodeFormat(SeqProductCategory, 4);
                                    else
                                    {
                                        var karet = db.KodeKaretDJ.SingleOrDefault(p => p.ID == d.IDKodeKaret);
                                        KodeKaret = karet.Nomor;
                                    }

                                    KodeKaretDetail = KodeKaret + "-" + Str.KodeFormat(SeqKodeKaretDetail, 4);

                                    KodeKaret_Detail det = new KodeKaret_Detail();
                                    det.IDKodeKaret = LastKodeKaret;
                                    det.IDProductOrder = LastSPKID;
                                    det.Increment = SeqKodeKaretDetail;
                                    det.KodeKaretDetail = KodeKaretDetail;
                                    det.NoSPK = (Counter == 1) ? d.NoSPK : "SPK/" + companycode + "/" + Tahun + "/" + Bulan + "/" + SPKNumber;
                                    det.StatusOrder = (Counter == 1) ? 1 : 2;

                                    db.KodeKaret_Detail.InsertOnSubmit(det);
                                    db.SubmitChanges();

                                    Counter++;
                                }

                                // Edit Master Kode Karet by Last SPK Repeat Order
                                updateKodeKaret(d.KodeKaretDJ, d);

                                //end edit
                            }

                            //Redirect
                            //Response.Redirect("SPK.aspx?id=" + myID);
                            //App.Reload(this);
                        }



                        try
                        {
                            Db.Execute("EXEC sp_postspk " + d.ID + ", '" + d.NoSPK + "'", Db.Cnn_DB);
                        }
                        catch { }


                    }//end if check box checked
                }
            }
            index++;
        }
        App.Reload(this);
    }
    protected void deleteProcess(ProductOrder d)
    {
        if (d.ProductOrder_PricingProduct != null)
            db.ProductOrder_PricingProduct.DeleteOnSubmit(
                d.ProductOrder_PricingProduct);

        if (d.ProductOrder_PricingBiaya != null)
            db.ProductOrder_PricingBiaya.DeleteOnSubmit(
                d.ProductOrder_PricingBiaya);

        if (d.ProductOrder_PricingMU != null)
            db.ProductOrder_PricingMU.DeleteOnSubmit(
                d.ProductOrder_PricingMU);

        //Commit
        db.SubmitChanges();
    }

    protected void saveStone(ProductOrder d)
    {
        saveStone1A(d);
        saveStone1B(d);
        saveStone2(d);
        saveStone3(d);
        saveStone4(d);
        saveStone5(d);
        
    }
    protected void saveStone1A(ProductOrder d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(0);
        decimal OngkosPB = 0m;

        var rs = from p in d.ProductOrder_Stone1A select p;
        foreach (var r in rs)
        {

            StonePricing1A prc = r.Stone1A.StonePricing1A;
            StoneCosting1A cost = r.Stone1A.StoneCosting1A;
            if (prc == null || cost == null) PricingError(sebab,d.ID);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab,d.ID);
                else
                {

                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone1A.Parcel101.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone1B(ProductOrder d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(1);

        decimal OngkosPB = 0m;

        var rs = from p in d.ProductOrder_Stone1B select p;
        foreach (var r in rs)
        {

            StonePricing1B prc = r.Stone1B.StonePricing1B;
            StoneCosting1B cost = r.Stone1B.StoneCosting1B;
            if (prc == null || cost == null) PricingError(sebab,d.ID);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue || !prc.InputH.HasValue || !prc.InputP.HasValue) PricingError(sebab,d.ID);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    //Rapaport
                    r.HargaInputH = prc.InputH.Value;
                    r.HargaInputP = prc.InputP.Value;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone1B.Parcel101.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone2(ProductOrder d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(2);

        decimal OngkosPB = 0m;

        var rs = from p in d.ProductOrder_Stone2 select p;
        foreach (var r in rs)
        {

            StonePricing2 prc = r.Stone2.StonePricing2;
            StoneCosting2 cost = r.Stone2.StoneCosting2;
            if (prc == null || cost == null) PricingError(sebab,d.ID);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab, d.ID);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone2.Parcel201.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone3(ProductOrder d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(3);

        decimal OngkosPB = 0m;

        var rs = from p in d.ProductOrder_Stone3 select p;
        foreach (var r in rs)
        {

            StonePricing3 prc = r.Stone3.StonePricing3;
            StoneCosting3 cost = r.Stone3.StoneCosting3;
            if (prc == null || cost == null) PricingError(sebab, d.ID);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab, d.ID);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone3.Parcel301.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone4(ProductOrder d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(4);

        decimal OngkosPB = 0m;

        var rs = from p in d.ProductOrder_Stone4 select p;
        foreach (var r in rs)
        {

            StonePricing4 prc = r.Stone4.StonePricing4;
            StoneCosting4 cost = r.Stone4.StoneCosting4;
            if (prc == null || cost == null) PricingError(sebab, d.ID);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab, d.ID);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone4.Parcel401.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone5(ProductOrder d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(5);

        decimal OngkosPB = 0m;

        var rs = from p in d.ProductOrder_Stone5 select p;
        foreach (var r in rs)
        {

            StonePricing5 prc = r.Stone5.StonePricing5;
            StoneCosting5 cost = r.Stone5.StoneCosting5;
            if (prc == null || cost == null) PricingError(sebab, d.ID);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab, d.ID);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone5.Parcel501.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }

    protected void saveFinishing(ProductOrder d, ProductOrder_CharProduct product)
    {
        var rs = from p in d.ProductOrder_Finishing select p;
        foreach (var r in rs)
        {
            string sebab = "";
            bool ok = true;

            //Pricing
            var prc_ff = db.PricingTableFF.SingleOrDefault(p =>
                p.IDItem == product.ProductItem &&
                p.IDFinishing == r.IDFinishing &&
                p.Biaya.HasValue &&
                p.ApprovalTgl.HasValue
                );

            //Pricing - Validasi
            if (prc_ff == null) { ok = false; sebab = "Finishing Fee"; }

            //Process
            if (!ok) PricingError(sebab,d.ID);
            else
            {
                r.Biaya = prc_ff.Biaya.Value;
                r.Efektif = prc_ff.ApprovalTgl.Value;

                //Commit
                db.SubmitChanges();
            }
        }
    }

    protected void saveProduct(ProductOrder d, ProductOrder_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        //Pricing

        int IDTG_Costing = CV.TG_PG1_Costing;
        int IDTG_Pricing = CV.TG_PG1_Pricing;
        if (d.ProductOrder_CharProduct.CharFrameMaterial.NamaKode == "EMS")
        {
            IDTG_Costing = CV.TG_PG1_Costing;
            IDTG_Pricing = CV.TG_PG1_Pricing;
        }
        else if (d.ProductOrder_CharProduct.CharFrameMaterial.NamaKode == "PAL")
        {
            IDTG_Costing = CV.TG_PG_Paladium;
            IDTG_Pricing = CV.TG_PG_Paladium2;
        }
        else if (d.ProductOrder_CharProduct.CharFrameMaterial.NamaKode == "PLA")
        {
            IDTG_Costing = CV.TG_PG_Platina;
            IDTG_Pricing = CV.TG_PG_Platina2;
        }
        else if (d.ProductOrder_CharProduct.CharFrameMaterial.NamaKode == "SIL")
        {
            IDTG_Costing = CV.TG_PG_Silver;
            IDTG_Pricing = CV.TG_PG_Silver2;
        }

        var prc_tgM = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == IDTG_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == IDTG_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_tg == null || prc_tgM == null) { ok = false; sebab = "Today Gold Price"; }

        //Process
        if (!ok) PricingError(sebab,d.ID);
        else
        {
            ProductOrder_PricingProduct n = new ProductOrder_PricingProduct();

            n.ID = d.ID;

            //Gold Price
            n.GoldRate = prc_tg.Harga.Value;
            n.EfektifGold = prc_tg.ApprovalTgl.Value;

            //Informasi Berat
            n.Netto = product.NettoWeight;
            n.Kadar = product.KadarTukaran;

            //Total
            n.TotalProduct = n.GoldRate * n.Netto * (n.Kadar / (decimal)100);
            n.TotalStone1A = Convert.ToDecimal(TotalStone1A(d));
            n.TotalStone1B = Convert.ToDecimal(TotalStone1B(d));
            n.TotalStone2 = Convert.ToDecimal(TotalStone2(d));
            n.TotalStone3 = Convert.ToDecimal(TotalStone3(d));
            n.TotalStone4 = Convert.ToDecimal(TotalStone4(d));
            n.TotalStone5 = Convert.ToDecimal(TotalStone5(d));
            n.Total = n.TotalProduct + Convert.ToDecimal(n.TotalStone1A) + Convert.ToDecimal(n.TotalStone1B) + Convert.ToDecimal(n.TotalStone2) + Convert.ToDecimal(n.TotalStone3) + Convert.ToDecimal(n.TotalStone4) + Convert.ToDecimal(n.TotalStone5);

            //Commit
            db.ProductOrder_PricingProduct.InsertOnSubmit(n);
            db.SubmitChanges();


        }
    }
    protected decimal TotalStone1A(ProductOrder d)
    {
        decimal total = 0;

        try
        {
            var rs_1A = from p in d.ProductOrder_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_1A) total += r;
        }
        catch (Exception) { }
        return total;
    }
    protected decimal TotalStone1B(ProductOrder d)
    {
        decimal total = 0;

        try
        {
            var rs_1B = from p in d.ProductOrder_Stone1B where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_1B) total += r;
        }
        catch (Exception) { }
        return total;
    }
    protected decimal TotalStone2(ProductOrder d)
    {
        decimal total = 0;

        try
        {
            var rs_2 = from p in d.ProductOrder_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_2) total += r;
        }
        catch (Exception) { }

        return total;
    }
    protected decimal TotalStone3(ProductOrder d)
    {
        decimal total = 0;

        try
        {
            var rs_3 = from p in d.ProductOrder_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_3) total += r;
        }
        catch (Exception) { }

        return total;
    }
    protected decimal TotalStone4(ProductOrder d)
    {
        decimal total = 0;

        try
        {
            var rs_4 = from p in d.ProductOrder_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_4) total += r;
        }
        catch (Exception) { }

        return total;
    }
    protected decimal TotalStone5(ProductOrder d)
    {
        decimal total = 0;

        try
        {
            var rs_5 = from p in d.ProductOrder_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_5) total += r;
        }
        catch (Exception) { }

        return total;
    }

    protected void saveBiaya(ProductOrder d, ProductOrder_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_fm = db.PricingTableFM.SingleOrDefault(p =>
            p.IDBrand == d.IDBrand &&
            p.IDItem == product.ProductItem &&
            p.IDCons == product.ProcessCons &&
            p.Biaya.HasValue &&
            p.Pembagi.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_fs = db.PricingTableFS.SingleOrDefault(p =>
            p.ID == FS_ID(d, product) &&
            p.Biaya.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_fm == null) { ok = false; sebab = "Mounting Fee"; }
        if (prc_fs == null) { ok = false; sebab = "Setting Fee"; }
        if (prc_us == null) { ok = false; sebab = "USD Rate"; }

        //Process
        if (!ok) PricingError(sebab,d.ID);
        else
        {
            ProductOrder_PricingBiaya n = new ProductOrder_PricingBiaya();
            n.ID = d.ID;

            //Informasi
            n.NamaBrand = "Calculator";
            n.NamaItem = product.CharProductItem.Nama;
            n.NamaCons = product.CharProcessCons.Nama;

            //Mounting
            n.Mounting = prc_fm.Biaya.Value;
            n.MountingNO = prc_fm.InputNO.Value;
            n.MountingRO = prc_fm.InputRO.Value;

            //Setting
            n.Setting = prc_fs.Biaya.Value;
            
            //Finishing
            n.Finishing = TotalFinishing(d);

            //Total
            n.TotalRupiah = n.Mounting + n.Setting + n.Finishing;
            n.TotalRate = prc_us.Harga.Value;
            n.Total = n.TotalRupiah / n.TotalRate;

            //Tgl. Efektif
            n.EfektifMounting = prc_fm.ApprovalTgl.Value;
            n.EfektifSetting = prc_fs.ApprovalTgl.Value;
            n.EfektifRate = prc_us.ApprovalTgl.Value;

            //Commit
            db.ProductOrder_PricingBiaya.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }

    protected int FS_ID(ProductOrder d, ProductOrder_CharProduct product)
    {
        return 1; //Masih di-hardcoded. BELUM SELESAI !
    }

    protected string saveMU(ProductOrder d, ProductOrder_CharProduct product)
    {
        string strDebug = "";

        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_mu = db.PricingTableMU.SingleOrDefault(p =>
            p.IDCategory == product.ProductCategory &&
            p.IDLevel == product.ProductLevel &&
            p.IDDist == product.StoneDist &&
            p.Persen.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //Pricing - Validasi
        if (prc_mu == null) { ok = false; sebab = "Diamond Jewelry MU"; }
        if (prc_us == null) { ok = false; sebab = "USD Rate"; }

        //Process
        if (!ok) PricingError(sebab, d.ID);
        else
        {
            ProductOrder_PricingMU n = new ProductOrder_PricingMU();

            n.ID = d.ID;

            //Informasi
            n.NamaCategory = product.CharProductCategory.Nama;
            n.NamaLevel = product.CharProductLevel.Nama;
            n.NamaDist = product.CharStoneDist.Nama;

            //Modal
            n.ModalUSD = TotalModal(d.ID);
            n.ModalRate = prc_us.Harga.Value;
            n.ModalRupiah = n.ModalUSD * n.ModalRate;

            //Mark-Up
            n.MU = prc_mu.Persen.Value;

            decimal OngkosLain = 0m;

            strDebug = "&TotalToMarkUp=" + TotalToMarkUp(d.ID).ToString()
                    + "&MU=" + n.MU.ToString()
                    + "&TotalStone1B=" + TotalStone1B(d.ID).ToString()
                    + "&TotalStone3NMU=" + TotalStone3NMU(d.ID).ToString()
                    ;

            //Harga Final
            n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID);
            n.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID)) * n.ModalRate + OngkosLain;
            n.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID)) * n.ModalRate + OngkosLain);




            //Tgl. Efektif
            n.EfektifRate = prc_us.ApprovalTgl.Value;

            //Commit
            db.ProductOrder_PricingMU.InsertOnSubmit(n);
            db.SubmitChanges();
        }

        return strDebug;
    }

    //Segmen System

    protected decimal TotalModal(int ID)
    {
        decimal total = 0;

        var product = db.ProductOrder_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.ProductOrder_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += product.Total;
        total += biaya.Total;

        return total;
    }
    protected decimal TotalToMarkUp(int ID)
    {
        decimal total = 0;

        var product = db.ProductOrder_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.ProductOrder_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        var stone3 = from p in db.ProductOrder_Stone3
                     where p.IDForm == ID && (p.GIA == "" || p.GIA == null)
                     select p;
        decimal Stone3ToMarkUp = 0m;
        try
        {
            Stone3ToMarkUp = stone3.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception) { }

        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(product.TotalStone3) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(Stone3ToMarkUp) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        total += biaya.Total;

        return total;
    }

    protected decimal TotalStone3NMU(int ID)
    {
        decimal total = 0;

        var stone3 = from p in db.ProductOrder_Stone3
                     where p.IDForm == ID && p.GIA != ""
                     select p;
        try
        {
            total = Convert.ToDecimal(stone3.Sum(p => p.HargaTotal.Value));
        }
        catch (Exception) { }

        //total += biaya.Total;

        return total;
    }
    protected decimal TotalStone1B(int ID)
    {
        decimal total = 0;

        var product = db.ProductOrder_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.ProductOrder_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalStone1B);
        //total += biaya.Total;

        return total;
    }

    protected decimal TotalFinishing(ProductOrder d)
    {
        decimal total = 0;

        var rs = from p in d.ProductOrder_Finishing where p.Biaya.HasValue select p.Biaya.Value;
        foreach (var r in rs) total += r;

        return total;
    }

    protected void saveSegmen(ProductOrder d, ProductOrder_CharProduct product)
    {
        string sebab = "Segmen System";
        bool ok = true;

        //Pricing - Validasi
        ok = checkSegmen(product);

        //Process
        if (!ok) PricingError(sebab,d.ID);
        else
        {
            //Product Value
            decimal harga = HargaUSD(d.ID);
            //decimal carat = product.StoneCarat;

            decimal carat = 0m;

            try
            {
                carat = (from p in db.ProductOrder_Stone1A where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductOrder_Stone1B where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductOrder_Stone2 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductOrder_Stone3 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductOrder_Stone4 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductOrder_Stone5 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Max(p => p.Carat);
            }
            catch (Exception) { }

        }
    }

    protected bool checkSegmen(ProductOrder_CharProduct product)
    {
        bool ok = true;

        var rs = from p in db.PricingTableSS
                 where
                     p.IDCategory == product.ProductCategory &&
                     p.IDLevel == product.ProductLevel &&
                     p.IDDist == product.StoneDist && p.CharProductSegmen.NamaKode != "NN"
                 select p;

        foreach (var r in rs)
        {
            if (!r.Minimum.HasValue || !r.ApprovalTgl.HasValue)
            {
                ok = false;
                break;
            }
        }

        return ok;
    }

    protected decimal HargaUSD(int ID)
    {
        var mu = db.ProductOrder_PricingMU.SingleOrDefault(p => p.ID == ID);
        return mu.HargaUSD;
    }

    protected void saveTotalStone(ProductOrder d)
    {
        decimal total = 0;

        var rs_1A = from p in d.ProductOrder_Stone1A select p.TotalButir;
        foreach (var r in rs_1A) total += r;

        var rs_1B = from p in d.ProductOrder_Stone1B where p.StatusTerpasang == 1 select p.TotalButir;
        foreach (var r in rs_1B) total += r;

        var rs_2 = from p in d.ProductOrder_Stone2 select p.TotalButir;
        foreach (var r in rs_2) total += r;

        var rs_3 = from p in d.ProductOrder_Stone3 select p.TotalButir;
        foreach (var r in rs_3) total += r;

        var rs_4 = from p in d.ProductOrder_Stone4 select p.TotalButir;
        foreach (var r in rs_4) total += r;

        var rs_5 = from p in d.ProductOrder_Stone5 select p.TotalButir;
        foreach (var r in rs_5) total += r;

        d.ProductOrder_CharProduct.StoneQty = total;
        db.SubmitChanges();

    }

    protected void saveStone(ProductOrder d, int SPKID)
    {
        saveStone1A(d, SPKID);
        saveStone1B(d, SPKID);
        saveStone2(d, SPKID);
        saveStone3(d, SPKID);
        saveStone4(d, SPKID);
        saveStone5(d, SPKID);
    }
    protected void saveStoneKaret(ProductOrder d, int IDKodeKaret)
    {
        saveStoneKaret1A(d, IDKodeKaret);
        saveStoneKaret1B(d, IDKodeKaret);
        saveStoneKaret2(d, IDKodeKaret);
        saveStoneKaret3(d, IDKodeKaret);
        saveStoneKaret4(d, IDKodeKaret);
        saveStoneKaret5(d, IDKodeKaret);
    }
    protected void saveStone1A(ProductOrder d, int SPKID)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(0);
        decimal OngkosPB = 0m;

        var rs = from p in d.ProductOrder_Stone1A where p.IDForm == SPKID select p;
        foreach (var r in rs)
        {
            StonePricing1A prc = r.Stone1A.StonePricing1A;
            StoneCosting1A cost = r.Stone1A.StoneCosting1A;

            ProductOrder_Stone1A s = new ProductOrder_Stone1A();
            s.IDForm = SPKID;
            s.IDStone = r.IDStone;
            s.TotalButir = r.TotalButir;
            s.TotalCarat = r.TotalCarat;
            s.CaratButir = r.CaratButir;
            s.DimensiP = r.DimensiP;
            s.DimensiL = r.DimensiL;
            s.DimensiT = r.DimensiT;


            //Harga
            s.HargaSatuan = prc.Harga.Value;
            s.HargaTotal = s.HargaSatuan * s.TotalCarat;

            s.HargaSatuanM = cost.Harga.Value;
            s.HargaTotalM = s.HargaSatuanM * s.TotalCarat;

            //Tgl. Efektif
            s.Efektif = prc.ApprovalTgl.Value;

            //Setting
            OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone1A.Parcel101.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
            s.Setting = OngkosPB;

            db.ProductOrder_Stone1A.InsertOnSubmit(s);
            db.SubmitChanges();
        }
    }
    protected void saveStone1B(ProductOrder d, int SPKID)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(1);
        decimal OngkosPB = 0m;

        var rs = from p in d.ProductOrder_Stone1B where p.IDForm == SPKID select p;
        foreach (var r in rs)
        {
            StonePricing1B prc = r.Stone1B.StonePricing1B;
            StoneCosting1B cost = r.Stone1B.StoneCosting1B;

            ProductOrder_Stone1B s = new ProductOrder_Stone1B();
            s.IDForm = SPKID;
            s.IDStone = r.IDStone;
            s.TotalButir = r.TotalButir;
            s.TotalCarat = r.TotalCarat;
            s.CaratButir = r.CaratButir;
            s.DimensiP = r.DimensiP;
            s.DimensiL = r.DimensiL;
            s.DimensiT = r.DimensiT;
            s.GIA = r.GIA;

            //Harga
            s.HargaSatuan = prc.Harga.Value;
            s.HargaTotal = s.HargaSatuan * s.TotalCarat;

            s.HargaSatuanM = cost.Harga.Value;
            s.HargaTotalM = s.HargaSatuanM * s.TotalCarat;

            //Tgl. Efektif
            s.Efektif = prc.ApprovalTgl.Value;

            //Setting
            OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone1B.Parcel101.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
            r.Setting = OngkosPB;

            db.ProductOrder_Stone1B.InsertOnSubmit(s);
            db.SubmitChanges();
        }
    }
    protected void saveStone2(ProductOrder d, int SPKID)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(2);
        decimal OngkosPB = 0m;

        var rs = from p in d.ProductOrder_Stone2 where p.IDForm == SPKID select p;
        foreach (var r in rs)
        {
            StonePricing2 prc = r.Stone2.StonePricing2;
            StoneCosting2 cost = r.Stone2.StoneCosting2;

            ProductOrder_Stone2 s = new ProductOrder_Stone2();
            s.IDForm = SPKID;
            s.IDStone = r.IDStone;
            s.TotalButir = r.TotalButir;
            s.TotalCarat = r.TotalCarat;
            s.CaratButir = r.CaratButir;
            s.DimensiP = r.DimensiP;
            s.DimensiL = r.DimensiL;
            s.DimensiT = r.DimensiT;

            //Harga
            s.HargaSatuan = prc.Harga.Value;
            s.HargaTotal = s.HargaSatuan * s.TotalCarat;

            s.HargaSatuanM = cost.Harga.Value;
            s.HargaTotalM = s.HargaSatuanM * s.TotalCarat;

            //Tgl. Efektif
            s.Efektif = prc.ApprovalTgl.Value;

            //Setting
            OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone2.Parcel201.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
            r.Setting = OngkosPB;

            db.ProductOrder_Stone2.InsertOnSubmit(s);
            db.SubmitChanges();
        }
    }
    protected void saveStone3(ProductOrder d, int SPKID)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(3);
        decimal OngkosPB = 0m;

        var rs = from p in d.ProductOrder_Stone3 where p.IDForm == SPKID select p;
        foreach (var r in rs)
        {
            StonePricing3 prc = r.Stone3.StonePricing3;
            StoneCosting3 cost = r.Stone3.StoneCosting3;

            ProductOrder_Stone3 s = new ProductOrder_Stone3();
            s.IDForm = SPKID;
            s.IDStone = r.IDStone;
            s.TotalButir = r.TotalButir;
            s.TotalCarat = r.TotalCarat;
            s.CaratButir = r.CaratButir;
            s.DimensiP = r.DimensiP;
            s.DimensiL = r.DimensiL;
            s.DimensiT = r.DimensiT;
            s.GIA = r.GIA;

            //Harga
            s.HargaSatuan = prc.Harga.Value;
            s.HargaTotal = s.HargaSatuan * s.TotalCarat;

            s.HargaSatuanM = cost.Harga.Value;
            s.HargaTotalM = s.HargaSatuanM * s.TotalCarat;

            //Tgl. Efektif
            s.Efektif = prc.ApprovalTgl.Value;

            //Setting
            OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone3.Parcel301.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
            r.Setting = OngkosPB;

            db.ProductOrder_Stone3.InsertOnSubmit(s);
            db.SubmitChanges();
        }
    }
    protected void saveStone4(ProductOrder d, int SPKID)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(4);
        decimal OngkosPB = 0m;

        var rs = from p in d.ProductOrder_Stone4 where p.IDForm == SPKID select p;
        foreach (var r in rs)
        {
            StonePricing4 prc = r.Stone4.StonePricing4;
            StoneCosting4 cost = r.Stone4.StoneCosting4;

            ProductOrder_Stone4 s = new ProductOrder_Stone4();
            s.IDForm = SPKID;
            s.IDStone = r.IDStone;
            s.TotalButir = r.TotalButir;
            s.TotalCarat = r.TotalCarat;
            s.CaratButir = r.CaratButir;
            s.DimensiP = r.DimensiP;
            s.DimensiL = r.DimensiL;
            s.DimensiT = r.DimensiT;

            //Harga
            s.HargaSatuan = prc.Harga.Value;
            s.HargaTotal = s.HargaSatuan * s.TotalCarat;

            s.HargaSatuanM = cost.Harga.Value;
            s.HargaTotalM = s.HargaSatuanM * s.TotalCarat;

            //Tgl. Efektif
            s.Efektif = prc.ApprovalTgl.Value;

            //Setting
            OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone4.Parcel401.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
            r.Setting = OngkosPB;

            db.ProductOrder_Stone4.InsertOnSubmit(s);
            db.SubmitChanges();
        }
    }
    protected void saveStone5(ProductOrder d, int SPKID)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(5);
        decimal OngkosPB = 0m;

        var rs = from p in d.ProductOrder_Stone5 where p.IDForm == SPKID select p;
        foreach (var r in rs)
        {
            StonePricing5 prc = r.Stone5.StonePricing5;
            StoneCosting5 cost = r.Stone5.StoneCosting5;

            ProductOrder_Stone5 s = new ProductOrder_Stone5();
            s.IDForm = SPKID;
            s.IDStone = r.IDStone;
            s.TotalButir = r.TotalButir;
            s.TotalCarat = r.TotalCarat;
            s.CaratButir = r.CaratButir;
            s.DimensiP = r.DimensiP;
            s.DimensiL = r.DimensiL;
            s.DimensiT = r.DimensiT;

            //Harga
            s.HargaSatuan = prc.Harga.Value;
            s.HargaTotal = s.HargaSatuan * s.TotalCarat;

            s.HargaSatuanM = cost.Harga.Value;
            s.HargaTotalM = s.HargaSatuanM * s.TotalCarat;

            //Tgl. Efektif
            s.Efektif = prc.ApprovalTgl.Value;

            //Setting
            OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone5.Parcel501.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
            r.Setting = OngkosPB;

            db.ProductOrder_Stone5.InsertOnSubmit(s);
            db.SubmitChanges();
        }
    }


    protected void saveStoneKaret1A(ProductOrder d, int IDKodeKaret)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(0);

        var z = db.KodeKaretDJ_Stone1A.Where(p => p.IDForm == IDKodeKaret);
        if (z != null)
        {
            db.KodeKaretDJ_Stone1A.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var rs = from p in d.ProductOrder_Stone1A where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {
            KodeKaretDJ_Stone1A s = new KodeKaretDJ_Stone1A();
            s.IDForm = IDKodeKaret;
            s.IDStone = r.IDStone;
            s.TotalButir = r.TotalButir;
            s.TotalCarat = r.TotalCarat;
            s.CaratButir = r.CaratButir;
            s.DimensiP = r.DimensiP;
            s.DimensiL = r.DimensiL;
            s.DimensiT = r.DimensiT;

            db.KodeKaretDJ_Stone1A.InsertOnSubmit(s);
            db.SubmitChanges();
        }
    }
    protected void saveStoneKaret1B(ProductOrder d, int IDKodeKaret)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(1);

        var z = db.KodeKaretDJ_Stone1B.Where(p => p.IDForm == IDKodeKaret);
        if (z != null)
        {
            db.KodeKaretDJ_Stone1B.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var rs = from p in d.ProductOrder_Stone1B where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {
            KodeKaretDJ_Stone1B s = new KodeKaretDJ_Stone1B();
            s.IDForm = IDKodeKaret;
            s.IDStone = r.IDStone;
            s.TotalButir = r.TotalButir;
            s.TotalCarat = r.TotalCarat;
            s.CaratButir = r.CaratButir;
            s.DimensiP = r.DimensiP;
            s.DimensiL = r.DimensiL;
            s.DimensiT = r.DimensiT;
            s.GIA = r.GIA;
            s.StatusTerpasang = r.StatusTerpasang;

            db.KodeKaretDJ_Stone1B.InsertOnSubmit(s);
            db.SubmitChanges();
        }
    }
    protected void saveStoneKaret2(ProductOrder d, int IDKodeKaret)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(2);

        var z = db.KodeKaretDJ_Stone2.Where(p => p.IDForm == IDKodeKaret);
        if (z != null)
        {
            db.KodeKaretDJ_Stone2.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var rs = from p in d.ProductOrder_Stone2 where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {

            KodeKaretDJ_Stone2 s = new KodeKaretDJ_Stone2();
            s.IDForm = IDKodeKaret;
            s.IDStone = r.IDStone;
            s.TotalButir = r.TotalButir;
            s.TotalCarat = r.TotalCarat;
            s.CaratButir = r.CaratButir;
            s.DimensiP = r.DimensiP;
            s.DimensiL = r.DimensiL;
            s.DimensiT = r.DimensiT;

            db.KodeKaretDJ_Stone2.InsertOnSubmit(s);
            db.SubmitChanges();
        }
    }
    protected void saveStoneKaret3(ProductOrder d, int IDKodeKaret)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(3);

        var z = db.KodeKaretDJ_Stone3.Where(p => p.IDForm == IDKodeKaret);
        if (z != null)
        {
            db.KodeKaretDJ_Stone3.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var rs = from p in d.ProductOrder_Stone3 where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {
            KodeKaretDJ_Stone3 s = new KodeKaretDJ_Stone3();
            s.IDForm = IDKodeKaret;
            s.IDStone = r.IDStone;
            s.TotalButir = r.TotalButir;
            s.TotalCarat = r.TotalCarat;
            s.CaratButir = r.CaratButir;
            s.DimensiP = r.DimensiP;
            s.DimensiL = r.DimensiL;
            s.DimensiT = r.DimensiT;
            s.GIA = r.GIA;

            db.KodeKaretDJ_Stone3.InsertOnSubmit(s);
            db.SubmitChanges();
        }
    }
    protected void saveStoneKaret4(ProductOrder d, int IDKodeKaret)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(4);

        var z = db.KodeKaretDJ_Stone4.Where(p => p.IDForm == IDKodeKaret);
        if (z != null)
        {
            db.KodeKaretDJ_Stone4.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var rs = from p in d.ProductOrder_Stone4 where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {
            KodeKaretDJ_Stone4 s = new KodeKaretDJ_Stone4();
            s.IDForm = IDKodeKaret;
            s.IDStone = r.IDStone;
            s.TotalButir = r.TotalButir;
            s.TotalCarat = r.TotalCarat;
            s.CaratButir = r.CaratButir;
            s.DimensiP = r.DimensiP;
            s.DimensiL = r.DimensiL;
            s.DimensiT = r.DimensiT;

            db.KodeKaretDJ_Stone4.InsertOnSubmit(s);
            db.SubmitChanges();
        }
    }
    protected void saveStoneKaret5(ProductOrder d, int IDKodeKaret)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(5);

        var z = db.KodeKaretDJ_Stone5.Where(p => p.IDForm == IDKodeKaret);
        if (z != null)
        {
            db.KodeKaretDJ_Stone5.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var rs = from p in d.ProductOrder_Stone5 where p.IDForm == d.ID select p;
        foreach (var r in rs)
        {
            KodeKaretDJ_Stone5 s = new KodeKaretDJ_Stone5();
            s.IDForm = IDKodeKaret;
            s.IDStone = r.IDStone;
            s.TotalButir = r.TotalButir;
            s.TotalCarat = r.TotalCarat;
            s.CaratButir = r.CaratButir;
            s.DimensiP = r.DimensiP;
            s.DimensiL = r.DimensiL;
            s.DimensiT = r.DimensiT;

            db.KodeKaretDJ_Stone5.InsertOnSubmit(s);
            db.SubmitChanges();
        }
    }

    protected void updateKodeKaret(KodeKaretDJ ms, ProductOrder m)
    {
        //updateKaretStone1A(ms, m);
        //updateKaretStone1B(ms, m);
        //updateKaretStone2(ms, m);
        //updateKaretStone3(ms, m);
        //updateKaretStone4(ms, m);
        //updateKaretStone5(ms, m);
    }

}