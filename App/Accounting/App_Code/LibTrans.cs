using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Configuration;


public partial class Lib
{
    /* PENTING !!! MOHON DIBACA !!!
     * Fungsi dibawah ini harus disamakan ke App_Code/LibTrans.cs modul Accounting & TransIncoming & TransOutgoing
     */
    public static void ReportOutgoingIncoming_Filter(
        DropDownList ddlc_tipe, DropDownList ddlc_id, DropDownList ddlf_tipe, DropDownList ddlf_id,
        DropDownList ddl_brand, DropDownList ddl_status,
        TextBox txt_TglFrom, TextBox txt_TglTo, TextBox txt_Keyword,
        System.Web.UI.Page page,
        System.Web.HttpRequest request,
        String Sign = "outgoing"
        )
    {
        using (Map_DB db = new Map_DB())
        {

            /// Init

            /// DLC
            DLC.CompanyBrand(ddl_brand);
            if (Sign == "outgoing") { DLC.OutgoingApprovalStatus(ddl_status); } else { DLC.IncomingApprovalStatus(ddl_status); }

            /// TransOutgoing : Lokasi tujuan terbuka untuk semua
            ddlf_tipe.Items.Clear();
            ddlf_tipe.Items.Add(new ListItem("All", "-1"));
            ddlf_tipe.Items.Add(new ListItem("Warehouse", "0"));
            ddlf_tipe.Items.Add(new ListItem("Outlet", "1"));
            ddlf_tipe.Items.Add(new ListItem("Exhibition", "2"));

            ddlf_id.Items.Clear();
            ddlf_id.Items.Add(new ListItem("All", "-1"));

            //Form-Default
            txt_TglFrom.Text = Str.Tgl(DateDrv.AwalBulan());
            txt_TglTo.Text = Str.Tgl(DateTime.Today);

            /// Check apakah User Login adalah AE (outlet) atau SK (warehouse), jika tidak maka buka semua filter
            int UserID = App.UserID;
            var d = from p in db.DataAdminSales
                    where p.IDUser == UserID
                    select p;
            var f = from p in db.DataSK
                    where p.IDUser == UserID
                    select p;

            /// Kondisi DLC untuk TipeLokasi dan IDLokasi (From -> utk Outgoing, To -> utk Incoming)
            if (d.Count() > 0 || f.Count() > 0)
            {
                if (d.Count() > 0)
                {
                    var x = d.First();

                    if (x.TipeLokasi == 1)
                    {
                        ddlc_tipe.Items.Clear();
                        ddlc_tipe.Items.Add(new ListItem("Outlet", "1"));

                        ddlc_id.Items.Clear();
                        DLC.LocationList(ddlc_id, 1, Convert.ToInt32(d.FirstOrDefault().IDLokasi));

                    }
                    else if (x.TipeLokasi == 2)
                    {
                        ddlc_tipe.Items.Clear();
                        ddlc_tipe.Items.Add(new ListItem("Exhibition", "2"));

                        ddlc_id.Items.Clear();
                        DLC.LocationList(ddlc_id, 2, Convert.ToInt32(d.FirstOrDefault().IDLokasi));
                    }
                }
                else if (f.Count() > 0)
                {
                    var x = f.First();

                    if (x.TipeLokasi == 0)
                    {
                        ddlc_tipe.Items.Clear();
                        ddlc_tipe.Items.Add(new ListItem("Warehouse", "0"));

                        ddlc_id.Items.Clear();
                        DLC.LocationList(ddlc_id, 0, Convert.ToInt32(d.FirstOrDefault().IDLokasi));
                    }
                    else if (x.TipeLokasi == 1)
                    {
                        ddlc_tipe.Items.Clear();
                        ddlc_tipe.Items.Add(new ListItem("Outlet", "1"));

                        ddlc_id.Items.Clear();
                        DLC.LocationList(ddlc_id, 1, Convert.ToInt32(d.FirstOrDefault().IDLokasi));
                    }
                    else if (x.TipeLokasi == 2)
                    {
                        ddlc_tipe.Items.Clear();
                        ddlc_tipe.Items.Add(new ListItem("Exhibition", "2"));

                        ddlc_id.Items.Clear();
                        DLC.LocationList(ddlc_id, 2, Convert.ToInt32(d.FirstOrDefault().IDLokasi));
                    }
                }
            }
            else
            {
                ddlc_tipe.Items.Clear();
                ddlc_tipe.Items.Add(new ListItem("All", "-1"));
                ddlc_tipe.Items.Add(new ListItem("Warehouse", "0"));
                ddlc_tipe.Items.Add(new ListItem("Outlet", "1"));
                ddlc_tipe.Items.Add(new ListItem("Exhibition", "2"));

                ddlc_id.Items.Clear();
                ddlc_id.Items.Add(new ListItem("All", "-1"));
            }

            /// Postback filter
            if (App.Refresh(page))
            {
                /// Refresh DDL from
                if (d.Count() == 0 && f.Count() == 0)
                {
                    if (Sign == "outgoing")
                    {
                        ddlc_id.Items.Clear();
                        if (request.QueryString["tipelokasifrom"] != "-1")
                        {
                            if (request.QueryString["tipelokasifrom"] == "0") { DLC.LocationList(ddlc_id, 0); }
                            else if (request.QueryString["tipelokasifrom"] == "1") { DLC.LocationList(ddlc_id, 1); }
                            else if (request.QueryString["tipelokasifrom"] == "2") { DLC.LocationList(ddlc_id, 2); }
                        }
                        else
                        {
                            ddlc_id.Items.Clear();
                            ddlc_id.Items.Add(new ListItem("All", "-1"));
                        }
                    }
                    else if (Sign == "incoming")
                    {
                        ddlc_id.Items.Clear();
                        if (request.QueryString["tipelokasito"] != "-1")
                        {
                            if (request.QueryString["tipelokasito"] == "0") { DLC.LocationList(ddlc_id, 0); }
                            else if (request.QueryString["tipelokasito"] == "1") { DLC.LocationList(ddlc_id, 1); }
                            else if (request.QueryString["tipelokasito"] == "2") { DLC.LocationList(ddlc_id, 2); }
                        }
                        else
                        {
                            ddlc_id.Items.Clear();
                            ddlc_id.Items.Add(new ListItem("All", "-1"));
                        }
                    }
                }

                /// Refresh DDL to
                if (Sign == "outgoing")
                {
                    ddlf_id.Items.Clear();
                    if (request.QueryString["tipelokasito"] != "-1")
                    {
                        if (request.QueryString["tipelokasito"] == "0") { DLC.LocationList(ddlf_id, 0); }
                        else if (request.QueryString["tipelokasito"] == "1") { DLC.LocationList(ddlf_id, 1); }
                        else if (request.QueryString["tipelokasito"] == "2") { DLC.LocationList(ddlf_id, 2); }
                    }
                    else
                    {
                        ddlf_id.Items.Clear();
                        ddlf_id.Items.Add(new ListItem("All", "-1"));
                    }
                }
                else if (Sign == "incoming")
                {
                    ddlf_id.Items.Clear();
                    if (request.QueryString["tipelokasifrom"] != "-1")
                    {
                        if (request.QueryString["tipelokasifrom"] == "0") { DLC.LocationList(ddlf_id, 0); }
                        else if (request.QueryString["tipelokasifrom"] == "1") { DLC.LocationList(ddlf_id, 1); }
                        else if (request.QueryString["tipelokasifrom"] == "2") { DLC.LocationList(ddlf_id, 2); }
                    }
                    else
                    {
                        ddlf_id.Items.Clear();
                        ddlf_id.Items.Add(new ListItem("All", "-1"));
                    }
                }

                txt_TglFrom.Text = request.QueryString["pf"];
                txt_TglTo.Text = request.QueryString["pt"];
                ddl_brand.SelectedValue = request.QueryString["brand"];
                ddlc_tipe.SelectedValue = request.QueryString[((Sign == "outgoing") ? "tipelokasifrom" : "tipelokasito")];
                ddlf_tipe.SelectedValue = request.QueryString[((Sign == "outgoing") ? "tipelokasito" : "tipelokasifrom")];
                ddlc_id.SelectedValue = request.QueryString[((Sign == "outgoing") ? "lokasifrom" : "lokasito")];
                ddlf_id.SelectedValue = request.QueryString[((Sign == "outgoing") ? "lokasito" : "lokasifrom")];
                ddl_status.SelectedValue = request.QueryString["status"];
                txt_Keyword.Text = request.QueryString["keyword"];

            }
            }
}

