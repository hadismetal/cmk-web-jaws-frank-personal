using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_Resell_Item : System.Web.UI.Page
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
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //HyperLink
            add.HRef += "?id=" + myID;
        }
        fillTable(sender, e);
    }

    //Exception
    protected void DataError() { Response.Redirect("Resell.aspx"); }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var d = db.Resell.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            decimal GT = 0m;

            //Item
            fillItemDJ(ref GT);
            fillItemPG(ref GT);
            fillItemGJ(ref GT);
            fillItemLD(ref GT);

            //Grand-Total
            Sub_Total(GT, true);
            tb.Focus();

            string error = string.Empty;
            error = ValidateResell();

            if (error == string.Empty) 
            {
                error = checkDocumentQC(d);
            }

            if (!string.IsNullOrEmpty(error))
            {
                divlblwarning.Visible = true;
                lblwarning.Visible = true;
                lblwarning.InnerText = error;
                save.Visible = false;
            }
            else 
            {
                divlblwarning.Visible = false;
                lblwarning.Visible = false;
                lblwarning.InnerText = "";
                save.Visible = true;
            }
        }
    }

    protected void Price_TextChanged(object sender, EventArgs e)
    {
        calc();

        string href = App.RefreshURL(this);
        //Page-Qstr
        href += "&id=" + myID;
        href += "&tb=" + ((TextBox)sender).ClientID;
        Response.Redirect(href);
    }

    protected void calc()
    {
        calcDJ();
        calcPG();
        calcGJ();
        calcLD();
    }
    protected void calcDJ()
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {
            decimal HargaBeli = 0m;
            try
            {
                hf = ((HiddenField)row.FindControl("hfdj" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("dj_" + index.ToString());
                var d = db.ResellDJ.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {
                        var pr = db.SalesOrderDJ.SingleOrDefault(p => p.IDProduct == d.IDProduct &&
                            p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                        HargaBeli = Convert.ToDecimal(txt.Text);

                        d.HargaRupiah = HargaBeli;

                        db.SubmitChanges();
                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
        }
    }
    protected void calcPG()
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";


        foreach (TableRow row in tb.Rows)
        {
            decimal HargaBeli = 0m;
            try
            {
                hf = ((HiddenField)row.FindControl("hfpg" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("pg_" + index.ToString());
                var d = db.ResellPG.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {

                        HargaBeli = Convert.ToDecimal(txt.Text);
                        //if (HargaBeli > d.HargaAcuan.Value)
                        //HargaBeli = d.HargaAcuan.Value;

                        d.HargaRupiah = HargaBeli;

                        db.SubmitChanges();
                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
        }
    }
    protected void calcGJ()
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";


        foreach (TableRow row in tb.Rows)
        {
            decimal HargaBeli = 0m;
            try
            {
                hf = ((HiddenField)row.FindControl("hfgj" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("gj_" + index.ToString());
                var d = db.ResellGJ.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {

                        var pr = db.SalesOrderGJ.SingleOrDefault(p => p.IDProduct == d.IDProduct &&
                            p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                        HargaBeli = Convert.ToDecimal(txt.Text);
                        //if (HargaBeli > d.HargaAcuan.Value && d.SalesOrder.Tgl.Value.AddMonths(pr.R2_PeriodeMin.Value) > DateTime.Today)
                        //    HargaBeli = d.HargaAcuan.Value;

                        d.HargaRupiah = HargaBeli;

                        db.SubmitChanges();
                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
        }
    }
    protected void calcLD()
    {
        int index = 1;

        TextBox txt = new TextBox();
        string hf = "";


        foreach (TableRow row in tb.Rows)
        {
            decimal HargaBeli = 0m;
            try
            {
                hf = ((HiddenField)row.FindControl("hfld" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("ld_" + index.ToString());
                var d = db.ResellLD.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (d != null)
                {
                    try
                    {

                        var pr = db.SalesOrderLD.SingleOrDefault(p => p.IDProduct == d.IDProduct &&
                            p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                        HargaBeli = Convert.ToDecimal(txt.Text);

                        d.HargaRupiah = HargaBeli;

                        db.SubmitChanges();
                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex) { }
            index++;
        }
    }
    //Item
    protected void fillItemDJ(ref decimal GT)
    {
        var rs = from p in db.ResellDJ
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(0);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(0, r.ID, r.IDProduct);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaAcuan.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DocQC(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "dj_" + index.ToString();
            txt.Attributes["style"] = "width: 130px;";
            txt.Attributes["class"] = "form-control text-right";
            txt.Attributes["type"] = "text";
            txt.Text = r.HargaRupiah.HasValue ? Convert.ToInt64(r.HargaRupiah.Value).ToString("N0") : Convert.ToInt64(r.HargaAcuan.Value).ToString("N0");
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
            txt.TextChanged += new EventHandler(Price_TextChanged);

            HiddenField hf = new HiddenField();
            hf.ID = "hfdj" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT += ST;

            Sub_Total(ST, false);
        }
    }
    protected void fillItemPG(ref decimal GT)
    {
        var rs = from p in db.ResellPG
                 where p.IDForm == myID
                 select p;

        int index = 1;
        int validatepg = 0;

        decimal ST = 0;

        //Sub-Header
        if (rs.Count() > 0)
        {
            Sub_Header(1);
        }

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(1, r.ID, r.IDProduct);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.Nomor + "<br /><p>(" + r.StockProductPG.CharGoldLevel.Nama + ")</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaAcuan.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DocQC(r);
            tr.Cells.Add(td);

            string querygetberattimbang = "SELECT TOP 1 ISNULL(BeratTimbang, 0) [BeratTimbang] FROM ResellPG WHERE ID = @ID";
            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@ID", SqlDbType.Int) { Value = r.ID } };
            decimal BeratTimbang = SingleDecimal(querygetberattimbang, Db.Cnn_DB, parameters, CommandType.Text);

            td = new TableCell();
            td.Text = BeratTimbang > 0 ? BeratTimbang.ToString("N2") : Convert.ToDecimal(r.StockProductPG.GrossWeight).ToString("N2");
            tr.Cells.Add(td);

            decimal TGP = GetTGPPGBeliNew(r.StockProductPG.ID);
            td = new TableCell();
            td.Text = TGP.ToString("N0");
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "pg_" + index.ToString();
            txt.Attributes["style"] = "width: 130px;";
            txt.Attributes["class"] = "form-control text-right";
            txt.Attributes["type"] = "text";
            txt.Text = r.HargaRupiah.HasValue ? Convert.ToInt64(r.HargaRupiah.Value).ToString("N0") : Convert.ToInt64(r.HargaAcuan.Value).ToString("N0");
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
            txt.TextChanged += new EventHandler(Price_TextChanged);

            HiddenField hf = new HiddenField();
            hf.ID = "hfpg" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT += ST;

            Sub_Total(ST, false);
        }

        if (validatepg > 0)
        {
            save.Disabled = true;
        }
    }
    protected void fillItemGJ(ref decimal GT)
    {
        var rs = from p in db.ResellGJ
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(2);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(2, r.ID, r.IDProduct);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductGJ.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaAcuan.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DocQC(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "gj_" + index.ToString();
            txt.Attributes["style"] = "width: 130px;";
            txt.Attributes["class"] = "form-control text-right";
            txt.Attributes["type"] = "text";
            txt.Text = r.HargaRupiah.HasValue ? Convert.ToInt64(r.HargaRupiah.Value).ToString("N0") : Convert.ToInt64(r.HargaAcuan.Value).ToString("N0");
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
            txt.TextChanged += new EventHandler(Price_TextChanged);

            HiddenField hf = new HiddenField();
            hf.ID = "hfgj" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT += ST;

            Sub_Total(ST, false);
        }
    }

    protected void fillItemLD(ref decimal GT)
    {
        var rs = from p in db.ResellLD
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(3);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(3, r.ID, r.IDProduct);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductLD_Stone1B.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaAcuan.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DocQC(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "ld_" + index.ToString();
            txt.Attributes["style"] = "width: 130px;";
            txt.Attributes["class"] = "form-control text-right";
            txt.Attributes["type"] = "text";
            txt.Text = r.HargaRupiah.HasValue ? Convert.ToInt64(r.HargaRupiah.Value).ToString("N0") : Convert.ToInt64(r.HargaAcuan.Value).ToString("N0");
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
            txt.TextChanged += new EventHandler(Price_TextChanged);

            HiddenField hf = new HiddenField();
            hf.ID = "hfld" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT += ST;

            Sub_Total(ST, false);
        }
    }

    //Custom Row
    protected void Sub_Header(int ItemIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.ItemIndex(ItemIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_Total(decimal Total, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 8;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p id=\"grand-total\" class=\"text-lg\">" + Str.Number0(Total) + "</p>";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);


        tb.Rows.Add(tr);
    }

    //Display Column
    protected string TD_HyperLink(int ItemIndex, int ID, int IDProduct)
    {
        string x = string.Empty;
        string href = string.Empty;

        x += "<div class=\"btn-group\">";
        x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + myID + "', '" + ItemIndex + "','" + ID + "')\">";
        x += "      <i class=\"icon-trash\"></i> ";
        x += "      Delete";
        x += "  </a>";
        x += "</div>";
        if (ItemIndex == 0)
        {
            var d = db.StockProductDJ.SingleOrDefault(p => p.ID == IDProduct);
            if (d.Legacy != 1)
            {
                x += "&nbsp;<div class=\"btn-group\">";
                x += "  <a target=\"_blank\" class=\"btn btn-info\" href=\"DJ_Detail.aspx?id=" + IDProduct + "\">";
                x += "      <i class=\"icon-search\"></i> ";
                x += "      Info";
                x += "  </a>";
            }
        }
        else if (ItemIndex == 1)
        {
            x += "&nbsp;<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-info\" href=\"PG_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Info";
            x += "  </a>";
        }
        else if (ItemIndex == 2)
        {
            x += "&nbsp;<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-info\" href=\"GJ_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Info";
            x += "  </a>";
        }
        else if (ItemIndex == 3)
        {
            x += "&nbsp;<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-info\" href=\"LD_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Info";
            x += "  </a>";
        }
        x += "</div>";

        return x;
    }
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

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        string status_SAP = "";
        string status_Resell = "";

        var d = db.Resell.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            string store = "";
            string errormsg = "";
            if (d.TipeLokasi == 1)
                store = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi).Kode;
            else if (d.TipeLokasi == 2)
                store = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi).Kode;
            
            errormsg = ValidateResell();
            //if(string.IsNullOrEmpty(errormsg))
            //errormsg = insertResellReceipt(Convert.ToInt32(payment.SelectedValue), nominal.Text, Convert.ToInt32(bank.SelectedValue), acnumber.Text, acname.Text, keterangan.Text, "");
            if (string.IsNullOrEmpty(errormsg))
            {
                string NewDocumentNumber = string.Empty;
                // Generate Document Number
                if (d.Nomor == null || d.Nomor == "")
                {
                    NewDocumentNumber = Document.Generate_Nomor_Resell(d);
                    d.Nomor = NewDocumentNumber;
                    db.SubmitChanges();
                }

                saveResell(d.ID, NewDocumentNumber, App.UserNama);

                // Validation and SAP Posting
                /*try
                {
                    status_Resell += ((status_Resell == "") ? checkDocumentQC(d) : "");

                    if (status_Resell == "")
                    {

                        if (SAP.xActivate)
                        {
                            //status_SAP += ((status_SAP == "") ? SAPM.prePostResellIntegration(d) : "");
                            status_SAP += ((status_SAP == "") ? SAPM.isValidLocationSAP(d) : "");
                            status_SAP += ((status_SAP == "") ? SAPM.postCustomerToSAP(d) : "");
                            status_SAP += ((status_SAP == "") ? SAPM.postResellGoodsToSAP(d) : "");
                        }

                    }

                }
                catch (Exception ex)
                {
                    status_SAP = "Terjadi kesalahan teknis. Silahkan hubungi IT Administrator. (Error message : "
                        + ex.Message
                        + ex.StackTrace
                        + ex.InnerException
                        + ")";
                }*/

                //status_Resell = "";  // for dev only, disable enable Resell validation
                status_SAP = ""; // for dev only, disable enable SAP validation
                if (status_SAP == "" && status_Resell == "")
                {
                    /*if (d.ResellDJ.Count() > 0)
                    {
                        foreach (var r in d.ResellDJ)
                        {
                            var x = db.SalesOrderDJ.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                            if (x != null)
                            {
                                x.StockProductDJ.StockActualDJ.Substorage = 2;
                                x.StockProductDJ.StockActualDJ.InHand = 1;
                                x.StatusResell = 1;
                                x.StockProductDJ.StockActualDJ.TipeLokasi = d.TipeLokasi;
                                x.StockProductDJ.StockActualDJ.IDLokasi = d.IDLokasi;
                                db.SubmitChanges();

                                StockLedgerDJ l = new StockLedgerDJ();

                                // Value (See Library DB - Ledger for Value and Remark)
                                decimal Value = 1; // 1 : in , -1 : out
                                int Remark = 11; // See Library DB - Ledger
                                string Keterangan = d.Nomor; // Commonly filled by document number or id 
                                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                                string Lokasi = "";
                                if (d.TipeLokasi == 1)
                                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
                                else if (d.TipeLokasi == 2)
                                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

                                //Input
                                l.IDProduct = r.IDProduct;
                                l.TipeLokasi = d.TipeLokasi;
                                l.IDLokasi = d.IDLokasi;
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
                            }
                        }
                    }

                    if (d.ResellPG.Count() > 0)
                    {
                        foreach (var r in d.ResellPG)
                        {
                            var x = db.SalesOrderPG.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                            if (x != null)
                            {
                                x.StockProductPG.StockActualPG.Substorage = 2;
                                x.StockProductPG.StockActualPG.InHand = 1;
                                x.StatusResell = 1;
                                x.StockProductPG.StockActualPG.TipeLokasi = d.TipeLokasi;
                                x.StockProductPG.StockActualPG.IDLokasi = d.IDLokasi;
                                db.SubmitChanges();

                                // Ledger 
                                StockLedgerPG l = new StockLedgerPG();

                                // Value (See Library DB - Ledger for Value and Remark)
                                decimal Value = 1; // 1 : in , -1 : out
                                int Remark = 11; // See Library DB - Ledger
                                string Keterangan = d.Nomor; // Commonly filled by document number or id 
                                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                                string Lokasi = "";
                                if (d.TipeLokasi == 1)
                                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
                                else if (d.TipeLokasi == 2)
                                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

                                //Input
                                l.IDProduct = r.IDProduct;
                                l.TipeLokasi = d.TipeLokasi;
                                l.IDLokasi = d.IDLokasi;
                                l.NamaLokasi = Lokasi;
                                l.Value = Value;
                                l.Remark = Remark;
                                l.Keterangan = Keterangan;

                                //User-Info
                                l.Operator = App.UserNama;
                                l.OperatorTgl = DateTime.Now;

                                //Commit
                                db.StockLedgerPG.InsertOnSubmit(l);
                                db.SubmitChanges();

                                int Consignment = 0;
                                Consignment = Db.SingleInteger("SELECT ISNULL(CAST(Consignment AS INT),0) Consignemtn from StockProductPG WHERE ID=" + r.IDProduct, Db.Cnn_DB);

                                if (Consignment == 1)
                                {
                                    string query = "UPDATE StockProductPG SET Consignment = @consignment WHERE ID = @idproduct";
                                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
                                    {
                                        // Adding parameters with values                                                            
                                        SqlCommand cmd = conn.CreateCommand();
                                        cmd.CommandType = System.Data.CommandType.Text;
                                        cmd.CommandText = query;
                                        cmd.Parameters.AddWithValue("@consignment", 0);
                                        cmd.Parameters.AddWithValue("@idproduct", r.IDProduct);

                                        StockLedgerPG m = new StockLedgerPG();

                                        m.IDProduct = r.IDProduct;
                                        m.TipeLokasi = d.TipeLokasi;
                                        m.IDLokasi = d.IDLokasi;
                                        m.NamaLokasi = Lokasi;
                                        m.Value = 0;
                                        m.Remark = 22;
                                        m.Keterangan = x.StockProductPG.Nomor;

                                        //User-Info
                                        m.Operator = App.UserNama;
                                        m.OperatorTgl = DateTime.Now;

                                        //Commit
                                        db.StockLedgerPG.InsertOnSubmit(m);
                                        db.SubmitChanges();
                                        try
                                        {
                                            conn.Open();
                                            cmd.ExecuteNonQuery();
                                            conn.Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("Error: " + ex.Message);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (d.ResellGJ.Count() > 0)
                    {
                        foreach (var r in d.ResellGJ)
                        {
                            var x = db.SalesOrderGJ.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                            if (x != null)
                            {
                                x.StockProductGJ.StockActualGJ.Substorage = 2;
                                x.StockProductGJ.StockActualGJ.InHand = 1;
                                x.StatusResell = 1;
                                x.StockProductGJ.StockActualGJ.TipeLokasi = d.TipeLokasi;
                                x.StockProductGJ.StockActualGJ.IDLokasi = d.IDLokasi;
                                db.SubmitChanges();

                                // Ledger 
                                StockLedgerGJ l = new StockLedgerGJ();

                                // Value (See Library DB - Ledger for Value and Remark)
                                decimal Value = 1; // 1 : in , -1 : out
                                int Remark = 11; // See Library DB - Ledger
                                string Keterangan = d.Nomor; // Commonly filled by document number or id 
                                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                                string Lokasi = "";
                                if (d.TipeLokasi == 1)
                                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
                                else if (d.TipeLokasi == 2)
                                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

                                //Input
                                l.IDProduct = r.IDProduct;
                                l.TipeLokasi = d.TipeLokasi;
                                l.IDLokasi = d.IDLokasi;
                                l.NamaLokasi = Lokasi;
                                l.Value = Value;
                                l.Remark = Remark;
                                l.Keterangan = Keterangan;

                                //User-Info
                                l.Operator = App.UserNama;
                                l.OperatorTgl = DateTime.Now;

                                //Commit
                                db.StockLedgerGJ.InsertOnSubmit(l);
                                db.SubmitChanges();
                            }
                        }
                    }

                    if (d.ResellLD.Count() > 0)
                    {
                        foreach (var r in d.ResellLD)
                        {
                            var x = db.SalesOrderLD.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                            if (x != null)
                            {
                                x.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage = 2;
                                x.StockProductLD_Stone1B.StockActualLD_Stone1B.InHand = 1;
                                x.StatusResell = 1;
                                x.StockProductLD_Stone1B.StockActualLD_Stone1B.TipeLokasi = d.TipeLokasi;
                                x.StockProductLD_Stone1B.StockActualLD_Stone1B.IDLokasi = d.IDLokasi;
                                db.SubmitChanges();

                                // Ledger 
                                StockLedgerLD l = new StockLedgerLD();

                                // Value (See Library DB - Ledger for Value and Remark)
                                decimal Value = 1; // 1 : in , -1 : out
                                int Remark = 11; // See Library DB - Ledger
                                string Keterangan = d.Nomor; // Commonly filled by document number or id 
                                if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                                string Lokasi = "";
                                if (d.TipeLokasi == 1)
                                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
                                else if (d.TipeLokasi == 2)
                                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

                                //Input
                                l.IDProduct = r.IDProduct;
                                l.TipeLokasi = d.TipeLokasi;
                                l.IDLokasi = d.IDLokasi;
                                l.NamaLokasi = Lokasi;
                                l.Value = Value;
                                l.Remark = Remark;
                                l.Keterangan = Keterangan;

                                //User-Info
                                l.Operator = App.UserNama;
                                l.OperatorTgl = DateTime.Now;

                                //Commit
                                db.StockLedgerLD.InsertOnSubmit(l);
                                db.SubmitChanges();
                            }
                        }
                    }

                    d.Draft = false;
                    d.DraftDate = null;
                    d.TotalHarga = d.ResellDJ.Sum(p => p.HargaRupiah.Value) + d.ResellPG.Sum(p => p.HargaRupiah.Value) + d.ResellGJ.Sum(p => p.HargaRupiah.Value) + d.ResellLD.Sum(p => p.HargaRupiah.Value);
                    db.SubmitChanges();
                    */
                    Response.Redirect("Print_Receipt.aspx?id=" + myID);
                }
                else
                {
                    if (status_Resell != "")
                    {
                        lit_error_resell.Text += "<div class='alert alert-danger' role='alert'>" + status_Resell + "</div>";
                    }

                    if (status_SAP != "")
                    {
                        lit_error_sap.Text += "<div class='alert alert-danger' role='alert'>" + status_SAP + "</div>";
                    }
                    divlblwarning.Visible = false;
                    lblwarning.Visible = false;
                    lblwarning.InnerText = "";
                }
            }
            else
            {
                divlblwarning.Visible = true;
                lblwarning.Visible = true;
                lblwarning.InnerText = errormsg;
            }
        }
    }

    protected string saveResell(int id, string noro, string operatornama)
    {
        string result = string.Empty;
        string query = "sp_saveResell";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = id },
            new SqlParameter("@NO_RESELL", SqlDbType.VarChar){ Value = noro },
            new SqlParameter("@OPERATOR", SqlDbType.VarChar){ Value = operatornama }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
        result = dt.Rows[0]["MESSAGE"].ToString();
        return result;
    }

    protected string checkDocumentQC(Resell d)
    {
        string msg = "";
        string plu = "";

        bool check = false;

        var dj = from p in d.ResellDJ
                 where p.IDDocQC == null
                 select p;
        if (dj.Count() > 0)
        {
            foreach (var p in dj)
            {
                plu += p.StockProductDJ.Nomor + ",";
            }
            check = true;
        }

        var pg = from p in d.ResellPG
                 where p.IDDocQC == null
                 select p;
        if (pg.Count() > 0)
        {
            foreach (var p in pg)
            {
                plu += p.StockProductPG.Nomor + ",";
            }
            check = true;
        }

        var gj = from p in d.ResellGJ
                 where p.IDDocQC == null
                 select p;
        if (gj.Count() > 0)
        {
            foreach (var p in gj)
            {
                plu += p.StockProductGJ.Nomor + ",";
            }
            check = true;
        }

        var ld = from p in d.ResellLD
                 where p.IDDocQC == null
                 select p;
        if (ld.Count() > 0)
        {
            foreach (var p in ld)
            {
                plu += p.StockProductLD_Stone1B.Nomor + ",";
            }
            check = true;
        }

        if (check == true)
        {
            if (!string.IsNullOrEmpty(plu) && plu.Length > 0)
            {
                plu = plu.Substring(0, plu.Length - 1);
            }
            msg = "Dokumen QC " + plu + " belum dipilih";
        }

        return msg;
    }

    protected string DocQC(ResellPG r)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (r.IDDocQC == null)
        {
            x += "<div class=\"btn-group btn-sm\">";
            x += "  <a class=\"btn btn-info btn-sm\" href=\"Resell_AddDocQC.aspx?id=" + myID + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=2\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Add Doc QC";
            x += "  </a>";
            x += "</div>";
        }
        else
        {
            x += "  <a class=\"btn btn-info btn-sm\" href=\"Resell_AddDocQC.aspx?id=" + myID + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=2\">";
            x += r.DocQCPG.Nomor;
            x += "  </a>";
        }

        return x;
    }

    protected string DocQC(ResellDJ r)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (r.IDDocQC == null)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-info btn-sm\" href=\"Resell_AddDocQC.aspx?id=" + myID + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=1\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Add Doc QC";
            x += "  </a>";
            x += "</div>";
        }
        else
        {
            x += "  <a class=\"btn btn-info btn-sm\" href=\"Resell_AddDocQC.aspx?id=" + myID + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=1\">";
            x += r.DocQCDJ.Nomor;
            x += "  </a>";
        }

        return x;
    }

    protected string DocQC(ResellGJ r)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (r.IDDocQC == null)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-info btn-sm\" href=\"Resell_AddDocQC.aspx?id=" + myID + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=3\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Add Doc QC";
            x += "  </a>";
            x += "</div>";
        }
        else
        {
            x += "  <a class=\"btn btn-info btn-sm\" href=\"Resell_AddDocQC.aspx?id=" + myID + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=3\">";
            x += r.DocQCGJ.Nomor;
            x += "  </a>";
        }

        return x;
    }
    protected string DocQC(ResellLD r)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (r.IDDocQC == null)
        {
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-info btn-sm\" href=\"Resell_AddDocQC.aspx?id=" + myID + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=4\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Add Doc QC";
            x += "  </a>";
            x += "</div>";
        }
        else
        {
            x += "  <a class=\"btn btn-info btn-sm\" href=\"Resell_AddDocQC.aspx?id=" + myID + "&rid=" + r.ID + "&itemid=" + r.IDProduct + "&tipe=4\">";
            x += r.DocQCLD.Nomor;
            x += "  </a>";
        }

        return x;
    }

    protected decimal GetTGPPGBeliNew(int IDProduct)
    {
        decimal GoldPrice = 0m;

        string query = "sp_get_tgp_jual_beli_plu";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID_PRODUCT", SqlDbType.Int){ Value = IDProduct },
            new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = "PG" }
        };

        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
        if (dt.Rows.Count == 1)
        {
            GoldPrice = Convert.ToDecimal(dt.Rows[0]["HARGA_BELI"]);
        }

        return GoldPrice;
    }

    protected decimal GetTGPPGBeli(StockProductPG r)
    {
        decimal GoldPrice = 0m;
        DataTable dt = Db.Rs("EXEC sp_get_tgp_jual_beli ", Db.Cnn_DB);
        if (dt.Rows.Count > 0)
        {
            if (r.CharGoldLevel.ID == 21)
            {
                //Preloved Good 37,5 % PLGPG375
                GoldPrice = Convert.ToDecimal(dt.Rows[13][2]);
            }
            else if (r.CharGoldLevel.ID == 22)
            {
                //Preloved Excellent 37,5 % PLEPG375
                GoldPrice = Convert.ToDecimal(dt.Rows[14][2]);
            }
            else if (r.CharGoldLevel.ID == 23)
            {
                //Preloved Good 75 % PLGPG75
                GoldPrice = Convert.ToDecimal(dt.Rows[15][2]);
            }
            else if (r.CharGoldLevel.ID == 24)
            {
                //Preloved Excellent 75 % PLEPG75
                GoldPrice = Convert.ToDecimal(dt.Rows[16][2]);
            }
            else if (r.CharGoldLevel.ID == 25)
            {
                //Preloved Good CZ 37,5 % PLGCZ375
                GoldPrice = Convert.ToDecimal(dt.Rows[17][2]);
            }
            else if (r.CharGoldLevel.ID == 26)
            {
                //Preloved Excellent CZ 37,5 % PLECZ375
                GoldPrice = Convert.ToDecimal(dt.Rows[18][2]);
            }
            else if (r.CharGoldLevel.ID == 27)
            {
                //Preloved Good CZ 75 % PLGCZ75
                GoldPrice = Convert.ToDecimal(dt.Rows[19][2]);
            }
            else if (r.CharGoldLevel.ID == 28)
            {
                //Preloved Excellent CZ 75 % PLECZ75
                GoldPrice = Convert.ToDecimal(dt.Rows[20][2]);
            }
            else if (r.CharGoldLevel.ID == 13)
            {
                //CZC 37,5
                GoldPrice = Convert.ToDecimal(dt.Rows[11][2]);
            }
            /*else if(r.CharGoldLevel.ID == 14)/
			{
				//PGC 37,5
				GoldPrice = Convert.ToDecimal(dt.Rows[10][2]);
			}
			*/
            //Update tgl 19 Oktober 2024, mappingan sama dengan CZC 37,5
            else if (r.CharGoldLevel.ID == 14)
            {
                //PGC 37,5
                GoldPrice = Convert.ToDecimal(dt.Rows[11][2]);
            }
            else if (r.CharGoldLevel.ID == 8 && r.CharFrameColor.ID == 2 && r.CharProductItem.ID == 59) // chain white 37.5
            {
                //IDPricing = 36; //PG / CHAIN KADAR 37,5 - Pricing
                GoldPrice = Convert.ToDecimal(dt.Rows[0][2].ToString());
            }
            else if (r.CharGoldLevel.ID == 8 && r.CharProductItem.ID != 59) // non chain 37.5
            {
                //IDPricing = 36; //PG / CHAIN KADAR 37,5 - Pricing
                GoldPrice = Convert.ToDecimal(dt.Rows[12][2].ToString());
            }
            else if (r.CharGoldLevel.ID == 6 || r.CharGoldLevel.ID == 7 || r.CharGoldLevel.ID == 9) // 6 = zirconia 37,5 ; 7 = zirconia 75 
            {
                if (r.CharGoldLevel.ID == 6)
                {
                    //IDPricing = 34; 
                    GoldPrice = Convert.ToDecimal(dt.Rows[8][2].ToString());
                }
                else if (r.CharGoldLevel.ID == 7)
                {
                    //IDPricing = 35; 
                    GoldPrice = Convert.ToDecimal(dt.Rows[6][2].ToString());
                }
                else if (r.CharGoldLevel.ID == 9)
                {
                    GoldPrice = Convert.ToDecimal(dt.Rows[7][2].ToString());
                }
            }
            else if (r.KadarLogam >= 99m)
            {
                if (r.ProductItem == 59) // 59 = Chain
                    GoldPrice = Convert.ToDecimal(dt.Rows[5][2].ToString());//IDPricing = 15;
                else
                    GoldPrice = Convert.ToDecimal(dt.Rows[5][2].ToString());//IDPricing = 16;
            }
            else
            {
                if (r.ProductItem != 59) // 59 = Chain
                {
                    if (r.FrameColor == 1)
                        GoldPrice = Convert.ToDecimal(dt.Rows[3][2].ToString());//IDPricing = 11;
                    else
                        GoldPrice = Convert.ToDecimal(dt.Rows[4][2].ToString());//IDPricing = 12;
                }
                else
                {
                    if (r.FrameColor == 1)
                        GoldPrice = Convert.ToDecimal(dt.Rows[1][2].ToString());//IDPricing = 13;
                    else
                        GoldPrice = Convert.ToDecimal(dt.Rows[2][2].ToString());//IDPricing = 14;
                }
            }
        }

        return GoldPrice;
    }

    protected string ValidateResell()
    {
        string msg = "";
        int countitem = 0;
        var rs = db.Resell.SingleOrDefault(p => p.ID == myID);
        if (rs != null)
        {
            foreach (var dj in rs.ResellDJ)
            {
                countitem++;
            }
            foreach (var dj in rs.ResellPG)
            {
                countitem++;
            }
            foreach (var dj in rs.ResellGJ)
            {
                countitem++;
            }
            foreach (var dj in rs.ResellLD)
            {
                countitem++;
            }

            if (countitem == 0)
            {
                msg = "Barang Resell belum diinput.";
            }
        }

        if (string.IsNullOrEmpty(msg))
        {
            bool ispositive = checkNilaiResell(myID);
            if (!ispositive)
                msg = msg + "Harga Resell tidak boleh kurang dari 0 Rupiah.";
        }

        return msg;
    }

    protected bool checkNilaiResell(int idro)
    {
        bool status = false;
        decimal totalsales = 0m;
        string query = "DECLARE @SUMTOTAL MONEY = 0 " +
            "IF EXISTS(SELECT 1 FROM ResellDJ WHERE IDForm = @ID) " +
            "BEGIN SELECT @SUMTOTAL = @SUMTOTAL + SUM(ISNULL(HargaRupiah,0)) FROM ResellDJ WHERE IDForm = @ID END " +
            "IF EXISTS(SELECT 1 FROM ResellPG WHERE IDForm = @ID) " +
            "BEGIN SELECT @SUMTOTAL = @SUMTOTAL + SUM(ISNULL(HargaRupiah,0)) FROM ResellPG WHERE IDForm = @ID END " +
            "IF EXISTS(SELECT 1 FROM ResellGJ WHERE IDForm = @ID) " +
            "BEGIN SELECT @SUMTOTAL = @SUMTOTAL + SUM(ISNULL(HargaRupiah,0)) FROM ResellGJ WHERE IDForm = @ID END " +
            "IF EXISTS(SELECT 1 FROM ResellLD WHERE IDForm = @ID) " +
            "BEGIN SELECT @SUMTOTAL = @SUMTOTAL + SUM(ISNULL(HargaRupiah,0)) FROM ResellLD WHERE IDForm = @ID END " +
            "SELECT @SUMTOTAL[SUM]";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = idro}
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);
        totalsales = Convert.ToDecimal(dt.Rows[0]["SUM"]);
        if (totalsales >= 0m)
            status = true;
        return status;
    }

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }

    //Execute Command
    protected void Execute(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                foreach (var x in param)
                {
                    cmd.Parameters.Add(x);
                }
                cmd.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                sqlCnn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    //Scalar Function
    protected string SingleString(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        string x = "";
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                foreach (var y in param)
                {
                    cmd.Parameters.Add(y);
                }
                sqlCnn.Open();
                x = (string)cmd.ExecuteScalar();
            }
        }
        return x;
    }

    //Single Integer
    protected int SingleInteger(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        int x = 0;
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                foreach (var y in param)
                {
                    cmd.Parameters.Add(y);
                }
                sqlCnn.Open();
                x = (int)cmd.ExecuteScalar();
            }
        }
        return x;
    }

    //Single Decimal
    protected decimal SingleDecimal(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        decimal x = 0;
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                foreach (var y in param)
                {
                    cmd.Parameters.Add(y);
                }
                sqlCnn.Open();
                x = Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }
        return x;
    }
}