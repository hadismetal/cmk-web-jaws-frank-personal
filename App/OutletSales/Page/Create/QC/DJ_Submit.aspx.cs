using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_QC_DJ_Submit : System.Web.UI.Page
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

        if (!IsPostBack)
        {
            DLC.CharFrameColor(framecolor);
            Ddl.FisikBarangQC(fisik);
            Ddl.GrafirLogoQC(logo);
            Ddl.GrafirKadarEmasQC(kadaremas);
            TipeLocation(tipe_lokasi);
            Location(lokasi, Convert.ToInt32(tipe_lokasi.SelectedValue));
            fillForm();
        }
        fillStone();
    }

    protected string getLocationUser()
    {
        string result = "";
        int xUserID = App.UserID;
        int xisAM = 0;
        string listStore = "";
        //check area manager
        var am = from p in db.DataAreaManager where p.IDUser == App.UserID select p;
        int amcount = am.Count();
        if (amcount > 0)
        {
            int i = 0;
            foreach (var x in am)
            {
                int j = 0;
                var lokasi = x.DataAreaManager_Lokasi
                    .GroupBy(p => new { p.IDForm, p.TipeLokasi, p.IDLokasi })
                    .Select(c => new { c.Key.IDForm, c.Key.TipeLokasi, c.Key.IDLokasi });
                foreach (var y in lokasi)
                {
                    //listStore = "";
                    if (y.TipeLokasi == 1)
                        listStore += db.LocOutlet.Single(p => p.ID == y.IDLokasi).Kode;
                    else
                        listStore += db.LocExhibition.Single(p => p.ID == y.IDLokasi).Kode;
                    j++;
                    listStore += ",";
                }
                i++;
            }
            xisAM = 1;
        }

        if (xisAM == 0)
        {
            //check admin sales
            var ad = from p in db.DataAdminSales where p.IDUser == App.UserID select p;
            if (ad.Count() > 0)
            {
                int k = 0;
                foreach (var x in ad)
                {
                    listStore = "";
                    if (x.TipeLokasi == 1)
                        listStore = db.LocOutlet.Single(p => p.ID == x.IDLokasi).Kode;
                    else
                        listStore = db.LocExhibition.Single(p => p.ID == x.IDLokasi).Kode;
                    k++;
                    listStore += ",";
                }
            }
        }

        result = (listStore.Length > 0) ? listStore.Remove(listStore.Length - 1) : listStore;

        return result;
    }

    protected void Location(DropDownList ddl, int tipelokasi)
    {
        string keyloc = getLocationUser();
        string query = string.Empty;
        if (tipelokasi == -1)
        {
            query = "SELECT A.* FROM (" +
                "SELECT TOP 0 ID, Kode, UPPER(Nama) [Nama] FROM LocOutlet WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',',')))) " +
                ") A";
        }
        else 
        if(tipelokasi == 1)
        {
            query = "SELECT A.* FROM(" +
                "SELECT ID, Kode, UPPER(Nama) [Nama] FROM LocOutlet WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',','))))" +
                ") A";
        }
        else if(tipelokasi == 2)
        {
            query = "SELECT A.* FROM(" +
                "SELECT ID, Kode, UPPER(Nama) [Nama] FROM LocExhibition WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',','))))" +
                ") A";
        }
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem("All", "-1"));
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Kode"].ToString() + " (" + item["Nama"].ToString() + ")";
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }

    protected void Location_Changed(object sender, EventArgs e)
    {
        Location(lokasi, Convert.ToInt32(tipe_lokasi.SelectedValue));
    }

    protected void TipeLocation(DropDownList ddl)
    {
        string keyloc = getLocationUser();

        string query = "SELECT A.* FROM (" +
            "SELECT TOP 1 1 [Tipe], 'Outlet' [Nama] FROM LocOutlet WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',',')))) " +
            "UNION ALL " +
            "SELECT TOP 1 2 [Tipe], 'Exhibition' [Nama] FROM LocExhibition WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',',')))) " +
            ") A";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem("Pilih Tipe Lokasi", "-1"));
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["Tipe"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }

    //Form
    protected void fillForm()
    {
        var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_plu.InnerHtml = d.Nomor;
            header_item.InnerHtml = d.StockProductDJ_CharProduct.CharProductItem.Nama;

            grossweightoriginal.Text = Str.Number2(d.StockProductDJ_CharProduct.GrossWeight);
            grossweight.Text = Str.Number2(d.StockProductDJ_CharProduct.GrossWeight);

            framecolor.SelectedValue = d.StockProductDJ_CharProduct.CharFrameColor.ID.ToString();

            //Picture Repair
            //Upload.Image(imgpicture, d.ImgPicture);

            string urlImage = ConfigurationManager.AppSettings["ImageAPI"];
            imgpicture.ImageUrl = urlImage + d.ImgPicture;

            string pqc = "";
            using (Map_AC w = new Map_AC())
            {
                var user = w.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
                if (user != null)
                    pqc = user.Nama;// + " (" + user.UserNumber +")"; 
            }

            petugasqc.Text = pqc;

            //fillStone();
            //fillProduct(d);
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
            tr.Attributes.Add("tipe", "1A");
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            string sertifikat = Db.SingleString("SELECT TOP 1 ISNULL(CertificateNumber,'') [GIA] FROM StockProductDJ_Stone1A WHERE ID = " + r.ID, Db.Cnn_DB);
            string appendcert = (!string.IsNullOrEmpty(sertifikat)) ? "(GIA : " + sertifikat + ")" : "";

            td = new TableCell();
            td.Text = r.Stone1A.Parcel101.Nama + " " + r.Stone1A.Parcel102.Nama + " " + appendcert;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            td.ID = "b1AR" + index.ToString();
            td.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            td.ID = "c1AR" + index.ToString();
            td.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            tr.Cells.Add(td);

            TextBox txtbutiract = new TextBox();
            txtbutiract.ID = "b1A" + index.ToString();
            //txtbutiract.Attributes["style"] = "width: 200px;"
            txtbutiract.CssClass = "form-control";
            txtbutiract.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtbutiract.TextMode = TextBoxMode.Number;
            txtbutiract.Text = Str.Integer(r.TotalButir);
            txtbutiract.Attributes.Add("onblur", "onkeypressStone1A" + txtbutiract.ID + "();");

            td = new TableCell();
            td.Controls.Add(txtbutiract);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txtcaratact = new TextBox();
            txtcaratact.ID = "c1A" + index.ToString();
            //txtcaratact.Attributes["style"] = "width: 200px;";
            txtcaratact.CssClass = "form-control";
            txtcaratact.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtcaratact.Attributes.Add("readonly", "readonly");
            txtcaratact.Text = Str.Number3(r.TotalCarat);

            td = new TableCell();
            td.Controls.Add(txtcaratact);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            CheckBox cb = new CheckBox();
            cb.ID = "cb1A" + index.ToString();
            cb.Checked = true;
            cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(cb);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "s1A" + index.ToString();
            txt.Attributes["style"] = "width: 200px;";
            txt.CssClass = "form-control";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            HiddenField hf = new HiddenField();
            hf.ID = "hfs1A" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ClientScript.RegisterStartupScript(Page.GetType(), "script_1A_" + index.ToString(), " " + setJavascriptKeyPressStone("b1AR" + index.ToString(), "c1AR" + index.ToString(), "b1A" + index.ToString(), "c1A" + index.ToString()) + " ", true);

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
            tr.Attributes.Add("tipe", "1B");
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            string sertifikat = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') [GIA] FROM StockProductDJ_Stone1B WHERE ID = " + r.ID, Db.Cnn_DB);
            string appendcert = (!string.IsNullOrEmpty(sertifikat)) ? "(GIA : " + sertifikat + ")" : "";

            td = new TableCell();
            td.Text = r.Stone1B.Parcel101.Nama + " " + r.Stone1B.Parcel102.Nama + " " + appendcert;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            td.ID = "b1BR" + index.ToString();
            td.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            td.ID = "c1BR" + index.ToString();
            td.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            tr.Cells.Add(td);

            TextBox txtbutiract = new TextBox();
            txtbutiract.ID = "b1B" + index.ToString();
            //txtbutiract.Attributes["style"] = "width: 200px;";
            txtbutiract.CssClass = "form-control";
            txtbutiract.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtbutiract.TextMode = TextBoxMode.Number;
            txtbutiract.Text = Str.Integer(r.TotalButir);
            txtbutiract.Attributes.Add("onblur", "onkeypressStone1B" + txtbutiract.ID + "();");

            td = new TableCell();
            td.Controls.Add(txtbutiract);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txtcaratact = new TextBox();
            txtcaratact.ID = "c1B" + index.ToString();
            //txtcaratact.Attributes["style"] = "width: 200px;";
            txtcaratact.CssClass = "form-control";
            txtcaratact.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtcaratact.Attributes.Add("readonly", "readonly");
            txtcaratact.Text = Str.Number3(r.TotalCarat);

            td = new TableCell();
            td.Controls.Add(txtcaratact);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            CheckBox cb = new CheckBox();
            cb.ID = "cb1B" + index.ToString();
            cb.Checked = true;
            cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(cb);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "s1B" + index.ToString();
            txt.Attributes["style"] = "width: 200px;";
            txt.CssClass = "form-control";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            HiddenField hf = new HiddenField();
            hf.ID = "hfs1B" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ClientScript.RegisterStartupScript(Page.GetType(), "script_1B_" + index.ToString(), " " + setJavascriptKeyPressStone("b1BR" + index.ToString(), "c1BR" + index.ToString(), "b1B" + index.ToString(), "c1B" + index.ToString()) + " ", true);

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
            tr.Attributes.Add("tipe", "2");
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            string sertifikat = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') [GIA] FROM StockProductDJ_Stone2 WHERE ID = " + r.ID, Db.Cnn_DB);
            string appendcert = (!string.IsNullOrEmpty(sertifikat)) ? "(GIA : " + sertifikat + ")" : "";

            td = new TableCell();
            td.Text = r.Stone2.Parcel201.Nama + " " + r.Stone2.Parcel202.Nama + " " + appendcert;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            td.ID = "b2R" + index.ToString();
            td.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            td.ID = "c2R" + index.ToString();
            td.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            tr.Cells.Add(td);

            TextBox txtbutiract = new TextBox();
            txtbutiract.ID = "b2" + index.ToString();
            //txtbutiract.Attributes["style"] = "width: 200px;";
            txtbutiract.CssClass = "form-control";
            txtbutiract.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtbutiract.TextMode = TextBoxMode.Number;
            txtbutiract.Text = Str.Integer(r.TotalButir);
            txtbutiract.Attributes.Add("onblur", "onkeypressStone2" + txtbutiract.ID + "();");

            td = new TableCell();
            td.Controls.Add(txtbutiract);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txtcaratact = new TextBox();
            txtcaratact.ID = "c2" + index.ToString();
            //txtcaratact.Attributes["style"] = "width: 200px;";
            txtcaratact.CssClass = "form-control";
            txtcaratact.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtcaratact.Attributes.Add("readonly", "readonly");
            txtcaratact.Text = Str.Number3(r.TotalCarat);

            td = new TableCell();
            td.Controls.Add(txtcaratact);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            CheckBox cb = new CheckBox();
            cb.ID = "cb2" + index.ToString();
            cb.Checked = true;
            cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(cb);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "s2" + index.ToString();
            txt.Attributes["style"] = "width: 200px;";
            txt.CssClass = "form-control";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            HiddenField hf = new HiddenField();
            hf.ID = "hfs2" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ClientScript.RegisterStartupScript(Page.GetType(), "script_2_" + index.ToString(), " " + setJavascriptKeyPressStone("b2R" + index.ToString(), "c2R" + index.ToString(), "b2" + index.ToString(), "c2" + index.ToString()) + " ", true);

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
            tr.Attributes.Add("tipe", "3");
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            string sertifikat = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') [GIA] FROM StockProductDJ_Stone3 WHERE ID = " + r.ID, Db.Cnn_DB);
            string appendcert = (!string.IsNullOrEmpty(sertifikat)) ? "(GIA : " + sertifikat + ")" : "";

            td = new TableCell();
            td.Text = r.Stone3.Parcel301.Nama + " " + r.Stone3.Parcel302.Nama + " " + appendcert;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            td.ID = "b3R" + index.ToString();
            td.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            td.ID = "c3R" + index.ToString();
            td.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            tr.Cells.Add(td);

            TextBox txtbutiract = new TextBox();
            txtbutiract.ID = "b3" + index.ToString();
            //txtbutiract.Attributes["style"] = "width: 200px;";
            txtbutiract.CssClass = "form-control";
            txtbutiract.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtbutiract.TextMode = TextBoxMode.Number;
            txtbutiract.Text = Str.Integer(r.TotalButir);
            txtbutiract.Attributes.Add("onblur", "onkeypressStone3" + txtbutiract.ID + "();");

            td = new TableCell();
            td.Controls.Add(txtbutiract);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txtcaratact = new TextBox();
            txtcaratact.ID = "c3" + index.ToString();
            //txtcaratact.Attributes["style"] = "width: 200px;";
            txtcaratact.CssClass = "form-control";
            txtcaratact.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtcaratact.Attributes.Add("readonly", "readonly");
            txtcaratact.Text = Str.Number3(r.TotalCarat);

            td = new TableCell();
            td.Controls.Add(txtcaratact);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            CheckBox cb = new CheckBox();
            cb.ID = "cb3" + index.ToString();
            cb.Checked = true;
            cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(cb);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "s3" + index.ToString();
            txt.Attributes["style"] = "width: 200px;";
            txt.CssClass = "form-control";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            HiddenField hf = new HiddenField();
            hf.ID = "hfs3" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ClientScript.RegisterStartupScript(Page.GetType(), "script_3_" + index.ToString(), " " + setJavascriptKeyPressStone("b3R" + index.ToString(), "c3R" + index.ToString(), "b3" + index.ToString(), "c3" + index.ToString()) + " ", true);

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
            tr.Attributes.Add("tipe", "4");
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            string sertifikat = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') [GIA] FROM StockProductDJ_Stone4 WHERE ID = " + r.ID, Db.Cnn_DB);
            string appendcert = (!string.IsNullOrEmpty(sertifikat)) ? "(GIA : " + sertifikat + ")" : "";

            td = new TableCell();
            td.Text = r.Stone4.Parcel401.Nama + " " + r.Stone4.Parcel402.Nama + " " + appendcert;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            td.ID = "b4R" + index.ToString();
            td.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            td.ID = "c4R" + index.ToString();
            td.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            tr.Cells.Add(td);

            TextBox txtbutiract = new TextBox();
            txtbutiract.ID = "b4" + index.ToString();
            //txtbutiract.Attributes["style"] = "width: 200px;";
            txtbutiract.CssClass = "form-control";
            txtbutiract.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtbutiract.TextMode = TextBoxMode.Number;
            txtbutiract.Text = Str.Integer(r.TotalButir);
            txtbutiract.Attributes.Add("onblur", "onkeypressStone4" + txtbutiract.ID + "();");

            td = new TableCell();
            td.Controls.Add(txtbutiract);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txtcaratact = new TextBox();
            txtcaratact.ID = "c4" + index.ToString();
            //txtcaratact.Attributes["style"] = "width: 200px;";
            txtcaratact.CssClass = "form-control";
            txtcaratact.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtcaratact.Attributes.Add("readonly", "readonly");
            txtcaratact.Text = Str.Number3(r.TotalCarat);

            td = new TableCell();
            td.Controls.Add(txtcaratact);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            CheckBox cb = new CheckBox();
            cb.ID = "cb4" + index.ToString();
            cb.Checked = true;
            cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(cb);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "s4" + index.ToString();
            txt.Attributes["style"] = "width: 200px;";
            txt.CssClass = "form-control";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            HiddenField hf = new HiddenField();
            hf.ID = "hfs4" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ClientScript.RegisterStartupScript(Page.GetType(), "script_4_" + index.ToString(), " " + setJavascriptKeyPressStone("b4R" + index.ToString(), "c4R" + index.ToString(), "b4" + index.ToString(), "c4" + index.ToString()) + " ", true);

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
            tr.Attributes.Add("tipe", "5");
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            string sertifikat = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') [GIA] FROM StockProductDJ_Stone5 WHERE ID = " + r.ID, Db.Cnn_DB);
            string appendcert = (!string.IsNullOrEmpty(sertifikat)) ? "(GIA : " + sertifikat + ")" : "";

            td = new TableCell();
            td.Text = r.Stone5.Parcel501.Nama + " " + r.Stone5.Parcel502.Nama + " " + appendcert;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            td.ID = "b5R" + index.ToString();
            td.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            td.ID = "c5R" + index.ToString();
            td.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            tr.Cells.Add(td);

            TextBox txtbutiract = new TextBox();
            txtbutiract.ID = "b5" + index.ToString();
            //txtbutiract.Attributes["style"] = "width: 200px;";
            txtbutiract.CssClass = "form-control";
            txtbutiract.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtbutiract.TextMode = TextBoxMode.Number;
            txtbutiract.Text = Str.Integer(r.TotalButir);
            txtbutiract.Attributes.Add("onblur", "onkeypressStone5" + txtbutiract.ID + "();");

            td = new TableCell();
            td.Controls.Add(txtbutiract);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txtcaratact = new TextBox();
            txtcaratact.ID = "c5" + index.ToString();
            //txtcaratact.Attributes["style"] = "width: 200px;";
            txtcaratact.CssClass = "form-control";
            txtcaratact.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txtcaratact.Attributes.Add("readonly", "readonly");
            txtcaratact.Text = Str.Number3(r.TotalCarat);

            td = new TableCell();
            td.Controls.Add(txtcaratact);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            CheckBox cb = new CheckBox();
            cb.ID = "cb5" + index.ToString();
            cb.Checked = true;
            cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(cb);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "s5" + index.ToString();
            txt.Attributes["style"] = "width: 200px;";
            txt.CssClass = "form-control";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            HiddenField hf = new HiddenField();
            hf.ID = "hfs5" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ClientScript.RegisterStartupScript(Page.GetType(), "script_5_" + index.ToString(), " " + setJavascriptKeyPressStone("b5R" + index.ToString(), "c5R" + index.ToString(), "b5" + index.ToString(), "c5" + index.ToString()) + " ", true);

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
        td.ColumnSpan = 9;
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

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        string msg = validateFillDJ();
        if (string.IsNullOrEmpty(msg))
        {
            var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myID);
            if (d == null) App.DataNull(this);
            else
            {
                DocQCDJ x = new DocQCDJ();

                decimal GW = Convert.ToDecimal(String.Format("{0:0.00}", grossweight.Text.Trim()));
                decimal NW = RecalWeight(GW, d);

                string NomorQC = Document.Generate_Nomor_QCDJ(x);

                x.QCNama = petugasqc.Text;
                x.Keterangan = keterangan.Text;
                x.Nomor = NomorQC;
                x.IDProduct = d.ID;
                x.Blemish = "";//blemish.Text;
                x.JumlahButir = "";//jlhbutir.Text;
                x.Sertifikat = "";//sertifikat.Text;
                x.NonCZ = "";//noncz.Text;
                x.CZ = "";//cz.Text;
                x.Logo = logo.Text;
                x.KadarEmas = kadaremas.Text;
                x.Merk = "";//merk.Text;
                x.FisikKeseluruhan = fisik.Text;
                x.GrossWeight = GW;
                x.NetWeight = Str.Number2(NW);
                x.WarnaEmas = framecolor.SelectedItem.Text;
                x.OperatorNama = App.UserNama;
                x.OperatorTgl = DateTime.Now;
                x.Draft = false;
                x.DraftDate = null;

                db.DocQCDJ.InsertOnSubmit(x);
                db.SubmitChanges();

                string query = "UPDATE DocQCDJ SET TipeLokasi = @TipeLokasi, IDLokasi = @IDLokasi WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@TipeLokasi", SqlDbType.Int){ Value = Convert.ToInt32(tipe_lokasi.SelectedValue) },
                    new SqlParameter("@IDLokasi", SqlDbType.Int){ Value = Convert.ToInt32(lokasi.SelectedValue) },
                    new SqlParameter("@ID", SqlDbType.Int){ Value = Convert.ToInt32(x.ID) }
                };
                Execute(query, Db.Cnn_DB, parameters, CommandType.Text);

                saveStone(x);

                //Redirect
                //Response.Redirect("DJ_Detail.aspx?id=" + x.ID);

                // Alert + Redirect
                string url = "../../Report/QCDJ.aspx?refreshPage=1&kw=" + x.Nomor + "&dari=" + Convert.ToDateTime(x.OperatorTgl).ToString("dd-MM-yyyy") + "&sampai=" + Convert.ToDateTime(x.OperatorTgl).ToString("dd-MM-yyyy");
                string script = "alert('Data saved successfully! Please Approve QC DJ before use on '); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert", script, true);
            }
            lblwarning.Visible = false;
            lblwarning.InnerText = "";
        }
        else
        {
            lblwarning.Visible = true;
            lblwarning.InnerText = msg;
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
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";
        TextBox butiract = new TextBox();
        string butiracttxt = "0";
        TextBox caratact = new TextBox();
        string caratacttxt = "0";
        CheckBox sesuaicert = new CheckBox();
        string sesuaicerttxt = "0";

        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfs1A" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("s1A" + index.ToString());

                butiract = (TextBox)row.FindControl("b1A" + index.ToString());
                butiracttxt = butiract.Text;

                caratact = (TextBox)row.FindControl("c1A" + index.ToString());
                caratacttxt = caratact.Text.Replace(".", "").Replace(",", ".");

                sesuaicert = (CheckBox)row.FindControl("cb1A" + index.ToString());
                sesuaicerttxt = (sesuaicert.Checked) ? "1" : "0";

                var d = db.StockProductDJ_Stone1A.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {


                        DocQCDJ_Stone1A j = new DocQCDJ_Stone1A();

                        j.IDForm = s.ID;
                        j.IDStone = d.ID;
                        j.Keterangan = txt.Text;

                        db.DocQCDJ_Stone1A.InsertOnSubmit(j);
                        db.SubmitChanges();

                        string query = "UPDATE DocQCDJ_Stone1A SET TotalButirActual = '" + butiracttxt + "', TotalCaratActual = '" + caratacttxt + "', SesuaiSertifikat = " + sesuaicerttxt + " WHERE ID = " + j.ID;
                        Db.Execute(query, Db.Cnn_DB);

                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            index++;
        }
    }

    protected void saveStone1B(DocQCDJ s)
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";
        TextBox butiract = new TextBox();
        string butiracttxt = "0";
        TextBox caratact = new TextBox();
        string caratacttxt = "0";
        CheckBox sesuaicert = new CheckBox();
        string sesuaicerttxt = "0";

        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfs1B" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("s1B" + index.ToString());

                butiract = (TextBox)row.FindControl("b1B" + index.ToString());
                butiracttxt = butiract.Text.Replace(".", "").Replace(",", ".");

                caratact = (TextBox)row.FindControl("c1B" + index.ToString());
                caratacttxt = caratact.Text.Replace(".", "").Replace(",", ".");

                sesuaicert = (CheckBox)row.FindControl("cb1B" + index.ToString());
                sesuaicerttxt = (sesuaicert.Checked) ? "1" : "0";

                var d = db.StockProductDJ_Stone1B.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {


                        DocQCDJ_Stone1B j = new DocQCDJ_Stone1B();

                        j.IDForm = s.ID;
                        j.IDStone = d.ID;
                        j.Keterangan = txt.Text;

                        db.DocQCDJ_Stone1B.InsertOnSubmit(j);
                        db.SubmitChanges();

                        string query = "UPDATE DocQCDJ_Stone1B SET TotalButirActual = '" + butiracttxt + "', TotalCaratActual = '" + caratacttxt + "', SesuaiSertifikat = " + sesuaicerttxt + " WHERE ID = " + j.ID;
                        Db.Execute(query, Db.Cnn_DB);

                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            index++;
        }
    }

    protected void saveStone2(DocQCDJ s)
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";
        TextBox butiract = new TextBox();
        string butiracttxt = "0";
        TextBox caratact = new TextBox();
        string caratacttxt = "0";
        CheckBox sesuaicert = new CheckBox();
        string sesuaicerttxt = "0";

        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfs2" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("s2" + index.ToString());

                butiract = (TextBox)row.FindControl("b2" + index.ToString());
                butiracttxt = butiract.Text.Replace(".", "").Replace(",", ".");

                caratact = (TextBox)row.FindControl("c2" + index.ToString());
                caratacttxt = caratact.Text.Replace(".", "").Replace(",", ".");

                sesuaicert = (CheckBox)row.FindControl("cb2" + index.ToString());
                sesuaicerttxt = (sesuaicert.Checked) ? "1" : "0";

                var d = db.StockProductDJ_Stone2.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {


                        DocQCDJ_Stone2 j = new DocQCDJ_Stone2();

                        j.IDForm = s.ID;
                        j.IDStone = d.ID;
                        j.Keterangan = txt.Text;

                        db.DocQCDJ_Stone2.InsertOnSubmit(j);
                        db.SubmitChanges();

                        string query = "UPDATE DocQCDJ_Stone2 SET TotalButirActual = '" + butiracttxt + "', TotalCaratActual = '" + caratacttxt + "', SesuaiSertifikat = " + sesuaicerttxt + " WHERE ID = " + j.ID;
                        Db.Execute(query, Db.Cnn_DB);

                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            index++;
        }
    }

    protected void saveStone3(DocQCDJ s)
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";
        TextBox butiract = new TextBox();
        string butiracttxt = "0";
        TextBox caratact = new TextBox();
        string caratacttxt = "0";
        CheckBox sesuaicert = new CheckBox();
        string sesuaicerttxt = "0";

        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfs3" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("s3" + index.ToString());

                butiract = (TextBox)row.FindControl("b3" + index.ToString());
                butiracttxt = butiract.Text.Replace(".", "").Replace(",", ".");

                caratact = (TextBox)row.FindControl("c3" + index.ToString());
                caratacttxt = caratact.Text.Replace(".", "").Replace(",", ".");

                sesuaicert = (CheckBox)row.FindControl("cb3" + index.ToString());
                sesuaicerttxt = (sesuaicert.Checked) ? "1" : "0";

                var d = db.StockProductDJ_Stone3.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {


                        DocQCDJ_Stone3 j = new DocQCDJ_Stone3();

                        j.IDForm = s.ID;
                        j.IDStone = d.ID;
                        j.Keterangan = txt.Text;

                        db.DocQCDJ_Stone3.InsertOnSubmit(j);
                        db.SubmitChanges();

                        string query = "UPDATE DocQCDJ_Stone3 SET TotalButirActual = '" + butiracttxt + "', TotalCaratActual = '" + caratacttxt + "', SesuaiSertifikat = " + sesuaicerttxt + " WHERE ID = " + j.ID;
                        Db.Execute(query, Db.Cnn_DB);

                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            index++;
        }
    }

    protected void saveStone4(DocQCDJ s)
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";
        TextBox butiract = new TextBox();
        string butiracttxt = "0";
        TextBox caratact = new TextBox();
        string caratacttxt = "0";
        CheckBox sesuaicert = new CheckBox();
        string sesuaicerttxt = "0";

        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfs4" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("s4" + index.ToString());

                butiract = (TextBox)row.FindControl("b4" + index.ToString());
                butiracttxt = butiract.Text.Replace(".", "").Replace(",", ".");

                caratact = (TextBox)row.FindControl("c4" + index.ToString());
                caratacttxt = caratact.Text.Replace(".", "").Replace(",", ".");

                sesuaicert = (CheckBox)row.FindControl("cb4" + index.ToString());
                sesuaicerttxt = (sesuaicert.Checked) ? "1" : "0";

                var d = db.StockProductDJ_Stone4.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {


                        DocQCDJ_Stone4 j = new DocQCDJ_Stone4();

                        j.IDForm = s.ID;
                        j.IDStone = d.ID;
                        j.Keterangan = txt.Text;

                        db.DocQCDJ_Stone4.InsertOnSubmit(j);
                        db.SubmitChanges();

                        string query = "UPDATE DocQCDJ_Stone4 SET TotalButirActual = '" + butiracttxt + "', TotalCaratActual = '" + caratacttxt + "', SesuaiSertifikat = " + sesuaicerttxt + " WHERE ID = " + j.ID;
                        Db.Execute(query, Db.Cnn_DB);

                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            index++;
        }
    }

    protected void saveStone5(DocQCDJ s)
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";
        TextBox butiract = new TextBox();
        string butiracttxt = "0";
        TextBox caratact = new TextBox();
        string caratacttxt = "0";
        CheckBox sesuaicert = new CheckBox();
        string sesuaicerttxt = "0";

        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfs5" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("s5" + index.ToString());

                butiract = (TextBox)row.FindControl("b5" + index.ToString());
                butiracttxt = butiract.Text.Replace(".", "").Replace(",", ".");

                caratact = (TextBox)row.FindControl("c5" + index.ToString());
                caratacttxt = caratact.Text.Replace(".", "").Replace(",", ".");

                sesuaicert = (CheckBox)row.FindControl("cb5" + index.ToString());
                sesuaicerttxt = (sesuaicert.Checked) ? "1" : "0";

                var d = db.StockProductDJ_Stone5.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {


                        DocQCDJ_Stone5 j = new DocQCDJ_Stone5();

                        j.IDForm = s.ID;
                        j.IDStone = d.ID;
                        j.Keterangan = txt.Text;

                        db.DocQCDJ_Stone5.InsertOnSubmit(j);
                        db.SubmitChanges();

                        string query = "UPDATE DocQCDJ_Stone5 SET TotalButirActual = '" + butiracttxt + "', TotalCaratActual = '" + caratacttxt + "', SesuaiSertifikat = " + sesuaicerttxt + " WHERE ID = " + j.ID;
                        Db.Execute(query, Db.Cnn_DB);

                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            index++;
        }
    }

    public decimal RecalWeight(Decimal GrossWeight, StockProductDJ d)
    {
        decimal NettoWeight = 0m;
        decimal TotalCarat = 0m;
        decimal StoneWeight = 0m;

        //Stone
        var rs_1A = from p in d.StockProductDJ_Stone1A select p.TotalCarat;
        foreach (var r in rs_1A) TotalCarat += r;

        var rs_1B = from p in d.StockProductDJ_Stone1B select p.TotalCarat;
        foreach (var r in rs_1B) TotalCarat += r;

        var rs_2 = from p in d.StockProductDJ_Stone2 select p.TotalCarat;
        foreach (var r in rs_2) TotalCarat += r;

        var rs_3 = from p in d.StockProductDJ_Stone3 select p.TotalCarat;
        foreach (var r in rs_3) TotalCarat += r;

        var rs_4 = from p in d.StockProductDJ_Stone4 select p.TotalCarat;
        foreach (var r in rs_4) TotalCarat += r;

        var rs_5 = from p in d.StockProductDJ_Stone5 select p.TotalCarat;
        foreach (var r in rs_5) TotalCarat += r;

        StoneWeight = TotalCarat * (decimal)0.2;
        NettoWeight = GrossWeight - StoneWeight;

        return NettoWeight;
    }


    public decimal RecalWeightTitipan(Decimal GrossWeight, StockProductDJCustomer d)
    {
        decimal NettoWeight = 0m;
        decimal TotalCarat = 0m;
        decimal StoneWeight = 0m;

        //Stone
        var rs_1A = from p in d.StockProductDJCustomer_Stone1A select p.TotalCarat;
        foreach (var r in rs_1A) TotalCarat += r;

        var rs_1B = from p in d.StockProductDJCustomer_Stone1B select p.TotalCarat;
        foreach (var r in rs_1B) TotalCarat += r;

        var rs_2 = from p in d.StockProductDJCustomer_Stone2 select p.TotalCarat;
        foreach (var r in rs_2) TotalCarat += r;

        var rs_3 = from p in d.StockProductDJCustomer_Stone3 select p.TotalCarat;
        foreach (var r in rs_3) TotalCarat += r;

        var rs_4 = from p in d.StockProductDJCustomer_Stone4 select p.TotalCarat;
        foreach (var r in rs_4) TotalCarat += r;

        var rs_5 = from p in d.StockProductDJCustomer_Stone5 select p.TotalCarat;
        foreach (var r in rs_5) TotalCarat += r;

        StoneWeight = TotalCarat * (decimal)0.2;
        NettoWeight = GrossWeight - StoneWeight;

        return NettoWeight;
    }

    protected string validateFillDJ()
    {
        string message = "";
        if (string.IsNullOrEmpty(petugasqc.Text))
        {
            message = "Petugas QC wajib diinput.";
            petugasqc.Focus();
        }

        if (string.IsNullOrEmpty(message))
        {
            string netweightactual = String.Format("{0:0.00}", grossweight.Text.Trim());
            decimal netweight1 = 0m;
            bool isnumber = decimal.TryParse(netweightactual, out netweight1);
            if (!isnumber)
            {
                message = "Berat harus berupa angka / nominal.";
                grossweight.Focus();
            }
        }

        if (string.IsNullOrEmpty(message))
        {
            if(!(Convert.ToInt32(tipe_lokasi.SelectedValue) >= 0))
            {
                message = "Tipe Lokasi wajib dipilih";
                tipe_lokasi.Focus();
            }
        }

        if (string.IsNullOrEmpty(message))
        {
            if(!(Convert.ToInt32(lokasi.SelectedValue) > 0))
            {
                message = "Lokasi wajib dipilih";
                lokasi.Focus();
            }
        }

        return message;
    }

    protected string setJavascriptKeyPressStone(string idtextboxbutiract, string idtextboxcaratact, string idtextboxbutir, string idtextboxcarat)
    {
        string x = string.Empty;
        if (idtextboxbutir.StartsWith("b1A"))
        {
            x += "function onkeypressStone1A" + idtextboxbutir + "(){ ";
        }
        else if (idtextboxbutir.StartsWith("b1B"))
        {
            x += "function onkeypressStone1B" + idtextboxbutir + "(){ ";
        }
        else if (idtextboxbutir.StartsWith("b2"))
        {
            x += "function onkeypressStone2" + idtextboxbutir + "(){ ";
        }
        else if (idtextboxbutir.StartsWith("b3"))
        {
            x += "function onkeypressStone3" + idtextboxbutir + "(){ ";
        }
        else if (idtextboxbutir.StartsWith("b4"))
        {
            x += "function onkeypressStone4" + idtextboxbutir + "(){ ";
        }
        else if (idtextboxbutir.StartsWith("b5"))
        {
            x += "function onkeypressStone5" + idtextboxbutir + "(){ ";
        }

        x += "var textboxbutir = document.getElementById('" + idtextboxbutir + "').value; ";
        x += "var textboxcaratact = document.getElementById('" + idtextboxcaratact + "').innerText; ";
        x += "var textboxbutiract = document.getElementById('" + idtextboxbutiract + "').innerText; ";
        x += "var textholdbutir = textboxbutiract; ";
        x += "var textholdcarat = textboxcaratact;";
        x += "var rataratacarat = parseFloat(parseFloat(textboxcaratact.toString().replace(',','.')) / parseInt(textboxbutiract)); ";
        x += "var totalcarat = rataratacarat * parseInt(textboxbutir); ";
        x += "document.getElementById('" + idtextboxcarat + "').value = totalcarat.toFixed(3).toString().replace('.',','); ";
        x += "if(textboxbutir.toString() == ''){ document.getElementById('" + idtextboxcarat + "').value = textholdcarat; document.getElementById('" + idtextboxbutir + "').value = textholdbutir; }; ";
        x += "}";
        return x;
    }

    //Execute Command
    protected void Execute(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = cmdType;
            foreach (var x in param)
            {
                cmd.Parameters.Add(x);
            }
            cmd.CommandTimeout = 300; // 300 sec or 5 mins for long time query
            cmd.ExecuteNonQuery();
        }
    }
}