using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Linq.Dynamic;


public partial class Page_Approval_CZ : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            //Generate

            DLC.CharGoldLevel(ddl_ProductLevel);
            DLC.CharGoldModelAllFilter(ddl_Model);
            DLC.CharProductItem(ddl_ProductItem);
            DLC.CharFrameColor(ddl_RangkaWarna);
            DLC.DDLSupplier(ddl_Supplier);

            datepicker.Text = Str.Tgl(DateTime.Today);
            datepicker2.Text = Str.Tgl(DateTime.Today);

            Set();
        }
    }

    //Display Table
    protected void Set()
    {

        var rs = (from p in db.StockReceivePG
                  where p.Approval == null && p.NoKirimCZ != null
                  group p by p.NoTTB into g

                  select new { g.Key });

        DateTime frm = Convert.ToDateTime(datepicker.Text);
        DateTime to = Convert.ToDateTime(datepicker2.Text);

        DataTable dtMember = new DataTable("AdditionalMember");
        DataRow drMember;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("id"), new DataColumn("check"), new DataColumn("ratetype"),
            new DataColumn("img"), new DataColumn("kodebarang"), new DataColumn("no_ttb"), new DataColumn("sj_supplier"), new DataColumn("tgl"), new DataColumn("product_level"), new DataColumn("model"), new DataColumn("product_item"), new DataColumn("qty"), new DataColumn("total_berat"), new DataColumn("harga"), new DataColumn("ongkos"), new DataColumn("total_harga"), new DataColumn("total_hargajual"), new DataColumn("total_berat24"), new DataColumn("harga24") });
        var counter = 1;

        decimal TotalHarga = 0m;
        decimal TotalHargaJual = 0m;
        decimal TotalHargaUSD = 0m;
        decimal TotalHargaJualUSD = 0m;
        decimal TotalBerat = 0m;
        decimal Harga = 0m;
        decimal HargaUSD = 0m;
        decimal OngkosM = 0m;
        decimal OngkosMUSD = 0m;
        decimal OngkosR = 0m;
        decimal OngkosRUSD = 0m;
        decimal Berat = 0m;
        decimal Qty = 0m;
        decimal KadarLogam = 0m;
        decimal KadarTukaranJual = 0m;
        decimal KadarTukaranBeli = 0m;
        decimal TotalBerat24 = 0m;
        decimal TotalHarga24 = 0m;

        foreach (var r in rs)
        {
            var det = from p in db.StockReceivePG
                      where p.NoTTB == r.Key
                      select p;

            try
            {
                if (Fv.isDateTime(datepicker, datepicker_ERR) && Fv.isDateTime(datepicker2, datepicker2_ERR))
                    det = det.Where(p => p.Tgl >= frm && p.Tgl < to.AddDays(1));

                Qty = (from x in det
                       select x.ID).Count();



                TotalBerat = (from x in det
                              select x.GrossWeight).Sum();
                Harga = (from x in det
                         select x.Harga.Value).Sum();

                HargaUSD = (from x in det
                            select x.HargaUSD.Value).Sum();

                OngkosM = (from x in det
                           select x.MountingM).Sum();

                OngkosMUSD = (from x in det
                              select x.MountingMUSD.Value).Sum();

                OngkosR = (from x in det
                           select x.MountingR).Sum();

                OngkosRUSD = (from x in det
                              select x.MountingRUSD.Value).Sum();

                TotalBerat24 = (from x in det
                                select x.NettoWeight).Sum();

                TotalHarga24 = (from x in det
                                select x.TotalHarga24).Sum();

                var d = (from x in det
                         select x).First();

                drMember = dtMember.NewRow();
                drMember["number"] = counter;
                //drMember["id"] = r.ID.ToString();
                drMember["ratetype"] = (d.FixRate.Value == true) ? "Fixed Rate" : "TGP";
                //drMember["img"] = "<img src=\"/Images.aspx?path=" + d.ImgPicture + "\" width=\"100\" />";
                //drMember["kodebarang"] = d.KodeBarang;
                drMember["no_ttb"] = r.Key;
                drMember["sj_supplier"] = d.SupplierSJ;
                drMember["tgl"] = Str.FullDate(d.Tgl);
                //drMember["product_level"] = d.CharGoldLevel.Nama;
                //drMember["model"] = d.CharGoldModel.Nama;
                //drMember["product_item"] = d.CharProductItem.Nama;
                drMember["qty"] = Qty.ToString("N0");
                drMember["total_berat"] = TotalBerat.ToString("N2");

                drMember["harga"] = Harga.ToString("N2");
                drMember["ongkos"] = OngkosM.ToString("N2");
                drMember["total_harga"] = (d.TotalHarga).ToString("N2");
                drMember["total_hargajual"] = (d.TotalHargaJual.Value).ToString("N2");
                drMember["total_berat24"] = TotalBerat24.ToString("N2");
                drMember["harga24"] = TotalHarga24.ToString("N2");

                dtMember.Rows.Add(drMember);
                counter++;
            }
            catch (Exception ex)
            {

            }
        }

        EmasGridView.DataSource = dtMember;
        EmasGridView.DataBind();
    }

    protected void btn_display_Click(object sender, EventArgs e)
    {

        var rs = (from p in db.StockReceivePG
                  where p.Approval == null && p.NoKirimCZ !=null
                  group p by p.NoTTB into g

                  select new { g.Key });

        DateTime frm = Convert.ToDateTime(datepicker.Text);
        DateTime to = Convert.ToDateTime(datepicker2.Text);

        DataTable dtMember = new DataTable("AdditionalMember");
        DataRow drMember;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("id"), new DataColumn("check"), new DataColumn("ratetype"), new DataColumn("img"), new DataColumn("kodebarang"), new DataColumn("no_ttb"), new DataColumn("sj_supplier"), new DataColumn("tgl"), new DataColumn("product_level"), new DataColumn("model"), new DataColumn("product_item"), new DataColumn("qty"), new DataColumn("total_berat"), new DataColumn("harga"), new DataColumn("ongkos"), new DataColumn("total_harga"), new DataColumn("total_hargajual"), new DataColumn("total_berat24"), new DataColumn("harga24") });
        var counter = 1;

        decimal TotalHarga = 0m;
        decimal TotalHargaJual = 0m;
        decimal TotalHargaUSD = 0m;
        decimal TotalHargaJualUSD = 0m;
        decimal TotalBerat = 0m;
        decimal Harga = 0m;
        decimal HargaUSD = 0m;
        decimal OngkosM = 0m;
        decimal OngkosMUSD = 0m;
        decimal OngkosR = 0m;
        decimal OngkosRUSD = 0m;
        decimal Berat = 0m;
        decimal Qty = 0m;
        decimal KadarLogam = 0m;
        decimal KadarTukaranJual = 0m;
        decimal KadarTukaranBeli = 0m;
        decimal TotalBerat24 = 0m;
        decimal TotalHarga24 = 0m;

        foreach (var r in rs)
        {
            var det = from p in db.StockReceivePG
                      where p.NoTTB == r.Key
                      select p;

            try
            {
                if (Fv.isDateTime(datepicker, datepicker_ERR) && Fv.isDateTime(datepicker2, datepicker2_ERR))
                    det = det.Where(p => p.Tgl >= frm && p.Tgl < to.AddDays(1));

                if (ddl_ProductLevel.SelectedValue != "" && ddl_ProductLevel.SelectedValue != "0")
                    det = det.Where(p => p.ProductLevel == Convert.ToInt32(ddl_ProductLevel.SelectedValue));
                if (ddl_Model.SelectedValue != "" && ddl_Model.SelectedValue != "0")
                    det = det.Where(p => p.Model == Convert.ToInt32(ddl_Model.SelectedValue));
                if (ddl_RangkaWarna.SelectedValue != "" && ddl_RangkaWarna.SelectedValue != "0")
                    det = det.Where(p => p.FrameColor == Convert.ToInt32(ddl_RangkaWarna.SelectedValue));
                if (ddl_ProductItem.SelectedValue != "" && ddl_ProductItem.SelectedValue != "0")
                    det = det.Where(p => p.ProductItem == Convert.ToInt32(ddl_ProductItem.SelectedValue));
                if (ddl_Supplier.SelectedValue != "" && ddl_Supplier.SelectedValue != "0")
                    det = det.Where(p => p.IDSupplier == Convert.ToInt32(ddl_Supplier.SelectedValue));

                Qty = (from x in det
                       select x.ID).Count();



                TotalBerat = (from x in det
                              select x.GrossWeight).Sum();
                Harga = (from x in det
                         select x.Harga.Value).Sum();

                HargaUSD = (from x in det
                            select x.HargaUSD.Value).Sum();

                OngkosM = (from x in det
                           select x.MountingM).Sum();

                OngkosMUSD = (from x in det
                              select x.MountingMUSD.Value).Sum();

                OngkosR = (from x in det
                           select x.MountingR).Sum();

                OngkosRUSD = (from x in det
                              select x.MountingRUSD.Value).Sum();

                TotalBerat24 = (from x in det
                                select x.NettoWeight).Sum();

                TotalHarga24 = (from x in det
                                select x.TotalHarga24).Sum();

                var d = (from x in det
                         select x).First();

                drMember = dtMember.NewRow();
                drMember["number"] = counter;
                //drMember["id"] = r.ID.ToString();
                drMember["ratetype"] = (d.FixRate.Value == true) ? "Fixed Rate" : "TGP";
                //drMember["img"] = "<img src=\"/Images.aspx?path=" + d.ImgPicture + "\" width=\"100\" />";
                //drMember["kodebarang"] = d.KodeBarang;
                drMember["no_ttb"] = r.Key;
                drMember["sj_supplier"] = d.SupplierSJ;
                drMember["tgl"] = Str.FullDate(d.Tgl);
                //drMember["product_level"] = d.CharGoldLevel.Nama;
                //drMember["model"] = d.CharGoldModel.Nama;
                //drMember["product_item"] = d.CharProductItem.Nama;
                drMember["qty"] = Qty.ToString("N0");
                drMember["total_berat"] = TotalBerat.ToString("N2");

                drMember["harga"] = Harga.ToString("N2");
                drMember["ongkos"] = OngkosM.ToString("N2");
                drMember["total_harga"] = (d.TotalHarga).ToString("N2");
                drMember["total_hargajual"] = (d.TotalHargaJual.Value).ToString("N2");
                drMember["total_berat24"] = TotalBerat24.ToString("N2");
                drMember["harga24"] = TotalHarga24.ToString("N2");

                dtMember.Rows.Add(drMember);
                counter++;
            }
            catch (Exception ex)
            {

            }
        }

        EmasGridView.DataSource = dtMember;
        EmasGridView.DataBind();
    }

    protected void EmasGridView_onRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dtMember = new DataTable("AdditionalMember");
        DataTable dtMember2 = new DataTable("AdditionalMember");

        DataRow drMember, drMember2;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("check"), new DataColumn("id"), new DataColumn("ratetype"), new DataColumn("img"), new DataColumn("kodebarang"), new DataColumn("no_ttb"), new DataColumn("sj_supplier"), new DataColumn("tgl"), new DataColumn("product_level"), new DataColumn("model"), new DataColumn("product_item"), new DataColumn("qty"), new DataColumn("total_berat"), new DataColumn("harga"), new DataColumn("ongkos"), new DataColumn("total_harga"), new DataColumn("total_hargajual"), new DataColumn("total_berat24"), new DataColumn("harga24") });
        dtMember2.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("check"), new DataColumn("id"), new DataColumn("ratetype"), new DataColumn("img"), new DataColumn("kodebarang"), new DataColumn("no_ttb"), new DataColumn("sj_supplier"), new DataColumn("tgl"), new DataColumn("product_level"), new DataColumn("model"), new DataColumn("product_item"), new DataColumn("qty"), new DataColumn("total_berat"), new DataColumn("harga"), new DataColumn("ongkos"), new DataColumn("total_harga"), new DataColumn("total_hargajual"), new DataColumn("total_berat24"), new DataColumn("harga24") });

        var counter = 1;
        int i = 0;
        for (i = 0; i < EmasGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = EmasGridView.Rows[i];

            drMember = dtMember.NewRow();
            drMember["number"] = counter;
            drMember["ratetype"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtRateType")).Text;
            //drMember["img"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("tbImg")).Text;
            //drMember["kodebarang"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("kodebarang")).Text;
            drMember["no_ttb"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtNoTTB")).Text;
            drMember["sj_supplier"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtSJSupplier")).Text;
            drMember["tgl"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtTgl")).Text;
            //drMember["product_level"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtProductLevel")).Text;
            //drMember["model"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtModel")).Text;
            //drMember["product_item"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtProductItem")).Text;
            drMember["qty"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtQty")).Text;
            drMember["total_berat"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalBerat")).Text;

            drMember["harga"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtHarga")).Text;
            drMember["ongkos"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtOngkos")).Text;
            drMember["total_harga"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalHarga")).Text;
            drMember["total_hargajual"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalHargaJual")).Text;
            drMember["total_berat24"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalBerat24")).Text;
            drMember["harga24"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtHarga24")).Text;

            dtMember.Rows.Add(drMember);
            counter++;
        }



        dtMember.Rows[e.RowIndex].Delete();

        EmasGridView.DataSource = dtMember;
        EmasGridView.DataBind();

        counter = 1;
        decimal totalbutir = 0;
        decimal totalongkospb = 0;
        decimal totalhargaparcel = 0;
        i = 0;
        for (i = 0; i < EmasGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = EmasGridView.Rows[i];

            drMember2 = dtMember2.NewRow();
            drMember2["number"] = counter;
            drMember2["ratetype"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtRateType")).Text;
            //drMember2["id"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtID")).Text;
            //drMember2["img"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("tbImg")).Text;
            //drMember2["kodebarang"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("kodebarang")).Text;
            drMember2["no_ttb"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtNoTTB")).Text;
            drMember2["sj_supplier"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtSJSupplier")).Text;
            drMember2["tgl"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtTgl")).Text;
            //drMember2["product_level"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtProductLevel")).Text;
            //drMember2["model"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtModel")).Text;
            //drMember2["product_item"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtProductItem")).Text;
            drMember2["qty"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtQty")).Text;

            drMember2["total_berat"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalBerat")).Text;

            drMember2["harga"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtHarga")).Text;
            drMember2["ongkos"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtOngkos")).Text;
            drMember2["total_harga"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalHarga")).Text;
            drMember2["total_hargajual"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalHargaJual")).Text;
            drMember2["total_berat24"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtTotalBerat24")).Text;
            drMember2["harga24"] = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtHarga24")).Text;

            dtMember2.Rows.Add(drMember2);
            counter++;
        }



        EmasGridView.DataSource = dtMember2;
        EmasGridView.DataBind();
    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {


        for (var i = 0; i < EmasGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = EmasGridView.Rows[i];

            string NoTTB = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtNoTTB")).Text;
            bool Check = ((CheckBox)gridViewRowTemp.Cells[1].FindControl("check")).Checked;

            var rs = from p in db.StockReceivePG
                     where p.NoTTB == "" + NoTTB + "" && p.Approval == null && p.NoKirimCZ !=null
                     select p;

            if (rs != null)
            {
                foreach (var r in rs)
                {

                    if (Check)
                    {
                        r.Approval = App.UserNama;
                        r.ApprovalTgl = DateTime.Now;

                        db.SubmitChanges();

                        StockProductPG x = new StockProductPG();

                        x.NomorSertifikat = r.NomorSertifikat;
                        x.IDBrand = r.LocWarehouse.CompanyBrand.ID;

                        x.KodeBarang = r.KodeBarang;
                        x.NamaBarang = r.NamaBarang;
                        x.Tgl = r.Tgl;
                        x.TglTJT = r.TglTJT;
                        x.NoTTB = r.NoTTB;
                        x.IDWarehouse = r.IDWarehouse;
                        x.Nomor = r.Nomor;
                        x.SupplierNama = r.SupplierNama;
                        x.SupplierSJ = r.SupplierSJ;
                        x.Keterangan = (r.Keterangan != null) ? r.Keterangan : "";
                        x.Draft = r.Draft;
                        x.DraftDate = r.DraftDate;
                        x.Operator = r.Operator;
                        x.OperatorTgl = r.OperatorTgl;
                        x.Approval = r.Approval;
                        x.ApprovalTgl = r.ApprovalTgl;
                        x.CatatanManager = r.CatatanManager;
                        x.ImgPicture = r.ImgPicture;
                        x.ProductLevel = r.ProductLevel;
                        x.Model = r.Model;
                        x.ProductItem = r.ProductItem;
                        x.FrameColor = r.FrameColor;
                        x.TargetAge = r.TargetAge;
                        x.KadarLogam = r.KadarLogam;
                        x.KadarTukaranJual = r.KadarTukaranJual;
                        x.KadarTukaranBeli = r.KadarTukaranBeli;
                        x.GrossWeight = r.GrossWeight;
                        x.NettoWeight = r.NettoWeight;
                        x.MountingM = r.MountingM;
                        x.MountingR = r.MountingR;
                        x.MountingMUSD = r.MountingMUSD;
                        x.MountingRUSD = r.MountingRUSD;
                        x.GoldRate = r.GoldRate;
                        x.EfektifGold = r.EfektifGold;
                        x.Harga = r.Harga;
                        x.HargaUSD = r.HargaUSD;
                        x.TotalHarga = r.TotalHarga;
                        x.TotalHargaJual = r.TotalHargaJual;
                        x.TotalHargaUSD = r.TotalHargaUSD;
                        x.TotalHargaJualUSD = r.TotalHargaJualUSD;
                        x.TotalHarga24 = r.TotalHarga24;
                        x.TotalRate = r.TotalRate;
                        x.HumanStock = r.HumanStock;
                        x.HumanStockNama = r.HumanStockNama;
                        x.EfektifTotalRate = r.EfektifTotalRate;
                        x.IDSupplier = r.IDSupplier;
                        x.FixRate = r.FixRate;
                        x.StatusLebur = false;
                        x.ReturnProduct = false;
                        x.StatusPenjualan = 0;
                        x.DimensiR = (r.DimensiR.HasValue ? r.DimensiR.Value : 0m);

                        db.StockProductPG.InsertOnSubmit(x);
                        db.SubmitChanges();

                        int LastID = Db.SingleInteger("SELECT TOP 1 ID FROM StockProductPG ORDER BY ID DESC", Db.Cnn_DB);

                        // Ledger 
                        StockLedgerPG l = new StockLedgerPG();

                        // Value (See Library DB - Ledger for Value and Remark)
                        decimal Value = 1; // 1 : in , -1 : out
                        int Remark = 4; // See Library DB - Ledger
                        string Keterangan = x.Nomor; // Commonly filled by document number or id 
                        if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                        //Input
                        l.IDProduct = x.ID;
                        l.TipeLokasi = 0;
                        l.IDLokasi = r.IDWarehouse;
                        l.NamaLokasi = r.LocWarehouse.Nama;
                        l.Value = Value;
                        l.Remark = Remark;
                        l.Keterangan = Keterangan;

                        //User-Info
                        l.Operator = App.UserNama;
                        l.OperatorTgl = DateTime.Now;

                        //Commit
                        db.StockLedgerPG.InsertOnSubmit(l);
                        db.SubmitChanges();

                        StockActualPG y = new StockActualPG();

                        y.IDProduct = LastID;
                        y.IDBrand = r.LocWarehouse.CompanyBrand.ID;
                        y.IDLokasi = r.IDWarehouse;
                        y.NamaLokasi = r.LocWarehouse.Nama;
                        y.TipeLokasi = 0;
                        y.Nomor = r.Nomor;
                        y.Keterangan = (r.Keterangan != null) ? r.Keterangan : "";
                        y.InHand = 1;
                        y.Operator = r.Operator;
                        y.Substorage = 1;
                        y.OperatorTgl = r.OperatorTgl.Value;

                        db.StockActualPG.InsertOnSubmit(y);
                        db.SubmitChanges();

                        string query = "exec sp_upcz '" + r.NoKirimCZ + "','" + r.Nomor + "'";
                        Db.Execute(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
                    }
                }
            }

        }
        Response.Redirect("CZ.aspx?d=1");
    }

    protected void btn_reject_Click(object sender, EventArgs e)
    {
        for (var i = 0; i < EmasGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = EmasGridView.Rows[i];

            string NoTTB = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtNoTTB")).Text;
            bool Check = ((CheckBox)gridViewRowTemp.Cells[1].FindControl("check")).Checked;

            var rs = from p in db.StockReceivePG
                     where p.NoTTB == "" + NoTTB + "" && p.Approval == null && p.NoKirimCZ != null
                     select p;

            if (rs != null)
            {
                foreach (var r in rs)
                {
                    if (Check == true)
                    {
                        var del = db.StockReceivePG.Where(p => p.ID == r.ID).Single();
                        if (del != null)
                        {
                            db.StockReceivePG.DeleteOnSubmit(del);
                            db.SubmitChanges();
                        }
                    }
                }
            }

        }
        Response.Redirect("CZ.aspx?d=1");
    }


    protected void EmasGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string com = e.CommandArgument.ToString();

        if (e.CommandName == "Detail")
        {
            Response.Redirect("CZDetail.aspx?ttb=" + com);

        }
    }
}