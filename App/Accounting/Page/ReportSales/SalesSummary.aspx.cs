using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;

public partial class SalesSummary : System.Web.UI.Page
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

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

            

            //Generate
            fillFilter();

            //if (App.Refresh(this)) fillTable(sender, e);
            if (App.Refresh(this)) fillDataTable(sender, e);
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


    public void fillDataTable(object sender, EventArgs e)
    {
        DateTime tempDari = DateTime.ParseExact(dari.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        string stgl1 = tempDari.ToString("yyyy-MM-dd");

        DateTime tempSampai = DateTime.ParseExact(sampai.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        string stgl2 = tempSampai.ToString("yyyy-MM-dd");

        var AdminSales = db.DataAdminSales.SingleOrDefault(p => p.IDUser == App.UserID);

        int stipelokasi;
        int sidlokasi;
        
       if ( AdminSales ==null )
        {
             stipelokasi = -1;
             sidlokasi = -1;
        }
       else
        {
            stipelokasi = AdminSales.TipeLokasi;
            sidlokasi = AdminSales.IDLokasi;
        }
        string sgroupby = groupby.SelectedValue;
        string stipeproduct = tipeproduct.SelectedValue;

        string query = " exec sp_sales_summary "
                     + "'" + stgl1 + "'"
                     + ",'" + stgl2 + "'"
                     + ",'" + stipelokasi + "'"
                     + ",'" + sidlokasi + "'"
                     + ",'" + stipeproduct + "'"
                     + ",'" + sgroupby + "'"
                     ;

        DataTable dt = new DataTable();
        dt = Db.Rs(" exec sp_sales_summary "
                     + "'" + stgl1 + "'"
                     + ",'" + stgl2 + "'"
                     + ",'" + stipelokasi + "'"
                     + ",'" + sidlokasi + "'"
                     + ",'" + stipeproduct + "'"
                     + ",'" + sgroupby + "'"
                     , System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        if (dt.Rows.Count > 0)
        {
            int index = 1;
            int indexColumn = 1;
            string result = "";

            decimal qty1 = 0m;
            decimal val1 = 0m;
            decimal qty2 = 0m;
            decimal val2 = 0m;
            decimal qty3 = 0m;
            decimal val3 = 0m;
            decimal qty4 = 0m;
            decimal val4 = 0m;
            decimal qty5 = 0m;
            decimal val5 = 0m;
            decimal qty6 = 0m;
            decimal val6 = 0m;
            decimal qty7 = 0m;
            decimal val7 = 0m;
            decimal qty8 = 0m;
            decimal val8 = 0m;

            TableRow tr;
            TableCell td;

            // On all tables' rows
            foreach (DataRow dtRow in dt.Rows)
            {
                tr = new TableRow();

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                indexColumn = 1;

                // On all tables' columns
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.Substring(0, 3) == "qty" || dc.ColumnName.Substring(0, 3) == "val")
                    {
                        result = Convert.ToDecimal(dtRow[dc]).ToString("N0");
                    }
                    else
                    {
                        result = dtRow[dc].ToString();
                    }

                    if (indexColumn == 2) { qty1 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 3) { val1 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 4) { qty2 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 5) { val2 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 6) { qty3 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 7) { val3 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 8) { qty4 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 9) { val4 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 10) { qty5 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 11) { val5 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 12) { qty6 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 13) { val6 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 14) { qty7 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 15) { val7 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 16) { qty8 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 17) { val8 += Convert.ToDecimal(dtRow[dc]); }

                    td = new TableCell();
                    td.Text = result;
                    tr.Cells.Add(td);

                    indexColumn++;
                }

                tb3.Rows.Add(tr);
                index++;
            }

            /// Grand Total
            tr = new TableRow();

            td = new TableCell();
            td.Text = "<b>" + "Total" + "</b>";
            td.HorizontalAlign = HorizontalAlign.Right;
            td.ColumnSpan = 2;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty1.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val1.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty2.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val2.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty3.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val3.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty4.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val4.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty5.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val5.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty6.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val6.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty7.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val7.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty8.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val8.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            tb3.Rows.Add(tr);

        }

    }


    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //var rs = from p in db.SalesOrderDJ
        //         where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
        //         && !(from z in db.SalesOrderDJ
        //              where z.StockProductDJ.Legacy.Value == 1
        //              select z.SalesOrder.ID).Contains(p.ID)
        //         select p;

        //var rs = from p in db.SalesOrderDJ
        //         where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
        //         && p.StockProductDJ.Legacy.Value == null
        //         && p.SalesOrder.StatusPembayaran == true
        //         select p;

        var rs = from p in db.SalesOrderDJ
                 where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                 p.SalesOrder.StatusVoid == false
                 && p.SalesOrder.Draft == false
                 && p.StockProductDJ.Legacy.Value == null
                 && p.SalesOrder.StatusPembayaran == true
                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }


        rs = rs.OrderByDescending(p => p.SalesOrder.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text));

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalBayar = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                decimal Butir = 0m;
                decimal Carat = 0m;
                int Qty = 1;

                Butir = r.StockProductDJ.StockProductDJ_Stone1A.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone1B.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone2.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone3.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone4.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone5.Sum(p => p.TotalButir);
                Carat = r.StockProductDJ.StockProductDJ_Stone1A.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone1B.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone2.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone3.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone4.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone5.Sum(p => p.TotalCarat);

                TotalHarga += r.HargaRupiah.Value;
                TotalBayar += r.TotalBayar.Value;
                TotalDiscount += r.Discount.Value;
                TotalDiscountProgram += r.DiscountProgram.Value;
                TotalDiscountGift += r.DiscountGift.Value;
                TotalDiscountRound += r.DiscountRound.Value;
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Jewellery</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0") + " %</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0") + " %</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }

        //PG

        //var pg = from p in db.SalesOrderPG
        //         where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
        //         select p;

        //var pg = from p in db.SalesOrderPG
        //         where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
        //         && p.StockProductPG.Legacy.Value != 1
        //         && p.SalesOrder.StatusPembayaran == true
        //         select p;

        var pg = from p in db.SalesOrderPG
                 //where p.SalesOrder.StatusPembayaran == false
                 where !(from z in db.SalesOrderPG
                         where z.StockProductPG.Legacy.Value == 1
                         select z.SalesOrder.ID).Contains(p.IDForm)
                 && !p.SalesOrder.Draft
                 && p.SalesOrder.StatusPembayaran == true
                 select p;


        //check admin sales
        if (ad.Count() > 0)
        {
            var c = ad.First();
            pg = pg.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            pg = pg.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }

        var s = from p in db.DataAdminSales
                where p.IDUser == App.UserID
                select p;

        if (dari.Text != "" && sampai.Text != "")
        {
            pg = pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (pg.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalBayar = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalNetWeight = 0m;
            int prev = 0;
            int index = 1;
            foreach (var r in pg)
            {
                int Qty = 1;

                TotalHarga += r.HargaRupiah.Value;
                TotalBayar += r.TotalBayar.Value;
                TotalDiscount += r.Discount.Value;
                TotalDiscountProgram += r.DiscountProgram.Value;
                TotalDiscountGift += r.DiscountGift.Value;
                TotalDiscountRound += r.DiscountRound.Value;
                TotalNetWeight += r.StockProductPG.GrossWeight.Value;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Plain Gold</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + pg.Count().ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0") + " %</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0") + " %</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>0</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>0</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }

        var rsgj = from p in db.SalesOrderGJ
                   where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
                   && p.SalesOrder.StatusPembayaran == true
                   select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rsgj = rsgj.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rsgj = rsgj.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }

        //if (Basic > 0)
        //{
        //    rsgj = from p in rsgj
        //           where (from x in db.StockBasic
        //                  select x.ProtoNomor).Contains(p.StockProductGJ.ProtoNomor)
        //           select p;
        //}

        rsgj = rsgj.OrderByDescending(p => p.SalesOrder.Tgl);

        if (keyword != "") rsgj = rsgj.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text));

        if (dari.Text != "" && sampai.Text != "")
        {
            rsgj = rsgj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rsgj.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalBayar = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            //int prev = 0;
            //int index = 1;
            foreach (var r in rsgj)
            {
                decimal Butir = 0m;
                decimal Carat = 0m;
                //int Qty = 1;

                Butir = 0m;
                Carat = 0m;

                //TotalHarga += r.HargaRupiah.Value;
                //TotalBayar += r.TotalBayar.Value;
                //TotalDiscount += r.Discount.Value;
                //TotalDiscountProgram += r.DiscountProgram.Value;
                //TotalDiscountGift += r.DiscountGift.Value;
                //TotalDiscountRound += r.DiscountRound.Value;
                //TotalButir += Butir;
                //TotalCarat += Carat;
                //TotalNetWeight += r.StockProductGJ.StockProductGJ_CharProduct.NettoWeight;

                TotalHarga += p_ForceConvertToDecimal(r.HargaRupiah);
                TotalBayar += p_ForceConvertToDecimal(r.TotalBayar);
                TotalDiscount += p_ForceConvertToDecimal(r.Discount);
                TotalDiscountProgram += p_ForceConvertToDecimal(r.DiscountProgram);
                TotalDiscountGift += p_ForceConvertToDecimal(r.DiscountGift);
                TotalDiscountRound += p_ForceConvertToDecimal(r.DiscountRound);
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += r.StockProductGJ.StockProductGJ_CharProduct.NettoWeight;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Gold Jewellery</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rsgj.Count().ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalHarga).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscount / ((rsgj.Count() > 0) ? rsgj.Count() : 1)).ToString("N0") + " %</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountProgram / ((rsgj.Count() > 0) ? rsgj.Count() : 1)).ToString("N0") + " %</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalBayar).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalButir).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalCarat).ToString("N3") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }

        //Loose Diamond
        var rsld = from p in db.SalesOrderLD
                   where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
                   && p.SalesOrder.StatusPembayaran == true
                   select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rsld = rsld.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rsld = rsld.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }

        rsld = rsld.OrderByDescending(p => p.SalesOrder.Tgl);

        if (keyword != "") rsld = rsld.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text));

        if (dari.Text != "" && sampai.Text != "")
        {
            rsld = rsld.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rsld.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalBayar = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            //int prev = 0;
            //int index = 1;
            foreach (var r in rsld)
            {
                decimal Butir = 0m;
                decimal Carat = 0m;
                //int Qty = 1;

                Butir = 1m;
                Carat = r.StockProductLD_Stone1B.CaratButir;

                //TotalHarga += r.HargaRupiah.Value;
                //TotalBayar += r.TotalBayar.Value;
                //TotalDiscount += r.Discount.Value;
                //TotalDiscountProgram += r.DiscountProgram.Value;
                //TotalDiscountGift += r.DiscountGift.Value;
                //TotalDiscountRound += r.DiscountRound.Value;
                //TotalButir += Butir;
                //TotalCarat += Carat;
                //TotalNetWeight += r.StockProductLD.StockProductLD_CharProduct.NettoWeight;

                TotalHarga += p_ForceConvertToDecimal(r.HargaRupiah);
                TotalBayar += p_ForceConvertToDecimal(r.TotalBayar);
                TotalDiscount += p_ForceConvertToDecimal(r.Discount);
                TotalDiscountProgram += p_ForceConvertToDecimal(r.DiscountProgram);
                TotalDiscountGift += p_ForceConvertToDecimal(r.DiscountGift);
                TotalDiscountRound += p_ForceConvertToDecimal(r.DiscountRound);
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += 0m; ;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Loose Diamond</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rsld.Count().ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalHarga).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscount / ((rsld.Count() > 0) ? rsld.Count() : 1)).ToString("N0") + " %</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountProgram / ((rsld.Count() > 0) ? rsld.Count() : 1)).ToString("N0") + " %</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalBayar).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalButir).ToString("N0") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalCarat).ToString("N3") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            td2.HorizontalAlign = HorizontalAlign.Right;
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }


        decimal TotalQtySales = 0m;
        decimal TotalOmzetSales = 0m;
        decimal TotalQtyResell = 0m;
        decimal TotalQtyOmset = 0m;
        decimal TotalOmzetResell = 0m;
        decimal TotalQtyTradeIn = 0m;
        decimal TotalOmzetTradeIn = 0m;
        decimal TotalQtyExchange = 0m;
        decimal TotalOmzetExchange = 0m;
        decimal TotalQtyAll = 0m;
        decimal TotalOmzetAll = 0m;
        decimal TotalGrossOmset = 0m;
        decimal TotalOmsetResellOutlet = 0m;
        decimal TotalQtyResellOutlet = 0m;
        decimal TotalOmsetResellOther = 0m;
        decimal TotalQtyResellOther = 0m;

        var o_dj = from p in db.SalesOrderDJ
                   where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                   p.SalesOrder.StatusVoid == false
                   && p.SalesOrder.Draft == false
                   && p.StockProductDJ.Legacy.Value == null
                   && p.SalesOrder.StatusPembayaran == true
                   select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            o_dj = o_dj.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            o_dj = o_dj.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }

        //if (Basic > 0)
        //{
        //    o_dj = from p in o_dj
        //           where (from x in db.StockBasic
        //                  select x.ProtoNomor).Contains(p.StockProductDJ.ProtoNomor)
        //           select p;
        //}

        o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

        if (dari.Text != "" && sampai.Text != "")
        {
            o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var r_dj = from p in db.ResellDJ
                   where !(from z in db.TradeIn
                           select z.IDResell).Contains(p.IDForm)
                   && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select p;

        var ro_dj = from p in db.ResellDJ
                    where !(from z in db.TradeIn
                            select z.IDResell).Contains(p.IDForm)
                    && p.Resell.StatusVoid == null && p.Resell.Draft == false
                    select p;

        var rot_dj = from p in db.ResellDJ
                     where !(from z in db.TradeIn
                             select z.IDResell).Contains(p.IDForm)
                     && p.Resell.StatusVoid == null && p.Resell.Draft == false
                     select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            r_dj = r_dj.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
            ro_dj = ro_dj.Where(p => p.SalesOrder.TipeLokasi != c.TipeLokasi && p.SalesOrder.IDLokasi != c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi && p.Resell.IDLokasi == c.IDLokasi);
            rot_dj = rot_dj.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi && p.Resell.IDLokasi != c.IDLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            r_dj = r_dj.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
            ro_dj = ro_dj.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
            rot_dj = rot_dj.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);

        }

        //if (Basic > 0)
        //{
        //    r_dj = from p in r_dj
        //           where (from x in db.StockBasic
        //                  select x.ProtoNomor).Contains(p.StockProductDJ.ProtoNomor)
        //           select p;
        //    ro_dj = from p in ro_dj
        //            where (from x in db.StockBasic
        //                   select x.ProtoNomor).Contains(p.StockProductDJ.ProtoNomor)
        //            select p;
        //    rot_dj = from p in rot_dj
        //             where (from x in db.StockBasic
        //                    select x.ProtoNomor).Contains(p.StockProductDJ.ProtoNomor)
        //             select p;
        //}


        r_dj = r_dj.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            r_dj = r_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            ro_dj = ro_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            rot_dj = rot_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }


        //var e_dj = from p in db.ResellDJ
        //           where (from z in db.TradeIn
        //                  where z.Draft == false
        //                  select z.IDResell).Contains(p.IDForm)
        //                  && p.Resell.Tgl.Value <= p.SalesOrder.Tgl.Value.AddDays(30)
        //                  && p.Resell.StatusVoid == null && p.Resell.Draft == false
        //           select p;

        var e_dj = from p in db.ResellDJ
                   where (from z in db.TradeIn
                          where !z.Draft
                          select z.IDResell).Contains(p.IDForm)
                           && p.Resell.Tgl.Value <= p.SalesOrder.Tgl.Value.AddDays(30)
                           && p.Resell.StatusVoid == null && !p.Resell.Draft
                           && p.SalesOrder.Tgl.HasValue
                   select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            e_dj = e_dj.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            e_dj = e_dj.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
        }

        e_dj = e_dj.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            e_dj = e_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        //var t_dj = from p in db.ResellDJ
        //           where (from z in db.TradeIn
        //                  where z.Tgl.Value >= p.SalesOrder.Tgl.Value.AddDays(30)
        //                  && z.Draft == false
        //                  select z.IDResell).Contains(p.IDForm)
        //                  && p.Resell.StatusVoid == null && p.Resell.Draft == false
        //           select p;

        //var t_dj = from p in db.ResellDJ
        //           where (from z in db.TradeIn
        //                  where z.Draft == false
        //                  select z.IDResell).Contains(p.IDForm)
        //                  && p.Resell.Tgl.Value > p.SalesOrder.Tgl.Value.AddDays(30)
        //                  && p.Resell.StatusVoid == null && p.Resell.Draft == false
        //           select p;

        var t_dj = from p in db.ResellDJ
                   where (from z in db.TradeIn
                          where !z.Draft
                          select z.IDResell).Contains(p.IDForm)
                           && p.Resell.Tgl.Value > p.SalesOrder.Tgl.Value.AddDays(30)
                           && p.Resell.StatusVoid == null && !p.Resell.Draft
                           && p.SalesOrder.Tgl.HasValue
                   select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            t_dj = t_dj.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            t_dj = t_dj.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
        }

        //var t_dj = from p in db.ResellDJ
        //           where (from z in db.TradeIn
        //                  where z.Draft == false
        //                  select z.IDResell).Contains(p.IDForm)
        //                  && p.Resell.StatusVoid == null && p.Resell.Draft == false
        //           select p;

        //var t_dj = from p in db.ResellDJ
        //           where (from z in db.TradeIn
        //                  where z.Draft == false
        //                  select z.IDResell).Contains(p.IDForm)
        //                  && p.Resell.StatusVoid == null && p.Resell.Draft == false
        //           select p;

        //if (Basic > 0)
        //{
        //    t_dj = from p in t_dj
        //           where (from x in db.StockBasic
        //                  select x.ProtoNomor).Contains(p.StockProductDJ.ProtoNomor)
        //           select p;
        //}

        t_dj = t_dj.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            t_dj = t_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }
        //var o_pg = from p in db.SalesOrderPG
        //           where /*!(from z in db.TradeIn
        //                   select z.IDSalesOrder).Contains(p.IDForm)
        //           && p.StatusResell == 0 &&*/
        //           p.SalesOrder.StatusVoid == false && p.TotalBayar.HasValue
        //           && p.SalesOrder.Draft == false
        //           select p;

        //var o_pg = from p in db.SalesOrderPG
        //           where /*!(from z in db.TradeIn
        //                       select z.IDSalesOrder).Contains(p.IDForm)
        //               && p.StatusResell == 0 &&*/
        //           p.SalesOrder.StatusVoid == false && p.TotalBayar.HasValue
        //           && p.SalesOrder.Draft == false
        //           && p.StockProductPG.Legacy.Value != 1
        //           && p.SalesOrder.StatusPembayaran == true
        //           select new
        //           {
        //               SalesOrder = p.SalesOrder,
        //               ProductItem = DLC.ProductItem(),
        //               ProductCategory = DLC.ProductCategory(),
        //               ProductLevel = DLC.ProductLevel(),
        //               ProductSegmen = DLC.ProductSegmen(),
        //               TotalBayar = p.TotalBayar,
        //           };

        var o_pg = from p in db.SalesOrderPG
                   //where p.SalesOrder.StatusPembayaran == false
                   where !(from z in db.SalesOrderPG
                           where z.StockProductPG.Legacy.Value == 1
                           select z.SalesOrder.ID).Contains(p.IDForm)
                   && !p.SalesOrder.Draft
                   && p.SalesOrder.StatusPembayaran == true
                   select new
                   {
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       TotalBayar = p.TotalBayar,
                   };

        if (ad.Count() > 0)
        {
            var c = ad.First();
            o_pg = o_pg.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            o_pg = o_pg.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }

        //var o_pg = from p in db.SalesOrderPG
        //           where /*!(from z in db.TradeIn
        //                       select z.IDSalesOrder).Contains(p.IDForm)
        //               && p.StatusResell == 0 &&*/
        //           p.SalesOrder.StatusVoid == false
        //           && p.SalesOrder.Draft == false
        //           && p.StockProductPG.Legacy.Value != 1
        //           && p.SalesOrder.StatusPembayaran == true
        //           select new
        //           {
        //               SalesOrder = p.SalesOrder,
        //               ProductItem = DLC.ProductItem(),
        //               ProductCategory = DLC.ProductCategory(),
        //               ProductLevel = DLC.ProductLevel(),
        //               ProductSegmen = DLC.ProductSegmen(),
        //               TotalBayar = p.TotalBayar,
        //           };


        o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

        if (dari.Text != "" && sampai.Text != "")
        {
            o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var r_pg = from p in db.ResellPG
                   where !(from z in db.TradeIn
                           where !z.Draft
                           select z.IDResell).Contains(p.IDForm)
                           && p.Resell.StatusVoid == null && !p.Resell.Draft
                   select new
                   {
                       Resell = p.Resell,
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       HargaRupiah = p.HargaRupiah
                   };

        var ro_pg = from p in db.ResellPG
                    where !(from z in db.TradeIn
                            select z.IDResell).Contains(p.IDForm)
                    && p.Resell.StatusVoid == null && !p.Resell.Draft

                    select new
                    {
                        Resell = p.Resell,
                        SalesOrder = p.SalesOrder,
                        ProductItem = DLC.ProductItem(),
                        ProductCategory = DLC.ProductCategory(),
                        ProductLevel = DLC.ProductLevel(),
                        ProductSegmen = DLC.ProductSegmen(),
                        HargaRupiah = p.HargaRupiah
                    };

        var rot_pg = from p in db.ResellPG
                     where !(from z in db.TradeIn
                             select z.IDResell).Contains(p.IDForm)
                     && p.Resell.StatusVoid == null && !p.Resell.Draft

                     select new
                     {
                         Resell = p.Resell,
                         SalesOrder = p.SalesOrder,
                         ProductItem = DLC.ProductItem(),
                         ProductCategory = DLC.ProductCategory(),
                         ProductLevel = DLC.ProductLevel(),
                         ProductSegmen = DLC.ProductSegmen(),
                         HargaRupiah = p.HargaRupiah
                     };

        if (ad.Count() > 0)
        {
            var c = ad.First();
            r_pg = r_pg.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
            ro_pg = ro_pg.Where(p => p.SalesOrder.TipeLokasi != c.TipeLokasi && p.SalesOrder.IDLokasi != c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi && p.Resell.IDLokasi == c.IDLokasi);
            rot_pg = rot_pg.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi && p.Resell.IDLokasi != c.IDLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            r_pg = r_pg.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
            ro_pg = ro_pg.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
            rot_pg = rot_pg.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);

        }

        

        r_pg = r_pg.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            r_pg = r_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));

        }

        var e_pg = from p in db.ResellPG
                   where (from z in db.TradeIn
                          where z.Draft == false
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.Tgl.Value <= p.SalesOrder.Tgl.Value.AddDays(30)
                          && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select new
                   {
                       Resell = p.Resell,
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       HargaRupiah = p.HargaRupiah
                   };

        if (ad.Count() > 0)
        {
            var c = ad.First();
            e_pg = e_pg.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            e_pg = e_pg.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
        }


        e_pg = e_pg.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            e_pg = e_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var t_pg = from p in db.ResellPG
                   where (from z in db.TradeIn
                          where z.Draft == false
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.Tgl.Value > p.SalesOrder.Tgl.Value.AddDays(30)
                          && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select new
                   {
                       Resell = p.Resell,
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       HargaRupiah = p.HargaRupiah
                   };

        if (ad.Count() > 0)
        {
            var c = ad.First();
            t_pg = t_pg.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            t_pg = t_pg.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
        }

        t_pg = t_pg.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            t_pg = t_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var o_gj = from p in db.SalesOrderGJ
                   where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                   p.SalesOrder.StatusVoid == false
                   && p.SalesOrder.Draft == false
                   && p.SalesOrder.StatusPembayaran == true
                   select p;

        //if (Basic > 0)
        //{
        //    o_gj = from p in o_gj
        //           where (from x in db.StockBasic
        //                  select x.ProtoNomor).Contains(p.StockProductgj.ProtoNomor)
        //           select p;
        //}

        if (ad.Count() > 0)
        {
            var c = ad.First();
            o_gj = o_gj.Where(p => p.SalesOrder.IDLokasi.Value == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            o_gj = o_gj.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }

        o_gj = o_gj.OrderByDescending(p => p.SalesOrder.Tgl);

        if (dari.Text != "" && sampai.Text != "")
        {
            o_gj = o_gj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var r_gj = from p in db.ResellGJ
                   where !(from z in db.TradeIn
                           select z.IDResell).Contains(p.IDForm)
                   && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select p;

        var ro_gj = from p in db.ResellGJ
                    where !(from z in db.TradeIn
                            select z.IDResell).Contains(p.IDForm)
                    && p.Resell.StatusVoid == null && p.Resell.Draft == false
                    select p;

        var rot_gj = from p in db.ResellGJ
                     where !(from z in db.TradeIn
                             select z.IDResell).Contains(p.IDForm)
                     && p.Resell.StatusVoid == null && p.Resell.Draft == false
                     select p;

        //if (Basic > 0)
        //{
        //    r_gj = from p in r_gj
        //           where (from x in db.StockBasic
        //                  select x.ProtoNomor).Contains(p.StockProductgj.ProtoNomor)
        //           select p;
        //    ro_gj = from p in ro_gj
        //            where (from x in db.StockBasic
        //                   select x.ProtoNomor).Contains(p.StockProductgj.ProtoNomor)
        //            select p;
        //    rot_gj = from p in rot_gj
        //             where (from x in db.StockBasic
        //                    select x.ProtoNomor).Contains(p.StockProductgj.ProtoNomor)
        //             select p;
        //}

        if (ad.Count() > 0)
        {
            var c = ad.First();
            r_gj = r_gj.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
            ro_gj = ro_gj.Where(p => p.SalesOrder.TipeLokasi != c.TipeLokasi && p.SalesOrder.IDLokasi != c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi && p.Resell.IDLokasi == c.IDLokasi);
            rot_gj = rot_gj.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi && p.Resell.IDLokasi != c.IDLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            r_gj = r_gj.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
            ro_gj = ro_gj.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
            rot_gj = rot_gj.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);

        }

        r_gj = r_gj.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            r_gj = r_gj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            ro_gj = ro_gj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            rot_gj = rot_gj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var e_gj = from p in db.ResellGJ
                   where (from z in db.TradeIn
                          where z.Draft == false
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.Tgl.Value <= p.SalesOrder.Tgl.Value.AddDays(30)
                          && p.Resell.StatusVoid == null && p.Resell.Draft == false

                   select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            e_gj = e_gj.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            e_gj = e_gj.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
        }

        e_gj = e_gj.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            e_gj = e_gj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var t_gj = from p in db.ResellGJ
                   where (from z in db.TradeIn
                          where z.Draft == false
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.Tgl.Value <= p.SalesOrder.Tgl.Value.AddDays(30)
                          && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select p;

        //var t_gj = from p in db.Resellgj
        //           where (from z in db.TradeIn
        //                  where z.Draft == false
        //                  select z.IDResell).Contains(p.IDForm)
        //                  && p.Resell.StatusVoid == null && p.Resell.Draft == false
        //           select p;

        //var t_gj = from p in db.Resellgj
        //           where (from z in db.TradeIn
        //                  where z.Draft == false
        //                  select z.IDResell).Contains(p.IDForm)
        //                  && p.Resell.StatusVoid == null && p.Resell.Draft == false
        //           select p;

        //if (Basic > 0)
        //{
        //    t_gj = from p in t_gj
        //           where (from x in db.StockBasic
        //                  select x.ProtoNomor).Contains(p.StockProductgj.ProtoNomor)
        //           select p;
        //}

        if (ad.Count() > 0)
        {
            var c = ad.First();
            t_gj = t_gj.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            t_gj = t_gj.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
        }

        t_gj = t_gj.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            t_gj = t_gj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var o_ld = from p in db.SalesOrderLD
                   where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                   p.SalesOrder.StatusVoid == false && p.TotalBayar.HasValue
                   && p.SalesOrder.Draft == false
                   && p.SalesOrder.StatusPembayaran == true
                   select new
                   {
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       TotalBayar = p.TotalBayar,
                   };

        if (ad.Count() > 0)
        {
            var c = ad.First();
            o_ld = o_ld.Where(p => p.SalesOrder.IDLokasi.Value == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            o_ld = o_ld.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }

        o_ld = o_ld.OrderByDescending(p => p.SalesOrder.Tgl);

        if (dari.Text != "" && sampai.Text != "")
        {
            o_ld = o_ld.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var r_ld = from p in db.ResellLD
                   where !(from z in db.TradeIn
                           select z.IDResell).Contains(p.IDForm)
                   && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select new
                   {
                       Resell = p.Resell,
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       HargaRupiah = p.HargaRupiah
                   };

        var ro_ld = from p in db.ResellLD
                    where !(from z in db.TradeIn
                            select z.IDResell).Contains(p.IDForm)
                    && p.Resell.StatusVoid == null && p.Resell.Draft == false
                    select new
                    {
                        Resell = p.Resell,
                        SalesOrder = p.SalesOrder,
                        ProductItem = DLC.ProductItem(),
                        ProductCategory = DLC.ProductCategory(),
                        ProductLevel = DLC.ProductLevel(),
                        ProductSegmen = DLC.ProductSegmen(),
                        HargaRupiah = p.HargaRupiah
                    };

        var rot_ld = from p in db.ResellLD
                     where !(from z in db.TradeIn
                             select z.IDResell).Contains(p.IDForm)
                     && p.Resell.StatusVoid == null && p.Resell.Draft == false
                     select new
                     {
                         Resell = p.Resell,
                         SalesOrder = p.SalesOrder,
                         ProductItem = DLC.ProductItem(),
                         ProductCategory = DLC.ProductCategory(),
                         ProductLevel = DLC.ProductLevel(),
                         ProductSegmen = DLC.ProductSegmen(),
                         HargaRupiah = p.HargaRupiah
                     };

        if (ad.Count() > 0)
        {
            var c = ad.First();
            r_ld = r_ld.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
            ro_ld = ro_ld.Where(p => p.SalesOrder.TipeLokasi != c.TipeLokasi && p.SalesOrder.IDLokasi != c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi && p.Resell.IDLokasi == c.IDLokasi);
            rot_ld = rot_ld.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi && p.Resell.IDLokasi != c.IDLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            r_ld = r_ld.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
            ro_ld = ro_ld.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
            rot_ld = rot_ld.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);

        }

        //if (Basic > 0)
        //{
        //    r_ld = from p in r_ld
        //           where (from x in db.StockBasic
        //                  select x.ProtoNomor).Contains(p.StockProductld.ProtoNomor)
        //           select p;
        //    ro_ld = from p in ro_ld
        //            where (from x in db.StockBasic
        //                   select x.ProtoNomor).Contains(p.StockProductld.ProtoNomor)
        //            select p;
        //    rot_ld = from p in rot_ld
        //             where (from x in db.StockBasic
        //                    select x.ProtoNomor).Contains(p.StockProductld.ProtoNomor)
        //             select p;
        //}

        r_ld = r_ld.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            r_ld = r_ld.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            ro_ld = ro_ld.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            rot_ld = rot_ld.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var e_ld = from p in db.ResellLD
                   where (from z in db.TradeIn
                          where z.Draft == false
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.Tgl.Value <= p.SalesOrder.Tgl.Value.AddDays(30)
                          && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select new
                   {
                       Resell = p.Resell,
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       HargaRupiah = p.HargaRupiah
                   };

        if (ad.Count() > 0)
        {
            var c = ad.First();
            e_ld = e_ld.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            e_ld = e_ld.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
        }

        e_ld = e_ld.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            e_ld = e_ld.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var t_ld = from p in db.ResellLD
                   where (from z in db.TradeIn
                          where z.Draft == false
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.Tgl.Value <= p.SalesOrder.Tgl.Value.AddDays(30)
                          && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select new
                   {
                       Resell = p.Resell,
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       HargaRupiah = p.HargaRupiah
                   };

        //var t_ld = from p in db.Resellld
        //           where (from z in db.TradeIn
        //                  where z.Draft == false
        //                  select z.IDResell).Contains(p.IDForm)
        //                  && p.Resell.StatusVoid == null && p.Resell.Draft == false
        //           select p;

        //var t_ld = from p in db.Resellld
        //           where (from z in db.TradeIn
        //                  where z.Draft == false
        //                  select z.IDResell).Contains(p.IDForm)
        //                  && p.Resell.StatusVoid == null && p.Resell.Draft == false
        //           select p;

        //if (Basic > 0)
        //{
        //    t_ld = from p in t_ld
        //           where (from x in db.StockBasic
        //                  select x.ProtoNomor).Contains(p.StockProductld.ProtoNomor)
        //           select p;
        //}

        if (ad.Count() > 0)
        {
            var c = ad.First();
            t_ld = t_ld.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
        }

        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            t_ld = t_ld.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
        }

        t_ld = t_ld.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            t_ld = t_ld.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        #region group by product type
        if (groupby.SelectedValue == "0")
        {

            if (1 == 1)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;

                Qty = o_dj.Count() - r_dj.Count() - t_dj.Count() - e_dj.Count();
                GrossOmzet = Convert.ToDecimal(o_dj.Sum(p => p.TotalBayar)) - Convert.ToDecimal(e_dj.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(t_dj.Sum(p => p.HargaRupiah));
                NetOmzet = Convert.ToDecimal(o_dj.Sum(p => p.TotalBayar)) - Convert.ToDecimal(e_dj.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(t_dj.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(r_dj.Sum(p => p.HargaRupiah));

                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Value</b>";
                td_h.Width = 150;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Value</b>";
                td_h.Width = 150;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Value</b>";
                td_h.Width = 150;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Value</b>";
                td_h.Width = 160;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Value</b>";
                td_h.Width = 150;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Value</b>";
                td_h.Width = 180;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Value</b>";
                td_h.Width = 180;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;

                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Value</b>";
                td_h.Width = 180;

                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Diamond Jewellery";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_dj.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(o_dj.Sum(p => p.TotalBayar)).ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = e_dj.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(e_dj.Sum(p => p.HargaRupiah)).ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = t_dj.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(t_dj.Sum(p => p.HargaRupiah)).ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = GrossOmzet.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r_dj.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r_dj.Sum(p => p.HargaRupiah)).ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rot_dj.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rot_dj.Sum(p => p.HargaRupiah)).ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ro_dj.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(ro_dj.Sum(p => p.HargaRupiah)).ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NetOmzet.ToString("N0");

                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalOmzetSales += Convert.ToDecimal(o_dj.Sum(p => p.TotalBayar));
                TotalQtySales += Convert.ToDecimal(o_dj.Count());
                TotalOmzetResell += Convert.ToDecimal(r_dj.Sum(p => p.HargaRupiah));
                TotalQtyResell += Convert.ToDecimal(r_dj.Count());
                TotalOmzetExchange += Convert.ToDecimal(e_dj.Sum(p => p.HargaRupiah));
                TotalQtyExchange += Convert.ToDecimal(e_dj.Count());
                TotalOmzetTradeIn += Convert.ToDecimal(t_dj.Sum(p => p.HargaRupiah));
                TotalQtyTradeIn += Convert.ToDecimal(t_dj.Count());
                TotalOmsetResellOutlet += Convert.ToDecimal(ro_dj.Sum(p => p.HargaRupiah));
                TotalQtyResellOutlet += Convert.ToDecimal(ro_dj.Count());
                TotalOmsetResellOther += Convert.ToDecimal(rot_dj.Sum(p => p.HargaRupiah));
                TotalQtyResellOther += Convert.ToDecimal(rot_dj.Count());

                TotalQtyAll += Qty;
                TotalOmzetAll += NetOmzet;
                TotalGrossOmset += GrossOmzet;
            }


            //PG
            if (1 == 1)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;
                decimal Exchange = 0m;
                try
                {
                    Exchange = Convert.ToDecimal(e_pg.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }
                decimal TradeIn = 0m;
                try
                {
                    TradeIn = Convert.ToDecimal(t_pg.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                decimal Resell = 0m;
                try
                {
                    Resell = Convert.ToDecimal(r_pg.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                decimal ResellOther = 0m;
                try
                {
                    ResellOther = Convert.ToDecimal(rot_pg.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                decimal ResellOutlet = 0m;
                try
                {
                    ResellOutlet = Convert.ToDecimal(ro_pg.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                Qty = o_pg.Count() - t_pg.Count() - e_pg.Count();
                //GrossOmzet = Convert.ToDecimal(o_pg.Sum(p => p.TotalBayar.Value)) - Exchange - TradeIn;
                GrossOmzet = Convert.ToDecimal((o_pg.Count() > 0) ? o_pg.Where(p => p.TotalBayar.HasValue).Sum(p => p.TotalBayar.Value) : 0) - Exchange - TradeIn;
                NetOmzet = Convert.ToDecimal((o_pg.Count() > 0) ? o_pg.Where(p => p.TotalBayar.HasValue).Sum(p => p.TotalBayar.Value) : 0) - Exchange - TradeIn;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Plain Gold";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_pg.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(o_pg.Sum(p => p.TotalBayar)).ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = e_pg.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = t_pg.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TradeIn.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = GrossOmzet.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r_pg.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rot_pg.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ResellOther.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ro_pg.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ResellOutlet.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NetOmzet.ToString("N0");

                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalOmzetSales += Convert.ToDecimal(o_pg.Sum(p => p.TotalBayar));
                TotalQtySales += Convert.ToDecimal(o_pg.Count());
                TotalOmzetResell += Resell;
                TotalQtyResell += Convert.ToDecimal(r_pg.Count());
                TotalOmzetExchange += Exchange;
                TotalQtyExchange += Convert.ToDecimal(e_pg.Count());
                TotalOmzetTradeIn += TradeIn;
                TotalQtyTradeIn += Convert.ToDecimal(t_pg.Count());
                TotalOmsetResellOutlet += Convert.ToDecimal(ro_pg.Sum(p => p.HargaRupiah));
                TotalQtyResellOutlet += Convert.ToDecimal(ro_pg.Count());
                TotalOmsetResellOther += Convert.ToDecimal(rot_pg.Sum(p => p.HargaRupiah));
                TotalQtyResellOther += Convert.ToDecimal(rot_pg.Count());

                TotalQtyAll += Qty;
                TotalOmzetAll += NetOmzet;
                TotalGrossOmset += GrossOmzet;
            }


            //GJ
            if (1 == 1)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;
                decimal Exchange = 0m;
                try
                {
                    Exchange = Convert.ToDecimal(e_gj.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }
                decimal TradeIn = 0m;
                try
                {
                    TradeIn = Convert.ToDecimal(t_gj.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                decimal Resell = 0m;
                try
                {
                    Resell = Convert.ToDecimal(r_gj.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                decimal ResellOther = 0m;
                try
                {
                    ResellOther = Convert.ToDecimal(rot_gj.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                decimal ResellOutlet = 0m;
                try
                {
                    ResellOutlet = Convert.ToDecimal(ro_gj.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                Qty = o_gj.Count() - r_gj.Count() - t_gj.Count() - e_gj.Count();
                //GrossOmzet = Convert.ToDecimal(o_pg.Sum(p => p.TotalBayar.Value)) - Exchange - TradeIn;
                GrossOmzet = Convert.ToDecimal((o_gj.Count() > 0) ? o_gj.Where(p => p.TotalBayar.HasValue).Sum(p => p.TotalBayar.Value) : 0) - Exchange - TradeIn;
                NetOmzet = Convert.ToDecimal((o_gj.Count() > 0) ? o_gj.Where(p => p.TotalBayar.HasValue).Sum(p => p.TotalBayar.Value) : 0) - Exchange - TradeIn - Resell;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Gold Jewellery";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_gj.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(o_gj.Sum(p => p.TotalBayar)).ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = e_gj.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = t_gj.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TradeIn.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = GrossOmzet.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r_gj.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rot_gj.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ResellOther.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ro_gj.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ResellOutlet.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NetOmzet.ToString("N0");

                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalOmzetSales += Convert.ToDecimal(o_gj.Sum(p => p.TotalBayar));
                TotalQtySales += Convert.ToDecimal(o_gj.Count());
                TotalOmzetResell += Resell;
                TotalQtyResell += Convert.ToDecimal(r_gj.Count());
                TotalOmzetExchange += Exchange;
                TotalQtyExchange += Convert.ToDecimal(e_gj.Count());
                TotalOmzetTradeIn += TradeIn;
                TotalQtyTradeIn += Convert.ToDecimal(t_gj.Count());
                TotalOmsetResellOutlet += Convert.ToDecimal(ro_gj.Sum(p => p.HargaRupiah));
                TotalQtyResellOutlet += Convert.ToDecimal(ro_gj.Count());
                TotalOmsetResellOther += Convert.ToDecimal(rot_gj.Sum(p => p.HargaRupiah));
                TotalQtyResellOther += Convert.ToDecimal(rot_gj.Count());

                TotalQtyAll += Qty;
                TotalOmzetAll += NetOmzet;
                TotalGrossOmset += GrossOmzet;
            }


            //LD
            if (1 == 1)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;
                decimal Exchange = 0m;
                try
                {
                    Exchange = Convert.ToDecimal(e_ld.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }
                decimal TradeIn = 0m;
                try
                {
                    TradeIn = Convert.ToDecimal(t_ld.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                decimal Resell = 0m;
                try
                {
                    Resell = Convert.ToDecimal(r_ld.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                decimal ResellOther = 0m;
                try
                {
                    ResellOther = Convert.ToDecimal(rot_ld.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                decimal ResellOutlet = 0m;
                try
                {
                    ResellOutlet = Convert.ToDecimal(ro_ld.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                Qty = o_ld.Count() - t_ld.Count() - e_ld.Count();
                //GrossOmzet = Convert.ToDecimal(o_pg.Sum(p => p.TotalBayar.Value)) - Exchange - TradeIn;
                GrossOmzet = Convert.ToDecimal((o_ld.Count() > 0) ? o_ld.Where(p => p.TotalBayar.HasValue).Sum(p => p.TotalBayar.Value) : 0) - Exchange - TradeIn;
                NetOmzet = Convert.ToDecimal((o_ld.Count() > 0) ? o_ld.Where(p => p.TotalBayar.HasValue).Sum(p => p.TotalBayar.Value) : 0) - Exchange - TradeIn;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Loose Diamond";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_ld.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(o_ld.Sum(p => p.TotalBayar)).ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = e_ld.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = t_ld.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TradeIn.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = GrossOmzet.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r_ld.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rot_ld.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ResellOther.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ro_ld.Count().ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ResellOutlet.ToString("N0");

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NetOmzet.ToString("N0");

                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalOmzetSales += Convert.ToDecimal(o_ld.Sum(p => p.TotalBayar));
                TotalQtySales += Convert.ToDecimal(o_ld.Count());
                TotalOmzetResell += Resell;
                TotalQtyResell += Convert.ToDecimal(r_ld.Count());
                TotalOmzetExchange += Exchange;
                TotalQtyExchange += Convert.ToDecimal(e_ld.Count());
                TotalOmzetTradeIn += TradeIn;
                TotalQtyTradeIn += Convert.ToDecimal(t_ld.Count());
                TotalOmsetResellOutlet += Convert.ToDecimal(ro_ld.Sum(p => p.HargaRupiah));
                TotalQtyResellOutlet += Convert.ToDecimal(ro_ld.Count());
                TotalOmsetResellOther += Convert.ToDecimal(rot_ld.Sum(p => p.HargaRupiah));
                TotalQtyResellOther += Convert.ToDecimal(rot_ld.Count());

                TotalQtyAll += Qty;
                TotalOmzetAll += NetOmzet;
                TotalGrossOmset += GrossOmzet;
            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalGrossOmset.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
        #endregion group by product type

        #region group by product item
        else if (groupby.SelectedValue == "1")
        {
            var item = from p in db.CharProductItem
                       orderby p.Nama
                       select p;

            //Header
            TableRow tr_h = new TableRow();
            TableCell td_h;

            td_h = new TableCell();
            td_h.Text = "";
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 160;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            tb2.Rows.Add(tr_h);

            foreach (var r in item)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal TotalOmset = 0m;
                decimal TotalResell = 0m;
                decimal TotalResellOmset = 0m;
                decimal TotalTradeIn = 0m;
                decimal TotalExchange = 0m;
                decimal Qty = 0m;
                decimal QtyResell = 0m;
                decimal QtyResellOmset = 0m;
                decimal QtyTradeIn = 0m;
                decimal QtyExchange = 0m;
                decimal Exchange = 0m;
                decimal TradeIn = 0m;
                decimal ResellOther = 0m;
                decimal ResellOutlet = 0m;
                decimal QtyResellOther = 0m;
                decimal QtyResellOutlet = 0m;

                int prev = 0;
                int index = 1;

                var d = from p in o_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                        select p;

                var d2 = from p in o_pg
                         where p.ProductItem == r.ID
                         select p;

                var d3 = from p in o_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                         select p;

                var d4 = from p in o_ld
                         where p.ProductItem == r.ID
                         select p;

                var f = from p in t_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                        select p;

                var f2 = from p in t_pg
                         where p.ProductItem == r.ID
                         select p;

                var f3 = from p in t_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                         select p;

                var f4 = from p in t_ld
                         where p.ProductItem == r.ID
                         select p;

                var g = from p in r_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                        select p;

                var g2 = from p in r_pg
                         where p.ProductItem == r.ID
                         select p;

                var g3 = from p in r_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                         select p;

                var g4 = from p in r_ld
                         where p.ProductItem == r.ID
                         select p;

                var h = from p in e_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                        select p;

                var h2 = from p in e_pg
                         where p.ProductItem == r.ID
                         select p;

                var h3 = from p in e_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                         select p;

                var h4 = from p in e_ld
                         where p.ProductItem == r.ID
                         select p;

                var i = from p in rot_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                        select p;

                var i2 = from p in rot_pg
                         where p.ProductItem == r.ID
                         select p;

                var i3 = from p in rot_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                         select p;

                var i4 = from p in rot_ld
                         where p.ProductItem == r.ID
                         select p;

                var j = from p in ro_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                        select p;

                var j2 = from p in ro_pg
                         where p.ProductItem == r.ID
                         select p;

                var j3 = from p in ro_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                         select p;

                var j4 = from p in ro_ld
                         where p.ProductItem == r.ID
                         select p;

                Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                QtyResellOmset = g.Count() + g3.Count();
                QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                QtyExchange = h.Count() + h2.Count() + h3.Count() + h4.Count();

                //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));

                TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                TotalResellOmset = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                TotalExchange = p_ForceConvertToDecimal(h.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h4.Sum(p => p.HargaRupiah));

                GrossOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - TotalExchange - TotalTradeIn;
                NetOmzet = GrossOmzet - TotalResellOmset;

                ResellOther = Convert.ToDecimal((i.Count() > 0) ? i.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i2.Count() > 0) ? i2.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i3.Count() > 0) ? i3.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i4.Count() > 0) ? i4.Sum(p => p.HargaRupiah.Value) : 0);

                ResellOutlet = Convert.ToDecimal(j != null ? ((j.Count() > 0) ? (j != null ? j.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j2 != null ? ((j2.Count() > 0) ? (j2 != null ? j2.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j3 != null ? ((j3.Count() > 0) ? (j3 != null ? j3.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j4 != null ? ((j4.Count() > 0) ? (j4 != null ? j4.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0);
                //Convert.ToDecimal((i3.Count() > 0) ? j3.Sum(p => p.HargaRupiah.Value) : 0) + 
                //Convert.ToDecimal((i4.Count() > 0) ? j4.Sum(p => p.HargaRupiah.Value) : 0);

                QtyResellOther = Convert.ToDecimal(i.Count()) + Convert.ToDecimal(i2.Count()) + Convert.ToDecimal(i3.Count()) + Convert.ToDecimal(i4.Count());
                QtyResellOutlet = Convert.ToDecimal(j.Count()) + Convert.ToDecimal(j2.Count()) + Convert.ToDecimal(j3.Count()) + Convert.ToDecimal(j4.Count());

                if (1 == 1)
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalOmset.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyExchange.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalExchange.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyTradeIn.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalTradeIn.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossOmzet.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResell.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalResell.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOther.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOther.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOutlet.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOutlet.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();

                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NetOmzet.ToString("N0");

                    tr.Cells.Add(td);

                    tb2.Rows.Add(tr);

                    TotalOmzetSales += p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalQtySales += p_ForceConvertToDecimal(Qty);
                    TotalQtyExchange += p_ForceConvertToDecimal(QtyExchange);
                    //TotalOmzetTradeIn += p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah) + f2.Sum(p => p.HargaRupiah) + f3.Sum(p => p.HargaRupiah) + f4.Sum(p => p.HargaRupiah));
                    TotalOmzetTradeIn += TotalTradeIn;
                    TotalOmzetExchange += TotalExchange;
                    TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                    //TotalOmzetResell += p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah) + g2.Sum(p => p.HargaRupiah) + g3.Sum(p => p.HargaRupiah) + g4.Sum(p => p.HargaRupiah));
                    TotalOmzetResell += TotalResell;
                    TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                    TotalQtyAll += Qty - QtyResell - QtyExchange - QtyTradeIn;
                    TotalOmzetAll += NetOmzet;
                    TotalGrossOmset += GrossOmzet;
                    TotalQtyResellOther += QtyResellOther;
                    TotalOmsetResellOther += ResellOther;
                    TotalQtyResellOutlet += QtyResellOutlet;
                    TotalOmsetResellOutlet += ResellOutlet;

                    TotalOmzetAll = TotalGrossOmset - TotalOmzetResell;
                }


            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalGrossOmset.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
        #endregion group by product item
        else if (groupby.SelectedValue == "2")
        {
            var item = ((from p in db.SalesOrder
                         orderby p.SalesNama
                         select new
                         {
                             IDSales = p.IDSales
                         }).Union(
                        from p in db.Resell
                        orderby p.SalesNama
                        select new
                        {
                            IDSales = p.IDSales
                        })).GroupBy(p => p.IDSales);

            //Header
            TableRow tr_h = new TableRow();
            TableCell td_h;

            td_h = new TableCell();
            td_h.Text = "";
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.HorizontalAlign = HorizontalAlign.Right;
            td_h.Width = 60;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 160;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;
            td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;
            td_h.HorizontalAlign = HorizontalAlign.Right; td_h.HorizontalAlign = HorizontalAlign.Right;
            tr_h.Cells.Add(td_h);

            tb2.Rows.Add(tr_h);

            foreach (var r in item)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal TotalOmset = 0m;
                decimal TotalResell = 0m;
                decimal TotalResellOmset = 0m;
                decimal TotalTradeIn = 0m;
                decimal TotalExchange = 0m;
                decimal Qty = 0m;
                decimal QtyResell = 0m;
                decimal QtyResellOmset = 0m;
                decimal QtyTradeIn = 0m;
                decimal QtyExchange = 0m;
                decimal Exchange = 0m;
                decimal TradeIn = 0m;
                decimal ResellOther = 0m;
                decimal ResellOutlet = 0m;
                decimal QtyResellOther = 0m;
                decimal QtyResellOutlet = 0m;

                int prev = 0;
                int index = 1;

                var d = from p in o_dj
                        where p.SalesOrder.IDSales == r.Key
                        select p;

                var d2 = from p in o_pg
                         where p.SalesOrder.IDSales == r.Key
                         select p;

                var d3 = from p in o_gj
                         where p.SalesOrder.IDSales == r.Key
                         select p;

                var d4 = from p in o_ld
                         where p.SalesOrder.IDSales == r.Key
                         select p;

                //var f = from p in t_dj
                //        where p.Resell.IDSales == r.Key
                //        select p;

                var f = from p in t_dj
                        join q in db.TradeIn on p.Resell.ID equals q.IDResell
                        where q.IDSales == r.Key
                        select p;

                var f2 = from p in t_pg
                         join q in db.TradeIn on p.Resell.ID equals q.IDResell
                         where q.IDSales == r.Key
                         select p;

                var f3 = from p in t_gj
                         join q in db.TradeIn on p.Resell.ID equals q.IDResell
                         where q.IDSales == r.Key
                         select p;

                var f4 = from p in t_ld
                         join q in db.TradeIn on p.Resell.ID equals q.IDResell
                         where q.IDSales == r.Key
                         select p;

                var g = from p in r_dj
                        where p.Resell.IDSales == r.Key
                        select p;

                var g2 = from p in r_pg
                         where p.Resell.IDSales == r.Key
                         select p;

                var g3 = from p in r_gj
                         where p.Resell.IDSales == r.Key
                         select p;

                var g4 = from p in r_ld
                         where p.Resell.IDSales == r.Key
                         select p;

                var h = from p in e_dj
                        where p.Resell.IDSales == r.Key
                        select p;

                var h2 = from p in e_pg
                         where p.Resell.IDSales == r.Key
                         select p;

                var h3 = from p in e_gj
                         where p.Resell.IDSales == r.Key
                         select p;

                var h4 = from p in e_ld
                         where p.Resell.IDSales == r.Key
                         select p;

                var i = from p in rot_dj
                        where p.Resell.IDSales == r.Key
                        select p;

                var i2 = from p in rot_pg
                         where p.Resell.IDSales == r.Key
                         select p;

                var i3 = from p in rot_gj
                         where p.Resell.IDSales == r.Key
                         select p;

                var i4 = from p in rot_ld
                         where p.Resell.IDSales == r.Key
                         select p;

                var j = from p in ro_dj
                        where p.Resell.IDSales == r.Key
                        select p;

                var j2 = from p in ro_pg
                         where p.Resell.IDSales == r.Key
                         select p;

                var j3 = from p in ro_gj
                         where p.Resell.IDSales == r.Key
                         select p;

                var j4 = from p in ro_ld
                         where p.Resell.IDSales == r.Key
                         select p;

                Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                QtyResellOmset = g.Count() + g3.Count();
                QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                QtyExchange = h.Count() + h2.Count() + h3.Count() + h4.Count();

                //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));

                TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                TotalResellOmset = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                TotalExchange = p_ForceConvertToDecimal(h.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h4.Sum(p => p.HargaRupiah));

                GrossOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - TotalExchange - TotalTradeIn;
                NetOmzet = GrossOmzet - TotalResellOmset;

                ResellOther = Convert.ToDecimal((i.Count() > 0) ? i.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i2.Count() > 0) ? i2.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i3.Count() > 0) ? i3.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i4.Count() > 0) ? i4.Sum(p => p.HargaRupiah.Value) : 0);

                ResellOutlet = Convert.ToDecimal(j != null ? ((j.Count() > 0) ? (j != null ? j.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j2 != null ? ((j2.Count() > 0) ? (j2 != null ? j2.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j3 != null ? ((j3.Count() > 0) ? (j3 != null ? j3.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j4 != null ? ((j4.Count() > 0) ? (j4 != null ? j4.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0);
                //Convert.ToDecimal((i3.Count() > 0) ? j3.Sum(p => p.HargaRupiah.Value) : 0) + 
                //Convert.ToDecimal((i4.Count() > 0) ? j4.Sum(p => p.HargaRupiah.Value) : 0);

                QtyResellOther = Convert.ToDecimal(i.Count()) + Convert.ToDecimal(i2.Count()) + Convert.ToDecimal(i3.Count()) + Convert.ToDecimal(i4.Count());
                QtyResellOutlet = Convert.ToDecimal(j.Count()) + Convert.ToDecimal(j2.Count()) + Convert.ToDecimal(j3.Count()) + Convert.ToDecimal(j4.Count());

                if (1 == 1)
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    var sn = db.DataSales.SingleOrDefault(p => p.ID == r.Key);

                    try
                    {
                        td = new TableCell();
                        td.Text = sn.Nama;
                        tr.Cells.Add(td);
                    }
                    catch (Exception ex)
                    {
                        td = new TableCell();
                        td.Text = "";
                        tr.Cells.Add(td);
                    }

                    td = new TableCell();
                    td.Text = Qty.ToString();
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalOmset.ToString("N0");
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyExchange.ToString();
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalExchange.ToString("N0");
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyTradeIn.ToString();
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalTradeIn.ToString("N0");
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossOmzet.ToString("N0");
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResell.ToString();
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalResell.ToString("N0");
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOther.ToString();
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOther.ToString("N0");
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOutlet.ToString();
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOutlet.ToString("N0");
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NetOmzet.ToString("N0");
                    td.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(td);

                    tb2.Rows.Add(tr);

                    TotalOmzetSales += p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalQtySales += p_ForceConvertToDecimal(Qty);
                    TotalQtyExchange += p_ForceConvertToDecimal(QtyExchange);
                    //TotalOmzetTradeIn += p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah) + f2.Sum(p => p.HargaRupiah) + f3.Sum(p => p.HargaRupiah) + f4.Sum(p => p.HargaRupiah));
                    TotalOmzetTradeIn += TotalTradeIn;
                    TotalOmzetExchange += TotalExchange;
                    TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                    //TotalOmzetResell += p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah) + g2.Sum(p => p.HargaRupiah) + g3.Sum(p => p.HargaRupiah) + g4.Sum(p => p.HargaRupiah));
                    TotalOmzetResell += TotalResell;
                    TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                    TotalQtyAll += Qty - QtyResell - QtyExchange - QtyTradeIn;
                    TotalOmzetAll += NetOmzet;
                    TotalGrossOmset += GrossOmzet;
                    TotalQtyResellOther += QtyResellOther;
                    TotalOmsetResellOther += ResellOther;
                    TotalQtyResellOutlet += QtyResellOutlet;
                    TotalOmsetResellOutlet += ResellOutlet;

                    TotalOmzetAll = TotalGrossOmset - TotalOmzetResell;
                }


            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyExchange.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetExchange.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalGrossOmset.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOther.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOther.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOutlet.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOutlet.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";
            td_all.HorizontalAlign = HorizontalAlign.Right;
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
        #region group by customer
        else if (groupby.SelectedValue == "3")
        {
            var sales = (from p in db.SalesOrder
                         where
                         p.StatusVoid == false
                         && p.Draft == false
                         && p.StatusPembayaran == true
                         select new
                         {
                             IDCustomer = p.IDCustomer.Value,
                             Tgl = p.Tgl.Value,
                             IDLokasi = p.IDLokasi.Value,
                             TipeLokasi = p.TipeLokasi.Value
                         }).Union
                        (from p in db.Resell
                         where
                         p.StatusVoid == null
                         && p.Draft == false
                         && !(from z in db.TradeIn
                              select z.IDResell).Contains(p.ID)
                         select new
                         {
                             IDCustomer = p.IDCustomer.Value,
                             Tgl = p.Tgl.Value,
                             IDLokasi = p.IDLokasi,
                             TipeLokasi = p.TipeLokasi
                         }).Union
                        (from p in db.Resell
                         where
                         p.StatusVoid == null
                         && p.Draft == false
                         && (from z in db.TradeIn
                             select z.IDResell).Contains(p.ID)
                         select new
                         {
                             IDCustomer = p.IDCustomer.Value,
                             Tgl = p.Tgl.Value,
                             IDLokasi = p.IDLokasi,
                             TipeLokasi = p.TipeLokasi
                         });

            if (dari.Text != "" && sampai.Text != "")
            {
                sales = sales.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var item = (from p in db.DataCustomer
                        join q in sales on p.ID equals q.IDCustomer
                        orderby p.Nama
                        select new { ID = p.ID }).GroupBy(z => z.ID);

            //Header
            TableRow tr_h = new TableRow();
            TableCell td_h;

            td_h = new TableCell();
            td_h.Text = "";
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 160;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            tb2.Rows.Add(tr_h);

            foreach (var r in item)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal TotalOmset = 0m;
                decimal TotalResell = 0m;
                decimal TotalResellOmset = 0m;
                decimal TotalTradeIn = 0m;
                decimal TotalExchange = 0m;
                decimal Qty = 0m;
                decimal QtyResell = 0m;
                decimal QtyResellOmset = 0m;
                decimal QtyTradeIn = 0m;
                decimal QtyExchange = 0m;
                decimal Exchange = 0m;
                decimal TradeIn = 0m;
                decimal ResellOther = 0m;
                decimal ResellOutlet = 0m;
                decimal QtyResellOther = 0m;
                decimal QtyResellOutlet = 0m;

                int prev = 0;
                int index = 1;

                var d = from p in o_dj
                        where p.SalesOrder.IDCustomer == r.Key
                        select p;

                var d2 = from p in o_pg
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var d3 = from p in o_gj
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var d4 = from p in o_ld
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                //var f = from p in t_dj
                //        where p.Resell.IDSales == r.Key
                //        select p;

                var f = from p in t_dj
                        join q in db.TradeIn on p.Resell.ID equals q.IDResell
                        where p.SalesOrder.IDCustomer == r.Key
                        select p;

                var f2 = from p in t_pg
                         join q in db.TradeIn on p.Resell.ID equals q.IDResell
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var f3 = from p in t_gj
                         join q in db.TradeIn on p.Resell.ID equals q.IDResell
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var f4 = from p in t_ld
                         join q in db.TradeIn on p.Resell.ID equals q.IDResell
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var g = from p in r_dj
                        where p.SalesOrder.IDCustomer == r.Key
                        select p;

                var g2 = from p in r_pg
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var g3 = from p in r_gj
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var g4 = from p in r_ld
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var h = from p in e_dj
                        where p.SalesOrder.IDCustomer == r.Key
                        select p;

                var h2 = from p in e_pg
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var h3 = from p in e_gj
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var h4 = from p in e_ld
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var i = from p in rot_dj
                        where p.SalesOrder.IDCustomer == r.Key
                        select p;

                var i2 = from p in rot_pg
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var i3 = from p in rot_gj
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var i4 = from p in rot_ld
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var j = from p in ro_dj
                        where p.SalesOrder.IDCustomer == r.Key
                        select p;

                var j2 = from p in ro_pg
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var j3 = from p in ro_gj
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                var j4 = from p in ro_ld
                         where p.SalesOrder.IDCustomer == r.Key
                         select p;

                Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                QtyResellOmset = g.Count() + g3.Count();
                QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                QtyExchange = h.Count() + h2.Count() + h3.Count() + h4.Count();

                //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));

                TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                TotalResellOmset = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                TotalExchange = p_ForceConvertToDecimal(h.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h4.Sum(p => p.HargaRupiah));

                GrossOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - TotalExchange - TotalTradeIn;
                NetOmzet = GrossOmzet - TotalResellOmset;

                ResellOther = Convert.ToDecimal((i.Count() > 0) ? i.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i2.Count() > 0) ? i2.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i3.Count() > 0) ? i3.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i4.Count() > 0) ? i4.Sum(p => p.HargaRupiah.Value) : 0);

                ResellOutlet = Convert.ToDecimal(j != null ? ((j.Count() > 0) ? (j != null ? j.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j2 != null ? ((j2.Count() > 0) ? (j2 != null ? j2.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j3 != null ? ((j3.Count() > 0) ? (j3 != null ? j3.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j4 != null ? ((j4.Count() > 0) ? (j4 != null ? j4.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0);
                //Convert.ToDecimal((i3.Count() > 0) ? j3.Sum(p => p.HargaRupiah.Value) : 0) + 
                //Convert.ToDecimal((i4.Count() > 0) ? j4.Sum(p => p.HargaRupiah.Value) : 0);

                QtyResellOther = Convert.ToDecimal(i.Count()) + Convert.ToDecimal(i2.Count()) + Convert.ToDecimal(i3.Count()) + Convert.ToDecimal(i4.Count());
                QtyResellOutlet = Convert.ToDecimal(j.Count()) + Convert.ToDecimal(j2.Count()) + Convert.ToDecimal(j3.Count()) + Convert.ToDecimal(j4.Count());

                if (1 == 1)
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    var sn = db.DataCustomer.SingleOrDefault(p => p.ID == r.Key);

                    try
                    {
                        td = new TableCell();
                        td.Text = sn.Nama;
                        tr.Cells.Add(td);
                    }
                    catch (Exception ex)
                    {
                        td = new TableCell();
                        td.Text = "";
                        tr.Cells.Add(td);
                    }

                    td = new TableCell();
                    td.Text = Qty.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalOmset.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyExchange.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalExchange.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyTradeIn.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalTradeIn.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossOmzet.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResell.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalResell.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOther.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOther.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOutlet.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOutlet.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();

                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NetOmzet.ToString("N0");

                    tr.Cells.Add(td);

                    tb2.Rows.Add(tr);

                    TotalOmzetSales += p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalQtySales += p_ForceConvertToDecimal(Qty);
                    TotalQtyExchange += p_ForceConvertToDecimal(QtyExchange);
                    //TotalOmzetTradeIn += p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah) + f2.Sum(p => p.HargaRupiah) + f3.Sum(p => p.HargaRupiah) + f4.Sum(p => p.HargaRupiah));
                    TotalOmzetTradeIn += TotalTradeIn;
                    TotalOmzetExchange += TotalExchange;
                    TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                    //TotalOmzetResell += p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah) + g2.Sum(p => p.HargaRupiah) + g3.Sum(p => p.HargaRupiah) + g4.Sum(p => p.HargaRupiah));
                    TotalOmzetResell += TotalResell;
                    TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                    TotalQtyAll += Qty - QtyResellOmset - QtyExchange - QtyTradeIn;
                    TotalOmzetAll += NetOmzet;
                    TotalGrossOmset += GrossOmzet;
                    TotalQtyResellOther += QtyResellOther;
                    TotalOmsetResellOther += ResellOther;
                    TotalQtyResellOutlet += QtyResellOutlet;
                    TotalOmsetResellOutlet += ResellOutlet;

                    TotalOmzetAll = TotalGrossOmset - TotalOmzetResell;
                }


            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalGrossOmset.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
        #endregion group by customer

        #region group by product category
        else if (groupby.SelectedValue == "4")
        {
            var item = from p in db.CharProductCategory
                       orderby p.Nama
                       select p;

            //Header
            TableRow tr_h = new TableRow();
            TableCell td_h;

            td_h = new TableCell();
            td_h.Text = "";
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 160;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            tb2.Rows.Add(tr_h);

            foreach (var r in item)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal TotalOmset = 0m;
                decimal TotalResell = 0m;
                decimal TotalResellOmset = 0m;
                decimal TotalTradeIn = 0m;
                decimal TotalExchange = 0m;
                decimal Qty = 0m;
                decimal QtyResell = 0m;
                decimal QtyResellOmset = 0m;
                decimal QtyTradeIn = 0m;
                decimal QtyExchange = 0m;
                decimal Exchange = 0m;
                decimal TradeIn = 0m;
                decimal ResellOther = 0m;
                decimal ResellOutlet = 0m;
                decimal QtyResellOther = 0m;
                decimal QtyResellOutlet = 0m;

                int prev = 0;
                int index = 1;

                var d = from p in o_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.ID
                        select p;

                var d2 = from p in o_pg
                         where p.ProductCategory == r.ID
                         select p;

                var d3 = from p in o_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                         select p;

                var d4 = from p in o_ld
                         where p.ProductCategory == r.ID
                         select p;

                var f = from p in t_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.ID
                        select p;

                var f2 = from p in t_pg
                         where p.ProductCategory == r.ID
                         select p;

                var f3 = from p in t_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                         select p;

                var f4 = from p in t_ld
                         where p.ProductCategory == r.ID
                         select p;

                var g = from p in r_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.ID
                        select p;

                var g2 = from p in r_pg
                         where p.ProductCategory == r.ID
                         select p;

                var g3 = from p in r_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                         select p;

                var g4 = from p in r_ld
                         where p.ProductCategory == r.ID
                         select p;

                var h = from p in e_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.ID
                        select p;

                var h2 = from p in e_pg
                         where p.ProductCategory == r.ID
                         select p;

                var h3 = from p in e_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                         select p;

                var h4 = from p in e_ld
                         where p.ProductCategory == r.ID
                         select p;

                var i = from p in rot_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.ID
                        select p;

                var i2 = from p in rot_pg
                         where p.ProductCategory == r.ID
                         select p;

                var i3 = from p in rot_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                         select p;

                var i4 = from p in rot_ld
                         where p.ProductCategory == r.ID
                         select p;

                var j = from p in ro_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.ID
                        select p;

                var j2 = from p in ro_pg
                         where p.ProductCategory == r.ID
                         select p;

                var j3 = from p in ro_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                         select p;

                var j4 = from p in ro_ld
                         where p.ProductCategory == r.ID
                         select p;

                Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                QtyResellOmset = g.Count() + g3.Count();
                QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                QtyExchange = h.Count() + h2.Count() + h3.Count() + h4.Count();

                //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));

                TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                TotalResellOmset = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                TotalExchange = p_ForceConvertToDecimal(h.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h4.Sum(p => p.HargaRupiah));

                GrossOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - TotalExchange - TotalTradeIn;
                NetOmzet = GrossOmzet - TotalResellOmset;

                ResellOther = Convert.ToDecimal((i.Count() > 0) ? i.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i2.Count() > 0) ? i2.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i3.Count() > 0) ? i3.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i4.Count() > 0) ? i4.Sum(p => p.HargaRupiah.Value) : 0);

                ResellOutlet = Convert.ToDecimal(j != null ? ((j.Count() > 0) ? (j != null ? j.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j2 != null ? ((j2.Count() > 0) ? (j2 != null ? j2.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j3 != null ? ((j3.Count() > 0) ? (j3 != null ? j3.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j4 != null ? ((j4.Count() > 0) ? (j4 != null ? j4.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0);
                //Convert.ToDecimal((i3.Count() > 0) ? j3.Sum(p => p.HargaRupiah.Value) : 0) + 
                //Convert.ToDecimal((i4.Count() > 0) ? j4.Sum(p => p.HargaRupiah.Value) : 0);

                QtyResellOther = Convert.ToDecimal(i.Count()) + Convert.ToDecimal(i2.Count()) + Convert.ToDecimal(i3.Count()) + Convert.ToDecimal(i4.Count());
                QtyResellOutlet = Convert.ToDecimal(j.Count()) + Convert.ToDecimal(j2.Count()) + Convert.ToDecimal(j3.Count()) + Convert.ToDecimal(j4.Count());

                if (1 == 1)
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalOmset.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyExchange.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalExchange.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyTradeIn.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalTradeIn.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossOmzet.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResell.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalResell.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOther.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOther.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOutlet.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOutlet.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();

                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NetOmzet.ToString("N0");

                    tr.Cells.Add(td);

                    tb2.Rows.Add(tr);

                    TotalOmzetSales += p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalQtySales += p_ForceConvertToDecimal(Qty);
                    TotalQtyExchange += p_ForceConvertToDecimal(QtyExchange);
                    //TotalOmzetTradeIn += p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah) + f2.Sum(p => p.HargaRupiah) + f3.Sum(p => p.HargaRupiah) + f4.Sum(p => p.HargaRupiah));
                    TotalOmzetTradeIn += TotalTradeIn;
                    TotalOmzetExchange += TotalExchange;
                    TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                    //TotalOmzetResell += p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah) + g2.Sum(p => p.HargaRupiah) + g3.Sum(p => p.HargaRupiah) + g4.Sum(p => p.HargaRupiah));
                    TotalOmzetResell += TotalResell;
                    TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                    TotalQtyAll += Qty - QtyResell - QtyExchange - QtyTradeIn;
                    TotalOmzetAll += NetOmzet;
                    TotalGrossOmset += GrossOmzet;
                    TotalQtyResellOther += QtyResellOther;
                    TotalOmsetResellOther += ResellOther;
                    TotalQtyResellOutlet += QtyResellOutlet;
                    TotalOmsetResellOutlet += ResellOutlet;

                    TotalOmzetAll = TotalGrossOmset - TotalOmzetResell;
                }


            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalGrossOmset.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
        #endregion group by product category

        #region group by product level
        else if (groupby.SelectedValue == "5")
        {
            var item = from p in db.CharProductLevel
                       orderby p.Nama
                       select p;

            //Header
            TableRow tr_h = new TableRow();
            TableCell td_h;

            td_h = new TableCell();
            td_h.Text = "";
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 160;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            tb2.Rows.Add(tr_h);

            foreach (var r in item)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal TotalOmset = 0m;
                decimal TotalResell = 0m;
                decimal TotalResellOmset = 0m;
                decimal TotalTradeIn = 0m;
                decimal TotalExchange = 0m;
                decimal Qty = 0m;
                decimal QtyResell = 0m;
                decimal QtyResellOmset = 0m;
                decimal QtyTradeIn = 0m;
                decimal QtyExchange = 0m;
                decimal Exchange = 0m;
                decimal TradeIn = 0m;
                decimal ResellOther = 0m;
                decimal ResellOutlet = 0m;
                decimal QtyResellOther = 0m;
                decimal QtyResellOutlet = 0m;

                int prev = 0;
                int index = 1;

                var d = from p in o_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.ID
                        select p;

                var d2 = from p in o_pg
                         where p.ProductLevel == r.ID
                         select p;

                var d3 = from p in o_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                         select p;

                var d4 = from p in o_ld
                         where p.ProductLevel == r.ID
                         select p;

                var f = from p in t_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.ID
                        select p;

                var f2 = from p in t_pg
                         where p.ProductLevel == r.ID
                         select p;

                var f3 = from p in t_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                         select p;

                var f4 = from p in t_ld
                         where p.ProductLevel == r.ID
                         select p;

                var g = from p in r_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.ID
                        select p;

                var g2 = from p in r_pg
                         where p.ProductLevel == r.ID
                         select p;

                var g3 = from p in r_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                         select p;

                var g4 = from p in r_ld
                         where p.ProductLevel == r.ID
                         select p;

                var h = from p in e_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.ID
                        select p;

                var h2 = from p in e_pg
                         where p.ProductLevel == r.ID
                         select p;

                var h3 = from p in e_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                         select p;

                var h4 = from p in e_ld
                         where p.ProductLevel == r.ID
                         select p;

                var i = from p in rot_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.ID
                        select p;

                var i2 = from p in rot_pg
                         where p.ProductLevel == r.ID
                         select p;

                var i3 = from p in rot_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                         select p;

                var i4 = from p in rot_ld
                         where p.ProductLevel == r.ID
                         select p;

                var j = from p in ro_dj
                        where p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.ID
                        select p;

                var j2 = from p in ro_pg
                         where p.ProductLevel == r.ID
                         select p;

                var j3 = from p in ro_gj
                         where p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                         select p;

                var j4 = from p in ro_ld
                         where p.ProductLevel == r.ID
                         select p;

                Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                QtyResellOmset = g.Count() + g3.Count();
                QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                QtyExchange = h.Count() + h2.Count() + h3.Count() + h4.Count();

                //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));

                TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                TotalResellOmset = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                TotalExchange = p_ForceConvertToDecimal(h.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h4.Sum(p => p.HargaRupiah));

                GrossOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - TotalExchange - TotalTradeIn;
                NetOmzet = GrossOmzet - TotalResellOmset;

                ResellOther = Convert.ToDecimal((i.Count() > 0) ? i.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i2.Count() > 0) ? i2.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i3.Count() > 0) ? i3.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i4.Count() > 0) ? i4.Sum(p => p.HargaRupiah.Value) : 0);

                ResellOutlet = Convert.ToDecimal(j != null ? ((j.Count() > 0) ? (j != null ? j.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j2 != null ? ((j2.Count() > 0) ? (j2 != null ? j2.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j3 != null ? ((j3.Count() > 0) ? (j3 != null ? j3.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j4 != null ? ((j4.Count() > 0) ? (j4 != null ? j4.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0);
                //Convert.ToDecimal((i3.Count() > 0) ? j3.Sum(p => p.HargaRupiah.Value) : 0) + 
                //Convert.ToDecimal((i4.Count() > 0) ? j4.Sum(p => p.HargaRupiah.Value) : 0);

                QtyResellOther = Convert.ToDecimal(i.Count()) + Convert.ToDecimal(i2.Count()) + Convert.ToDecimal(i3.Count()) + Convert.ToDecimal(i4.Count());
                QtyResellOutlet = Convert.ToDecimal(j.Count()) + Convert.ToDecimal(j2.Count()) + Convert.ToDecimal(j3.Count()) + Convert.ToDecimal(j4.Count());

                if (1 == 1)
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalOmset.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyExchange.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalExchange.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyTradeIn.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalTradeIn.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossOmzet.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResell.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalResell.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOther.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOther.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOutlet.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOutlet.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();

                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NetOmzet.ToString("N0");

                    tr.Cells.Add(td);

                    tb2.Rows.Add(tr);

                    TotalOmzetSales += p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalQtySales += p_ForceConvertToDecimal(Qty);
                    TotalQtyExchange += p_ForceConvertToDecimal(QtyExchange);
                    //TotalOmzetTradeIn += p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah) + f2.Sum(p => p.HargaRupiah) + f3.Sum(p => p.HargaRupiah) + f4.Sum(p => p.HargaRupiah));
                    TotalOmzetTradeIn += TotalTradeIn;
                    TotalOmzetExchange += TotalExchange;
                    TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                    //TotalOmzetResell += p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah) + g2.Sum(p => p.HargaRupiah) + g3.Sum(p => p.HargaRupiah) + g4.Sum(p => p.HargaRupiah));
                    TotalOmzetResell += TotalResell;
                    TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                    TotalQtyAll += Qty - QtyResell - QtyExchange - QtyTradeIn;
                    TotalOmzetAll += NetOmzet;
                    TotalGrossOmset += GrossOmzet;
                    TotalQtyResellOther += QtyResellOther;
                    TotalOmsetResellOther += ResellOther;
                    TotalQtyResellOutlet += QtyResellOutlet;
                    TotalOmsetResellOutlet += ResellOutlet;

                    TotalOmzetAll = TotalGrossOmset - TotalOmzetResell;
                }


            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalGrossOmset.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
        #endregion group by product level

        #region group by product segmen
        else if (groupby.SelectedValue == "6")
        {
            var item = from p in db.CharProductSegmen
                       orderby p.Nama
                       select p;

            //Header
            TableRow tr_h = new TableRow();
            TableCell td_h;

            td_h = new TableCell();
            td_h.Text = "";
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 160;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            tb2.Rows.Add(tr_h);

            foreach (var r in item)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal TotalOmset = 0m;
                decimal TotalResell = 0m;
                decimal TotalResellOmset = 0m;
                decimal TotalTradeIn = 0m;
                decimal TotalExchange = 0m;
                decimal Qty = 0m;
                decimal QtyResell = 0m;
                decimal QtyResellOmset = 0m;
                decimal QtyTradeIn = 0m;
                decimal QtyExchange = 0m;
                decimal Exchange = 0m;
                decimal TradeIn = 0m;
                decimal ResellOther = 0m;
                decimal ResellOutlet = 0m;
                decimal QtyResellOther = 0m;
                decimal QtyResellOutlet = 0m;

                int prev = 0;
                int index = 1;

                var d = from p in o_dj
                        where p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen == r.ID
                        select p;

                var d2 = from p in o_pg
                         where p.ProductSegmen == r.ID
                         select p;

                var d3 = from p in o_gj
                         where p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                         select p;

                var d4 = from p in o_ld
                         where p.ProductSegmen == r.ID
                         select p;

                var f = from p in t_dj
                        where p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen == r.ID
                        select p;

                var f2 = from p in t_pg
                         where p.ProductSegmen == r.ID
                         select p;

                var f3 = from p in t_gj
                         where p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                         select p;

                var f4 = from p in t_ld
                         where p.ProductSegmen == r.ID
                         select p;

                var g = from p in r_dj
                        where p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen == r.ID
                        select p;

                var g2 = from p in r_pg
                         where p.ProductSegmen == r.ID
                         select p;

                var g3 = from p in r_gj
                         where p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                         select p;

                var g4 = from p in r_ld
                         where p.ProductSegmen == r.ID
                         select p;

                var h = from p in e_dj
                        where p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen == r.ID
                        select p;

                var h2 = from p in e_pg
                         where p.ProductSegmen == r.ID
                         select p;

                var h3 = from p in e_gj
                         where p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                         select p;

                var h4 = from p in e_ld
                         where p.ProductSegmen == r.ID
                         select p;

                var i = from p in rot_dj
                        where p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen == r.ID
                        select p;

                var i2 = from p in rot_pg
                         where p.ProductSegmen == r.ID
                         select p;

                var i3 = from p in rot_gj
                         where p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                         select p;

                var i4 = from p in rot_ld
                         where p.ProductSegmen == r.ID
                         select p;

                var j = from p in ro_dj
                        where p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen == r.ID
                        select p;

                var j2 = from p in ro_pg
                         where p.ProductSegmen == r.ID
                         select p;

                var j3 = from p in ro_gj
                         where p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                         select p;

                var j4 = from p in ro_ld
                         where p.ProductSegmen == r.ID
                         select p;

                Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                QtyResellOmset = g.Count() + g3.Count();
                QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                QtyExchange = h.Count() + h2.Count() + h3.Count() + h4.Count();

                //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));

                TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                TotalResellOmset = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                TotalExchange = p_ForceConvertToDecimal(h.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h4.Sum(p => p.HargaRupiah));

                GrossOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - TotalExchange - TotalTradeIn;
                NetOmzet = GrossOmzet - TotalResellOmset;

                ResellOther = Convert.ToDecimal((i.Count() > 0) ? i.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i2.Count() > 0) ? i2.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i3.Count() > 0) ? i3.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i4.Count() > 0) ? i4.Sum(p => p.HargaRupiah.Value) : 0);

                ResellOutlet = Convert.ToDecimal(j != null ? ((j.Count() > 0) ? (j != null ? j.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j2 != null ? ((j2.Count() > 0) ? (j2 != null ? j2.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j3 != null ? ((j3.Count() > 0) ? (j3 != null ? j3.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j4 != null ? ((j4.Count() > 0) ? (j4 != null ? j4.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0);
                //Convert.ToDecimal((i3.Count() > 0) ? j3.Sum(p => p.HargaRupiah.Value) : 0) + 
                //Convert.ToDecimal((i4.Count() > 0) ? j4.Sum(p => p.HargaRupiah.Value) : 0);

                QtyResellOther = Convert.ToDecimal(i.Count()) + Convert.ToDecimal(i2.Count()) + Convert.ToDecimal(i3.Count()) + Convert.ToDecimal(i4.Count());
                QtyResellOutlet = Convert.ToDecimal(j.Count()) + Convert.ToDecimal(j2.Count()) + Convert.ToDecimal(j3.Count()) + Convert.ToDecimal(j4.Count());

                if (1 == 1)
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalOmset.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyExchange.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalExchange.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyTradeIn.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalTradeIn.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossOmzet.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResell.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalResell.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOther.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOther.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOutlet.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOutlet.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();

                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NetOmzet.ToString("N0");

                    tr.Cells.Add(td);

                    tb2.Rows.Add(tr);

                    TotalOmzetSales += p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalQtySales += p_ForceConvertToDecimal(Qty);
                    TotalQtyExchange += p_ForceConvertToDecimal(QtyExchange);
                    //TotalOmzetTradeIn += p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah) + f2.Sum(p => p.HargaRupiah) + f3.Sum(p => p.HargaRupiah) + f4.Sum(p => p.HargaRupiah));
                    TotalOmzetTradeIn += TotalTradeIn;
                    TotalOmzetExchange += TotalExchange;
                    TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                    //TotalOmzetResell += p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah) + g2.Sum(p => p.HargaRupiah) + g3.Sum(p => p.HargaRupiah) + g4.Sum(p => p.HargaRupiah));
                    TotalOmzetResell += TotalResell;
                    TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                    TotalQtyAll += Qty - QtyResell - QtyExchange - QtyTradeIn;
                    TotalOmzetAll += NetOmzet;
                    TotalGrossOmset += GrossOmzet;
                    TotalQtyResellOther += QtyResellOther;
                    TotalOmsetResellOther += ResellOther;
                    TotalQtyResellOutlet += QtyResellOutlet;
                    TotalOmsetResellOutlet += ResellOutlet;

                    TotalOmzetAll = TotalGrossOmset - TotalOmzetResell;
                }


            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalGrossOmset.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
        #endregion group by product segmen

        #region group by product outlet
        else if (groupby.SelectedValue == "7")
        {
            var item = (from p in db.LocOutlet
                        orderby p.Nama
                        select new
                        {
                            TipeLokasi = 1,
                            IDLokasi = p.ID,
                            Nama = p.Nama
                        }).Union(
                        from p in db.LocExhibition
                        orderby p.Nama
                        select new
                        {
                            TipeLokasi = 2,
                            IDLokasi = p.ID,
                            Nama = p.Nama
                        });

            //Header
            TableRow tr_h = new TableRow();
            TableCell td_h;

            td_h = new TableCell();
            td_h.Text = "";
            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 160;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 100;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Qty</b>";
            td_h.Width = 60;

            tr_h.Cells.Add(td_h);

            td_h = new TableCell();
            td_h.Text = "<b>Value</b>";
            td_h.Width = 150;

            tr_h.Cells.Add(td_h);

            tb2.Rows.Add(tr_h);

            foreach (var r in item)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal TotalOmset = 0m;
                decimal TotalResell = 0m;
                decimal TotalResellOmset = 0m;
                decimal TotalTradeIn = 0m;
                decimal TotalExchange = 0m;
                decimal Qty = 0m;
                decimal QtyResell = 0m;
                decimal QtyResellOmset = 0m;
                decimal QtyTradeIn = 0m;
                decimal QtyExchange = 0m;
                decimal Exchange = 0m;
                decimal TradeIn = 0m;
                decimal ResellOther = 0m;
                decimal ResellOutlet = 0m;
                decimal QtyResellOther = 0m;
                decimal QtyResellOutlet = 0m;

                int prev = 0;
                int index = 1;

                var d = from p in o_dj
                        where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                        select p;

                var d2 = from p in o_pg
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var d3 = from p in o_gj
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var d4 = from p in o_ld
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var f = from p in t_dj
                        where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                        select p;

                var f2 = from p in t_pg
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var f3 = from p in t_gj
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var f4 = from p in t_ld
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var g = from p in r_dj
                        where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                        select p;

                var g2 = from p in r_pg
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var g3 = from p in r_gj
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var g4 = from p in r_ld
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var h = from p in e_dj
                        where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                        select p;

                var h2 = from p in e_pg
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var h3 = from p in e_gj
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var h4 = from p in e_ld
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var i = from p in rot_dj
                        where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                        select p;

                var i2 = from p in rot_pg
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var i3 = from p in rot_gj
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var i4 = from p in rot_ld
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var j = from p in ro_dj
                        where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                        select p;

                var j2 = from p in ro_pg
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var j3 = from p in ro_gj
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                var j4 = from p in ro_ld
                         where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                         select p;

                Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                QtyResellOmset = g.Count() + g3.Count();
                QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                QtyExchange = h.Count() + h2.Count() + h3.Count() + h4.Count();

                //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));

                TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                TotalResellOmset = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                TotalExchange = p_ForceConvertToDecimal(h.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(h4.Sum(p => p.HargaRupiah));

                GrossOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - TotalExchange - TotalTradeIn;
                NetOmzet = GrossOmzet - TotalResellOmset;

                ResellOther = Convert.ToDecimal((i.Count() > 0) ? i.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i2.Count() > 0) ? i2.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i3.Count() > 0) ? i3.Sum(p => p.HargaRupiah.Value) : 0) +
                               Convert.ToDecimal((i4.Count() > 0) ? i4.Sum(p => p.HargaRupiah.Value) : 0);

                ResellOutlet = Convert.ToDecimal(j != null ? ((j.Count() > 0) ? (j != null ? j.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j2 != null ? ((j2.Count() > 0) ? (j2 != null ? j2.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j3 != null ? ((j3.Count() > 0) ? (j3 != null ? j3.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0) +
                               Convert.ToDecimal(j4 != null ? ((j4.Count() > 0) ? (j4 != null ? j4.Sum(p => p.HargaRupiah.Value) : 0) : 0) : 0);
                //Convert.ToDecimal((i3.Count() > 0) ? j3.Sum(p => p.HargaRupiah.Value) : 0) + 
                //Convert.ToDecimal((i4.Count() > 0) ? j4.Sum(p => p.HargaRupiah.Value) : 0);

                QtyResellOther = Convert.ToDecimal(i.Count()) + Convert.ToDecimal(i2.Count()) + Convert.ToDecimal(i3.Count()) + Convert.ToDecimal(i4.Count());
                QtyResellOutlet = Convert.ToDecimal(j.Count()) + Convert.ToDecimal(j2.Count()) + Convert.ToDecimal(j3.Count()) + Convert.ToDecimal(j4.Count());

                if (1 == 1)
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalOmset.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyExchange.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalExchange.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyTradeIn.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalTradeIn.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossOmzet.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResell.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalResell.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOther.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOther.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = QtyResellOutlet.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = ResellOutlet.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();

                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NetOmzet.ToString("N0");

                    tr.Cells.Add(td);

                    tb2.Rows.Add(tr);

                    TotalOmzetSales += p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalQtySales += p_ForceConvertToDecimal(Qty);
                    TotalQtyExchange += p_ForceConvertToDecimal(QtyExchange);
                    //TotalOmzetTradeIn += p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah) + f2.Sum(p => p.HargaRupiah) + f3.Sum(p => p.HargaRupiah) + f4.Sum(p => p.HargaRupiah));
                    TotalOmzetTradeIn += TotalTradeIn;
                    TotalOmzetExchange += TotalExchange;
                    TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                    //TotalOmzetResell += p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah) + g2.Sum(p => p.HargaRupiah) + g3.Sum(p => p.HargaRupiah) + g4.Sum(p => p.HargaRupiah));
                    TotalOmzetResell += TotalResell;
                    TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                    TotalQtyAll += Qty - QtyResell - QtyExchange - QtyTradeIn;
                    TotalOmzetAll += NetOmzet;
                    TotalGrossOmset += GrossOmzet;
                    TotalQtyResellOther += QtyResellOther;
                    TotalOmsetResellOther += ResellOther;
                    TotalQtyResellOutlet += QtyResellOutlet;
                    TotalOmsetResellOutlet += ResellOutlet;

                    TotalOmzetAll = TotalGrossOmset - TotalOmzetResell;
                }


            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetExchange.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalGrossOmset.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOther.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOutlet.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";

            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
        #endregion group by outlet
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            groupby.SelectedValue = Request.QueryString["groupby"];
            tipeproduct.SelectedValue = Request.QueryString["tipeproduct"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&tipeproduct=" + tipeproduct.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&basic=" + basic.Text;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "Sales_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&tipeproduct=" + tipeproduct.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&basic=" + basic.Text;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "Sales_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&tipeproduct=" + tipeproduct.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&basic=" + basic.Text;
        Response.Redirect(href);
    }
}