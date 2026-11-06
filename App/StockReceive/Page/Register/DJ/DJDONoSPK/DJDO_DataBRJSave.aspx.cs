using System;
using System.Linq;
using System.IO;
using System.Configuration;

public partial class Page_Register_DJDO_DataBRJSave : System.Web.UI.Page
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

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (cs != null)
            cs.Dispose();
        base.Dispose();
    }
    //protected int myID { get { return App.QueryInt(this); } }
    //protected int myCMD { get { return App.QueryInt(this); } }
    protected string myNODO { get { return Request.QueryString["nodo"]; } }
    protected string myNOBRG { get { return Request.QueryString["nobrg"]; } }
    protected string myNOVAL { get { return Request.QueryString["noval"]; } }
    protected string myNOBAG { get { return Request.QueryString["nobag"]; } }
	protected string ConStringPabrig { get { return ConfigurationManager.ConnectionStrings["Cnn_MG"].ConnectionString; } }

    //Exception
    protected void DataError() { Response.Redirect("DJDO.aspx?nodo=" + myNODO); }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        var d = cs.T_brj.SingleOrDefault(p => p.NoDOBRJ == myNODO
            && p.NoKodeBarang == myNOBRG
            && p.NoValidasi == myNOVAL
            && p.NoKodeBagian == myNOBAG
            );
        if (d == null) DataError();
        else
        {

            if (d.IDReceive == null)
            {
                var brj_stone = cs.T_brj_stone.Where(p => p.NoDOBRJ == myNODO
                    && p.NoKodeBarang == myNOBRG
                    && p.NoValidasi == myNOVAL
                    && p.NoKodeBagian == myNOBAG
                    );

                var master_stone_1A = (from p in db.Stone1A where p.Draft == false && p.Disable == false select p);
                var master_stone_1B = (from p in db.Stone1B where p.Draft == false && p.Disable == false select p);
                var master_stone_2 = (from p in db.Stone2 where p.Draft == false && p.Disable == false select p);
                var master_stone_3 = (from p in db.Stone3 where p.Draft == false && p.Disable == false select p);
                var master_stone_4 = (from p in db.Stone4 where p.Draft == false && p.Disable == false select p);
                var master_stone_5 = (from p in db.Stone5 where p.Draft == false && p.Disable == false select p);

                //// add Main Data
                int IDWrh = Convert.ToInt32(cs.T_brj_receive.SingleOrDefault(p => p.NoDOBRJ == myNODO).IDWarehouse);
                int IDSpkBrj = Convert.ToInt32(1);

                //Process
                var po = db.ProductOrder.Single(p => p.ID == IDSpkBrj);
                var kkdtl = po.KodeKaretDJ;

                StockReceiveDJ n = new StockReceiveDJ();

                //Location
                n.IDWarehouse = IDWrh;

                //Trans. Form
                n.Sumber = 1; // 1 = SPK
                n.Keterangan = "";
                n.Tgl = System.DateTime.Now;
                //n.Nomor = Generate_Nomor(n);

                //Status
                n.Draft = true;
                n.DraftDate = DateTime.Now;

                //User-Info
                n.Operator = App.UserNama;
                n.OperatorTgl = DateTime.Now;

                string ext = ".jpg";
                bool flagPath = false;
                string path = string.Empty;
                string curFile = "";
                //path += App.ImagesDir;
                //path += "StockReceiveDJ" + "\\";

                // check if image with conditional is exist
                // 1. image with kodekaretdetail + frame color code
                curFile = @path + d.NoKodeBarang.Split('/').First() + po.ProductOrder_CharProduct.CharFrameColor.NamaKode + ext;
                if (File.Exists(curFile) && !flagPath) { n.ImgPicture = curFile; flagPath = true; }

                // 2. image with kodekaretdetail + frame color code (fallback ke white)
                curFile = @path + d.NoKodeBarang.Split('/').First() + "W" + ext;
                if (File.Exists(curFile) && !flagPath) { n.ImgPicture = curFile; flagPath = true; }

                // 3. image with brj + frame color code
                curFile = @path + d.NoKodeBarang.Split('/').First() + po.ProductOrder_CharProduct.CharFrameColor.NamaKode + ext;
                if (File.Exists(curFile) && !flagPath) { n.ImgPicture = curFile; flagPath = true; }

                // 4. image with brj + frame color code (fallback ke white)
                curFile = @path + d.NoKodeBarang.Split('/').First() + "W" + ext;
                if (File.Exists(curFile) && !flagPath) { n.ImgPicture = curFile; flagPath = true; }

                // 5. image with brj (tanpa frame color)
                curFile = @path + d.NoKodeBarang.Split('/').First() + ext;
                if (File.Exists(curFile) && !flagPath) { n.ImgPicture = curFile; flagPath = true; }

                // 6. image with brj (tanpa frame color)
                curFile = @path + d.NoKodeBarang.Split('/').First() + ext;
                if (!flagPath) { n.ImgPicture = curFile; flagPath = true; } // last condition

                //Commit
                //db.CommandTimeout = 5 * 60;
                db.StockReceiveDJ.InsertOnSubmit(n);
                db.SubmitChanges();

                var x = db.StockReceiveDJ.SingleOrDefault(p => p.ID == n.ID && p.Draft);
                if (x == null) DataError();
                else
                {
                    //// add Kode Karet
                    /*var proto = db.KodeKaret_Detail.SingleOrDefault(p => p.IDProductOrder == IDSpkBrj);
                    if (proto == null) DataError();
                    else
                    {
                        x.Proto = proto.ID;
                        x.ProtoNomor = proto.KodeKaretDJ.Nomor;
						//x.Proto = 0;
						//x.ProtoNomor = "";

                        db.SubmitChanges();
                    }

                    //// add product char design
                    StockReceiveDJ_CharDesign n1 = new StockReceiveDJ_CharDesign();
                    var po_cd = po.ProductOrder_CharDesign;

                    n1.ID = n.ID;
                    n1.DesignCategory = po_cd.DesignCategory;
                    n1.DesignConcept = po_cd.DesignConcept;
                    n1.DesignProcess = po_cd.DesignProcess;
                    n1.TargetAge = po_cd.TargetAge;
                    n1.TargetGender = po_cd.TargetGender;
                    n1.Naming = po_cd.Naming;
                    n1.SetItem = po_cd.SetItem;
                    n1.Special = po_cd.Special;
                    n1.Grafir = po_cd.Grafir;

                    //Commit
                    db.StockReceiveDJ_CharDesign.InsertOnSubmit(n1);
                    db.SubmitChanges();

                    //// add product char product
                    StockReceiveDJ_CharProduct n2 = new StockReceiveDJ_CharProduct();
                    var po_cp = po.ProductOrder_CharProduct;
                    decimal brj_gw = d.GrossWeight;
                    decimal brj_sq = d.StoneQty;
                    decimal brj_sc = d.StoneCarat;
                    decimal brj_sw = brj_sc * 0.2m;
                    decimal brj_nw = brj_gw - brj_sw;

                    n2.ID = n.ID;
                    n2.ProductItem = po_cp.ProductItem;
                    n2.ProductCategory = po_cp.ProductCategory;
                    n2.ProductLevel = po_cp.ProductLevel;
                    n2.StoneDist = po_cp.StoneDist;
                    n2.FrameMaterial = po_cp.FrameMaterial;
                    n2.FrameFinishing = po_cp.FrameFinishing;
                    n2.FrameColor = po_cp.FrameColor;
                    n2.ProcessCons = po_cp.ProcessCons;
                    n2.GrossWeight = brj_gw;
                    n2.StoneCarat = brj_sc;
                    n2.StoneWeight = brj_sw;
                    n2.StoneQty = brj_sq;
                    n2.NettoWeight = brj_nw;
                    n2.KadarLogam = 75.50m;
                    n2.KadarTukaran = 84.50m;
                    n2.DimensiD = po_cp.DimensiD;
                    n2.DimensiP = po_cp.DimensiP;
                    n2.DimensiL = po_cp.DimensiL;
                    n2.DimensiR = po_cp.DimensiR;

                    //Commit
                    db.StockReceiveDJ_CharProduct.InsertOnSubmit(n2);
                    db.SubmitChanges();

                    //// add product char finishing
                    int ID_Doff = 1;
                    int ID_Chroom = 7;
                    int ID_Grain = 5;
                    int ID_LP = 8;
                    int ID_LS = 9;
                    int ID_StoneSetting = 10;

                    decimal brj_finishing_total = 0m;

                    if (d.OngkosDoff != 0)
                    {
                        decimal brj_finishing = d.OngkosDoff * 1000;
                        brj_finishing_total += brj_finishing;
                        StockReceiveDJ_Finishing n4 = new StockReceiveDJ_Finishing();

                        n4.IDForm = n.ID;
                        n4.IDFinishing = ID_Doff;
                        n4.Biaya = null;
                        n4.BiayaModal = brj_finishing;
                        n4.Efektif = null;
                        //Commit
                        db.StockReceiveDJ_Finishing.InsertOnSubmit(n4);
                        db.SubmitChanges();
                    }
                    if (d.OngkosChroom != 0)
                    {
                        decimal brj_finishing = d.OngkosChroom * 1000;
                        brj_finishing_total += brj_finishing;
                        StockReceiveDJ_Finishing n4 = new StockReceiveDJ_Finishing();

                        n4.IDForm = n.ID;
                        n4.IDFinishing = ID_Chroom;
                        n4.Biaya = null;
                        n4.BiayaModal = brj_finishing;
                        n4.Efektif = null;
                        //Commit
                        db.StockReceiveDJ_Finishing.InsertOnSubmit(n4);
                        db.SubmitChanges();
                    }
                    if (d.OngkosGrain != 0)
                    {
                        decimal brj_finishing = d.OngkosGrain * 1000;
                        brj_finishing_total += brj_finishing;
                        StockReceiveDJ_Finishing n4 = new StockReceiveDJ_Finishing();

                        n4.IDForm = n.ID;
                        n4.IDFinishing = ID_Grain;
                        n4.Biaya = null;
                        n4.BiayaModal = brj_finishing;
                        n4.Efektif = null;
                        //Commit
                        db.StockReceiveDJ_Finishing.InsertOnSubmit(n4);
                        db.SubmitChanges();
                    }
                    if (d.OngkosLP != 0)
                    {
                        decimal brj_finishing = d.OngkosLP * 1000;
                        brj_finishing_total += brj_finishing;
                        StockReceiveDJ_Finishing n4 = new StockReceiveDJ_Finishing();

                        n4.IDForm = n.ID;
                        n4.IDFinishing = ID_LP;
                        n4.Biaya = null;
                        n4.BiayaModal = brj_finishing;
                        n4.Efektif = null;
                        //Commit
                        db.StockReceiveDJ_Finishing.InsertOnSubmit(n4);
                        db.SubmitChanges();
                    }
                    if (d.OngkosLS != 0)
                    {
                        decimal brj_finishing = d.OngkosLS * 1000;
                        brj_finishing_total += brj_finishing;
                        StockReceiveDJ_Finishing n4 = new StockReceiveDJ_Finishing();

                        n4.IDForm = n.ID;
                        n4.IDFinishing = ID_LS;
                        n4.Biaya = null;
                        n4.BiayaModal = brj_finishing;
                        n4.Efektif = null;
                        //Commit
                        db.StockReceiveDJ_Finishing.InsertOnSubmit(n4);
                        db.SubmitChanges();
                    }
                    if (d.OngkosStoneSetting != 0)
                    {
                        decimal brj_finishing = d.OngkosStoneSetting * 1000;
                        brj_finishing_total += brj_finishing;
                        StockReceiveDJ_Finishing n4 = new StockReceiveDJ_Finishing();

                        n4.IDForm = n.ID;
                        n4.IDFinishing = ID_StoneSetting;
                        n4.Biaya = null;
                        n4.BiayaModal = brj_finishing;
                        n4.Efektif = null;
                        //Commit
                        db.StockReceiveDJ_Finishing.InsertOnSubmit(n4);
                        db.SubmitChanges();
                    }

                    //// add costing
                    StockReceiveDJ_Costing n3 = new StockReceiveDJ_Costing();
                    decimal brj_pb = (d.OngkosPB.HasValue ? d.OngkosPB.Value * 1000 : 0); // ((brj_stone.Count() > 0) ? (brj_stone.Sum(p => p.HPP_R) * 1000) : 0m);
                    decimal brj_no = d.OngkosMountingNO * 1000;
                    decimal brj_ro = d.OngkosMountingRO * 1000;
                    //decimal brj_cogs = d.StoneQty * 1000;

                    n3.ID = n.ID;
                    n3.UnitPrice = 0m;
                    n3.Frame = 0m;
                    n3.MountingNO = brj_no; // NO
                    n3.MountingRO = brj_ro; // RO
                    n3.Pembagi = 10; // default 10
                    n3.Setting = brj_pb; // pasang batu
                    n3.Finishing = brj_finishing_total;
                    n3.OngkosLainCOGS = 0m;
                    n3.OngkosLain = 0m;
                    n3.TotalRate = 0m;
                    n3.TotalRupiah = 0m;
                    n3.TotalBiaya = 0m;

                    //Commit
                    db.StockReceiveDJ_Costing.InsertOnSubmit(n3);
                    db.SubmitChanges();
					*/


                    //// add stone
                    if (brj_stone.Count() > 0)
                    {
                        foreach (var stone in brj_stone)
                        {
                            decimal xButir = stone.Butir;
                            decimal xCarat = stone.Carat;
                            decimal xCaratButir = (xButir > 0 ? (xCarat / xButir) : 0m);
                            string xGIA = stone.GIA;

                            bool has_1A = false;
                            bool has_1B = false;
                            bool has_2 = false;
                            bool has_3 = false;
                            bool has_4 = false;
                            bool has_5 = false;

                            has_1A = master_stone_1A.Any(p => p.Nama == stone.Parcel
                                        && p.Parcel102.SizeMin <= xCaratButir
                                        && p.Parcel102.SizeMax >= xCaratButir);
                            if (!has_1A)
                            {
                                has_1B = master_stone_1B.Any(p => p.Nama == stone.Parcel
                                        && p.Parcel102.SizeMin <= xCaratButir
                                        && p.Parcel102.SizeMax >= xCaratButir
                                        );
                            }
                            if (!has_1A && !has_1B)
                            {
                                has_2 = master_stone_2.Any(p => p.Nama == stone.Parcel
                                        && p.Parcel203.SizeMin <= xCaratButir
                                        && p.Parcel203.SizeMax >= xCaratButir
                                        );
                            }
                            if (!has_1A && !has_1B && !has_2)
                            {
                                has_3 = master_stone_3.Any(p => p.Nama == stone.Parcel
                                        && p.Parcel303.SizeMin <= xCaratButir
                                        && p.Parcel303.SizeMax >= xCaratButir
                                        );
                            }
                            if (!has_1A && !has_1B && !has_2 && !has_3)
                            {
                                has_4 = master_stone_4.Any(p => p.Nama == stone.Parcel
                                        && p.Parcel403.SizeMin <= xCaratButir
                                        && p.Parcel403.SizeMax >= xCaratButir
                                        );
                            }
                            if (!has_1A && !has_1B && !has_2 && !has_3 && !has_4)
                            {
                                has_5 = master_stone_5.Any(p => p.Nama == stone.Parcel);
                            }

                            if (has_1A || has_1B || has_2 || has_3 || has_4 || has_5)
                            {
                                if (has_1A)
                                {
                                    var stone_x = master_stone_1A.OrderByDescending(p => p.Tgl).Where(p => p.Nama == stone.Parcel
                                        && p.Parcel102.SizeMin <= xCaratButir
                                        && p.Parcel102.SizeMax >= xCaratButir
                                        ).First();

                                    StockReceiveDJ_Stone1A stone_add = new StockReceiveDJ_Stone1A();

                                    stone_add.IDForm = n.ID;
                                    stone_add.IDStone = stone_x.ID;
                                    stone_add.TotalButir = Convert.ToInt32(xButir);
                                    stone_add.TotalCarat = xCarat;
                                    stone_add.CaratButir = xCarat / xButir;
                                    stone_add.DimensiP = 0;
                                    stone_add.DimensiL = 0;
                                    stone_add.DimensiT = 0;
                                    stone_add.DimensiL = 0;

                                    //Commit
                                    db.StockReceiveDJ_Stone1A.InsertOnSubmit(stone_add);
                                    db.SubmitChanges();
									
									//Certificate
									string IDNew = Db.SingleString("SELECT TOP 1 ISNULL(CAST(ID AS NVARCHAR(10)),'') FROM StockReceiveDJ_Stone1A WHERE IDForm = '" + n.ID.ToString() + "' AND IDStone = '" + stone_x.ID.ToString() + "' ORDER BY ID DESC ", Db.Cnn_DB);
                                    Db.Execute("UPDATE StockReceiveDJ_Stone1A SET CertificateNumber = '" + xGIA.Trim() + "' " + ((xGIA != "") ? ", TglCertificate = ''" : "") + " WHERE IDForm = '" + n.ID.ToString() + "' AND IDStone = '" + stone_x.ID.ToString() + "' AND ID = '" + IDNew + "' ", Db.Cnn_DB);
                                }
                                else if (has_1B)
                                {
                                    var stone_x = master_stone_1B.OrderByDescending(p => p.Tgl).Where(p => p.Nama == stone.Parcel
                                        && p.Parcel102.SizeMin <= xCaratButir
                                        && p.Parcel102.SizeMax >= xCaratButir
                                        ).First();

                                    StockReceiveDJ_Stone1B stone_add = new StockReceiveDJ_Stone1B();

                                    stone_add.IDForm = n.ID;
                                    stone_add.IDStone = stone_x.ID;
                                    stone_add.TotalButir = Convert.ToInt32(xButir);
                                    stone_add.TotalCarat = xCarat;
                                    stone_add.CaratButir = (xButir > 0 ? (xCarat / xButir) : 0m);
                                    stone_add.DimensiP = 0;
                                    stone_add.DimensiL = 0;
                                    stone_add.DimensiT = 0;
                                    stone_add.DimensiL = 0;

                                    stone_add.StatusTerpasang = 1;
                                    stone_add.GIA = xGIA; //// GIA

                                    //Commit
                                    db.StockReceiveDJ_Stone1B.InsertOnSubmit(stone_add);
                                    db.SubmitChanges();
                                }
                                else if (has_2)
                                {
                                    var stone_x = master_stone_2.OrderByDescending(p => p.Tgl).Where(p => p.Nama == stone.Parcel
                                        && p.Parcel203.SizeMin <= xCaratButir
                                        && p.Parcel203.SizeMax >= xCaratButir
                                        ).First();

                                    StockReceiveDJ_Stone2 stone_add = new StockReceiveDJ_Stone2();

                                    stone_add.IDForm = n.ID;
                                    stone_add.IDStone = stone_x.ID;
                                    stone_add.TotalButir = Convert.ToInt32(xButir);
                                    stone_add.TotalCarat = xCarat;
                                    stone_add.CaratButir = (xButir > 0 ? (xCarat / xButir) : 0m);
                                    stone_add.DimensiP = 0;
                                    stone_add.DimensiL = 0;
                                    stone_add.DimensiT = 0;
                                    stone_add.DimensiL = 0;

                                    //Commit
                                    db.StockReceiveDJ_Stone2.InsertOnSubmit(stone_add);
                                    db.SubmitChanges();
                                }
                                else if (has_3)
                                {
                                    var stone_x = master_stone_3.OrderByDescending(p => p.Tgl).Where(p => p.Nama == stone.Parcel
                                        && p.Parcel303.SizeMin <= xCaratButir
                                        && p.Parcel303.SizeMax >= xCaratButir
                                        ).First();

                                    StockReceiveDJ_Stone3 stone_add = new StockReceiveDJ_Stone3();

                                    stone_add.IDForm = n.ID;
                                    stone_add.IDStone = stone_x.ID;
                                    stone_add.TotalButir = Convert.ToInt32(xButir);
                                    stone_add.TotalCarat = xCarat;
                                    stone_add.CaratButir = (xButir > 0 ? (xCarat / xButir) : 0m);
                                    stone_add.DimensiP = 0;
                                    stone_add.DimensiL = 0;
                                    stone_add.DimensiT = 0;
                                    stone_add.DimensiL = 0;

                                    stone_add.GIA = xGIA; //// GIA

                                    //Commit
                                    db.StockReceiveDJ_Stone3.InsertOnSubmit(stone_add);
                                    db.SubmitChanges();
                                }
                                else if (has_4)
                                {
                                    var stone_x = master_stone_4.OrderByDescending(p => p.Tgl).Where(p => p.Nama == stone.Parcel
                                        && p.Parcel403.SizeMin <= xCaratButir
                                        && p.Parcel403.SizeMax >= xCaratButir
                                        ).First();

                                    StockReceiveDJ_Stone4 stone_add = new StockReceiveDJ_Stone4();

                                    stone_add.IDForm = n.ID;
                                    stone_add.IDStone = stone_x.ID;
                                    stone_add.TotalButir = Convert.ToInt32(xButir);
                                    stone_add.TotalCarat = xCarat;
                                    stone_add.CaratButir = (xButir > 0 ? (xCarat / xButir) : 0m);
                                    stone_add.DimensiP = 0;
                                    stone_add.DimensiL = 0;
                                    stone_add.DimensiT = 0;
                                    stone_add.DimensiL = 0;

                                    //Commit
                                    db.StockReceiveDJ_Stone4.InsertOnSubmit(stone_add);
                                    db.SubmitChanges();
                                }
                                else if (has_5)
                                {
                                    var stone_x = master_stone_5.Where(p => p.Nama == stone.Parcel).First();

                                    StockReceiveDJ_Stone5 stone_add = new StockReceiveDJ_Stone5();

                                    stone_add.IDForm = n.ID;
                                    stone_add.IDStone = stone_x.ID;
                                    stone_add.TotalButir = Convert.ToInt32(xButir);
                                    stone_add.TotalCarat = xCarat;
                                    stone_add.CaratButir = (xButir > 0 ? (xCarat / xButir) : 0m);
                                    stone_add.DimensiP = 0;
                                    stone_add.DimensiL = 0;
                                    stone_add.DimensiT = 0;
                                    stone_add.DimensiL = 0;

                                    //Commit
                                    db.StockReceiveDJ_Stone5.InsertOnSubmit(stone_add);
                                    db.SubmitChanges();
                                }
                            }

                        }
                    }
					

                    d.IDReceive = n.ID;
                    d.Operator = App.UserNama;
                    d.OperatorTgl = System.DateTime.Now;
                    cs.SubmitChanges();
					

                    //Redirect
                    Response.Redirect("DJDO_Detail.aspx?id=" + n.ID + "&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG);
                }

            }
            else
            {
				//d.IDReceive = n.ID;
                d.Operator = App.UserNama;
                d.OperatorTgl = System.DateTime.Now;
                cs.SubmitChanges();
					

                    //Redirect
                    Response.Redirect("DJDO_Detail.aspx?id=" + d.IDReceive + "&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG);
            }

        }
    }

}