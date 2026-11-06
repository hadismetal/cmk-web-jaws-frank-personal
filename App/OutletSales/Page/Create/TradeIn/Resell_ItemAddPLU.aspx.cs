using System;
using System.Linq;

public partial class Page_Register_DJ_CharProduct01 : System.Web.UI.Page
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
    protected int myIDSO { get { return App.QueryInt(this, "so"); } }
    protected int myIDRO { get { return App.QueryInt(this, "ro"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            productitem.Focus();

            //DLC
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);
            DLC.CharFrameMaterial(framematerial);
            DLC.CharFrameFinishing(framefinishing);
            DLC.CharFrameColor(framecolor);
            DLC.CharProcessCons(processcons);

            //DLC-Control
            App.DropDown(save, productitem, productitem_ERR);
            App.DropDown(save, productcategory, productcategory_ERR);
            App.DropDown(save, productlevel, productlevel_ERR);
            App.DropDown(save, framematerial, framematerial_ERR);
            App.DropDown(save, framefinishing, framefinishing_ERR);
            App.DropDown(save, framecolor, framecolor_ERR);
            App.DropDown(save, processcons, processcons_ERR);

            App.NumberFormat(grossweight);

            //Form
            fillForm();
            fillDist(sender, e);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("TradeIn.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.TradeIn.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {

        }
    }
    protected void fillDist(object sender, EventArgs e)
    {
        //Clear-Selection
        stonedist_ERR.Visible = false;
        stonedist.ClearSelection();
        stonedist.Items.Clear();

        //DLC
        if (productcategory.Items.Count > 0 && productlevel.Items.Count > 0)
        {
            int IDCategory = Convert.ToInt32(productcategory.SelectedValue);
            int IDLevel = Convert.ToInt32(productlevel.SelectedValue);
            DLC.SettingsStoneDist(stonedist, IDCategory, IDLevel);
        }
    }

    //Save
    protected bool valid_Dist
    {
        get
        {
            bool x = stonedist.Items.Count > 0;
            stonedist_ERR.Visible = !x;
            return x;
        }
    }

    protected string isValid()
    {
        string result = string.Empty;
        if (string.IsNullOrEmpty(plu.Text))
            result = "PLU tidak boleh kosong.";
        else if (db.StockProductDJ.Any(p => p.Nomor.Equals(plu.Text)))
            result = "PLU sudah terdaftar.";
        return result;
    }

    protected void save_Click(object sender, EventArgs e)
    {
        string isValidate = isValid();
        if (string.IsNullOrEmpty(isValidate))
        {
            if (valid_Dist)
            {
                var x = db.StockProductDJ.SingleOrDefault(p => p.Nomor.Equals(plu.Text) && !p.Draft.Value);
                if (x != null)
                {
                    if (x.StockActualDJ.InHand == 0 && x.Legacy == 1)
                    {
                        var res = db.Resell.SingleOrDefault(p => p.ID == myID);

                        var inv = from p in db.SalesOrderDJ
                                  where p.IDProduct == x.ID
                                  select p;
                        if (inv.Count() > 0)
                        {
                            var so = db.SalesOrder.SingleOrDefault(p => p.ID == inv.First().IDForm);

                            db.SalesOrder.DeleteOnSubmit(so);
                            db.SalesOrderDJ.DeleteAllOnSubmit(inv);
                            db.SubmitChanges();

                        }
                        if (res != null)
                        {

                            // Ledger 
                            StockLedgerDJ l = new StockLedgerDJ();

                            // Value (See Library DB - Ledger for Value and Remark)
                            decimal Value = 1; // 1 : in , -1 : out
                            int Remark = 4; // See Library DB - Ledger
                            string Keterangan = x.Nomor; // Commonly filled by document number or id 
                            if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                            string Lokasi = "";
                            int IDBrand = 1;
                            if (res.TipeLokasi == 0)
                            {
                                Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
                                IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocWarehouse WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
                            }
                            else if (res.TipeLokasi == 1)
                            {
                                Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
                                IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocOutlet WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
                            }
                            else if (res.TipeLokasi == 2)
                            {
                                Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
                                IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocExhibition WHERE ID=" + res.IDLokasi, Db.Cnn_DB);
                            }



                            //Input
                            l.IDProduct = x.ID;
                            l.TipeLokasi = res.TipeLokasi;
                            l.IDLokasi = res.IDLokasi;
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

                            x.StockActualDJ.NamaLokasi = Lokasi;
                            x.StockActualDJ.TipeLokasi = res.TipeLokasi;
                            x.StockActualDJ.IDLokasi = res.IDLokasi;
                            db.SubmitChanges();

                            SalesOrder n = new SalesOrder();

                            //Location
                            n.IDLokasi = res.IDLokasi;
                            n.TipeLokasi = res.TipeLokasi;
                            n.IDCustomer = res.IDCustomer;
                            n.CustomerNama = res.CustomerNama;
                            n.Tgl = DateTime.Today;

                            //Status
                            n.Draft = false;
                            n.DraftDate = null;

                            //User-Info
                            n.Operator = App.UserNama;
                            n.OperatorTgl = DateTime.Now;
                            n.StatusVoid = false;
                            n.StatusPembayaran = true;

                            //Commit
                            db.SalesOrder.InsertOnSubmit(n);
                            db.SubmitChanges();

                            var pr = db.SalesOrder.SingleOrDefault(p => p.ID == n.ID);
                            pr.Nomor = Document.Generate_Nomor_SalesOrder(pr);
                            db.SubmitChanges();

                            SalesOrderDJ dj = new SalesOrderDJ();

                            dj.IDForm = n.ID;
                            dj.IDProduct = x.ID;
                            dj.ModalRupiah = 0m;
                            dj.TotalBayar = 0m;
                            dj.TotalRupiah = 0m;
                            dj.HargaRupiah = 0m;
                            dj.Discount = 0m;
                            dj.DiscountGift = 0m;
                            dj.DiscountNominal = 0m;
                            dj.DiscountProgram = 0m;
                            dj.DiscountProgramNominal = 0m;
                            dj.DiscountPromo = 0m;
                            dj.DiscountRound = 0m;
                            dj.MENominal = 0m;
                            dj.MEPeriodeMax = 0;
                            dj.MEPeriodeMin = 0;
                            dj.MERumus = 0m;
                            dj.R1_Nominal = 0m;
                            dj.R1_PeriodeMax = 0;
                            dj.R1_PeriodeMin = 0;
                            dj.R1_Rumus = 0m;
                            dj.R2_Nominal = 0m;
                            dj.R2_PeriodeMax = 0;
                            dj.R2_PeriodeMin = 0;
                            dj.R2_Rumus = 0m;
                            dj.TI1_Nominal = 0m;
                            dj.TI1_PeriodeMax = 0;
                            dj.TI1_PeriodeMin = 0;
                            dj.TI1_Rumus = 0;
                            dj.TI2_Nominal = 0m;
                            dj.TI2_PeriodeMax = 0;
                            dj.TI2_PeriodeMin = 0;
                            dj.TI2_Rumus = 0;
                            dj.TI3_Nominal = 0m;
                            dj.TI3_PeriodeMax = 0;
                            dj.TI3_PeriodeMin = 0;
                            dj.TI3_Rumus = 0;
                            dj.StatusResell = 0;

                            db.SalesOrderDJ.InsertOnSubmit(dj);
                            db.SubmitChanges();

                            var prdj = db.SalesOrderDJ.SingleOrDefault(p => p.ID == dj.ID);
                            prdj.Invoice = Document.Generate_Nomor_SalesOrder_DJ(prdj);
                            db.SubmitChanges();

                        }

                        Response.Redirect("Resell_ItemAddSave.aspx?id=" + myID + "&item=1&iditem=" + x.ID);
                    }
                    else
                        err_plu.InnerText = "PLU sudah terdaftar";
                }
                else
                {
                    var res = db.Resell.SingleOrDefault(p => p.ID == myID);

                    StockProductDJ n = new StockProductDJ();

                    //User-Info
                    n.Operator = App.UserNama;
                    n.OperatorTgl = DateTime.Now;
                    n.Nomor = plu.Text.Trim();
                    n.Draft = true;
                    n.DraftDate = DateTime.Now;
                    n.Legacy = 1;
                    n.Sumber = 0;
                    n.Tgl = DateTime.Today;
                    n.ReturnProduct = false;
                    n.StatusPenjualan = 2;
                    n.StatusLebur = false;

                    //Commit
                    db.StockProductDJ.InsertOnSubmit(n);
                    db.SubmitChanges();

                    var d = db.StockProductDJ.SingleOrDefault(p => p.ID == n.ID);
                    if (d == null) DataError();
                    else
                    {
                        //Process
                        deleteProcess(d);
                        insertProcess(d);

                        //Redirect
                        Response.Redirect("Resell_DJ_CharDesign.aspx?id=" + myID + "&itemid=" + n.ID + "&ro=" + myIDRO + "&so=" + myIDSO);
                    }
                }
            }
        }
        else
        {
            err_plu.InnerText = isValidate;
        }
    }
    protected void deleteProcess(StockProductDJ ms)
    {
        var d = ms.StockProductDJ_CharProduct;
        if (d != null)
        {
            //Commit
            db.StockProductDJ_CharProduct.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void insertProcess(StockProductDJ ms)
    {
        StockProductDJ_CharProduct n = new StockProductDJ_CharProduct();

        n.ID = ms.ID;

        n.ProductItem = Convert.ToInt32(productitem.SelectedValue);
        n.ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        n.ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        n.StoneDist = Convert.ToInt32(stonedist.SelectedValue);
        n.FrameMaterial = Convert.ToInt32(framematerial.SelectedValue);
        n.FrameFinishing = Convert.ToInt32(framefinishing.SelectedValue);
        n.FrameColor = Convert.ToInt32(framecolor.SelectedValue);
        n.ProcessCons = Convert.ToInt32(processcons.SelectedValue);
        n.GrossWeight = Convert.ToDecimal(grossweight.Text);
        n.KadarLogam = 75.5m;
        n.KadarTukaran = 84.5m;

        //Commit
        db.StockProductDJ_CharProduct.InsertOnSubmit(n);
        db.SubmitChanges();
    }
    
}