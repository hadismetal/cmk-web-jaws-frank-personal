using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Linq;
using System.Web.Configuration;



public partial class Page_DJ : System.Web.UI.Page
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

    

//    System.Data.Linq.DataContext dct = new System.Data.Linq.DataContext;

//    using (System.Data.Linq.DataContext dct = new System.Data.Linq.DataContext())
//{
//    dct.CommandTimeout = 3 * 60; // 3 Mins
//}

    //var objectContext = ((IObjectContextAdapter)db).ObjectContext;

    //// Sets the command timeout for all the commands
    //objectContext.CommandTimeout = 900;

    
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=ItemRekap_Export.xls");
            fillTable2(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var us = from p in db.PricingTableUS
                 where p.ID == CV.US_General_Pricing
                 select p;

        DataContext ctx = new DataContext(WebConfigurationManager.ConnectionStrings["Cnn_DB"].ToString());
        ctx.CommandTimeout = 1200000;

        var rs = (from p in db.StockProductDJ
                  where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
                            && !(from s in db.SalesOrderDJ
                                 where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                 select s.IDProduct).Contains(p.ID)
                            && !(from s in db.SalesOrderSouvenir
                                 where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                                 select s.IDProductSouvenir).Contains(p.ID)
                  && p.StockActualDJ.TipeLokasi == tipe
                  select new
                  {
                      Qty = 1,
                      Tipe = "Diamond Jewellery",
                      ApprovalTgl = p.ApprovalTgl,
                      TipeLokasi = p.StockActualDJ.TipeLokasi,
                      IDLokasi = p.StockActualDJ.IDLokasi,
                      Substorage = p.StockActualDJ.Substorage,
                      HargaM = Convert.ToDecimal((p.StockProductDJ_Costing.TotalBiaya + p.StockProductDJ_CostingProduct.Total)),
                      HargaR = Convert.ToDecimal((p.StockProductDJ_PricingMU.HargaUSD)),
                      HargaJual = p.StockProductDJ_PricingMU.HargaRupiahRound.Value,
                      GrossWeight = p.StockProductDJ_CharProduct.GrossWeight,
                      NetWeight = p.StockProductDJ_CharProduct.NettoWeight,
                      TotalButir = Convert.ToDecimal(p.StockProductDJ_CharProduct.StoneQty),
                      TotalCarat = Convert.ToDecimal(p.StockProductDJ_CharProduct.StoneCarat),
                      ProductItem = p.StockProductDJ_CharProduct.ProductItem
                  }).Concat(
                  from p in db.StockProductGJ
                  where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
                            && !(from s in db.SalesOrderGJ
                                 where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                 select s.IDProduct).Contains(p.ID)
                            && !(from s in db.SalesOrderSouvenir
                                 where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 2
                                 select s.IDProductSouvenir).Contains(p.ID)
                    && p.StockActualGJ.TipeLokasi == tipe
                  select new
                  {
                      Qty = 1,
                      Tipe = "Gold Jewellery",
                      ApprovalTgl = p.ApprovalTgl,
                      TipeLokasi = p.StockActualGJ.TipeLokasi,
                      IDLokasi = p.StockActualGJ.IDLokasi,
                      Substorage = p.StockActualGJ.Substorage,
                      HargaM = Convert.ToDecimal((p.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ_CostingProduct.Total)),
                      HargaR = Convert.ToDecimal((p.StockProductGJ_PricingMU.HargaUSD)),
                      HargaJual = p.StockProductGJ_PricingMU.HargaRupiahRound.Value,
                      GrossWeight = p.StockProductGJ_CharProduct.GrossWeight,
                      NetWeight = p.StockProductGJ_CharProduct.NettoWeight,
                      TotalButir = 0m,
                      TotalCarat = 0m,
                      ProductItem = p.StockProductGJ_CharProduct.ProductItem
                  }).Concat(
                  from p in db.StockProductLD_Stone1B
                  where p.ReturnProduct == false && !p.StockProductLD.Draft.Value //&& p.Legacy == null
                            && !(from s in db.SalesOrderLD
                                 where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                 select s.IDProduct).Contains(p.ID)
                            && !(from s in db.SalesOrderSouvenir
                                 where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 4
                                 select s.IDProductSouvenir).Contains(p.ID)
                    && p.StockActualLD_Stone1B.TipeLokasi == tipe
                  select new
                  {
                      Qty = 1,
                      Tipe = "Loose Diamond",
                      ApprovalTgl = p.StockProductLD.ApprovalTgl,
                      TipeLokasi = p.StockActualLD_Stone1B.TipeLokasi,
                      IDLokasi = p.StockActualLD_Stone1B.IDLokasi,
                      Substorage = p.StockActualLD_Stone1B.Substorage,
                      HargaM = Convert.ToDecimal(p.HargaTotalM.Value),
                      HargaR = Convert.ToDecimal(p.HargaTotal.Value),
                      HargaJual = Convert.ToDecimal((p.Stone1B.StonePricing1B.Harga.HasValue) ? p.Stone1B.StonePricing1B.Harga.Value * us.First().Harga.Value : 0m),
                      GrossWeight = 0m,
                      NetWeight = 0m,
                      TotalButir = 1m,
                      TotalCarat = Convert.ToDecimal(p.TotalCarat),
                      ProductItem = p.ProductItem.Value
                  }).Concat(
                  from p in db.StockProductPG
                  where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
                            && !(from s in db.SalesOrderPG
                                 where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                 select s.IDProduct).Contains(p.ID)
                            && !(from s in db.SalesOrderSouvenir
                                 where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 3
                                 select s.IDProductSouvenir).Contains(p.ID)
                    && p.StockActualPG.TipeLokasi == tipe
                  select new
                  {
                      Qty = 1,
                      Tipe = "Plain Gold",
                      ApprovalTgl = p.ApprovalTgl,
                      TipeLokasi = p.StockActualPG.TipeLokasi,
                      IDLokasi = p.StockActualPG.IDLokasi,
                      Substorage = p.StockActualPG.Substorage,
                      HargaM = Convert.ToDecimal(p.TotalHargaUSD.Value),
                      HargaR = 0m,
                      HargaJual = p.TotalHargaJual.Value,
                      GrossWeight = 0m,
                      NetWeight = Convert.ToDecimal(p.GrossWeight.Value),
                      TotalButir = 0m,
                      TotalCarat = 0m,
                      ProductItem = p.ProductItem
                  });

        //var rs = (from p in db.StockProductDJ
        //          where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
        //                    && !(from s in db.SalesOrderDJ
        //                         where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
        //                         select s.IDProduct).Contains(p.ID)
        //          select new
        //          {
        //              Qty = p.StockActualDJ.InHand,
        //              Tipe = "Diamond Jewellery",
        //              ApprovalTgl = p.ApprovalTgl,
        //              TipeLokasi = p.StockActualDJ.TipeLokasi,
        //              IDLokasi = p.StockActualDJ.IDLokasi,
        //              Substorage = p.StockActualDJ.Substorage,
        //          }).Union(
        //          from p in db.StockProductGJ
        //          where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
        //                    && !(from s in db.SalesOrderGJ
        //                         where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
        //                         select s.IDProduct).Contains(p.ID)
        //          select new
        //          {
        //              Qty = p.StockActualGJ.InHand,
        //              Tipe = "Gold Jewellery",
        //              ApprovalTgl = p.ApprovalTgl,
        //              TipeLokasi = p.StockActualGJ.TipeLokasi,
        //              IDLokasi = p.StockActualGJ.IDLokasi,
        //              Substorage = p.StockActualGJ.Substorage,
        //          });

        rs = rs.OrderByDescending(p => p.ApprovalTgl);
        

        int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
        if (Substorage > 0)
        {
            rs = rs.Where(p => p.Substorage == Substorage);
        }



        int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        if (Cabang > 0) rs = rs.Where(p => p.IDLokasi == Cabang);
        //else rs = rs.Where(p => p.TipeLokasi == tipe);
        //int Brand = Convert.ToInt32(brand.SelectedValue);
        //if (Brand > 0) rs = rs.Where(p => p.LocWarehouse.CompanyBrand.ID == Brand);


        //var rsg = rs.GroupBy(p => new { p.TipeLokasi, p.IDLokasi, p.Tipe });
        var rsg = from p in rs
                  group p by new { p.IDLokasi, p.TipeLokasi, p.Tipe } into g
                  select g;
        int counter = rsg.Count();
        var test = rsg.FirstOrDefault();
        var test2 = rsg.ToArray();

        decimal GTotalM = 0m;
        decimal GTotalR = 0m;
        decimal GTotalJual = 0m;
        decimal GTotalButir = 0m;
        decimal GTotalCarat = 0m;
        decimal GTotalQty = 0m;
        decimal GTotalBerat = 0m;

        if (rs != null)
        {
            int index = 1;
            int count = 1;
            int count2 = 0;

            decimal TotalM = 0m;
            decimal TotalR = 0m;
            decimal TotalJual = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalQty = 0m;
            decimal TotalBerat = 0m;

            string prev = "";
            foreach (var r in rsg)
            {

                //Sub-Header
                if (prev != r.Key.TipeLokasi.ToString() + " " + r.Key.IDLokasi.ToString())
                {
                    if (count > 1)
                    {
                        TableRow tr2 = new TableRow();
                        TableCell td2;

                        td2 = new TableCell();
                        td2.ColumnSpan = 2;
                        td2.Text = "<b>Sub Total</b>";
                        tr2.Cells.Add(td2);

                        td2 = new TableCell();
                        td2.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
                        tr2.Cells.Add(td2);

                        td2 = new TableCell();
                        td2.Text = "<b>" + TotalM.ToString("N2") + "</b>";
                        tr2.Cells.Add(td2);

                        td2 = new TableCell();
                        td2.Text = "<b>" + TotalR.ToString("N2") + "</b>";
                        tr2.Cells.Add(td2);

                        td2 = new TableCell();
                        td2.Text = "<b>" + TotalJual.ToString("N0") + "</b>";
                        tr2.Cells.Add(td2);

                        td2 = new TableCell();
                        td2.Text = "<b>" + TotalBerat.ToString("N2") + "</b>";
                        tr2.Cells.Add(td2);

                        td2 = new TableCell();
                        td2.Text = "<b>" + TotalButir.ToString("N0") + "</b>";
                        tr2.Cells.Add(td2);

                        td2 = new TableCell();
                        td2.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
                        tr2.Cells.Add(td2);

                        tb.Rows.Add(tr2);

                        TotalM = 0m;
                        TotalR = 0m;
                        TotalJual = 0m;
                        TotalButir = 0m;
                        TotalCarat = 0m;
                        TotalQty = 0m;
                        TotalBerat = 0m;
                    }

                    Sub_Header(r.Key.TipeLokasi, r.Key.IDLokasi);
                    prev = r.Key.TipeLokasi.ToString() + " " + r.Key.IDLokasi.ToString();

                    index = 1;
                    count2++;
                }

                count++;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                //string ProductItem = Db.SingleString("SELECT Nama FROM CharProductItem WHERE ID=" + r.Key.ProductItem, Db.Cnn_DB);

                td = new TableCell();
                td.Text = r.Key.Tipe;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Count().ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Sum(p => p.HargaM).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Sum(p => p.HargaR).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Sum(p => p.HargaJual).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Sum(p => p.NetWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Sum(p => p.TotalButir).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Sum(p => p.TotalCarat).ToString("N3");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalQty += r.Count();
                TotalM += r.Sum(p => p.HargaM);
                TotalR += r.Sum(p => p.HargaR);
                TotalJual += r.Sum(p => p.HargaJual);
                TotalButir += r.Sum(p => p.TotalButir);
                TotalCarat += r.Sum(p => p.TotalCarat);
                TotalBerat += r.Sum(p => p.NetWeight);


                GTotalQty += r.Count();
                GTotalM += r.Sum(p => p.HargaM);
                GTotalR += r.Sum(p => p.HargaR);
                GTotalJual += r.Sum(p => p.HargaJual);
                GTotalButir += r.Sum(p => p.TotalButir);
                GTotalCarat += r.Sum(p => p.TotalCarat);
                GTotalBerat += r.Sum(p => p.NetWeight);


                index++;

            }

            // Totalan SubTotal
            TableRow tr3 = new TableRow();
            TableCell td3;

            td3 = new TableCell();
            td3.ColumnSpan = 2;
            td3.Text = "<b>Sub Total</b>";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "<b>" + TotalM.ToString("N2") + "</b>";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "<b>" + TotalR.ToString("N2") + "</b>";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "<b>" + TotalJual.ToString("N0") + "</b>";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "<b>" + TotalBerat.ToString("N2") + "</b>";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "<b>" + TotalButir.ToString("N0") + "</b>";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
            tr3.Cells.Add(td3);

            tb.Rows.Add(tr3);

        }

        TableRow trS = new TableRow();
        TableCell tdS;

        tdS = new TableCell();
        tdS.ColumnSpan = 9;
        tdS.Text = "";
        trS.Cells.Add(tdS);

        tb.Rows.Add(trS);


        // Grand Total
        TableRow trG = new TableRow();
        TableCell tdG;

        // DJ
        if (rs.Where(p => p.Tipe == "Diamond Jewellery").Count() > 0)
        {
            tdG = new TableCell();
            tdG.ColumnSpan = 2;
            tdG.Text = "<b>Diamond Jewellery</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Diamond Jewellery").Sum(p => p.Qty)).ToString("N0") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Diamond Jewellery").Sum(p => p.HargaM)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Diamond Jewellery").Sum(p => p.HargaR)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Diamond Jewellery").Sum(p => p.HargaJual)).ToString("N0") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Diamond Jewellery").Sum(p => p.NetWeight)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Diamond Jewellery").Sum(p => p.TotalButir)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Diamond Jewellery").Sum(p => p.TotalCarat)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tb.Rows.Add(trG);
        }

        // PG
        if (rs.Where(p => p.Tipe == "Plain Gold").Count() > 0)
        {
            trG = new TableRow();

            tdG = new TableCell();
            tdG.ColumnSpan = 2;
            tdG.Text = "<b>Plain Gold</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Plain Gold").Sum(p => p.Qty)).ToString("N0") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Plain Gold").Sum(p => p.HargaM)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Plain Gold").Sum(p => p.HargaR)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Plain Gold").Sum(p => p.HargaJual)).ToString("N0") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Plain Gold").Sum(p => p.NetWeight)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Plain Gold").Sum(p => p.TotalButir)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Plain Gold").Sum(p => p.TotalCarat)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tb.Rows.Add(trG);
        }

        // GJ
        if (rs.Where(p => p.Tipe == "Gold Jewellery").Count() > 0)
        {
            trG = new TableRow();

            tdG = new TableCell();
            tdG.ColumnSpan = 2;
            tdG.Text = "<b>Gold Jewellery</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Gold Jewellery").Sum(p => p.Qty)).ToString("N0") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Gold Jewellery").Sum(p => p.HargaM)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Gold Jewellery").Sum(p => p.HargaR)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Gold Jewellery").Sum(p => p.HargaJual)).ToString("N0") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Gold Jewellery").Sum(p => p.NetWeight)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Gold Jewellery").Sum(p => p.TotalButir)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Gold Jewellery").Sum(p => p.TotalCarat)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tb.Rows.Add(trG);
        }

        // LD
        if (rs.Where(p => p.Tipe == "Loose Diamond").Count() > 0)
        {
            trG = new TableRow();

            tdG = new TableCell();
            tdG.ColumnSpan = 2;
            tdG.Text = "<b>Loose Diamond</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Loose Diamond").Sum(p => p.Qty)).ToString("N0") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Loose Diamond").Sum(p => p.HargaM)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Loose Diamond").Sum(p => p.HargaR)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Loose Diamond").Sum(p => p.HargaJual)).ToString("N0") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Loose Diamond").Sum(p => p.NetWeight)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Loose Diamond").Sum(p => p.TotalButir)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tdG = new TableCell();
            tdG.Text = "<b>" + (rs.Where(p => p.Tipe == "Loose Diamond").Sum(p => p.TotalCarat)).ToString("N2") + "</b>";
            trG.Cells.Add(tdG);

            tb.Rows.Add(trG);
        }

        // GRAND TOTAL
        trG = new TableRow();

        tdG = new TableCell();
        tdG.ColumnSpan = 2;
        tdG.Text = "<b>Grand Total</b>";
        trG.Cells.Add(tdG);

        tdG = new TableCell();
        tdG.Text = "<b>" + GTotalQty.ToString("N0") + "</b>";
        trG.Cells.Add(tdG);

        tdG = new TableCell();
        tdG.Text = "<b>" + GTotalM.ToString("N2") + "</b>";
        trG.Cells.Add(tdG);

        tdG = new TableCell();
        tdG.Text = "<b>" + GTotalR.ToString("N2") + "</b>";
        trG.Cells.Add(tdG);

        tdG = new TableCell();
        tdG.Text = "<b>" + GTotalJual.ToString("N0") + "</b>";
        trG.Cells.Add(tdG);

        tdG = new TableCell();
        tdG.Text = "<b>" + GTotalBerat.ToString("N2") + "</b>";
        trG.Cells.Add(tdG);

        tdG = new TableCell();
        tdG.Text = "<b>" + GTotalButir.ToString("N0") + "</b>";
        trG.Cells.Add(tdG);

        tdG = new TableCell();
        tdG.Text = "<b>" + GTotalCarat.ToString("N3") + "</b>";
        trG.Cells.Add(tdG);

        tb.Rows.Add(trG);


    }

    protected void fillTable2(object sender, EventArgs e)
    {
        string spname = "sp_reportitemrekap";
        string query = " exec " + spname
                     + " " + tipe.ToString() + ""
                     + ", " + Request.QueryString["cabang"].ToString() + ""
                     + ", " + Request.QueryString["substorage"].ToString() + ""
                     ;
        try
        {
            DataTable dt = new DataTable();
            dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

            if (dt.Rows.Count > 0)
            {
                string location = "";
                string keterangan = "";
                string productordernumber = "";

                decimal TotalM = 0m;
                decimal TotalR = 0m;
                decimal TotalJual = 0m;
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                decimal TotalQty = 0m;
                decimal TotalBerat = 0m;

                decimal STotalM = 0m;
                decimal STotalR = 0m;
                decimal STotalJual = 0m;
                decimal STotalButir = 0m;
                decimal STotalCarat = 0m;
                decimal STotalQty = 0m;
                decimal STotalBerat = 0m;

                decimal G1TotalM = 0m;
                decimal G1TotalR = 0m;
                decimal G1TotalJual = 0m;
                decimal G1TotalButir = 0m;
                decimal G1TotalCarat = 0m;
                decimal G1TotalQty = 0m;
                decimal G1TotalBerat = 0m;

                decimal G2TotalM = 0m;
                decimal G2TotalR = 0m;
                decimal G2TotalJual = 0m;
                decimal G2TotalButir = 0m;
                decimal G2TotalCarat = 0m;
                decimal G2TotalQty = 0m;
                decimal G2TotalBerat = 0m;

                decimal G3TotalM = 0m;
                decimal G3TotalR = 0m;
                decimal G3TotalJual = 0m;
                decimal G3TotalButir = 0m;
                decimal G3TotalCarat = 0m;
                decimal G3TotalQty = 0m;
                decimal G3TotalBerat = 0m;

                decimal G4TotalM = 0m;
                decimal G4TotalR = 0m;
                decimal G4TotalJual = 0m;
                decimal G4TotalButir = 0m;
                decimal G4TotalCarat = 0m;
                decimal G4TotalQty = 0m;
                decimal G4TotalBerat = 0m;

                decimal GTotalM = 0m;
                decimal GTotalR = 0m;
                decimal GTotalJual = 0m;
                decimal GTotalButir = 0m;
                decimal GTotalCarat = 0m;
                decimal GTotalQty = 0m;
                decimal GTotalBerat = 0m;

                string flagLoclast = "";
                string flagLoccurrent = "";

                int idxrow = 0;
                int idxlocation = 1;

                TableRow tr;
                TableCell td;

                TableRow trS;
                TableCell tdS;

                TableRow trG;
                TableCell tdG;

                int index = 1;

                foreach (DataRow dtRow in dt.Rows)
                {
                    location = dtRow["LocName"].ToString();
                    keterangan = dtRow["TipeProduct"].ToString();
                    productordernumber = dtRow["OrderNumber"].ToString();

                    //lit_test.Text += "<br />" + location + " - " + keterangan;

                    flagLoccurrent = location;
                    if (flagLoclast != flagLoccurrent)
                    {
                        // subtotal (from first)
                        if (idxrow > 0)
                        {
                            string queryBudget = "SELECT A.* FROM PricingTableFB A JOIN LocOutlet B ON A.IDLokasi = B.ID AND A.TipeLokasi = 1 WHERE B.Nama = '" + flagLoclast + "'";
                            DataTable dtBudget = new DataTable();
                            dtBudget = Db.Rs(queryBudget, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

                            index = 1;
                            tr = new TableRow();
                            tr.Style.Add("font-weight", "bold");
                            tr.Style.Add("background-color", "#eaf8fd");

                            td = new TableCell();
                            td.Text = "Sub Total ";
                            td.Style.Add("text-align", "right");
                            td.ColumnSpan = 2;
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number0(STotalQty);
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number2(STotalM);
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number2(STotalR);
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number0(STotalJual);
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number2(STotalBerat);
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number0(STotalButir);
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number3(STotalCarat);
                            tr.Cells.Add(td);

                            tb.Rows.Add(tr);

                            if (dtBudget.Rows.Count > 0)
                            {
                                DataRow drBudget = dtBudget.Rows[0];

                                decimal selisihQty = STotalQty - Convert.ToDecimal(drBudget["Qty"]);
                                decimal selisihCOGS = STotalR - Convert.ToDecimal(drBudget["HargaCOGS"]);
                                decimal selisihJual = STotalJual - Convert.ToDecimal(drBudget["HargaJual"]);

                                tr = new TableRow();
                                tr.Style.Add("font-weight", "bold");
                                tr.Style.Add("background-color", "#FFFD95");

                                td = new TableCell();
                                td.Text = "Burget";
                                td.Style.Add("text-align", "right");
                                td.ColumnSpan = 2;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number0(Convert.ToDecimal(drBudget["Qty"]));
                                tr.Cells.Add(td);

                                td = new TableCell();
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number2(Convert.ToDecimal(drBudget["HargaCOGS"]));
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number0(Convert.ToDecimal(drBudget["HargaJual"]));
                                tr.Cells.Add(td);

                                td = new TableCell();
                                tr.Cells.Add(td);

                                td = new TableCell();
                                tr.Cells.Add(td);

                                td = new TableCell();
                                tr.Cells.Add(td);

                                tb.Rows.Add(tr);

                                tr = new TableRow();
                                tr.Style.Add("font-weight", "bold");
                                tr.Style.Add("background-color", "#FFFD95");

                                td = new TableCell();
                                td.Text = "Selisih";
                                td.Style.Add("text-align", "right");
                                td.ColumnSpan = 2;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number0(selisihQty);
                                if (selisihQty < 0) td.Style.Add("color", "#FF0000");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number2(selisihCOGS);
                                if (selisihCOGS < 0) td.Style.Add("color", "#FF0000");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number0(selisihJual);
                                if (selisihJual < 0) td.Style.Add("color", "#FF0000");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                tr.Cells.Add(td);

                                td = new TableCell();
                                tr.Cells.Add(td);

                                td = new TableCell();
                                tr.Cells.Add(td);

                                tb.Rows.Add(tr);
                            }

                            STotalM = 0m;
                            STotalR = 0m;
                            STotalJual = 0m;
                            STotalButir = 0m;
                            STotalCarat = 0m;
                            STotalQty = 0m;
                            STotalBerat = 0m;

                        }

                        Sub_Header(location, idxlocation);
                        idxlocation++;
                    }
                    flagLoclast = flagLoccurrent;

                    TotalM = Convert.ToDecimal(dtRow["PriceM"].ToString());
                    TotalR = Convert.ToDecimal(dtRow["PriceR"].ToString());
                    TotalJual = Convert.ToDecimal(dtRow["PriceJ"].ToString());
                    TotalButir = Convert.ToDecimal(dtRow["TotalButir"].ToString());
                    TotalCarat = Convert.ToDecimal(dtRow["TotalCarat"].ToString());
                    TotalQty = Convert.ToDecimal(dtRow["Qty"].ToString());
                    TotalBerat = Convert.ToDecimal(dtRow["NettWeight"].ToString());

                    STotalM += TotalM;
                    STotalR += TotalR;
                    STotalJual += TotalJual;
                    STotalButir += TotalButir;
                    STotalCarat += TotalCarat;
                    STotalQty += TotalQty;
                    STotalBerat += TotalBerat;

                    if (productordernumber == "1")
                    {
                        G1TotalM += TotalM;
                        G1TotalR += TotalR;
                        G1TotalJual += TotalJual;
                        G1TotalButir += TotalButir;
                        G1TotalCarat += TotalCarat;
                        G1TotalQty += TotalQty;
                        G1TotalBerat += TotalBerat;
                    }

                    if (productordernumber == "2")
                    {
                        G2TotalM += TotalM;
                        G2TotalR += TotalR;
                        G2TotalJual += TotalJual;
                        G2TotalButir += TotalButir;
                        G2TotalCarat += TotalCarat;
                        G2TotalQty += TotalQty;
                        G2TotalBerat += TotalBerat;
                    }

                    if (productordernumber == "3")
                    {
                        G3TotalM += TotalM;
                        G3TotalR += TotalR;
                        G3TotalJual += TotalJual;
                        G3TotalButir += TotalButir;
                        G3TotalCarat += TotalCarat;
                        G3TotalQty += TotalQty;
                        G3TotalBerat += TotalBerat;
                    }

                    if (productordernumber == "4")
                    {
                        G4TotalM += TotalM;
                        G4TotalR += TotalR;
                        G4TotalJual += TotalJual;
                        G4TotalButir += TotalButir;
                        G4TotalCarat += TotalCarat;
                        G4TotalQty += TotalQty;
                        G4TotalBerat += TotalBerat;
                    }

                    GTotalM += TotalM;
                    GTotalR += TotalR;
                    GTotalJual += TotalJual;
                    GTotalButir += TotalButir;
                    GTotalCarat += TotalCarat;
                    GTotalQty += TotalQty;
                    GTotalBerat += TotalBerat;

                    tr = new TableRow();

                    td = new TableCell();
                    td.Text = index.ToString() + ".";
                    tr.Cells.Add(td);
                    index++;

                    td = new TableCell();
                    td.Text = keterangan;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number0(TotalQty);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number2(TotalM);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number2(TotalR);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number0(TotalJual);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number2(TotalBerat);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number0(TotalButir);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number3(TotalCarat);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    idxrow++;

                    //lit_test.Text += "<br />idxlocation = " + idxlocation.ToString();
                    //lit_test.Text += "<br />idxrow = " + idxrow.ToString();
                    //lit_test.Text += "<br />dt.Rows.Count = " + dt.Rows.Count.ToString();
                    //lit_test.Text += "<br />--------------------------------------";

                    // subtotal (last)
                    if (idxrow == dt.Rows.Count)
                    {
                        string queryBudget = "SELECT A.* FROM PricingTableFB A JOIN LocOutlet B ON A.IDLokasi = B.ID AND A.TipeLokasi = 1 WHERE B.Nama = '" + location + "'";
                        DataTable dtBudget = new DataTable();
                        dtBudget = Db.Rs(queryBudget, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

                        tr = new TableRow();
                        tr.Style.Add("font-weight", "bold");
                        tr.Style.Add("background-color", "#eaf8fd");

                        td = new TableCell();
                        td.Text = "Sub Total";
                        td.Style.Add("text-align", "right");
                        td.ColumnSpan = 2;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Str.Number0(STotalQty);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Str.Number2(STotalM);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Str.Number2(STotalR);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Str.Number0(STotalJual);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Str.Number2(STotalBerat);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Str.Number0(STotalButir);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Str.Number3(STotalCarat);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);

                        if (dtBudget.Rows.Count > 0)
                        {
                            DataRow drBudget = dtBudget.Rows[0];

                            decimal selisihQty = STotalQty - Convert.ToDecimal(drBudget["Qty"]);
                            decimal selisihCOGS = STotalR - Convert.ToDecimal(drBudget["HargaCOGS"]);
                            decimal selisihJual = STotalJual - Convert.ToDecimal(drBudget["HargaJual"]);

                            tr = new TableRow();
                            tr.Style.Add("font-weight", "bold");
                            tr.Style.Add("background-color", "#FFFD95");

                            td = new TableCell();
                            td.Text = "Burget";
                            td.Style.Add("text-align", "right");
                            td.ColumnSpan = 2;
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number0(Convert.ToDecimal(drBudget["Qty"]));
                            tr.Cells.Add(td);

                            td = new TableCell();
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number2(Convert.ToDecimal(drBudget["HargaCOGS"]));
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number0(Convert.ToDecimal(drBudget["HargaJual"]));
                            tr.Cells.Add(td);

                            td = new TableCell();
                            tr.Cells.Add(td);

                            td = new TableCell();
                            tr.Cells.Add(td);

                            td = new TableCell();
                            tr.Cells.Add(td);

                            tb.Rows.Add(tr);

                            tr = new TableRow();
                            tr.Style.Add("font-weight", "bold");
                            tr.Style.Add("background-color", "#FFFD95");

                            td = new TableCell();
                            td.Text = "Selisih";
                            td.Style.Add("text-align", "right");
                            td.ColumnSpan = 2;
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number0(selisihQty);
                            if (selisihQty < 0) td.Style.Add("color", "#FF0000");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number2(selisihCOGS);
                            if (selisihCOGS < 0) td.Style.Add("color", "#FF0000");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number0(selisihJual);
                            if (selisihJual < 0) td.Style.Add("color", "#FF0000");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            tr.Cells.Add(td);

                            td = new TableCell();
                            tr.Cells.Add(td);

                            td = new TableCell();
                            tr.Cells.Add(td);

                            tb.Rows.Add(tr);
                        }

                    }


                }

                Sub_Header("Grand Total", 0);

                //total 1
                tr = new TableRow();
                tr.Style.Add("font-weight", "bold");

                td = new TableCell();
                td.Text = "1.";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Diamond Jewellery";
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(G1TotalQty);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(G1TotalM);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(G1TotalR);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(G1TotalJual);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(G1TotalBerat);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(G1TotalButir);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number3(G1TotalCarat);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                //total 2
                tr = new TableRow();
                tr.Style.Add("font-weight", "bold");

                td = new TableCell();
                td.Text = "2.";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Plain Gold";
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(G2TotalQty);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(G2TotalM);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(G2TotalR);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(G2TotalJual);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(G2TotalBerat);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(G2TotalButir);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number3(G2TotalCarat);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                //total 3
                tr = new TableRow();
                tr.Style.Add("font-weight", "bold");

                td = new TableCell();
                td.Text = "3.";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Gold Jewellery";
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(G3TotalQty);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(G3TotalM);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(G3TotalR);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(G3TotalJual);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(G3TotalBerat);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(G3TotalButir);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number3(G3TotalCarat);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                //total 4
                tr = new TableRow();
                tr.Style.Add("font-weight", "bold");

                td = new TableCell();
                td.Text = "4.";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "Loose Diamond";
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(G4TotalQty);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(G4TotalM);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(G4TotalR);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(G4TotalJual);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(G4TotalBerat);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(G4TotalButir);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number3(G4TotalCarat);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                //grand total
                tr = new TableRow();
                tr.Style.Add("font-weight", "bold");
                tr.Style.Add("background-color", "#ffdada");

                td = new TableCell();
                td.Text = "Grand Total";
                td.Style.Add("text-align", "right");
                td.ColumnSpan = 2;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(GTotalQty);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(GTotalM);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(GTotalR);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(GTotalJual);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(GTotalBerat);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(GTotalButir);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number3(GTotalCarat);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                if (tipe == 0)
                {
                    string queryBudget = "SELECT * FROM PricingTableFB WHERE TipeLokasi = 0 AND IDLokasi = 0";
                    DataTable dtBudget = new DataTable();
                    dtBudget = Db.Rs(queryBudget, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

                    DataRow drBudget = dtBudget.Rows[0];

                    decimal selisihQty = GTotalQty - Convert.ToDecimal(drBudget["Qty"]);
                    decimal selisihCOGS = GTotalR - Convert.ToDecimal(drBudget["HargaCOGS"]);
                    decimal selisihJual = GTotalJual - Convert.ToDecimal(drBudget["HargaJual"]);

                    tr = new TableRow();
                    tr.Style.Add("font-weight", "bold");
                    tr.Style.Add("background-color", "#FFFD95");

                    td = new TableCell();
                    td.Text = "Burget";
                    td.Style.Add("text-align", "right");
                    td.ColumnSpan = 2;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number0(Convert.ToDecimal(drBudget["Qty"]));
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number2(Convert.ToDecimal(drBudget["HargaCOGS"]));
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number0(Convert.ToDecimal(drBudget["HargaJual"]));
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    tr = new TableRow();
                    tr.Style.Add("font-weight", "bold");
                    tr.Style.Add("background-color", "#FFFD95");

                    td = new TableCell();
                    td.Text = "Selisih";
                    td.Style.Add("text-align", "right");
                    td.ColumnSpan = 2;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number0(selisihQty);
                    if (selisihQty < 0) td.Style.Add("color", "#FF0000");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number2(selisihCOGS);
                    if (selisihCOGS < 0) td.Style.Add("color", "#FF0000");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Number0(selisihJual);
                    if (selisihJual < 0) td.Style.Add("color", "#FF0000");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }

            }
        }
        catch (Exception ex) { }



    }

    protected void Sub_Header(int TipeLokasi, int IDLokasi)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        string Lokasi = "";
        if (tipe == 0)
            Lokasi += Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + IDLokasi, Db.Cnn_DB);
        else if (tipe == 1)
            Lokasi += Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + IDLokasi, Db.Cnn_DB);
        else if (tipe == 2)
            Lokasi += Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + IDLokasi, Db.Cnn_DB);

        x += "<br />";
        x += "<span class=\"text-orange\">";
        x += Lokasi;
        x += "</span>";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void Sub_Header(string namalokasi, int idx)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += "<span class=\"text-orange\">";
        x += namalokasi;
        x += "</span>";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        td.Style.Add("text-align", (idx == 0 ? "center" : "left"));
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
}