    /* PENTING !!! MOHON DIBACA !!!
     * Fungsi dibawah ini harus disamakan ke App_Code/LibTrans.cs modul Accounting & TransIncoming & TransOutgoing
     */
    public static void ReportOutgoingIncoming_FillTable(
        String Mode, String Tipe, String Modul, String Sign, String Action,
        Table tbl_Table,
        String s_tF,
        String s_kF,
        String s_tT,
        String s_kT,
        String s_vB,
        String s_stats,
        String s_pf,
        String s_pt,
        String s_kw,
        System.Web.UI.Page page,
        System.Web.HttpRequest request,
        System.Web.HttpResponse response
        )
    {
        using (Map_DB db = new Map_DB())
        {

            /// Master Brand
            var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            tbl_Table.Visible = true;

            if (Sign == "outgoing")
            {
                if (Mode == "DJ")
                {
                    if (Tipe.ToLower() == "rekap") // rekap
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockOutgoingDJ select p);

                        /// WHERE
                        rs = rs.Where(p => p.IDBrand == ((vB >= 0) ? vB : p.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransOut == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            if (Action.ToLower() == "display") // display only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "View";
                                htd.Width = 50;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Print";
                                htd.Width = 50;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "R";
                                htd.Width = 150;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                TableRow tr = new TableRow();
                                TableCell td;

                                if (Action.ToLower() == "display") // display only
                                {
                                    td = new TableCell();
                                    td.Text = TD_HyperLink(item.ID, Mode);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = TD_Print(item.ID, Mode);
                                    tr.Cells.Add(td);
                                }

                                td = new TableCell();
                                td.Text = "<p>" + index++ + ".</p>";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = rsBrand[item.IDBrand];
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.Tgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.HtmlBreak(item.Keterangan);
                                tr.Cells.Add(td);

                                string NamaAsal = "";
                                if (item.TipeAsal == 0) { NamaAsal = rsWarehouse[item.IDAsal]; }
                                else if (item.TipeAsal == 1) { NamaAsal = rsOutlet[item.IDAsal]; }
                                else if (item.TipeAsal == 2) { NamaAsal = rsExhibition[item.IDAsal]; }

                                td = new TableCell();
                                td.Text = NamaAsal;
                                tr.Cells.Add(td);

                                string NamaTujuan = "";
                                if (item.TipeTujuan == 0) { NamaTujuan = rsWarehouse[item.IDTujuan]; }
                                else if (item.TipeTujuan == 1) { NamaTujuan = rsOutlet[item.IDTujuan]; }
                                else if (item.TipeTujuan == 2) { NamaTujuan = rsExhibition[item.IDTujuan]; }

                                td = new TableCell();
                                td.Text = NamaTujuan;
                                tr.Cells.Add(td);

                                int Qty = 0;
                                Qty = item.StockOutgoingDJ_Product.Count();

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Qty) : Qty.ToString("N0");
                                //td.Text = Str.Number0(Qty);
                                qty += Qty;
                                tr.Cells.Add(td);

                                var rslt = db.StockIncomingDJ_Product.Where(p => p.StockIncomingDJ.StatusTransIn == 2 && p.StockIncomingDJ.IDOutgoing == item.ID);

                                int QtyReceived = 0;
                                int QtyBroken = 0;
                                int QtyLost = 0;

                                if (rslt != null)
                                {
                                    QtyReceived = (from p in rslt
                                                   where p.StockIncomingDJ.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                   select p).Count();
                                    QtyLost = (from p in rslt
                                               where p.StockIncomingDJ.StatusTransIn == 2 && p.SubStorageResult == 99
                                               select p).Count();
                                    QtyBroken = (from p in rslt
                                                 where p.StockIncomingDJ.StatusTransIn == 2 && p.SubStorageResult == 96
                                                 select p).Count();
                                }

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(QtyReceived) : QtyReceived.ToString("N0");
                                qtyReceived += QtyReceived;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(QtyBroken) : QtyBroken.ToString("N0");
                                qtyBroken += QtyBroken;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(QtyLost) : QtyLost.ToString("N0");
                                qtyLost += QtyLost;
                                tr.Cells.Add(td);

                                decimal Netto = 0m;
                                Netto = item != null ? item.StockOutgoingDJ_Product.Sum(p => (p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight != null && p.SubStorage != 99 ? p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight : 0)) : 0;

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                netto += Netto;
                                tr.Cells.Add(td);

                                decimal Gross = 0m;
                                Gross = item != null ? item.StockOutgoingDJ_Product.Sum(p => (p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight != null && p.SubStorage != 99 ? p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight : 0)) : 0;

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                                gross += Gross;
                                tr.Cells.Add(td);

                                decimal Butir = 0m;
                                Butir = item != null ? item.StockOutgoingDJ_Product.Sum(p => (p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value != null && p.SubStorage != 99 ? p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value : 0)) : 0;

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Butir) : Butir.ToString("N0");
                                butir += Convert.ToDecimal(Butir);
                                tr.Cells.Add(td);

                                decimal Carat = 0m;
                                Carat = item != null ? item.StockOutgoingDJ_Product.Sum(p => (p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat != null && p.SubStorage != 99 ? p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat : 0)) : 0;

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Carat) : Carat.ToString("N3");
                                carat += Carat;
                                tr.Cells.Add(td);

                                decimal PriceR = 0m;
                                PriceR = item != null ? item.StockOutgoingDJ_Product.Sum(p => (p.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value != null && p.SubStorage != 99 ? p.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value : 0)) : 0;

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(PriceR) : PriceR.ToString("N0");
                                harga += Convert.ToDecimal(PriceR);
                                tr.Cells.Add(td);

                                if (Modul.ToLower() == "accounting") // accounting only
                                {
                                    td = new TableCell();
                                    td.Text =  Str.Number2(item.StockOutgoingDJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductDJ.StockProductDJ_Costing.TotalBiaya + p.StockProductDJ.StockProductDJ_CostingProduct.Total : 0));
                                    hargaM += Convert.ToDecimal(item.StockOutgoingDJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductDJ.StockProductDJ_Costing.TotalBiaya + p.StockProductDJ.StockProductDJ_CostingProduct.Total : 0));
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Number2(item.StockOutgoingDJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0));
                                    hargaR += Convert.ToDecimal(item.StockOutgoingDJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0));
                                    tr.Cells.Add(td);
                                }

                                td = new TableCell();
                                td.Text = item.Operator;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.OperatorTgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Approval;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.ApprovalTgl);
                                tr.Cells.Add(td);

                                tbl_Table.Rows.Add(tr);
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            if (Action.ToLower() == "display") // display only
                            {
                                td31 = new TableCell();
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(qty ) : qty.ToString("N0");
                            // td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(qtyBroken) : qtyBroken.ToString("N0");
                            // td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            // td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            // td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(butir) : butir.ToString("N0");
                            //  td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(carat) : carat.ToString("N3");
                            //  td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(harga) : harga.ToString("N0");
                            //   td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                td31.Text = "<b>" + hargaR.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                    else if (Tipe.ToLower() == "detail") // detail
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockOutgoingDJ select p);

                        /// WHERE
                        rs = rs.Where(p => p.IDBrand == ((vB >= 0) ? vB : p.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransOut == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "PLU";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Kode Karet";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Item";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Category";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "R";
                                htd.Width = 150;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                foreach (var u in item.StockOutgoingDJ_Product)
                                {
                                    if (!response.IsClientConnected) break;

                                    TableRow tr = new TableRow();
                                    TableCell td;

                                    td = new TableCell();
                                    td.Text = "<p>" + index++ + ".</p>";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = rsBrand[u.StockOutgoingDJ.IDBrand];
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.Tgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductDJ.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductDJ.ProtoNomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.HtmlBreak(item.Keterangan);
                                    tr.Cells.Add(td);

                                    string NamaAsal = "";
                                    if (item.TipeAsal == 0) { NamaAsal = rsWarehouse[item.IDAsal]; }
                                    else if (item.TipeAsal == 1) { NamaAsal = rsOutlet[item.IDAsal]; }
                                    else if (item.TipeAsal == 2) { NamaAsal = rsExhibition[item.IDAsal]; }

                                    td = new TableCell();
                                    td.Text = NamaAsal;
                                    tr.Cells.Add(td);

                                    string NamaTujuan = "";
                                    if (item.TipeTujuan == 0) { NamaTujuan = rsWarehouse[item.IDTujuan]; }
                                    else if (item.TipeTujuan == 1) { NamaTujuan = rsOutlet[item.IDTujuan]; }
                                    else if (item.TipeTujuan == 2) { NamaTujuan = rsExhibition[item.IDTujuan]; }

                                    td = new TableCell();
                                    td.Text = NamaTujuan;
                                    tr.Cells.Add(td);

                                    int Qty = 0;
                                    Qty = 1;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Qty) : Qty.ToString("N0");
                                   // td.Text = Str.Number0(Qty);
                                    qty += Qty;
                                    tr.Cells.Add(td);

                                    var rslt = db.StockIncomingDJ_Product.Where(p => p.StockIncomingDJ.StatusTransIn == 2 && p.StockIncomingDJ.IDOutgoing == item.ID && p.IDProduct == u.IDProduct);

                                    int QtyReceived = 0;
                                    int QtyBroken = 0;
                                    int QtyLost = 0;

                                    if (rslt != null)
                                    {
                                        QtyReceived = (from p in rslt
                                                       where p.StockIncomingDJ.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                       select p).Count();
                                        QtyLost = (from p in rslt
                                                   where p.StockIncomingDJ.StatusTransIn == 2 && p.SubStorageResult == 99
                                                   select p).Count();
                                        QtyBroken = (from p in rslt
                                                     where p.StockIncomingDJ.StatusTransIn == 2 && p.SubStorageResult == 96
                                                     select p).Count();
                                    }

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(QtyReceived) : QtyReceived.ToString("N0");
                                    //td.Text = Str.Number0(QtyReceived);
                                    qtyReceived += QtyReceived;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(QtyBroken) : QtyBroken.ToString("N0");
                                   // td.Text = Str.Number0(QtyBroken);
                                    qtyBroken += QtyBroken;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(QtyLost) : QtyLost.ToString("N0");
                                    //td.Text = Str.Number0(QtyLost);
                                    qtyLost += QtyLost;
                                    tr.Cells.Add(td);

                                    decimal Netto = 0m;
                                    Netto = u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_CharProduct.NettoWeight : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                    //td.Text = Str.Number2(Netto);
                                    netto += Netto;
                                    tr.Cells.Add(td);

                                    decimal Gross = 0m;
                                    Gross = u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_CharProduct.GrossWeight : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Gross) : Gross.ToString("N2");
                                   // td.Text = Str.Number2(Gross);
                                    gross += Gross;
                                    tr.Cells.Add(td);

                                    decimal Butir = 0m;
                                    Butir = u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Butir) : Butir.ToString("N0");
                                    //td.Text = Str.Number0(Butir);
                                    butir += Convert.ToDecimal(Butir);
                                    tr.Cells.Add(td);

                                    decimal Carat = 0m;
                                    Carat = u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_CharProduct.StoneCarat : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Carat) : Carat.ToString("N3");
                                   // td.Text = Str.Number3(Carat);
                                    carat += Carat;
                                    tr.Cells.Add(td);

                                    decimal PriceR = 0m;
                                    PriceR = u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(PriceR) : PriceR.ToString("N0");
                                   // td.Text = Str.Number0(PriceR);
                                    harga += Convert.ToDecimal(PriceR);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_Costing.TotalBiaya + u.StockProductDJ.StockProductDJ_CostingProduct.Total : 0) : (u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_Costing.TotalBiaya + u.StockProductDJ.StockProductDJ_CostingProduct.Total : 0).ToString("N2");
                                    //td.Text = Str.Number2(u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_Costing.TotalBiaya + u.StockProductDJ.StockProductDJ_CostingProduct.Total : 0);
                                    hargaM += Convert.ToDecimal(u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_Costing.TotalBiaya + u.StockProductDJ.StockProductDJ_CostingProduct.Total : 0);
                                    tr.Cells.Add(td);

                                    td = new TableCell();

                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0) : (u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0).ToString("N2");
                                    //td.Text = Str.Number2(u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0);
                                    hargaR += Convert.ToDecimal(u.SubStorage != 99 ? u.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Operator;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.OperatorTgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Approval;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.ApprovalTgl);
                                    tr.Cells.Add(td);

                                    tbl_Table.Rows.Add(tr);
                                }
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(qty) : qty.ToString("N0");
                           // td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(qtyReceived) : qtyReceived.ToString("N0");
                           // td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(harga) : harga.ToString("N0");
                            //td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaR) : hargaR.ToString("N2");
                                //td31.Text = "<b>" + hargaR.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                }
                else if (Mode == "PG")
                {
                    if (Tipe.ToLower() == "rekap") // rekap
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockOutgoingPG select p);

                        /// WHERE
                        rs = rs.Where(p => p.IDBrand == ((vB >= 0) ? vB : p.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransOut == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            if (Action.ToLower() == "display") // display only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "View";
                                htd.Width = 50;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Print";
                                htd.Width = 50;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                TableRow tr = new TableRow();
                                TableCell td;

                                if (Action.ToLower() == "display") // display only
                                {
                                    td = new TableCell();
                                    td.Text = TD_HyperLink(item.ID, Mode);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = TD_Print(item.ID, Mode);
                                    tr.Cells.Add(td);
                                }

                                td = new TableCell();
                                td.Text = "<p>" + index++ + ".</p>";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = rsBrand[item.IDBrand];
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.Tgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.HtmlBreak(item.Keterangan);
                                tr.Cells.Add(td);

                                string NamaAsal = "";
                                if (item.TipeAsal == 0) { NamaAsal = rsWarehouse[item.IDAsal]; }
                                else if (item.TipeAsal == 1) { NamaAsal = rsOutlet[item.IDAsal]; }
                                else if (item.TipeAsal == 2) { NamaAsal = rsExhibition[item.IDAsal]; }

                                td = new TableCell();
                                td.Text = NamaAsal;
                                tr.Cells.Add(td);

                                string NamaTujuan = "";
                                if (item.TipeTujuan == 0) { NamaTujuan = rsWarehouse[item.IDTujuan]; }
                                else if (item.TipeTujuan == 1) { NamaTujuan = rsOutlet[item.IDTujuan]; }
                                else if (item.TipeTujuan == 2) { NamaTujuan = rsExhibition[item.IDTujuan]; }

                                td = new TableCell();
                                td.Text = NamaTujuan;
                                tr.Cells.Add(td);

                                int Qty = 0;
                                Qty = item.StockOutgoingPG_Product.Count();

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Qty) : Qty.ToString("N0");
                                //td.Text = Str.Number0(Qty);
                                qty += Qty;
                                tr.Cells.Add(td);

                                var rslt = db.StockIncomingPG_Product.Where(p => p.StockIncomingPG.StatusTransIn == 2 && p.StockIncomingPG.IDOutgoing == item.ID);

                                int QtyReceived = 0;
                                int QtyBroken = 0;
                                int QtyLost = 0;

                                if (rslt != null)
                                {
                                    QtyReceived = (from p in rslt
                                                   where p.StockIncomingPG.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                   select p).Count();
                                    QtyLost = (from p in rslt
                                               where p.StockIncomingPG.StatusTransIn == 2 && p.SubStorageResult == 99
                                               select p).Count();
                                    QtyBroken = (from p in rslt
                                                 where p.StockIncomingPG.StatusTransIn == 2 && p.SubStorageResult == 96
                                                 select p).Count();
                                }

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Qty) : Qty.ToString("N0");
                                //td.Text = Str.Number0(QtyReceived);
                                qtyReceived += QtyReceived;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(QtyBroken) : QtyBroken.ToString("N0");
                                //td.Text = Str.Number0(QtyBroken);
                                qtyBroken += QtyBroken;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(QtyLost) : QtyLost.ToString("N0");
                                //td.Text = Str.Number0(QtyLost);
                                qtyLost += QtyLost;
                                tr.Cells.Add(td);

                                decimal Netto = 0m;
                                Netto = item.StockOutgoingPG_Product.Sum(p => p.StockProductPG.NettoWeight.Value);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                //td.Text = Str.Number2(Netto);
                                netto += Netto;
                                tr.Cells.Add(td);

                                decimal Gross = 0m;
                                Gross = item.StockOutgoingPG_Product.Sum(p => p.StockProductPG.GrossWeight.Value);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Gross) : Gross.ToString("N2");
                                //td.Text = Str.Number2(Gross);
                                gross += Gross;
                                tr.Cells.Add(td);

                                decimal Butir = 0m;
                                //Butir = item.StockOutgoingPG_Product.Sum(p => p.StockProductPG.StockProductDJ_CharProduct.StoneQty.Value);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Butir) : Butir.ToString("N0");
                                //td.Text = Str.Number0(Butir);
                                butir += Convert.ToDecimal(Butir);
                                tr.Cells.Add(td);

                                decimal Carat = 0m;
                                //Carat = item.StockOutgoingPG_Product.Sum(p => p.StockProductPG.StockProductPG_CharProduct.StoneCarat);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Carat) : Carat.ToString("N3");
                                //td.Text = Str.Number3(Carat);
                                carat += Carat;
                                tr.Cells.Add(td);

                                decimal PriceR = 0m;
                                PriceR = item.StockOutgoingPG_Product.Sum(p => p.SubStorage != 99 ? p.StockProductPG.TotalHargaJual.Value : 0);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(PriceR) : PriceR.ToString("N0");
                                //td.Text = Str.Number0(PriceR);
                                harga += Convert.ToDecimal(PriceR);
                                tr.Cells.Add(td);

                                if (Modul.ToLower() == "accounting") // accounting only
                                {
                                    Decimal HargaM = 0m;
                                    HargaM = Convert.ToDecimal(item.StockOutgoingPG_Product.Sum(p => p.SubStorage != 99 ? p.StockProductPG.TotalHarga24 / p.StockProductPG.TotalRate : 0));
                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(HargaM) : HargaM.ToString("N2");
                                    //td.Text = Str.Number2(HargaM);
                                    hargaM += HargaM;
                                    tr.Cells.Add(td);

                                }

                                td = new TableCell();
                                td.Text = item.Operator;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.OperatorTgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Approval;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.ApprovalTgl);
                                tr.Cells.Add(td);

                                tbl_Table.Rows.Add(tr);
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            if (Action.ToLower() == "display") // display only
                            {
                                td31 = new TableCell();
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                    else if (Tipe.ToLower() == "detail") // detail
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockOutgoingPG select p);

                        /// WHERE
                        rs = rs.Where(p => p.IDBrand == ((vB >= 0) ? vB : p.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransOut == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "PLU";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                foreach (var u in item.StockOutgoingPG_Product)
                                {
                                    if (!response.IsClientConnected) break;

                                    TableRow tr = new TableRow();
                                    TableCell td;

                                    td = new TableCell();
                                    td.Text = "<p>" + index++ + ".</p>";
                                    td.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = rsBrand[u.StockOutgoingPG.IDBrand];
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.Tgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductPG.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.HtmlBreak(item.Keterangan);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.NamaAsal;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.NamaTujuan;
                                    tr.Cells.Add(td);

                                    int Qty = 1;

                                    var rslt = db.StockIncomingPG_Product.Where(p => p.StockIncomingPG.StatusTransIn == 2 && p.StockIncomingPG.IDOutgoing == item.ID && p.IDProduct == u.IDProduct);

                                    int QtyReceived = 0;
                                    int QtyBroken = 0;
                                    int QtyLost = 0;

                                    if (rslt != null)
                                    {
                                        QtyReceived = (from p in rslt
                                                       where p.StockIncomingPG.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                       select p).Count();
                                        QtyLost = (from p in rslt
                                                   where p.StockIncomingPG.StatusTransIn == 2 && p.SubStorageResult == 99
                                                   select p).Count();
                                        QtyBroken = (from p in rslt
                                                     where p.StockIncomingPG.StatusTransIn == 2 && p.SubStorageResult == 96
                                                     select p).Count();
                                    }

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Qty) : Qty.ToString("N0");
                                   // td.Text = Str.Number0(Qty);
                                    qty += Qty;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(QtyReceived) : QtyReceived.ToString("N0");
                                   // td.Text = Str.Number0(QtyReceived);
                                    qtyReceived += QtyReceived;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(QtyBroken) : QtyBroken.ToString("N0");
                                   // td.Text = Str.Number0(QtyBroken);
                                    qtyBroken += QtyBroken;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(QtyLost) : QtyLost.ToString("N0");
                                   // td.Text = Str.Number0(QtyLost);
                                    qtyLost += QtyLost;
                                    tr.Cells.Add(td);

                                    decimal Netto = 0m;
                                    Netto = u.StockProductPG.NettoWeight.Value;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                    //td.Text = Str.Number2(Netto);
                                    netto += Netto;
                                    td.HorizontalAlign = HorizontalAlign.Right;
                                    tr.Cells.Add(td);

                                    decimal Gross = 0m;
                                    Gross = u.StockProductPG.GrossWeight.Value;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Gross) : Gross.ToString("N2");
                                    //td.Text = Str.Number2(Gross);
                                    gross += Gross;
                                    td.HorizontalAlign = HorizontalAlign.Right;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Number0(0);
                                    td.HorizontalAlign = HorizontalAlign.Right;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Number3(0);
                                    td.HorizontalAlign = HorizontalAlign.Right;
                                    tr.Cells.Add(td);

                                    decimal PriceR = 0m;
                                    PriceR = u.SubStorage != 99 ? u.StockProductPG.TotalHargaJual.Value : 0;

    
    
                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(PriceR) : PriceR.ToString("N0");
                                    //td.Text = Str.Number0(PriceR);
                                    harga += PriceR;
                                    td.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(td);

                                    decimal PriceM = 0m;
                                    PriceM = u.SubStorage != 99 ? u.StockProductPG.TotalHarga24.Value / u.StockProductPG.TotalRate.Value : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(PriceM) : PriceM.ToString("N2");
                                    //td.Text = Str.Number2(PriceM);
                                    hargaM += PriceM;
                                    td.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Operator;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.OperatorTgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Approval;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.ApprovalTgl);
                                    tr.Cells.Add(td);

                                    tbl_Table.Rows.Add(tr);
                                }
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(harga) : harga.ToString("N0");
                            //td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                }
                else if (Mode == "GJ")
                {
                    if (Tipe.ToLower() == "rekap") // rekap
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockOutgoingGJ select p);

                        /// WHERE
                        rs = rs.Where(p => p.IDBrand == ((vB >= 0) ? vB : p.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransOut == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            if (Action.ToLower() == "display") // display only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "View";
                                htd.Width = 50;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Print";
                                htd.Width = 50;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "R";
                                htd.Width = 150;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                TableRow tr = new TableRow();
                                TableCell td;

                                if (Action.ToLower() == "display") // display only
                                {
                                    td = new TableCell();
                                    td.Text = TD_HyperLink(item.ID, Mode);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = TD_Print(item.ID, Mode);
                                    tr.Cells.Add(td);
                                }

                                td = new TableCell();
                                td.Text = "<p>" + index++ + ".</p>";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = rsBrand[item.IDBrand];
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.Tgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.HtmlBreak(item.Keterangan);
                                tr.Cells.Add(td);

                                string NamaAsal = "";
                                if (item.TipeAsal == 0) { NamaAsal = rsWarehouse[item.IDAsal]; }
                                else if (item.TipeAsal == 1) { NamaAsal = rsOutlet[item.IDAsal]; }
                                else if (item.TipeAsal == 2) { NamaAsal = rsExhibition[item.IDAsal]; }

                                td = new TableCell();
                                td.Text = NamaAsal;
                                tr.Cells.Add(td);

                                string NamaTujuan = "";
                                if (item.TipeTujuan == 0) { NamaTujuan = rsWarehouse[item.IDTujuan]; }
                                else if (item.TipeTujuan == 1) { NamaTujuan = rsOutlet[item.IDTujuan]; }
                                else if (item.TipeTujuan == 2) { NamaTujuan = rsExhibition[item.IDTujuan]; }

                                td = new TableCell();
                                td.Text = NamaTujuan;
                                tr.Cells.Add(td);

                                int Qty = 0;
                                Qty = item.StockOutgoingGJ_Product.Count();

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Qty) : Qty.ToString("N0");
                                //td.Text = Str.Number0(Qty);
                                qty += Qty;
                                tr.Cells.Add(td);

                                var rslt = db.StockIncomingGJ_Product.Where(p => p.StockIncomingGJ.StatusTransIn == 2 && p.StockIncomingGJ.IDOutgoing == item.ID);

                                int QtyReceived = 0;
                                int QtyBroken = 0;
                                int QtyLost = 0;

                                if (rslt != null)
                                {
                                    QtyReceived = (from p in rslt
                                                   where p.StockIncomingGJ.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                   select p).Count();
                                    QtyLost = (from p in rslt
                                               where p.StockIncomingGJ.StatusTransIn == 2 && p.SubStorageResult == 99
                                               select p).Count();
                                    QtyBroken = (from p in rslt
                                                 where p.StockIncomingGJ.StatusTransIn == 2 && p.SubStorageResult == 96
                                                 select p).Count();
                                }

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyReceived) : QtyReceived.ToString("N0");
                                //td.Text = Str.Number0(QtyReceived);
                                qtyReceived += QtyReceived;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyBroken) : QtyBroken.ToString("N0");
                                //td.Text = Str.Number0(QtyBroken);
                                qtyBroken += QtyBroken;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyLost) : QtyLost.ToString("N0");
                                //td.Text = Str.Number0(QtyLost);
                                qtyLost += QtyLost;
                                tr.Cells.Add(td);

                                decimal Netto = 0m;
                                Netto = item.StockOutgoingGJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight : 0);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                //td.Text = Str.Number2(Netto);
                                netto += Netto;
                                tr.Cells.Add(td);

                                decimal Gross = 0m;
                                Gross = item.StockOutgoingGJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductGJ.StockProductGJ_CharProduct.GrossWeight : 0);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Gross) : Gross.ToString("N2");
                                //td.Text = Str.Number2(Gross);
                                gross += Gross;
                                tr.Cells.Add(td);

                                decimal Butir = 0m;
                                //Butir = item.StockOutgoingGJ_Product.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Butir) : Butir.ToString("N0");
                                //td.Text = Str.Number0(Butir);
                                butir += Convert.ToDecimal(Butir);
                                tr.Cells.Add(td);

                                decimal Carat = 0m;
                                //Carat = item.StockOutgoingGJ_Product.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number3(Carat) : Carat.ToString("N3");
                                //td.Text = Str.Number3(Carat);
                                carat += Carat;
                                tr.Cells.Add(td);

                                decimal PriceR = 0m;
                                PriceR = item.StockOutgoingGJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value : 0);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(PriceR) : PriceR.ToString("N0");
                                //td.Text = Str.Number0(PriceR);
                                harga += Convert.ToDecimal(PriceR);
                                tr.Cells.Add(td);

                                if (Modul.ToLower() == "accounting") // accounting only
                                {
                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(item.StockOutgoingGJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total : 0)) : (item.StockOutgoingGJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total : 0)).ToString("N2");
                                    //td.Text = Str.Number2(item.StockOutgoingGJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total : 0));
                                    hargaM += Convert.ToDecimal(item.StockOutgoingGJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total : 0));
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(item.StockOutgoingGJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0)) : (item.StockOutgoingGJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0)).ToString("N2");
                                    //td.Text = Str.Number2(item.StockOutgoingGJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0));
                                    hargaR += Convert.ToDecimal(item.StockOutgoingGJ_Product.Sum(p => p.SubStorage != 99 ? p.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0));
                                    tr.Cells.Add(td);
                                }

                                td = new TableCell();
                                td.Text = item.Operator;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.OperatorTgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Approval;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.ApprovalTgl);
                                tr.Cells.Add(td);

                                tbl_Table.Rows.Add(tr);
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            if (Action.ToLower() == "display") // display only
                            {
                                td31 = new TableCell();
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number3(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(harga) : harga.ToString("N0");
                            //td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaR) : hargaR.ToString("N2");
                                //td31.Text = "<b>" + hargaR.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                    else if (Tipe.ToLower() == "detail") // detail
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockOutgoingGJ select p);

                        /// WHERE
                        rs = rs.Where(p => p.IDBrand == ((vB >= 0) ? vB : p.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransOut == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "PLU";
                            htd.Width = 120;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Kode Karet";
                            htd.Width = 120;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "R";
                                htd.Width = 150;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                foreach (var u in item.StockOutgoingGJ_Product)
                                {
                                    if (!response.IsClientConnected) break;

                                    TableRow tr = new TableRow();
                                    TableCell td;

                                    td = new TableCell();
                                    td.Text = "<p>" + index++ + ".</p>";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = rsBrand[u.StockOutgoingGJ.IDBrand];
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.Tgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductGJ.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductGJ.ProtoNomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.HtmlBreak(item.Keterangan);
                                    tr.Cells.Add(td);

                                    string NamaAsal = "";
                                    if (item.TipeAsal == 0) { NamaAsal = rsWarehouse[item.IDAsal]; }
                                    else if (item.TipeAsal == 1) { NamaAsal = rsOutlet[item.IDAsal]; }
                                    else if (item.TipeAsal == 2) { NamaAsal = rsExhibition[item.IDAsal]; }

                                    td = new TableCell();
                                    td.Text = NamaAsal;
                                    tr.Cells.Add(td);

                                    string NamaTujuan = "";
                                    if (item.TipeTujuan == 0) { NamaTujuan = rsWarehouse[item.IDTujuan]; }
                                    else if (item.TipeTujuan == 1) { NamaTujuan = rsOutlet[item.IDTujuan]; }
                                    else if (item.TipeTujuan == 2) { NamaTujuan = rsExhibition[item.IDTujuan]; }

                                    td = new TableCell();
                                    td.Text = NamaTujuan;
                                    tr.Cells.Add(td);

                                    int Qty = 1;

                                    var rslt = db.StockIncomingGJ_Product.Where(p => p.StockIncomingGJ.StatusTransIn == 2 && p.StockIncomingGJ.IDOutgoing == item.ID && p.IDProduct == u.IDProduct);

                                    int QtyReceived = 0;
                                    int QtyBroken = 0;
                                    int QtyLost = 0;

                                    if (rslt != null)
                                    {
                                        QtyReceived = (from p in rslt
                                                       where p.StockIncomingGJ.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                       select p).Count();
                                        QtyLost = (from p in rslt
                                                   where p.StockIncomingGJ.StatusTransIn == 2 && p.SubStorageResult == 99
                                                   select p).Count();
                                        QtyBroken = (from p in rslt
                                                     where p.StockIncomingGJ.StatusTransIn == 2 && p.SubStorageResult == 96
                                                     select p).Count();
                                    }

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(Qty) : Qty.ToString("N0");
                                    //td.Text = Str.Number0(Qty);
                                    qty += Qty;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyReceived) : QtyReceived.ToString("N0");
                                    //td.Text = Str.Number0(QtyReceived);
                                    qtyReceived += QtyReceived;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyBroken) : QtyBroken.ToString("N0");
                                    //td.Text = Str.Number0(QtyBroken);
                                    qtyBroken += QtyBroken;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyLost) : QtyLost.ToString("N0");
                                    //td.Text = Str.Number0(QtyLost);
                                    qtyLost += QtyLost;
                                    tr.Cells.Add(td);

                                    decimal Netto = 0m;
                                    Netto = u.SubStorage != 99 ? u.StockProductGJ.StockProductGJ_CharProduct.NettoWeight : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                    //td.Text = Str.Number2(Netto);
                                    netto += Netto;
                                    tr.Cells.Add(td);

                                    decimal Gross = 0m;
                                    Gross = u.SubStorage != 99 ? u.StockProductGJ.StockProductGJ_CharProduct.GrossWeight : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Gross) : Gross.ToString("N2");
                                    //td.Text = Str.Number2(Gross);
                                    gross += Gross;
                                    tr.Cells.Add(td);

                                    decimal Butir = 0m;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(Butir) : Butir.ToString("N0");
                                    //td.Text = Str.Number0(Butir);
                                    butir += Convert.ToDecimal(Butir);
                                    tr.Cells.Add(td);

                                    decimal Carat = 0m;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number3(Carat) : Carat.ToString("N3");
                                    //td.Text = Str.Number3(Carat);
                                    carat += Carat;
                                    tr.Cells.Add(td);

                                    decimal PriceR = 0m;
                                    PriceR = u.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value;

                                    td = new TableCell();
                                    td.Text = Str.Number0(PriceR);
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(PriceR) : PriceR.ToString("N0");
                                    //harga += Convert.ToDecimal(PriceR);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(u.SubStorage != 99 ? u.StockProductGJ.StockProductGJ_Costing.TotalBiaya + u.StockProductGJ.StockProductGJ_CostingProduct.Total : 0) : (u.SubStorage != 99 ? u.StockProductGJ.StockProductGJ_Costing.TotalBiaya + u.StockProductGJ.StockProductGJ_CostingProduct.Total : 0).ToString("N2");
                                    //td.Text = Str.Number2(u.SubStorage != 99 ? u.StockProductGJ.StockProductGJ_Costing.TotalBiaya + u.StockProductGJ.StockProductGJ_CostingProduct.Total : 0);
                                    hargaM += Convert.ToDecimal(u.SubStorage != 99 ? u.StockProductGJ.StockProductGJ_Costing.TotalBiaya + u.StockProductGJ.StockProductGJ_CostingProduct.Total : 0);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(u.SubStorage != 99 ? u.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0) : (u.SubStorage != 99 ? u.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0).ToString("N2");
                                    //td.Text = Str.Number2(u.SubStorage != 99 ? u.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0);
                                    hargaR += Convert.ToDecimal(u.SubStorage != 99 ? u.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Operator;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.OperatorTgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Approval;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.ApprovalTgl);
                                    tr.Cells.Add(td);

                                    tbl_Table.Rows.Add(tr);
                                }
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number3(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(harga) : harga.ToString("N0");
                            //td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaR) : hargaR.ToString("N2");
                                //td31.Text = "<b>" + hargaR.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                }
                else if (Mode == "LD")
                {
                    if (Tipe.ToLower() == "rekap") // rekap
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockOutgoingLD select p);

                        /// WHERE
                        rs = rs.Where(p => p.IDBrand == ((vB >= 0) ? vB : p.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransOut == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            if (Action.ToLower() == "display") // display only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "View";
                                htd.Width = 50;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Print";
                                htd.Width = 50;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                TableRow tr = new TableRow();
                                TableCell td;

                                if (Action.ToLower() == "display") // display only
                                {
                                    td = new TableCell();
                                    td.Text = TD_HyperLink(item.ID, Mode);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = TD_Print(item.ID, Mode);
                                    tr.Cells.Add(td);
                                }

                                td = new TableCell();
                                td.Text = "<p>" + index++ + ".</p>";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = rsBrand[item.IDBrand];
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.Tgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.HtmlBreak(item.Keterangan);
                                tr.Cells.Add(td);

                                string NamaAsal = "";
                                if (item.TipeAsal == 0) { NamaAsal = rsWarehouse[item.IDAsal]; }
                                else if (item.TipeAsal == 1) { NamaAsal = rsOutlet[item.IDAsal]; }
                                else if (item.TipeAsal == 2) { NamaAsal = rsExhibition[item.IDAsal]; }

                                td = new TableCell();
                                td.Text = NamaAsal;
                                tr.Cells.Add(td);

                                string NamaTujuan = "";
                                if (item.TipeTujuan == 0) { NamaTujuan = rsWarehouse[item.IDTujuan]; }
                                else if (item.TipeTujuan == 1) { NamaTujuan = rsOutlet[item.IDTujuan]; }
                                else if (item.TipeTujuan == 2) { NamaTujuan = rsExhibition[item.IDTujuan]; }

                                td = new TableCell();
                                td.Text = NamaTujuan;
                                tr.Cells.Add(td);

                                int Qty = 0;
                                Qty = item.StockOutgoingLD_Product.Count();

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Qty) : Qty.ToString("N0");
                                //td.Text = Str.Number0(Qty);
                                qty += Qty;
                                tr.Cells.Add(td);

                                var rslt = db.StockIncomingLD_Product.Where(p => p.StockIncomingLD.StatusTransIn == 2 && p.StockIncomingLD.IDOutgoing == item.ID);

                                int QtyReceived = 0;
                                int QtyBroken = 0;
                                int QtyLost = 0;

                                if (rslt != null)
                                {
                                    QtyReceived = (from p in rslt
                                                   where p.StockIncomingLD.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                   select p).Count();
                                    QtyLost = (from p in rslt
                                               where p.StockIncomingLD.StatusTransIn == 2 && p.SubStorageResult == 99
                                               select p).Count();
                                    QtyBroken = (from p in rslt
                                                 where p.StockIncomingLD.StatusTransIn == 2 && p.SubStorageResult == 96
                                                 select p).Count();
                                }

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyReceived) : QtyReceived.ToString("N0");
                                // td.Text = Str.Number0(QtyReceived);
                                qtyReceived += QtyReceived;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyBroken) : QtyBroken.ToString("N0");
                                //td.Text = Str.Number0(QtyBroken);
                                qtyBroken += QtyBroken;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyLost) : QtyLost.ToString("N0");
                                //td.Text = Str.Number0(QtyLost);
                                qtyLost += QtyLost;
                                tr.Cells.Add(td);

                                decimal Netto = 0m;
                                //Netto = item.StockOutgoingPG_Product.Sum(p => p.StockProductPG.StockProductPG_CharProduct.NettoWeight);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                //td.Text = Str.Number2(Netto);
                                netto += Netto;
                                tr.Cells.Add(td);

                                decimal Gross = 0m;
                                //Gross = item.StockOutgoingPG_Product.Sum(p => p.StockProductPG.StockProductPG_CharProduct.GrossWeight);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Gross) : Gross.ToString("N2");
                                //td.Text = Str.Number2(Gross);
                                gross += Gross;
                                tr.Cells.Add(td);

                                decimal Butir = 0m;
                                Butir = item.StockOutgoingLD_Product.Sum(p => p.SubStorage != 99 ? p.StockProductLD_Stone1B.TotalButir : 0);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Butir) : Butir.ToString("N0");
                                //td.Text = Str.Number0(Butir);
                                butir += Convert.ToDecimal(Butir);
                                tr.Cells.Add(td);

                                decimal Carat = 0m;
                                Carat = item.StockOutgoingLD_Product.Sum(p => p.SubStorage != 99 ? p.StockProductLD_Stone1B.TotalCarat : 0);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Carat) : Carat.ToString("N3");
                                //td.Text = Str.Number3(Carat);
                                carat += Carat;
                                tr.Cells.Add(td);

                                decimal PriceR = 0m;
                                PriceR = Convert.ToDecimal(item.StockOutgoingLD_Product.Sum(p => p.SubStorage != 99 ? p.StockProductLD_Stone1B.HargaTotal : 0));

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(PriceR) : PriceR.ToString("N0");
                                //td.Text = Str.Number0(PriceR);
                                harga += Convert.ToDecimal(PriceR);
                                tr.Cells.Add(td);

                                if (Modul.ToLower() == "accounting") // accounting only
                                {
                                    Decimal HargaM = 0m;
                                    HargaM = Convert.ToDecimal(item.StockOutgoingLD_Product.Sum(p => p.SubStorage != 99 ? p.StockProductLD_Stone1B.HargaTotalM : 0));
                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(HargaM) : HargaM.ToString("N2");
                                    //td.Text = Str.Number2(HargaM);
                                    hargaM += HargaM;
                                    tr.Cells.Add(td);

                                }

                                td = new TableCell();
                                td.Text = item.Operator;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.OperatorTgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Approval;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.ApprovalTgl);
                                tr.Cells.Add(td);

                                tbl_Table.Rows.Add(tr);
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            if (Action.ToLower() == "display") // display only
                            {
                                td31 = new TableCell();
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(harga) : harga.ToString("N0");
                            //td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                    else if (Tipe.ToLower() == "detail") // detail
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockOutgoingLD select p);

                        /// WHERE
                        rs = rs.Where(p => p.IDBrand == ((vB >= 0) ? vB : p.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransOut == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "PLU";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                foreach (var u in item.StockOutgoingLD_Product)
                                {

                                    if (!response.IsClientConnected) break;

                                    TableRow tr = new TableRow();
                                    TableCell td;

                                    td = new TableCell();
                                    td.Text = "<p>" + index++ + ".</p>";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = rsBrand[u.StockOutgoingLD.IDBrand];
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.Tgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductLD_Stone1B.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.HtmlBreak(item.Keterangan);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.NamaAsal;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.NamaTujuan;
                                    tr.Cells.Add(td);

                                    int Qty = 0;
                                    Qty = 1;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(Qty) : Qty.ToString("N0");
                                    //td.Text = Str.Number0(Qty);
                                    qty += Qty;
                                    tr.Cells.Add(td);

                                    var rslt = db.StockIncomingLD_Product.Where(p => p.StockIncomingLD.StatusTransIn == 2 && p.StockIncomingLD.IDOutgoing == item.ID && p.IDProduct == u.IDProduct);

                                    int QtyReceived = 0;
                                    int QtyBroken = 0;
                                    int QtyLost = 0;

                                    if (rslt != null)
                                    {
                                        QtyReceived = (from p in rslt
                                                       where p.StockIncomingLD.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                       select p).Count();
                                        QtyLost = (from p in rslt
                                                   where p.StockIncomingLD.StatusTransIn == 2 && p.SubStorageResult == 99
                                                   select p).Count();
                                        QtyBroken = (from p in rslt
                                                     where p.StockIncomingLD.StatusTransIn == 2 && p.SubStorageResult == 96
                                                     select p).Count();
                                    }

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyReceived) : QtyReceived.ToString("N0");
                                    //td.Text = Str.Number0(QtyReceived);
                                    qtyReceived += QtyReceived;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyBroken) : QtyBroken.ToString("N0");
                                    //td.Text = Str.Number0(QtyBroken);
                                    qtyBroken += QtyBroken;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyLost) : QtyLost.ToString("N0");
                                    //td.Text = Str.Number0(QtyLost);
                                    qtyLost += QtyLost;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                                    //td.Text = Str.Number2(0); // nett
                                    netto += netto;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N0");
                                    //td.Text = Str.Number2(0); // gross
                                    gross += gross;
                                    tr.Cells.Add(td);

                                    decimal Butir = 0m;
                                    Butir = u.SubStorage != 99 ? u.StockProductLD_Stone1B.TotalButir : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(Butir) : Butir.ToString("N0");
                                    //td.Text = Str.Number0(Butir);
                                    butir += Convert.ToDecimal(Butir);
                                    tr.Cells.Add(td);

                                    decimal Carat = 0m;
                                    Carat = u.SubStorage != 99 ? u.StockProductLD_Stone1B.TotalCarat : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number3(Carat) : Carat.ToString("N3");
                                    //td.Text = Str.Number3(Carat);
                                    carat += Carat;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(u.SubStorage != 99 ? u.StockProductLD_Stone1B.HargaTotal.Value : 0) : (u.SubStorage != 99 ? u.StockProductLD_Stone1B.HargaTotal.Value : 0).ToString("N0");
                                    //td.Text = Str.Number2(u.SubStorage != 99 ? u.StockProductLD_Stone1B.HargaTotal.Value : 0);
                                    harga += u.StockProductLD_Stone1B.HargaTotal.Value;
                                    tr.Cells.Add(td);

                                    if (Modul.ToLower() == "accounting") // accounting only
                                    {
                                        Decimal HargaM = 0m;
                                        HargaM = Convert.ToDecimal(item.StockOutgoingLD_Product.Sum(p => u.SubStorage != 99 ? p.StockProductLD_Stone1B.HargaTotalM : 0));
                                        td = new TableCell();
                                        td.Text = (Action.ToLower() != "excel") ? Str.Number2(HargaM) : HargaM.ToString("N2");
                                        //td.Text = Str.Number2(HargaM);
                                        hargaM += HargaM;
                                        tr.Cells.Add(td);
                                    }

                                    td = new TableCell();
                                    td.Text = item.Operator;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.OperatorTgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Approval;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.ApprovalTgl);
                                    tr.Cells.Add(td);

                                    tbl_Table.Rows.Add(tr);
                                }
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number3(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(harga) : harga.ToString("N2");
                            //td31.Text = "<b>" + harga.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                }
            }
            else if (Sign == "incoming")
            {
                if (Mode == "DJ")
                {
                    if (Tipe.ToLower() == "rekap") // rekap
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockIncomingDJ select p);

                        /// WHERE
                        rs = rs.Where(p => p.StockOutgoingDJ.IDBrand == ((vB >= 0) ? vB : p.StockOutgoingDJ.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingDJ.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingDJ.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingDJ.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingDJ.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingDJ.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingDJ.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransIn == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingDJ.TipeTujuan == d.First().TipeLokasi && p.StockOutgoingDJ.IDTujuan == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingDJ.TipeTujuan == f.First().TipeLokasi && p.StockOutgoingDJ.IDTujuan == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingDJ.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.StockOutgoingDJ.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingDJ.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.StockOutgoingDJ.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            if (Action.ToLower() == "display") // display only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "View";
                                htd.Width = 50;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Print";
                                htd.Width = 50;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Terima";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "R";
                                htd.Width = 150;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                TableRow tr = new TableRow();
                                TableCell td;

                                if (Action.ToLower() == "display") // display only
                                {
                                    td = new TableCell();
                                    td.Text = TD_HyperLink(item.ID, Mode);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = TD_Print(item.ID, Mode);
                                    tr.Cells.Add(td);
                                }

                                td = new TableCell();
                                td.Text = "<p>" + index++ + ".</p>";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = rsBrand[item.StockOutgoingDJ.IDBrand];
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.Tgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.StockOutgoingDJ.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = DL.IncomingApprovalStatus(item.StatusTransIn);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.HtmlBreak(item.StockOutgoingDJ.Keterangan);
                                tr.Cells.Add(td);

                                string NamaAsal = "";
                                if (item.StockOutgoingDJ.TipeAsal == 0) { NamaAsal = rsWarehouse[item.StockOutgoingDJ.IDAsal]; }
                                else if (item.StockOutgoingDJ.TipeAsal == 1) { NamaAsal = rsOutlet[item.StockOutgoingDJ.IDAsal]; }
                                else if (item.StockOutgoingDJ.TipeAsal == 2) { NamaAsal = rsExhibition[item.StockOutgoingDJ.IDAsal]; }

                                td = new TableCell();
                                td.Text = NamaAsal;
                                tr.Cells.Add(td);

                                string NamaTujuan = "";
                                if (item.StockOutgoingDJ.TipeTujuan == 0) { NamaTujuan = rsWarehouse[item.StockOutgoingDJ.IDTujuan]; }
                                else if (item.StockOutgoingDJ.TipeTujuan == 1) { NamaTujuan = rsOutlet[item.StockOutgoingDJ.IDTujuan]; }
                                else if (item.StockOutgoingDJ.TipeTujuan == 2) { NamaTujuan = rsExhibition[item.StockOutgoingDJ.IDTujuan]; }

                                td = new TableCell();
                                td.Text = NamaTujuan;
                                tr.Cells.Add(td);

                                int Qty = 0;
                                Qty = item.StockIncomingDJ_Product.Count();

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Qty) : Qty.ToString("N0");
                                //td.Text = Str.Number0(Qty);
                                qty += Qty;
                                tr.Cells.Add(td);

                                var rslt = db.StockIncomingDJ_Product.Where(p => p.StockIncomingDJ.StatusTransIn == 2 && p.StockIncomingDJ.ID == item.ID);

                                int QtyReceived = 0;
                                int QtyBroken = 0;
                                int QtyLost = 0;

                                if (rslt != null)
                                {
                                    QtyReceived = (from p in rslt
                                                   where p.StockIncomingDJ.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                   select p).Count();
                                    QtyLost = (from p in rslt
                                               where p.StockIncomingDJ.StatusTransIn == 2 && p.SubStorageResult == 99
                                               select p).Count();
                                    QtyBroken = (from p in rslt
                                                 where p.StockIncomingDJ.StatusTransIn == 2 && p.SubStorageResult == 96
                                                 select p).Count();
                                }

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Qty) : Qty.ToString("N0");
                                //td.Text = Str.Number0(QtyReceived);
                                qtyReceived += QtyReceived;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyBroken) : QtyBroken.ToString("N0");
                                //td.Text = Str.Number0(QtyBroken);
                                qtyBroken += QtyBroken;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyLost) : QtyLost.ToString("N0");
                                //td.Text = Str.Number0(QtyLost);
                                qtyLost += QtyLost;
                                tr.Cells.Add(td);

                                decimal Netto = 0m;
                                Netto = item != null ? item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? (p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight != null ? p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight : 0) : 0) : 0;

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                //td.Text = Str.Number2(Netto);
                                netto += Netto;
                                tr.Cells.Add(td);

                                decimal Gross = 0m;
                                Gross = item != null ? item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? (p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight != null ? p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight : 0) : 0) : 0;

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Gross) : Gross.ToString("N2");
                                //td.Text = Str.Number2(Gross);
                                gross += Gross;
                                tr.Cells.Add(td);

                                decimal Butir = 0m;
                                Butir = item != null ? item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? (p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value != null ? p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value : 0) : 0) : 0;

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Butir) : Butir.ToString("N0");
                                //td.Text = Str.Number0(Butir);
                                butir += Convert.ToDecimal(Butir);
                                tr.Cells.Add(td);

                                decimal Carat = 0m;
                                Carat = item != null ? item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? (p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat != null ? p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat : 0) : 0) : 0;

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number3(Carat) : Carat.ToString("N3");
                                //td.Text = Str.Number3(Carat);
                                carat += Carat;
                                tr.Cells.Add(td);

                                decimal PriceR = 0m;
                                PriceR = item != null ? item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? (p.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value != null ? p.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value : 0) : 0) : 0;

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(PriceR) : PriceR.ToString("N0");
                                //td.Text = Str.Number0(PriceR);
                                harga += Convert.ToDecimal(PriceR);
                                tr.Cells.Add(td);

                                if (Modul.ToLower() == "accounting") // accounting only
                                {
                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductDJ.StockProductDJ_Costing.TotalBiaya + p.StockProductDJ.StockProductDJ_CostingProduct.Total : 0)) : (item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductDJ.StockProductDJ_Costing.TotalBiaya + p.StockProductDJ.StockProductDJ_CostingProduct.Total : 0)).ToString("N2");
                                    //td.Text = Str.Number2(item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductDJ.StockProductDJ_Costing.TotalBiaya + p.StockProductDJ.StockProductDJ_CostingProduct.Total : 0));
                                    hargaM += Convert.ToDecimal(item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductDJ.StockProductDJ_Costing.TotalBiaya + p.StockProductDJ.StockProductDJ_CostingProduct.Total : 0));
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0)) : (item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0)).ToString("N2");
                                    //td.Text = Str.Number2(item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0));
                                    hargaR += Convert.ToDecimal(item.StockIncomingDJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0));
                                    tr.Cells.Add(td);
                                }

                                td = new TableCell();
                                td.Text = item.Operator;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.OperatorTgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Approval;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.ApprovalTgl);
                                tr.Cells.Add(td);

                                tbl_Table.Rows.Add(tr);
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            if (Action.ToLower() == "display") // display only
                            {
                                td31 = new TableCell();
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number3(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(harga) : harga.ToString("N0");
                            //td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaR) : hargaR.ToString("N2");
                                //td31.Text = "<b>" + hargaR.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                    else if (Tipe.ToLower() == "detail") // detail
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockIncomingDJ select p);

                        /// WHERE
                        rs = rs.Where(p => p.StockOutgoingDJ.IDBrand == ((vB >= 0) ? vB : p.StockOutgoingDJ.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingDJ.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingDJ.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingDJ.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingDJ.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingDJ.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingDJ.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransIn == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingDJ.TipeTujuan == d.First().TipeLokasi && p.StockOutgoingDJ.IDTujuan == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingDJ.TipeTujuan == f.First().TipeLokasi && p.StockOutgoingDJ.IDTujuan == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingDJ.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.StockOutgoingDJ.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingDJ.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.StockOutgoingDJ.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Terima";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "PLU";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Kode Karet";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Item";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Category";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "R";
                                htd.Width = 150;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                foreach (var u in item.StockIncomingDJ_Product)
                                {
                                    if (!response.IsClientConnected) break;

                                    TableRow tr = new TableRow();
                                    TableCell td;

                                    td = new TableCell();
                                    td.Text = "<p>" + index++ + ".</p>";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = rsBrand[u.StockIncomingDJ.StockOutgoingDJ.IDBrand];
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.Tgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.StockOutgoingDJ.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = DL.IncomingApprovalStatus(item.StatusTransIn);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductDJ.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductDJ.ProtoNomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.HtmlBreak(item.StockOutgoingDJ.Keterangan);
                                    tr.Cells.Add(td);

                                    string NamaAsal = "";
                                    if (item.StockOutgoingDJ.TipeAsal == 0) { NamaAsal = rsWarehouse[item.StockOutgoingDJ.IDAsal]; }
                                    else if (item.StockOutgoingDJ.TipeAsal == 1) { NamaAsal = rsOutlet[item.StockOutgoingDJ.IDAsal]; }
                                    else if (item.StockOutgoingDJ.TipeAsal == 2) { NamaAsal = rsExhibition[item.StockOutgoingDJ.IDAsal]; }

                                    td = new TableCell();
                                    td.Text = NamaAsal;
                                    tr.Cells.Add(td);

                                    string NamaTujuan = "";
                                    if (item.StockOutgoingDJ.TipeTujuan == 0) { NamaTujuan = rsWarehouse[item.StockOutgoingDJ.IDTujuan]; }
                                    else if (item.StockOutgoingDJ.TipeTujuan == 1) { NamaTujuan = rsOutlet[item.StockOutgoingDJ.IDTujuan]; }
                                    else if (item.StockOutgoingDJ.TipeTujuan == 2) { NamaTujuan = rsExhibition[item.StockOutgoingDJ.IDTujuan]; }

                                    td = new TableCell();
                                    td.Text = NamaTujuan;
                                    tr.Cells.Add(td);

                                    int Qty = 0;
                                    Qty = 1;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(Qty) : Qty.ToString("N0");
                                    //td.Text = Str.Number0(Qty);
                                    qty += Qty;
                                    tr.Cells.Add(td);

                                    var rslt = db.StockIncomingDJ_Product.Where(p => p.StockIncomingDJ.StatusTransIn == 2 && p.StockIncomingDJ.ID == item.ID && p.IDProduct == u.IDProduct);

                                    int QtyReceived = 0;
                                    int QtyBroken = 0;
                                    int QtyLost = 0;

                                    if (rslt != null)
                                    {
                                        QtyReceived = (from p in rslt
                                                       where p.StockIncomingDJ.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                       select p).Count();
                                        QtyLost = (from p in rslt
                                                   where p.StockIncomingDJ.StatusTransIn == 2 && p.SubStorageResult == 99
                                                   select p).Count();
                                        QtyBroken = (from p in rslt
                                                     where p.StockIncomingDJ.StatusTransIn == 2 && p.SubStorageResult == 96
                                                     select p).Count();
                                    }

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyReceived) : QtyReceived.ToString("N0");
                                    //td.Text = Str.Number0(QtyReceived);
                                    qtyReceived += QtyReceived;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyBroken) : QtyBroken.ToString("N0");
                                    //td.Text = Str.Number0(QtyBroken);
                                    qtyBroken += QtyBroken;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyLost) : QtyLost.ToString("N0");
                                    //td.Text = Str.Number0(QtyLost);
                                    qtyLost += QtyLost;
                                    tr.Cells.Add(td);

                                    decimal Netto = 0m;
                                    Netto = u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_CharProduct.NettoWeight : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                    //td.Text = Str.Number2(Netto);
                                    netto += Netto;
                                    tr.Cells.Add(td);

                                    decimal Gross = 0m;
                                    Gross = u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_CharProduct.GrossWeight : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Qty) : Qty.ToString("N2");
                                    //td.Text = Str.Number2(Gross);
                                    gross += Gross;
                                    tr.Cells.Add(td);

                                    decimal Butir = 0m;
                                    Butir = u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(Butir) : Butir.ToString("N0");
                                    //td.Text = Str.Number0(Butir);
                                    butir += Convert.ToDecimal(Butir);
                                    tr.Cells.Add(td);

                                    decimal Carat = 0m;
                                    Carat = u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_CharProduct.StoneCarat : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number3(Carat) : Carat.ToString("N3");
                                    //td.Text = Str.Number3(Carat);
                                    carat += Carat;
                                    tr.Cells.Add(td);

                                    decimal PriceR = 0m;
                                    PriceR = u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(PriceR) : PriceR.ToString("N0");
                                    //td.Text = Str.Number0(PriceR);
                                    harga += Convert.ToDecimal(PriceR);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_Costing.TotalBiaya + u.StockProductDJ.StockProductDJ_CostingProduct.Total : 0) : (u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_Costing.TotalBiaya + u.StockProductDJ.StockProductDJ_CostingProduct.Total : 0).ToString("N2");
                                    //td.Text = Str.Number2(u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_Costing.TotalBiaya + u.StockProductDJ.StockProductDJ_CostingProduct.Total : 0);
                                    hargaM += Convert.ToDecimal(u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_Costing.TotalBiaya + u.StockProductDJ.StockProductDJ_CostingProduct.Total : 0);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0) : (u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0).ToString("N2");
                                    //td.Text = Str.Number2(u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0);
                                    hargaR += Convert.ToDecimal(u.SubStorageResult != 99 ? u.StockProductDJ.StockProductDJ_PricingMU.ModalUSD : 0);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Operator;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.OperatorTgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Approval;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.ApprovalTgl);
                                    tr.Cells.Add(td);

                                    tbl_Table.Rows.Add(tr);
                                }
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number3(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(harga) : harga.ToString("N0");
                            //td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaR) : hargaR.ToString("N2");
                                //td31.Text = "<b>" + hargaR.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                }
                else if (Mode == "PG")
                {
                    if (Tipe.ToLower() == "rekap") // rekap
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockIncomingPG select p);

                        /// WHERE
                        rs = rs.Where(p => p.StockOutgoingPG.IDBrand == ((vB >= 0) ? vB : p.StockOutgoingPG.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingPG.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingPG.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingPG.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingPG.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingPG.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingPG.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransIn == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingPG.TipeTujuan == d.First().TipeLokasi && p.StockOutgoingPG.IDTujuan == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingPG.TipeTujuan == f.First().TipeLokasi && p.StockOutgoingPG.IDTujuan == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingPG.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.StockOutgoingPG.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingPG.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.StockOutgoingPG.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            if (Action.ToLower() == "display") // display only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "View";
                                htd.Width = 50;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Print";
                                htd.Width = 50;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Terima";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                TableRow tr = new TableRow();
                                TableCell td;

                                if (Action.ToLower() == "display") // display only
                                {
                                    td = new TableCell();
                                    td.Text = TD_HyperLink(item.ID, Mode);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = TD_Print(item.ID, Mode);
                                    tr.Cells.Add(td);
                                }

                                td = new TableCell();
                                td.Text = "<p>" + index++ + ".</p>";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = rsBrand[item.StockOutgoingPG.IDBrand];
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.Tgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.StockOutgoingPG.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = DL.IncomingApprovalStatus(item.StatusTransIn);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.HtmlBreak(item.StockOutgoingPG.Keterangan);
                                tr.Cells.Add(td);

                                string NamaAsal = "";
                                if (item.StockOutgoingPG.TipeAsal == 0) { NamaAsal = rsWarehouse[item.StockOutgoingPG.IDAsal]; }
                                else if (item.StockOutgoingPG.TipeAsal == 1) { NamaAsal = rsOutlet[item.StockOutgoingPG.IDAsal]; }
                                else if (item.StockOutgoingPG.TipeAsal == 2) { NamaAsal = rsExhibition[item.StockOutgoingPG.IDAsal]; }

                                td = new TableCell();
                                td.Text = NamaAsal;
                                tr.Cells.Add(td);

                                string NamaTujuan = "";
                                if (item.StockOutgoingPG.TipeTujuan == 0) { NamaTujuan = rsWarehouse[item.StockOutgoingPG.IDTujuan]; }
                                else if (item.StockOutgoingPG.TipeTujuan == 1) { NamaTujuan = rsOutlet[item.StockOutgoingPG.IDTujuan]; }
                                else if (item.StockOutgoingPG.TipeTujuan == 2) { NamaTujuan = rsExhibition[item.StockOutgoingPG.IDTujuan]; }

                                td = new TableCell();
                                td.Text = NamaTujuan;
                                tr.Cells.Add(td);

                                int Qty = 0;
                                Qty = item.StockIncomingPG_Product.Count();

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Qty) : Qty.ToString("N0");
                                //td.Text = Str.Number0(Qty);
                                qty += Qty;
                                tr.Cells.Add(td);

                                var rslt = db.StockIncomingPG_Product.Where(p => p.StockIncomingPG.StatusTransIn == 2 && p.StockIncomingPG.ID == item.ID);

                                int QtyReceived = 0;
                                int QtyBroken = 0;
                                int QtyLost = 0;

                                if (rslt != null)
                                {
                                    QtyReceived = (from p in rslt
                                                   where p.StockIncomingPG.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                   select p).Count();
                                    QtyLost = (from p in rslt
                                               where p.StockIncomingPG.StatusTransIn == 2 && p.SubStorageResult == 99
                                               select p).Count();
                                    QtyBroken = (from p in rslt
                                                 where p.StockIncomingPG.StatusTransIn == 2 && p.SubStorageResult == 96
                                                 select p).Count();
                                }

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyReceived) : QtyReceived.ToString("N0");
                                //td.Text = Str.Number0(QtyReceived);
                                qtyReceived += QtyReceived;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyBroken) : QtyBroken.ToString("N0");
                                //td.Text = Str.Number0(QtyBroken);
                                qtyBroken += QtyBroken;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyLost) : QtyLost.ToString("N0");
                                //td.Text = Str.Number0(QtyLost);
                                qtyLost += QtyLost;
                                tr.Cells.Add(td);

                                decimal Netto = 0m;
                                Netto = item.StockIncomingPG_Product.Sum(p => p.StockProductPG.NettoWeight.Value);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                //td.Text = Str.Number2(Netto);
                                netto += Netto;
                                tr.Cells.Add(td);

                                decimal Gross = 0m;
                                Gross = item.StockIncomingPG_Product.Sum(p => p.StockProductPG.GrossWeight.Value);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Gross) : Gross.ToString("N2");
                                //td.Text = Str.Number2(Gross);
                                gross += Gross;
                                tr.Cells.Add(td);

                                decimal Butir = 0m;
                                //Butir = item.StockIncomingPG_Product.Sum(p => p.StockProductPG.StockProductDJ_CharProduct.StoneQty.Value);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Butir) : Butir.ToString("N0");
                                //td.Text = Str.Number0(Butir);
                                butir += Convert.ToDecimal(Butir);
                                tr.Cells.Add(td);

                                decimal Carat = 0m;
                                //Carat = item.StockIncomingPG_Product.Sum(p => p.StockProductPG.StockProductPG_CharProduct.StoneCarat);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number3(Carat) : Carat.ToString("N3");
                                //td.Text = Str.Number3(Carat);
                                carat += Carat;
                                tr.Cells.Add(td);

                                decimal PriceR = 0m;
                                PriceR = item.StockIncomingPG_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductPG.TotalHargaJual.Value : 0);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(PriceR) : PriceR.ToString("N0");
                                //td.Text = Str.Number0(PriceR);
                                harga += Convert.ToDecimal(PriceR);
                                tr.Cells.Add(td);

                                if (Modul.ToLower() == "accounting") // accounting only
                                {
                                    Decimal HargaM = 0m;
                                    HargaM = Convert.ToDecimal(item.StockIncomingPG_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductPG.TotalHarga24 / p.StockProductPG.TotalRate : 0));
                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(HargaM) : HargaM.ToString("N2");
                                    //td.Text = Str.Number2(HargaM);
                                    hargaM += HargaM;
                                    tr.Cells.Add(td);

                                }

                                td = new TableCell();
                                td.Text = item.Operator;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.OperatorTgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Approval;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.ApprovalTgl);
                                tr.Cells.Add(td);

                                tbl_Table.Rows.Add(tr);
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            if (Action.ToLower() == "display") // display only
                            {
                                td31 = new TableCell();
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number3(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(harga) : harga.ToString("N0");
                            //td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                    else if (Tipe.ToLower() == "detail") // detail
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockIncomingPG select p);

                        /// WHERE
                        rs = rs.Where(p => p.StockOutgoingPG.IDBrand == ((vB >= 0) ? vB : p.StockOutgoingPG.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingPG.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingPG.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingPG.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingPG.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingPG.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingPG.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransIn == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingPG.TipeTujuan == d.First().TipeLokasi && p.StockOutgoingPG.IDTujuan == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingPG.TipeTujuan == f.First().TipeLokasi && p.StockOutgoingPG.IDTujuan == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingPG.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.StockOutgoingPG.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingPG.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.StockOutgoingPG.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Terima";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "PLU";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                foreach (var u in item.StockIncomingPG_Product)
                                {
                                    if (!response.IsClientConnected) break;

                                    TableRow tr = new TableRow();
                                    TableCell td;

                                    td = new TableCell();
                                    td.Text = "<p>" + index++ + ".</p>";
                                    td.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = rsBrand[u.StockIncomingPG.StockOutgoingPG.IDBrand];
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.Tgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.StockOutgoingPG.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = DL.IncomingApprovalStatus(item.StatusTransIn);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductPG.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.HtmlBreak(item.StockOutgoingPG.Keterangan);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.StockOutgoingPG.NamaAsal;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.StockOutgoingPG.NamaTujuan;
                                    tr.Cells.Add(td);

                                    int Qty = 1;

                                    var rslt = db.StockIncomingPG_Product.Where(p => p.StockIncomingPG.StatusTransIn == 2 && p.StockIncomingPG.ID == item.ID && p.IDProduct == u.IDProduct);

                                    int QtyReceived = 0;
                                    int QtyBroken = 0;
                                    int QtyLost = 0;

                                    if (rslt != null)
                                    {
                                        QtyReceived = (from p in rslt
                                                       where p.StockIncomingPG.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                       select p).Count();
                                        QtyLost = (from p in rslt
                                                   where p.StockIncomingPG.StatusTransIn == 2 && p.SubStorageResult == 99
                                                   select p).Count();
                                        QtyBroken = (from p in rslt
                                                     where p.StockIncomingPG.StatusTransIn == 2 && p.SubStorageResult == 96
                                                     select p).Count();
                                    }

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(Qty) : Qty.ToString("N0");
                                    //td.Text = Str.Number0(Qty);
                                    qty += Qty;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyReceived) : QtyReceived.ToString("N0");
                                    //td.Text = Str.Number0(QtyReceived);
                                    qtyReceived += QtyReceived;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyBroken) : QtyBroken.ToString("N0");
                                    //td.Text = Str.Number0(QtyBroken);
                                    qtyBroken += QtyBroken;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyLost) : QtyLost.ToString("N0");
                                    //td.Text = Str.Number0(QtyLost);
                                    qtyLost += QtyLost;
                                    tr.Cells.Add(td);

                                    decimal Netto = 0m;
                                    Netto = u.StockProductPG.NettoWeight.Value;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                    //td.Text = Str.Number2(Netto);
                                    netto += Netto;
                                    td.HorizontalAlign = HorizontalAlign.Right;
                                    tr.Cells.Add(td);

                                    decimal Gross = 0m;
                                    Gross = u.StockProductPG.GrossWeight.Value;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Gross) : Gross.ToString("N2");
                                    //td.Text = Str.Number2(Gross);
                                    gross += Gross;
                                    td.HorizontalAlign = HorizontalAlign.Right;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Number0(0);
                                    td.HorizontalAlign = HorizontalAlign.Right;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Number3(0);
                                    td.HorizontalAlign = HorizontalAlign.Right;
                                    tr.Cells.Add(td);

                                    decimal PriceR = 0m;
                                    PriceR = u.SubStorageResult != 99 ? u.StockProductPG.TotalHargaJual.Value : 0;

    
    
                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(PriceR) : PriceR.ToString("N0");
                                    //td.Text = Str.Number0(PriceR);
                                    harga += PriceR;
                                    td.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(td);

                                    decimal PriceM = 0m;
                                    PriceM = u.SubStorageResult != 99 ? u.StockProductPG.TotalHarga24.Value / u.StockProductPG.TotalRate.Value : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(PriceM) : PriceM.ToString("N2");
                                    //td.Text = Str.Number2(PriceM);
                                    hargaM += PriceM;
                                    td.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Operator;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.OperatorTgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Approval;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.ApprovalTgl);
                                    tr.Cells.Add(td);

                                    tbl_Table.Rows.Add(tr);
                                }
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number3(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(harga) : harga.ToString("N0");
                            //td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                }
                else if (Mode == "GJ")
                {
                    if (Tipe.ToLower() == "rekap") // rekap
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockIncomingGJ select p);

                        /// WHERE
                        rs = rs.Where(p => p.StockOutgoingGJ.IDBrand == ((vB >= 0) ? vB : p.StockOutgoingGJ.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingGJ.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingGJ.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingGJ.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingGJ.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingGJ.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingGJ.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransIn == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingGJ.TipeTujuan == d.First().TipeLokasi && p.StockOutgoingGJ.IDTujuan == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingGJ.TipeTujuan == f.First().TipeLokasi && p.StockOutgoingGJ.IDTujuan == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingGJ.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.StockOutgoingGJ.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingGJ.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.StockOutgoingGJ.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            if (Action.ToLower() == "display") // display only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "View";
                                htd.Width = 50;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Print";
                                htd.Width = 50;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Terima";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "R";
                                htd.Width = 150;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                TableRow tr = new TableRow();
                                TableCell td;

                                if (Action.ToLower() == "display") // display only
                                {
                                    td = new TableCell();
                                    td.Text = TD_HyperLink(item.ID, Mode);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = TD_Print(item.ID, Mode);
                                    tr.Cells.Add(td);
                                }

                                td = new TableCell();
                                td.Text = "<p>" + index++ + ".</p>";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = rsBrand[item.StockOutgoingGJ.IDBrand];
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.Tgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.StockOutgoingGJ.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = DL.IncomingApprovalStatus(item.StatusTransIn);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.HtmlBreak(item.StockOutgoingGJ.Keterangan);
                                tr.Cells.Add(td);

                                string NamaAsal = "";
                                if (item.StockOutgoingGJ.TipeAsal == 0) { NamaAsal = rsWarehouse[item.StockOutgoingGJ.IDAsal]; }
                                else if (item.StockOutgoingGJ.TipeAsal == 1) { NamaAsal = rsOutlet[item.StockOutgoingGJ.IDAsal]; }
                                else if (item.StockOutgoingGJ.TipeAsal == 2) { NamaAsal = rsExhibition[item.StockOutgoingGJ.IDAsal]; }

                                td = new TableCell();
                                td.Text = NamaAsal;
                                tr.Cells.Add(td);

                                string NamaTujuan = "";
                                if (item.StockOutgoingGJ.TipeTujuan == 0) { NamaTujuan = rsWarehouse[item.StockOutgoingGJ.IDTujuan]; }
                                else if (item.StockOutgoingGJ.TipeTujuan == 1) { NamaTujuan = rsOutlet[item.StockOutgoingGJ.IDTujuan]; }
                                else if (item.StockOutgoingGJ.TipeTujuan == 2) { NamaTujuan = rsExhibition[item.StockOutgoingGJ.IDTujuan]; }

                                td = new TableCell();
                                td.Text = NamaTujuan;
                                tr.Cells.Add(td);

                                int Qty = 0;
                                Qty = item.StockIncomingGJ_Product.Count();

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Qty) : Qty.ToString("N0");
                                //td.Text = Str.Number0(Qty);
                                qty += Qty;
                                tr.Cells.Add(td);

                                var rslt = db.StockIncomingGJ_Product.Where(p => p.StockIncomingGJ.StatusTransIn == 2 && p.StockIncomingGJ.ID == item.ID);

                                int QtyReceived = 0;
                                int QtyBroken = 0;
                                int QtyLost = 0;

                                if (rslt != null)
                                {
                                    QtyReceived = (from p in rslt
                                                   where p.StockIncomingGJ.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                   select p).Count();
                                    QtyLost = (from p in rslt
                                               where p.StockIncomingGJ.StatusTransIn == 2 && p.SubStorageResult == 99
                                               select p).Count();
                                    QtyBroken = (from p in rslt
                                                 where p.StockIncomingGJ.StatusTransIn == 2 && p.SubStorageResult == 96
                                                 select p).Count();
                                }

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyReceived) : QtyReceived.ToString("N0");
                                //td.Text = Str.Number0(QtyReceived);
                                qtyReceived += QtyReceived;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(gross) : gross.ToString("N0");
                                //td.Text = Str.Number0(QtyBroken);
                                qtyBroken += QtyBroken;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyLost) : QtyLost.ToString("N0");
                                //td.Text = Str.Number0(QtyLost);
                                qtyLost += QtyLost;
                                tr.Cells.Add(td);

                                decimal Netto = 0m;
                                Netto = item.StockIncomingGJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight : 0);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                //td.Text = Str.Number2(Netto);
                                netto += Netto;
                                tr.Cells.Add(td);

                                decimal Gross = 0m;
                                Gross = item.StockIncomingGJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductGJ.StockProductGJ_CharProduct.GrossWeight : 0);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Gross) : Gross.ToString("N2");
                                //td.Text = Str.Number2(Gross);
                                gross += Gross;
                                tr.Cells.Add(td);

                                decimal Butir = 0m;
                                //Butir = item.StockIncomingGJ_Product.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneQty.Value);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Butir) : Butir.ToString("N0");
                                //td.Text = Str.Number0(Butir);
                                butir += Convert.ToDecimal(Butir);
                                tr.Cells.Add(td);

                                decimal Carat = 0m;
                                //Carat = item.StockIncomingGJ_Product.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.StoneCarat);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number3(Carat) : Carat.ToString("N3");
                                //td.Text = Str.Number3(Carat);
                                carat += Carat;
                                tr.Cells.Add(td);

                                decimal PriceR = 0m;
                                PriceR = item.StockIncomingGJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value : 0);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(PriceR) : PriceR.ToString("N0");
                                //td.Text = Str.Number0(PriceR);
                                harga += Convert.ToDecimal(PriceR);
                                tr.Cells.Add(td);

                                if (Modul.ToLower() == "accounting") // accounting only
                                {
                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(item.StockIncomingGJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total : 0)) : (item.StockIncomingGJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total : 0)).ToString("N2");
                                    //td.Text = Str.Number2(item.StockIncomingGJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total : 0));
                                    hargaM += Convert.ToDecimal(item.StockIncomingGJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total : 0));
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(item.StockIncomingGJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0)) : (item.StockIncomingGJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0)).ToString("N2");
                                    //td.Text = Str.Number2(item.StockIncomingGJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0));
                                    hargaR += Convert.ToDecimal(item.StockIncomingGJ_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0));
                                    tr.Cells.Add(td);
                                }

                                td = new TableCell();
                                td.Text = item.Operator;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.OperatorTgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Approval;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.ApprovalTgl);
                                tr.Cells.Add(td);

                                tbl_Table.Rows.Add(tr);
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            if (Action.ToLower() == "display") // display only
                            {
                                td31 = new TableCell();
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number3(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(harga) : harga.ToString("N0");
                            //td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaR) : hargaR.ToString("N2");
                                //td31.Text = "<b>" + hargaR.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                    else if (Tipe.ToLower() == "detail") // detail
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockIncomingGJ select p);

                        /// WHERE
                        rs = rs.Where(p => p.StockOutgoingGJ.IDBrand == ((vB >= 0) ? vB : p.StockOutgoingGJ.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingGJ.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingGJ.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingGJ.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingGJ.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingGJ.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingGJ.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransIn == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingGJ.TipeTujuan == d.First().TipeLokasi && p.StockOutgoingGJ.IDTujuan == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingGJ.TipeTujuan == f.First().TipeLokasi && p.StockOutgoingGJ.IDTujuan == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingGJ.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.StockOutgoingGJ.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingGJ.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.StockOutgoingGJ.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Terima";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "PLU";
                            htd.Width = 120;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Kode Karet";
                            htd.Width = 120;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "R";
                                htd.Width = 150;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                foreach (var u in item.StockIncomingGJ_Product)
                                {
                                    if (!response.IsClientConnected) break;

                                    TableRow tr = new TableRow();
                                    TableCell td;

                                    td = new TableCell();
                                    td.Text = "<p>" + index++ + ".</p>";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = rsBrand[u.StockIncomingGJ.StockOutgoingGJ.IDBrand];
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.Tgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.StockOutgoingGJ.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = DL.IncomingApprovalStatus(item.StatusTransIn);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductGJ.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductGJ.ProtoNomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.HtmlBreak(item.StockOutgoingGJ.Keterangan);
                                    tr.Cells.Add(td);

                                    string NamaAsal = "";
                                    if (item.StockOutgoingGJ.TipeAsal == 0) { NamaAsal = rsWarehouse[item.StockOutgoingGJ.IDAsal]; }
                                    else if (item.StockOutgoingGJ.TipeAsal == 1) { NamaAsal = rsOutlet[item.StockOutgoingGJ.IDAsal]; }
                                    else if (item.StockOutgoingGJ.TipeAsal == 2) { NamaAsal = rsExhibition[item.StockOutgoingGJ.IDAsal]; }

                                    td = new TableCell();
                                    td.Text = NamaAsal;
                                    tr.Cells.Add(td);

                                    string NamaTujuan = "";
                                    if (item.StockOutgoingGJ.TipeTujuan == 0) { NamaTujuan = rsWarehouse[item.StockOutgoingGJ.IDTujuan]; }
                                    else if (item.StockOutgoingGJ.TipeTujuan == 1) { NamaTujuan = rsOutlet[item.StockOutgoingGJ.IDTujuan]; }
                                    else if (item.StockOutgoingGJ.TipeTujuan == 2) { NamaTujuan = rsExhibition[item.StockOutgoingGJ.IDTujuan]; }

                                    td = new TableCell();
                                    td.Text = NamaTujuan;
                                    tr.Cells.Add(td);

                                    int Qty = 1;

                                    var rslt = db.StockIncomingGJ_Product.Where(p => p.StockIncomingGJ.StatusTransIn == 2 && p.StockIncomingGJ.ID == item.ID && p.IDProduct == u.IDProduct);

                                    int QtyReceived = 0;
                                    int QtyBroken = 0;
                                    int QtyLost = 0;

                                    if (rslt != null)
                                    {
                                        QtyReceived = (from p in rslt
                                                       where p.StockIncomingGJ.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                       select p).Count();
                                        QtyLost = (from p in rslt
                                                   where p.StockIncomingGJ.StatusTransIn == 2 && p.SubStorageResult == 99
                                                   select p).Count();
                                        QtyBroken = (from p in rslt
                                                     where p.StockIncomingGJ.StatusTransIn == 2 && p.SubStorageResult == 96
                                                     select p).Count();
                                    }

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(Qty) : Qty.ToString("N0");
                                    //td.Text = Str.Number0(Qty);
                                    qty += Qty;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyReceived) : QtyReceived.ToString("N0");
                                    //td.Text = Str.Number0(QtyReceived);
                                    qtyReceived += QtyReceived;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyBroken) : QtyBroken.ToString("N0");
                                    //td.Text = Str.Number0(QtyBroken);
                                    qtyBroken += QtyBroken;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyLost) : QtyLost.ToString("N0");
                                    //td.Text = Str.Number0(QtyLost);
                                    qtyLost += QtyLost;
                                    tr.Cells.Add(td);

                                    decimal Netto = 0m;
                                    Netto = u.SubStorageResult != 99 ? u.StockProductGJ.StockProductGJ_CharProduct.NettoWeight : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                    //td.Text = Str.Number2(Netto);
                                    netto += Netto;
                                    tr.Cells.Add(td);

                                    decimal Gross = 0m;
                                    Gross = u.SubStorageResult != 99 ? u.StockProductGJ.StockProductGJ_CharProduct.GrossWeight : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(Gross) : Gross.ToString("N2");
                                    //td.Text = Str.Number2(Gross);
                                    gross += Gross;
                                    tr.Cells.Add(td);

                                    decimal Butir = 0m;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(Butir) : Butir.ToString("N0");
                                    //td.Text = Str.Number0(Butir);
                                    butir += Convert.ToDecimal(Butir);
                                    tr.Cells.Add(td);

                                    decimal Carat = 0m;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number3(Carat) : Carat.ToString("N3");
                                    //td.Text = Str.Number3(Carat);
                                    carat += Carat;
                                    tr.Cells.Add(td);

                                    decimal PriceR = 0m;
                                    PriceR = u.SubStorageResult != 99 ? u.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(PriceR) : PriceR.ToString("N0");
                                    //td.Text = Str.Number0(PriceR);
                                    harga += Convert.ToDecimal(PriceR);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(u.SubStorageResult != 99 ? u.StockProductGJ.StockProductGJ_Costing.TotalBiaya + u.StockProductGJ.StockProductGJ_CostingProduct.Total : 0) : (u.SubStorageResult != 99 ? u.StockProductGJ.StockProductGJ_Costing.TotalBiaya + u.StockProductGJ.StockProductGJ_CostingProduct.Total : 0).ToString("N2");
                                    //td.Text = Str.Number2(u.SubStorageResult != 99 ? u.StockProductGJ.StockProductGJ_Costing.TotalBiaya + u.StockProductGJ.StockProductGJ_CostingProduct.Total : 0);
                                    hargaM += Convert.ToDecimal(u.SubStorageResult != 99 ? u.StockProductGJ.StockProductGJ_Costing.TotalBiaya + u.StockProductGJ.StockProductGJ_CostingProduct.Total : 0);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(u.SubStorageResult != 99 ? u.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0) : (u.SubStorageResult != 99 ? u.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0).ToString("N2");
                                    //td.Text = Str.Number2(u.SubStorageResult != 99 ? u.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0);
                                    hargaR += Convert.ToDecimal(u.SubStorageResult != 99 ? u.StockProductGJ.StockProductGJ_PricingMU.ModalUSD : 0);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Operator;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.OperatorTgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Approval;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.ApprovalTgl);
                                    tr.Cells.Add(td);

                                    tbl_Table.Rows.Add(tr);
                                }
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number3(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(harga) : harga.ToString("N0");
                            //td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaR) : hargaR.ToString("N0");
                                //td31.Text = "<b>" + hargaR.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                }
                else if (Mode == "LD")
                {
                    if (Tipe.ToLower() == "rekap") // rekap
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockIncomingLD select p);

                        /// WHERE
                        rs = rs.Where(p => p.StockOutgoingLD.IDBrand == ((vB >= 0) ? vB : p.StockOutgoingLD.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingLD.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingLD.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingLD.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingLD.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingLD.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingLD.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransIn == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingLD.TipeTujuan == d.First().TipeLokasi && p.StockOutgoingLD.IDTujuan == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingLD.TipeTujuan == f.First().TipeLokasi && p.StockOutgoingLD.IDTujuan == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingLD.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.StockOutgoingLD.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingLD.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.StockOutgoingLD.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            if (Action.ToLower() == "display") // display only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "View";
                                htd.Width = 50;
                                htr.Cells.Add(htd);

                                htd = new TableHeaderCell();
                                htd.Text = "Print";
                                htd.Width = 50;
                                htr.Cells.Add(htd);
                            }

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Terima";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                TableRow tr = new TableRow();
                                TableCell td;

                                if (Action.ToLower() == "display") // display only
                                {
                                    td = new TableCell();
                                    td.Text = TD_HyperLink(item.ID, Mode);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = TD_Print(item.ID, Mode);
                                    tr.Cells.Add(td);
                                }

                                td = new TableCell();
                                td.Text = "<p>" + index++ + ".</p>";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = rsBrand[item.StockOutgoingLD.IDBrand];
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.Tgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.StockOutgoingLD.Nomor;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = DL.IncomingApprovalStatus(item.StatusTransIn);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.HtmlBreak(item.StockOutgoingLD.Keterangan);
                                tr.Cells.Add(td);

                                string NamaAsal = "";
                                if (item.StockOutgoingLD.TipeAsal == 0) { NamaAsal = rsWarehouse[item.StockOutgoingLD.IDAsal]; }
                                else if (item.StockOutgoingLD.TipeAsal == 1) { NamaAsal = rsOutlet[item.StockOutgoingLD.IDAsal]; }
                                else if (item.StockOutgoingLD.TipeAsal == 2) { NamaAsal = rsExhibition[item.StockOutgoingLD.IDAsal]; }

                                td = new TableCell();
                                td.Text = NamaAsal;
                                tr.Cells.Add(td);

                                string NamaTujuan = "";
                                if (item.StockOutgoingLD.TipeTujuan == 0) { NamaTujuan = rsWarehouse[item.StockOutgoingLD.IDTujuan]; }
                                else if (item.StockOutgoingLD.TipeTujuan == 1) { NamaTujuan = rsOutlet[item.StockOutgoingLD.IDTujuan]; }
                                else if (item.StockOutgoingLD.TipeTujuan == 2) { NamaTujuan = rsExhibition[item.StockOutgoingLD.IDTujuan]; }

                                td = new TableCell();
                                td.Text = NamaTujuan;
                                tr.Cells.Add(td);

                                int Qty = 0;
                                Qty = item.StockIncomingLD_Product.Count();

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Qty) : Qty.ToString("N0");
                                //td.Text = Str.Number0(Qty);
                                qty += Qty;
                                tr.Cells.Add(td);

                                var rslt = db.StockIncomingLD_Product.Where(p => p.StockIncomingLD.StatusTransIn == 2 && p.StockIncomingLD.ID == item.ID);

                                int QtyReceived = 0;
                                int QtyBroken = 0;
                                int QtyLost = 0;

                                if (rslt != null)
                                {
                                    QtyReceived = (from p in rslt
                                                   where p.StockIncomingLD.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                   select p).Count();
                                    QtyLost = (from p in rslt
                                               where p.StockIncomingLD.StatusTransIn == 2 && p.SubStorageResult == 99
                                               select p).Count();
                                    QtyBroken = (from p in rslt
                                                 where p.StockIncomingLD.StatusTransIn == 2 && p.SubStorageResult == 96
                                                 select p).Count();
                                }

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyReceived) : QtyReceived.ToString("N0");
                                //td.Text = Str.Number0(QtyReceived);
                                qtyReceived += QtyReceived;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyBroken) : QtyBroken.ToString("N0");
                                //td.Text = Str.Number0(QtyBroken);
                                qtyBroken += QtyBroken;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyLost) : QtyLost.ToString("N0");
                                //td.Text = Str.Number0(QtyLost);
                                qtyLost += QtyLost;
                                tr.Cells.Add(td);

                                decimal Netto = 0m;
                                //Netto = item.StockIncomingPG_Product.Sum(p => p.StockProductPG.StockProductPG_CharProduct.NettoWeight);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Netto) : Netto.ToString("N2");
                                //td.Text = Str.Number2(Netto);
                                netto += Netto;
                                tr.Cells.Add(td);

                                decimal Gross = 0m;
                                //Gross = item.StockIncomingPG_Product.Sum(p => p.StockProductPG.StockProductPG_CharProduct.GrossWeight);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number2(Gross) : Gross.ToString("N2");
                                //td.Text = Str.Number2(Gross);
                                gross += Gross;
                                tr.Cells.Add(td);

                                decimal Butir = 0m;
                                Butir = item.StockIncomingLD_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductLD_Stone1B.TotalButir : 0);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(Butir) : Butir.ToString("N0");
                                //td.Text = Str.Number0(Butir);
                                butir += Convert.ToDecimal(Butir);
                                tr.Cells.Add(td);

                                decimal Carat = 0m;
                                Carat = item.StockIncomingLD_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductLD_Stone1B.TotalCarat : 0);

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number3(Carat) : Carat.ToString("N3");
                                //td.Text = Str.Number3(Carat);
                                carat += Carat;
                                tr.Cells.Add(td);

                                decimal PriceR = 0m;
                                PriceR = Convert.ToDecimal(item.StockIncomingLD_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductLD_Stone1B.HargaTotal : 0));

                                td = new TableCell();
                                td.Text = (Action.ToLower() != "excel") ? Str.Number0(PriceR) : PriceR.ToString("N0");
                                //td.Text = Str.Number0(PriceR);
                                harga += Convert.ToDecimal(PriceR);
                                tr.Cells.Add(td);

                                if (Modul.ToLower() == "accounting") // accounting only
                                {
                                    Decimal HargaM = 0m;
                                    HargaM = Convert.ToDecimal(item.StockIncomingLD_Product.Sum(p => p.SubStorageResult != 99 ? p.StockProductLD_Stone1B.HargaTotalM : 0));
                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number2(HargaM) : HargaM.ToString("N2");
                                    //td.Text = Str.Number2(HargaM);
                                    hargaM += HargaM;
                                    tr.Cells.Add(td);

                                }

                                td = new TableCell();
                                td.Text = item.Operator;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.OperatorTgl);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = item.Approval;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Tgl(item.ApprovalTgl);
                                tr.Cells.Add(td);

                                tbl_Table.Rows.Add(tr);
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;

                            if (Action.ToLower() == "display") // display only
                            {
                                td31 = new TableCell();
                                tr31.Cells.Add(td31);

                                td31 = new TableCell();
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number3(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(harga) : harga.ToString("N0");
                            //td31.Text = "<b>" + harga.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);

                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                    else if (Tipe.ToLower() == "detail") // detail
                    {
                        int vB = ((s_vB != "") ? Convert.ToInt32(s_vB) : -1);
                        int stats = ((s_stats != "") ? Convert.ToInt32(s_stats) : -1);
                        int kF = ((s_kF != "") ? Convert.ToInt32(s_kF) : -1);
                        int kT = ((s_kT != "") ? Convert.ToInt32(s_kT) : -1);
                        int tF = ((s_tF != "") ? Convert.ToInt32(s_tF) : -1); // type from
                        int tT = ((s_tT != "") ? Convert.ToInt32(s_tT) : -1); // type to

                        DateTime d_pf = Convert.ToDateTime(s_pf);
                        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

                        /// SELECT
                        var rs = (from p in db.StockIncomingLD select p);

                        /// WHERE
                        rs = rs.Where(p => p.StockOutgoingLD.IDBrand == ((vB >= 0) ? vB : p.StockOutgoingLD.IDBrand))
                               .Where(p => p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null)
                               ;
                        /// filter keyword
                        string keyword = s_kw.Trim();
                        if (keyword != "") { rs = rs.Where(p => p.Nomor.Contains(keyword)); }

                        /// filter lokasi
                        if (kF > 0)
                        {
                            if (tF == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingLD.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingLD.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                            else if (tF == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingLD.IDAsal equals q.ID
                                     where q.ID == (kF < 0 ? q.ID : kF)
                                     select p;
                            }
                        }

                        if (kT > 0)
                        {

                            if (tT == 0)
                            {
                                rs = from p in rs
                                     join q in db.LocWarehouse on p.StockOutgoingLD.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 1)
                            {
                                rs = from p in rs
                                     join q in db.LocOutlet on p.StockOutgoingLD.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                            else if (tT == 2)
                            {
                                rs = from p in rs
                                     join q in db.LocExhibition on p.StockOutgoingLD.IDTujuan equals q.ID
                                     where q.ID == (kT < 0 ? q.ID : kT)
                                     select p;
                            }
                        }

                        if (stats >= 0) { rs = rs.Where(p => p.StatusTransIn == stats); }

                        /// Check jika user login adalah AE atau SK
                        int UserID = App.UserID;
                        var d = from p in db.DataAdminSales
                                where p.IDUser == UserID
                                select p;

                        var f = from p in db.DataSK
                                where p.IDUser == UserID
                                select p;

                        if (d.Count() > 0 || f.Count() > 0)
                        {
                            if (d.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingLD.TipeTujuan == d.First().TipeLokasi && p.StockOutgoingLD.IDTujuan == d.First().IDLokasi)
                                     select p;
                            }
                            else if (f.Count() > 0)
                            {
                                rs = from p in rs
                                     where (p.StockOutgoingLD.TipeTujuan == f.First().TipeLokasi && p.StockOutgoingLD.IDTujuan == f.First().IDLokasi)
                                     select p;
                            }
                        }

                        /// Setting Tipe dan ID
                        if (tF >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingLD.TipeAsal == tF);
                            if (kF > 0) { rs = rs.Where(p => p.StockOutgoingLD.IDAsal == kF); }
                        }
                        if (tT >= 0)
                        {
                            rs = rs.Where(p => p.StockOutgoingLD.TipeTujuan == tT);
                            if (kT > 0) { rs = rs.Where(p => p.StockOutgoingLD.IDTujuan == kT); }
                        }

                        /// ORDERBY
                        rs = rs.OrderBy(p => p.Tgl);

                        int index = 1;

                        if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                        else
                        {
                            decimal qty = 0;
                            decimal qtyReceived = 0;
                            decimal qtyBroken = 0;
                            decimal qtyLost = 0;
                            decimal netto = 0;
                            decimal gross = 0;
                            decimal butir = 0m;
                            decimal carat = 0;
                            decimal harga = 0;

                            decimal hargaM = 0m;
                            decimal hargaR = 0m;

                            TableHeaderRow htr = new TableHeaderRow();
                            TableHeaderCell htd;

                            htd = new TableHeaderCell();
                            htd.Text = "No.";
                            htd.Width = 50;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Company Brand";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Tanggal";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Terima";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "No. Dokumen Kirim";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Status Dokumen Kirim";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "PLU";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Keterangan";
                            htd.Width = 200;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "From";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "To";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Received";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Broken";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Qty Lost";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Netto";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Gross";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Butir";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Carat";
                            htd.Width = 70;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Harga Jual";
                            htd.Width = 150;
                            htr.Cells.Add(htd);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                htd = new TableHeaderCell();
                                htd.Text = "M";
                                htd.Width = 150;
                                htr.Cells.Add(htd);

                            }

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Operator Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Nama";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            htd = new TableHeaderCell();
                            htd.Text = "Approval Tgl";
                            htd.Width = 100;
                            htr.Cells.Add(htd);

                            tbl_Table.Rows.Add(htr);

                            foreach (var item in rs)
                            {
                                if (!response.IsClientConnected) break;

                                foreach (var u in item.StockIncomingLD_Product)
                                {

                                    if (!response.IsClientConnected) break;

                                    TableRow tr = new TableRow();
                                    TableCell td;

                                    td = new TableCell();
                                    td.Text = "<p>" + index++ + ".</p>";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = rsBrand[u.StockIncomingLD.StockOutgoingLD.IDBrand];
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.Tgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.StockOutgoingLD.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = DL.IncomingApprovalStatus(item.StatusTransIn);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = u.StockProductLD_Stone1B.Nomor;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.HtmlBreak(item.StockOutgoingLD.Keterangan);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.StockOutgoingLD.NamaAsal;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.StockOutgoingLD.NamaTujuan;
                                    tr.Cells.Add(td);

                                    int Qty = 0;
                                    Qty = 1;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(Qty) : Qty.ToString("N0");
                                    //td.Text = Str.Number0(Qty);
                                    qty += Qty;
                                    tr.Cells.Add(td);

                                    var rslt = db.StockIncomingLD_Product.Where(p => p.StockIncomingLD.StatusTransIn == 2 && p.StockIncomingLD.ID == item.ID && p.IDProduct == u.IDProduct);

                                    int QtyReceived = 0;
                                    int QtyBroken = 0;
                                    int QtyLost = 0;

                                    if (rslt != null)
                                    {
                                        QtyReceived = (from p in rslt
                                                       where p.StockIncomingLD.StatusTransIn == 2 && p.SubStorageResult != 96 && p.SubStorageResult != 99
                                                       select p).Count();
                                        QtyLost = (from p in rslt
                                                   where p.StockIncomingLD.StatusTransIn == 2 && p.SubStorageResult == 99
                                                   select p).Count();
                                        QtyBroken = (from p in rslt
                                                     where p.StockIncomingLD.StatusTransIn == 2 && p.SubStorageResult == 96
                                                     select p).Count();
                                    }

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyReceived) : QtyReceived.ToString("N0");
                                    //td.Text = Str.Number0(QtyReceived);
                                    qtyReceived += QtyReceived;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyBroken) : QtyBroken.ToString("N0");
                                    //td.Text = Str.Number0(QtyBroken);
                                    qtyBroken += QtyBroken;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(QtyLost) : QtyLost.ToString("N0");
                                    //td.Text = Str.Number0(QtyLost);
                                    qtyLost += QtyLost;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(0) : (0).ToString("N0");
                                    //td.Text = Str.Number2(0); // nett
                                    qtyBroken += QtyBroken;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(0) : (0).ToString("N0");
                                    //td.Text = Str.Number2(0); // gross
                                    qtyLost += QtyLost;
                                    tr.Cells.Add(td);

                                    decimal Butir = 0m;
                                    Butir = u.SubStorageResult != 99 ? u.StockProductLD_Stone1B.TotalButir : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(Butir) : Butir.ToString("N0");
                                    //td.Text = Str.Number0(Butir);
                                    butir += Convert.ToDecimal(Butir);
                                    tr.Cells.Add(td);

                                    decimal Carat = 0m;
                                    Carat = u.SubStorageResult != 99 ? u.StockProductLD_Stone1B.TotalCarat : 0;

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number3(Carat) : Carat.ToString("N3");
                                    //td.Text = Str.Number3(Carat);
                                    carat += Carat;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = (Action.ToLower() != "excel") ? Str.Number0(u.StockProductLD_Stone1B.HargaTotal.Value) : (u.StockProductLD_Stone1B.HargaTotal.Value).ToString("N0");
                                    //td.Text = Str.Number2(u.StockProductLD_Stone1B.HargaTotal.Value);
                                    harga += u.SubStorageResult != 99 ? u.StockProductLD_Stone1B.HargaTotal.Value : 0;
                                    tr.Cells.Add(td);

                                    if (Modul.ToLower() == "accounting") // accounting only
                                    {
                                        Decimal HargaM = 0m;
                                        HargaM = Convert.ToDecimal(item.StockIncomingLD_Product.Sum(p => u.SubStorageResult != 99 ? p.StockProductLD_Stone1B.HargaTotalM : 0));
                                        td = new TableCell();
                                        td.Text = (Action.ToLower() != "excel") ? Str.Number3(HargaM) : HargaM.ToString("N3");
                                        //td.Text = Str.Number2(HargaM);
                                        hargaM += HargaM;
                                        tr.Cells.Add(td);
                                    }

                                    td = new TableCell();
                                    td.Text = item.Operator;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.OperatorTgl);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = item.Approval;
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = Str.Tgl(item.ApprovalTgl);
                                    tr.Cells.Add(td);

                                    tbl_Table.Rows.Add(tr);
                                }
                            }
                            TableRow tr31 = new TableRow();
                            TableCell td31;
                        
                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "<b>Total</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qty) : qty.ToString("N0");
                            //td31.Text = "<b>" + qty.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyReceived) : qtyReceived.ToString("N0");
                            //td31.Text = "<b>" + qtyReceived.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyBroken) : qtyBroken.ToString("N0");
                            //td31.Text = "<b>" + qtyBroken.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(qtyLost) : qtyLost.ToString("N0");
                            //td31.Text = "<b>" + qtyLost.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(netto) : netto.ToString("N2");
                            //td31.Text = "<b>" + netto.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(gross) : gross.ToString("N2");
                            //td31.Text = "<b>" + gross.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number0(butir) : butir.ToString("N0");
                            //td31.Text = "<b>" + butir.ToString("N0") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number3(carat) : carat.ToString("N3");
                            //td31.Text = "<b>" + carat.ToString("N3") + "</b>";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = (Action.ToLower() != "excel") ? Str.Number2(harga) : harga.ToString("N2");
                            //td31.Text = "<b>" + harga.ToString("N2") + "</b>";
                            tr31.Cells.Add(td31);

                            if (Modul.ToLower() == "accounting") // accounting only
                            {
                                td31 = new TableCell();
                                td31.Text = (Action.ToLower() != "excel") ? Str.Number2(hargaM) : hargaM.ToString("N2");
                                //td31.Text = "<b>" + hargaM.ToString("N2") + "</b>";
                                tr31.Cells.Add(td31);
                            }

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            td31 = new TableCell();
                            td31.Text = "";
                            tr31.Cells.Add(td31);

                            tbl_Table.Rows.Add(tr31);
                        }
                    }
                }
            }
            }
}


    protected static string TD_HyperLink(int ID, string Mode)
    {
        string x = string.Empty;

        x += "<a class='btn btn-info' href='" + Mode + "_Detail.aspx?id=" + ID + "'>";
        x += "  <i class='icon-search'></i> ";
        //x += "  View";
        x += "</a>";

        return x;
    }
    protected static string TD_Print(int ID, string Mode)
    {
        string x = string.Empty;

        x += "<a class='btn btn-info' href='" + Mode + "_Print.aspx?id=" + ID + "'>";
        x += "  <i class='icon-print'></i> ";
        //x += "  Print";
        x += "</a>";

        return x;
    }
    protected static string TD_Nama(int t, int r)
    {
        using (Map_DB db = new Map_DB())
        {
            string x = string.Empty;

            if (t == 0)
            {
                var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
                x += "<p><strong>" + rs.Nama + "</strong></p>";
            }
            else if (t == 1)
            {
                var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
                x += "<p><strong>" + rs.Nama + "</strong></p>";
            }
            else if (t == 2)
            {
                var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
                x += "<p><strong>" + rs.Nama + "</strong></p>";
            }

            return x;
            }
}
    protected static string TD_Addr(int t, int r)
    {
        using (Map_DB db = new Map_DB())
        {
            string x = string.Empty;

            if (t == 0)
            {
                var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
                if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                    x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
            }
            else if (t == 1)
            {
                var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
                if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                    x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
            }
            else if (t == 2)
            {
                var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
                if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                    x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
            }

            return x;
            }
}
    protected static string TD_Group(int t, int r)
    {
        using (Map_DB db = new Map_DB())
        {
            string x = string.Empty;

            if (t == 0)
            {
                var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
                x += "<p class=\"text-info\">" + rs.LocWarehouseGroup.NamaKode + "</p>";
            }
            else if (t == 1)
            {
                var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
                x += "<p class=\"text-info\">" + rs.LocOutletGroup.NamaKode + "</p>";
            }
            else if (t == 2)
            {
                var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
                x += "<p class=\"text-info\">" + rs.LocExhibitionGroup.NamaKode + "</p>";
            }

            return x;
            }
}
    protected static void TableNoResult(Table tb)
    {
        tb.Width = new Unit("100%");

        TableRow tr = new TableRow();
		TableCell td = new TableCell();

		td.Text = "No Data";
		//td.ColumnSpan = TableColspan(tb);
        //td.Width = new Unit("100%");
		tr.Cells.Add(td);

		tr.CssClass = "no-result";
		tb.Rows.Add(tr);
    }


        public static void Fill_OutgoingIncoming(
        String Mode, String Tipe, String Modul, String Sign, String Action,
        Table tbl,
        String stipelokasifrom,
        String slokasifrom,
        String stipelokasito,
        String slokasito,
        String sbrand,
        String sstatusdokumen,
        String stglfrom,
        String stglto,
        String snomordokumen,
        System.Web.UI.Page page,
        System.Web.HttpRequest request,
        System.Web.HttpResponse response
    )
    {
        //String Mode = "DJ"; // DJ, PG, GJ, LD
        //String Tipe = "rekap"; // rekap, detail
        //String Modul = "accounting"; // accounting, outlet (outlet tidak ada harga M dan harga R pada tabel)
        //String Sign = "outgoing"; // incoming, outgoing
        //String Action = "display"; // display, excel, print

        tbl.Visible = true;

        DateTime tempDari = DateTime.ParseExact(stglfrom, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        string stgl1 = tempDari.ToString("yyyy-MM-dd");

        DateTime tempSampai = DateTime.ParseExact(stglto, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        string stgl2 = tempSampai.ToString("yyyy-MM-dd");

        string query = " exec sp_report_outgoingincoming_" + Mode + " "
                    + " '" + Sign + "'" 
                    + ",'" + Modul + "'"
                    + ",'" + Tipe + "'"
                    + ",'" + stgl1 + "'"
                    + ",'" + stgl2 + "'"
                    + ",'" + snomordokumen + "'"
                    + ",'" + sbrand + "'"
                    + ",'" + stipelokasifrom + "'"
                    + ",'" + slokasifrom + "'"
                    + ",'" + stipelokasito + "'"
                    + ",'" + slokasito + "'"
                    + ",'" + sstatusdokumen + "'"
                    ;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        if (dt == null) { TableNoResult(tbl); }
        else if (dt.Rows.Count == 0) { if (Action == "display") { TableNoResult(tbl); } }
        else
        {
            int index = 1;
            int indexColumn = 1;
            string result = "";

            decimal tqty = 0m;
            decimal tqtyr = 0m;
            decimal tqtyb = 0m;
            decimal tqtyl = 0m;
            decimal tnetto = 0m;
            decimal tgross = 0m;
            decimal tbutir = 0m;
            decimal tcarat = 0m;
            decimal tnilaiharga = 0m;
            decimal tnilaimawal = 0m;
            decimal tnilaim = 0m;
            decimal tnilaimreval = 0m;
            decimal tnilair = 0m;

            TableHeaderRow htr;
            TableHeaderCell htd;

            /// Header 1
            htr = new TableHeaderRow();

            htd = new TableHeaderCell();
            htd.Text = "No.";
            htd.Width = 50;
            htd.RowSpan = 2;
            htr.Cells.Add(htd);

            if (Action == "display" && Tipe == "rekap")
            {
                htd = new TableHeaderCell();
                htd.Text = "View";
                htd.Width = 50;
                htd.RowSpan = 2;
                htr.Cells.Add(htd);

                htd = new TableHeaderCell();
                htd.Text = "Print";
                htd.Width = 50;
                htd.RowSpan = 2;
                htr.Cells.Add(htd);
            }

            htd = new TableHeaderCell();
            htd.Text = "Company Brand";
            htd.Width = 120;
            htd.RowSpan = 2;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Kirim";
            htd.Width = 550;
            htd.ColumnSpan = 4;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Terima";
            htd.Width = 550;
            htd.ColumnSpan = 4;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Status" + " " + Sign.ToUpper();
            htd.Width = 100;
            htd.RowSpan = 2;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Keterangan";
            htd.Width = 250;
            htd.RowSpan = 2;
            htr.Cells.Add(htd);

            if (Tipe == "detail")
            {
                htd = new TableHeaderCell();
                htd.Text = "PLU";
                htd.Width = 100;
                htd.RowSpan = 2;
                htr.Cells.Add(htd);

                htd = new TableHeaderCell();
                htd.Text = "Kode Karet";
                htd.Width = 100;
                htd.RowSpan = 2;
                htr.Cells.Add(htd);

                htd = new TableHeaderCell();
                htd.Text = "Item";
                htd.Width = 100;
                htd.RowSpan = 2;
                htr.Cells.Add(htd);

                htd = new TableHeaderCell();
                htd.Text = "Level";
                htd.Width = 100;
                htd.RowSpan = 2;
                htr.Cells.Add(htd);

                htd = new TableHeaderCell();
                htd.Text = "Category";
                htd.Width = 100;
                htd.RowSpan = 2;
                htr.Cells.Add(htd);
            }

            htd = new TableHeaderCell();
            htd.Text = "Asal";
            htd.Width = 200;
            htd.ColumnSpan = 2;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Tujuan";
            htd.Width = 200;
            htd.ColumnSpan = 2;
            htr.Cells.Add(htd);

            if (Modul == "accounting" && Tipe != "rekap")
            {
                htd = new TableHeaderCell();
                htd.Text = "Tgl Reval";
                htd.Width = 100;
                htd.RowSpan = 2;
                htr.Cells.Add(htd);
            }

            htd = new TableHeaderCell();
            htd.Text = "Qty";
            htd.Width = 100;
            htd.RowSpan = 2;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Penerimaan";
            htd.Width = 300;
            htd.ColumnSpan = 3;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Netto";
            htd.Width = 100;
            htd.RowSpan = 2;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Gross";
            htd.Width = 100;
            htd.RowSpan = 2;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Butir";
            htd.Width = 100;
            htd.RowSpan = 2;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Carat";
            htd.Width = 100;
            htd.RowSpan = 2;
            htr.Cells.Add(htd);

            if (Modul == "accounting")
            {
                htd = new TableHeaderCell();
                htd.Text = "M";
                htd.Width = 300;
                htd.ColumnSpan = 3;
                htr.Cells.Add(htd);

                htd = new TableHeaderCell();
                htd.Text = "R";
                htd.Width = 100;
                htd.RowSpan = 2;
                htr.Cells.Add(htd);
            }

            htd = new TableHeaderCell();
            htd.Text = "Harga Jual";
            htd.Width = 150;
            htd.RowSpan = 2;
            htr.Cells.Add(htd);

            tbl.Rows.Add(htr);

            /// Header 2
            htr = new TableHeaderRow();

            htd = new TableHeaderCell();
            htd.Text = "No. Dokumen";
            htd.Width = 150;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Operator";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Approved By";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Tanggal";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "No. Dokumen";
            htd.Width = 150;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Operator";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Approved By";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Tanggal";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Tipe";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Lokasi";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Tipe";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Lokasi";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Received";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Broken";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Lost";
            htd.Width = 100;
            htr.Cells.Add(htd);

            if (Modul == "accounting")
            {
                htd = new TableHeaderCell();
                htd.Text = "Awal";
                htd.Width = 100;
                htr.Cells.Add(htd);

                htd = new TableHeaderCell();
                htd.Text = "Sebelum Reval";
                htd.Width = 100;
                htr.Cells.Add(htd);

                htd = new TableHeaderCell();
                htd.Text = "Sesudah Reval";
                htd.Width = 100;
                htr.Cells.Add(htd);
            }

            tbl.Rows.Add(htr);


            TableRow tr;
            TableCell td;

            // On all tables' rows
            foreach (DataRow dtRow in dt.Rows)
            {
                tr = new TableRow();

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                indexColumn = 0;

                // On all tables' columns
                foreach (DataColumn dc in dt.Columns)
                {
                    if (Modul != "accounting" && (dc.ColumnName == "val2Mrec" || dc.ColumnName == "val2Mold" || dc.ColumnName == "val2Mrev" || dc.ColumnName == "val2R")) { }
                    else
                    {
                        if (dc.ColumnName.Substring(0, 3) == "val" && Action == "excel") 
                        {
                            if (dc.ColumnName.Substring(3, 1) == "1") { result = Convert.ToDecimal(dtRow[dc]).ToString("0.0").Replace(",", "."); }
                            else if (dc.ColumnName.Substring(3, 1) == "2") { result = Convert.ToDecimal(dtRow[dc]).ToString("0.00").Replace(",", "."); }
                            else if (dc.ColumnName.Substring(3, 1) == "3") { result = Convert.ToDecimal(dtRow[dc]).ToString("0.000").Replace(",", "."); }
                            else if (dc.ColumnName.Substring(3, 1) == "4") { result = Convert.ToDecimal(dtRow[dc]).ToString("0.0000").Replace(",", "."); }
                            else { result = Convert.ToDecimal(dtRow[dc]).ToString("0").Replace(",","."); }
                        }
                        else if (dc.ColumnName.Substring(0, 4) == "val3") { result = Convert.ToDecimal(dtRow[dc]).ToString("N3"); }
                        else if (dc.ColumnName.Substring(0, 4) == "val2") { result = Convert.ToDecimal(dtRow[dc]).ToString("N2"); }
                        else if (dc.ColumnName.Substring(0, 4) == "val1") { result = Convert.ToDecimal(dtRow[dc]).ToString("N1"); }
                        else if (dc.ColumnName.Substring(0, 4) == "val0") { result = Convert.ToDecimal(dtRow[dc]).ToString("N0"); }
                        else if (dc.ColumnName == "IDView") { result = "<a class='btn btn-info' href='" + Mode + "_Detail.aspx?id=" + dtRow[dc].ToString() + "'>  <span class='glyphicon glyphicon-search' aria-hidden='true'></span> </a>"; }
                        else if (dc.ColumnName == "IDPrint") { result = "<a class='btn btn-info' href='" + Mode + "_Print.aspx?id=" + dtRow[dc].ToString() + "'>  <span class='glyphicon glyphicon-print' aria-hidden='true'></span> </a>"; }
                        else if (dc.ColumnName == "StatusDokumen" && Sign == "outgoing") { result = DL.OutgoingApprovalStatus(Convert.ToInt32(dtRow[dc])); }
                        else if (dc.ColumnName == "StatusDokumen" && Sign == "incoming") { result = DL.IncomingApprovalStatus(Convert.ToInt32(dtRow[dc])); }
                        else { result = dtRow[dc].ToString(); }

                        if (dc.ColumnName == "val0Qty") { tqty += Convert.ToDecimal(dtRow[dc]); }
                        else if (dc.ColumnName == "val0QtyRec") { tqtyr += Convert.ToDecimal(dtRow[dc]); }
                        else if (dc.ColumnName == "val0QtyBro") { tqtyb += Convert.ToDecimal(dtRow[dc]); }
                        else if (dc.ColumnName == "val0QtyLos") { tqtyl += Convert.ToDecimal(dtRow[dc]); }
                        else if (dc.ColumnName == "val2Netto") { tnetto += Convert.ToDecimal(dtRow[dc]); }
                        else if (dc.ColumnName == "val2Gross") { tgross += Convert.ToDecimal(dtRow[dc]); }
                        else if (dc.ColumnName == "val0Butir") { tbutir += Convert.ToDecimal(dtRow[dc]); }
                        else if (dc.ColumnName == "val3Carat") { tcarat += Convert.ToDecimal(dtRow[dc]); }
                        else if (dc.ColumnName == "val2Mrec") { tnilaimawal += Convert.ToDecimal(dtRow[dc]); }
                        else if (dc.ColumnName == "val2Mold") { tnilaim += Convert.ToDecimal(dtRow[dc]); }
                        else if (dc.ColumnName == "val2Mrev") { tnilaimreval += Convert.ToDecimal(dtRow[dc]); }
                        else if (dc.ColumnName == "val2R") { tnilair += Convert.ToDecimal(dtRow[dc]); }
                        else if (dc.ColumnName == "val0Harga") { tnilaiharga += Convert.ToDecimal(dtRow[dc]); }

                        td = new TableCell();
                        td.Text = result;
                        tr.Cells.Add(td);
                    }
                    indexColumn++;
                }

                tbl.Rows.Add(tr);
                index++;
            }

            /// Grand Total
            tr = new TableRow();

            td = new TableCell();
            td.Text = "<b>" + "Total" + "</b>";
            td.HorizontalAlign = HorizontalAlign.Right;
            td.ColumnSpan = 19 - ((Action == "display" && Tipe == "rekap") ? 0 : 2) + (Tipe == "detail" ? 4 : 0) - (Modul == "accounting" && Tipe == "rekap" ? 1 : 0);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + ((Action == "excel") ? tqty.ToString("0").Replace(",", ".") : tqty.ToString("N0")) + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + ((Action == "excel") ? tqtyr.ToString("0").Replace(",", ".") : tqtyr.ToString("N0")) + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + ((Action == "excel") ? tqtyb.ToString("0").Replace(",", ".") : tqtyb.ToString("N0")) + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + ((Action == "excel") ? tqtyl.ToString("0").Replace(",", ".") : tqtyl.ToString("N0")) + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + ((Action == "excel") ? tnetto.ToString("0.00").Replace(",", ".") : tnetto.ToString("N2")) + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + ((Action == "excel") ? tgross.ToString("0.00").Replace(",", ".") : tgross.ToString("N2")) + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + ((Action == "excel") ? tbutir.ToString("0").Replace(",", ".") : tbutir.ToString("N0")) + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + ((Action == "excel") ? tcarat.ToString("0.000").Replace(",", ".") : tcarat.ToString("N3")) + "</b>";
            tr.Cells.Add(td);

            if (Modul != "accounting") { }
            else
            {
                td = new TableCell();
                td.Text = "<b>" + ((Action == "excel") ? tnilaimawal.ToString("0.00").Replace(",", ".") : tnilaimawal.ToString("N2")) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + ((Action == "excel") ? tnilaim.ToString("0.00").Replace(",", ".") : tnilaim.ToString("N2")) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + ((Action == "excel") ? tnilaimreval.ToString("0.00").Replace(",", ".") : tnilaimreval.ToString("N2")) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + ((Action == "excel") ? tnilair.ToString("0.00").Replace(",", ".") : tnilair.ToString("N2")) + "</b>";
                tr.Cells.Add(td);
            }

            td = new TableCell();
            td.Text = "<b>" + ((Action == "excel") ? tnilaiharga.ToString("0").Replace(",", ".") : tnilaiharga.ToString("N0")) + "</b>";
            tr.Cells.Add(td);

            tbl.Rows.Add(tr);

        }


    }
}