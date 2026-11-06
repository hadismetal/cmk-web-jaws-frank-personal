using System;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using CrossBrand;
using CrossBrand.Model;
using System.Security.Policy;

public partial class Lib
{
    public static void FormRegisterUpdateDokumenDraft_DJ(Literal lit_Message, TextBox inputproduct, System.Web.UI.HtmlControls.HtmlButton save, CheckBoxList cbl_Product, int id, string Mode = "")
    {
        using (Map_DB d = new Map_DB())
        {

            /// Master Brand
            var rsBrand = (from p in d.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in d.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in d.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in d.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            var rsH = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id);
            int vB = rsH.IDBrand;
            int vF = rsH.TipeAsal;
            int LokasiAsal = rsH.IDAsal;

            lit_Message.Text = "";
            string input = inputproduct.Text.Trim();

            if(input.ToLower().StartsWith("https://pos-jaws.cmk.co.id"))
            {
                string ShortUrl = input.Substring(input.LastIndexOf('/') + 1);
                string ExtractedPLU = Db.SingleString("SELECT ISNULL(PLU, '') [PLU] FROM mondial_db_real.dbo.QRCatalogJaws WHERE ShortUrl = '" + ShortUrl + "'", Db.Cnn_DB);
                input = ExtractedPLU;
            }

            try
            {
                List<StockActualDJ> list = d.StockActualDJ.Where(p => p.StockProductDJ.Nomor == input
                    && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.Draft == false).ToList();

                var rs = list.SingleOrDefault(p => !(from z in d.StockOutgoingDJ_Product
                                                 where (z.StockOutgoingDJ.StatusTransOut == 2 || z.StockOutgoingDJ.StatusTransOut == 1) && z.StockOutgoingDJ.StockIncomingDJ.Any(u => u.StatusTransIn == 2) 
                                                 select z.IDProduct).Contains(p.IDProduct)); 

                if (rs != null)
                {
                    /// Check Apakah produk sudah pernah diinput (tidak bisa input barang yang sama 2x pada dokumen yang sama)
                    string err_msg = validDJ(input, cbl_Product);

                    /// Check Gambar Produk apakah ada
                    //string curFile = @"" + rs.StockProductDJ.ImgPicture;
                    ////if (!File.Exists(curFile) && vF == 0)
                    //if (1 != 1)
                    //{
                    //    err_msg = "Gambar Product tidak ada.";
                    //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                    //}

                    /// Check Apakah produk sudah pernah terinput di dokumen draft
                    if (Mode != "update")
                    {
                        var rsProductDraft = (from z in d.StockOutgoingDJ_Product where z.StockProductDJ.Nomor == input && z.StockOutgoingDJ.StatusTransOut == 0 select z);
                        if (rsProductDraft.Count() > 0)
                        {
                            err_msg = "Kode Produk <b>" + input + "</b> sudah pernah terinput di dokumen Draft. Silahkan selesaikan / hapus terlebih dahulu dokumen Draft bersangkutan.";
                            err_msg += "<br /><br /><table class='table-list'>";
                            err_msg += "<tr>"
                                    + "<th style='width:200px;'>Operator</th>"
                                    + "<th style='width:150px;'>Tanggal</th>"
                                    + "<th style='width:100px;'>Tipe Asal</th>"
                                    + "<th style='width:200px;'>Nama Asal</th>"
                                    + "<th style='width:100px;'>Tipe Tujuan</th>"
                                    + "<th style='width:200px;'>Nama Tujuan</th>"
                                    + "</tr>";
                            foreach (var drft in rsProductDraft)
                            {
                                string sTipeAsal = (drft.StockOutgoingDJ.TipeAsal == 0) ? "Warehouse"
                                                 : (drft.StockOutgoingDJ.TipeAsal == 1) ? "Outlet"
                                                 : (drft.StockOutgoingDJ.TipeAsal == 2) ? "Exhibition"
                                                 : ""
                                                 ;
                                string sNamaAsal = (drft.StockOutgoingDJ.TipeAsal == 0) ? rsWarehouse[drft.StockOutgoingDJ.IDAsal]
                                                 : (drft.StockOutgoingDJ.TipeAsal == 1) ? rsOutlet[drft.StockOutgoingDJ.IDAsal]
                                                 : (drft.StockOutgoingDJ.TipeAsal == 2) ? rsExhibition[drft.StockOutgoingDJ.IDAsal]
                                                 : ""
                                                 ;
                                string sTipeTujuan = (drft.StockOutgoingDJ.TipeTujuan == 0) ? "Warehouse"
                                                   : (drft.StockOutgoingDJ.TipeTujuan == 1) ? "Outlet"
                                                   : (drft.StockOutgoingDJ.TipeTujuan == 2) ? "Exhibition"
                                                   : ""
                                                   ;

                                string sNamaTujuan = (drft.StockOutgoingDJ.TipeTujuan == 0) ? rsWarehouse[drft.StockOutgoingDJ.IDTujuan]
                                                   : (drft.StockOutgoingDJ.TipeTujuan == 1) ? rsOutlet[drft.StockOutgoingDJ.IDTujuan]
                                                   : (drft.StockOutgoingDJ.TipeTujuan == 2) ? rsExhibition[drft.StockOutgoingDJ.IDTujuan]
                                                   : ""
                                                   ;

                                err_msg += "<tr>";
                                err_msg += "<td>" + drft.StockOutgoingDJ.Operator + "</td>";
                                err_msg += "<td>" + Str.FullDate(drft.StockOutgoingDJ.OperatorTgl) + "</td>";
                                err_msg += "<td>" + sTipeAsal + "</td>";
                                err_msg += "<td>" + sNamaAsal + "</td>";
                                err_msg += "<td>" + sTipeTujuan + "</td>";

                                err_msg += "<td>" + sNamaTujuan + "</td>";
                                err_msg += "<td></td>";
                                err_msg += "</tr>";
                            }
                            err_msg += "</table>";
                        }
                    }

                    /// Jika Tidak ada Error maka insert produk jika tidak tampilkan error message
                    if (err_msg == "")
                    {
                        lit_Message.Text = ""; /// tidak ada error message

                        string v = rs.StockProductDJ.ID.ToString();
                        string t = Logistic.ProductList_DJ(
                                rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                                rs.StockProductDJ.Nomor,
                                rs.StockProductDJ.ImgPicture,
                                DL.SubStorage(rs.Substorage),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                                Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
                                );

                        int jlh = cbl_Product.Items.Count;
                        string[] item = new string[1000];
                        string[] itemvalue = new string[1000];

                        for (int i = 0; i < jlh; i++)
                        {
                            item[i] = cbl_Product.Items[i].Text;
                            itemvalue[i] = cbl_Product.Items[i].Value;
                        }

                        cbl_Product.Items.Clear();

                        cbl_Product.Items.Add(new ListItem(t, v));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items.Add(new ListItem(item[i], itemvalue[i]));
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                        }

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items[i].Selected = true;
                        }

                        //upl.Update();

                        inputproduct.Text = "";
                        inputproduct.Focus();
                    }
                    else
                    {
                        // error message
                        lit_Message.Text = "<div class='alert alert-danger' role='alert'>" + err_msg + "</div>";
                        inputproduct.Focus();
                    }

                }
                else
                {
                    lit_Message.Text = "<span style='color:red;'>Barang sudah terinput / tidak terdapat di cabang ini.</span>";
                        inputproduct.Focus();
                }

    
                save.Visible = cbl_Product.Items.Count > 0 ? true : false;
        }
            catch (Exception ex)
            {
                lit_Message.Text = "<span style='color:red;'>Barang sudah ter-register, cek menu Approval</span>";
                        inputproduct.Focus();
            }
            }
}

    public static void FormRegisterUpdateDokumenDraft_PG(Literal lit_Message, TextBox inputproduct, System.Web.UI.HtmlControls.HtmlButton save, CheckBoxList cbl_Product, int id)
    {
        using (Map_DB d = new Map_DB())
        {

            /// Master Brand
            var rsBrand = (from p in d.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in d.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in d.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in d.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            var rsH = d.StockOutgoingPG.SingleOrDefault(p => p.ID == id);
            int vB = rsH.IDBrand;
            int vF = rsH.TipeAsal;
            int LokasiAsal = rsH.IDAsal;

            lit_Message.Text = "";
            string input = inputproduct.Text.Trim();

            if (input.ToLower().StartsWith("https://pos-jaws.cmk.co.id"))
            {
                string ShortUrl = input.Substring(input.LastIndexOf('/') + 1);
                string ExtractedPLU = Db.SingleString("SELECT ISNULL(PLU, '') [PLU] FROM mondial_db_real.dbo.QRCatalogJaws WHERE ShortUrl = '" + ShortUrl + "'", Db.Cnn_DB);
                input = ExtractedPLU;
            }

            try
            {
                List<StockActualPG> list = d.StockActualPG.Where(p => p.StockProductPG.Nomor == input
                    && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductPG.StatusLebur == false && p.StockProductPG.ReturnProduct == false).ToList();

                var rs = list.SingleOrDefault(p => !(from z in d.StockOutgoingPG_Product
                                                   where (z.StockOutgoingPG.StatusTransOut == 2 || z.StockOutgoingPG.StatusTransOut == 1) && z.StockOutgoingPG.StockIncomingPG.Any(u => u.StatusTransIn == 2)
                                                   select z.IDProduct).Contains(p.IDProduct));

                if (rs != null)
                {
                    /// Check Apakah produk sudah pernah diinput (tidak bisa input barang yang sama 2x pada dokumen yang sama)
                    string err_msg = validPG(input, cbl_Product, false, id);

                    /// Check Gambar Produk apakah ada
                    //string curFile = @"" + rs.StockProductPG.ImgPicture;
                    ////if (!File.Exists(curFile) && vF == 0)
                    //if (1 != 1)
                    //{
                    //    err_msg = "Gambar Product tidak ada.";
                    //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                    //}

                    /// Check Apakah produk sudah pernah terinput di dokumen draft
                    var rsProductDraft = (from z in d.StockOutgoingPG_Product where z.StockProductPG.Nomor == input && z.StockOutgoingPG.StatusTransOut == 0 select z);
                    if (rsProductDraft.Count() > 0)
                    {
                        err_msg = "Kode Produk <b>" + input + "</b> sudah pernah terinput di dokumen Draft. Silahkan selesaikan / hapus terlebih dahulu dokumen Draft bersangkutan.";
                        err_msg += "<br /><br /><table class='table-list'>";
                        err_msg += "<tr>"
                                + "<th style='width:200px;'>Operator</th>"
                                + "<th style='width:150px;'>Tanggal</th>"
                                + "<th style='width:100px;'>Tipe Asal</th>"
                                + "<th style='width:200px;'>Nama Asal</th>"
                                + "<th style='width:100px;'>Tipe Tujuan</th>"
                                + "<th style='width:200px;'>Nama Tujuan</th>"
                                + "</tr>";
                        foreach (var drft in rsProductDraft)
                        {
                            string sTipeAsal = (drft.StockOutgoingPG.TipeAsal == 0) ? "Warehouse"
                                             : (drft.StockOutgoingPG.TipeAsal == 1) ? "Outlet"
                                             : (drft.StockOutgoingPG.TipeAsal == 2) ? "Exhibition"
                                             : ""
                                             ;
                            string sNamaAsal = (drft.StockOutgoingPG.TipeAsal == 0) ? rsWarehouse[drft.StockOutgoingPG.IDAsal]
                                             : (drft.StockOutgoingPG.TipeAsal == 1) ? rsOutlet[drft.StockOutgoingPG.IDAsal]
                                             : (drft.StockOutgoingPG.TipeAsal == 2) ? rsExhibition[drft.StockOutgoingPG.IDAsal]
                                             : ""
                                             ;
                            string sTipeTujuan = (drft.StockOutgoingPG.TipeTujuan == 0) ? "Warehouse"
                                               : (drft.StockOutgoingPG.TipeTujuan == 1) ? "Outlet"
                                               : (drft.StockOutgoingPG.TipeTujuan == 2) ? "Exhibition"
                                               : ""
                                               ;

                            string sNamaTujuan = (drft.StockOutgoingPG.TipeTujuan == 0) ? rsWarehouse[drft.StockOutgoingPG.IDTujuan]
                                               : (drft.StockOutgoingPG.TipeTujuan == 1) ? rsOutlet[drft.StockOutgoingPG.IDTujuan]
                                               : (drft.StockOutgoingPG.TipeTujuan == 2) ? rsExhibition[drft.StockOutgoingPG.IDTujuan]
                                               : ""
                                               ;

                            err_msg += "<tr>";
                            err_msg += "<td>" + drft.StockOutgoingPG.Operator + "</td>";
                            err_msg += "<td>" + Str.FullDate(drft.StockOutgoingPG.OperatorTgl) + "</td>";
                            err_msg += "<td>" + sTipeAsal + "</td>";
                            err_msg += "<td>" + sNamaAsal + "</td>";
                            err_msg += "<td>" + sTipeTujuan + "</td>";

                            err_msg += "<td>" + sNamaTujuan + "</td>";
                            err_msg += "<td></td>";
                            err_msg += "</tr>";
                        }
                        err_msg += "</table>";
                    }

                    /// Jika Tidak ada Error maka insert produk jika tidak tampilkan error message
                    if (err_msg == "")
                    {
                        lit_Message.Text = ""; /// tidak ada error message

                        string v = rs.StockProductPG.ID.ToString();
                        string t = Logistic.ProductList_PG(
                                    rs.StockProductPG.CharProductItem.Nama,
                                    rs.StockProductPG.Nomor,
                                    rs.StockProductPG.ImgPicture,
                                    DL.SubStorage(rs.Substorage),
                                    Str.Number2(rs.StockProductPG.NettoWeight),
                                    Str.Number2(rs.StockProductPG.GrossWeight),
                                    rs.StockProductPG.CharGoldModel.Nama
                                    );

                        int jlh = cbl_Product.Items.Count;
                        string[] item = new string[1000];
                        string[] itemvalue = new string[1000];

                        for (int i = 0; i < jlh; i++)
                        {
                            item[i] = cbl_Product.Items[i].Text;
                            itemvalue[i] = cbl_Product.Items[i].Value;
                        }

                        cbl_Product.Items.Clear();

                        cbl_Product.Items.Add(new ListItem(t, v));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items.Add(new ListItem(item[i], itemvalue[i]));
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                        }

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items[i].Selected = true;
                        }

                        //upl.Update();

                        inputproduct.Text = "";
                        inputproduct.Focus();
                    }
                    else
                    {
                        // error message
                        lit_Message.Text = "<div class='alert alert-danger' role='alert'>" + err_msg + "</div>";
                        inputproduct.Focus();
                    }

                }
                else
                {
                    lit_Message.Text = "<span style='color:red;'>Barang sudah terinput / tidak terdapat di cabang ini</span>";
                        inputproduct.Focus();
                }

    
                save.Visible = cbl_Product.Items.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                lit_Message.Text = "<span style='color:red;'>Barang sudah ter-register, cek menu Approval</span>";
                        inputproduct.Focus();
            }
            }
}

    public static void FormRegisterUpdateDokumenDraft_GJ(Literal lit_Message, TextBox inputproduct, System.Web.UI.HtmlControls.HtmlButton save, CheckBoxList cbl_Product, int id)
    {
        using (Map_DB d = new Map_DB())
        {

            /// Master Brand
            var rsBrand = (from p in d.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in d.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in d.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in d.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            var rsH = d.StockOutgoingGJ.SingleOrDefault(p => p.ID == id);
            int vB = rsH.IDBrand;
            int vF = rsH.TipeAsal;
            int LokasiAsal = rsH.IDAsal;

            lit_Message.Text = "";
            string input = inputproduct.Text.Trim();

            try
            {
                var rs = d.StockActualGJ.SingleOrDefault(p => p.StockProductGJ.Nomor == input
                    && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductGJ.StatusLebur == false && p.StockProductGJ.ReturnProduct == false
                    && !(from z in d.StockOutgoingGJ_Product
                         where (z.StockOutgoingGJ.StatusTransOut == 2 || z.StockOutgoingGJ.StatusTransOut == 1) && z.StockOutgoingGJ.StockIncomingGJ.Any(u => u.StatusTransIn == 2)
                         select z.IDProduct).Contains(p.IDProduct)
                    );

                if (rs != null)
                {
                    /// Check Apakah produk sudah pernah diinput (tidak bisa input barang yang sama 2x pada dokumen yang sama)
                    string err_msg = validGJ(input, cbl_Product);

                    /// Check Gambar Produk apakah ada
                    //string curFile = @"" + rs.StockProductGJ.ImgPicture;
                    ////if (!File.Exists(curFile) && vF == 0)
                    //if (1 != 1)
                    //{
                    //    err_msg = "Gambar Product tidak ada.";
                    //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                    //}

                    /// Check Apakah produk sudah pernah terinput di dokumen draft
                    var rsProductDraft = (from z in d.StockOutgoingGJ_Product where z.StockProductGJ.Nomor == input && z.StockOutgoingGJ.StatusTransOut == 0 select z);
                    if (rsProductDraft.Count() > 0)
                    {
                        err_msg = "Kode Produk <b>" + input + "</b> sudah pernah terinput di dokumen Draft. Silahkan selesaikan / hapus terlebih dahulu dokumen Draft bersangkutan.";
                        err_msg += "<br /><br /><table class='table-list'>";
                        err_msg += "<tr>"
                                + "<th style='width:200px;'>Operator</th>"
                                + "<th style='width:150px;'>Tanggal</th>"
                                + "<th style='width:100px;'>Tipe Asal</th>"
                                + "<th style='width:200px;'>Nama Asal</th>"
                                + "<th style='width:100px;'>Tipe Tujuan</th>"
                                + "<th style='width:200px;'>Nama Tujuan</th>"
                                + "</tr>";
                        foreach (var drft in rsProductDraft)
                        {
                            string sTipeAsal = (drft.StockOutgoingGJ.TipeAsal == 0) ? "Warehouse"
                                             : (drft.StockOutgoingGJ.TipeAsal == 1) ? "Outlet"
                                             : (drft.StockOutgoingGJ.TipeAsal == 2) ? "Exhibition"
                                             : ""
                                             ;
                            string sNamaAsal = (drft.StockOutgoingGJ.TipeAsal == 0) ? rsWarehouse[drft.StockOutgoingGJ.IDAsal]
                                             : (drft.StockOutgoingGJ.TipeAsal == 1) ? rsOutlet[drft.StockOutgoingGJ.IDAsal]
                                             : (drft.StockOutgoingGJ.TipeAsal == 2) ? rsExhibition[drft.StockOutgoingGJ.IDAsal]
                                             : ""
                                             ;
                            string sTipeTujuan = (drft.StockOutgoingGJ.TipeTujuan == 0) ? "Warehouse"
                                               : (drft.StockOutgoingGJ.TipeTujuan == 1) ? "Outlet"
                                               : (drft.StockOutgoingGJ.TipeTujuan == 2) ? "Exhibition"
                                               : ""
                                               ;

                            string sNamaTujuan = (drft.StockOutgoingGJ.TipeTujuan == 0) ? rsWarehouse[drft.StockOutgoingGJ.IDTujuan]
                                               : (drft.StockOutgoingGJ.TipeTujuan == 1) ? rsOutlet[drft.StockOutgoingGJ.IDTujuan]
                                               : (drft.StockOutgoingGJ.TipeTujuan == 2) ? rsExhibition[drft.StockOutgoingGJ.IDTujuan]
                                               : ""
                                               ;

                            err_msg += "<tr>";
                            err_msg += "<td>" + drft.StockOutgoingGJ.Operator + "</td>";
                            err_msg += "<td>" + Str.FullDate(drft.StockOutgoingGJ.OperatorTgl) + "</td>";
                            err_msg += "<td>" + sTipeAsal + "</td>";
                            err_msg += "<td>" + sNamaAsal + "</td>";
                            err_msg += "<td>" + sTipeTujuan + "</td>";

                            err_msg += "<td>" + sNamaTujuan + "</td>";
                            err_msg += "<td></td>";
                            err_msg += "</tr>";
                        }
                        err_msg += "</table>";
                    }

                    /// Jika Tidak ada Error maka insert produk jika tidak tampilkan error message
                    if (err_msg == "")
                    {
                        lit_Message.Text = ""; /// tidak ada error message

                        string v = rs.StockProductGJ.ID.ToString();
                        string t = Logistic.ProductList_GJ(
                                rs.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                                rs.StockProductGJ.Nomor,
                                rs.StockProductGJ.ImgPicture,
                                DL.SubStorage(rs.Substorage),
                                Str.Number2(rs.StockProductGJ.StockProductGJ_CharProduct.NettoWeight),
                                Str.Number2(rs.StockProductGJ.StockProductGJ_CharProduct.GrossWeight)
                                );

                        int jlh = cbl_Product.Items.Count;
                        string[] item = new string[1000];
                        string[] itemvalue = new string[1000];

                        for (int i = 0; i < jlh; i++)
                        {
                            item[i] = cbl_Product.Items[i].Text;
                            itemvalue[i] = cbl_Product.Items[i].Value;
                        }

                        cbl_Product.Items.Clear();

                        cbl_Product.Items.Add(new ListItem(t, v));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items.Add(new ListItem(item[i], itemvalue[i]));
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                        }

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items[i].Selected = true;
                        }

                        //upl.Update();

                        inputproduct.Text = "";
                        inputproduct.Focus();
                    }
                    else
                    {
                        // error message
                        lit_Message.Text = "<div class='alert alert-danger' role='alert'>" + err_msg + "</div>";
                        inputproduct.Focus();
                    }

                }
                else
                {
                    lit_Message.Text = "<span style='color:red;'>Barang sudah terinput / tidak terdapat di cabang ini</span>";
                        inputproduct.Focus();
                }

    
                save.Visible = cbl_Product.Items.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                lit_Message.Text = "<span style='color:red;'>Barang sudah ter-register, cek menu Approval</span>";
                        inputproduct.Focus();
            }
            }
}

    public static void FormRegisterUpdateDokumenDraft_LD(Literal lit_Message, TextBox inputproduct, System.Web.UI.HtmlControls.HtmlButton save, CheckBoxList cbl_Product, int id)
    {
        using (Map_DB d = new Map_DB())
        {

            /// Master Brand
            var rsBrand = (from p in d.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in d.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in d.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in d.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            var rsH = d.StockOutgoingLD.SingleOrDefault(p => p.ID == id);
            int vB = rsH.IDBrand;
            int vF = rsH.TipeAsal;
            int LokasiAsal = rsH.IDAsal;

            lit_Message.Text = "";
            string input = inputproduct.Text.Trim();

            try
            {
                var rs = d.StockActualLD_Stone1B.SingleOrDefault(p => p.StockProductLD_Stone1B.Nomor == input
                    && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductLD_Stone1B.ReturnProduct == false
                    && !(from z in d.StockOutgoingLD_Product
                         where (z.StockOutgoingLD.StatusTransOut == 2 || z.StockOutgoingLD.StatusTransOut == 1) && z.StockOutgoingLD.StockIncomingLD.Any(u => u.StatusTransIn == 2)
                         select z.IDProduct).Contains(p.IDProduct)
                    );

                if (rs != null)
                {
                    /// Check Apakah produk sudah pernah diinput (tidak bisa input barang yang sama 2x pada dokumen yang sama)
                    string err_msg = validLD(input, cbl_Product);

                    /// Check Gambar Produk apakah ada
                    //string curFile = @"" + rs.StockProductLD.ImgPicture;
                    ////if (!File.Exists(curFile) && vF == 0)
                    //if (1 != 1)
                    //{
                    //    err_msg = "Gambar Product tidak ada.";
                    //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                    //}

                    /// Check Apakah produk sudah pernah terinput di dokumen draft
                    var rsProductDraft = (from z in d.StockOutgoingLD_Product where z.StockProductLD_Stone1B.Nomor == input && z.StockOutgoingLD.StatusTransOut == 0 select z);
                    if (rsProductDraft.Count() > 0)
                    {
                        err_msg = "Kode Produk <b>" + input + "</b> sudah pernah terinput di dokumen Draft. Silahkan selesaikan / hapus terlebih dahulu dokumen Draft bersangkutan.";
                        err_msg += "<br /><br /><table class='table-list'>";
                        err_msg += "<tr>"
                                + "<th style='width:200px;'>Operator</th>"
                                + "<th style='width:150px;'>Tanggal</th>"
                                + "<th style='width:100px;'>Tipe Asal</th>"
                                + "<th style='width:200px;'>Nama Asal</th>"
                                + "<th style='width:100px;'>Tipe Tujuan</th>"
                                + "<th style='width:200px;'>Nama Tujuan</th>"
                                + "</tr>";
                        foreach (var drft in rsProductDraft)
                        {
                            string sTipeAsal = (drft.StockOutgoingLD.TipeAsal == 0) ? "Warehouse"
                                             : (drft.StockOutgoingLD.TipeAsal == 1) ? "Outlet"
                                             : (drft.StockOutgoingLD.TipeAsal == 2) ? "Exhibition"
                                             : ""
                                             ;
                            string sNamaAsal = (drft.StockOutgoingLD.TipeAsal == 0) ? rsWarehouse[drft.StockOutgoingLD.IDAsal]
                                             : (drft.StockOutgoingLD.TipeAsal == 1) ? rsOutlet[drft.StockOutgoingLD.IDAsal]
                                             : (drft.StockOutgoingLD.TipeAsal == 2) ? rsExhibition[drft.StockOutgoingLD.IDAsal]
                                             : ""
                                             ;
                            string sTipeTujuan = (drft.StockOutgoingLD.TipeTujuan == 0) ? "Warehouse"
                                               : (drft.StockOutgoingLD.TipeTujuan == 1) ? "Outlet"
                                               : (drft.StockOutgoingLD.TipeTujuan == 2) ? "Exhibition"
                                               : ""
                                               ;

                            string sNamaTujuan = (drft.StockOutgoingLD.TipeTujuan == 0) ? rsWarehouse[drft.StockOutgoingLD.IDTujuan]
                                               : (drft.StockOutgoingLD.TipeTujuan == 1) ? rsOutlet[drft.StockOutgoingLD.IDTujuan]
                                               : (drft.StockOutgoingLD.TipeTujuan == 2) ? rsExhibition[drft.StockOutgoingLD.IDTujuan]
                                               : ""
                                               ;

                            err_msg += "<tr>";
                            err_msg += "<td>" + drft.StockOutgoingLD.Operator + "</td>";
                            err_msg += "<td>" + Str.FullDate(drft.StockOutgoingLD.OperatorTgl) + "</td>";
                            err_msg += "<td>" + sTipeAsal + "</td>";
                            err_msg += "<td>" + sNamaAsal + "</td>";
                            err_msg += "<td>" + sTipeTujuan + "</td>";

                            err_msg += "<td>" + sNamaTujuan + "</td>";
                            err_msg += "<td></td>";
                            err_msg += "</tr>";
                        }
                        err_msg += "</table>";
                    }

                    /// Jika Tidak ada Error maka insert produk jika tidak tampilkan error message
                    if (err_msg == "")
                    {
                        lit_Message.Text = ""; /// tidak ada error message

                        string v = rs.StockProductLD_Stone1B.ID.ToString();
                        string t = Logistic.ProductList_LD(
                                rs.StockProductLD_Stone1B.Nomor,
                                rs.StockProductLD_Stone1B.ImgPicture,
                                DL.SubStorage(rs.Substorage),
                                Str.Number3(rs.StockProductLD_Stone1B.TotalCarat)
                                );

                        int jlh = cbl_Product.Items.Count;
                        string[] item = new string[1000];
                        string[] itemvalue = new string[1000];

                        for (int i = 0; i < jlh; i++)
                        {
                            item[i] = cbl_Product.Items[i].Text;
                            itemvalue[i] = cbl_Product.Items[i].Value;
                        }

                        cbl_Product.Items.Clear();

                        cbl_Product.Items.Add(new ListItem(t, v));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items.Add(new ListItem(item[i], itemvalue[i]));
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                        }

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items[i].Selected = true;
                        }

                        //upl.Update();

                        inputproduct.Text = "";
                        inputproduct.Focus();
                    }
                    else
                    {
                        // error message
                        lit_Message.Text = "<div class='alert alert-danger' role='alert'>" + err_msg + "</div>";
                        inputproduct.Focus();
                    }

                }
                else
                {
                    lit_Message.Text = "<span style='color:red;'>Barang sudah terinput / tidak terdapat di cabang ini</span>";
                        inputproduct.Focus();
                }

    
                save.Visible = cbl_Product.Items.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                lit_Message.Text = "<span style='color:red;'>Barang sudah ter-register, cek menu Approval</span>";
                        inputproduct.Focus();
            }
            }
}

    private static string validDJ(string input, CheckBoxList cbl)
    {
        using (Map_DB d = new Map_DB())
        {
            string message = "";

            // 1. Check apakah input string merupakan kode product / ada di database
            var rs1 = d.StockProductDJ.SingleOrDefault(p => p.Nomor == input && !p.Draft.Value);

            if (rs1 == null) { message = "Invalid Product"; }
            else
            {
                // 2. Check Apakah Product Item sudah pernah terinput
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + input + " sudah terinput"; }
                }
            }

            // 3. Check Apakah product valid status substoragenya untuk dikirim ke gudang

            return message;
            }
}

    private static string validPG(string input, CheckBoxList cbl, bool isCrossBrand, int id)
    {
        using (Map_DB d = new Map_DB())
        {
            string message = "";

            // 1. Check apakah input string merupakan kode product / ada di database
            var rs1 = d.StockProductPG.SingleOrDefault(p => p.Nomor == input && !p.Draft.Value);

            if (rs1 == null) { message = "Invalid Product"; }
            else
            {
                // 2. Check Apakah Product Item sudah pernah terinput
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + input + " sudah terinput"; }
                }
                if(isCrossBrand)
                {
                    if (rs1.ProductLevel != 11 && !rs1.KodeBarang.Contains("TinyTAN") && rs1.ProductItem != 59 && rs1.CharGoldLevel.NamaKode != "VLEI") 
                    {
                        message = "Barang yang dapat dicross brand hanya barang PGE, TinyTan dan Chain PG75%";
                    }
                    if (rs1.ProductItem == 59 && !Db.SingleBool("select CASE WHEN IDBrandAsal in (3,4,7) AND IDBrandTujuan in (3,4,7) THEN CONVERT(bit, 1, 0) ELSE CONVERT(bit, 0, 0) end from CrossBrand_StockOutgoingPG WHERE ID = " + id, Db.Cnn_DB)) 
                    {
                        message = "Barang Chain PG75% hanya bisa dicross brand antara Frank dan Mondial";
                    }
                }
            }

            // 3. Check Apakah product valid status substoragenya untuk dikirim ke gudang

            return message;
            }
}

    private static string validGJ(string input, CheckBoxList cbl)
    {
        using (Map_DB d = new Map_DB())
        {
            string message = "";

            // 1. Check apakah input string merupakan kode product / ada di database
            var rs1 = d.StockProductGJ.SingleOrDefault(p => p.Nomor == input && !p.Draft.Value);

            if (rs1 == null) { message = "Invalid Product"; }
            else
            {
                // 2. Check Apakah Product Item sudah pernah terinput
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + input + " sudah terinput"; }
                }
            }

            // 3. Check Apakah product valid status substoragenya untuk dikirim ke gudang

            return message;
            }
}

    private static string validLD(string input, CheckBoxList cbl)
    {
        using (Map_DB d = new Map_DB())
        {
            string message = "";

            // 1. Check apakah input string merupakan kode product / ada di database
            var rs1 = d.StockProductLD_Stone1B.SingleOrDefault(p => p.Nomor == input);

            if (rs1 == null) { message = "Invalid Product"; }
            else
            {
                // 2. Check Apakah Product Item sudah pernah terinput
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + input + " sudah terinput"; }
                }
            }

            // 3. Check Apakah product valid status substoragenya untuk dikirim ke gudang

            return message;
            }
}

    public static void FormRegisterUpdateDokumenDraft_DJ_CrossBrandV3(Literal lit_Message, TextBox inputproduct, System.Web.UI.HtmlControls.HtmlButton save, CheckBoxList cbl_Product, int id, string Mode = "")
    {
        using (Map_DB d = new Map_DB())
        {
            Modelize cross = new Modelize();

            /// Master Brand
            var rsBrand = (from p in d.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in d.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in d.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in d.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            var rsH = cross.OutgoingDJ(id).SingleOrDefault();
            int vB = rsH.IDBrandAsal;
            int vF = rsH.TipeAsal;
            int LokasiAsal = rsH.IDAsal;

            lit_Message.Text = "";
            string input = inputproduct.Text.Trim();

            if (input.StartsWith("https://pos-jaws.cmk.co.id"))
            {
                string ShortUrl = input.Substring(input.LastIndexOf('/') + 1);
                string ExtractedPLU = Db.SingleString("SELECT ISNULL(PLU, '') [PLU] FROM mondial_db_real.dbo.QRCatalogJaws WHERE ShortUrl = '" + ShortUrl + "'", Db.Cnn_DB);
                input = ExtractedPLU;
            }

            try
            {
                List<StockActualDJ> list = d.StockActualDJ.Where(p => p.StockProductDJ.Nomor == input
                    && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false).ToList();

                var rs = list.SingleOrDefault(p => !(from z in d.StockOutgoingDJ_Product
                                                     where (z.StockOutgoingDJ.StatusTransOut == 2 || z.StockOutgoingDJ.StatusTransOut == 1) && z.StockOutgoingDJ.StockIncomingDJ.Any(u => u.StatusTransIn == 2)
                                                     select z.IDProduct).Contains(p.IDProduct));

                // CrossBrand_StockOutgoingDJ outgoing = new CrossBrand_StockOutgoingDJ();

                // if (list.Count > 0)
                // {
                //     outgoing = cross.OutgoingDJ().SingleOrDefault(p => p.StatusTransOut != 4 && p.TipeAsal == vF && p.IDAsal == LokasiAsal && p.CrossBrand_StockOutgoingDJ_Product.Any(c => c.Nomor == input));
                // }

                if (rs != null )
                {
                    /// Check Apakah produk sudah pernah diinput (tidak bisa input barang yang sama 2x pada dokumen yang sama)
                    string err_msg = validDJ(input, cbl_Product);

                    /// Check Gambar Produk apakah ada
                    //string curFile = @"" + rs.StockProductDJ.ImgPicture;
                    ////if (!File.Exists(curFile) && vF == 0)
                    //if (1 != 1)
                    //{
                    //    err_msg = "Gambar Product tidak ada.";
                    //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                    //}

                    /// Check Apakah produk sudah pernah terinput di dokumen draft
                    if (Mode != "update")
                    {
                        var rsProductDraft = (from z in d.StockOutgoingDJ_Product where z.StockProductDJ.Nomor == input && z.StockOutgoingDJ.StatusTransOut == 0 select z);
                        if (rsProductDraft.Count() > 0)
                        {
                            err_msg = "Kode Produk <b>" + input + "</b> sudah pernah terinput di dokumen Draft. Silahkan selesaikan / hapus terlebih dahulu dokumen Draft bersangkutan.";
                            err_msg += "<br /><br /><table class='table-list'>";
                            err_msg += "<tr>"
                                    + "<th style='width:200px;'>Operator</th>"
                                    + "<th style='width:150px;'>Tanggal</th>"
                                    + "<th style='width:100px;'>Tipe Asal</th>"
                                    + "<th style='width:200px;'>Nama Asal</th>"
                                    + "<th style='width:100px;'>Tipe Tujuan</th>"
                                    + "<th style='width:200px;'>Nama Tujuan</th>"
                                    + "</tr>";
                            foreach (var drft in rsProductDraft)
                            {
                                string sTipeAsal = (drft.StockOutgoingDJ.TipeAsal == 0) ? "Warehouse"
                                                 : (drft.StockOutgoingDJ.TipeAsal == 1) ? "Outlet"
                                                 : (drft.StockOutgoingDJ.TipeAsal == 2) ? "Exhibition"
                                                 : ""
                                                 ;
                                string sNamaAsal = (drft.StockOutgoingDJ.TipeAsal == 0) ? rsWarehouse[drft.StockOutgoingDJ.IDAsal]
                                                 : (drft.StockOutgoingDJ.TipeAsal == 1) ? rsOutlet[drft.StockOutgoingDJ.IDAsal]
                                                 : (drft.StockOutgoingDJ.TipeAsal == 2) ? rsExhibition[drft.StockOutgoingDJ.IDAsal]
                                                 : ""
                                                 ;
                                string sTipeTujuan = (drft.StockOutgoingDJ.TipeTujuan == 0) ? "Warehouse"
                                                   : (drft.StockOutgoingDJ.TipeTujuan == 1) ? "Outlet"
                                                   : (drft.StockOutgoingDJ.TipeTujuan == 2) ? "Exhibition"
                                                   : ""
                                                   ;

                                string sNamaTujuan = (drft.StockOutgoingDJ.TipeTujuan == 0) ? rsWarehouse[drft.StockOutgoingDJ.IDTujuan]
                                                   : (drft.StockOutgoingDJ.TipeTujuan == 1) ? rsOutlet[drft.StockOutgoingDJ.IDTujuan]
                                                   : (drft.StockOutgoingDJ.TipeTujuan == 2) ? rsExhibition[drft.StockOutgoingDJ.IDTujuan]
                                                   : ""
                                                   ;

                                err_msg += "<tr>";
                                err_msg += "<td>" + drft.StockOutgoingDJ.Operator + "</td>";
                                err_msg += "<td>" + Str.FullDate(drft.StockOutgoingDJ.OperatorTgl) + "</td>";
                                err_msg += "<td>" + sTipeAsal + "</td>";
                                err_msg += "<td>" + sNamaAsal + "</td>";
                                err_msg += "<td>" + sTipeTujuan + "</td>";

                                err_msg += "<td>" + sNamaTujuan + "</td>";
                                err_msg += "<td></td>";
                                err_msg += "</tr>";
                            }
                            err_msg += "</table>";
                        }
                    }

                    /// Jika Tidak ada Error maka insert produk jika tidak tampilkan error message
                    if (err_msg == "")
                    {
                        lit_Message.Text = ""; /// tidak ada error message

                        string v = rs.StockProductDJ.ID.ToString();
                        string t = Logistic.ProductList_DJ(
                                rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                                rs.StockProductDJ.Nomor,
                                rs.StockProductDJ.ImgPicture,
                                DL.SubStorage(rs.Substorage),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                                Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
                                );

                        int jlh = cbl_Product.Items.Count;
                        string[] item = new string[1000];
                        string[] itemvalue = new string[1000];

                        for (int i = 0; i < jlh; i++)
                        {
                            item[i] = cbl_Product.Items[i].Text;
                            itemvalue[i] = cbl_Product.Items[i].Value;
                        }

                        cbl_Product.Items.Clear();

                        cbl_Product.Items.Add(new ListItem(t, v));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items.Add(new ListItem(item[i], itemvalue[i]));
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                        }

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items[i].Selected = true;
                        }

                        //upl.Update();

                        inputproduct.Text = "";
                        inputproduct.Focus();
                    }
                    else
                    {
                        // error message
                        lit_Message.Text = "<div class='alert alert-danger' role='alert'>" + err_msg + "</div>";
                        inputproduct.Focus();
                    }

                }
                else
                {
                    lit_Message.Text = "<span style='color:red;'>Barang sudah terinput / tidak terdapat di cabang ini.</span>";
                    inputproduct.Focus();
                }

    
                save.Visible = cbl_Product.Items.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                lit_Message.Text = "<span style='color:red;'>Barang sudah ter-register, cek menu Approval</span>";
                inputproduct.Focus();
            }
            }
}

    public static void FormRegisterUpdateDokumenDraft_DJ_CrossBrand(Literal lit_Message, TextBox inputproduct, System.Web.UI.HtmlControls.HtmlButton save, CheckBoxList cbl_Product, int id, string Mode = "")
    {
        using (Map_DB d = new Map_DB())
        {
            Modelize cross = new Modelize();

            /// Master Brand
            var rsBrand = (from p in d.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in d.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in d.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in d.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            var rsH = cross.OutgoingDJ(id).SingleOrDefault();
            int vB = rsH.IDBrandAsal;
            int vF = rsH.TipeAsal;
            int LokasiAsal = rsH.IDAsal;

            lit_Message.Text = "";
            string input = inputproduct.Text.Trim();

            if (input.StartsWith("https://pos-jaws.cmk.co.id"))
            {
                string ShortUrl = input.Substring(input.LastIndexOf('/') + 1);
                string ExtractedPLU = Db.SingleString("SELECT ISNULL(PLU, '') [PLU] FROM mondial_db_real.dbo.QRCatalogJaws WHERE ShortUrl = '" + ShortUrl + "'", Db.Cnn_DB);
                input = ExtractedPLU;
            }

            try
            {
                List<StockActualDJ> list = d.StockActualDJ.Where(p => p.StockProductDJ.Nomor == input
                    && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false).ToList();

                var rs = list.SingleOrDefault(p => !(from z in d.StockOutgoingDJ_Product
                                                     where (z.StockOutgoingDJ.StatusTransOut == 2 || z.StockOutgoingDJ.StatusTransOut == 1) && z.StockOutgoingDJ.StockIncomingDJ.Any(u => u.StatusTransIn == 2)
                                                     select z.IDProduct).Contains(p.IDProduct));

                CrossBrand_StockOutgoingDJ outgoing = new CrossBrand_StockOutgoingDJ();

                if (list.Count > 0)
                {
                    outgoing = cross.OutgoingDJ().SingleOrDefault(p => p.StatusTransOut != 4 && p.TipeAsal == vF && p.IDAsal == LokasiAsal && p.CrossBrand_StockOutgoingDJ_Product.Any(c => c.Nomor == input));
                }

                if (rs != null && outgoing == null)
                {
                    /// Check Apakah produk sudah pernah diinput (tidak bisa input barang yang sama 2x pada dokumen yang sama)
                    string err_msg = validDJ(input, cbl_Product);

                    /// Check Gambar Produk apakah ada
                    //string curFile = @"" + rs.StockProductDJ.ImgPicture;
                    ////if (!File.Exists(curFile) && vF == 0)
                    //if (1 != 1)
                    //{
                    //    err_msg = "Gambar Product tidak ada.";
                    //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                    //}

                    /// Check Apakah produk sudah pernah terinput di dokumen draft
                    if (Mode != "update")
                    {
                        var rsProductDraft = (from z in d.StockOutgoingDJ_Product where z.StockProductDJ.Nomor == input && z.StockOutgoingDJ.StatusTransOut == 0 select z);
                        if (rsProductDraft.Count() > 0)
                        {
                            err_msg = "Kode Produk <b>" + input + "</b> sudah pernah terinput di dokumen Draft. Silahkan selesaikan / hapus terlebih dahulu dokumen Draft bersangkutan.";
                            err_msg += "<br /><br /><table class='table-list'>";
                            err_msg += "<tr>"
                                    + "<th style='width:200px;'>Operator</th>"
                                    + "<th style='width:150px;'>Tanggal</th>"
                                    + "<th style='width:100px;'>Tipe Asal</th>"
                                    + "<th style='width:200px;'>Nama Asal</th>"
                                    + "<th style='width:100px;'>Tipe Tujuan</th>"
                                    + "<th style='width:200px;'>Nama Tujuan</th>"
                                    + "</tr>";
                            foreach (var drft in rsProductDraft)
                            {
                                string sTipeAsal = (drft.StockOutgoingDJ.TipeAsal == 0) ? "Warehouse"
                                                 : (drft.StockOutgoingDJ.TipeAsal == 1) ? "Outlet"
                                                 : (drft.StockOutgoingDJ.TipeAsal == 2) ? "Exhibition"
                                                 : ""
                                                 ;
                                string sNamaAsal = (drft.StockOutgoingDJ.TipeAsal == 0) ? rsWarehouse[drft.StockOutgoingDJ.IDAsal]
                                                 : (drft.StockOutgoingDJ.TipeAsal == 1) ? rsOutlet[drft.StockOutgoingDJ.IDAsal]
                                                 : (drft.StockOutgoingDJ.TipeAsal == 2) ? rsExhibition[drft.StockOutgoingDJ.IDAsal]
                                                 : ""
                                                 ;
                                string sTipeTujuan = (drft.StockOutgoingDJ.TipeTujuan == 0) ? "Warehouse"
                                                   : (drft.StockOutgoingDJ.TipeTujuan == 1) ? "Outlet"
                                                   : (drft.StockOutgoingDJ.TipeTujuan == 2) ? "Exhibition"
                                                   : ""
                                                   ;

                                string sNamaTujuan = (drft.StockOutgoingDJ.TipeTujuan == 0) ? rsWarehouse[drft.StockOutgoingDJ.IDTujuan]
                                                   : (drft.StockOutgoingDJ.TipeTujuan == 1) ? rsOutlet[drft.StockOutgoingDJ.IDTujuan]
                                                   : (drft.StockOutgoingDJ.TipeTujuan == 2) ? rsExhibition[drft.StockOutgoingDJ.IDTujuan]
                                                   : ""
                                                   ;

                                err_msg += "<tr>";
                                err_msg += "<td>" + drft.StockOutgoingDJ.Operator + "</td>";
                                err_msg += "<td>" + Str.FullDate(drft.StockOutgoingDJ.OperatorTgl) + "</td>";
                                err_msg += "<td>" + sTipeAsal + "</td>";
                                err_msg += "<td>" + sNamaAsal + "</td>";
                                err_msg += "<td>" + sTipeTujuan + "</td>";

                                err_msg += "<td>" + sNamaTujuan + "</td>";
                                err_msg += "<td></td>";
                                err_msg += "</tr>";
                            }
                            err_msg += "</table>";
                        }
                    }

                    /// Jika Tidak ada Error maka insert produk jika tidak tampilkan error message
                    if (err_msg == "")
                    {
                        lit_Message.Text = ""; /// tidak ada error message

                        string v = rs.StockProductDJ.ID.ToString();
                        string t = Logistic.ProductList_DJ(
                                rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                                rs.StockProductDJ.Nomor,
                                rs.StockProductDJ.ImgPicture,
                                DL.SubStorage(rs.Substorage),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                                Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
                                );

                        int jlh = cbl_Product.Items.Count;
                        string[] item = new string[1000];
                        string[] itemvalue = new string[1000];

                        for (int i = 0; i < jlh; i++)
                        {
                            item[i] = cbl_Product.Items[i].Text;
                            itemvalue[i] = cbl_Product.Items[i].Value;
                        }

                        cbl_Product.Items.Clear();

                        cbl_Product.Items.Add(new ListItem(t, v));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items.Add(new ListItem(item[i], itemvalue[i]));
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                        }

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items[i].Selected = true;
                        }

                        //upl.Update();

                        inputproduct.Text = "";
                        inputproduct.Focus();
                    }
                    else
                    {
                        // error message
                        lit_Message.Text = "<div class='alert alert-danger' role='alert'>" + err_msg + "</div>";
                        inputproduct.Focus();
                    }

                }
                else
                {
                    lit_Message.Text = "<span style='color:red;'>Barang sudah terinput / tidak terdapat di cabang ini.</span>";
                    inputproduct.Focus();
                }

    
                save.Visible = cbl_Product.Items.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                lit_Message.Text = "<span style='color:red;'>Barang sudah ter-register, cek menu Approval</span>";
                inputproduct.Focus();
            }
            }
}

    /*public static void FormRegisterUpdateDokumenDraft_DJ_CrossBrand_V2(Literal lit_Message, TextBox inputproduct, System.Web.UI.HtmlControls.HtmlButton save, CheckBoxList cbl_Product, int id, string Mode = "")
    {
		Modelize cross = new Modelize();
		
        var rsH = cross.OutgoingDJ(id).SingleOrDefault();
        int vB = rsH.IDBrandAsal;
        int vF = rsH.TipeAsal;
        int LokasiAsal = rsH.IDAsal;

        lit_Message.Text = "";
        string input = inputproduct.Text.Trim();

        if (input.StartsWith("https://pos-jaws.cmk.co.id"))
        {
            string ShortUrl = input.Substring(input.LastIndexOf('/') + 1);
            string ExtractedPLU = Db.SingleString("SELECT ISNULL(PLU, '') [PLU] FROM mondial_db_real.dbo.QRCatalogJaws WHERE ShortUrl = '" + ShortUrl + "'", Db.Cnn_DB);
            input = ExtractedPLU;
        }

        try
        {
            using (Map_DB d = new Map_DB())
            {
                /*List<StockActualDJ> list = d.StockActualDJ.Where(p => p.StockProductDJ.Nomor == input
                    && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false).ToList();

                var rs = list.SingleOrDefault(p => !(from z in d.StockOutgoingDJ_Product
                                                     where (z.StockOutgoingDJ.StatusTransOut == 2 || z.StockOutgoingDJ.StatusTransOut == 1) && z.StockOutgoingDJ.StockIncomingDJ.Any(u => u.StatusTransIn == 2)
                                                     select z.IDProduct).Contains(p.IDProduct));

                var rs = d.StockActualDJ.Where(p =>
                    p.StockProductDJ.Nomor == input &&
                    p.TipeLokasi == vF &&
                    p.IDLokasi == LokasiAsal &&
                    p.InHand >= 1 &&
                    p.StockProductDJ.StatusLebur == false &&
                    p.StockProductDJ.ReturnProduct == false &&
                    !d.StockOutgoingDJ_Product
                        .Where(z =>
                            (z.StockOutgoingDJ.StatusTransOut == 1 || z.StockOutgoingDJ.StatusTransOut == 2) &&
                            z.StockOutgoingDJ.StockIncomingDJ.Any(u => u.StatusTransIn == 2))
                        .Select(z => z.IDProduct)
                        .Contains(p.IDProduct))
                .SingleOrDefault();

                App_Code.Model.CrossBrand_StockOutgoingDJ outgoing = new App_Code.Model.CrossBrand_StockOutgoingDJ();

                //if (outgoingProductIds.Count() > 0)
                //{
                    outgoing = cross.OutgoingDJ().SingleOrDefault(p => p.StatusTransOut != 4 && p.TipeAsal == vF && p.IDAsal == LokasiAsal && p.CrossBrand_StockOutgoingDJ_Product.Any(c => c.Nomor == input));
                //}

                if (rs != null && outgoing == null)
                {
                    /// Check Apakah produk sudah pernah diinput (tidak bisa input barang yang sama 2x pada dokumen yang sama)
                    string err_msg = validDJ(input, cbl_Product);

                    /// Jika Tidak ada Error maka insert produk jika tidak tampilkan error message
                    if (err_msg == "")
                    {
                        lit_Message.Text = ""; /// tidak ada error message

                        string v = rs.StockProductDJ.ID.ToString();
                        string t = Logistic.ProductList_DJ(
                                rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                                rs.StockProductDJ.Nomor,
                                rs.StockProductDJ.ImgPicture,
                                DL.SubStorage(rs.Substorage),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                                Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                                Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
                                );

                        int jlh = cbl_Product.Items.Count;
                        string[] item = new string[1000];
                        string[] itemvalue = new string[1000];

                        for (int i = 0; i < jlh; i++)
                        {
                            item[i] = cbl_Product.Items[i].Text;
                            itemvalue[i] = cbl_Product.Items[i].Value;
                        }

                        cbl_Product.Items.Clear();

                        cbl_Product.Items.Add(new ListItem(t, v));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items.Add(new ListItem(item[i], itemvalue[i]));
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                        }

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items[i].Selected = true;
                        }

                        //upl.Update();

                        inputproduct.Text = "";
                        inputproduct.Focus();
                    }
                    else
                    {
                        // error message
                        lit_Message.Text = "<div class='alert alert-danger' role='alert'>" + err_msg + "</div>";
                        inputproduct.Focus();
                    }

                }
                else
                {
                    lit_Message.Text = "<span style='color:red;'>Barang sudah terinput / tidak terdapat di cabang ini.</span>";
                    inputproduct.Focus();
                }
                save.Visible = cbl_Product.Items.Count > 0 ? true : false;
            }
        }
        catch (Exception ex)
        {
            lit_Message.Text = "<span style='color:red;'>Barang sudah ter-register, cek menu Approval</span>";
            inputproduct.Focus();
        }
    }*/

     public static void FormRegisterUpdateDokumenDraft_PG_CrossBrandV3(Literal lit_Message, TextBox inputproduct, System.Web.UI.HtmlControls.HtmlButton save, CheckBoxList cbl_Product, int id)
    {
        using (Map_DB d = new Map_DB())
        {
            Modelize cross = new Modelize();

            /// Master Brand
            var rsBrand = (from p in d.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in d.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in d.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in d.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            var rsH = cross.OutgoingPG(id).SingleOrDefault();
            int vB = rsH.IDBrandAsal;
            int vF = rsH.TipeAsal;
            int LokasiAsal = rsH.IDAsal;

            lit_Message.Text = "";
            string input = inputproduct.Text.Trim();

            if (input.StartsWith("https://pos-jaws.cmk.co.id"))
            {
                string ShortUrl = input.Substring(input.LastIndexOf('/') + 1);
                string ExtractedPLU = Db.SingleString("SELECT ISNULL(PLU, '') [PLU] FROM mondial_db_real.dbo.QRCatalogJaws WHERE ShortUrl = '" + ShortUrl + "'", Db.Cnn_DB);
                input = ExtractedPLU;
            }

            try
            {
                List<StockActualPG> list = d.StockActualPG.Where(p => p.StockProductPG.Nomor == input
                    && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductPG.StatusLebur == false && p.StockProductPG.ReturnProduct == false).ToList();

                var rs = list.SingleOrDefault(p => !(from z in d.StockOutgoingPG_Product
                                                     where (z.StockOutgoingPG.StatusTransOut == 2 || z.StockOutgoingPG.StatusTransOut == 1) && z.StockOutgoingPG.StockIncomingPG.Any(u => u.StatusTransIn == 2)
                                                     select z.IDProduct).Contains(p.IDProduct));

                if (rs != null)
                {
                    /// Check Apakah produk sudah pernah diinput (tidak bisa input barang yang sama 2x pada dokumen yang sama)
                    string err_msg = validPG(input, cbl_Product, true, id);

                    /// Check Gambar Produk apakah ada
                    //string curFile = @"" + rs.StockProductPG.ImgPicture;
                    ////if (!File.Exists(curFile) && vF == 0)
                    //if (1 != 1)
                    //{
                    //    err_msg = "Gambar Product tidak ada.";
                    //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                    //}

                    /// Check Apakah produk sudah pernah terinput di dokumen draft
                    var rsProductDraft = (from z in d.StockOutgoingPG_Product where z.StockProductPG.Nomor == input && z.StockOutgoingPG.StatusTransOut == 0 select z);
                    if (rsProductDraft.Count() > 0)
                    {
                        err_msg = "Kode Produk <b>" + input + "</b> sudah pernah terinput di dokumen Draft. Silahkan selesaikan / hapus terlebih dahulu dokumen Draft bersangkutan.";
                        err_msg += "<br /><br /><table class='table-list'>";
                        err_msg += "<tr>"
                                + "<th style='width:200px;'>Operator</th>"
                                + "<th style='width:150px;'>Tanggal</th>"
                                + "<th style='width:100px;'>Tipe Asal</th>"
                                + "<th style='width:200px;'>Nama Asal</th>"
                                + "<th style='width:100px;'>Tipe Tujuan</th>"
                                + "<th style='width:200px;'>Nama Tujuan</th>"
                                + "</tr>";
                        foreach (var drft in rsProductDraft)
                        {
                            string sTipeAsal = (drft.StockOutgoingPG.TipeAsal == 0) ? "Warehouse"
                                             : (drft.StockOutgoingPG.TipeAsal == 1) ? "Outlet"
                                             : (drft.StockOutgoingPG.TipeAsal == 2) ? "Exhibition"
                                             : ""
                                             ;
                            string sNamaAsal = (drft.StockOutgoingPG.TipeAsal == 0) ? rsWarehouse[drft.StockOutgoingPG.IDAsal]
                                             : (drft.StockOutgoingPG.TipeAsal == 1) ? rsOutlet[drft.StockOutgoingPG.IDAsal]
                                             : (drft.StockOutgoingPG.TipeAsal == 2) ? rsExhibition[drft.StockOutgoingPG.IDAsal]
                                             : ""
                                             ;
                            string sTipeTujuan = (drft.StockOutgoingPG.TipeTujuan == 0) ? "Warehouse"
                                               : (drft.StockOutgoingPG.TipeTujuan == 1) ? "Outlet"
                                               : (drft.StockOutgoingPG.TipeTujuan == 2) ? "Exhibition"
                                               : ""
                                               ;

                            string sNamaTujuan = (drft.StockOutgoingPG.TipeTujuan == 0) ? rsWarehouse[drft.StockOutgoingPG.IDTujuan]
                                               : (drft.StockOutgoingPG.TipeTujuan == 1) ? rsOutlet[drft.StockOutgoingPG.IDTujuan]
                                               : (drft.StockOutgoingPG.TipeTujuan == 2) ? rsExhibition[drft.StockOutgoingPG.IDTujuan]
                                               : ""
                                               ;

                            err_msg += "<tr>";
                            err_msg += "<td>" + drft.StockOutgoingPG.Operator + "</td>";
                            err_msg += "<td>" + Str.FullDate(drft.StockOutgoingPG.OperatorTgl) + "</td>";
                            err_msg += "<td>" + sTipeAsal + "</td>";
                            err_msg += "<td>" + sNamaAsal + "</td>";
                            err_msg += "<td>" + sTipeTujuan + "</td>";

                            err_msg += "<td>" + sNamaTujuan + "</td>";
                            err_msg += "<td></td>";
                            err_msg += "</tr>";
                        }
                        err_msg += "</table>";
                    }

                    /// Jika Tidak ada Error maka insert produk jika tidak tampilkan error message
                    if (err_msg == "")
                    {
                        lit_Message.Text = ""; /// tidak ada error message

                        string v = rs.StockProductPG.ID.ToString();
                        string t = Logistic.ProductList_PG(
                                    rs.StockProductPG.CharProductItem.Nama,
                                    rs.StockProductPG.Nomor,
                                    rs.StockProductPG.ImgPicture,
                                    DL.SubStorage(rs.Substorage),
                                    Str.Number2(rs.StockProductPG.NettoWeight),
                                    Str.Number2(rs.StockProductPG.GrossWeight),
                                    rs.StockProductPG.CharGoldModel.Nama
                                    );

                        int jlh = cbl_Product.Items.Count;
                        string[] item = new string[1000];
                        string[] itemvalue = new string[1000];

                        for (int i = 0; i < jlh; i++)
                        {
                            item[i] = cbl_Product.Items[i].Text;
                            itemvalue[i] = cbl_Product.Items[i].Value;
                        }

                        cbl_Product.Items.Clear();

                        cbl_Product.Items.Add(new ListItem(t, v));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items.Add(new ListItem(item[i], itemvalue[i]));
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                        }

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items[i].Selected = true;
                        }

                        //upl.Update();

                        inputproduct.Text = "";
                        inputproduct.Focus();
                    }
                    else
                    {
                        // error message
                        lit_Message.Text = "<div class='alert alert-danger' role='alert'>" + err_msg + "</div>";
                    }

                }
                else
                {
                    lit_Message.Text = "<span style='color:red;'>Barang sudah terinput / tidak terdapat di cabang ini</span>";
                }

    
                save.Visible = cbl_Product.Items.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                lit_Message.Text = "<span style='color:red;'>Barang sudah ter-register, cek menu Approval</span>";
            }
            }
}

    public static void FormRegisterUpdateDokumenDraft_PG_CrossBrand(Literal lit_Message, TextBox inputproduct, System.Web.UI.HtmlControls.HtmlButton save, CheckBoxList cbl_Product, int id)    
    {
        using (Map_DB d = new Map_DB())
        {
            Modelize cross = new Modelize();

            /// Master Brand
            var rsBrand = (from p in d.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in d.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in d.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in d.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            var rsH = cross.OutgoingPG(id).SingleOrDefault();
            int vB = rsH.IDBrandAsal;
            int vF = rsH.TipeAsal;
            int LokasiAsal = rsH.IDAsal;

            lit_Message.Text = "";
            string input = inputproduct.Text.Trim();

            if (input.StartsWith("https://pos-jaws.cmk.co.id"))
            {
                string ShortUrl = input.Substring(input.LastIndexOf('/') + 1);
                string ExtractedPLU = Db.SingleString("SELECT ISNULL(PLU, '') [PLU] FROM mondial_db_real.dbo.QRCatalogJaws WHERE ShortUrl = '" + ShortUrl + "'", Db.Cnn_DB);
                input = ExtractedPLU;
            }

            try
            {
                List<StockActualPG> list = d.StockActualPG.Where(p => p.StockProductPG.Nomor == input
                    && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductPG.StatusLebur == false && p.StockProductPG.ReturnProduct == false).ToList();

                var rs = list.SingleOrDefault(p => !(from z in d.StockOutgoingPG_Product
                                                     where (z.StockOutgoingPG.StatusTransOut == 2 || z.StockOutgoingPG.StatusTransOut == 1) && z.StockOutgoingPG.StockIncomingPG.Any(u => u.StatusTransIn == 2)
                                                     select z.IDProduct).Contains(p.IDProduct));

                if (rs != null)
                {
                    /// Check Apakah produk sudah pernah diinput (tidak bisa input barang yang sama 2x pada dokumen yang sama)
                    string err_msg = validPG(input, cbl_Product, true, id);

                    /// Check Gambar Produk apakah ada
                    //string curFile = @"" + rs.StockProductPG.ImgPicture;
                    ////if (!File.Exists(curFile) && vF == 0)
                    //if (1 != 1)
                    //{
                    //    err_msg = "Gambar Product tidak ada.";
                    //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                    //}

                    /// Check Apakah produk sudah pernah terinput di dokumen draft
                    var rsProductDraft = (from z in d.StockOutgoingPG_Product where z.StockProductPG.Nomor == input && z.StockOutgoingPG.StatusTransOut == 0 select z);
                    if (rsProductDraft.Count() > 0)
                    {
                        err_msg = "Kode Produk <b>" + input + "</b> sudah pernah terinput di dokumen Draft. Silahkan selesaikan / hapus terlebih dahulu dokumen Draft bersangkutan.";
                        err_msg += "<br /><br /><table class='table-list'>";
                        err_msg += "<tr>"
                                + "<th style='width:200px;'>Operator</th>"
                                + "<th style='width:150px;'>Tanggal</th>"
                                + "<th style='width:100px;'>Tipe Asal</th>"
                                + "<th style='width:200px;'>Nama Asal</th>"
                                + "<th style='width:100px;'>Tipe Tujuan</th>"
                                + "<th style='width:200px;'>Nama Tujuan</th>"
                                + "</tr>";
                        foreach (var drft in rsProductDraft)
                        {
                            string sTipeAsal = (drft.StockOutgoingPG.TipeAsal == 0) ? "Warehouse"
                                             : (drft.StockOutgoingPG.TipeAsal == 1) ? "Outlet"
                                             : (drft.StockOutgoingPG.TipeAsal == 2) ? "Exhibition"
                                             : ""
                                             ;
                            string sNamaAsal = (drft.StockOutgoingPG.TipeAsal == 0) ? rsWarehouse[drft.StockOutgoingPG.IDAsal]
                                             : (drft.StockOutgoingPG.TipeAsal == 1) ? rsOutlet[drft.StockOutgoingPG.IDAsal]
                                             : (drft.StockOutgoingPG.TipeAsal == 2) ? rsExhibition[drft.StockOutgoingPG.IDAsal]
                                             : ""
                                             ;
                            string sTipeTujuan = (drft.StockOutgoingPG.TipeTujuan == 0) ? "Warehouse"
                                               : (drft.StockOutgoingPG.TipeTujuan == 1) ? "Outlet"
                                               : (drft.StockOutgoingPG.TipeTujuan == 2) ? "Exhibition"
                                               : ""
                                               ;

                            string sNamaTujuan = (drft.StockOutgoingPG.TipeTujuan == 0) ? rsWarehouse[drft.StockOutgoingPG.IDTujuan]
                                               : (drft.StockOutgoingPG.TipeTujuan == 1) ? rsOutlet[drft.StockOutgoingPG.IDTujuan]
                                               : (drft.StockOutgoingPG.TipeTujuan == 2) ? rsExhibition[drft.StockOutgoingPG.IDTujuan]
                                               : ""
                                               ;

                            err_msg += "<tr>";
                            err_msg += "<td>" + drft.StockOutgoingPG.Operator + "</td>";
                            err_msg += "<td>" + Str.FullDate(drft.StockOutgoingPG.OperatorTgl) + "</td>";
                            err_msg += "<td>" + sTipeAsal + "</td>";
                            err_msg += "<td>" + sNamaAsal + "</td>";
                            err_msg += "<td>" + sTipeTujuan + "</td>";

                            err_msg += "<td>" + sNamaTujuan + "</td>";
                            err_msg += "<td></td>";
                            err_msg += "</tr>";
                        }
                        err_msg += "</table>";
                    }

                    /// Jika Tidak ada Error maka insert produk jika tidak tampilkan error message
                    if (err_msg == "")
                    {
                        lit_Message.Text = ""; /// tidak ada error message

                        string v = rs.StockProductPG.ID.ToString();
                        string t = Logistic.ProductList_PG(
                                    rs.StockProductPG.CharProductItem.Nama,
                                    rs.StockProductPG.Nomor,
                                    rs.StockProductPG.ImgPicture,
                                    DL.SubStorage(rs.Substorage),
                                    Str.Number2(rs.StockProductPG.NettoWeight),
                                    Str.Number2(rs.StockProductPG.GrossWeight),
                                    rs.StockProductPG.CharGoldModel.Nama
                                    );

                        int jlh = cbl_Product.Items.Count;
                        string[] item = new string[1000];
                        string[] itemvalue = new string[1000];

                        for (int i = 0; i < jlh; i++)
                        {
                            item[i] = cbl_Product.Items[i].Text;
                            itemvalue[i] = cbl_Product.Items[i].Value;
                        }

                        cbl_Product.Items.Clear();

                        cbl_Product.Items.Add(new ListItem(t, v));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items.Add(new ListItem(item[i], itemvalue[i]));
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                        }

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items[i].Selected = true;
                        }

                        //upl.Update();

                        inputproduct.Text = "";
                        inputproduct.Focus();
                    }
                    else
                    {
                        // error message
                        lit_Message.Text = "<div class='alert alert-danger' role='alert'>" + err_msg + "</div>";
                    }

                }
                else
                {
                    lit_Message.Text = "<span style='color:red;'>Barang sudah terinput / tidak terdapat di cabang ini</span>";
                }

    
                save.Visible = cbl_Product.Items.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                lit_Message.Text = "<span style='color:red;'>Barang sudah ter-register, cek menu Approval</span>";
            }
            }
}

    public static void FormRegisterUpdateDokumenDraft_LD_CrossBrandV3(Literal lit_Message, TextBox inputproduct, System.Web.UI.HtmlControls.HtmlButton save, CheckBoxList cbl_Product, int id, string Mode = "")
    {
        using (Map_DB d = new Map_DB())
        {
            Modelize cross = new Modelize();

            /// Master Brand
            var rsBrand = (from p in d.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in d.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in d.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in d.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            var rsH = cross.OutgoingLD(id).SingleOrDefault();
            int vB = rsH.IDBrandAsal;
            int vF = rsH.TipeAsal;
            int LokasiAsal = rsH.IDAsal;

            lit_Message.Text = "";
            string input = inputproduct.Text.Trim();

            try
            {
                List<StockActualLD_Stone1B> list = d.StockActualLD_Stone1B.Where(p => p.StockProductLD_Stone1B.Nomor == input
                    && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductLD_Stone1B.ReturnProduct == false).ToList();

                var rs = list.SingleOrDefault(p => !(from z in d.StockOutgoingLD_Product
                                                     where (z.StockOutgoingLD.StatusTransOut == 2 || z.StockOutgoingLD.StatusTransOut == 1) && z.StockOutgoingLD.StockIncomingLD.Any(u => u.StatusTransIn == 2)
                                                     select z.IDProduct).Contains(p.IDProduct));
            
    
                if (rs != null )
                {
                    /// Check Apakah produk sudah pernah diinput (tidak bisa input barang yang sama 2x pada dokumen yang sama)
                    string err_msg = validLD(input, cbl_Product);

                    /// Check Gambar Produk apakah ada
                    //string curFile = @"" + rs.StockProductDJ.ImgPicture;
                    ////if (!File.Exists(curFile) && vF == 0)
                    //if (1 != 1)
                    //{
                    //    err_msg = "Gambar Product tidak ada.";
                    //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                    //}

                    /// Check Apakah produk sudah pernah terinput di dokumen draft
                    if (Mode != "update")
                    {
                        var rsProductDraft = (from z in d.StockOutgoingLD_Product where z.StockProductLD_Stone1B.Nomor == input && z.StockOutgoingLD.StatusTransOut == 0 select z);
                        if (rsProductDraft.Count() > 0)
                        {
                            err_msg = "Kode Produk <b>" + input + "</b> sudah pernah terinput di dokumen Draft. Silahkan selesaikan / hapus terlebih dahulu dokumen Draft bersangkutan.";
                            err_msg += "<br /><br /><table class='table-list'>";
                            err_msg += "<tr>"
                                    + "<th style='width:200px;'>Operator</th>"
                                    + "<th style='width:150px;'>Tanggal</th>"
                                    + "<th style='width:100px;'>Tipe Asal</th>"
                                    + "<th style='width:200px;'>Nama Asal</th>"
                                    + "<th style='width:100px;'>Tipe Tujuan</th>"
                                    + "<th style='width:200px;'>Nama Tujuan</th>"
                                    + "</tr>";
                            foreach (var drft in rsProductDraft)
                            {
                                string sTipeAsal = (drft.StockOutgoingLD.TipeAsal == 0) ? "Warehouse"
                                                 : (drft.StockOutgoingLD.TipeAsal == 1) ? "Outlet"
                                                 : (drft.StockOutgoingLD.TipeAsal == 2) ? "Exhibition"
                                                 : ""
                                                 ;
                                string sNamaAsal = (drft.StockOutgoingLD.TipeAsal == 0) ? rsWarehouse[drft.StockOutgoingLD.IDAsal]
                                                 : (drft.StockOutgoingLD.TipeAsal == 1) ? rsOutlet[drft.StockOutgoingLD.IDAsal]
                                                 : (drft.StockOutgoingLD.TipeAsal == 2) ? rsExhibition[drft.StockOutgoingLD.IDAsal]
                                                 : ""
                                                 ;
                                string sTipeTujuan = (drft.StockOutgoingLD.TipeTujuan == 0) ? "Warehouse"
                                                   : (drft.StockOutgoingLD.TipeTujuan == 1) ? "Outlet"
                                                   : (drft.StockOutgoingLD.TipeTujuan == 2) ? "Exhibition"
                                                   : ""
                                                   ;

                                string sNamaTujuan = (drft.StockOutgoingLD.TipeTujuan == 0) ? rsWarehouse[drft.StockOutgoingLD.IDTujuan]
                                                   : (drft.StockOutgoingLD.TipeTujuan == 1) ? rsOutlet[drft.StockOutgoingLD.IDTujuan]
                                                   : (drft.StockOutgoingLD.TipeTujuan == 2) ? rsExhibition[drft.StockOutgoingLD.IDTujuan]
                                                   : ""
                                                   ;

                                err_msg += "<tr>";
                                err_msg += "<td>" + drft.StockOutgoingLD.Operator + "</td>";
                                err_msg += "<td>" + Str.FullDate(drft.StockOutgoingLD.OperatorTgl) + "</td>";
                                err_msg += "<td>" + sTipeAsal + "</td>";
                                err_msg += "<td>" + sNamaAsal + "</td>";
                                err_msg += "<td>" + sTipeTujuan + "</td>";

                                err_msg += "<td>" + sNamaTujuan + "</td>";
                                err_msg += "<td></td>";
                                err_msg += "</tr>";
                            }
                            err_msg += "</table>";
                        }
                    }

                    /// Jika Tidak ada Error maka insert produk jika tidak tampilkan error message
                    if (err_msg == "")
                    {
                        lit_Message.Text = ""; /// tidak ada error message

                        string v = rs.StockProductLD_Stone1B.ID.ToString();
                        string t = Logistic.ProductList_LD(
                                rs.StockProductLD_Stone1B.Nomor,
                                rs.StockProductLD_Stone1B.ImgPicture,
                                DL.SubStorage(rs.Substorage),
                                Str.Number3(rs.StockProductLD_Stone1B.TotalCarat)                            
                                );

                        int jlh = cbl_Product.Items.Count;
                        string[] item = new string[1000];
                        string[] itemvalue = new string[1000];

                        for (int i = 0; i < jlh; i++)
                        {
                            item[i] = cbl_Product.Items[i].Text;
                            itemvalue[i] = cbl_Product.Items[i].Value;
                        }

                        cbl_Product.Items.Clear();

                        cbl_Product.Items.Add(new ListItem(t, v));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items.Add(new ListItem(item[i], itemvalue[i]));
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                        }

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items[i].Selected = true;
                        }

                        //upl.Update();

                        inputproduct.Text = "";
                        inputproduct.Focus();
                    }
                    else
                    {
                        // error message
                        lit_Message.Text = "<div class='alert alert-danger' role='alert'>" + err_msg + "</div>";
                    }

                }
                else
                {
                    lit_Message.Text = "<span style='color:red;'>Barang sudah terinput / tidak terdapat di cabang ini.</span>";
                }

    
                save.Visible = cbl_Product.Items.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                lit_Message.Text = "<span style='color:red;'>Barang sudah ter-register, cek menu Approval</span>";
            }
            }
}

    public static void FormRegisterUpdateDokumenDraft_LD_CrossBrand(Literal lit_Message, TextBox inputproduct, System.Web.UI.HtmlControls.HtmlButton save, CheckBoxList cbl_Product, int id, string Mode = "")
    {
        using (Map_DB d = new Map_DB())
        {
            Modelize cross = new Modelize();

            /// Master Brand
            var rsBrand = (from p in d.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in d.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in d.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in d.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            var rsH = cross.OutgoingLD(id).SingleOrDefault();
            int vB = rsH.IDBrandAsal;
            int vF = rsH.TipeAsal;
            int LokasiAsal = rsH.IDAsal;

            lit_Message.Text = "";
            string input = inputproduct.Text.Trim();

            try
            {
                List<StockActualLD_Stone1B> list = d.StockActualLD_Stone1B.Where(p => p.StockProductLD_Stone1B.Nomor == input
                    && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductLD_Stone1B.ReturnProduct == false).ToList();

                var rs = list.SingleOrDefault(p => !(from z in d.StockOutgoingLD_Product
                                                     where (z.StockOutgoingLD.StatusTransOut == 2 || z.StockOutgoingLD.StatusTransOut == 1) && z.StockOutgoingLD.StockIncomingLD.Any(u => u.StatusTransIn == 2)
                                                     select z.IDProduct).Contains(p.IDProduct));

                CrossBrand_StockOutgoingLD outgoing = new CrossBrand_StockOutgoingLD();

                if (list.Count > 0)
                {
                    outgoing = cross.OutgoingLD().SingleOrDefault(p => p.StatusTransOut != 4 && p.TipeAsal == vF && p.IDAsal == LokasiAsal && p.CrossBrand_StockOutgoingLD_Product.Any(c => c.Nomor == input));
                }

                if (rs != null && outgoing == null)
                {
                    /// Check Apakah produk sudah pernah diinput (tidak bisa input barang yang sama 2x pada dokumen yang sama)
                    string err_msg = validLD(input, cbl_Product);

                    /// Check Gambar Produk apakah ada
                    //string curFile = @"" + rs.StockProductDJ.ImgPicture;
                    ////if (!File.Exists(curFile) && vF == 0)
                    //if (1 != 1)
                    //{
                    //    err_msg = "Gambar Product tidak ada.";
                    //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                    //}

                    /// Check Apakah produk sudah pernah terinput di dokumen draft
                    if (Mode != "update")
                    {
                        var rsProductDraft = (from z in d.StockOutgoingLD_Product where z.StockProductLD_Stone1B.Nomor == input && z.StockOutgoingLD.StatusTransOut == 0 select z);
                        if (rsProductDraft.Count() > 0)
                        {
                            err_msg = "Kode Produk <b>" + input + "</b> sudah pernah terinput di dokumen Draft. Silahkan selesaikan / hapus terlebih dahulu dokumen Draft bersangkutan.";
                            err_msg += "<br /><br /><table class='table-list'>";
                            err_msg += "<tr>"
                                    + "<th style='width:200px;'>Operator</th>"
                                    + "<th style='width:150px;'>Tanggal</th>"
                                    + "<th style='width:100px;'>Tipe Asal</th>"
                                    + "<th style='width:200px;'>Nama Asal</th>"
                                    + "<th style='width:100px;'>Tipe Tujuan</th>"
                                    + "<th style='width:200px;'>Nama Tujuan</th>"
                                    + "</tr>";
                            foreach (var drft in rsProductDraft)
                            {
                                string sTipeAsal = (drft.StockOutgoingLD.TipeAsal == 0) ? "Warehouse"
                                                 : (drft.StockOutgoingLD.TipeAsal == 1) ? "Outlet"
                                                 : (drft.StockOutgoingLD.TipeAsal == 2) ? "Exhibition"
                                                 : ""
                                                 ;
                                string sNamaAsal = (drft.StockOutgoingLD.TipeAsal == 0) ? rsWarehouse[drft.StockOutgoingLD.IDAsal]
                                                 : (drft.StockOutgoingLD.TipeAsal == 1) ? rsOutlet[drft.StockOutgoingLD.IDAsal]
                                                 : (drft.StockOutgoingLD.TipeAsal == 2) ? rsExhibition[drft.StockOutgoingLD.IDAsal]
                                                 : ""
                                                 ;
                                string sTipeTujuan = (drft.StockOutgoingLD.TipeTujuan == 0) ? "Warehouse"
                                                   : (drft.StockOutgoingLD.TipeTujuan == 1) ? "Outlet"
                                                   : (drft.StockOutgoingLD.TipeTujuan == 2) ? "Exhibition"
                                                   : ""
                                                   ;

                                string sNamaTujuan = (drft.StockOutgoingLD.TipeTujuan == 0) ? rsWarehouse[drft.StockOutgoingLD.IDTujuan]
                                                   : (drft.StockOutgoingLD.TipeTujuan == 1) ? rsOutlet[drft.StockOutgoingLD.IDTujuan]
                                                   : (drft.StockOutgoingLD.TipeTujuan == 2) ? rsExhibition[drft.StockOutgoingLD.IDTujuan]
                                                   : ""
                                                   ;

                                err_msg += "<tr>";
                                err_msg += "<td>" + drft.StockOutgoingLD.Operator + "</td>";
                                err_msg += "<td>" + Str.FullDate(drft.StockOutgoingLD.OperatorTgl) + "</td>";
                                err_msg += "<td>" + sTipeAsal + "</td>";
                                err_msg += "<td>" + sNamaAsal + "</td>";
                                err_msg += "<td>" + sTipeTujuan + "</td>";

                                err_msg += "<td>" + sNamaTujuan + "</td>";
                                err_msg += "<td></td>";
                                err_msg += "</tr>";
                            }
                            err_msg += "</table>";
                        }
                    }

                    /// Jika Tidak ada Error maka insert produk jika tidak tampilkan error message
                    if (err_msg == "")
                    {
                        lit_Message.Text = ""; /// tidak ada error message

                        string v = rs.StockProductLD_Stone1B.ID.ToString();
                        string t = Logistic.ProductList_LD(
                                rs.StockProductLD_Stone1B.Nomor,
                                rs.StockProductLD_Stone1B.ImgPicture,
                                DL.SubStorage(rs.Substorage),
                                Str.Number3(rs.StockProductLD_Stone1B.TotalCarat)
                                );

                        int jlh = cbl_Product.Items.Count;
                        string[] item = new string[1000];
                        string[] itemvalue = new string[1000];

                        for (int i = 0; i < jlh; i++)
                        {
                            item[i] = cbl_Product.Items[i].Text;
                            itemvalue[i] = cbl_Product.Items[i].Value;
                        }

                        cbl_Product.Items.Clear();

                        cbl_Product.Items.Add(new ListItem(t, v));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items.Add(new ListItem(item[i], itemvalue[i]));
                            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                        }

                        for (int i = 0; i < jlh; i++)
                        {
                            cbl_Product.Items[i].Selected = true;
                        }

                        //upl.Update();

                        inputproduct.Text = "";
                        inputproduct.Focus();
                    }
                    else
                    {
                        // error message
                        lit_Message.Text = "<div class='alert alert-danger' role='alert'>" + err_msg + "</div>";
                    }

                }
                else
                {
                    lit_Message.Text = "<span style='color:red;'>Barang sudah terinput / tidak terdapat di cabang ini.</span>";
                }

    
                save.Visible = cbl_Product.Items.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                lit_Message.Text = "<span style='color:red;'>Barang sudah ter-register, cek menu Approval</span>";
            }
            }
}

}