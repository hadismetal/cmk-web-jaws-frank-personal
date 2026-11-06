using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Configuration;

public partial class Page_Register_DJDO_DJDO_Detail : System.Web.UI.Page
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
    protected int myID;
    protected string myNODO { get { return Request.QueryString["nodo"]; } }
    protected string myNOBRG; 
    protected string myNOVAL; 
    protected string myNOBAG; 
	protected string ConStringPabrig { get { return ConfigurationManager.ConnectionStrings["Cnn_MG"].ConnectionString; } }
    protected string frameColor { get { return Request.QueryString["frm"]; } }
    protected string imgPath;
    protected bool imageChange = false;
    protected decimal pasangBatu;

    protected int conf { get { return App.QueryInt(this, "conf"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.MaintainScrollPositionOnPostBack = true;

        App.Protect();
        App.ProtectRole(Role.Operator);

        var allbrj = cs.T_brj.Where(p => p.NoDOBRJ == myNODO);

        foreach (var product in allbrj)
        {
            myNOBRG = product.NoKodeBarang;
            myNOVAL = product.NoValidasi;
            myNOBAG = product.NoKodeBagian;
            myID = Convert.ToInt32(product.IDReceive);

            var da = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID);
            

            if (!IsPostBack)
            {


                //search_proto.HRef = "DJDO_KodeKaretList.aspx";

                //Form
                fillForm_data01();
                fillForm_Data02();

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

                fillForm_charproduct01();
                fillDist_charproduct01(sender, e);

                //stonedist.SelectedValue = da.StockReceiveDJ_CharProduct.StoneDist.ToString();

                //Number-Format
                App.NumberFormat(gweight);
                App.NumberFormat(kadarlogam);
                App.NumberFormat(kadartukaran);
                App.NumberFormat(dimensid);
                App.NumberFormat(dimensip);
                App.NumberFormat(dimensil);
                App.NumberFormat(dimensir);

                //Form-Default
                kadarlogam.Text = Str.Number2(CV.KadarLogam);
                kadartukaran.Text = Str.Number2(CV.KadarTukaran);

                fillForm_charproduct02();

                //DLC
                DLC.CharDesignCategory(designcategory);
                DLC.CharDesignConcept(designconcept);
                DLC.CharDesignProcess(designprocess);
                DLC.CharTargetAge(targetage);
                DLC.CharTargetGender(targetgender);

                //DLC-Control
                App.DropDown(save, designcategory, designcategory_ERR);
                App.DropDown(save, designconcept, designconcept_ERR);
                App.DropDown(save, designprocess, designprocess_ERR);
                App.DropDown(save, targetage, targetage_ERR);
                App.DropDown(save, targetgender, targetgender_ERR);

                fillForm_chardesign01();
                fillForm_chardesign02();

                //HyperLink
                add.HRef += "?id=" + myID
                            + "&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG
                            ;

                //Form
                //fillForm_stone();

                //Generate
                fillTable_stone(sender, e);

                //Number-Format
                App.NumberFormat(unitprice);
                //App.NumberFormat(frame);
                App.NumberFormat(mountingno);
                App.NumberFormat(mountingro);
                App.NumberFormat(setting);
                //App.NumberFormat(finishing);
                App.NumberFormat(ongkoslaincogs);
                App.NumberFormat(ongkoslain);

                //Form
                //fillForm_costing();

                //Form-Default
                kb.InnerHtml = (App.MaxUpload / 1000).ToString("N0") + " mb";
                imgpicture_format.InnerHtml = Str.ComaExt(Upload.ImageFormat);

                var cp = da.StockReceiveDJ_CharProduct;
                var cd = da.StockReceiveDJ_CharDesign;
                var cos = da.StockReceiveDJ_Costing;

                int IDRec = da.ID;

                var brj_info = cs.T_brj.Single(p => p.IDReceive == IDRec && p.NoDOBRJ == myNODO);

                number_deliveryorder.InnerText = brj_info.NoDOBRJ;
                number_section.InnerText = brj_info.NoKodeBagian;
                number_item.InnerText = brj_info.NoKodeBarang;
                number_validation.InnerText = brj_info.NoValidasi;

                btn_back.HRef += "?nodo=" + brj_info.NoDOBRJ + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG;

                //Response.Redirect("DJDO.aspx?nodo=" + brj_info.NoDOBRJ);

                humanstocknama.Text = App.UserNama;

                var dbrj = cs.T_brj.SingleOrDefault(p => p.NoDOBRJ == myNODO
                && p.NoKodeBarang == myNOBRG
                && p.NoValidasi == myNOVAL
                && p.NoKodeBagian == myNOBAG
                );

                if (dbrj != null)
                {
                    decimal brj_gw = dbrj.GrossWeight;
                    decimal brj_sq = dbrj.StoneQty;
                    decimal brj_sc = dbrj.StoneCarat;
                    decimal brj_sw = brj_sc * 0.2m;
                    decimal brj_nw = brj_gw - brj_sw;
                    // decimal kadarlogamz = 75.50m;
                    // decimal kadartukaranz = 84.50m;

                    gweight.Text = brj_gw.ToString("N2");
                    grossweight.InnerText = brj_gw.ToString("N2");
                    kadarlogam.Text = (75.50m).ToString("N2");
                    kadartukaran.Text = (84.50m).ToString("N2");
                    stonecarat.InnerText = brj_sc.ToString();
                    stoneweight.InnerText = brj_sw.ToString("N2");
                    lbbutir.Text = brj_sq.ToString("N0");
                    nettoweight.InnerText = (brj_nw - brj_sw).ToString("N2");

                    // n.GrossWeight = Convert.ToDecimal(gweight.Text);
                    // n.NettoWeight = n.GrossWeight;
                    // n.KadarLogam = Convert.ToDecimal(kadarlogam.Text);
                    // n.KadarTukaran = Convert.ToDecimal(kadartukaran.Text);

                    // n.StoneCarat = Convert.ToDecimal(stonecarat.InnerText);
                    // n.StoneWeight = Convert.ToDecimal(stoneweight.InnerText);
                    // n.StoneQty = Convert.ToDecimal(lbbutir.Text);

                    //dd product char finishing
                    int ID_Doff = 1;
                    int ID_Chroom = 7;
                    int ID_Grain = 5;
                    int ID_LP = 8;
                    int ID_LS = 9;
                    int ID_StoneSetting = 10;

                    decimal brj_finishing_total = 0m;



                    //// add costing
                    //StockReceiveDJ_Costing n3 = new StockReceiveDJ_Costing();
                    decimal brj_pb = (dbrj.OngkosPB.HasValue ? dbrj.OngkosPB.Value : 0); // ((brj_stone.Count() > 0) ? (brj_stone.Sum(p => p.HPP_R) * 1000) : 0m);
                    decimal brj_ss = dbrj.OngkosStoneSetting;
                    decimal brj_no = dbrj.OngkosMountingNO;
                    decimal brj_ro = dbrj.OngkosMountingRO;
                    //decimal brj_cogs = d.StoneQty * 1000;

                    //n3.ID = n.ID;
                    unitprice.Text = (0m).ToString("N0");
                    //frame.Text = (0m).ToString();
                    mountingno.Text = brj_no.ToString("N0"); // NO
                    mountingro.Text = brj_ro.ToString("N0"); // RO
                    pembagi.Text = (10).ToString(); // default 10
                    setting.Text = (brj_pb).ToString("N0"); // pasang batu
                    pasangBatu = brj_ss;
                    //n3.Finishing = brj_finishing_total;
                    ongkoslaincogs.Text = (0m).ToString("N0");
                    ongkoslain.Text = (0m).ToString("N0");
                    //n3.TotalRate = 0m;
                    //n3.TotalRupiah = 0m;
                    //n3.TotalBiaya = 0m;

                    if (cp != null) {
                        gweight.Text = cp.GrossWeight.ToString("N2");
                        grossweight.InnerText = cp.GrossWeight.ToString("N2");
                        kadarlogam.Text = cp.KadarLogam.ToString("N2");
                        kadartukaran.Text = cp.KadarTukaran.ToString("N2");
                        stonecarat.InnerText = cp.StoneCarat.ToString();
                        stoneweight.InnerText = cp.StoneWeight.ToString("N2");
                        //lbbutir.Text = Str.Number0(cp.CaratButir);
                        nettoweight.InnerText = (cp.GrossWeight - cp.StoneWeight).ToString("N2");
                    }
                    if (cos != null) {
                        unitprice.Text = Convert.ToDecimal(cos.UnitPrice).ToString("N0");
                        pembagi.Text = Convert.ToInt32(cos.Pembagi).ToString("N0");
                        setting.Text = Convert.ToInt32(cos.Setting).ToString("N0");
                        mountingno.Text = Convert.ToDecimal(cos.MountingNO).ToString("N0");
                        mountingro.Text = Convert.ToDecimal(cos.MountingRO).ToString("N0");

                        ongkoslaincogs.Text = Convert.ToDecimal(cos.OngkosLainCOGS).ToString("N0");
                        ongkoslain.Text = Convert.ToDecimal(cos.OngkosLain).ToString("N0");
                    }
                    if (cd != null) {
                        naming.Text = cd.Naming;
                        setitem.Checked = (cd.SetItem == true) ? true : false;
                        special.Checked = (cd.Special == true) ? true : false;
                        grafir.Text = cd.Grafir;
                    }
                }
                if (da.SupplierNama == null) { supplier_ERR.Visible = true; save.Disabled = true; }

                string ext = ".jpg";
                string fc_code = "-";//da.StockReceiveDJ_CharProduct.CharFrameColor.NamaKode;
                string filename = "";
                if (!String.IsNullOrEmpty(frameColor))
                {
                    framecolor.SelectedValue = frameColor;
                }

                if (da.ImgPicture != null)
                {

                    string path = string.Empty;
                    path += App.ImagesDir;
                    path += "StockReceiveDJ" + "\\";
                    if (da.StockReceiveDJ_CharProduct == null)
                    {
                        fc_code = db.CharFrameColor.SingleOrDefault(p => p.ID == Convert.ToInt32(framecolor.SelectedValue)).NamaKode;
                    }
                    else fc_code = da.StockReceiveDJ_CharProduct.CharFrameColor.NamaKode;
                    //var kkdj = db.KodeKaret_Detail.Single(p => p.ID == da.Proto).KodeKaretDJ;

                    // 1
                    //filename = kkdj.Nomor + fc_code + ext;
                    lit_filename_1.Text = filename;
                    if (File.Exists(path + filename))
                    {
                        btn_filename_1.Visible = (conf != 1 && conf != 2 ? true : false);
                        //btn_filename_1.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
                    }
                    else
                    {
                        lit_fileexist_1.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
                        btn_filename_1.Visible = false;
                    }

                    // 2
                    //filename = kkdj.Nomor + "W" + ext;
                    lit_filename_2.Text = filename;
                    if (File.Exists(path + filename))
                    {
                        btn_filename_2.Visible = (conf != 1 && conf != 2 ? true : false);
                        //btn_filename_2.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
                    }
                    else
                    {
                        lit_fileexist_2.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
                        btn_filename_2.Visible = false;
                    }

                    // 3
                    filename = brj_info.NoKodeBarang.Split('/').First().Replace(" ", "") + fc_code + ext;
                    lit_filename_3.Text = filename;
                    if (File.Exists(path + filename))
                    {
                        btn_filename_3.Visible = (conf != 1 && conf != 2 ? true : false);
                        btn_filename_3.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
                    }
                    else
                    {
                        lit_fileexist_3.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
                        btn_filename_3.Visible = false;
                    }

                    string fc_lama = Db.SingleString("SELECT TOP 1 KodeLama FROM CharFrameColor WHERE ID = " + Convert.ToInt32(framecolor.SelectedValue) + " ", Db.Cnn_DB);
                    filename = brj_info.NoKodeBarang.Split('/').First().Replace(" ", "") + fc_lama + ".jpg";
                    lit_filename_4.Text = filename;
                    if (File.Exists(path + filename))
                    {
                        btn_filename_4.Visible = (conf != 1 && conf != 2 ? true : false);
                        btn_filename_4.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
                    }
                    else
                    {
                        lit_fileexist_4.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
                        btn_filename_4.Visible = false;
                    }

                    // 5
                    filename = brj_info.NoKodeBarang.Split('/').First() + ext;
                    lit_filename_5.Text = filename;
                    if (File.Exists(path + filename))
                    {
                        btn_filename_5.Visible = (conf != 1 && conf != 2 ? true : false);
                        btn_filename_5.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
                    }
                    else
                    {
                        lit_fileexist_5.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
                        btn_filename_5.Visible = false;
                    }

                    // 6
                    //filename = kkdj.Nomor + ext;
                    // string fc_lama = Db.SingleString("SELECT TOP 1 KodeLama FROM CharFrameColor WHERE ID = " + Convert.ToInt32(framecolor.SelectedValue) + " ", Db.Cnn_DB);
                    // filename = brj_info.NoKodeBarang.Split('/').First() + fc_lama + ".jpg";
                    lit_filename_6.Text = filename;
                    if (File.Exists(path + filename))
                    {
                        btn_filename_6.Visible = (conf != 1 && conf != 2 ? true : false);
                        btn_filename_6.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
                    }
                    else
                    {
                        lit_fileexist_6.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
                        btn_filename_6.Visible = false;
                    }

                    path = da.ImgPicture;
                    string file = path.Split('\\').Last();
                    Upload.Image(imgpicture1, path);
                    img_filename.InnerText = file;

                    if (!File.Exists(path))
                    {
                        div_pic.Visible = false;
                        img_ERR.Visible = true;
                        imgpicture1.Visible = false;
                    }
                    else
                    {
                        div_pic.Visible = true;
                        img_ERR.Visible = false;
                        imgpicture1.Visible = true;
                    }
                }
                else
                {
                    string path = string.Empty;
                    path += App.ImagesDir;
                    path += "StockReceiveDJ" + "\\";
                    bool flagPath = false;

                    //var kkdj = db.KodeKaret_Detail.Single(p => p.ID == da.Proto).KodeKaretDJ;
                    fc_code = db.CharFrameColor.SingleOrDefault(p => p.ID == Convert.ToInt32(framecolor.SelectedValue)).NamaKode;
                    // 1
                    //filename = brj_info.NoKodeBarang.Split('/').First() + fc_code + ext;
                    lit_filename_1.Text = filename;
                    if (File.Exists(path + filename) && !flagPath)
                    {
                        btn_filename_1.Visible = (conf != 1 && conf != 2 ? true : false);
                        // flagPath = true;
                        // imgPath = path + filename;
                        // btn_filename_1.Disabled = ((path + filename) == da.ImgPicture) ? true : false;

                        filename = string.Empty;
                    }
                    else
                    {
                        lit_fileexist_1.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
                        btn_filename_1.Visible = false;
                        filename = string.Empty;
                    }

                    // 2
                    //filename = brj_info.NoKodeBarang.Split('/').First() + "W" + ext;
                    lit_filename_2.Text = filename;
                    if (File.Exists(path + filename) && !flagPath)
                    {
                        btn_filename_2.Visible = (conf != 1 && conf != 2 ? true : false);
                        // flagPath = true;
                        // imgPath = path + filename;
                        // btn_filename_2.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
                    }
                    else
                    {
                        lit_fileexist_2.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
                        btn_filename_2.Visible = false;
                        filename = string.Empty;
                    }

                    // 3
                    filename = brj_info.NoKodeBarang.Split('/').First().Replace(" ", "") + fc_code + ext;
                    lit_filename_3.Text = filename;
                    if (File.Exists(path + filename) && !flagPath)
                    {
                        btn_filename_3.Visible = (conf != 1 && conf != 2 ? true : false);
                        flagPath = true;
                        imgPath = path + filename;
                        btn_filename_3.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
                        filename = string.Empty;
                    }
                    else
                    {
                        lit_fileexist_3.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
                        btn_filename_3.Visible = false;
                        filename = string.Empty;
                    }

                    // 4
                    string fc_lama = Db.SingleString("SELECT TOP 1 KodeLama FROM CharFrameColor WHERE ID = " + Convert.ToInt32(framecolor.SelectedValue) + " ", Db.Cnn_DB);
                    filename = brj_info.NoKodeBarang.Split('/').First().Replace(" ", "") + fc_lama + ".jpg";
                    lit_filename_4.Text = filename;
                    if (File.Exists(path + filename) && !flagPath)
                    {
                        btn_filename_4.Visible = (conf != 1 && conf != 2 ? true : false);
                        flagPath = true;
                        imgPath = path + filename;
                        btn_filename_4.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
                        filename = string.Empty;
                    }
                    else
                    {
                        lit_fileexist_4.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
                        btn_filename_4.Visible = false;
                        filename = string.Empty;
                    }

                    // 5
                    filename = brj_info.NoKodeBarang.Split('/').First() + ext;
                    lit_filename_5.Text = filename;
                    if (File.Exists(path + filename) && !flagPath)
                    {
                        btn_filename_5.Visible = (conf != 1 && conf != 2 ? true : false);
                        flagPath = true;
                        imgPath = path + filename;
                        btn_filename_5.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
                        filename = string.Empty;
                    }
                    else
                    {
                        lit_fileexist_5.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
                        btn_filename_5.Visible = false;
                        filename = string.Empty;
                    }

                    // 6
                    //filename = brj_info.NoKodeBarang.Split('/') + ext;
                    // string fc_lama = Db.SingleString("SELECT TOP 1 KodeLama CharFrameColor FROM WHERE ID = " + Convert.ToInt32(framecolor.SelectedValue) + " ", Db.Cnn_DB);
                    // filename = brj_info.NoKodeBarang.Split('/').First() + fc_lama + ".jpg";
                    lit_filename_6.Text = filename;
                    if (File.Exists(path + filename) && !flagPath)
                    {
                        btn_filename_6.Visible = (conf != 1 && conf != 2 ? true : false);
                        // flagPath = true;
                        // imgPath = path + filename;
                        // btn_filename_6.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
                    }
                    else
                    {
                        lit_fileexist_6.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
                        btn_filename_6.Visible = false;
                        filename = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(imgPath))
                    {
                        path = imgPath;
                        string file = path.Split('\\').Last();
                        Upload.Image(imgpicture1, path);
                        img_filename.InnerText = file;

                        if (!File.Exists(path))
                        {
                            div_pic.Visible = false;
                            img_ERR.Visible = true;
                            imgpicture1.Visible = false;
                        }
                        else
                        {
                            div_pic.Visible = true;
                            img_ERR.Visible = false;
                            imgpicture1.Visible = true;
                        }
                    }
                }

                //lit_test_msg.Text += "<br />----123123123test.conf_" + conf.ToString();

                if (conf == 1 || conf == 2)
                {
                    add.Visible = false;

                    save.Visible = false;
                    div_confirm.Visible = ((conf == 2) ? false : true);

                    btn_framecolor_update.Visible = false;

                    pembagi.Enabled = false;

                    requestcustomernama.Text = da.RequestCustomerNama;
                    requestcustomernama.Enabled = false;
                    humandesignernama.Text = da.HumanDesignerNama;
                    humandesignernama.Enabled = false;
                    humanstocknama.Text = da.HumanStockNama;
                    humanstocknama.Enabled = false;

                    search_supplier.Disabled = true;
                    suppliernama.Text = da.SupplierNama;
                    suppliernama.Enabled = false;

                    suppliernomor.Text = da.SupplierNomor;
                    suppliernomor.Enabled = false;
                    expiredproduct.Text = Str.Tgl(da.ExpiredProduct);
                    expiredproduct.Enabled = false;
                    expiredrequest.Text = Str.Tgl(da.ExpiredRequest);
                    expiredrequest.Enabled = false;

                    productitem.Enabled = false;
                    productcategory.Enabled = false;
                    productlevel.Enabled = false;
                    stonedist.Enabled = false;
                    framematerial.Enabled = false;
                    framefinishing.Enabled = false;
                    framecolor.Enabled = false;
                    processcons.Enabled = false;

                    gweight.Text = Convert.ToDecimal(cp.GrossWeight).ToString("N2");
                    gweight.Enabled = false;
                    kadarlogam.Enabled = false;
                    kadartukaran.Enabled = false;
                    dimensid.Enabled = false;
                    dimensip.Enabled = false;
                    dimensil.Enabled = false;
                    dimensip.Enabled = false;
                    dimensir.Enabled = false;

                    designcategory.Enabled = false;
                    designconcept.Enabled = false;
                    designprocess.Enabled = false;
                    targetage.Enabled = false;
                    targetgender.Enabled = false;

                    naming.Text = cd.Naming;
                    naming.Enabled = false;
                    setitem.Checked = (cd.SetItem == true) ? true : false;
                    setitem.Enabled = false;
                    special.Checked = (cd.Special == true) ? true : false;
                    special.Enabled = false;
                    grafir.Text = cd.Grafir;
                    grafir.Enabled = false;

                    mountingno.Text = Str.Number0(cos.MountingNO);
                    mountingno.Enabled = false;
                    mountingro.Text = Str.Number0(cos.MountingRO);
                    mountingro.Enabled = false;
                    gweight.Text = Str.Number2(cp.GrossWeight);
                    grossweight.InnerText = Str.Number2(cp.GrossWeight);
                    kadarlogam.Text = Str.Number2(cp.KadarLogam);
                    kadartukaran.Text = Str.Number2(cp.KadarTukaran);
                    stonecarat.InnerText = Str.Number2(cp.StoneCarat);
                    stoneweight.InnerText = Str.Number2(cp.StoneWeight);
                    //lbbutir.Text = Str.Number0(cp.CaratButir);
                    nettoweight.InnerText = Str.Number2(cp.NettoWeight);
                    unitprice.Text = Str.Number2(cos.UnitPrice);
                    pembagi.Text = Str.Number0(cos.Pembagi);
                    setting.Text = cos.Setting.ToString("N0");
                    setting.Enabled = false;
                    ongkoslaincogs.Text = Convert.ToDecimal(cos.OngkosLainCOGS).ToString("N0");
                    ongkoslaincogs.Enabled = false;
                    ongkoslain.Text = Convert.ToDecimal(cos.OngkosLain).ToString("N0");
                    ongkoslain.Enabled = false;

                    //finishing_tr.Enabled = false;

                    imgpicture.Enabled = false;
                    btn_Picture_save.Disabled = true;
                    btn_filename_1.Disabled = true;
                    btn_filename_2.Disabled = true;
                    btn_filename_3.Disabled = true;
                    btn_filename_4.Disabled = true;
                    btn_filename_5.Disabled = true;
                    btn_filename_6.Disabled = true;
                }
                else
                {

                    /*naming.Text = cd.Naming;
                    suppliernomor.Text = brj_info.NoKodeBarang;
                    mountingno.Text = Str.Number0(cos.MountingNO);
                    mountingro.Text = Str.Number0(cos.MountingRO);
                    setting.Text = cos.Setting.ToString("N0");
                    ongkoslaincogs.Text = Convert.ToDecimal(cos.OngkosLainCOGS).ToString("N0");
                    ongkoslain.Text = Convert.ToDecimal(cos.OngkosLain).ToString("N0");*/
                }


                //stonedist.SelectedValue = da.StockReceiveDJ_CharProduct.StoneDist.ToString();

                //lit_test_msg.Text += "<br />----123123123test.stonedist_" + da.StockReceiveDJ_CharProduct.StoneDist.ToString();


                set_default_finishing_initial_value(); //// tidak jadi default chroom 0

            }
            else
            {
                //var cek = da;
                //if (cek == null) Data_Error();
                //else
                //{
                //    if (cek.IDMounting == null)
                //        DLC.CharProcessFinishingModal2(tb_finishing, cek.ID);
                //    //else
                //    //    DLC.CharProcessFinishingMountingModal(finishing_tr, cek.IDMounting.Value);
                //}

                //saveFinishing_costing(da);
            }
            confirm_Click(sender, e);
            save_Click(sender, e);
        }
        Response.Redirect("DJDO.aspx?nodo=" + myNODO);
    }

    protected void set_default_finishing_initial_value()
    {
        //// tidak jadi default chroom 0
        if (conf == 0)
        {
            var fns = (from p in db.CharProcessFinishing
                       //where 1==1
                       //     && (p.ID == 7 && p.Nama.ToLower() == "chroom")
                       orderby p.SN ascending
                       select p.ID).ToList().ToArray();

            int index = 0;
			
			var dbrj = cs.T_brj.SingleOrDefault(p => p.NoDOBRJ == myNODO
            && p.NoKodeBarang == myNOBRG
            && p.NoValidasi == myNOVAL
            && p.NoKodeBagian == myNOBAG
            );

            foreach (TableRow row in tb_finishing.Rows)
            {


                //lit_test_msg.Text += "<br />----123123123test.set_default_finishing_initial_value_" + index.ToString();
                //lit_test_msg.Text += "<br />----123123123test.set_default_finishing_initial_value_" + fns[index].ToString();

                if (fns[index] == 7)
                {
                    TextBox txt = ((TextBox)row.FindControl("Finishing_" + fns[index]));
                    txt.Text = dbrj.OngkosDoff.ToString("N0");
                    //txt.Text = (txt.Text.Trim() == "" ? "0" : txt.Text.Trim());
					//txt.Text = "";
                }
				if (fns[index] == 1)
				{
					TextBox txt = ((TextBox)row.FindControl("Finishing_" + fns[index]));
					//txt.Text = dbrj.OngkosDoff.ToString("N0");
				}
				if (fns[index] == 8)
				{
					TextBox txt = ((TextBox)row.FindControl("Finishing_" + fns[index]));
					//txt.Text = dbrj.OngkosLainLain.ToString("N0");
				}
                if (fns[index] == 10)
				{
					TextBox txt = ((TextBox)row.FindControl("Finishing_" + fns[index]));
					txt.Text = pasangBatu.ToString("N0");
				}

                index++;
            }
        }
    }

    protected void Data_Error() { Response.Redirect("DJDO_Detail_Warehouse.aspx?id=" + myID + "&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG + "&err=data"); }
    protected void LinkError1() { Response.Redirect("DJDO_Detail_Warehouse.aspx?id=" + myID + "&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG + "&err=linkcd"); }
    protected void LinkError() { Response.Redirect("DJDO_Detail_Warehouse.aspx?id=" + myID + "&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG + "&err=link"); }
    protected void Data_Finished() { Response.Redirect("DJDO_Detail_Warehouse.aspx?id=" + myID + "&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG + "&conf=2"); }

    protected void ProductError() { Response.Redirect("DJDO.aspx?id=" + myID + "&nodo=" + myNODO + "&err=product"); }
    protected void fillForm_data01()
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && (p.Draft || (!p.Draft && conf == 2)));
		string qstr = "?id=" + myID +"&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG;
		search_supplier.HRef += qstr;
		if (d != null) 
		{
			var dbrj = cs.T_brj.SingleOrDefault(p => p.NoDOBRJ == myNODO
            && p.NoKodeBarang == myNOBRG
            && p.NoValidasi == myNOVAL
            && p.NoKodeBagian == myNOBAG
            );
			
			protonomor.Text = (String.IsNullOrEmpty(d.ProtoNomor)) ? dbrj.NoKodeBarang : d.ProtoNomor ;
			protonomor.Enabled = false;
            if(string.IsNullOrEmpty(protonomor.Text.Replace(" ", "")))
            {
                protonomor.Text = Db.SingleString("SELECT TOP 1 m_kodemat from t_brj where NoValidasi = '"+ myNOVAL +"' ", ConfigurationManager.ConnectionStrings["Cnn_CS"].ConnectionString);
            }
			productordernomor.Text = myNODO;
			productordernomor.Enabled = false;
			
			string supplier = string.Empty;
            if(conf != 1)
            {
                dbrj.NoKodeBarang = string.IsNullOrEmpty(dbrj.NoKodeBarang.Replace(" ", "")) ? Db.SingleString("SELECT TOP 1 m_kodemat from t_brj where NoValidasi = '" + myNOVAL + "' ", ConfigurationManager.ConnectionStrings["Cnn_CS"].ConnectionString) : dbrj.NoKodeBarang;
                supplier = dbrj.NoValidasi;
            }
			suppliernomor.Text = (String.IsNullOrEmpty(d.SupplierNomor)) ? supplier : d.SupplierNomor;
			suppliernomor.Enabled = false;
			suppliernama.Text = d.SupplierNama;
			requestoutletnama.Text = d.RequestOutletNama;
			requestcustomernama.Text = d.RequestCustomerNama;
			expiredrequest.Text = Str.Tgl(d.ExpiredRequest);
			expiredproduct.Text = Str.Tgl(d.ExpiredProduct);
		}
		else
		{
            try
            {
                protonomor.Text = Db.SingleString("SELECT TOP 1 ISNULL(m_kodebarang,'') FROM sk_order where m_kodebarang like '%" + myNOBRG + "%' ", ConStringPabrig);
            }
            catch { }
            try
            {
                productordernomor.Text = Db.SingleString("SELECT TOP 1 ISNULL(m_validasi,'') FROM sk_dobrj2 where m_validasi = '" + myNOVAL + "' ", ConStringPabrig);
            }
            catch { }
            try
            {
                suppliernomor.Text = Db.SingleString("SELECT TOP 1 ISNULL(m_nomor,'') FROM sk_dobrj where m_nomor = '" + myNODO + "' ", ConStringPabrig);
            }
            catch { }
        }
    }

    protected void fillForm_Data02()
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && (p.Draft || (!p.Draft && conf == 2)));
		if (d != null) 
		{
			humandesignernama.Text = d.HumanDesignerNama;
			humanstocknama.Text = d.HumanStockNama;
            expiredproduct.Text = Str.Tgl(d.Tgl.AddYears(1));
		}
		else
		{
			humandesignernama.Text = "";
			humanstocknama.Text = "";
			expiredproduct.Text = Str.Tgl(DateTime.Now);
			expiredrequest.Text = Str.Tgl(DateTime.Now);
		}
    }

    protected void fillForm_charproduct01()
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && (p.Draft || (!p.Draft && conf == 2)));
		if (d != null) 
		{
			var x = d.StockReceiveDJ_CharProduct;
			if(x != null)
			{
				productitem.SelectedValue = d.StockReceiveDJ_CharProduct.ProductItem.ToString();
				productcategory.SelectedValue = d.StockReceiveDJ_CharProduct.ProductCategory.ToString();
				productlevel.SelectedValue = d.StockReceiveDJ_CharProduct.ProductLevel.ToString();
				framematerial.SelectedValue = d.StockReceiveDJ_CharProduct.FrameMaterial.ToString();
				framefinishing.SelectedValue = d.StockReceiveDJ_CharProduct.FrameFinishing.ToString();
				framecolor.SelectedValue = d.StockReceiveDJ_CharProduct.FrameColor.ToString();
				processcons.SelectedValue = d.StockReceiveDJ_CharProduct.ProcessCons.ToString();
				stonedist.SelectedValue = d.StockReceiveDJ_CharProduct.StoneDist.ToString();
			}
			else
			{
                try
                {
                    DataTable dt1 = new DataTable();
                    dt1 = Db.Rs("SELECT TOP 1 m_item, m_kategori, m_stonedist, m_productlevel FROM sk_order where m_nomor = '" + myNOVAL + "' ", ConStringPabrig);
                    string framemat = "";
                    framemat = Db.SingleString("SELECT TOP 1 m_material FROM sk_uploadpro where m_nopro = '" + myNOVAL + "' ", ConStringPabrig);
                    if (dt1.Rows.Count == 1){
						try{
						productitem.SelectedValue = db.CharProductItem.SingleOrDefault(p => p.NamaKode == dt1.Rows[0]["m_item"].ToString()).ID.ToString();
						}
						catch{}
						try{
						productcategory.SelectedValue = db.CharProductCategory.SingleOrDefault(p => p.NamaKode == dt1.Rows[0]["m_kategori"].ToString()).ID.ToString();
						}
						catch{}
						try{
						productlevel.SelectedValue = db.CharProductLevel.SingleOrDefault(p => p.NamaKode == dt1.Rows[0]["m_productlevel"].ToString()).ID.ToString();
						}
						catch{}
						try{
						stonedist.SelectedValue = db.CharStoneDist.SingleOrDefault(p => p.NamaKode == dt1.Rows[0]["m_stonedist"].ToString()).ID.ToString();
						}
						catch{}
						try{
						framematerial.SelectedValue = db.CharFrameMaterial.SingleOrDefault(p => p.NamaKode == framemat).ID.ToString();
						}
						catch{}
					}
                }
                catch
                {

                }
            }
		}
    }


    protected void fillForm_charproduct02()
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && (p.Draft || (!p.Draft && conf == 2)));
		if (d != null) 
		{
			var x = d.StockReceiveDJ_CharProduct;
			if(x != null)
			{
				kadarlogam.Text = Str.Number2(d.StockReceiveDJ_CharProduct.KadarLogam);
				kadartukaran.Text = Str.Number2(d.StockReceiveDJ_CharProduct.KadarTukaran);
				gweight.Text = Str.Number2(d.StockReceiveDJ_CharProduct.GrossWeight);
				dimensid.Text = Str.Number2(d.StockReceiveDJ_CharProduct.DimensiD);
				dimensip.Text = Str.Number2(d.StockReceiveDJ_CharProduct.DimensiP);
				dimensil.Text = Str.Number2(d.StockReceiveDJ_CharProduct.DimensiL);
				dimensir.Text = Str.Number2(d.StockReceiveDJ_CharProduct.DimensiR);
			}
		}
    }

    protected void fillForm_chardesign01()
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && (p.Draft || (!p.Draft && conf == 2)));
		if (d != null) 
		{
			var x = d.StockReceiveDJ_CharDesign;
			if(x != null)
			{
				designcategory.SelectedValue = d.StockReceiveDJ_CharDesign.DesignCategory.ToString();
				designconcept.SelectedValue = d.StockReceiveDJ_CharDesign.DesignConcept.ToString();
				designprocess.SelectedValue = d.StockReceiveDJ_CharDesign.DesignProcess.ToString();
				targetage.SelectedValue = d.StockReceiveDJ_CharDesign.TargetAge.ToString();
				targetgender.SelectedValue = d.StockReceiveDJ_CharDesign.TargetGender.ToString();
			}
		}
    }

    protected void fillForm_chardesign02()
    {
        //var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        //if (d == null) Data_Error();
        //else
        //{
        //    if (d.Proto != null && d.Proto != 0 && d.Sumber == 1)
        //    {
        //        var proto = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
        //        if (proto != null)
        //        {
        //            naming.Text = proto.ProductOrder.ProductOrder_CharDesign.Naming;
        //            setitem.Checked = (proto.ProductOrder.ProductOrder_CharDesign.SetItem == true) ? true : false;
        //            special.Checked = (proto.ProductOrder.ProductOrder_CharDesign.Special == true) ? true : false;
        //            grafir.Text = proto.ProductOrder.ProductOrder_CharDesign.Grafir;

        //            //DLC
        //            DLC.CharProcessFinishingFromSPK(cb, Convert.ToInt32(proto.IDProductOrder));
        //        }
        //    }
        //    else if (d.IDMounting.HasValue && d.Sumber == 3)
        //    {
        //        var mt = db.StockReceiveMounting.SingleOrDefault(p => p.ID == d.IDMounting);
        //        if (mt != null)
        //        {
        //            naming.Text = mt.StockReceiveMounting_CharDesign.Naming;
        //            setitem.Checked = (mt.StockReceiveMounting_CharDesign.SetItem == true) ? true : false;
        //            special.Checked = (mt.StockReceiveMounting_CharDesign.Special == true) ? true : false;
        //            grafir.Text = mt.StockReceiveMounting_CharDesign.Grafir;

        //            //DLC
        //            DLC.CharProcessFinishingFromMounting(cb, Convert.ToInt32(mt.ID));
        //        }
        //    }
        //    else
        //    {
        //        DLC.CharProcessFinishing(cb);
        //    }
        //}
    }


    protected void fillForm_stone()
    {
        //var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && (p.Draft || (!p.Draft && conf == 2)));
        //if (d == null) Data_Error();
        //else
        //{
            /*//Header
            var product = d.StockReceiveDJ_CharProduct;
            //var product = (from p in db.StockReceiveDJ_CharProduct where p.ID == myID select p).SingleOrDefault();
            if (product == null) ProductError();
            else
            {
                grossweight.InnerHtml = Str.Number2(product.GrossWeight);
                stonecarat.InnerHtml = Str.Number3(product.StoneCarat);
                stoneweight.InnerHtml = Str.Number2(product.StoneWeight);
                nettoweight.InnerHtml = Str.Number2(product.NettoWeight);
            }*/
        //}
    }

    protected void fillForm_costing()
    {
        //var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && (p.Draft || (!p.Draft && conf == 2)));
        //if (d == null) Data_Error();
        //else
        //{
            /*pembagi.Text = Str.Number0(d.StockReceiveDJ_Costing.Pembagi);

            //var mnt = db.PricingTableFM.SingleOrDefault(p => p.IDBrand == d.LocWarehouse.CompanyBrand.ID && p.IDItem == d.StockReceiveDJ_CharProduct.ProductItem && p.IDCons == d.StockReceiveDJ_CharProduct.ProcessCons);
            //if (mnt != null)
            //    pembagi.Text = Convert.ToDecimal(mnt.Pembagi.HasValue ? mnt.Pembagi.Value : 1).ToString("N0");
            //else
            //    pembagi.Text = "1";

            if (d.Sumber != 2)
            {
                unitprice.Text = "0";
                unitprice_Row.Visible = false;
            }
            //if (d.Sumber == 3 && d.IDMounting != null)
            //{
            //    var mt = db.StockReceiveMounting.SingleOrDefault(p => p.ID == d.IDMounting);
            //    if (mt != null)
            //    {
            //        mountingno.Text = Str.Number0(mt.StockReceiveMounting_Costing.MountingNO);
            //        mountingro.Text = Str.Number0(mt.StockReceiveMounting_Costing.MountingRO);
            //        pembagi.Text = Str.Number0(mt.StockReceiveMounting_Costing.Pembagi);
            //        setting.Text = Str.Number0(mt.StockReceiveMounting_Costing.Setting);
            //        ongkoslaincogs.Text = Str.Number0(mt.StockReceiveMounting_Costing.OngkosLainCOGS);
            //        ongkoslain.Text = Str.Number0(mt.StockReceiveMounting_Costing.OngkosLain);
            //    }
            //}*/
        //}
    }

    protected bool valid_costing
    {
        get
        {
            bool x = true;
            //x = x && Fv.isDecimal(frame, frame_ERR);
            x = x && Fv.isDecimal(mountingno, mountingno_ERR);
            x = x && Fv.isDecimal(mountingro, mountingro_ERR);
            x = x && Fv.isInt(pembagi, pembagi_ERR);
            x = x && Fv.isDecimal(setting, setting_ERR);
            //x = x && Fv.isDecimal(finishing, finishing_ERR);

            return x;
        }
    }
    protected void fillTable_stone(object sender, EventArgs e)
    {
        int GT_Butir = 0;
        decimal GT_Carat = 0;

        //Stone
        fillStone1A_stone(0, ref GT_Butir, ref GT_Carat);
        fillStone1B_stone(1, ref GT_Butir, ref GT_Carat);
        fillStone2_stone(2, ref GT_Butir, ref GT_Carat);
        fillStone3_stone(3, ref GT_Butir, ref GT_Carat);
        fillStone4_stone(4, ref GT_Butir, ref GT_Carat);
        fillStone5_stone(5, ref GT_Butir, ref GT_Carat);

        //Grand-Total
        Sub_Total_stone(GT_Butir, GT_Carat, true);
    }


    protected string TD_HyperLink_stone(int StoneIndex, int TableID)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (conf != 1 && conf != 2)
        {
            href += "DJDO_StoneUpdate.aspx";
            href += "?id=" + myID;
            href += "&si=" + StoneIndex;
            href += "&tid=" + TableID;
            href += "&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG ;

            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-info\" href=\"" + href + "\">";
            x += "      <i class=\"icon-pencil\"></i> ";
            x += "      Update";
            x += "  </a>";
            x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + myID + "', '" + StoneIndex + "', '" + TableID + "', '" + myNODO + "')\">";
            x += "      <i class=\"icon-trash\"></i> ";
            x += "      Delete";
            x += "  </a>";
            x += "</div>";
        }

        return x;
    }
    protected string TD_Nama_stone(string Nama, string size)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong> " + "<span class=\"text-muted text-sm\">(" + size + ")</span></p>";

        return x;
    }
    protected string TD_Nama_stone(string Nama, string size, string GIA, int Status, string tglGIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong> " + "<span class=\"text-muted text-sm\">(" + size + ")</span></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\">GIA : " + GIA + (tglGIA == "" ? "" : (" (" + tglGIA + ")")) + "</p>";
        if (Status == 1)
            x += "<p class=\"text-muted text-sm\">Terpasang</p>";
        else
            x += "<p class=\"text-muted text-sm\">Tidak Terpasang</p>";

        return x;
    }

    protected string TD_Dimensi_stone(decimal? Dimensi)
    {
        string x = string.Empty;

        x += "<p>" + Str.Number2(Dimensi) + "</p>";

        return x;
    }

    protected string TD_CaratButir_stone(decimal? CaratButir)
    {
        string x = string.Empty;

        x += "<p class=\"text-muted\">" + Str.Number3(CaratButir) + "</p>";

        return x;
    }
    protected string TD_TotalButir_stone(int? TotalButir, ref int ST)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-success\">" + Str.Integer(TotalButir) + "</p>";

        //Sub-Total
        if (TotalButir.HasValue) ST += TotalButir.Value;

        return x;
    }

    protected string TD_TotalCarat_stone(decimal? TotalCarat, ref decimal ST)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-info\">" + Str.Number3(TotalCarat) + "</p>";

        //Sub-Total
        if (TotalCarat.HasValue) ST += TotalCarat.Value;

        return x;
    }

    protected void Sub_Header_stone(int StoneIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.StoneIndex(StoneIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void Sub_Total_stone(int TotalButir, decimal TotalCarat, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 7;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Integer(TotalButir) + "</p>";
        td.CssClass = "text-center";
        lbbutir.Text = TotalButir.ToString();
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number3(TotalCarat) + "</p>";
        td.CssClass = "text-right";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void fillStone1A_stone(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockReceiveDJ_Stone1A
                 where p.IDForm == myID
                 orderby p.Stone1A.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header_stone(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink_stone(StoneIndex, r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            // td = new TableCell();
            // td.Text = TD_Nama_stone(r.Stone1A.Nama, (Str.Number3(r.Stone1A.Parcel102.SizeMin) + " - " + Str.Number3(r.Stone1A.Parcel102.SizeMax)));
            // td.Style["text-align"] = "left";
            // tr.Cells.Add(td);
			
			string Certificate = Db.SingleString("SELECT TOP 1 ISNULL(CertificateNumber,'') FROM StockReceiveDJ_Stone1A WHERE ID = " + r.ID + " ", Db.Cnn_DB);
            td = new TableCell();
            td.Text = TD_Nama_stone(
                r.Stone1A.Nama
                , (Str.Number3(r.Stone1A.Parcel102.SizeMin) + " - " + Str.Number3(r.Stone1A.Parcel102.SizeMax))
                , Certificate , 1, "");
            td.Style["text-align"] = "left";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir_stone(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir_stone(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat_stone(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total_stone(ST_Butir, ST_Carat, false);
        }
    }

    protected void fillStone1B_stone(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockReceiveDJ_Stone1B
                 where p.IDForm == myID
                 orderby p.Stone1B.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header_stone(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink_stone(StoneIndex, r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama_stone(
                r.Stone1B.Nama
                , (Str.Number3(r.Stone1B.Parcel102.SizeMin) + " - " + Str.Number3(r.Stone1B.Parcel102.SizeMax))
                , r.GIA, Convert.ToInt32(r.StatusTerpasang), (r.TglGIA.HasValue ? Str.Tgl(r.TglGIA) : ""));
            td.Style["text-align"] = "left";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir_stone(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir_stone(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat_stone(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total_stone(ST_Butir, ST_Carat, false);
        }
    }

    protected void fillStone2_stone(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockReceiveDJ_Stone2
                 where p.IDForm == myID
                 orderby p.Stone2.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header_stone(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink_stone(StoneIndex, r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama_stone(r.Stone2.Nama, (Str.Number3(r.Stone2.Parcel203.SizeMin) + " - " + Str.Number3(r.Stone2.Parcel203.SizeMax)));
            td.Style["text-align"] = "left";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir_stone(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir_stone(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat_stone(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total_stone(ST_Butir, ST_Carat, false);
        }
    }

    protected void fillStone3_stone(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockReceiveDJ_Stone3
                 where p.IDForm == myID
                 orderby p.Stone3.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header_stone(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink_stone(StoneIndex, r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            //td.Text = TD_Nama_stone(r.Stone3.Nama);
            td.Text = TD_Nama_stone(
                r.Stone3.Nama,
                (Str.Number3(r.Stone3.Parcel303.SizeMin) + " - " + Str.Number3(r.Stone3.Parcel303.SizeMax))
                , r.GIA, 1, (r.TglGIA.HasValue ? Str.Tgl(r.TglGIA) : ""));
            td.Style["text-align"] = "left";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir_stone(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir_stone(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat_stone(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total_stone(ST_Butir, ST_Carat, false);
        }
    }

    protected void fillStone4_stone(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockReceiveDJ_Stone4
                 where p.IDForm == myID
                 orderby p.Stone4.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header_stone(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink_stone(StoneIndex, r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama_stone(r.Stone4.Nama, (Str.Number3(r.Stone4.Parcel403.SizeMin) + " - " + Str.Number3(r.Stone4.Parcel403.SizeMax)));
            td.Style["text-align"] = "left";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir_stone(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir_stone(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat_stone(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total_stone(ST_Butir, ST_Carat, false);
        }
    }

    protected void fillStone5_stone(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockReceiveDJ_Stone5
                 where p.IDForm == myID
                 orderby p.Stone5.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header_stone(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink_stone(StoneIndex, r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama_stone(r.Stone5.Nama, r.Stone5.Parcel506.NamaKode);
            td.Style["text-align"] = "left";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi_stone(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir_stone(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir_stone(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat_stone(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total_stone(ST_Butir, ST_Carat, false);
        }
    }

    protected void fillDist_charproduct01(object sender, EventArgs e)
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
			string stndist = "";
			try{
				var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && (p.Draft || (!p.Draft && conf == 2)));
				var x = d.StockReceiveDJ_CharProduct;
				if(x == null){
					stndist = Db.SingleString("SELECT TOP 1 m_stonedist FROM sk_order where m_nomor = '"+ myNOVAL +"' ", ConStringPabrig).ToString();
					stonedist.SelectedValue = db.CharStoneDist.SingleOrDefault(p => p.NamaKode == stndist).ID.ToString();
				}
				else{
					stonedist.SelectedValue = x.StoneDist.ToString();
				}
			}
			catch{}
			
        }

        if (Page.IsPostBack == true)
        {
            fillTable_stone(sender, e);
            //add.HRef += "&"
        }

        
    }

    protected void fillgross(object sender, EventArgs e)
    {
        App.NumberFormat(gweight);
        if (Page.IsPostBack == true)
        {
            fillTable_stone(sender, e);
            //add.HRef += "&"
        }
    }


    protected bool valid_charproduct02
    {
        get
        {
            bool x = true;

            x = x && Fv.isDecimal(gweight, grossweight_ERR);
            x = x && Fv.isDecimal(kadarlogam, kadarlogam_ERR);
            x = x && Fv.isDecimal(kadartukaran, kadartukaran_ERR);
            x = x && Fv.isDecimal(dimensid, dimensid_ERR);
            x = x && Fv.isDecimal(dimensip, dimensip_ERR);
            x = x && Fv.isDecimal(dimensil, dimensil_ERR);
            x = x && Fv.isDecimal(dimensir, dimensir_ERR, true);
            //Value
            x = x && valid_GrossWeight_charproduct02;
            x = x && valid_KadarLogam_charproduct02;
            x = x && valid_KadarTukaran_charproduct02;

            return x;
        }
    }

    protected bool valid_GrossWeight_charproduct02
    {
        get
        {
            decimal dv = Convert.ToDecimal(gweight.Text);

            bool x = dv > 0;
            grossweight_VAL.Visible = !x;

            return x;
        }
    }
    protected bool valid_KadarLogam_charproduct02
    {
        get
        {
            decimal dv = Convert.ToDecimal(kadarlogam.Text);

            bool x = dv > 0;
            kadarlogam_VAL.Visible = !x;

            return x;
        }
    }
    protected bool valid_KadarTukaran_charproduct02
    {
        get
        {
            decimal dv = Convert.ToDecimal(kadartukaran.Text);

            bool x = dv > 0;
            kadartukaran_VAL.Visible = !x;

            return x;
        }
    }
    protected bool valid_data02
    {
        get
        {
            bool x = true;
            x = x && Fv.isDateTime(expiredproduct, expiredproduct_ERR);
            x = x && Fv.isDateTime(expiredrequest, expiredrequest_ERR, true);
            return x;
        }
    }

    protected bool valid_Dist_charproduct01
    {
        get
        {
            bool x = stonedist.Items.Count > 0;
            stonedist_ERR.Visible = !x;
            return x;
        }
    }

    protected void save_Click(object sender, EventArgs e)
    {
        //lit_test_msg.Text += "<br />----123123123test.save_Click";


        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) Data_Error();
        else
        {
            int IDRec = d.ID;
            var brj_info = cs.T_brj.Single(p => p.IDReceive == IDRec && p.NoDOBRJ == myNODO);

            //Status
            d.Draft = false;
            d.DraftDate = System.DateTime.Now;
			d.NoValidasi = number_validation.InnerText;

            //Commit
            //db.CommandTimeout = 5 * 60;
            db.SubmitChanges();

            //Redirect
            //Response.Redirect("DJDO.aspx?nodo=" + brj_info.NoDOBRJ);
        }

        //Response.Redirect("DJDO_Submit.aspx?id=" + myID);
        //save.Visible = false;
        //div_confirm.Visible = true;


    }
    protected void cancel_Click(object sender, EventArgs e)
    {

        Response.Redirect("DJDO_Detail_Warehouse.aspx?id=" + myID + "&nodo=" + myNODO);
    }

    protected void update_image_filename_by_selection(int code = 6)
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) Data_Error();
        else
        {

            int IDRec = d.ID;
            var brj_info = cs.T_brj.Single(p => p.IDReceive == IDRec && p.NoDOBRJ == myNODO);

            string path = string.Empty;
            path += App.ImagesDir;
            path += "StockReceiveDJ" + "\\";

            string ext = ".jpg";
            string fc_code = "-";
            
            if(d.StockReceiveDJ_CharProduct == null)
            {
                fc_code = db.CharFrameColor.SingleOrDefault(p => p.ID == Convert.ToInt32(framecolor.SelectedValue)).NamaKode;
            }
            else fc_code = d.StockReceiveDJ_CharProduct.CharFrameColor.NamaKode;
            string filename = "";
		
            KodeKaretDJ kkdj = new KodeKaretDJ();
            try
            {
                    kkdj = db.KodeKaret_Detail.Single(p => p.ID == d.Proto).KodeKaretDJ;
            }
            catch {}

            string kodeBarang = string.IsNullOrEmpty(brj_info.NoKodeBarang.Split('/').First().Replace(" ", "")) ? protonomor.Text : brj_info.NoKodeBarang.Split('/').First().Replace(" ", "");

            // 1
            if (code == 1)
            {
                filename = kkdj.Nomor + fc_code + ext;
            }
            if (code == 2)
            {
                filename = kkdj.Nomor + "W" + ext;
            }
            if (code == 3)
            {
                filename = kodeBarang + fc_code + ext;
            }
            if (code == 4)
            {
                filename = kodeBarang + "W" + ext;
            }
            if (code == 5)
            {
                filename = kodeBarang + ext;
            }
            if (code == 6)
            {
                filename = kkdj.Nomor + ext;
            }

            d.ImgPicture = path + filename;
            db.SubmitChanges();
            imageChange = true;
            //confirm_Click(sender, e);

            //Response.Redirect("DJDO_Detail_Warehouse.aspx?id=" + myID + "&nodo=" + myNODO + "&frm=" + framecolor.SelectedValue);
        }
    }

    protected void btn_filename_1_Click(object sender, EventArgs e)
    {
        update_image_filename_by_selection(1);
        confirm_Click(sender, e);
    }

    protected void btn_filename_2_Click(object sender, EventArgs e)
    {

        update_image_filename_by_selection(2);
        confirm_Click(sender, e);
    }

    protected void btn_filename_3_Click(object sender, EventArgs e)
    {
        update_image_filename_by_selection(3);
        confirm_Click(sender, e);
    }

    protected void btn_filename_4_Click(object sender, EventArgs e)
    {
        update_image_filename_by_selection(4);
        confirm_Click(sender, e);
    }

    protected void btn_filename_5_Click(object sender, EventArgs e)
    {
        update_image_filename_by_selection(5);
        confirm_Click(sender, e);
    }

    protected void btn_filename_6_Click(object sender, EventArgs e)
    {
        update_image_filename_by_selection(6);
        confirm_Click(sender, e);
    }

    protected void btn_Picture_save_Click(object sender, EventArgs e)
    {
        //lit_test_msg.Text += "<br />----123123123test.btn_filename_1_Click";
        save_Click_picture(sender, e);
        confirm_Click(sender, e);
    }

    protected void framecolorupdate_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) Data_Error();
        else
        {
            d.StockReceiveDJ_CharProduct.FrameColor = Convert.ToInt32(framecolor.SelectedValue);

            db.SubmitChanges();

            Response.Redirect("DJDO_Detail_Warehouse.aspx?id=" + myID + "&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG + "#div_picture");
        }

    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        //if (d == null) Data_Error();
        //else
        //{
			//try
			//{
        save_Click_data01(sender, e);
        save_Click_data02(sender, e);
        save_Click_charproduct01(sender, e);
        save_Click_charproduct02(sender, e);
        save_Click_chardesign01(sender, e);
        save_Click_chardesign02(sender, e);
        save_Click_costing(sender, e);

        // if(!imageChange) Response.Redirect("DJDO_Detail_Warehouse.aspx?id=" + myID + "&nodo=" + myNODO + "&conf=1");
        // else Response.Redirect("DJDO_Detail_Warehouse.aspx?id=" + myID + "&nodo=" + myNODO);
			//}
			//catch(Exception ex)
			//{
				//Response.Redirect("DJDO_Detail_Warehouse.aspx?id=" + myID + "&nodo=" + myNODO + "&errlink="+ex.Message+"");
			//}
        //}

    }


    protected void save_Click_data01(object sender, EventArgs e)
    {
        if (protonomor.Text != "")
        {
            var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
            //if (d == null) Data_Error();
            //else
            //{
                if(String.IsNullOrEmpty(d.ImgPicture)) d.ImgPicture = imgPath;
                d.ProtoNomor = Str.CapUpper(protonomor.Text);
                d.ProductOrderNomor = Str.CapUpper(productordernomor.Text);
				d.NoValidasi = number_validation.InnerText;

                if (d.Sumber == 1)
                {
                    //var pt = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                    //if (pt != null)
                    //    d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + pt.KodeKaretDJ.Nomor + ".jpg";
                }
                else
                    d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + d.ProtoNomor + ".jpg";

                var sp = (from p in db.DataSupplier
                          where p.Nama.Equals(suppliernama.Text)
                          select p);
                if (sp.Count() > 0)
                {
                    var spl = sp.First();
                    d.Supplier = spl.ID;
                }

                int UserID = App.UserID;
                var f = from p in db.DataSK
                        where p.IDUser == UserID
                        select p;
                if (f.Count() > 0)
                {
                    var sk = f.First();
                    d.HumanStock = sk.ID;
                    d.HumanStockNama = sk.Nama;
                }

                d.SupplierNama = Str.CapTitle(suppliernama.Text.Trim());
                d.SupplierNomor = Str.CapUpper(suppliernomor.Text.Trim());

                d.RequestOutletNama = Str.CapTitle(requestoutletnama.Text);
                d.RequestCustomerNama = Str.CapTitle(requestcustomernama.Text);

                //Commit
                db.SubmitChanges();
                //Redirect
                //Response.Redirect("DJ_Data02.aspx?id=" + myID);
            //}
        }
    }

    protected void save_Click_data02(object sender, EventArgs e)
    {
        if (valid_data02)
        {
            var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
            //if (d == null) Data_Error();
            //else
            //{
                d.HumanDesignerNama = Str.CapTitle(humandesignernama.Text);
                d.HumanStockNama = Str.CapTitle(humanstocknama.Text);

                d.ExpiredProduct = Convert.ToDateTime(expiredproduct.Text);
                if (expiredrequest.Text != "")
                    d.ExpiredRequest = Convert.ToDateTime(expiredrequest.Text);
                else
                    d.ExpiredRequest = null;

                //Commit
                db.SubmitChanges();
                //Redirect
                // Response.Redirect("DJ_CharProduct01.aspx?id=" + myID);
            //}
        }
    }

    protected void save_Click_charproduct01(object sender, EventArgs e)
    {
        if (valid_Dist_charproduct01)
        {
            var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
            var x = d.StockReceiveDJ_CharProduct;
            if (x == null) {
				insertProcess_charproduct01(d);
			}
            else
            {
                //Process
                deleteProcess_charproduct01(d);
                insertProcess_charproduct01(d);

                //Redirect
                //Response.Redirect("DJ_CharProduct02.aspx?id=" + myID);
            }
        }
    }

    protected void save_Click_charproduct02(object sender, EventArgs e)
    {
        if (valid_charproduct02)
        {
            var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
            //if (d == null) Data_Error();
            //else
            //{
                //Process
                saveData_charproduct02(d);
                //Redirect
                //Response.Redirect("DJ_CharDesign01.aspx?id=" + myID);
            //}
        }
    }

    protected void save_Click_chardesign01(object sender, EventArgs e)
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
		var x = d.StockReceiveDJ_CharDesign;
        if (x == null) {
			insertProcess_chardesign01(d);
		}
        else
        {
            //Process
            deleteProcess_chardesign01(d);
            insertProcess_chardesign01(d);
            //Redirect
            //Response.Redirect("DJ_CharDesign02.aspx?id=" + myID);
        }
    }

    protected void save_Click_chardesign02(object sender, EventArgs e)
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);

        //if (d == null) Data_Error();
        //else
        //{
            // var product = d.StockReceiveDJ_CharProduct;
            var product = (from p in db.StockReceiveDJ_CharProduct where p.ID == myID select p).SingleOrDefault();

            var x = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);

            if (d.Sumber == 1)
            {
                //var pt = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                var pt = (from p in db.KodeKaret_Detail where p.ID == d.Proto select p).SingleOrDefault();
                if (pt != null)
                {
                    //d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + pt.KodeKaretDJ.Nomor + ".jpg";
                    //d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + pt.KodeKaretDJ.Nomor + pt.KodeKaretDJ.KodeKaretDJ_CharProduct.CharFrameColor.NamaKode + ".jpg";
                    //int cf = Convert.ToInt32((from p in db.StockReceiveDJ_CharProduct where p.ID == myID select p.CharFrameColor).SingleOrDefault());
                    //string framecolorname = (from p in db.CharFrameColor where p.ID == cf select p.Nama).SingleOrDefault();

                    // d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + pt.KodeKaretDJ.Nomor + d.StockReceiveDJ_CharProduct.CharFrameColor.NamaKode + ".jpg";

                }
            }
            else
            {
                d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + d.ProtoNomor + ".jpg";
                //d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + d.ProtoNomor + d.StockReceiveDJ_CharProduct.CharFrameColor.NamaKode + ".jpg";
            }
            //Process
            saveData_chardesign02(d);
            saveFinishing_chardesign02(d);
            if (d.Proto != null && d.Proto != 0)
                //moveStone_chardesign02(d, x);

                //Recal Weight
                Lib.RecalWeight(db, d, product);
            //Redirect
            //Response.Redirect("DJ_Stone.aspx?id=" + myID);
        //}
    }

    protected void save_Click_costing(object sender, EventArgs e)
    {
        //lit_test_msg.Text += "<br />----123123123test.save_Click_costing_1";

        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        var x = d.StockReceiveDJ_Costing;
        if (x == null) {
            insertProcess_costing(d);
        }
        else
        {
            //lit_test_msg.Text += "<br />----123123123test.save_Click_costing_3";

            //Process
            //deleteProcess_costing(d);
            //insertProcess_costing(d);
            updateProcess_costing(d);

            //updateChar_costing(d); //// sementara di-disable dulu karena belum siap 2017-01-06
            //Redirect
            //Response.Redirect("DJ_Picture.aspx?id=" + myID);
            //Response.Redirect("DJ_Submit.aspx?id=" + myID);
        }
    }
    protected void save_Click_picture(object sender, EventArgs e)
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) Data_Error();
        else
        {
             //Process
             finishUpload(d);
             //Redirect
             Response.Redirect("DJDO_Detail_Warehouse.aspx?id=" + myID
                         + "&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG
                         + "#div_picture"
                         );
        }
    }

    protected bool finishUpload(StockReceiveDJ d)
    {
        bool x = true;

        x = x && saveImage(d, imgpicture, imgpicture_ERR, "Picture");

        return x;
    }

    //Process
    protected bool saveImage(StockReceiveDJ d, FileUpload file, HtmlGenericControl lb, string name)
    {
        if (!Upload.HasFile(file))
        {
            //lit_test_msg.Text += "<br />----123123123test.saveImageA";
            //Commit
            d.ImgPicture = null;
            db.SubmitChanges();

            return true;
        }
        else
        {
            //lit_test_msg.Text += "<br />----123123123test.saveImageB" + file.PostedFile.FileName;

            if (!Upload.FormatValid(file, lb, Upload.ImageFormat)) return false;
            else
            {
                //lit_test_msg.Text += "<br />----123123123test.saveImageC" + file.PostedFile.FileName;
                string path = string.Empty;

                path += App.ImagesDir;
                path += "StockReceiveDJ" + "\\";

                if (Upload.CreateDir(path))
                {
                    //lit_test_msg.Text += "<br />----123123123test.saveImageD" + file.PostedFile.FileName;
                    path += name + "_";
                    path += myID + Upload.GetExt(file.PostedFile.FileName);

                    //Upload
                    file.PostedFile.SaveAs(path);

                    //Commit
                    d.ImgPicture = path;
                    db.SubmitChanges();

                    return true;
                }
                else
                {
                    //lit_test_msg.Text += "<br />----123123123test.saveImageE" + file.PostedFile.FileName;
                    dir_ERR.InnerHtml += path;
                    dir_ERR.Visible = true;

                    return false;
                }
            }
        }
    }


    protected void saveData_chardesign02(StockReceiveDJ ms)
    {
        //var d = ms.StockReceiveDJ_CharDesign;
        var d = (from p in db.StockReceiveDJ_CharDesign where p.ID == myID select p).SingleOrDefault();
        //if (d == null) LinkError1();
        //else
        //{
            d.Naming = Str.CapUpper(naming.Text);
            d.Grafir = Str.CapUpper(grafir.Text);

            d.SetItem = setitem.Checked;
            d.Special = special.Checked;

            //Commit
            db.SubmitChanges();
        //}
    }
    protected void insertProcess_chardesign02(StockReceiveDJ ms, int myIDFinishing)
    {
        var d = ms.StockReceiveDJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d == null)
        {
            StockReceiveDJ_Finishing n = new StockReceiveDJ_Finishing();

            n.IDForm = ms.ID;
            n.IDFinishing = myIDFinishing;

            //Commit
            db.StockReceiveDJ_Finishing.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected void deleteProcess_chardesign02(StockReceiveDJ ms, int myIDFinishing)
    {
        var d = ms.StockReceiveDJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d != null)
        {
            //Commit
            db.StockReceiveDJ_Finishing.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void saveFinishing_chardesign02(StockReceiveDJ d)
    {
        //var rs = from p in cb.Items.Cast<ListItem>()
        //         select p;

        //foreach (var r in rs)
        //{
        //    int myIDFinishing = Convert.ToInt32(r.Value);
        //    if (r.Selected)
        //        insertProcess_chardesign02(d, myIDFinishing);
        //    else
        //        deleteProcess_chardesign02(d, myIDFinishing);
        //}
    }
    protected void moveStone_chardesign02(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        moveStone1A_chardesign02(ms, x);
        moveStone1B_chardesign02(ms, x);
        moveStone2_chardesign02(ms, x);
        moveStone3_chardesign02(ms, x);
        moveStone4_chardesign02(ms, x);
        moveStone5_chardesign02(ms, x);
    }

    protected void moveStone1A_chardesign02(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone1A.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone1A.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in db.ProductOrder_Stone1A
                    where p.IDForm == x.IDProductOrder
                    orderby p.Stone1A.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockReceiveDJ_Stone1A n = new StockReceiveDJ_Stone1A();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockReceiveDJ_Stone1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }

    protected void moveStone1B_chardesign02(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone1B.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone1B.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in db.ProductOrder_Stone1B
                    where p.IDForm == x.IDProductOrder
                    orderby p.Stone1B.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockReceiveDJ_Stone1B n = new StockReceiveDJ_Stone1B();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;
                n.GIA = r.GIA;
                n.StatusTerpasang = r.StatusTerpasang;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockReceiveDJ_Stone1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }

    protected void moveStone2_chardesign02(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone2.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone2.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in db.ProductOrder_Stone2
                    where p.IDForm == x.IDProductOrder
                    orderby p.Stone2.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockReceiveDJ_Stone2 n = new StockReceiveDJ_Stone2();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockReceiveDJ_Stone2.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }

    protected void moveStone3_chardesign02(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone3.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone3.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in db.ProductOrder_Stone3
                    where p.IDForm == x.IDProductOrder
                    orderby p.Stone3.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockReceiveDJ_Stone3 n = new StockReceiveDJ_Stone3();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;
                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockReceiveDJ_Stone3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }

    protected void moveStone4_chardesign02(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone4.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone4.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in db.ProductOrder_Stone4
                    where p.IDForm == x.IDProductOrder
                    orderby p.Stone4.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockReceiveDJ_Stone4 n = new StockReceiveDJ_Stone4();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockReceiveDJ_Stone4.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }

    protected void moveStone5_chardesign02(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone5.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone5.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in db.ProductOrder_Stone5
                    where p.IDForm == x.IDProductOrder
                    orderby p.Stone5.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockReceiveDJ_Stone5 n = new StockReceiveDJ_Stone5();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockReceiveDJ_Stone5.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void deleteProcess_charproduct01(StockReceiveDJ ms)
    {
        var d = ms.StockReceiveDJ_CharProduct;
        if (d != null)
        {
            //Commit
            db.StockReceiveDJ_CharProduct.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void insertProcess_charproduct01(StockReceiveDJ ms)
    {
        StockReceiveDJ_CharProduct n = new StockReceiveDJ_CharProduct();
        n.ID = ms.ID;

        n.ProductItem = Convert.ToInt32(productitem.SelectedValue);
        n.ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        n.ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        n.StoneDist = Convert.ToInt32(stonedist.SelectedValue);
        n.FrameMaterial = Convert.ToInt32(framematerial.SelectedValue);
        n.FrameFinishing = Convert.ToInt32(framefinishing.SelectedValue);
        n.FrameColor = Convert.ToInt32(framecolor.SelectedValue);
        n.ProcessCons = Convert.ToInt32(processcons.SelectedValue);

        n.GrossWeight = Convert.ToDecimal(gweight.Text);
        n.NettoWeight = Convert.ToDecimal(gweight.Text) - (Convert.ToDecimal(stonecarat.InnerText)*0.2m);
        n.KadarLogam = Convert.ToDecimal(kadarlogam.Text);
        n.KadarTukaran = Convert.ToDecimal(kadartukaran.Text);

        n.StoneCarat = Convert.ToDecimal(stonecarat.InnerText);
        n.StoneWeight = Convert.ToDecimal(stoneweight.InnerText);
        n.StoneQty = Convert.ToDecimal(lbbutir.Text);
		n.FrameColor = Convert.ToInt32(framecolor.SelectedValue);

        //Dimensi
        n.DimensiD = Convert.ToDecimal(dimensid.Text);
        n.DimensiP = Convert.ToDecimal(dimensip.Text);
        n.DimensiL = Convert.ToDecimal(dimensil.Text);

        //Ring-Size
        if (dimensir.Text != "")
            n.DimensiR = Convert.ToDecimal(dimensir.Text);
        else
            n.DimensiR = null;
        //Commit
        db.StockReceiveDJ_CharProduct.InsertOnSubmit(n);
        db.SubmitChanges();
    }

    protected void saveData_charproduct02(StockReceiveDJ ms)
    {
        //var d = ms.StockReceiveDJ_CharProduct;
        var d = (from p in db.StockReceiveDJ_CharProduct where p.ID == myID select p).SingleOrDefault();
        // if (d == null) LinkError();
        // else
        // {
            d.GrossWeight = Convert.ToDecimal(gweight.Text);
            d.NettoWeight = Convert.ToDecimal(gweight.Text) - (Convert.ToDecimal(stonecarat.InnerText)*0.2m);
            d.KadarLogam = Convert.ToDecimal(kadarlogam.Text);
            d.KadarTukaran = Convert.ToDecimal(kadartukaran.Text);

            //Dimensi
            d.DimensiD = Convert.ToDecimal(dimensid.Text);
            d.DimensiP = Convert.ToDecimal(dimensip.Text);
            d.DimensiL = Convert.ToDecimal(dimensil.Text);

            //Ring-Size
            if (dimensir.Text != "")
                d.DimensiR = Convert.ToDecimal(dimensir.Text);
            else
                d.DimensiR = null;

            //Commit
            db.SubmitChanges();
        //}
    }

    protected void deleteProcess_chardesign01(StockReceiveDJ ms)
    {
        var d = ms.StockReceiveDJ_CharDesign;
        if (d != null)
        {
            //Commit
            db.StockReceiveDJ_CharDesign.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void insertProcess_chardesign01(StockReceiveDJ ms)
    {
        StockReceiveDJ_CharDesign n = new StockReceiveDJ_CharDesign();

        n.ID = ms.ID;

        n.DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
        n.DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
        n.DesignProcess = Convert.ToInt32(designprocess.SelectedValue);
        n.TargetAge = Convert.ToInt32(targetage.SelectedValue);
        n.TargetGender = Convert.ToInt32(targetgender.SelectedValue);

        //Commit
        db.StockReceiveDJ_CharDesign.InsertOnSubmit(n);
        db.SubmitChanges();
    }

    protected void deleteProcess_costing(StockReceiveDJ ms)
    {
        var d = ms.StockReceiveDJ_Costing;
        if (d != null)
        {
            //Commit
            db.StockReceiveDJ_Costing.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void updateProcess_costing(StockReceiveDJ ms)
    {
        //lit_test_msg.Text += "<br />----123123123test.updateProcess_costing";

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        var n = ms.StockReceiveDJ_Costing;

        //n.ID = ms.ID;

        string xPembagi = (pembagi.Text.Trim() == "" ? "1" : pembagi.Text.Trim());

        decimal Pembagi = (Convert.ToDecimal(xPembagi) > 0 && (xPembagi != null)) ? Convert.ToDecimal(xPembagi) : 1;

        n.UnitPrice = Convert.ToDecimal(unitprice.Text);
        n.MountingNO = Convert.ToDecimal(mountingno.Text);
        n.MountingRO = Convert.ToDecimal(mountingro.Text);
        n.Pembagi = Convert.ToDecimal(Pembagi);
        n.Setting = Convert.ToDecimal(setting.Text);
        n.MountingNO = Convert.ToDecimal(mountingno.Text) / Convert.ToDecimal(Pembagi);

        //decimal testValueDecimal = saveFinishing_costing(ms);
        //lit_test_msg.Text += "<br />----123123123test.updateProcess_costing_saveFinishing_costing_" + testValueDecimal.ToString();
        //n.Finishing = testValueDecimal;

        n.Finishing = saveFinishing_costing(ms);
        n.OngkosLainCOGS = Convert.ToDecimal(ongkoslaincogs.Text);
        n.OngkosLain = Convert.ToDecimal(ongkoslain.Text);

        //Total
        //n.TotalRupiah = Convert.ToDecimal(n.UnitPrice) + n.MountingNO + n.MountingRO.Value + n.Setting + n.Finishing + Convert.ToDecimal(n.OngkosLainCOGS);
        n.TotalRupiah = n.MountingNO + n.MountingRO.Value + n.Setting + n.Finishing + Convert.ToDecimal(n.OngkosLainCOGS);
        n.TotalRate = prc_us.Harga;
        n.TotalBiaya = n.TotalRupiah.Value / n.TotalRate.Value;

        //save finishing
        //Commit
        //db.StockReceiveDJ_Costing.InsertOnSubmit(n);
        db.SubmitChanges();
    }

    protected void insertProcess_costing(StockReceiveDJ ms)
    {
        //lit_test_msg.Text += "<br />----123123123test.insertProcess_costing";

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        StockReceiveDJ_Costing n = new StockReceiveDJ_Costing();

        n.ID = ms.ID;

        decimal Pembagi = (Convert.ToDecimal(pembagi.Text) > 0 && (pembagi.Text != null)) ? Convert.ToDecimal(pembagi.Text) : 1;

        n.UnitPrice = Convert.ToDecimal(unitprice.Text);
        //n.Frame = Convert.ToDecimal(frame.Text);
        n.MountingNO = Convert.ToDecimal(mountingno.Text) / Pembagi;
        n.MountingRO = Convert.ToDecimal(mountingro.Text);
        n.Pembagi = Convert.ToDecimal(pembagi.Text);
        n.Setting = Convert.ToDecimal(setting.Text);
		decimal savefinishing = 0m;
		savefinishing = saveFinishing_costing(ms);
        n.Finishing = savefinishing;
        //n.Finishing = Convert.ToDecimal(finishing.Text);
        n.OngkosLainCOGS = Convert.ToDecimal(ongkoslaincogs.Text);
        n.OngkosLain = Convert.ToDecimal(ongkoslain.Text);

        //Total
        //n.TotalRupiah = Convert.ToDecimal(n.UnitPrice) + n.MountingNO + n.MountingRO.Value + n.Setting + n.Finishing + Convert.ToDecimal(n.OngkosLainCOGS);
		decimal total = 0m;
		total = Convert.ToDecimal(mountingno.Text) / Pembagi + Convert.ToDecimal(mountingro.Text) + Convert.ToDecimal(setting.Text) + savefinishing + Convert.ToDecimal(ongkoslaincogs.Text);
        n.TotalRupiah = total;
        n.TotalRate = prc_us.Harga;
        n.TotalBiaya = total / (prc_us.Harga ?? 1m);

        //save finishing
        //Commit
        db.StockReceiveDJ_Costing.InsertOnSubmit(n);
        db.SubmitChanges();
    }

    protected decimal saveFinishing_costing(StockReceiveDJ ms)
    {
        //lit_test_msg.Text += "<br />----123123123test.saveFinishing_costing";

        decimal Finishing = 0m;

        if (ms.Sumber != 3)
        {
            //var fns = from p in db.StockReceiveDJ_Finishing
            //          where p.IDForm == ms.ID
            //          select p;

            var fns = (from p in db.CharProcessFinishing
                       orderby p.SN ascending
                       select p.ID).ToList().ToArray();

            int index = 0;
            bool isDelete = false;

            foreach (TableRow row in tb_finishing.Rows)
            {
                //lit_test_msg.Text += "<br />----123123123test.saveFinishing_costing_A" + index.ToString();

                decimal val = 0m;
                string txt = "";
                txt = ((TextBox)row.FindControl("Finishing_" + fns[index])).Text.Trim();
                if (txt == "") { txt = "0"; isDelete = true; } else { isDelete = false; } // jika string kosong maka anggap angka 0
                val = Convert.ToDecimal(txt);

                //lit_test_msg.Text += "<br />----123123123test.saveFinishing_costing_B_" + index.ToString();
                //lit_test_msg.Text += "<br />----123123123test.saveFinishing_costing_C_" + fns[index].ToString();
                //lit_test_msg.Text += "<br />----123123123test.saveFinishing_costing_D_" + val;
                //lit_test_msg.Text += "<br />----123123123test._______________________________________";

                Finishing += val;

                if (fns.Contains(fns[index]))
                {

                    if (val >= 0m && !isDelete)
                    {
                        // if exists then update, if not then insert
                        var z = from p in db.StockReceiveDJ_Finishing where p.IDForm == ms.ID && p.IDFinishing == fns[index] select p;

                        if (z.Count() == 1)
                        {
                            // update
                            var x = z.FirstOrDefault(p => p.IDForm == ms.ID && p.IDFinishing == fns[index]);
                            x.BiayaModal = val;
                            db.SubmitChanges();
                        }
                        else
                        {
                            // delete all first then insert
                            var z1 = from p in db.StockReceiveDJ_Finishing where p.IDForm == ms.ID && p.IDFinishing == fns[index] select p;
                            db.StockReceiveDJ_Finishing.DeleteAllOnSubmit(z1);
                            db.SubmitChanges();

                            // insert
                            StockReceiveDJ_Finishing fns_add = new StockReceiveDJ_Finishing();

                            fns_add.IDForm = ms.ID;
                            fns_add.IDFinishing = fns[index];
                            fns_add.Biaya = null;
                            fns_add.BiayaModal = val;
                            fns_add.Efektif = null;

                            db.StockReceiveDJ_Finishing.InsertOnSubmit(fns_add);
                            db.SubmitChanges();
                        }
                    }
                    else
                    {
                        var z2 = from p in db.StockReceiveDJ_Finishing where p.IDForm == ms.ID && p.IDFinishing == fns[index] select p;
                        db.StockReceiveDJ_Finishing.DeleteAllOnSubmit(z2);
                        db.SubmitChanges();
                    }

                }


                index++;
            }


            //if (fns.Count() > 0)
            //{
            //    foreach (var r in fns)
            //    {
            //        TableRow row = tb.Rows;
            //        decimal val = Convert.ToDecimal(((TextBox)tb_finishing.Rows.FindControl("Finishing_" + r.ID.ToString())).Text);

            //        //test += ((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text != "" ? ((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text : "0";
            //    }
            //}
        }
        else if (ms.Sumber == 3)
        {
            var fns = from p in db.StockReceiveMounting_Finishing
                      where p.IDForm == ms.IDMounting
                      select p;
            if (fns.Count() > 0)
            {
                foreach (var r in fns)
                {
                    decimal val = 0;

                    val = Convert.ToDecimal(((TextBox)FindControl("Finishing_" + r.ID.ToString())).Text);

                    Finishing += val;

                    var x = db.StockReceiveMounting_Finishing.SingleOrDefault(p => p.ID == r.ID);
                    x.BiayaModal = val;
                    db.SubmitChanges();

                    //test += ((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text != "" ? ((TextBox)finishing_tr.FindControl("Finishing_" + r.ID.ToString())).Text : "0";
                }
            }
        }
        return Finishing;
    }

    protected void updateChar_costing(StockReceiveDJ d)
    {
        decimal stone = 0m;
        int IDCategory = 1;
        //int currCategory = d.StockReceiveDJ_CharProduct.ProductCategory;
        int currCategory = (from p in db.StockReceiveDJ_CharProduct where p.ID == myID select p.ProductCategory).SingleOrDefault();

        if (currCategory == 1 || currCategory == 6 || currCategory == 5)
        {
            stone = (from p in d.StockReceiveDJ_Stone1A
                     select new
                     {
                         CaratButir = p.CaratButir
                     }).Union(from p in d.StockReceiveDJ_Stone1B
                              select new
                              {
                                  CaratButir = p.CaratButir
                              }).Union(from p in d.StockReceiveDJ_Stone2
                                       select new
                                       {
                                           CaratButir = p.CaratButir
                                       }).Union(from p in d.StockReceiveDJ_Stone3
                                                select new
                                                {
                                                    CaratButir = p.CaratButir
                                                }).Union(from p in d.StockReceiveDJ_Stone4
                                                         select new
                                                         {
                                                             CaratButir = p.CaratButir
                                                         }).Union(from p in d.StockReceiveDJ_Stone5
                                                                  select new
                                                                  {
                                                                      CaratButir = p.CaratButir
                                                                  }).Max(p => p.CaratButir);
            if (stone <= 0.079m)
            {
                IDCategory = 1;
            }
            else if (stone > 0.079m && stone <= 0.899m)
            {
                IDCategory = 6;
            }
            else if (stone >= 0.9m)
            {
                IDCategory = 5;
            }

            //d.StockReceiveDJ_CharProduct.ProductCategory = IDCategory;
            var dc = (from p in db.StockReceiveDJ_CharProduct where p.ID == myID select p).SingleOrDefault();
            dc.ProductCategory = IDCategory;
            db.SubmitChanges();
        }
    }
    protected void framecolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        string path = string.Empty;
        path += App.ImagesDir;
        path += "StockReceiveDJ" + "\\";
        var da = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID);
        string fc_code = db.CharFrameColor.SingleOrDefault(p => p.ID == Convert.ToInt32(framecolor.SelectedValue)).NamaKode;
        string filename = protonomor.Text.Replace(" ", "") + fc_code + ".jpg";
        lit_filename_3.Text = filename;
        if (File.Exists(path + filename))
        {
            btn_filename_3.Visible = (conf != 1 && conf != 2 ? true : false);
            btn_filename_3.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
        }else
        {
            lit_fileexist_3.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
            btn_filename_3.Visible = false;
        }

        string fc_lama = Db.SingleString("SELECT TOP 1 KodeLama FROM CharFrameColor WHERE ID = " + Convert.ToInt32(framecolor.SelectedValue) + " ", Db.Cnn_DB);
        filename = protonomor.Text.Replace(" ", "") + fc_lama + ".jpg";
        lit_filename_4.Text = filename;
        if (File.Exists(path + filename))
        {
            btn_filename_4.Visible = (conf != 1 && conf != 2 ? true : false);
            btn_filename_4.Disabled = ((path + filename) == da.ImgPicture) ? true : false;
        }else
        {
            lit_fileexist_4.Text = "<span style=\"color:red;\" class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>";
            btn_filename_4.Visible = false;
        }

        if (Page.IsPostBack == true)
        {
            fillTable_stone(sender, e);
            //add.HRef += "&"
        }
    }
}