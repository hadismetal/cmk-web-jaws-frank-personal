using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Reflection;

public partial class Page_Summary : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            datepicker.Text = Str.Tgl(DateDrv.AwalBulan());
            datepicker2.Text = Str.Tgl(DateTime.Now);

            LocationList();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        err.InnerHtml = "PLU Tidak ditemukan.";
    }

    protected void refreshPage2(object sender, EventArgs e)
    {
        string keywords = kw.Text;
        string tgldari = datepicker.Text;
        string tglend = datepicker2.Text;

        int TipeLokasi = Convert.ToInt32(tipelokasi.SelectedValue);
        int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);

        string kodelokasi = "";
        try
        {
            if (TipeLokasi == 1)
            {
                kodelokasi = db.LocOutlet.SingleOrDefault(p => p.ID == IDLokasi).Kode;
            }
            else if (TipeLokasi == 2)
            {
                kodelokasi = db.LocExhibition.SingleOrDefault(p => p.ID == IDLokasi).Kode;
            }
        }
        catch
        {
            kodelokasi = "";
        }

        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);

        if (ProductLevel < 0)
            ProductLevel = 0;

        if (ProductItem < 0)
            ProductItem = 0;

        if (ProductCategory < 0)
            ProductCategory = 0;

        string query = "sp_report_repair_customer_sales_new";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@KEYWORD", SqlDbType.VarChar, 100){ Value = keywords },
            new SqlParameter("@TANGGALSTART", SqlDbType.Date){ Value = tgldari },
            new SqlParameter("@TANGGALEND", SqlDbType.Date) { Value = tglend },
            new SqlParameter("@LOCCODE", SqlDbType.VarChar, 1000){ Value = kodelokasi },
            new SqlParameter("@ITEM", SqlDbType.Int){ Value = ProductItem },
            new SqlParameter("@CATEGORY", SqlDbType.Int){ Value = ProductCategory },
            new SqlParameter("@LEVEL", SqlDbType.Int){ Value = ProductLevel },
            new SqlParameter("@STATUS", SqlDbType.Int){ Value = 0 },
            new SqlParameter("@PAGE", SqlDbType.Int){ Value = 1 },
            new SqlParameter("@ROWS", SqlDbType.Int){ Value = 20 },
            new SqlParameter("@EXCEL", SqlDbType.Bit){ Value = 1 }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
        int rowcount = dt.Rows.Count;

        if (rowcount > 0)
        {
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                int No = Convert.ToInt32(dr["NO"]);
                int ID = Convert.ToInt32(dr["ID"]);
                string NomorRpr = dr["NOMORRPR"].ToString();
                string Invoice = dr["INVOICE"].ToString();
                string PLU = dr["PLU"].ToString();
                int IDResult = Convert.ToInt32(dr["IDRESULT"]);
                string Location = dr["STORE"].ToString();
                string TglRpr = Convert.ToDateTime(dr["TGLRPR"]).ToString("dd-MM-yyyy");
                string Sales = dr["SALESNAMA"].ToString();
                string Customer = dr["CUSTNAMA"].ToString();
                string Item = dr["ITEM"].ToString();
                string Kategory = dr["KATEGORI"].ToString();
                string Level = dr["LEVEL"].ToString();
                string GrossWeightBefore = Convert.ToDecimal(dr["GROSS_WEIGHT_BEFORE"]).ToString("N2");
                string GrossWeightAfter = Convert.ToDecimal(dr["GROSS_WEIGHT_AFTER"]).ToString("N2");
                string NettoWeightBefore = Convert.ToDecimal(dr["NETTO_WEIGHT_BEFORE"]).ToString("N2");
                string NettoWeightAfter = Convert.ToDecimal(dr["NETTO_WEIGHT_AFTER"]).ToString("N2");
                string TotalButirBefore = Convert.ToInt32(dr["TOTAL_BUTIR_BEFORE"]).ToString("N0");
                string TotalButirAfter = Convert.ToInt32(dr["TOTAL_BUTIR_AFTER"]).ToString("N0");
                string TotalCaratBefore = Convert.ToDecimal(dr["TOTAL_CARAT_BEFORE"]).ToString("N3");
                string TotalCaratAfter = Convert.ToDecimal(dr["TOTAL_CARAT_AFTER"]).ToString("N3");
                string TotalBiaya = Convert.ToDecimal(dr["TOTAL_BIAYA"]).ToString("N0");
                string TotalDiscount = Convert.ToDecimal(dr["TOTAL_DISCOUNT"]).ToString("N0");
                string TotalBayar = Convert.ToDecimal(dr["TOTAL_BAYAR"]).ToString("N0");
                string Action = TD_HyperlinkPrintNew(Convert.ToInt32(dr["ID"]), Convert.ToInt32(dr["IDPRODUCT"]), 1)
                        + "&nbsp;&nbsp;&nbsp;"
                        + TD_HyperlinkPrintNew(Convert.ToInt32(dr["ID"]), Convert.ToInt32(dr["IDPRODUCT"]), 2);
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = Action;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = i.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Location.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NomorRpr;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Invoice;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = PLU;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TglRpr;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Customer;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Sales;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Item;
                tr.Cells.Add(td);

                // Gross
                td = new TableCell();
                td.Text = GrossWeightBefore;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = GrossWeightAfter;
                tr.Cells.Add(td);

                // Netto
                td = new TableCell();
                td.Text = NettoWeightBefore;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NettoWeightAfter;
                tr.Cells.Add(td);

                // Stone Qty
                td = new TableCell();
                td.Text = TotalButirBefore;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalButirAfter;
                tr.Cells.Add(td);

                // Stone Carat
                td = new TableCell();
                td.Text = TotalCaratBefore;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalCaratAfter;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalBiaya;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalDiscount;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalBayar;
                tr.Cells.Add(td);
                tb.Rows.Add(tr);

                i++;
            }
        }
    }

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType typex)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = typex;
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

    protected void refreshPage(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.DocRepair
                 where 1 == 1
                    && (p.IDProduct != null || p.IDProductTitipan != null)
                    && p.TipeLokasi != 0
                    && !p.Draft.Value
                 select p;


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
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1);
        }


        rs = rs.OrderByDescending(p => p.Tgl);

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end.AddDays(1));
        }

        int TipeLokasi = -1;
        int IDLokasi = -1;

        try
        {
            TipeLokasi = Convert.ToInt32(tipelokasi.SelectedValue);
        }
        catch (Exception) { }

        try
        {
            IDLokasi = Convert.ToInt32(lokasi.SelectedValue);
        }
        catch (Exception) { }

        if (TipeLokasi == 1 || TipeLokasi == 2)
        {
            rs = rs.Where(p => p.TipeLokasi == TipeLokasi);

            if (IDLokasi > 0)
            {
                rs = rs.Where(p => p.IDLokasi == IDLokasi);
            }
        }

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor.Contains(kw.Text));

        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == ProductCategory);

        if (rs.Count() > 0)
        {
            int index = 1;
            foreach (var r in rs)
            {
                //var s = db.DocRepairResult.SingleOrDefault(p => p.IDWorkOrder == r.ID);

                var s = (from p in db.DocRepairResult where p.IDWorkOrder == r.ID select p).OrderByDescending(p => p.OperatorTgl).FirstOrDefault();

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperlinkPrint(r, 1)
                        + "&nbsp;&nbsp;&nbsp;"
                        + TD_HyperlinkPrint(r, 2)
                        ;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                string Lokasi = "";
                if (r.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                else if (r.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_HyperlinkRepair(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (s != null) ? TD_HyperlinkResult(s) : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDProduct != null ? r.StockProductDJ.Nomor
                                              : r.StockProductDJCustomer.Nomor
                                              ;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CustomerNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDProduct != null ? r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama
                                              : r.StockProductDJCustomer.StockProductDJCustomer_CharProduct.CharProductItem.Nama
                                              ;
                tr.Cells.Add(td);

                bool isExists_DR_CP = (db.DocRepair_CharProduct.Any(u => u.ID == r.ID));
                bool isExists_DRR_CP = false;

                if (s != null)
                {
                    isExists_DRR_CP = (db.DocRepairResult_CharProduct.Any(u => u.ID == s.ID));
                }

                // Gross
                td = new TableCell();
                td.Text = isExists_DR_CP ? (r.DocRepair_CharProduct.GrossWeight != null) ? Convert.ToDecimal(r.DocRepair_CharProduct.GrossWeight.Value).ToString("N2") : "0" : "0";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = isExists_DRR_CP ? (s.DocRepairResult_CharProduct.GrossWeight != null) ? Convert.ToDecimal(s.DocRepairResult_CharProduct.GrossWeight.Value).ToString("N2") : "0" : "0";
                tr.Cells.Add(td);

                // Netto
                td = new TableCell();
                td.Text = isExists_DR_CP ? (r.DocRepair_CharProduct.NettoWeight != null) ? Convert.ToDecimal(r.DocRepair_CharProduct.NettoWeight.Value).ToString("N2") : "0" : "0";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = isExists_DRR_CP ? (s.DocRepairResult_CharProduct.NettoWeight != null) ? Convert.ToDecimal(s.DocRepairResult_CharProduct.NettoWeight.Value).ToString("N2") : "0" : "0";
                tr.Cells.Add(td);

                // Stone Qty
                td = new TableCell();
                td.Text = isExists_DR_CP ? (r.DocRepair_CharProduct.StoneQty != null) ? Convert.ToDecimal(r.DocRepair_CharProduct.StoneQty.Value).ToString("N2") : "0" : "0";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = isExists_DRR_CP ? (s.DocRepairResult_CharProduct.StoneQty != null) ? Convert.ToDecimal(s.DocRepairResult_CharProduct.StoneQty.Value).ToString("N2") : "0" : "0";
                tr.Cells.Add(td);

                // Stone Carat
                td = new TableCell();
                td.Text = isExists_DR_CP ? (r.DocRepair_CharProduct.StoneCarat != null) ? Convert.ToDecimal(r.DocRepair_CharProduct.StoneCarat.Value).ToString("N2") : "0" : "0";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = isExists_DRR_CP ? (s.DocRepairResult_CharProduct.StoneCarat != null) ? Convert.ToDecimal(s.DocRepairResult_CharProduct.StoneCarat.Value).ToString("N2") : "0" : "0";
                tr.Cells.Add(td);


                decimal TotalBiaya = 0;
                TotalBiaya = Db.SingleDecimal("SELECT HargaRupiah FROM SalesOrderRepair WHERE IDrepair=" + r.ID, Db.Cnn_DB);

                td = new TableCell();
                td.Text = TotalBiaya.ToString("N0");
                tr.Cells.Add(td);

                decimal TotalDiscount = 0;
                TotalDiscount = Db.SingleDecimal("SELECT DiscountNominal FROM SalesOrderRepair WHERE IDrepair=" + r.ID, Db.Cnn_DB);

                td = new TableCell();
                td.Text = TotalDiscount.ToString("N0");
                tr.Cells.Add(td);

                decimal TotalBayar = 0;
                TotalBayar = Db.SingleDecimal("SELECT TotalBayar FROM SalesOrderRepair WHERE IDrepair=" + r.ID, Db.Cnn_DB);

                td = new TableCell();
                td.Text = TotalBayar.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }
        }
    }

    protected string TD_HyperlinkRepair(DocRepair d)
    {
        string x = string.Empty;

        //x += "<a target='_blank' href='Repair_Item.aspx?id=" + d.StockProductDJ.ID  + "'>" + d.Nomor + "</a>";
        x += d.Nomor;

        return x;
    }

    protected string TD_HyperlinkResult(DocRepairResult d)
    {
        string x = string.Empty;

        //x += "<a target='_blank' href='Repair_Item.aspx?id=" + d.StockProductDJ.ID + "'>" + d.Nomor + "</a>";
        x += d.Nomor;

        return x;
    }

    protected string TD_HyperlinkPrint(DocRepair d, int mode = 1)
    {

        string x = string.Empty;

        if (mode == 1)
        {
            if (d.IDProduct != null)
            {
                x += "<a class=\"btn btn-warning\" target='_blank' href='Repair_Print.aspx?id=" + d.ID + "&idstock=" + d.StockProductDJ.ID + "'>";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
            }
            else
            {
                x += "<a class=\"btn btn-warning\" target='_blank' href='Repair_Print.aspx?id=" + d.ID + "'>";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
            }
        }
        else if (mode == 2)
        {
            if (d.IDProduct != null)
            {
                x += "<a class=\"btn btn-info\" target='_blank' href='Repair_Print2.aspx?id=" + d.ID + "&idstock=" + d.StockProductDJ.ID + "'>";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
            }
            else
            {
                x += "<a class=\"btn btn-info\" target='_blank' href='Repair_Print2.aspx?id=" + d.ID + "'>";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
            }
        }

        return x;
    }

    protected string TD_HyperlinkPrintNew(int id, int idproduct, int mode = 1)
    {

        string x = string.Empty;

        if (mode == 1)
        {
            if (idproduct != 0)
            {
                x += "<a class=\"btn btn-warning\" target='_blank' href='Repair_Print.aspx?id=" + id + "&idstock=" + idproduct + "'>";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
            }
            else
            {
                x += "<a class=\"btn btn-warning\" target='_blank' href='Repair_Print.aspx?id=" + id + "'>";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
            }
        }
        else if (mode == 2)
        {
            if (idproduct != 0)
            {
                x += "<a class=\"btn btn-info\" target='_blank' href='Repair_Print2.aspx?id=" + id + "&idstock=" + idproduct + "'>";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
            }
            else
            {
                x += "<a class=\"btn btn-info\" target='_blank' href='Repair_Print2.aspx?id=" + id + "'>";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
            }
        }

        return x;
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            DLC.LocationList(lokasi, Tipe);
        }
        else
        {
            lokasi.Items.Clear();
            lokasi.Items.Add(new ListItem("Lokasi", "0"));
        }
        lokasi.Focus();
    }

    protected void LocationList()
    {
        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;

        if (d.Count() > 0)
        {
            var x = d.First();
            if(x.TipeLokasi == 1)
            {
                tipelokasi.SelectedValue = "1";
                lokasi.Items.Clear();

                string query = "SELECT ID, Kode, Nama FROM LocOutlet WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ID", SqlDbType.Int){ Value = x.IDLokasi }
                };
                DataTable dt = Rs(query,Db.Cnn_DB, parameters, CommandType.Text);
                foreach (DataRow r in dt.Rows)
                {
                    string text = r["Kode"] + " (" + r["Nama"] + ")";
                    string value = r["ID"].ToString();
                    lokasi.Items.Add(new ListItem(text, value));
                }

                tipelokasi.Enabled = false;
                lokasi.Enabled = false;
            }
            else if(x.TipeLokasi == 2)
            {
                tipelokasi.SelectedValue = "2";
                lokasi.Items.Clear();

                string query = "SELECT ID, Kode, Nama FROM LocExhibition WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ID", SqlDbType.Int){ Value = x.IDLokasi }
                };
                DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);
                foreach (DataRow r in dt.Rows)
                {
                    string text = r["Kode"] + " (" + r["Nama"] + ")";
                    string value = r["ID"].ToString();
                    lokasi.Items.Add(new ListItem(text, value));
                }

                tipelokasi.Enabled = false;
                lokasi.Enabled = false;
            }
        }
    }
}