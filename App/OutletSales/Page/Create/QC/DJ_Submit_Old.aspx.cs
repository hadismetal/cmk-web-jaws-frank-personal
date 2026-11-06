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

        if (!IsPostBack)
        {
            fillForm();
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

            //Picture Repair
            Upload.Image(imgpicture, d.ImgPicture);

            fillStone();
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
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone1A.Parcel101.Nama  + " "  + r.Stone1A.Parcel102.Nama;
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

            TextBox txt = new TextBox();
            txt.ID = "s1A" + index.ToString();
            txt.Attributes["style"] = "width: 200px;";
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
            td.Text = r.Stone1B.Parcel101.Nama + " " + r.Stone1B.Parcel102.Nama;
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

            TextBox txt = new TextBox();
            txt.ID = "s1B" + index.ToString();
            txt.Attributes["style"] = "width: 200px;";
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
            td.Text = r.Stone2.Parcel201.Nama + " " + r.Stone2.Parcel202.Nama;
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

            TextBox txt = new TextBox();
            txt.ID = "s2" + index.ToString();
            txt.Attributes["style"] = "width: 200px;";
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
            td.Text = r.Stone3.Parcel301.Nama + " " + r.Stone3.Parcel302.Nama;
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

            TextBox txt = new TextBox();
            txt.ID = "s3" + index.ToString();
            txt.Attributes["style"] = "width: 200px;";
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
            td.Text = r.Stone4.Parcel401.Nama + " " + r.Stone4.Parcel402.Nama;
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

            TextBox txt = new TextBox();
            txt.ID = "s4" + index.ToString();
            txt.Attributes["style"] = "width: 200px;";
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
            td.Text = r.Stone5.Parcel501.Nama + " " + r.Stone5.Parcel502.Nama;
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

            TextBox txt = new TextBox();
            txt.ID = "s5" + index.ToString();
            txt.Attributes["style"] = "width: 200px;";
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

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
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
            x.Blemish = blemish.Text;
            x.JumlahButir = jlhbutir.Text;
            x.Sertifikat = sertifikat.Text;
            x.NonCZ = noncz.Text;
            x.CZ = cz.Text;
            x.Logo = logo.Text;
            x.KadarEmas = kadaremas.Text;
            x.Merk = merk.Text;
            x.FisikKeseluruhan = fisik.Text;
            x.GrossWeight = GW;
            x.NetWeight = Str.Number2(NW);
            x.WarnaEmas = framecolor.Text;
            x.OperatorNama = App.UserNama;
            x.OperatorTgl = DateTime.Now;
            x.Draft = false;
            x.DraftDate = null;

            db.DocQCDJ.InsertOnSubmit(x);
            db.SubmitChanges();

            saveStone(x);

            //Redirect
            Response.Redirect("/OutletSales/Page/Report/QCDJ.aspx?refreshPage=1&kw=" + NomorQC + "&dari=" + Str.Tgl(DateTime.Today) + "&sampai=" + Str.Tgl(DateTime.Today) + "");
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


        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfs1A" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("s1A" + index.ToString());

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


        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfs1B" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("s1B" + index.ToString());

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


        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfs2" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("s2" + index.ToString());

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


        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfs3" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("s3" + index.ToString());

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


        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfs4" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("s4" + index.ToString());

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


        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfs5" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("s5" + index.ToString());

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
}