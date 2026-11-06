using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {

            datepicker.Text = Str.Tgl(DateTime.Today);
            datepicker2.Text = Str.Tgl(DateTime.Today);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        int Qty = 0;
        decimal Carat = 0m;
        decimal Butir = 0m;
        decimal HargaM = 0;
        decimal Harga = 0m;
        decimal Netweight = 0m;
        int QtyJual = 0;
        decimal JualCarat = 0m;
        decimal JualButir = 0m;
        decimal JualHargaM = 0;
        decimal JualHarga = 0m;
        decimal JualNetweight = 0m;
        int QtyActual = 0;
        decimal ActualCarat = 0m;
        decimal ActualButir = 0m;
        decimal ActualHargaM = 0;
        decimal ActualHarga = 0m;
        decimal ActualNetweight = 0m;
        int QtyTradeIn = 0;
        decimal TradeInCarat = 0m;
        decimal TradeInButir = 0m;
        decimal TradeInHargaM = 0;
        decimal TradeInHarga = 0m;
        decimal TradeInNetweight = 0m;
        int QtyResell = 0;
        decimal ResellCarat = 0m;
        decimal ResellButir = 0m;
        decimal ResellHargaM = 0;
        decimal ResellHarga = 0m;
        decimal ResellNetweight = 0m;
        int QtyTransferIn = 0;
        decimal TransferInCarat = 0m;
        decimal TransferInButir = 0m;
        decimal TransferInHargaM = 0;
        decimal TransferInHarga = 0m;
        decimal TransferInNetweight = 0m;
        int QtyTransferOut = 0;
        decimal TransferOutCarat = 0m;
        decimal TransferOutButir = 0m;
        decimal TransferOutHargaM = 0;
        decimal TransferOutHarga = 0m;
        decimal TransferOutNetweight = 0m;
        int QtyReceive = 0;
        decimal ReceiveCarat = 0m;
        decimal ReceiveButir = 0m;
        decimal ReceiveHargaM = 0;
        decimal ReceiveHarga = 0m;
        decimal ReceiveNetweight = 0m;
        int QtyLebur = 0;
        decimal LeburCarat = 0m;
        decimal LeburButir = 0m;
        decimal LeburHargaM = 0;
        decimal LeburHarga = 0m;
        decimal LeburNetweight = 0m;
        int QtyRetur = 0;
        decimal ReturCarat = 0m;
        decimal ReturButir = 0m;
        decimal ReturHargaM = 0;
        decimal ReturHarga = 0m;
        decimal ReturNetweight = 0m;

        /*var GJ = from p in db.StockProductGJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;
        var pg = from p in db.StockProductPG
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;*/

        start = Convert.ToDateTime(datepicker.Text);
        end = Convert.ToDateTime(datepicker2.Text);

        int Bulan = Convert.ToInt32(start.Month);
        int Tahun = Convert.ToInt32(start.Year);

        int jlh = 0;
        int jlhJual = 0;
        int jlhTradeIn = 0;
        int jlhResell = 0;
        int jlhTransferIn = 0;
        int jlhTransferOut = 0;
        int jlhActual = 0;
        int jlhReceive = 0;
        int jlhRetur = 0;
        int jlhLebur = 0;
        decimal jlhCarat = 0m;
        decimal jlhButir = 0m;
        decimal jlhHargaM = 0m;
        decimal jlhHarga = 0m;
        decimal jlhNetweight = 0m;

        decimal jlhJualCarat = 0m;
        decimal jlhJualButir = 0m;
        decimal jlhJualHargaM = 0m;
        decimal jlhJualHarga = 0m;
        decimal jlhJualNetweight = 0m;

        decimal jlhTradeInCarat = 0m;
        decimal jlhTradeInButir = 0m;
        decimal jlhTradeInHargaM = 0m;
        decimal jlhTradeInHarga = 0m;
        decimal jlhTradeInNetweight = 0m;

        decimal jlhResellCarat = 0m;
        decimal jlhResellButir = 0m;
        decimal jlhResellHargaM = 0m;
        decimal jlhResellHarga = 0m;
        decimal jlhResellNetweight = 0m;

        decimal jlhTransferInCarat = 0m;
        decimal jlhTransferInButir = 0m;
        decimal jlhTransferInHargaM = 0m;
        decimal jlhTransferInHarga = 0m;
        decimal jlhTransferInNetweight = 0m;

        decimal jlhTransferOutCarat = 0m;
        decimal jlhTransferOutButir = 0m;
        decimal jlhTransferOutHargaM = 0m;
        decimal jlhTransferOutHarga = 0m;
        decimal jlhTransferOutNetweight = 0m;

        decimal jlhActualCarat = 0m;
        decimal jlhActualButir = 0m;
        decimal jlhActualHargaM = 0m;
        decimal jlhActualHarga = 0m;
        decimal jlhActualNetweight = 0m;

        decimal jlhReceiveCarat = 0m;
        decimal jlhReceiveButir = 0m;
        decimal jlhReceiveHargaM = 0m;
        decimal jlhReceiveHarga = 0m;
        decimal jlhReceiveNetweight = 0m;

        decimal jlhReturCarat = 0m;
        decimal jlhReturButir = 0m;
        decimal jlhReturHargaM = 0m;
        decimal jlhReturHarga = 0m;
        decimal jlhReturNetweight = 0m;

        decimal jlhLeburCarat = 0m;
        decimal jlhLeburButir = 0m;
        decimal jlhLeburHargaM = 0m;
        decimal jlhLeburHarga = 0m;
        decimal jlhLeburNetweight = 0m;

        TableRow trh = new TableRow();
        TableCell tdh;

        tdh = new TableCell();
        tdh.Text = "<b></b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Qty</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>H. Jual</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Butir</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Carat</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>NetWeight</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Qty</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>H. Jual</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Butir</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Carat</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>NetWeight</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Qty</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>H. Jual</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Butir</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Carat</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>NetWeight</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Qty</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>H. Jual</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Butir</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Carat</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>NetWeight</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Qty</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>H. Jual</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Butir</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Carat</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>NetWeight</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Qty</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>H. Jual</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Butir</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Carat</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>NetWeight</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Qty</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>H. Jual</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Butir</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Carat</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>NetWeight</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Qty</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>H. Jual</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Butir</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Carat</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>NetWeight</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Qty</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>H. Jual</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Butir</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>Carat</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center; background: #ffffcc;";
        tdh.Text = "<b>NetWeight</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Qty</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>H. Jual</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Butir</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>Carat</b>";
        trh.Cells.Add(tdh);

        tdh = new TableCell();
        tdh.Attributes["style"] = "text-align: center;";
        tdh.Text = "<b>NetWeight</b>";
        trh.Cells.Add(tdh);

        tb.Rows.Add(trh);

        int GroupBy = Convert.ToInt32(groupby.SelectedValue);
        if (GroupBy == 1)
        {
            var pi = from p in db.CharProductItem
                     select p;
            foreach (var r in pi)
            {
                jlh = 0;
                jlhJual = 0;
                jlhTradeIn = 0;
                jlhResell = 0;
                jlhTransferIn = 0;
                jlhTransferOut = 0;
                jlhActual = 0;
                jlhReceive = 0;
                jlhLebur = 0;
                jlhRetur = 0;

                jlhButir = 0;
                jlhJualButir = 0;
                jlhTradeInButir = 0;
                jlhResellButir = 0;
                jlhTransferInButir = 0;
                jlhTransferOutButir = 0;
                jlhActualButir = 0;
                jlhReceiveButir = 0;
                jlhLeburButir = 0;
                jlhReturButir = 0;

                jlhCarat = 0;
                jlhJualCarat = 0;
                jlhTradeInCarat = 0;
                jlhResellCarat = 0;
                jlhTransferInCarat = 0;
                jlhTransferOutCarat = 0;
                jlhActualCarat = 0;
                jlhReceiveCarat = 0;
                jlhLeburCarat = 0;
                jlhReturCarat = 0;

                jlhHarga = 0;
                jlhJualHarga = 0;
                jlhTradeInHarga = 0;
                jlhResellHarga = 0;
                jlhTransferInHarga = 0;
                jlhTransferOutHarga = 0;
                jlhActualHarga = 0;
                jlhReceiveHarga = 0;
                jlhLeburHarga = 0;
                jlhReturHarga = 0;

                jlhHargaM = 0;
                jlhJualHargaM = 0;
                jlhTradeInHargaM = 0;
                jlhResellHargaM = 0;
                jlhTransferInHargaM = 0;
                jlhTransferOutHargaM = 0;
                jlhActualHargaM = 0;
                jlhReceiveHargaM = 0;
                jlhLeburHargaM = 0;
                jlhReturHargaM = 0;

                jlhNetweight = 0;
                jlhJualNetweight = 0;
                jlhTradeInNetweight = 0;
                jlhResellNetweight = 0;
                jlhTransferInNetweight = 0;
                jlhTransferOutNetweight = 0;
                jlhActualNetweight = 0;
                jlhReceiveNetweight = 0;
                jlhLeburNetweight = 0;
                jlhReturNetweight = 0;

                var item = from p in db.SaldoStockGJ
                           where p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                           && p.Bulan == Bulan && p.Tahun == Tahun
                           select p;

                var itemPenjualan = (from p in db.SalesOrderGJ
                                     where !p.SalesOrder.Draft
                                     && !(from z in db.SalesOrderGJ
                                          select z.SalesOrder.ID).Contains(p.IDForm)
                                     && p.SalesOrder.StatusPembayaran == true
                                     && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                     && p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                                     select p);

                var itemTradeIn = (from p in db.ResellGJ
                                   where (from z in db.TradeIn
                                          where !z.Draft
                                          select z.IDResell).Contains(p.IDForm)
                                           && p.Resell.StatusVoid == null && !p.Resell.Draft && p.SalesOrder.Tgl.HasValue
                                           && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                           && p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                                   select p);

                var itemResell = (from p in db.ResellGJ
                                  where !(from z in db.TradeIn
                                          where !z.Draft
                                          select z.IDResell).Contains(p.IDForm)
                                          && p.Resell.StatusVoid == null && !p.Resell.Draft && p.SalesOrder.Tgl.HasValue
                                          && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                          && p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                                  select p);

                //var itemTransferIn = (from p in db.StockOutgoingGJ_Product
                //                      where p.StockOutgoingGJ.Tgl >= start && p.StockOutgoingGJ.Tgl < end
                //                      && p.StockOutgoingGJ.StatusTransOut == 4
                //                      && p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                //                      select p);

                //var itemTransferOut = (from p in db.StockOutgoingGJ_Product
                //                       where p.StockOutgoingGJ.Tgl >= start && p.StockOutgoingGJ.Tgl < end
                //                       && p.StockOutgoingGJ.StatusTransOut == 4
                //                       && p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                //                       select p);

                var itemTransferIn = (from p in db.StockIncomingGJ_Product
                                      where p.StockIncomingGJ.Tgl >= start && p.StockIncomingGJ.Tgl < end.AddDays(1)
                                      && p.StockIncomingGJ.StockOutgoingGJ.StatusTransOut == 4 && p.StockIncomingGJ.StatusTransIn == 2
                                      && p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                                      && p.SubStorageResult != 99
                                      select p);

                var itemTransferOut = (from p in db.StockIncomingGJ_Product
                                       where p.StockIncomingGJ.Tgl >= start && p.StockIncomingGJ.Tgl < end.AddDays(1)
                                       && p.StockIncomingGJ.StockOutgoingGJ.StatusTransOut == 4 && p.StockIncomingGJ.StatusTransIn == 2
                                       && p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                                       && p.SubStorageResult != 99
                                       select p);

                var itemReceive = (from p in db.StockReceiveGJ
                                       where p.ApprovalTgl >= start && p.ApprovalTgl < end
                                       && p.Approval != null
                                       && p.StockReceiveGJ_CharProduct.ProductItem == r.ID
                                       select p);

                var itemRetur = (from p in db.StockReturnGJ_Product
                                       where p.StockReturnGJ.ApprovalTgl >= start && p.StockReturnGJ.ApprovalTgl < end
                                       && p.StockReturnGJ.Approval != null
                                       && p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                                       select p);

                var itemLebur = (from p in db.StockRetireGJ
                                 where p.ApprovalTgl >= start && p.ApprovalTgl < end
                                 && p.Approval != null
                                 && p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                                 select p);

                int UserID = App.UserID;
                var d = from p in db.DataAdminSales
                        where p.IDUser == UserID
                        select p;
                if (d.Count() > 0)
                {
                    item = item.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemPenjualan = itemPenjualan.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTradeIn = itemTradeIn.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemResell = itemResell.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTransferIn = itemTransferIn.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTransferOut = itemTransferOut.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemReceive = itemReceive.Where(p => p.IDWarehouse == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                int Cabang = 0;
                if (Cabang > 0)
                {
                    item = item.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);
                    itemPenjualan = itemPenjualan.Where(p => p.SalesOrder.IDLokasi == Cabang && p.SalesOrder.TipeLokasi == tipe);
                    itemTradeIn = itemTradeIn.Where(p => p.Resell.IDLokasi == Cabang && p.Resell.TipeLokasi == tipe);
                    itemResell = itemResell.Where(p => p.Resell.IDLokasi == Cabang && p.Resell.TipeLokasi == tipe);
                    itemTransferIn = itemTransferIn.Where(p => p.StockIncomingGJ.StockOutgoingGJ.IDTujuan == Cabang && p.StockIncomingGJ.StockOutgoingGJ.TipeTujuan == tipe);
                    itemTransferOut = itemTransferOut.Where(p => p.StockIncomingGJ.StockOutgoingGJ.IDAsal == Cabang && p.StockIncomingGJ.StockOutgoingGJ.TipeAsal == tipe);
                    itemReceive = itemReceive.Where(p => p.IDWarehouse == Cabang);
                    itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi == Cabang && p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                    itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi == Cabang && p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                }
                else
                {
                    //item = item.Where(p => p.TipeLokasi == tipe);
                    //itemPenjualan = itemPenjualan.Where(p => p.SalesOrder.TipeLokasi == tipe);
                    //itemTradeIn = itemTradeIn.Where(p => p.Resell.TipeLokasi == tipe);
                    //itemResell = itemResell.Where(p => p.Resell.TipeLokasi == tipe);
                    //itemTransferIn = itemTransferIn.Where(p => p.StockOutgoingGJ.TipeTujuan == tipe);
                    //itemTransferOut = itemTransferOut.Where(p => p.StockOutgoingGJ.TipeAsal == tipe);
                    //itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                    //itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                }
                
                try
                {
                    jlh = item.Count();
                    jlhNetweight = Convert.ToDecimal(item.Sum(p => p.NetWeight.Value));
                    jlhHargaM = Convert.ToDecimal(item.Sum(p => p.HargaM.Value));
                    jlhHarga = Convert.ToDecimal(item.Sum(p => p.HargaJual.Value));
                }
                catch{}
                try
                {
                    jlhJual = itemPenjualan.Count();
                    jlhJualButir = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhJualCarat = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhJualNetweight = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhJualHargaM = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhJualHarga = itemPenjualan.Sum(p => p.HargaRupiah.Value);
                }
                catch { }
                try
                {
                    jlhTradeIn = itemTradeIn.Count();
                    jlhTradeInButir = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTradeInCarat = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTradeInNetweight = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTradeInHargaM = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTradeInHarga = itemTradeIn.Sum(p => p.HargaRupiah.Value);
                }
                catch { }
                
                try
                {
                    jlhResell = itemResell.Count();
                    jlhResellButir = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhResellCarat = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhResellNetweight = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhResellHargaM = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhResellHarga = itemResell.Sum(p => p.HargaRupiah.Value);
                }
                catch { }
                
                try
                {
                    jlhTransferIn = itemTransferIn.Count();
                    jlhTransferInButir = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTransferInCarat = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTransferInNetweight = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTransferInHargaM = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTransferInHarga = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }
                
                try
                {
                    jlhTransferOut = itemTransferOut.Count();
                    jlhTransferOutButir = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTransferOutCarat = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTransferOutNetweight = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTransferOutHargaM = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTransferOutHarga = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhReceive = itemReceive.Count();
                    jlhReceiveButir = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.StoneQty.Value);
                    jlhReceiveCarat = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.StoneCarat);
                    jlhReceiveNetweight = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.NettoWeight);
                    jlhReceiveHargaM = itemReceive.Sum(p => p.StockReceiveGJ_Costing.TotalBiaya + p.StockReceiveGJ_CostingProduct.Total);
                    jlhReceiveHarga = itemReceive.Sum(p => p.StockReceiveGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }
                
                try
                {
                    jlhRetur = itemRetur.Count();
                    jlhReturButir = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhReturCarat = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhReturNetweight = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhReturHargaM = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhReturHarga = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }
                

                try
                {
                    jlhLebur = itemLebur.Count();
                    jlhLeburButir = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhLeburCarat = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhLeburNetweight = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhLeburHargaM = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhLeburHarga = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                jlhActual = jlh - jlhJual + jlhTradeIn + jlhResell + jlhTransferIn - jlhTransferOut + jlhReceive - jlhLebur - jlhRetur;
                jlhActualCarat = jlhCarat - jlhJualCarat + jlhTradeInCarat + jlhResellCarat + jlhTransferInCarat - jlhTransferOutCarat + jlhReceiveCarat - jlhLeburCarat - jlhReturCarat;
                jlhActualButir = jlhButir - jlhJualButir + jlhTradeInButir + jlhResellButir + jlhTransferInButir - jlhTransferOutButir + jlhReceiveButir - jlhLeburButir - jlhReturButir;
                jlhActualHarga = jlhHarga - jlhJualHarga + jlhTradeInHarga + jlhResellHarga + jlhTransferInHarga - jlhTransferOutHarga + jlhReceiveHarga - jlhLeburHarga - jlhReturHarga;
                jlhActualHargaM = jlhHargaM - jlhJualHargaM + jlhTradeInHargaM + jlhResellHargaM + jlhTransferInHargaM - jlhTransferOutHargaM + jlhReceiveHargaM - jlhLeburHargaM - jlhReturHargaM;
                jlhActualNetweight = jlhNetweight - jlhJualNetweight + jlhTradeInNetweight + jlhResellNetweight + jlhTransferInNetweight - jlhTransferOutNetweight + jlhReceiveNetweight - jlhLeburNetweight - jlhReturNetweight;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlh.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceive.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeIn.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResell.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferIn.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOut.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhRetur.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLebur.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualNetweight.ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                Carat += jlhCarat;
                Butir += jlhButir;
                HargaM += jlhHargaM;
                Harga += jlhHarga;
                Netweight += jlhNetweight;

                QtyReceive += jlhReceive;
                ReceiveCarat += jlhReceiveCarat;
                ReceiveButir += jlhReceiveButir;
                ReceiveHargaM += jlhReceiveHargaM;
                ReceiveHarga += jlhReceiveHarga;
                ReceiveNetweight += jlhReceiveNetweight;

                QtyJual += jlhJual;
                JualCarat += jlhJualCarat;
                JualButir += jlhJualButir;
                JualHargaM += jlhJualHargaM;
                JualHarga += jlhJualHarga;
                JualNetweight += jlhJualNetweight;

                QtyActual += jlhActual;
                ActualCarat += jlhActualCarat;
                ActualButir += jlhActualButir;
                ActualHargaM += jlhActualHargaM;
                ActualHarga += jlhActualHarga;
                ActualNetweight += jlhActualNetweight;

                QtyTradeIn += jlhTradeIn;
                TradeInCarat += jlhTradeInCarat;
                TradeInButir += jlhTradeInButir;
                TradeInHargaM += jlhTradeInHargaM;
                TradeInHarga += jlhTradeInHarga;
                TradeInNetweight += jlhTradeInNetweight;

                QtyResell += jlhResell;
                ResellCarat += jlhResellCarat;
                ResellButir += jlhResellButir;
                ResellHargaM += jlhResellHargaM;
                ResellHarga += jlhResellHarga;
                ResellNetweight += jlhResellNetweight;

                QtyTransferIn += jlhTransferIn;
                TransferInCarat += jlhTransferInCarat;
                TransferInButir += jlhTransferInButir;
                TransferInHargaM += jlhTransferInHargaM;
                TransferInHarga += jlhTransferInHarga;
                TransferInNetweight += jlhTransferInNetweight;

                QtyTransferOut += jlhTransferOut;
                TransferOutCarat += jlhTransferOutCarat;
                TransferOutButir += jlhTransferOutButir;
                TransferOutHargaM += jlhTransferOutHargaM;
                TransferOutHarga += jlhTransferOutHarga;
                TransferOutNetweight += jlhTransferOutNetweight;

                QtyRetur += jlhRetur;
                ReturCarat += jlhReturCarat;
                ReturButir += jlhReturButir;
                ReturHargaM += jlhReturHargaM;
                ReturHarga += jlhReturHarga;
                ReturNetweight += jlhReturNetweight;

                QtyLebur += jlhLebur;
                LeburCarat += jlhLeburCarat;
                LeburButir += jlhLeburButir;
                LeburHargaM += jlhLeburHargaM;
                LeburHarga += jlhLeburHarga;
                LeburNetweight += jlhLeburNetweight;
            }

            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Qty.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Harga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Butir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Carat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Netweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyReceive.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);


            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyJual.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyTradeIn.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyResell.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyTransferIn.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyTransferOut.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyRetur.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyLebur.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyActual.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);


            tb.Rows.Add(tr1);

        }
        else if (GroupBy == 2)
        {
            var pl = from p in db.CharProductLevel
                     select p;
            foreach (var r in pl)
            {
                jlh = 0;
                jlhJual = 0;
                jlhTradeIn = 0;
                jlhResell = 0;
                jlhTransferIn = 0;
                jlhTransferOut = 0;
                jlhActual = 0;
                jlhReceive = 0;
                jlhLebur = 0;
                jlhRetur = 0;

                jlhButir = 0;
                jlhJualButir = 0;
                jlhTradeInButir = 0;
                jlhResellButir = 0;
                jlhTransferInButir = 0;
                jlhTransferOutButir = 0;
                jlhActualButir = 0;
                jlhReceiveButir = 0;
                jlhLeburButir = 0;
                jlhReturButir = 0;

                jlhCarat = 0;
                jlhJualCarat = 0;
                jlhTradeInCarat = 0;
                jlhResellCarat = 0;
                jlhTransferInCarat = 0;
                jlhTransferOutCarat = 0;
                jlhActualCarat = 0;
                jlhReceiveCarat = 0;
                jlhLeburCarat = 0;
                jlhReturCarat = 0;

                jlhHarga = 0;
                jlhJualHarga = 0;
                jlhTradeInHarga = 0;
                jlhResellHarga = 0;
                jlhTransferInHarga = 0;
                jlhTransferOutHarga = 0;
                jlhActualHarga = 0;
                jlhReceiveHarga = 0;
                jlhLeburHarga = 0;
                jlhReturHarga = 0;

                jlhHargaM = 0;
                jlhJualHargaM = 0;
                jlhTradeInHargaM = 0;
                jlhResellHargaM = 0;
                jlhTransferInHargaM = 0;
                jlhTransferOutHargaM = 0;
                jlhActualHargaM = 0;
                jlhReceiveHargaM = 0;
                jlhLeburHargaM = 0;
                jlhReturHargaM = 0;

                jlhNetweight = 0;
                jlhJualNetweight = 0;
                jlhTradeInNetweight = 0;
                jlhResellNetweight = 0;
                jlhTransferInNetweight = 0;
                jlhTransferOutNetweight = 0;
                jlhActualNetweight = 0;
                jlhReceiveNetweight = 0;
                jlhLeburNetweight = 0;
                jlhReturNetweight = 0;

                var item = from p in db.SaldoStockGJ
                           where p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                           && p.Bulan == Bulan && p.Tahun == Tahun
                           select p;

                var itemPenjualan = (from p in db.SalesOrderGJ
                                     where !p.SalesOrder.Draft
                                     && !(from z in db.SalesOrderGJ
                                          
                                          select z.SalesOrder.ID).Contains(p.IDForm)
                                     && p.SalesOrder.StatusPembayaran == true
                                     && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                     && p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                                     select p);

                var itemTradeIn = (from p in db.ResellGJ
                                   where (from z in db.TradeIn
                                          where !z.Draft
                                          select z.IDResell).Contains(p.IDForm)
                                           && p.Resell.StatusVoid == null && !p.Resell.Draft && p.SalesOrder.Tgl.HasValue
                                           && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                           && p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                                   select p);

                var itemResell = (from p in db.ResellGJ
                                  where !(from z in db.TradeIn
                                          where !z.Draft
                                          select z.IDResell).Contains(p.IDForm)
                                          && p.Resell.StatusVoid == null && !p.Resell.Draft && p.SalesOrder.Tgl.HasValue
                                          && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                          && p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                                  select p);

                //var itemTransferIn = (from p in db.StockOutgoingGJ_Product
                //                      where p.StockOutgoingGJ.Tgl >= start && p.StockOutgoingGJ.Tgl < end
                //                      && p.StockOutgoingGJ.StatusTransOut == 4
                //                      && p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                //                      select p);

                //var itemTransferOut = (from p in db.StockOutgoingGJ_Product
                //                       where p.StockOutgoingGJ.Tgl >= start && p.StockOutgoingGJ.Tgl < end
                //                       && p.StockOutgoingGJ.StatusTransOut == 4
                //                       && p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                //                       select p);

                var itemTransferIn = (from p in db.StockIncomingGJ_Product
                                      where p.StockIncomingGJ.Tgl >= start && p.StockIncomingGJ.Tgl < end.AddDays(1)
                                      && p.StockIncomingGJ.StockOutgoingGJ.StatusTransOut == 4 && p.StockIncomingGJ.StatusTransIn == 2
                                      && p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                                      && p.SubStorageResult != 99
                                      select p);

                var itemTransferOut = (from p in db.StockIncomingGJ_Product
                                       where p.StockIncomingGJ.Tgl >= start && p.StockIncomingGJ.Tgl < end.AddDays(1)
                                       && p.StockIncomingGJ.StockOutgoingGJ.StatusTransOut == 4 && p.StockIncomingGJ.StatusTransIn == 2
                                       && p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                                       && p.SubStorageResult != 99
                                       select p);

                var itemReceive = (from p in db.StockReceiveGJ
                                   where p.ApprovalTgl >= start && p.ApprovalTgl < end
                                   && p.Approval != null
                                   && p.StockReceiveGJ_CharProduct.ProductLevel == r.ID
                                   select p);

                var itemRetur = (from p in db.StockReturnGJ_Product
                                 where p.StockReturnGJ.ApprovalTgl >= start && p.StockReturnGJ.ApprovalTgl < end
                                 && p.StockReturnGJ.Approval != null
                                 && p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                                 select p);

                var itemLebur = (from p in db.StockRetireGJ
                                 where p.ApprovalTgl >= start && p.ApprovalTgl < end
                                 && p.Approval != null
                                 && p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                                 select p);

                int UserID = App.UserID;
                var d = from p in db.DataAdminSales
                        where p.IDUser == UserID
                        select p;
                if (d.Count() > 0)
                {
                    item = item.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemPenjualan = itemPenjualan.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTradeIn = itemTradeIn.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemResell = itemResell.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTransferIn = itemTransferIn.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTransferOut = itemTransferOut.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemReceive = itemReceive.Where(p => p.IDWarehouse == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                int Cabang = 0;
                if (Cabang > 0)
                {
                    item = item.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);
                    itemPenjualan = itemPenjualan.Where(p => p.SalesOrder.IDLokasi == Cabang && p.SalesOrder.TipeLokasi == tipe);
                    itemTradeIn = itemTradeIn.Where(p => p.Resell.IDLokasi == Cabang && p.Resell.TipeLokasi == tipe);
                    itemResell = itemResell.Where(p => p.Resell.IDLokasi == Cabang && p.Resell.TipeLokasi == tipe);
                    itemTransferIn = itemTransferIn.Where(p => p.StockIncomingGJ.StockOutgoingGJ.IDTujuan == Cabang && p.StockIncomingGJ.StockOutgoingGJ.TipeTujuan == tipe);
                    itemTransferOut = itemTransferOut.Where(p => p.StockIncomingGJ.StockOutgoingGJ.IDAsal == Cabang && p.StockIncomingGJ.StockOutgoingGJ.TipeAsal == tipe);
                    itemReceive = itemReceive.Where(p => p.IDWarehouse == Cabang);
                    itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi == Cabang && p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                    itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi == Cabang && p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                }
                else
                {
                    //item = item.Where(p => p.TipeLokasi == tipe);
                    //itemPenjualan = itemPenjualan.Where(p => p.SalesOrder.TipeLokasi == tipe);
                    //itemTradeIn = itemTradeIn.Where(p => p.Resell.TipeLokasi == tipe);
                    //itemResell = itemResell.Where(p => p.Resell.TipeLokasi == tipe);
                    //itemTransferIn = itemTransferIn.Where(p => p.StockOutgoingGJ.TipeTujuan == tipe);
                    //itemTransferOut = itemTransferOut.Where(p => p.StockOutgoingGJ.TipeAsal == tipe);
                    //itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                    //itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                }

                try
                {
                    jlh = item.Count();
                    jlhNetweight = Convert.ToDecimal(item.Sum(p => p.NetWeight.Value));
                    jlhHargaM = Convert.ToDecimal(item.Sum(p => p.HargaM.Value));
                    jlhHarga = Convert.ToDecimal(item.Sum(p => p.HargaJual.Value));
                }
                catch { }
                try
                {
                    jlhJual = itemPenjualan.Count();
                    jlhJualButir = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhJualCarat = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhJualNetweight = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhJualHargaM = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhJualHarga = itemPenjualan.Sum(p => p.HargaRupiah.Value);
                }
                catch { }
                try
                {
                    jlhTradeIn = itemTradeIn.Count();
                    jlhTradeInButir = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTradeInCarat = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTradeInNetweight = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTradeInHargaM = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTradeInHarga = itemTradeIn.Sum(p => p.HargaRupiah.Value);
                }
                catch { }

                try
                {
                    jlhResell = itemResell.Count();
                    jlhResellButir = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhResellCarat = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhResellNetweight = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhResellHargaM = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhResellHarga = itemResell.Sum(p => p.HargaRupiah.Value);
                }
                catch { }

                try
                {
                    jlhTransferIn = itemTransferIn.Count();
                    jlhTransferInButir = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTransferInCarat = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTransferInNetweight = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTransferInHargaM = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTransferInHarga = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhTransferOut = itemTransferOut.Count();
                    jlhTransferOutButir = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTransferOutCarat = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTransferOutNetweight = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTransferOutHargaM = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTransferOutHarga = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhReceive = itemReceive.Count();
                    jlhReceiveButir = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.StoneQty.Value);
                    jlhReceiveCarat = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.StoneCarat);
                    jlhReceiveNetweight = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.NettoWeight);
                    jlhReceiveHargaM = itemReceive.Sum(p => p.StockReceiveGJ_Costing.TotalBiaya + p.StockReceiveGJ_CostingProduct.Total);
                    jlhReceiveHarga = itemReceive.Sum(p => p.StockReceiveGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhRetur = itemRetur.Count();
                    jlhReturButir = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhReturCarat = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhReturNetweight = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhReturHargaM = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhReturHarga = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }


                try
                {
                    jlhLebur = itemLebur.Count();
                    jlhLeburButir = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhLeburCarat = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhLeburNetweight = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhLeburHargaM = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhLeburHarga = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                jlhActual = jlh - jlhJual + jlhTradeIn + jlhResell + jlhTransferIn - jlhTransferOut + jlhReceive - jlhLebur - jlhRetur;
                jlhActualCarat = jlhCarat - jlhJualCarat + jlhTradeInCarat + jlhResellCarat + jlhTransferInCarat - jlhTransferOutCarat + jlhReceiveCarat - jlhLeburCarat - jlhReturCarat;
                jlhActualButir = jlhButir - jlhJualButir + jlhTradeInButir + jlhResellButir + jlhTransferInButir - jlhTransferOutButir + jlhReceiveButir - jlhLeburButir - jlhReturButir;
                jlhActualHarga = jlhHarga - jlhJualHarga + jlhTradeInHarga + jlhResellHarga + jlhTransferInHarga - jlhTransferOutHarga + jlhReceiveHarga - jlhLeburHarga - jlhReturHarga;
                jlhActualHargaM = jlhHargaM - jlhJualHargaM + jlhTradeInHargaM + jlhResellHargaM + jlhTransferInHargaM - jlhTransferOutHargaM + jlhReceiveHargaM - jlhLeburHargaM - jlhReturHargaM;
                jlhActualNetweight = jlhNetweight - jlhJualNetweight + jlhTradeInNetweight + jlhResellNetweight + jlhTransferInNetweight - jlhTransferOutNetweight + jlhReceiveNetweight - jlhLeburNetweight - jlhReturNetweight;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlh.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceive.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeIn.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResell.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferIn.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOut.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhRetur.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLebur.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualNetweight.ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                Carat += jlhCarat;
                Butir += jlhButir;
                HargaM += jlhHargaM;
                Harga += jlhHarga;
                Netweight += jlhNetweight;

                QtyReceive += jlhReceive;
                ReceiveCarat += jlhReceiveCarat;
                ReceiveButir += jlhReceiveButir;
                ReceiveHargaM += jlhReceiveHargaM;
                ReceiveHarga += jlhReceiveHarga;
                ReceiveNetweight += jlhReceiveNetweight;

                QtyJual += jlhJual;
                JualCarat += jlhJualCarat;
                JualButir += jlhJualButir;
                JualHargaM += jlhJualHargaM;
                JualHarga += jlhJualHarga;
                JualNetweight += jlhJualNetweight;

                QtyActual += jlhActual;
                ActualCarat += jlhActualCarat;
                ActualButir += jlhActualButir;
                ActualHargaM += jlhActualHargaM;
                ActualHarga += jlhActualHarga;
                ActualNetweight += jlhActualNetweight;

                QtyTradeIn += jlhTradeIn;
                TradeInCarat += jlhTradeInCarat;
                TradeInButir += jlhTradeInButir;
                TradeInHargaM += jlhTradeInHargaM;
                TradeInHarga += jlhTradeInHarga;
                TradeInNetweight += jlhTradeInNetweight;

                QtyResell += jlhResell;
                ResellCarat += jlhResellCarat;
                ResellButir += jlhResellButir;
                ResellHargaM += jlhResellHargaM;
                ResellHarga += jlhResellHarga;
                ResellNetweight += jlhResellNetweight;

                QtyTransferIn += jlhTransferIn;
                TransferInCarat += jlhTransferInCarat;
                TransferInButir += jlhTransferInButir;
                TransferInHargaM += jlhTransferInHargaM;
                TransferInHarga += jlhTransferInHarga;
                TransferInNetweight += jlhTransferInNetweight;

                QtyTransferOut += jlhTransferOut;
                TransferOutCarat += jlhTransferOutCarat;
                TransferOutButir += jlhTransferOutButir;
                TransferOutHargaM += jlhTransferOutHargaM;
                TransferOutHarga += jlhTransferOutHarga;
                TransferOutNetweight += jlhTransferOutNetweight;

                QtyRetur += jlhRetur;
                ReturCarat += jlhReturCarat;
                ReturButir += jlhReturButir;
                ReturHargaM += jlhReturHargaM;
                ReturHarga += jlhReturHarga;
                ReturNetweight += jlhReturNetweight;

                QtyLebur += jlhLebur;
                LeburCarat += jlhLeburCarat;
                LeburButir += jlhLeburButir;
                LeburHargaM += jlhLeburHargaM;
                LeburHarga += jlhLeburHarga;
                LeburNetweight += jlhLeburNetweight;
            }

            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Qty.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Harga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Butir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Carat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Netweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyReceive.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyJual.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyTradeIn.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyResell.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyTransferIn.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyTransferOut.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyRetur.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyLebur.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyActual.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);


            tb.Rows.Add(tr1);
        }
        else if (GroupBy == 3)
        {
            var pc = from p in db.CharProductCategory
                     select p;
            foreach (var r in pc)
            {
                jlh = 0;
                jlhJual = 0;
                jlhTradeIn = 0;
                jlhResell = 0;
                jlhTransferIn = 0;
                jlhTransferOut = 0;
                jlhActual = 0;
                jlhReceive = 0;
                jlhLebur = 0;
                jlhRetur = 0;

                jlhButir = 0;
                jlhJualButir = 0;
                jlhTradeInButir = 0;
                jlhResellButir = 0;
                jlhTransferInButir = 0;
                jlhTransferOutButir = 0;
                jlhActualButir = 0;
                jlhReceiveButir = 0;
                jlhLeburButir = 0;
                jlhReturButir = 0;

                jlhCarat = 0;
                jlhJualCarat = 0;
                jlhTradeInCarat = 0;
                jlhResellCarat = 0;
                jlhTransferInCarat = 0;
                jlhTransferOutCarat = 0;
                jlhActualCarat = 0;
                jlhReceiveCarat = 0;
                jlhLeburCarat = 0;
                jlhReturCarat = 0;

                jlhHarga = 0;
                jlhJualHarga = 0;
                jlhTradeInHarga = 0;
                jlhResellHarga = 0;
                jlhTransferInHarga = 0;
                jlhTransferOutHarga = 0;
                jlhActualHarga = 0;
                jlhReceiveHarga = 0;
                jlhLeburHarga = 0;
                jlhReturHarga = 0;

                jlhHargaM = 0;
                jlhJualHargaM = 0;
                jlhTradeInHargaM = 0;
                jlhResellHargaM = 0;
                jlhTransferInHargaM = 0;
                jlhTransferOutHargaM = 0;
                jlhActualHargaM = 0;
                jlhReceiveHargaM = 0;
                jlhLeburHargaM = 0;
                jlhReturHargaM = 0;

                jlhNetweight = 0;
                jlhJualNetweight = 0;
                jlhTradeInNetweight = 0;
                jlhResellNetweight = 0;
                jlhTransferInNetweight = 0;
                jlhTransferOutNetweight = 0;
                jlhActualNetweight = 0;
                jlhReceiveNetweight = 0;
                jlhLeburNetweight = 0;
                jlhReturNetweight = 0;

                var item = from p in db.SaldoStockGJ
                           where p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                           && p.Bulan == Bulan && p.Tahun == Tahun
                           select p;

                var itemPenjualan = (from p in db.SalesOrderGJ
                                     where !p.SalesOrder.Draft
                                     && !(from z in db.SalesOrderGJ
                                          
                                          select z.SalesOrder.ID).Contains(p.IDForm)
                                     && p.SalesOrder.StatusPembayaran == true
                                     && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                     && p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                                     select p);

                var itemTradeIn = (from p in db.ResellGJ
                                   where (from z in db.TradeIn
                                          where !z.Draft
                                          select z.IDResell).Contains(p.IDForm)
                                           && p.Resell.StatusVoid == null && !p.Resell.Draft && p.SalesOrder.Tgl.HasValue
                                           && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                           && p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                                   select p);

                var itemResell = (from p in db.ResellGJ
                                  where !(from z in db.TradeIn
                                          where !z.Draft
                                          select z.IDResell).Contains(p.IDForm)
                                          && p.Resell.StatusVoid == null && !p.Resell.Draft && p.SalesOrder.Tgl.HasValue
                                          && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                          && p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                                  select p);

                //var itemTransferIn = (from p in db.StockOutgoingGJ_Product
                //                      where p.StockOutgoingGJ.Tgl >= start && p.StockOutgoingGJ.Tgl < end
                //                      && p.StockOutgoingGJ.StatusTransOut == 4
                //                      && p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                //                      select p);

                //var itemTransferOut = (from p in db.StockOutgoingGJ_Product
                //                       where p.StockOutgoingGJ.Tgl >= start && p.StockOutgoingGJ.Tgl < end
                //                       && p.StockOutgoingGJ.StatusTransOut == 4
                //                       && p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                //                       select p);

                var itemTransferIn = (from p in db.StockIncomingGJ_Product
                                      where p.StockIncomingGJ.Tgl >= start && p.StockIncomingGJ.Tgl < end.AddDays(1)
                                      && p.StockIncomingGJ.StockOutgoingGJ.StatusTransOut == 4 && p.StockIncomingGJ.StatusTransIn == 2
                                      && p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                                      && p.SubStorageResult != 99
                                      select p);

                var itemTransferOut = (from p in db.StockIncomingGJ_Product
                                       where p.StockIncomingGJ.Tgl >= start && p.StockIncomingGJ.Tgl < end.AddDays(1)
                                       && p.StockIncomingGJ.StockOutgoingGJ.StatusTransOut == 4 && p.StockIncomingGJ.StatusTransIn == 2
                                       && p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                                       && p.SubStorageResult != 99
                                       select p);

                var itemReceive = (from p in db.StockReceiveGJ
                                   where p.ApprovalTgl >= start && p.ApprovalTgl < end
                                   && p.Approval != null
                                   && p.StockReceiveGJ_CharProduct.ProductCategory == r.ID
                                   select p);

                var itemRetur = (from p in db.StockReturnGJ_Product
                                 where p.StockReturnGJ.ApprovalTgl >= start && p.StockReturnGJ.ApprovalTgl < end
                                 && p.StockReturnGJ.Approval != null
                                 && p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                                 select p);

                var itemLebur = (from p in db.StockRetireGJ
                                 where p.ApprovalTgl >= start && p.ApprovalTgl < end
                                 && p.Approval != null
                                 && p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                                 select p);

                int UserID = App.UserID;
                var d = from p in db.DataAdminSales
                        where p.IDUser == UserID
                        select p;
                if (d.Count() > 0)
                {
                    item = item.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemPenjualan = itemPenjualan.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTradeIn = itemTradeIn.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemResell = itemResell.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTransferIn = itemTransferIn.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTransferOut = itemTransferOut.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemReceive = itemReceive.Where(p => p.IDWarehouse == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                int Cabang = 0;
                if (Cabang > 0)
                {
                    item = item.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);
                    itemPenjualan = itemPenjualan.Where(p => p.SalesOrder.IDLokasi == Cabang && p.SalesOrder.TipeLokasi == tipe);
                    itemTradeIn = itemTradeIn.Where(p => p.Resell.IDLokasi == Cabang && p.Resell.TipeLokasi == tipe);
                    itemResell = itemResell.Where(p => p.Resell.IDLokasi == Cabang && p.Resell.TipeLokasi == tipe);
                    itemTransferIn = itemTransferIn.Where(p => p.StockIncomingGJ.StockOutgoingGJ.IDTujuan == Cabang && p.StockIncomingGJ.StockOutgoingGJ.TipeTujuan == tipe);
                    itemTransferOut = itemTransferOut.Where(p => p.StockIncomingGJ.StockOutgoingGJ.IDAsal == Cabang && p.StockIncomingGJ.StockOutgoingGJ.TipeAsal == tipe);
                    itemReceive = itemReceive.Where(p => p.IDWarehouse == Cabang);
                    itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi == Cabang && p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                    itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi == Cabang && p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                }
                else
                {
                    //item = item.Where(p => p.TipeLokasi == tipe);
                    //itemPenjualan = itemPenjualan.Where(p => p.SalesOrder.TipeLokasi == tipe);
                    //itemTradeIn = itemTradeIn.Where(p => p.Resell.TipeLokasi == tipe);
                    //itemResell = itemResell.Where(p => p.Resell.TipeLokasi == tipe);
                    //itemTransferIn = itemTransferIn.Where(p => p.StockOutgoingGJ.TipeTujuan == tipe);
                    //itemTransferOut = itemTransferOut.Where(p => p.StockOutgoingGJ.TipeAsal == tipe);
                    //itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                    //itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                }

                try
                {
                    jlh = item.Count();
                    jlhNetweight = Convert.ToDecimal(item.Sum(p => p.NetWeight.Value));
                    jlhHargaM = Convert.ToDecimal(item.Sum(p => p.HargaM.Value));
                    jlhHarga = Convert.ToDecimal(item.Sum(p => p.HargaJual.Value));
                }
                catch { }
                try
                {
                    jlhJual = itemPenjualan.Count();
                    jlhJualButir = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhJualCarat = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhJualNetweight = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhJualHargaM = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhJualHarga = itemPenjualan.Sum(p => p.HargaRupiah.Value);
                }
                catch { }
                try
                {
                    jlhTradeIn = itemTradeIn.Count();
                    jlhTradeInButir = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTradeInCarat = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTradeInNetweight = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTradeInHargaM = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTradeInHarga = itemTradeIn.Sum(p => p.HargaRupiah.Value);
                }
                catch { }

                try
                {
                    jlhResell = itemResell.Count();
                    jlhResellButir = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhResellCarat = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhResellNetweight = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhResellHargaM = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhResellHarga = itemResell.Sum(p => p.HargaRupiah.Value);
                }
                catch { }

                try
                {
                    jlhTransferIn = itemTransferIn.Count();
                    jlhTransferInButir = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTransferInCarat = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTransferInNetweight = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTransferInHargaM = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTransferInHarga = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhTransferOut = itemTransferOut.Count();
                    jlhTransferOutButir = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTransferOutCarat = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTransferOutNetweight = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTransferOutHargaM = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTransferOutHarga = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhReceive = itemReceive.Count();
                    jlhReceiveButir = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.StoneQty.Value);
                    jlhReceiveCarat = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.StoneCarat);
                    jlhReceiveNetweight = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.NettoWeight);
                    jlhReceiveHargaM = itemReceive.Sum(p => p.StockReceiveGJ_Costing.TotalBiaya + p.StockReceiveGJ_CostingProduct.Total);
                    jlhReceiveHarga = itemReceive.Sum(p => p.StockReceiveGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhRetur = itemRetur.Count();
                    jlhReturButir = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhReturCarat = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhReturNetweight = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhReturHargaM = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhReturHarga = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }


                try
                {
                    jlhLebur = itemLebur.Count();
                    jlhLeburButir = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhLeburCarat = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhLeburNetweight = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhLeburHargaM = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhLeburHarga = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                jlhActual = jlh - jlhJual + jlhTradeIn + jlhResell + jlhTransferIn - jlhTransferOut + jlhReceive - jlhLebur - jlhRetur;
                jlhActualCarat = jlhCarat - jlhJualCarat + jlhTradeInCarat + jlhResellCarat + jlhTransferInCarat - jlhTransferOutCarat + jlhReceiveCarat - jlhLeburCarat - jlhReturCarat;
                jlhActualButir = jlhButir - jlhJualButir + jlhTradeInButir + jlhResellButir + jlhTransferInButir - jlhTransferOutButir + jlhReceiveButir - jlhLeburButir - jlhReturButir;
                jlhActualHarga = jlhHarga - jlhJualHarga + jlhTradeInHarga + jlhResellHarga + jlhTransferInHarga - jlhTransferOutHarga + jlhReceiveHarga - jlhLeburHarga - jlhReturHarga;
                jlhActualHargaM = jlhHargaM - jlhJualHargaM + jlhTradeInHargaM + jlhResellHargaM + jlhTransferInHargaM - jlhTransferOutHargaM + jlhReceiveHargaM - jlhLeburHargaM - jlhReturHargaM;
                jlhActualNetweight = jlhNetweight - jlhJualNetweight + jlhTradeInNetweight + jlhResellNetweight + jlhTransferInNetweight - jlhTransferOutNetweight + jlhReceiveNetweight - jlhLeburNetweight - jlhReturNetweight;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlh.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceive.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeIn.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResell.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferIn.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOut.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhRetur.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLebur.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualNetweight.ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                Carat += jlhCarat;
                Butir += jlhButir;
                HargaM += jlhHargaM;
                Harga += jlhHarga;
                Netweight += jlhNetweight;

                QtyReceive += jlhReceive;
                ReceiveCarat += jlhReceiveCarat;
                ReceiveButir += jlhReceiveButir;
                ReceiveHargaM += jlhReceiveHargaM;
                ReceiveHarga += jlhReceiveHarga;
                ReceiveNetweight += jlhReceiveNetweight;

                QtyJual += jlhJual;
                JualCarat += jlhJualCarat;
                JualButir += jlhJualButir;
                JualHargaM += jlhJualHargaM;
                JualHarga += jlhJualHarga;
                JualNetweight += jlhJualNetweight;

                QtyActual += jlhActual;
                ActualCarat += jlhActualCarat;
                ActualButir += jlhActualButir;
                ActualHargaM += jlhActualHargaM;
                ActualHarga += jlhActualHarga;
                ActualNetweight += jlhActualNetweight;

                QtyTradeIn += jlhTradeIn;
                TradeInCarat += jlhTradeInCarat;
                TradeInButir += jlhTradeInButir;
                TradeInHargaM += jlhTradeInHargaM;
                TradeInHarga += jlhTradeInHarga;
                TradeInNetweight += jlhTradeInNetweight;

                QtyResell += jlhResell;
                ResellCarat += jlhResellCarat;
                ResellButir += jlhResellButir;
                ResellHargaM += jlhResellHargaM;
                ResellHarga += jlhResellHarga;
                ResellNetweight += jlhResellNetweight;

                QtyTransferIn += jlhTransferIn;
                TransferInCarat += jlhTransferInCarat;
                TransferInButir += jlhTransferInButir;
                TransferInHargaM += jlhTransferInHargaM;
                TransferInHarga += jlhTransferInHarga;
                TransferInNetweight += jlhTransferInNetweight;

                QtyTransferOut += jlhTransferOut;
                TransferOutCarat += jlhTransferOutCarat;
                TransferOutButir += jlhTransferOutButir;
                TransferOutHargaM += jlhTransferOutHargaM;
                TransferOutHarga += jlhTransferOutHarga;
                TransferOutNetweight += jlhTransferOutNetweight;

                QtyRetur += jlhRetur;
                ReturCarat += jlhReturCarat;
                ReturButir += jlhReturButir;
                ReturHargaM += jlhReturHargaM;
                ReturHarga += jlhReturHarga;
                ReturNetweight += jlhReturNetweight;

                QtyLebur += jlhLebur;
                LeburCarat += jlhLeburCarat;
                LeburButir += jlhLeburButir;
                LeburHargaM += jlhLeburHargaM;
                LeburHarga += jlhLeburHarga;
                LeburNetweight += jlhLeburNetweight;
            }

            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Qty.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Harga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Butir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Carat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Netweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyReceive.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyJual.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyTradeIn.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyResell.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyTransferIn.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyTransferOut.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyRetur.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyLebur.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyActual.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);


            tb.Rows.Add(tr1);


            tb.Rows.Add(tr1);
        }
        else if (GroupBy == 4)
        {
            var pc = from p in db.CharProductSegmen
                     select p;
            foreach (var r in pc)
            {
                jlh = 0;
                jlhJual = 0;
                jlhTradeIn = 0;
                jlhResell = 0;
                jlhTransferIn = 0;
                jlhTransferOut = 0;
                jlhActual = 0;
                jlhReceive = 0;
                jlhLebur = 0;
                jlhRetur = 0;

                jlhButir = 0;
                jlhJualButir = 0;
                jlhTradeInButir = 0;
                jlhResellButir = 0;
                jlhTransferInButir = 0;
                jlhTransferOutButir = 0;
                jlhActualButir = 0;
                jlhReceiveButir = 0;
                jlhLeburButir = 0;
                jlhReturButir = 0;

                jlhCarat = 0;
                jlhJualCarat = 0;
                jlhTradeInCarat = 0;
                jlhResellCarat = 0;
                jlhTransferInCarat = 0;
                jlhTransferOutCarat = 0;
                jlhActualCarat = 0;
                jlhReceiveCarat = 0;
                jlhLeburCarat = 0;
                jlhReturCarat = 0;

                jlhHarga = 0;
                jlhJualHarga = 0;
                jlhTradeInHarga = 0;
                jlhResellHarga = 0;
                jlhTransferInHarga = 0;
                jlhTransferOutHarga = 0;
                jlhActualHarga = 0;
                jlhReceiveHarga = 0;
                jlhLeburHarga = 0;
                jlhReturHarga = 0;

                jlhHargaM = 0;
                jlhJualHargaM = 0;
                jlhTradeInHargaM = 0;
                jlhResellHargaM = 0;
                jlhTransferInHargaM = 0;
                jlhTransferOutHargaM = 0;
                jlhActualHargaM = 0;
                jlhReceiveHargaM = 0;
                jlhLeburHargaM = 0;
                jlhReturHargaM = 0;

                jlhNetweight = 0;
                jlhJualNetweight = 0;
                jlhTradeInNetweight = 0;
                jlhResellNetweight = 0;
                jlhTransferInNetweight = 0;
                jlhTransferOutNetweight = 0;
                jlhActualNetweight = 0;
                jlhReceiveNetweight = 0;
                jlhLeburNetweight = 0;
                jlhReturNetweight = 0;

                var item = from p in db.SaldoStockGJ
                           where p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                           && p.Bulan == Bulan && p.Tahun == Tahun
                           select p;

                var itemPenjualan = (from p in db.SalesOrderGJ
                                     where !p.SalesOrder.Draft
                                     && !(from z in db.SalesOrderGJ
                                          select z.SalesOrder.ID).Contains(p.IDForm)
                                     && p.SalesOrder.StatusPembayaran == true
                                     && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                     && p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                                     select p);

                var itemTradeIn = (from p in db.ResellGJ
                                   where (from z in db.TradeIn
                                          where !z.Draft
                                          select z.IDResell).Contains(p.IDForm)
                                           && p.Resell.StatusVoid == null && !p.Resell.Draft && p.SalesOrder.Tgl.HasValue
                                           && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                           && p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                                   select p);

                var itemResell = (from p in db.ResellGJ
                                  where !(from z in db.TradeIn
                                          where !z.Draft
                                          select z.IDResell).Contains(p.IDForm)
                                          && p.Resell.StatusVoid == null && !p.Resell.Draft && p.SalesOrder.Tgl.HasValue
                                          && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                          && p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                                  select p);

                //var itemTransferIn = (from p in db.StockOutgoingGJ_Product
                //                      where p.StockOutgoingGJ.Tgl >= start && p.StockOutgoingGJ.Tgl < end
                //                      && p.StockOutgoingGJ.StatusTransOut == 4
                //                      && p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                //                      select p);

                //var itemTransferOut = (from p in db.StockOutgoingGJ_Product
                //                       where p.StockOutgoingGJ.Tgl >= start && p.StockOutgoingGJ.Tgl < end
                //                       && p.StockOutgoingGJ.StatusTransOut == 4
                //                       && p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                //                       select p);

                var itemTransferIn = (from p in db.StockIncomingGJ_Product
                                      where p.StockIncomingGJ.Tgl >= start && p.StockIncomingGJ.Tgl < end.AddDays(1)
                                      && p.StockIncomingGJ.StockOutgoingGJ.StatusTransOut == 4 && p.StockIncomingGJ.StatusTransIn == 2
                                      && p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                                      && p.SubStorageResult != 99
                                      select p);

                var itemTransferOut = (from p in db.StockIncomingGJ_Product
                                       where p.StockIncomingGJ.Tgl >= start && p.StockIncomingGJ.Tgl < end.AddDays(1)
                                       && p.StockIncomingGJ.StockOutgoingGJ.StatusTransOut == 4 && p.StockIncomingGJ.StatusTransIn == 2
                                       && p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                                       && p.SubStorageResult != 99
                                       select p);

                var itemReceive = (from p in db.StockReceiveGJ
                                   where p.ApprovalTgl >= start && p.ApprovalTgl < end
                                   && p.Approval != null
                                   && p.StockReceiveGJ_PricingSegmen.ProductSegmen == r.ID
                                   select p);

                var itemRetur = (from p in db.StockReturnGJ_Product
                                 where p.StockReturnGJ.ApprovalTgl >= start && p.StockReturnGJ.ApprovalTgl < end
                                 && p.StockReturnGJ.Approval != null
                                 && p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                                 select p);

                var itemLebur = (from p in db.StockRetireGJ
                                 where p.ApprovalTgl >= start && p.ApprovalTgl < end
                                 && p.Approval != null
                                 && p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                                 select p);

                int UserID = App.UserID;
                var d = from p in db.DataAdminSales
                        where p.IDUser == UserID
                        select p;
                if (d.Count() > 0)
                {
                    item = item.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemPenjualan = itemPenjualan.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTradeIn = itemTradeIn.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemResell = itemResell.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTransferIn = itemTransferIn.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTransferOut = itemTransferOut.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemReceive = itemReceive.Where(p => p.IDWarehouse == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                int Cabang = 0;
                if (Cabang > 0)
                {
                    item = item.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);
                    itemPenjualan = itemPenjualan.Where(p => p.SalesOrder.IDLokasi == Cabang && p.SalesOrder.TipeLokasi == tipe);
                    itemTradeIn = itemTradeIn.Where(p => p.Resell.IDLokasi == Cabang && p.Resell.TipeLokasi == tipe);
                    itemResell = itemResell.Where(p => p.Resell.IDLokasi == Cabang && p.Resell.TipeLokasi == tipe);
                    itemTransferIn = itemTransferIn.Where(p => p.StockIncomingGJ.StockOutgoingGJ.IDTujuan == Cabang && p.StockIncomingGJ.StockOutgoingGJ.TipeTujuan == tipe);
                    itemTransferOut = itemTransferOut.Where(p => p.StockIncomingGJ.StockOutgoingGJ.IDAsal == Cabang && p.StockIncomingGJ.StockOutgoingGJ.TipeAsal == tipe);
                    itemReceive = itemReceive.Where(p => p.IDWarehouse == Cabang);
                    itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi == Cabang && p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                    itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi == Cabang && p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                }
                else
                {
                    //item = item.Where(p => p.TipeLokasi == tipe);
                    //itemPenjualan = itemPenjualan.Where(p => p.SalesOrder.TipeLokasi == tipe);
                    //itemTradeIn = itemTradeIn.Where(p => p.Resell.TipeLokasi == tipe);
                    //itemResell = itemResell.Where(p => p.Resell.TipeLokasi == tipe);
                    //itemTransferIn = itemTransferIn.Where(p => p.StockOutgoingGJ.TipeTujuan == tipe);
                    //itemTransferOut = itemTransferOut.Where(p => p.StockOutgoingGJ.TipeAsal == tipe);
                    //itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                    //itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                }

                try
                {
                    jlh = item.Count();
                    jlhNetweight = Convert.ToDecimal(item.Sum(p => p.NetWeight.Value));
                    jlhHargaM = Convert.ToDecimal(item.Sum(p => p.HargaM.Value));
                    jlhHarga = Convert.ToDecimal(item.Sum(p => p.HargaJual.Value));
                }
                catch { }
                try
                {
                    jlhJual = itemPenjualan.Count();
                    jlhJualButir = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhJualCarat = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhJualNetweight = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhJualHargaM = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhJualHarga = itemPenjualan.Sum(p => p.HargaRupiah.Value);
                }
                catch { }
                try
                {
                    jlhTradeIn = itemTradeIn.Count();
                    jlhTradeInButir = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTradeInCarat = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTradeInNetweight = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTradeInHargaM = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTradeInHarga = itemTradeIn.Sum(p => p.HargaRupiah.Value);
                }
                catch { }

                try
                {
                    jlhResell = itemResell.Count();
                    jlhResellButir = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhResellCarat = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhResellNetweight = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhResellHargaM = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhResellHarga = itemResell.Sum(p => p.HargaRupiah.Value);
                }
                catch { }

                try
                {
                    jlhTransferIn = itemTransferIn.Count();
                    jlhTransferInButir = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTransferInCarat = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTransferInNetweight = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTransferInHargaM = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTransferInHarga = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhTransferOut = itemTransferOut.Count();
                    jlhTransferOutButir = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTransferOutCarat = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTransferOutNetweight = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTransferOutHargaM = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTransferOutHarga = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhReceive = itemReceive.Count();
                    jlhReceiveButir = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.StoneQty.Value);
                    jlhReceiveCarat = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.StoneCarat);
                    jlhReceiveNetweight = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.NettoWeight);
                    jlhReceiveHargaM = itemReceive.Sum(p => p.StockReceiveGJ_Costing.TotalBiaya + p.StockReceiveGJ_CostingProduct.Total);
                    jlhReceiveHarga = itemReceive.Sum(p => p.StockReceiveGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhRetur = itemRetur.Count();
                    jlhReturButir = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhReturCarat = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhReturNetweight = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhReturHargaM = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhReturHarga = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }


                try
                {
                    jlhLebur = itemLebur.Count();
                    jlhLeburButir = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhLeburCarat = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhLeburNetweight = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhLeburHargaM = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhLeburHarga = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                jlhActual = jlh - jlhJual + jlhTradeIn + jlhResell + jlhTransferIn - jlhTransferOut + jlhReceive - jlhLebur - jlhRetur;
                jlhActualCarat = jlhCarat - jlhJualCarat + jlhTradeInCarat + jlhResellCarat + jlhTransferInCarat - jlhTransferOutCarat + jlhReceiveCarat - jlhLeburCarat - jlhReturCarat;
                jlhActualButir = jlhButir - jlhJualButir + jlhTradeInButir + jlhResellButir + jlhTransferInButir - jlhTransferOutButir + jlhReceiveButir - jlhLeburButir - jlhReturButir;
                jlhActualHarga = jlhHarga - jlhJualHarga + jlhTradeInHarga + jlhResellHarga + jlhTransferInHarga - jlhTransferOutHarga + jlhReceiveHarga - jlhLeburHarga - jlhReturHarga;
                jlhActualHargaM = jlhHargaM - jlhJualHargaM + jlhTradeInHargaM + jlhResellHargaM + jlhTransferInHargaM - jlhTransferOutHargaM + jlhReceiveHargaM - jlhLeburHargaM - jlhReturHargaM;
                jlhActualNetweight = jlhNetweight - jlhJualNetweight + jlhTradeInNetweight + jlhResellNetweight + jlhTransferInNetweight - jlhTransferOutNetweight + jlhReceiveNetweight - jlhLeburNetweight - jlhReturNetweight;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlh.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceive.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeIn.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResell.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferIn.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOut.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhRetur.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLebur.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualNetweight.ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                Carat += jlhCarat;
                Butir += jlhButir;
                HargaM += jlhHargaM;
                Harga += jlhHarga;
                Netweight += jlhNetweight;

                QtyReceive += jlhReceive;
                ReceiveCarat += jlhReceiveCarat;
                ReceiveButir += jlhReceiveButir;
                ReceiveHargaM += jlhReceiveHargaM;
                ReceiveHarga += jlhReceiveHarga;
                ReceiveNetweight += jlhReceiveNetweight;

                QtyJual += jlhJual;
                JualCarat += jlhJualCarat;
                JualButir += jlhJualButir;
                JualHargaM += jlhJualHargaM;
                JualHarga += jlhJualHarga;
                JualNetweight += jlhJualNetweight;

                QtyActual += jlhActual;
                ActualCarat += jlhActualCarat;
                ActualButir += jlhActualButir;
                ActualHargaM += jlhActualHargaM;
                ActualHarga += jlhActualHarga;
                ActualNetweight += jlhActualNetweight;

                QtyTradeIn += jlhTradeIn;
                TradeInCarat += jlhTradeInCarat;
                TradeInButir += jlhTradeInButir;
                TradeInHargaM += jlhTradeInHargaM;
                TradeInHarga += jlhTradeInHarga;
                TradeInNetweight += jlhTradeInNetweight;

                QtyResell += jlhResell;
                ResellCarat += jlhResellCarat;
                ResellButir += jlhResellButir;
                ResellHargaM += jlhResellHargaM;
                ResellHarga += jlhResellHarga;
                ResellNetweight += jlhResellNetweight;

                QtyTransferIn += jlhTransferIn;
                TransferInCarat += jlhTransferInCarat;
                TransferInButir += jlhTransferInButir;
                TransferInHargaM += jlhTransferInHargaM;
                TransferInHarga += jlhTransferInHarga;
                TransferInNetweight += jlhTransferInNetweight;

                QtyTransferOut += jlhTransferOut;
                TransferOutCarat += jlhTransferOutCarat;
                TransferOutButir += jlhTransferOutButir;
                TransferOutHargaM += jlhTransferOutHargaM;
                TransferOutHarga += jlhTransferOutHarga;
                TransferOutNetweight += jlhTransferOutNetweight;

                QtyRetur += jlhRetur;
                ReturCarat += jlhReturCarat;
                ReturButir += jlhReturButir;
                ReturHargaM += jlhReturHargaM;
                ReturHarga += jlhReturHarga;
                ReturNetweight += jlhReturNetweight;

                QtyLebur += jlhLebur;
                LeburCarat += jlhLeburCarat;
                LeburButir += jlhLeburButir;
                LeburHargaM += jlhLeburHargaM;
                LeburHarga += jlhLeburHarga;
                LeburNetweight += jlhLeburNetweight;
            }

            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Qty.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Harga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Butir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Carat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Netweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyReceive.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyJual.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyTradeIn.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyResell.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyTransferIn.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyTransferOut.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyRetur.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyLebur.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyActual.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);


            tb.Rows.Add(tr1);
        }
        else if (GroupBy == 5)
        {
            var ps = from p in db.StockProductGJ
                     select p;
            var sp = ps.GroupBy(p => p.SupplierNama);
            foreach (var r in sp)
            {
                jlh = 0;
                jlhJual = 0;
                jlhTradeIn = 0;
                jlhResell = 0;
                jlhTransferIn = 0;
                jlhTransferOut = 0;
                jlhActual = 0;
                jlhReceive = 0;
                jlhLebur = 0;
                jlhRetur = 0;

                jlhButir = 0;
                jlhJualButir = 0;
                jlhTradeInButir = 0;
                jlhResellButir = 0;
                jlhTransferInButir = 0;
                jlhTransferOutButir = 0;
                jlhActualButir = 0;
                jlhReceiveButir = 0;
                jlhLeburButir = 0;
                jlhReturButir = 0;

                jlhCarat = 0;
                jlhJualCarat = 0;
                jlhTradeInCarat = 0;
                jlhResellCarat = 0;
                jlhTransferInCarat = 0;
                jlhTransferOutCarat = 0;
                jlhActualCarat = 0;
                jlhReceiveCarat = 0;
                jlhLeburCarat = 0;
                jlhReturCarat = 0;

                jlhHarga = 0;
                jlhJualHarga = 0;
                jlhTradeInHarga = 0;
                jlhResellHarga = 0;
                jlhTransferInHarga = 0;
                jlhTransferOutHarga = 0;
                jlhActualHarga = 0;
                jlhReceiveHarga = 0;
                jlhLeburHarga = 0;
                jlhReturHarga = 0;

                jlhHargaM = 0;
                jlhJualHargaM = 0;
                jlhTradeInHargaM = 0;
                jlhResellHargaM = 0;
                jlhTransferInHargaM = 0;
                jlhTransferOutHargaM = 0;
                jlhActualHargaM = 0;
                jlhReceiveHargaM = 0;
                jlhLeburHargaM = 0;
                jlhReturHargaM = 0;

                jlhNetweight = 0;
                jlhJualNetweight = 0;
                jlhTradeInNetweight = 0;
                jlhResellNetweight = 0;
                jlhTransferInNetweight = 0;
                jlhTransferOutNetweight = 0;
                jlhActualNetweight = 0;
                jlhReceiveNetweight = 0;
                jlhLeburNetweight = 0;
                jlhReturNetweight = 0;

                var item = from p in db.SaldoStockGJ
                           where p.StockProductGJ.SupplierNama == r.Key
                           && p.Bulan == Bulan && p.Tahun == Tahun
                           select p;

                var itemPenjualan = (from p in db.SalesOrderGJ
                                     where !p.SalesOrder.Draft
                                     && !(from z in db.SalesOrderGJ
                                          
                                          select z.SalesOrder.ID).Contains(p.IDForm)
                                     && p.SalesOrder.StatusPembayaran == true
                                     && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                     && p.StockProductGJ.SupplierNama == r.Key
                                     select p);

                var itemTradeIn = (from p in db.ResellGJ
                                   where (from z in db.TradeIn
                                          where !z.Draft
                                          select z.IDResell).Contains(p.IDForm)
                                           && p.Resell.StatusVoid == null && !p.Resell.Draft && p.SalesOrder.Tgl.HasValue
                                           && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                           && p.StockProductGJ.SupplierNama == r.Key
                                   select p);

                var itemResell = (from p in db.ResellGJ
                                  where !(from z in db.TradeIn
                                          where !z.Draft
                                          select z.IDResell).Contains(p.IDForm)
                                          && p.Resell.StatusVoid == null && !p.Resell.Draft && p.SalesOrder.Tgl.HasValue
                                          && p.SalesOrder.Tgl >= start && p.SalesOrder.Tgl < end
                                          && p.StockProductGJ.SupplierNama == r.Key
                                  select p);

                //var itemTransferIn = (from p in db.StockOutgoingGJ_Product
                //                      where p.StockOutgoingGJ.Tgl >= start && p.StockOutgoingGJ.Tgl < end
                //                      && p.StockOutgoingGJ.StatusTransOut == 4
                //                      && p.StockProductGJ.SupplierNama == r.Key
                //                      select p);

                //var itemTransferOut = (from p in db.StockOutgoingGJ_Product
                //                       where p.StockOutgoingGJ.Tgl >= start && p.StockOutgoingGJ.Tgl < end
                //                       && p.StockOutgoingGJ.StatusTransOut == 4
                //                       && p.StockProductGJ.SupplierNama == r.Key
                //                       select p);

                var itemTransferIn = (from p in db.StockIncomingGJ_Product
                                      where p.StockIncomingGJ.Tgl >= start && p.StockIncomingGJ.Tgl < end.AddDays(1)
                                      && p.StockIncomingGJ.StockOutgoingGJ.StatusTransOut == 4 && p.StockIncomingGJ.StatusTransIn == 2
                                      && p.StockProductGJ.SupplierNama == r.Key
                                      && p.SubStorageResult != 99
                                      select p);

                var itemTransferOut = (from p in db.StockIncomingGJ_Product
                                       where p.StockIncomingGJ.Tgl >= start && p.StockIncomingGJ.Tgl < end.AddDays(1)
                                       && p.StockIncomingGJ.StockOutgoingGJ.StatusTransOut == 4 && p.StockIncomingGJ.StatusTransIn == 2
                                       && p.StockProductGJ.SupplierNama == r.Key
                                       && p.SubStorageResult != 99
                                       select p);

                var itemReceive = (from p in db.StockReceiveGJ
                                   where p.ApprovalTgl >= start && p.ApprovalTgl < end
                                   && p.Approval != null
                                   && p.SupplierNama == r.Key
                                   select p);

                var itemRetur = (from p in db.StockReturnGJ_Product
                                 where p.StockReturnGJ.ApprovalTgl >= start && p.StockReturnGJ.ApprovalTgl < end
                                 && p.StockReturnGJ.Approval != null
                                 && p.StockProductGJ.SupplierNama == r.Key
                                 select p);

                var itemLebur = (from p in db.StockRetireGJ
                                 where p.ApprovalTgl >= start && p.ApprovalTgl < end
                                 && p.Approval != null
                                 && p.StockProductGJ.SupplierNama == r.Key
                                 select p);

                int UserID = App.UserID;
                var d = from p in db.DataAdminSales
                        where p.IDUser == UserID
                        select p;
                if (d.Count() > 0)
                {
                    item = item.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemPenjualan = itemPenjualan.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTradeIn = itemTradeIn.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemResell = itemResell.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTransferIn = itemTransferIn.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemTransferOut = itemTransferOut.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemReceive = itemReceive.Where(p => p.IDWarehouse == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                if (d.Count() > 0)
                {
                    itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockProductGJ.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                int Cabang = 0;
                if (Cabang > 0)
                {
                    item = item.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);
                    itemPenjualan = itemPenjualan.Where(p => p.SalesOrder.IDLokasi == Cabang && p.SalesOrder.TipeLokasi == tipe);
                    itemTradeIn = itemTradeIn.Where(p => p.Resell.IDLokasi == Cabang && p.Resell.TipeLokasi == tipe);
                    itemResell = itemResell.Where(p => p.Resell.IDLokasi == Cabang && p.Resell.TipeLokasi == tipe);
                    itemTransferIn = itemTransferIn.Where(p => p.StockIncomingGJ.StockOutgoingGJ.IDTujuan == Cabang && p.StockIncomingGJ.StockOutgoingGJ.TipeTujuan == tipe);
                    itemTransferOut = itemTransferOut.Where(p => p.StockIncomingGJ.StockOutgoingGJ.IDAsal == Cabang && p.StockIncomingGJ.StockOutgoingGJ.TipeAsal == tipe);
                    itemReceive = itemReceive.Where(p => p.IDWarehouse == Cabang);
                    itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi == Cabang && p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                    itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi == Cabang && p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                }
                else
                {
                    //item = item.Where(p => p.TipeLokasi == tipe);
                    //itemPenjualan = itemPenjualan.Where(p => p.SalesOrder.TipeLokasi == tipe);
                    //itemTradeIn = itemTradeIn.Where(p => p.Resell.TipeLokasi == tipe);
                    //itemResell = itemResell.Where(p => p.Resell.TipeLokasi == tipe);
                    //itemTransferIn = itemTransferIn.Where(p => p.StockOutgoingGJ.TipeTujuan == tipe);
                    //itemTransferOut = itemTransferOut.Where(p => p.StockOutgoingGJ.TipeAsal == tipe);
                    //itemRetur = itemRetur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                    //itemLebur = itemLebur.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi == tipe);
                }

                try
                {
                    jlh = item.Count();
                    jlhNetweight = Convert.ToDecimal(item.Sum(p => p.NetWeight.Value));
                    jlhHargaM = Convert.ToDecimal(item.Sum(p => p.HargaM.Value));
                    jlhHarga = Convert.ToDecimal(item.Sum(p => p.HargaJual.Value));
                }
                catch { }
                try
                {
                    jlhJual = itemPenjualan.Count();
                    jlhJualButir = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhJualCarat = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhJualNetweight = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhJualHargaM = itemPenjualan.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhJualHarga = itemPenjualan.Sum(p => p.HargaRupiah.Value);
                }
                catch { }
                try
                {
                    jlhTradeIn = itemTradeIn.Count();
                    jlhTradeInButir = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTradeInCarat = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTradeInNetweight = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTradeInHargaM = itemTradeIn.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTradeInHarga = itemTradeIn.Sum(p => p.HargaRupiah.Value);
                }
                catch { }

                try
                {
                    jlhResell = itemResell.Count();
                    jlhResellButir = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhResellCarat = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhResellNetweight = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhResellHargaM = itemResell.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhResellHarga = itemResell.Sum(p => p.HargaRupiah.Value);
                }
                catch { }

                try
                {
                    jlhTransferIn = itemTransferIn.Count();
                    jlhTransferInButir = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTransferInCarat = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTransferInNetweight = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTransferInHargaM = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTransferInHarga = itemTransferIn.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhTransferOut = itemTransferOut.Count();
                    jlhTransferOutButir = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhTransferOutCarat = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhTransferOutNetweight = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhTransferOutHargaM = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhTransferOutHarga = itemTransferOut.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhReceive = itemReceive.Count();
                    jlhReceiveButir = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.StoneQty.Value);
                    jlhReceiveCarat = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.StoneCarat);
                    jlhReceiveNetweight = itemReceive.Sum(p => p.StockReceiveGJ_CharProduct.NettoWeight);
                    jlhReceiveHargaM = itemReceive.Sum(p => p.StockReceiveGJ_Costing.TotalBiaya + p.StockReceiveGJ_CostingProduct.Total);
                    jlhReceiveHarga = itemReceive.Sum(p => p.StockReceiveGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                try
                {
                    jlhRetur = itemRetur.Count();
                    jlhReturButir = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhReturCarat = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhReturNetweight = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhReturHargaM = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhReturHarga = itemRetur.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }


                try
                {
                    jlhLebur = itemLebur.Count();
                    jlhLeburButir = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);
                    jlhLeburCarat = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);
                    jlhLeburNetweight = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                    jlhLeburHargaM = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total);
                    jlhLeburHarga = itemLebur.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                }
                catch { }

                jlhActual = jlh - jlhJual + jlhTradeIn + jlhResell + jlhTransferIn - jlhTransferOut + jlhReceive - jlhLebur - jlhRetur;
                jlhActualCarat = jlhCarat - jlhJualCarat + jlhTradeInCarat + jlhResellCarat + jlhTransferInCarat - jlhTransferOutCarat + jlhReceiveCarat - jlhLeburCarat - jlhReturCarat;
                jlhActualButir = jlhButir - jlhJualButir + jlhTradeInButir + jlhResellButir + jlhTransferInButir - jlhTransferOutButir + jlhReceiveButir - jlhLeburButir - jlhReturButir;
                jlhActualHarga = jlhHarga - jlhJualHarga + jlhTradeInHarga + jlhResellHarga + jlhTransferInHarga - jlhTransferOutHarga + jlhReceiveHarga - jlhLeburHarga - jlhReturHarga;
                jlhActualHargaM = jlhHargaM - jlhJualHargaM + jlhTradeInHargaM + jlhResellHargaM + jlhTransferInHargaM - jlhTransferOutHargaM + jlhReceiveHargaM - jlhLeburHargaM - jlhReturHargaM;
                jlhActualNetweight = jlhNetweight - jlhJualNetweight + jlhTradeInNetweight + jlhResellNetweight + jlhTransferInNetweight - jlhTransferOutNetweight + jlhReceiveNetweight - jlhLeburNetweight - jlhReturNetweight;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Key;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlh.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceive.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReceiveNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhJualNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeIn.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTradeInNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResell.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhResellNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferIn.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhTransferInNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOut.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhTransferOutNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhRetur.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhReturNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLebur.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = jlhLeburNetweight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = jlhActualNetweight.ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                Carat += jlhCarat;
                Butir += jlhButir;
                HargaM += jlhHargaM;
                Harga += jlhHarga;
                Netweight += jlhNetweight;

                QtyReceive += jlhReceive;
                ReceiveCarat += jlhReceiveCarat;
                ReceiveButir += jlhReceiveButir;
                ReceiveHargaM += jlhReceiveHargaM;
                ReceiveHarga += jlhReceiveHarga;
                ReceiveNetweight += jlhReceiveNetweight;

                QtyJual += jlhJual;
                JualCarat += jlhJualCarat;
                JualButir += jlhJualButir;
                JualHargaM += jlhJualHargaM;
                JualHarga += jlhJualHarga;
                JualNetweight += jlhJualNetweight;

                QtyActual += jlhActual;
                ActualCarat += jlhActualCarat;
                ActualButir += jlhActualButir;
                ActualHargaM += jlhActualHargaM;
                ActualHarga += jlhActualHarga;
                ActualNetweight += jlhActualNetweight;

                QtyTradeIn += jlhTradeIn;
                TradeInCarat += jlhTradeInCarat;
                TradeInButir += jlhTradeInButir;
                TradeInHargaM += jlhTradeInHargaM;
                TradeInHarga += jlhTradeInHarga;
                TradeInNetweight += jlhTradeInNetweight;

                QtyResell += jlhResell;
                ResellCarat += jlhResellCarat;
                ResellButir += jlhResellButir;
                ResellHargaM += jlhResellHargaM;
                ResellHarga += jlhResellHarga;
                ResellNetweight += jlhResellNetweight;

                QtyTransferIn += jlhTransferIn;
                TransferInCarat += jlhTransferInCarat;
                TransferInButir += jlhTransferInButir;
                TransferInHargaM += jlhTransferInHargaM;
                TransferInHarga += jlhTransferInHarga;
                TransferInNetweight += jlhTransferInNetweight;

                QtyTransferOut += jlhTransferOut;
                TransferOutCarat += jlhTransferOutCarat;
                TransferOutButir += jlhTransferOutButir;
                TransferOutHargaM += jlhTransferOutHargaM;
                TransferOutHarga += jlhTransferOutHarga;
                TransferOutNetweight += jlhTransferOutNetweight;

                QtyRetur += jlhRetur;
                ReturCarat += jlhReturCarat;
                ReturButir += jlhReturButir;
                ReturHargaM += jlhReturHargaM;
                ReturHarga += jlhReturHarga;
                ReturNetweight += jlhReturNetweight;

                QtyLebur += jlhLebur;
                LeburCarat += jlhLeburCarat;
                LeburButir += jlhLeburButir;
                LeburHargaM += jlhLeburHargaM;
                LeburHarga += jlhLeburHarga;
                LeburNetweight += jlhLeburNetweight;
            }

            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Qty.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Harga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Butir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Carat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + Netweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyReceive.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReceiveNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyJual.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + JualNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyTradeIn.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TradeInNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyResell.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + ResellNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyTransferIn.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + TransferInNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyTransferOut.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + TransferOutNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyRetur.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ReturNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + QtyLebur.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td1.Text = "<b>" + LeburNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + QtyActual.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualHarga.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualButir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualCarat.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Attributes["style"] = "text-align: center;";
            td1.Text = "<b>" + ActualNetweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);


            tb.Rows.Add(tr1);
        }
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            
            groupby.SelectedValue = Request.QueryString["groupby"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&tipe=" + tipe;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "GJ_Saldo_Export.aspx?id=1";

        //Customize-Qstr
        href += "&tipe=" + tipe;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "GJ_Saldo_Print.aspx?id=1";

        //Customize-Qstr
        href += "&tipe=" + tipe;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
}