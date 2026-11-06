using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Globalization;
using System.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data.SqlClient;
using System.Collections;
using System.Reflection;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using System.IO;
using OfficeOpenXml.DataValidation;
using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json;
using System.Security.Cryptography;

public partial class PreLEI : System.Web.UI.Page
{
    protected string myBrand = ConfigurationManager.AppSettings["KodeBrand"];
    protected string connleiString = ConfigurationManager.ConnectionStrings["Cnn_DB_LEI"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        //App.ProtectRole(Role.Operator);
        fillTable();
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        fillTable();
    }

    public void TableGagalKirim(Table tb, string message)
    {
        TableHeaderRow tr = new TableHeaderRow();
        TableCell td = new TableCell();

        td.Text = message;
        td.Width = 990;
        td.ColumnSpan = 1;
        tr.Cells.Add(td);

        tr.CssClass = "no-result";
        tb.Rows.Add(tr);
    }

    //Display Table
    protected void fillTable()
    {
        tb.Rows.Clear();
        //GET DDL BRAND FROM-TO TYPE VALUE
        string kTW = ddl_tipe_plu.SelectedValue.Trim();
        string kW = txt_rts.Text.Trim();
        string vT = ddl_to.SelectedValue;

        int index = 1;
        int id_retur = 0;

        string validasi_surat_jalan = CheckSuratJalanSendiri();
        if (string.IsNullOrEmpty(validasi_surat_jalan))
            validasi_surat_jalan = CheckSuratJalan();
        //NO-RESULT
        if (!string.IsNullOrEmpty(validasi_surat_jalan))
        {
            save.Visible = false;
            TableGagalKirim(tb, validasi_surat_jalan);
        }
        else
        {
            string query = "sp_get_data_product_stock_return_new_iqbal";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@KEYWORDS", SqlDbType.VarChar){ Value = kW },
                new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = kTW }
            };
            DataTable rs1 = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);

            if (rs1.Rows.Count == 0) { TableGagalKirim(tb, "No Data"); save.Visible = false; btn_download_template.Enabled = false; }
            else
            {
                if (kTW == "PG")
                {
                    btn_download_template.Enabled = true;

                    TableHeaderRow thr = new TableHeaderRow();
                    TableHeaderCell thd = new TableHeaderCell();
                    thd.Text = "No.";
                    thd.Width = 40;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Gambar";
                    thd.Width = 100;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Tipe Product";
                    thd.Width = 100;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "PLU";
                    thd.Width = 100;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Kode Barang";
                    thd.Width = 100;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Item";
                    thd.Width = 100;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Frame Color";
                    thd.Width = 100;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Qty";
                    thd.Width = 60;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Tipe PG";
                    thd.Width = 100;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Weight";
                    thd.Width = 70;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Weight Actual";
                    thd.Width = 110;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Kadar Tukaran Beli";
                    thd.Width = 110;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Kadar Tukaran Beli Actual";
                    thd.Width = 110;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Kadar Tukaran Jual";
                    thd.Width = 110;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Kadar Tukaran Jual Actual";
                    thd.Width = 110;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Level";
                    thd.Width = 250;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Supplier";
                    thd.Width = 150;
                    thr.Cells.Add(thd);

                    thd = new TableHeaderCell();
                    thd.Text = "Warehouse";
                    thd.Width = 150;
                    thr.Cells.Add(thd);

                    tb.Rows.Add(thr);

                    double TotalWeight = 0;
                    foreach (DataRow rs in rs1.Rows)
                    {
                        if (id_retur == 0)
                        {
                            id_retur = Convert.ToInt32(rs["ID"]);
                            hf_id.Value = id_retur.ToString();
                            hf_warehouse_id.Value = vT.ToString();
                            hf_tipe_plu.Value = kTW.ToString();
                        }

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Style.Add("text-align", "center");
                        td.Text = "<p>" + index + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Style.Add("text-align", "center");
                        td.Text = "<img src=\"/ImageCrop.aspx?path=" + rs["IMAGE"].ToString() + "\" />";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Style.Add("text-align", "center");
                        td.Text = rs["TIPE_PRODUCT"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Style.Add("text-align", "center");
                        td.Text = rs["NOMOR"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Style.Add("text-align", "center");
                        td.Text = rs["KODEBARANG"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Style.Add("text-align", "center");
                        td.Text = rs["ITEM"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Style.Add("text-align", "center");
                        td.Text = rs["FRAME_COLOR"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Style.Add("text-align", "right");
                        td.Text = 1.ToString();
                        tr.Cells.Add(td);

                        HiddenField hfp = new HiddenField();
                        hfp.ID = "ddltipeori" + index.ToString();
                        hfp.Value = rs["TIPE_PG"].ToString();
                        hfp.ClientIDMode = ClientIDMode.Static;

                        DropDownList ddltipe = new DropDownList();
                        ddltipe.ID = "ddltipe" + index.ToString();
                        ddltipe.CssClass = "form-control";
                        ddltipe.Attributes["onchange"] = "onFindMappingDDL(this);";
                        ddltipe.Items.Add(new ListItem("PG", "PG"));
                        ddltipe.Items.Add(new ListItem("CZ", "CZ"));
                        ddltipe.SelectedValue = rs["TIPE_PG"].ToString();

                        td = new TableCell();
                        td.Style.Add("text-align", "right");
                        //td.Text = rs.TIPE_PG.ToString();
                        td.Controls.Add(ddltipe);
                        td.Controls.Add(hfp);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Style.Add("text-align", "right");
                        td.Text = Convert.ToDouble(rs["WEIGHT"]).ToString();
                        tr.Cells.Add(td);

                        TextBox txt = new TextBox();
                        txt.ID = "actw" + index.ToString();
                        txt.Attributes["style"] = "width: 100px;";
                        txt.Attributes["type"] = "number";
                        txt.Attributes["step"] = "0.01";
                        txt.Attributes["min"] = "0";
                        txt.Attributes["max"] = "100000";
                        txt.Attributes["oninput"] = "onbluray(this);";
                        txt.Attributes["onblur"] = "onValidateWeight(this);";
                        txt.Text = Convert.ToDouble(rs["WEIGHT"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                        txt.CssClass = "form-control text-center";
                        txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                        HiddenField hfw = new HiddenField();
                        hfw.ID = "oriw" + index.ToString();
                        hfw.Value = Convert.ToDouble(rs["WEIGHT"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                        hfw.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                        HiddenField hf = new HiddenField();
                        hf.ID = "hw" + index.ToString();
                        hf.Value = Convert.ToInt32(rs["ID_PRODUCT"]).ToString();
                        hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                        td = new TableCell();
                        td.Controls.Add(txt);
                        td.HorizontalAlign = HorizontalAlign.Center;
                        td.Controls.Add(hf);
                        td.Controls.Add(hfw);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Style.Add("text-align", "center");
                        td.Text = Convert.ToDouble(rs["KADAR_TUKARAN_BELI"]).ToString();
                        tr.Cells.Add(td);

                        TextBox txt2 = new TextBox();
                        txt2.ID = "actkb" + index.ToString();
                        txt2.Attributes["style"] = "width: 100px;";
                        txt2.Attributes["type"] = "number";
                        txt2.Attributes["step"] = ".01";
                        txt2.Attributes["min"] = "0";
                        txt2.Attributes["max"] = "100";
                        txt2.Attributes["onblur"] = "onValidateKadarTukaranBeli(this);";
                        txt2.Text = Convert.ToDouble(rs["KADAR_TUKARAN_BELI"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                        txt2.CssClass = "form-control text-center";
                        txt2.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                        HiddenField hfkb2 = new HiddenField();
                        hfkb2.ID = "orikb" + index.ToString();
                        hfkb2.Value = Convert.ToDouble(rs["KADAR_TUKARAN_BELI"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                        hfkb2.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                        td = new TableCell();
                        td.Controls.Add(txt2);
                        td.HorizontalAlign = HorizontalAlign.Center;
                        td.Controls.Add(hfkb2);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Style.Add("text-align", "center");
                        td.Text = Convert.ToDouble(rs["KADAR_TUKARAN_JUAL"]).ToString();
                        tr.Cells.Add(td);

                        TextBox txt3 = new TextBox();
                        txt3.ID = "actkj" + index.ToString();
                        txt3.Attributes["style"] = "width: 100px;";
                        txt3.Attributes["type"] = "number";
                        txt3.Attributes["step"] = "0.01";
                        txt3.Attributes["min"] = "0";
                        txt3.Attributes["max"] = "100";
                        txt3.Attributes["onblur"] = "onValidateKadarTukaranJual(this);";
                        txt3.Text = Convert.ToDouble(rs["KADAR_TUKARAN_JUAL"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                        txt3.CssClass = "form-control text-center";
                        txt3.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                        HiddenField hfkj3 = new HiddenField();
                        hfkj3.ID = "orikj" + index.ToString();
                        hfkj3.Value = Convert.ToDouble(rs["KADAR_TUKARAN_JUAL"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                        hfkj3.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                        td = new TableCell();
                        td.Controls.Add(txt3);
                        td.HorizontalAlign = HorizontalAlign.Center;
                        td.Controls.Add(hfkj3);
                        tr.Cells.Add(td);

                        string mappingan = GetMappingString(Convert.ToDecimal(rs["KADAR_TUKARAN_JUAL"]), rs["TIPE_PG"].ToString());
                        Label txt4 = new Label();
                        txt4.ID = "mapkj" + index.ToString();
                        txt4.Attributes["style"] = "width: 240px;background-color:lightblue;";
                        txt4.Text = mappingan;
                        txt4.CssClass = "form-control text-center";
                        txt4.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                        HiddenField hfkj4 = new HiddenField();
                        hfkj4.ID = "orimapkj" + index.ToString();
                        hfkj4.Value = mappingan;
                        hfkj4.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                        td = new TableCell();
                        td.Controls.Add(txt4);
                        td.Controls.Add(hfkj4);
                        td.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Style.Add("text-align", "center");
                        td.Text = rs["SUPPLIER_NAMA"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Style.Add("text-align", "center");
                        td.Text = rs["WAREHOUSE_NAMA"].ToString();
                        tr.Cells.Add(td);

                        //ClientScript.RegisterStartupScript(Page.GetType(), "script_" + index.ToString(), " "+ setJavascriptKeyPress(index) +" ", true);
                        TotalWeight += Convert.ToDouble(rs["WEIGHT"]);
                        index++;
                        tb.Rows.Add(tr);
                    }
                    TableRow tr1 = new TableRow();
                    tr1.Style.Add("border", "solid 1px darkcyan;");
                    tr1.Style.Add("color", "darkcyan");
                    TableCell td1;

                    td1 = new TableCell();
                    td1.ColumnSpan = 7;
                    td1.Style.Add("border", "solid 1px darkcyan;");
                    td1.Style.Add("text-align", "left");
                    tr1.Style.Add("color", "black");
                    td1.Text = "<b>TOTAL</b>";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Style.Add("border", "solid 1px darkcyan;");
                    td1.Style.Add("text-align", "right");
                    tr1.Style.Add("color", "black");
                    td1.Text = "<b>" + (index - 1).ToString() + "</b>";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Style.Add("border", "solid 1px darkcyan;");
                    td1.Style.Add("text-align", "right");
                    tr1.Style.Add("color", "black");
                    td1.Text = "";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Style.Add("border", "solid 1px darkcyan;");
                    td1.Style.Add("text-align", "right");
                    tr1.Style.Add("color", "black");
                    td1.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString(CultureInfo.CreateSpecificCulture("en-GB")) + "</b>";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Style.Add("border", "solid 1px darkcyan;");
                    td1.Style.Add("text-align", "center");
                    tr1.Style.Add("color", "black");
                    td1.Text = "<b id='cuy'>" + Convert.ToDecimal(TotalWeight).ToString(CultureInfo.CreateSpecificCulture("en-GB")) + "</b>";
                    tr1.Cells.Add(td1);

                    tb.Rows.Add(tr1);
                    save.Visible = true;
                }
                else
                {
                    btn_download_template.Enabled = false;

                    TableHeaderRow htr = new TableHeaderRow();
                    TableHeaderCell htd;

                    htd = new TableHeaderCell();
                    htd.Text = "No.";
                    htd.Width = 50;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "No. PLU";
                    htd.Width = 90;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Brand";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Warehouse";
                    htd.Width = 140;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Kode Karet";
                    htd.Width = 140;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Nomor Supplier";
                    htd.Width = 140;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Item";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Stone Dist.";
                    htd.Width = 140;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Category";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Segmen";
                    htd.Width = 130;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Net Weight";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Gross Weight";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Gross Weight Actual";
                    htd.Width = 150;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Harga M <span class='text-muted'>USD</span>";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Harga R <span class='text-muted'>USD</span>";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Parcel";
                    htd.Width = 130;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "T. Butir";
                    htd.Width = 70;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "T. Carat";
                    htd.Width = 70;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Stone M";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Stone R";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Approval Tanggal";
                    htd.Width = 150;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Approval";
                    htd.Width = 150;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Keterangan";
                    htd.Width = 150;
                    htr.Cells.Add(htd);

                    tb.Rows.Add(htr);

                    int PrevID = 0;
                    int indexCuy = 0;

                    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                    string plu = string.Empty;
                    foreach (DataRow dr in rs1.Rows)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;

                        bool isSamePLU = plu == dr["PLU"].ToString();

                        if (id_retur == 0)
                        {
                            id_retur = Convert.ToInt32(dr["ID"]);
                            hf_id.Value = id_retur.ToString();
                            hf_warehouse_id.Value = vT.ToString();
                            hf_tipe_plu.Value = kTW.ToString();
                        }

                        if (!isSamePLU) indexCuy++;

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : indexCuy.ToString() + ".";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : dr["PLU"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : dr["BRAND"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : dr["NAMA_LOKASI"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : dr["PROTO_NOMOR"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : dr["SUPPLIER_NOMOR"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : dr["PRODUCT_ITEM"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : dr["PRODUCT_CATEGORY"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : dr["STONE_DIST"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : dr["SEGMENT"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : Convert.ToDecimal(dr["NETTO_WEIGHT"]).ToString("N2", nfi);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : Convert.ToDecimal(dr["GROSS_WEIGHT"]).ToString("N2", nfi);
                        tr.Cells.Add(td);

                        if (!isSamePLU)
                        {
                            TextBox txt = new TextBox();
                            txt.ID = "actw" + indexCuy.ToString();
                            txt.Attributes["style"] = "width: 100px;";
                            txt.Attributes["type"] = "number";
                            txt.Attributes["step"] = "0.01";
                            txt.Attributes["oninput"] = "onbluray(this);";
                            txt.Text = Convert.ToDouble(dr["GROSS_WEIGHT"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                            txt.CssClass = "form-control text-center";
                            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                            HiddenField hfw = new HiddenField();
                            hfw.ID = "oriw" + indexCuy.ToString();
                            hfw.Value = Convert.ToDouble(dr["GROSS_WEIGHT"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                            hfw.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                            HiddenField hf = new HiddenField();
                            hf.ID = "hw" + indexCuy.ToString();
                            hf.Value = Convert.ToInt32(dr["ID_PRODUCT"]).ToString();
                            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                            td = new TableCell();
                            td.Controls.Add(txt);
                            td.HorizontalAlign = HorizontalAlign.Center;
                            td.Controls.Add(hf);
                            td.Controls.Add(hfw);
                            tr.Cells.Add(td);
                        }
                        else
                        {
                            td = new TableCell();
                            td.Text = "";
                            tr.Cells.Add(td);
                        }

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : Convert.ToDecimal(dr["HARGAM_USD"]).ToString("N2", nfi);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : Convert.ToDecimal(dr["HARGAR_USD"]).ToString("N2", nfi);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = dr["STONE"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(dr["TOTAL_BUTIR"]).ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(dr["TOTAL_CARAT"]).ToString("N4", nfi);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(dr["TOTAL_HARGA_M"]).ToString("N2", nfi);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(dr["TOTAL_HARGA_R"]).ToString("N2", nfi);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : Str.Tgl(Convert.ToDateTime(dr["APPROVAL_TGL_RETURN"]));
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : dr["APPROVAL_RETURN"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = isSamePLU ? "" : dr["KETERANGAN_RETURN"].ToString();
                        tr.Cells.Add(td);

                        plu = dr["PLU"].ToString();


                        tb.Rows.Add(tr);

                        save.Visible = true;
                    }
                }

            }
        }
    }

    protected void DownloadTemplate(object sender, EventArgs e)
    {
        string kTW = ddl_tipe_plu.SelectedValue.Trim();
        string kW = txt_rts.Text.Trim();
        string vT = ddl_to.SelectedValue;

        int index = 1;
        int id_retur = 0;

        string query = "sp_get_data_product_stock_return_new_iqbal";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@KEYWORDS", SqlDbType.VarChar){ Value = kW },
            new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = kTW }
        };
        DataTable rs1 = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
        using (MemoryStream ms = new MemoryStream())
        {
            using (ExcelPackage package = new ExcelPackage(ms))
            {
                // Add a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("Upload");

                // Add some data to the worksheet
                ExcelRange range1 = worksheet.Cells[1, 1];
                range1.Value = "No. RTS";
                ExcelRange range2 = worksheet.Cells[1, 2];
                range2.Value = "PLU";
                ExcelRange range3 = worksheet.Cells[1, 3];
                range3.Value = "Product Type";
                ExcelRange range4 = worksheet.Cells[1, 4];
                range4.Value = "Weight Actual";
                ExcelRange range5 = worksheet.Cells[1, 5];
                range5.Value = "Kadar Tukaran Beli Actual";
                ExcelRange range6 = worksheet.Cells[1, 6];
                range6.Value = "Kadar Tukaran Jual Actual";

                using (var headerRow = worksheet.Cells[1, 1, 1, 6])
                {
                    headerRow.Style.Font.Name = "Consolas";
                    headerRow.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRow.Style.Fill.BackgroundColor.SetColor(Color.AliceBlue);
                    headerRow.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    headerRow.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                    headerRow.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    headerRow.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                }

                NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;

                foreach (DataRow dr in rs1.Rows)
                {
                    using (var detailRow = worksheet.Cells[index + 1, 1, index + 1, 6])
                    {
                        detailRow.Style.Font.Name = "Consolas";
                        detailRow.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        detailRow.Style.Fill.BackgroundColor.SetColor(Color.White);
                        detailRow.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        detailRow.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        detailRow.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        detailRow.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    ;

                    int endRow = index + 1;
                    int column = 3; // Column C

                    string columnLetter = ExcelCellAddress.GetColumnLetter(column);
                    string rangeAddress = "C" + endRow + ":C" + endRow;

                    var validation = worksheet.DataValidations.AddListValidation(rangeAddress);
                    validation.Formula.Values.Add("PG");
                    validation.Formula.Values.Add("CZ");
                    validation.AllowBlank = false;
                    validation.Error = "Must Choose PG and CZ.";

                    worksheet.Cells[index + 1, 1].Value = dr["NO_RETURN"].ToString();
                    worksheet.Cells[index + 1, 2].Value = dr["NOMOR"].ToString();
                    worksheet.Cells[index + 1, 3].Value = dr["TIPE_PG"].ToString();
                    worksheet.Cells[index + 1, 4].Value = Convert.ToDecimal(dr["WEIGHT"]).ToString("N2", nfi);
                    worksheet.Cells[index + 1, 5].Value = Convert.ToDecimal(dr["KADAR_TUKARAN_BELI"]).ToString("N2", nfi);
                    worksheet.Cells[index + 1, 6].Value = Convert.ToDecimal(dr["KADAR_TUKARAN_JUAL"]).ToString("N2", nfi);
                    
                    index++;
                }

                worksheet.Cells[2, 10].Value = "Nomor RTS dan PLU tidak boleh diubah karena jika diubah tidak akan terupload. Ubah hanya bagian berat, kadar tukaran jual dan kadar tukaran beli saja.";

                worksheet.Cells.AutoFitColumns();
                package.Save();
            }

            // Set the response to prompt the user to download the file
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=Template Upload Bulk One Price.xlsx");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }

    protected void UploadRTS(object sender, EventArgs e)
    {
        ValidateRTS();
    }

    protected string ValidateRTS()
    {
        string result = string.Empty;

        try
        {
            if (fileupload.HasFile)
            {
                ddl_tipe_plu.SelectedValue = "PG";

                DataTable dt = new DataTable();
                dt.Columns.Add("NoRTS", typeof(string));
                dt.Columns.Add("PLU", typeof(string));
                dt.Columns.Add("ProductType", typeof(string));
                dt.Columns.Add("WeightActual", typeof(decimal));
                dt.Columns.Add("KadarTukaranBeli", typeof(decimal));
                dt.Columns.Add("KadarTukaranJual", typeof(decimal));
                
                using (Stream fileStream = fileupload.PostedFile.InputStream)
                {
                    using (ExcelPackage package = new ExcelPackage(fileStream))
                    {
                        int maxcolumn = 6;
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Read first worksheet

                        // Assume first row has headers
                        //foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                        //{
                        //dt.Columns.Add(firstRowCell.Text);
                        //}

                        for (int rowNum = 2; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                        {
                            var row = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];
                            DataRow dr = dt.NewRow();
                            for (int col = 1; col <= maxcolumn; col++)
                            {
                                if (col - 1 == 3 || col - 1 == 4 || col - 1 == 5)
                                    dr[col - 1] = Convert.ToDecimal(row[rowNum, col].Text.Replace('.', ','));
                                else
                                    dr[col - 1] = row[rowNum, col].Text;
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("PLU", typeof(string));

                foreach (DataRow dr in dt.Rows)
                {
                    dt2.Rows.Add(dr["PLU"].ToString());
                }

                string validasi = string.Empty;
                tb.Rows.Clear();

                int distinctCountNoRts = dt.AsEnumerable()
                      .Select(row => row[0]) // Column index 0
                      .Distinct()
                      .Count();
                int countOutOfScopedKadarTukaran = dt.AsEnumerable()
                      .Select(row => row["KadarTukaranJual"])
                      //.Where(value => value != DBNull.Value && !new decimal[] { 84, 80, 40, 38 }.Contains(Convert.ToDecimal(value)))
					  .Where(value => value != DBNull.Value && !new decimal[] { 84, 40 }.Contains(Convert.ToDecimal(value)))
                      .Count();
                int countOutOfScopedProductType = dt.AsEnumerable()
                    .Where(row => row["ProductType"] != DBNull.Value)
                    .Select(row => row["ProductType"].ToString())
                    .Count(productType => productType != "PG" && productType != "CZ");

                int countOutOffScopedKadarTukaranBeli = dt.AsEnumerable()
                    .Where(row => row["KadarTukaranBeli"] != DBNull.Value)
                    .Select(row => Convert.ToDecimal(row["KadarTukaranBeli"]))
                    .Count(value => value < 0 && value > 100);

                if (distinctCountNoRts > 1)
                {
                    validasi = "Upload Gagal. Terdapat Lebih Dari Satu Nomor RTS yang DiUpload.";
                    lit_valid.Text = validasi;
                    save.Visible = false;
                }
                else if (distinctCountNoRts == 0)
                {
                    validasi = "Upload gagal. Nomor RTS tidak ditemukan.";
                    lit_valid.Text = validasi;
                    save.Visible = false;
                }
                else if (countOutOfScopedKadarTukaran > 0)
                {
                    validasi = "Upload gagal, terdapat kadar tukaran yang salah. Kadar Tukaran Jual Yang Tersedia Adalah 84 dan 40.";
                    lit_valid.Text = validasi;
                    save.Visible = false;
                }
                else if(countOutOffScopedKadarTukaranBeli > 0)
                {
                    validasi = "Upload gagal. Kadar tukaran beli minimal 0 dan maksimal 100.";
                    lit_valid.Text = validasi;
                    save.Visible = false;
                }
                else if(countOutOfScopedProductType > 0)
                {
                    validasi = "Upload gagal. Product Type yang tersedia adalah PG dan CZ.";
                    lit_valid.Text = validasi;
                    save.Visible = false;
                }
                else
                {
                    lit_valid.Text = "";
                    string NoRts = dt.AsEnumerable()
                        .Select(row => row.Field<string>("NoRTS").ToString()).FirstOrDefault();

                    txt_rts.Text = NoRts;
                    if (string.IsNullOrEmpty(validasi))
                    {
                        string validasi_surat_jalan = CheckSuratJalanSendiri();
                        if (string.IsNullOrEmpty(validasi_surat_jalan))
                            validasi_surat_jalan = CheckSuratJalan();
                        //NO-RESULT
                        if (!string.IsNullOrEmpty(validasi_surat_jalan))
                        {
                            validasi = validasi_surat_jalan;
                        }
                    }
                    else
                    {
                        lit_valid.Text = validasi;
                    }

                    if (string.IsNullOrEmpty(validasi))
                    {
                        string query = "sp_check_data_rts_for_prelei_new";
                        List<SqlParameter> parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@NO_RTS", SqlDbType.Text){ Value = NoRts },
                            new SqlParameter("@TIPE_PRODUCT", SqlDbType.Text){ Value = "PG" },
                            new SqlParameter("@DATA", SqlDbType.Structured){ Value = dt2 }
                        };
                        DataTable dtcheck = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
                        validasi = dtcheck.Rows[0]["MESSAGE"].ToString();
                    }
                    else
                    {
                        lit_valid.Text = validasi;
                    }

                    if (string.IsNullOrEmpty(validasi))
                    {
                        //GET DDL BRAND FROM-TO TYPE VALUE
                        string kTW = ddl_tipe_plu.SelectedValue.Trim();
                        string kW = txt_rts.Text.Trim();
                        string vT = ddl_to.SelectedValue;

                        int index = 1;
                        int id_retur = 0;

                        string query = "sp_get_data_product_stock_return_new_iqbal";
                        List<SqlParameter> parameters2 = new List<SqlParameter>
                        {
                            new SqlParameter("@KEYWORDS", SqlDbType.VarChar){ Value = kW },
                            new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = kTW }
                        };
                        DataTable rs1 = Rs(query, Db.Cnn_DB, parameters2, CommandType.StoredProcedure);

                        List<ReturJoinModel> joined = (from row1 in dt.AsEnumerable()
                                                       join row2 in rs1.AsEnumerable()
                                                       on row1.Field<string>("PLU") equals row2.Field<string>("NOMOR")
                                                       select new ReturJoinModel
                                                       {
                                                           ID = row2.Field<int>("ID"),
                                                           IMAGE = row2.Field<string>("IMAGE"),
                                                           TIPE_PRODUCT = row2.Field<string>("TIPE_PRODUCT"),
                                                           TIPE_PG = row1.Field<string>("PRODUCTTYPE"),
                                                           NOMOR = row2.Field<string>("NOMOR"),
                                                           KODEBARANG = row2.Field<string>("KODEBARANG"),
                                                           ITEM = row2.Field<string>("ITEM"),
                                                           FRAME_COLOR = row2.Field<string>("FRAME_COLOR"),
                                                           WEIGHT = row2.Field<decimal>("WEIGHT"),
                                                           WEIGHT_INPUTAN = row1.Field<decimal>("WEIGHTACTUAL"),
                                                           ID_PRODUCT = row2.Field<int>("ID_PRODUCT"),
                                                           KADAR_TUKARAN_BELI = row2.Field<decimal>("KADAR_TUKARAN_BELI"),
                                                           KADAR_TUKARAN_JUAL = row2.Field<decimal>("KADAR_TUKARAN_JUAL"),
                                                           KADAR_TUKARAN_BELI_INPUTAN = row1.Field<decimal>("KADARTUKARANBELI"),
                                                           KADAR_TUKARAN_JUAL_INPUTAN = row1.Field<decimal>("KADARTUKARANJUAL"),
                                                           SUPPLIER_NAMA = row2.Field<string>("SUPPLIER_NAMA"),
                                                           WAREHOUSE_NAMA = row2.Field<string>("WAREHOUSE_NAMA")
                                                       }).ToList();

                        if (rs1.Rows.Count == 0) { TableGagalKirim(tb, "No Data"); save.Visible = false; }
                        else
                        {
                            if (kTW == "PG")
                            {
                                TableHeaderRow thr = new TableHeaderRow();
                                TableHeaderCell thd = new TableHeaderCell();
                                thd.Text = "No.";
                                thd.Width = 40;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Gambar";
                                thd.Width = 100;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Tipe Product";
                                thd.Width = 100;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "PLU";
                                thd.Width = 100;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Kode Barang";
                                thd.Width = 100;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Item";
                                thd.Width = 100;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Frame Color";
                                thd.Width = 100;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Qty";
                                thd.Width = 60;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Tipe PG";
                                thd.Width = 100;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Weight";
                                thd.Width = 70;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Weight Actual";
                                thd.Width = 110;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Kadar Tukaran Beli";
                                thd.Width = 110;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Kadar Tukaran Beli Actual";
                                thd.Width = 110;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Kadar Tukaran Jual";
                                thd.Width = 110;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Kadar Tukaran Jual Actual";
                                thd.Width = 110;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Level";
                                thd.Width = 250;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Supplier";
                                thd.Width = 150;
                                thr.Cells.Add(thd);

                                thd = new TableHeaderCell();
                                thd.Text = "Warehouse";
                                thd.Width = 150;
                                thr.Cells.Add(thd);

                                tb.Rows.Add(thr);

                                double TotalWeight = 0;
                                foreach (ReturJoinModel rs in joined)
                                {
                                    if (id_retur == 0)
                                    {
                                        id_retur = Convert.ToInt32(rs.ID);
                                        hf_id.Value = id_retur.ToString();
                                        hf_warehouse_id.Value = vT.ToString();
                                        hf_tipe_plu.Value = kTW.ToString();
                                    }

                                    TableRow tr = new TableRow();
                                    TableCell td;

                                    td = new TableCell();
                                    td.Style.Add("text-align", "center");
                                    td.Text = "<p>" + index + ".</p>";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Style.Add("text-align", "center");
                                    td.Text = "<img src=\"/ImageCrop.aspx?path=" + rs.IMAGE.ToString() + "\" />";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Style.Add("text-align", "center");
                                    td.Text = rs.TIPE_PRODUCT.ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Style.Add("text-align", "center");
                                    td.Text = rs.NOMOR.ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Style.Add("text-align", "center");
                                    td.Text = rs.KODEBARANG.ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Style.Add("text-align", "center");
                                    td.Text = rs.ITEM.ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Style.Add("text-align", "center");
                                    td.Text = rs.FRAME_COLOR.ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Style.Add("text-align", "right");
                                    td.Text = 1.ToString();
                                    tr.Cells.Add(td);

                                    HiddenField hfp = new HiddenField();
                                    hfp.ID = "ddltipeori" + index.ToString();
                                    hfp.Value = rs.TIPE_PG.ToString();
                                    hfp.ClientIDMode = ClientIDMode.Static;

                                    DropDownList ddltipe = new DropDownList();
                                    ddltipe.ID = "ddltipe" + index.ToString();
                                    ddltipe.CssClass = "form-control";
                                    ddltipe.Attributes["onchange"] = "onFindMappingDDL(this);";
                                    ddltipe.Items.Add(new ListItem("PG", "PG"));
                                    ddltipe.Items.Add(new ListItem("CZ", "CZ"));
                                    ddltipe.SelectedValue = rs.TIPE_PG.ToString();

                                    td = new TableCell();
                                    td.Style.Add("text-align", "right");
                                    //td.Text = rs.TIPE_PG.ToString();
                                    td.Controls.Add(ddltipe);
                                    td.Controls.Add(hfp);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Style.Add("text-align", "right");
                                    td.Text = Convert.ToDouble(rs.WEIGHT).ToString();
                                    tr.Cells.Add(td);

                                    TextBox txt = new TextBox();
                                    txt.ID = "actw" + index.ToString();
                                    txt.Attributes["style"] = "width: 100px;";
                                    txt.Attributes["type"] = "number";
                                    txt.Attributes["step"] = "0.01";
                                    txt.Attributes["min"] = "0";
                                    txt.Attributes["max"] = "100000";
                                    txt.Attributes["oninput"] = "onbluray(this);";
                                    txt.Attributes["onblur"] = "onValidateWeight(this);";
                                    txt.Text = Convert.ToDouble(rs.WEIGHT_INPUTAN).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                                    txt.CssClass = "form-control text-center";
                                    txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                    HiddenField hfw = new HiddenField();
                                    hfw.ID = "oriw" + index.ToString();
                                    hfw.Value = Convert.ToDouble(rs.WEIGHT).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                                    hfw.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                    HiddenField hf = new HiddenField();
                                    hf.ID = "hw" + index.ToString();
                                    hf.Value = Convert.ToInt32(rs.ID_PRODUCT).ToString();
                                    hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                    td = new TableCell();
                                    td.Controls.Add(txt);
                                    td.HorizontalAlign = HorizontalAlign.Center;
                                    td.Controls.Add(hf);
                                    td.Controls.Add(hfw);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Style.Add("text-align", "center");
                                    td.Text = Convert.ToDouble(rs.KADAR_TUKARAN_BELI).ToString();
                                    tr.Cells.Add(td);

                                    TextBox txt2 = new TextBox();
                                    txt2.ID = "actkb" + index.ToString();
                                    txt2.Attributes["style"] = "width: 100px;";
                                    txt2.Attributes["type"] = "number";
                                    txt2.Attributes["step"] = "0.01";
                                    txt2.Attributes["min"] = "0";
                                    txt2.Attributes["max"] = "100";
                                    txt2.Attributes["onblur"] = "onValidateKadarTukaranBeli(this);";
                                    txt2.Text = Convert.ToDouble(rs.KADAR_TUKARAN_BELI_INPUTAN).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                                    txt2.CssClass = "form-control text-center";
                                    txt2.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                    HiddenField hfkb2 = new HiddenField();
                                    hfkb2.ID = "orikb" + index.ToString();
                                    hfkb2.Value = Convert.ToDouble(rs.KADAR_TUKARAN_BELI).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                                    hfkb2.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                    td = new TableCell();
                                    td.Controls.Add(txt2);
                                    td.HorizontalAlign = HorizontalAlign.Center;
                                    td.Controls.Add(hfkb2);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Style.Add("text-align", "center");
                                    td.Text = Convert.ToDouble(rs.KADAR_TUKARAN_JUAL).ToString();
                                    tr.Cells.Add(td);

                                    TextBox txt3 = new TextBox();
                                    txt3.ID = "actkj" + index.ToString();
                                    txt3.Attributes["style"] = "width: 100px;";
                                    txt3.Attributes["type"] = "number";
                                    txt3.Attributes["step"] = "0.01";
                                    txt3.Attributes["min"] = "0";
                                    txt3.Attributes["max"] = "100";
                                    txt3.Attributes["onblur"] = "onValidateKadarTukaranJual(this);";
                                    txt3.Text = Convert.ToDouble(rs.KADAR_TUKARAN_JUAL_INPUTAN).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                                    txt3.CssClass = "form-control text-center";
                                    txt3.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                    HiddenField hfkj3 = new HiddenField();
                                    hfkj3.ID = "orikj" + index.ToString();
                                    hfkj3.Value = Convert.ToDouble(rs.KADAR_TUKARAN_JUAL).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                                    hfkj3.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                    td = new TableCell();
                                    td.Controls.Add(txt3);
                                    td.HorizontalAlign = HorizontalAlign.Center;
                                    td.Controls.Add(hfkj3);
                                    tr.Cells.Add(td);

                                    string mappingan = GetMappingString(Convert.ToDecimal(rs.KADAR_TUKARAN_JUAL_INPUTAN), rs.TIPE_PG);
                                    Label txt4 = new Label();
                                    txt4.ID = "mapkj" + index.ToString();
                                    txt4.Attributes["style"] = "width: 240px;background-color:lightblue;";
                                    txt4.Text = mappingan;
                                    txt4.CssClass = "form-control text-center";
                                    txt4.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                    HiddenField hfkj4 = new HiddenField();
                                    hfkj4.ID = "orimapkj" + index.ToString();
                                    hfkj4.Value = mappingan;
                                    hfkj4.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                    td = new TableCell();
                                    td.Controls.Add(txt4);
                                    td.Controls.Add(hfkj4);
                                    td.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Style.Add("text-align", "center");
                                    td.Text = rs.SUPPLIER_NAMA.ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Style.Add("text-align", "center");
                                    td.Text = rs.WAREHOUSE_NAMA.ToString();
                                    tr.Cells.Add(td);

                                    TotalWeight += Convert.ToDouble(rs.WEIGHT);
                                    index++;
                                    tb.Rows.Add(tr);
                                }
                                TableRow tr1 = new TableRow();
                                tr1.Style.Add("border", "solid 1px darkcyan;");
                                tr1.Style.Add("color", "darkcyan");
                                TableCell td1;

                                td1 = new TableCell();
                                td1.ColumnSpan = 7;
                                td1.Style.Add("border", "solid 1px darkcyan;");
                                td1.Style.Add("text-align", "left");
                                tr1.Style.Add("color", "black");
                                td1.Text = "<b>TOTAL</b>";
                                tr1.Cells.Add(td1);

                                td1 = new TableCell();
                                td1.Style.Add("border", "solid 1px darkcyan;");
                                td1.Style.Add("text-align", "right");
                                tr1.Style.Add("color", "black");
                                td1.Text = "<b>" + (index - 1).ToString() + "</b>";
                                tr1.Cells.Add(td1);

                                td1 = new TableCell();
                                td1.Style.Add("border", "solid 1px darkcyan;");
                                td1.Style.Add("text-align", "right");
                                tr1.Style.Add("color", "black");
                                td1.Text = "";
                                tr1.Cells.Add(td1);

                                td1 = new TableCell();
                                td1.Style.Add("border", "solid 1px darkcyan;");
                                td1.Style.Add("text-align", "right");
                                tr1.Style.Add("color", "black");
                                td1.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString(CultureInfo.CreateSpecificCulture("en-GB")) + "</b>";
                                tr1.Cells.Add(td1);

                                td1 = new TableCell();
                                td1.Style.Add("border", "solid 1px darkcyan;");
                                td1.Style.Add("text-align", "center");
                                tr1.Style.Add("color", "black");
                                td1.Text = "<b id='cuy'>" + Convert.ToDecimal(TotalWeight).ToString(CultureInfo.CreateSpecificCulture("en-GB")) + "</b>";
                                tr1.Cells.Add(td1);

                                tb.Rows.Add(tr1);
                                save.Visible = true;
                            }
                            else
                            {
                                TableHeaderRow htr = new TableHeaderRow();
                                TableHeaderCell htd;

                                htd = new TableHeaderCell();
                                htd.Text = "No.";
                                htd.Width = 50;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "No. PLU";
                                htd.Width = 90;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Brand";
                                htd.Width = 120;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Warehouse";
                                htd.Width = 140;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Kode Karet";
                                htd.Width = 140;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Nomor Supplier";
                                htd.Width = 140;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Item";
                                htd.Width = 120;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Stone Dist.";
                                htd.Width = 140;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Category";
                                htd.Width = 120;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Segmen";
                                htd.Width = 130;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Net Weight";
                                htd.Width = 100;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Gross Weight";
                                htd.Width = 100;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Gross Weight Actual";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Harga M <span class='text-muted'>USD</span>";
                                htd.Width = 100;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Harga R <span class='text-muted'>USD</span>";
                                htd.Width = 100;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Parcel";
                                htd.Width = 130;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "T. Butir";
                                htd.Width = 70;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "T. Carat";
                                htd.Width = 70;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Stone M";
                                htd.Width = 100;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Stone R";
                                htd.Width = 100;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Approval Tanggal";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Approval";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Keterangan";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                                tb.Rows.Add(htr);

                                int PrevID = 0;
                                int indexCuy = 0;

                                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                                string plu = string.Empty;
                                foreach (DataRow dr in rs1.Rows)
                                {
                                    TableRow tr = new TableRow();
                                    TableCell td;

                                    bool isSamePLU = plu == dr["PLU"].ToString();

                                    if (id_retur == 0)
                                    {
                                        id_retur = Convert.ToInt32(dr["ID"]);
                                        hf_id.Value = id_retur.ToString();
                                        hf_warehouse_id.Value = vT.ToString();
                                        hf_tipe_plu.Value = kTW.ToString();
                                    }

                                    if (!isSamePLU) indexCuy++;

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : indexCuy.ToString() + ".";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : dr["PLU"].ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : dr["BRAND"].ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : dr["NAMA_LOKASI"].ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : dr["PROTO_NOMOR"].ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : dr["SUPPLIER_NOMOR"].ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : dr["PRODUCT_ITEM"].ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : dr["PRODUCT_CATEGORY"].ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : dr["STONE_DIST"].ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : dr["SEGMENT"].ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : Convert.ToDecimal(dr["NETTO_WEIGHT"]).ToString("N2", nfi);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : Convert.ToDecimal(dr["GROSS_WEIGHT"]).ToString("N2", nfi);
                                    tr.Cells.Add(td);

                                    if (!isSamePLU)
                                    {
                                        TextBox txt = new TextBox();
                                        txt.ID = "actw" + indexCuy.ToString();
                                        txt.Attributes["style"] = "width: 100px;";
                                        txt.Attributes["type"] = "number";
                                        txt.Attributes["step"] = "0.01";
                                        txt.Attributes["oninput"] = "onbluray(this);";
                                        txt.Text = Convert.ToDouble(dr["GROSS_WEIGHT"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                                        txt.CssClass = "form-control text-center";
                                        txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                        HiddenField hfw = new HiddenField();
                                        hfw.ID = "oriw" + indexCuy.ToString();
                                        hfw.Value = Convert.ToDouble(dr["GROSS_WEIGHT"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                                        hfw.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                        HiddenField hf = new HiddenField();
                                        hf.ID = "hw" + indexCuy.ToString();
                                        hf.Value = Convert.ToInt32(dr["ID_PRODUCT"]).ToString();
                                        hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                        td = new TableCell();
                                        td.Controls.Add(txt);
                                        td.HorizontalAlign = HorizontalAlign.Center;
                                        td.Controls.Add(hf);
                                        td.Controls.Add(hfw);
                                        tr.Cells.Add(td);
                                    }
                                    else
                                    {
                                        td = new TableCell();
                                        td.Text = "";
                                        tr.Cells.Add(td);
                                    }

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : Convert.ToDecimal(dr["HARGAM_USD"]).ToString("N2", nfi);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : Convert.ToDecimal(dr["HARGAR_USD"]).ToString("N2", nfi);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = dr["STONE"].ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Convert.ToDecimal(dr["TOTAL_BUTIR"]).ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Convert.ToDecimal(dr["TOTAL_CARAT"]).ToString("N4", nfi);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Convert.ToDecimal(dr["TOTAL_HARGA_M"]).ToString("N2", nfi);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Convert.ToDecimal(dr["TOTAL_HARGA_R"]).ToString("N2", nfi);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : Str.Tgl(Convert.ToDateTime(dr["APPROVAL_TGL_RETURN"]));
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : dr["APPROVAL_RETURN"].ToString();
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = isSamePLU ? "" : dr["KETERANGAN_RETURN"].ToString();
                                    tr.Cells.Add(td);

                                    plu = dr["PLU"].ToString();

                                    tb.Rows.Add(tr);

                                    save.Visible = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        lit_valid.Text = validasi;
                        save.Visible = false;
                    }
                }
            }
            else
            {
                lit_valid.Text = "Upload Gagal. File Belum Dipilih";
                save.Visible = false;
                tb.Rows.Clear();
            }
        }
        catch (Exception ex)
        {
            result = "Format Excel Ada Yang Salah.";
            lit_valid.Text = result + "##" + ex.Message;
            save.Visible = false;
            tb.Rows.Clear();
        }
        return result;
    }

    public class ReturJoinModel
    {
        public int ID { get; set; }
        public string IMAGE { get; set; }
        public string TIPE_PRODUCT { get; set; }
        public string TIPE_PG { get; set; }
        public string NOMOR { get; set; }
        public string KODEBARANG { get; set; }
        public string ITEM { get; set; }
        public string FRAME_COLOR { get; set; }
        public decimal WEIGHT { get; set; }
        public decimal WEIGHT_INPUTAN { get; set; }
        public int ID_PRODUCT { get; set; }
        public decimal KADAR_TUKARAN_BELI { get; set; }
        public decimal KADAR_TUKARAN_BELI_INPUTAN { get; set; }
        public decimal KADAR_TUKARAN_JUAL { get; set; }
        public decimal KADAR_TUKARAN_JUAL_INPUTAN { get; set; }
        public string SUPPLIER_NAMA { get; set; }
        public string WAREHOUSE_NAMA { get; set; }
    }

    protected string CheckSuratJalan()
    {
        string tipe_plu = ddl_tipe_plu.SelectedValue.Trim();
        string Cnn_DB = connleiString;

        if (tipe_plu == "PG")
        {
            string query = "SELECT COUNT('') [Total] FROM StockReceivePG " +
                "WHERE NoTTB = @NoTTB " +
                "AND BRANDASAL = @BrandAsal";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@NoTTB", SqlDbType.VarChar){ Value = txt_rts.Text.Trim() },
                new SqlParameter("@BrandAsal", SqlDbType.VarChar){ Value = myBrand }
            };
            int counttotal = SingleInteger(query, Cnn_DB, parameters, CommandType.Text);

            if (counttotal > 0)
                return "Gagal Mengirim RTS = " + txt_rts.Text.Trim() + " karena sudah pernah dikirimkan ke PreLEI.";
            else
                return "";
        }
        else if (tipe_plu == "DJ")
        {
            string query = "SELECT COUNT('') [Total] FROM StockReceiveDJ " +
                "WHERE NoTTB = @NoTTB " +
                "AND BRANDASAL = @BrandAsal";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@NoTTB", SqlDbType.VarChar){ Value = txt_rts.Text.Trim() },
                new SqlParameter("@BrandAsal", SqlDbType.VarChar){ Value = myBrand }
            };

            int counttotal = SingleInteger(query, Cnn_DB, parameters, CommandType.Text);

            if (counttotal > 0)
                return "Gagal Mengirim RTS = " + txt_rts.Text.Trim() + " karena sudah pernah dikirimkan ke PreLEI.";
            else
                return "";
        }
        else
        {
            return "";
        }
    }

    protected string CheckSuratJalanSendiri()
    {
        string query = "SELECT COUNT('') [Total] FROM PreLei_StockReceive WHERE Nomor = @NoRTS";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@NoRTS", SqlDbType.VarChar){ Value = txt_rts.Text.Trim() },
        };
        int counttotal = SingleInteger(query, Db.Cnn_DB, parameters, CommandType.Text);

        if (counttotal > 0)
        {
            List<SqlParameter> parameters2 = new List<SqlParameter>
            {
                new SqlParameter("@NoRTS", SqlDbType.VarChar){ Value = txt_rts.Text.Trim() },
            };

            query += " AND Approval IS NOT NULL";
            int counttotalapproved = SingleInteger(query, Db.Cnn_DB, parameters2, CommandType.Text);
            if (counttotalapproved > 0)
                return "Gagal Mengirim RTS = " + txt_rts.Text.Trim() + " karena sudah approved, check menu report.";
            else
                return "Gagal Mengirim RTS = " + txt_rts.Text.Trim() + " karena sudah pernah dibuat, check menu approval.";
        }
        else
            return "";
    }

    protected void LocationWarehouse(DropDownList c)
    {
        c.Items.Clear();
        string query = "SELECT ID, 0 Tipe, Nama [Kode], UPPER(Nama) [Nama] FROM LocWarehouse WHERE Draft = 0 AND Disable = 0";

        DataTable dt = Rs(query, Db.Cnn_DB, new List<SqlParameter>(), CommandType.Text);
        c.Items.Add(new ListItem("All", "0"));
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString() + " (" + item["Kode"].ToString() + ")";
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }

        dt.Dispose();
    }

    //Filter
    protected void save_Click(object sender, EventArgs e)
    {
        /*string querynomor = "sp_check_data_stock_return_new";
		List<SqlParameter> parameters = new List<SqlParameter>
		{
			new SqlParameter("@ID", SqlDbType.Int){ Value = hf_id.Value },
			new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = hf_tipe_plu.Value }
		};

        string nomor = Rs(querynomor, Db.Cnn_DB, parameters, CommandType.StoredProcedure).Rows[0][0].ToString();

		DataTable dt = new DataTable();
		DataColumn[] columns = new DataColumn[7];
		columns[0] = new DataColumn("IDProduct", typeof(int));
        columns[1] = new DataColumn("WeightOriginal", typeof(decimal));
        columns[2] = new DataColumn("WeightActual", typeof(decimal));
        columns[3] = new DataColumn("KadarTukaranBeli", typeof(decimal));
        columns[4] = new DataColumn("KadarTukaranBeliActual", typeof(decimal));
        columns[5] = new DataColumn("KadarTukaranJual", typeof(decimal));
        columns[6] = new DataColumn("KadarTukaranJualActual", typeof(decimal));
        dt.Columns.AddRange(columns);

		int index = 1;
        foreach (TableRow row in tb.Rows)
        {
            try
            {
                HiddenField hwq1 = (HiddenField)row.FindControl("hw" + index.ToString());

                TextBox txtactwq1 = (TextBox)row.FindControl("actw" + index.ToString());
                HiddenField horiwq1 = (HiddenField)row.FindControl("oriw" + index.ToString());

                TextBox txtactkbq2 = (TextBox)row.FindControl("actkb" + index.ToString());
                HiddenField horikbq2 = (HiddenField)row.FindControl("orikb" + index.ToString());

                TextBox txtactkjq3 = (TextBox)row.FindControl("actkj" + index.ToString());
                HiddenField horikjq3 = (HiddenField)row.FindControl("orikj" + index.ToString());

                // Use a culture that uses ',' as decimal separator (e.g., German)
                CultureInfo culture = new CultureInfo("de-DE");

                string txthwq1 = hwq1.Value;

                string actwq1 = txtactwq1.Text;
                string oriwq1 = horiwq1.Value;

                string actkbq2 = txtactkbq2.Text;
                string orikbq2 = horikbq2.Value;

                string actkjq3 = txtactkjq3.Text;
                string orikjq3 = horikjq3.Value;
                DataRow dr = dt.NewRow();
				dr["IDProduct"] = Convert.ToInt32(txthwq1);
				dr["WeightOriginal"] = Convert.ToDecimal(oriwq1, culture);
                dr["WeightActual"] = Convert.ToDecimal(actwq1, culture);
                dr["KadarTukaranBeli"] = Convert.ToDecimal(orikbq2, culture);
                dr["KadarTukaranBeliActual"] = Convert.ToDecimal(actkbq2, culture);
                dr["KadarTukaranJual"] = Convert.ToDecimal(orikjq3, culture);
                dr["KadarTukaranJualActual"] = Convert.ToDecimal(actkjq3, culture);
				dt.Rows.Add(dr);

				string query = "sp_insert_data_pg_prelei_stock_receive";
				List<SqlParameter> parameters_1 = new List<SqlParameter>
				{
					new SqlParameter("@ID_RETUR", SqlDbType.Int){ Value =  },
                    new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value =  },
                    new SqlParameter("@TIPE_TUJUAN", SqlDbType.Int){ Value =  },
                    new SqlParameter("@ID_TUJUAN", SqlDbType.Int){ Value =  },
                    new SqlParameter("@NAMA_TUJUAN", SqlDbType.VarChar){ Value =  },
                    new SqlParameter("@OPERATOR_NAMA", SqlDbType.VarChar){ Value =  },
                    new SqlParameter("@KETERANGAN", SqlDbType.VarChar){ Value =  },
                    new SqlParameter("@Data", SqlDbType.Structured){ Value = dt },
                };
            }

            catch (Exception ex)
            {

            }

            index++;
        }
		*/
        Save();
        save.Visible = false;
        txt_rts.Text = "";
        ddl_to.SelectedValue = "0";
        //fillTable();
        tb.Rows.Clear();
    }

    //Fill
    protected void Save()
    {
        string nomor = "";
        string response = "";
        string querycheck = "EXEC sp_check_data_stock_return_new " + hf_id.Value + ", '" + hf_tipe_plu.Value + "'";
        nomor = Db.SingleString(querycheck, Db.Cnn_DB);

        /*string validasi_surat_jalan = "Nomor Return Tidak Ditemukan.";
		if(!string.IsNullOrEmpty(nomor))
		{
			validasi_surat_jalan = CheckSuratJalan(nomor);
		}
		
		if(!string.IsNullOrEmpty(validasi_surat_jalan))
		{
			Response.Write("<script>alert('" + validasi_surat_jalan + "')</script>");
		}
		else
		{*/
        string querystart = "DECLARE @TABLE TABLE(ID INT, [MESSAGE] VARCHAR(1000)) ";
        string queryinsertprelei = " EXEC sp_insert_data_prelei_stock_receive " + hf_id.Value + ",'" + hf_tipe_plu.Value + "',0," + hf_warehouse_id.Value + ",'" + GetNamaTujuan(Convert.ToInt32(hf_warehouse_id.Value)) + "','" + App.UserNama + "','Reborn PreLEI " + nomor + "' " +
        " DECLARE @IDForm INT = 0";

        string queryinsertdataprelei = "";
        queryinsertdataprelei += SavePG();

        querystart = querystart + " INSERT INTO @TABLE([MESSAGE], [ID]) " + queryinsertprelei + " SELECT @IDForm = (SELECT TOP 1 ID FROM @TABLE) " + queryinsertdataprelei;

        string query = "BEGIN TRY BEGIN TRAN DJANCUK " + querystart + " COMMIT TRAN DJANCUK SELECT * FROM @TABLE END TRY BEGIN CATCH SELECT 0[ID], '' [NO_PL], ERROR_MESSAGE() [MESSAGE] ROLLBACK TRAN DJANCUK END CATCH";

        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        string message = dt.Rows[0]["MESSAGE"].ToString();
        int id_cuy = Convert.ToInt32(dt.Rows[0]["ID"]);

        if (string.IsNullOrEmpty(message) && id_cuy > 0)
        {
            response = "Data Return " + nomor + " telah dibuat. Mohon dicek di menu Approval -> PreLEI.";
        }
        else
        {
            response = message;
        }

        if (string.IsNullOrEmpty(response))
            response = "Data Sukses dibuat. Mohon dicek di menu Approval -> PreLEI.";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(`" + response + "`)", true);
    }

    protected string GetNamaTujuan(int idtujuan)
    {
        if (idtujuan > 0)
        {
            string query = "SELECT Nama FROM LocWarehouse WHERE ID = @IDTujuan";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@IDTujuan", SqlDbType.Int){ Value = idtujuan }
            };
            string namatujuan = Rs(query, connleiString, parameters, CommandType.Text).Rows[0][0].ToString();
            return namatujuan;
        }
        else
        {
            return "";
        }
    }

    protected string SavePG()
    {
        string query = "";
        int index = 1;
        if (hf_tipe_plu.Value == "PG")
        {
            foreach (TableRow row in tb.Rows)
            {
                try
                {
                    HiddenField hwq1 = ((HiddenField)row.FindControl("hw" + index.ToString()));

                    TextBox txtactwq1 = (TextBox)row.FindControl("actw" + index.ToString());
                    HiddenField horiwq1 = ((HiddenField)row.FindControl("oriw" + index.ToString()));

                    TextBox txtactkbq2 = (TextBox)row.FindControl("actkb" + index.ToString());
                    HiddenField horikbq2 = ((HiddenField)row.FindControl("orikb" + index.ToString()));

                    TextBox txtactkjq3 = (TextBox)row.FindControl("actkj" + index.ToString());
                    HiddenField horikjq3 = ((HiddenField)row.FindControl("orikj" + index.ToString()));

                    DropDownList ddlacttpe_pg = (DropDownList)row.FindControl("ddltipe" + index.ToString());

                    string txthwq1 = hwq1.Value;

                    string actwq1 = txtactwq1.Text;
                    string oriwq1 = horiwq1.Value;

                    string actkbq2 = txtactkbq2.Text;
                    string orikbq2 = horikbq2.Value;

                    string actkjq3 = txtactkjq3.Text;
                    string orikjq3 = horikjq3.Value;

                    string acttpe_pg3 = ddlacttpe_pg.SelectedValue;

                    query += " EXEC sp_insert_data_pg_prelei_stock_receive @IDForm," + txthwq1 + "," + oriwq1 + "," + actwq1 + "," + orikbq2 + "," + actkbq2 + "," + orikjq3 + "," + actkjq3 + ",'" + hf_tipe_plu.Value + "','" + acttpe_pg3 + "'";
                }
                catch (Exception ex)
                {

                }

                index++;
            }
        }
        else if (hf_tipe_plu.Value == "DJ")
        {
            foreach (TableRow row in tb.Rows)
            {
                try
                {
                    HiddenField hwq1 = ((HiddenField)row.FindControl("hw" + index.ToString()));

                    TextBox txtactwq1 = (TextBox)row.FindControl("actw" + index.ToString());
                    HiddenField horiwq1 = ((HiddenField)row.FindControl("oriw" + index.ToString()));

                    string txthwq1 = hwq1.Value;
                    string actwq1 = txtactwq1.Text;
                    string oriwq1 = horiwq1.Value;

                    if (!String.IsNullOrEmpty(txthwq1))
                    {
                        query += " EXEC sp_insert_data_pg_prelei_stock_receive @IDForm,'" + txthwq1 + "'," + oriwq1 + "," + actwq1 + ",0, 0, 0, 0,'" + hf_tipe_plu.Value + "',''";

                    }

                }
                catch (Exception ex)
                {

                }

                index++;
            }
        }
        return query;
    }

    protected string GetMappingString(decimal kadartukaranjual, string producttype)
    {
        string x = string.Empty;

        if (kadartukaranjual == 84)
        {
            if (producttype == "PG")
            {
                x = "Preloved Excellent PG 75%";
            }
            else
            {
                x = "Preloved Excellent CZ 75%";
            }
        }
        else if (kadartukaranjual == 80)
        {
			kadartukaranjual = 84;
            if (producttype == "PG")
            {
                x = "Preloved Excellent PG 75%";
            }
            else
            {
                x = "Preloved Excellent CZ 75%";
            }
        }
        else if (kadartukaranjual == 40)
        {
            if (producttype == "PG")
            {
                x = "Preloved Excellent PG 37,5%";
            }
            else
            {
                x = "Preloved Excellent CZ 37,5%";
            }
        }
        else if (kadartukaranjual == 38)
        {
			kadartukaranjual = 40;
            if (producttype == "PG")
            {
                x = "Preloved Excellent PG 37,5%";
            }
            else
            {
                x = "Preloved Excellent CZ 37,5%";
            }
        }

        return x;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false)]
    public static object GetMappingLevel(decimal kadartukaranjual, decimal kadartukaranjualori, string producttype)
    {
        if (kadartukaranjual == 84)
        {
            if (producttype == "PG")
            {
                return new TgpPreloved
                {
                    Name = "Preloved Excellent PG 75%",
                    KadarTukaranJualInput = kadartukaranjual,
                    KadarTukaranJualOri = kadartukaranjualori
                };
            }
            else
            {
                return new TgpPreloved
                {
                    Name = "Preloved Excellent CZ 75%",
                    KadarTukaranJualInput = kadartukaranjual,
                    KadarTukaranJualOri = kadartukaranjualori
                };
            }
        }
        /*else if (kadartukaranjual == 80)
        {
			kadartukaranjualori = 84;
            if (producttype == "PG")
            {
                return new TgpPreloved
                {
                    Name = "Preloved Good PG 75%",
                    KadarTukaranJualInput = kadartukaranjual,
                    KadarTukaranJualOri = kadartukaranjualori
                };
            }
            else
            {
                return new TgpPreloved
                {
                    Name = "Preloved Good CZ 75%",
                    KadarTukaranJualInput = kadartukaranjual,
                    KadarTukaranJualOri = kadartukaranjualori
                };
            }
        }*/
        else if (kadartukaranjual == 40)
        {
            if (producttype == "PG")
            {
                return new TgpPreloved
                {
                    Name = "Preloved Excellent PG 37,5%",
                    KadarTukaranJualInput = kadartukaranjual,
                    KadarTukaranJualOri = kadartukaranjualori
                };
            }
            else
            {
                return new TgpPreloved
                {
                    Name = "Preloved Excellent CZ 37,5%",
                    KadarTukaranJualInput = kadartukaranjual,
                    KadarTukaranJualOri = kadartukaranjualori
                };
            }
        }
        /*else if (kadartukaranjual == 38)
        {
			kadartukaranjualori = 40;
            if (producttype == "PG")
            {
                return new TgpPreloved
                {
                    Name = "Preloved Good PG 37,5%",
                    KadarTukaranJualInput = kadartukaranjual,
                    KadarTukaranJualOri = kadartukaranjualori
                };
            }
            else
            {
                return new TgpPreloved
                {
                    Name = "Preloved Good CZ 37,5%",
                    KadarTukaranJualInput = kadartukaranjual,
                    KadarTukaranJualOri = kadartukaranjualori
                };
            }
        }*/
        else
        {
            return new TgpPreloved
            {
                Name = "",
                KadarTukaranJualInput = kadartukaranjual,
                KadarTukaranJualOri = kadartukaranjualori
            };
        }
    }

    public class TgpPreloved
    {
        public string Name { get; set; }
        public decimal KadarTukaranJualInput { get; set; }
        public decimal KadarTukaranJualOri { get; set; }
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
                foreach (var x in param)
                {
                    cmd.Parameters.Add(x);
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
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
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

    //Integer
    protected int SingleInteger(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        int x = 0;
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                foreach (var y in param)
                {
                    cmd.Parameters.Add(y);
                }
                x = (int)cmd.ExecuteScalar();
            }
        }
        return x;
    }
}