using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using System.Data.SqlClient;

public partial class PG_List : System.Web.UI.Page
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
    App price = new App();

    decimal Setting = 0m;

    int Reval_M = 0;
    int Reval_R = 0;
    int Reval_G = 0;
    int Reval_SK = 0;
    int Reval_SB = 0;
    int Reval_MF = 0;
    int Reval_MU = 0;
    int Reval_Rate = 0;

    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int act { get { return App.QueryInt(this, "act"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharGoldModelAllFilter(productmodel);
            DLC.CharGoldLevel(productlevel);


            fillFilter();

            DLC.LocationList(lokasi, Convert.ToInt32(tipelokasi.SelectedValue));
            lokasi.SelectedValue = Request.QueryString["lokasi"];

            //Generate

            string error = HttpUtility.UrlDecode(Request.QueryString["error"]);
            if (!String.IsNullOrEmpty(error))
            {
                err.Text = "<span class=\"text-danger\">PLU Error : " + error + "</span>";
            }
            //if (error != "")


        }


        if (App.Refresh(this)) fillTable(sender, e);
        //if (act == 1) Apply(sender, e);

    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        string spname = "sp_getExecute_reval_pg";
        string query = " exec " + spname
                     + " '" + kw.Text + "', "
                     + lokasi.SelectedValue + ", "
                     + tipelokasi.SelectedValue + ", "
                     + productitem.SelectedValue + ", "
                     + productmodel.SelectedValue + ", "
                     + productlevel.SelectedValue;

        DataTable dt = Db.Rs(query, Db.Cnn_DB);

        if (dt.Rows.Count > 0)
        {
            int index = 1;

            foreach (DataRow r in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td;

                CheckBox cb = new CheckBox();

                cb.ID = "cb_po" + index.ToString();
                cb.Style.Add("display", "block");
                cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                HiddenField hf = new HiddenField();
                hf.ID = "hf" + index.ToString();
                hf.Value = r["ID"].ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Style.Add("text-align", "center");
                td.Controls.Add(cb);
                td.Controls.Add(hf);

                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["Nomor"].ToString();
                tr.Cells.Add(td);

                string KodeKaret;
                KodeKaret = (r["ProtoNomor"] != null) ? r["ProtoNomor"].ToString() : "";
                td = new TableCell();
                td.Text = KodeKaret;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["ProductItem"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["GoldLevel"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["HargaJualSebelum"]).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["HargaJualSesudah"]).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Mutasi(Convert.ToDecimal(r["HargaJualSebelum"]), Convert.ToDecimal(r["HargaJualSesudah"]));
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Convert.ToDecimal(r["HPPMAwal"]).ToString("N3");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Convert.ToDecimal(r["HPPMSebelum"]).ToString("N3");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Convert.ToDecimal(r["HPPMSesudah"]).ToString("N3");
                //tr.Cells.Add(td);
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Convert.ToDecimal(r["HargaAwal"]).ToString("N2");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Convert.ToDecimal(r["HargaSebelum"]).ToString("N2");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Convert.ToDecimal(r["HargaSesudah"]).ToString("N2");
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["NettoWeight"]).ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["GrossWeight"]).ToString("N3");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;

            }


        }


    }

    protected string TD_Mutasi(decimal OldPrice, decimal NewPrice)
    {
        string x = string.Empty;
        decimal mutasi = 0m;
        if (OldPrice == 0) mutasi = ((NewPrice - OldPrice) / 1) * 100;
        else mutasi = ((NewPrice - OldPrice) / OldPrice) * 100;

        string sgn = mutasi > 0 ? "+" : "";
        string css = mutasi > 10 || mutasi < -10 ? "text-red" : "";

        x += "<p class=\"" + css + " text-lg\">";
        x += sgn;
        x += Str.Number2(mutasi);
        x += "%";
        x += "</p>";

        return x;
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productmodel.SelectedValue = Request.QueryString["productmodel"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            orderby.SelectedValue = Request.QueryString["orderby"];
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];

        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productmodel=" + productmodel.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }
    protected void ExportList(object sender, EventArgs e)
    {
        string href = "DJ_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productmodel=" + productmodel.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "DJ_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productmodel=" + productmodel.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }
    protected void Execute(object sender, EventArgs e)
    {
        /*var rs = from p in db.RevalProductDJ

                 select p;

        rs = rs.OrderBy(p => p.RevalProductDJ_CharProduct.ProductItem);

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "");
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.RevalProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.RevalProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productmodel.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.RevalProductDJ_CharProduct.ProductCategory == ProductCategory);*/
        string spname = "sp_getExecute_reval_pg";
        string query = " exec " + spname
                     + " '" + kw.Text + "', "
                     + lokasi.SelectedValue + ", "
                     + tipelokasi.SelectedValue + ", "
                     + productitem.SelectedValue + ", "
                     + productmodel.SelectedValue + ", "
                     + productlevel.SelectedValue;

        DataTable dt = Db.Rs(query, Db.Cnn_DB);

        string error = "";

        if (dt.Rows.Count > 0)
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("ID");

            foreach (DataRow x in dt.Rows)
            {
                DataRow dr = dt1.NewRow();
                dr["ID"] = x["ID"];
                dt1.Rows.Add(dr);
            }

            using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "dbo.sp_ExecuteReval_pg"; //The name of the above mentioned stored procedure.  
                SqlParameter param = cmd.Parameters.AddWithValue("@IDs", dt1); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productmodel=" + productmodel.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&error=" + HttpUtility.UrlEncode(error);
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }


    protected void Delete(object sender, EventArgs e)
    {
        int index = 1;

        CheckBox cb = new CheckBox();
        string hf = "";

        DataTable dt = new DataTable();
        dt.Columns.Add("ID");

        foreach (TableRow row in tb.Rows)
        {

            try
            {

                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                cb = (CheckBox)row.FindControl("cb_po" + index.ToString());
                if (cb.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = hf;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
            }
            index++;
        }

        using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_delateReval_pg"; //The name of the above mentioned stored procedure.  
            SqlParameter param = cmd.Parameters.AddWithValue("@IDs", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        string href = "PG_List.aspx?refreshPage=1&d=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productmodel=" + productmodel.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
    }

    protected void Apply(object sender, EventArgs e)
    {
        /// NEW APPLY FUNCTION : APPLY CHECKED ITEM ONLY

        int index = 1;
        int count_success = 0;

        CheckBox cb = new CheckBox();
        string hf = "";

        string errmsg = "";

        //int test_index = 1;
        //string test_msg = "";

        string msg_plu_error = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");

        foreach (TableRow row in tb.Rows)
        {
            //test_msg += test_index.ToString();
            //test_index++;

            if (index < tb.Rows.Count)
            {
                hf = ((HiddenField)row.FindControl("hf" + index)).Value;
                cb = (CheckBox)row.FindControl("cb_po" + index);

                //hf = ((HiddenField)row.FindControl("hf1")).Value;
                //cb = (CheckBox)row.FindControl("cb_po1");

                if (cb.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = hf;
                    dt.Rows.Add(dr);

                    // close if not null
                }
                // close cb checked
            }
            // close looping checkbox

            //catch (Exception ex) { Response.Write(ex.ToString()); }

            index++;
        }

        using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_applyReval_pg"; //The name of the above mentioned stored procedure.  
            SqlParameter param = cmd.Parameters.AddWithValue("@IDs", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  
            SqlParameter param2 = cmd.Parameters.Add("@User", SqlDbType.VarChar);
            param2.Value = App.UserNama;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        // clear data row
        //for (int i = 1; i < tb.Rows.Count; i++)
        //{
        //    tb.Rows.RemoveAt(i);
        //}
        //tb.Rows.Clear();

        //fillTable(sender, e);

        //string href = App.RefreshURL(this);
        //string href = "DJ_List_v2.aspx?refreshPage=1&d=1&totalproduct=" + index;

        //Customize-Qstr

        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&d=1";
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productmodel=" + productmodel.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "ExportDJ.aspx?t=expport";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productmodel=" + productmodel.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;

        Response.Write("<script>window.open('" + href + "');</script>");
    }

    //Apply Reval
    //insert product log
    protected int insertProduct(StockProductDJ d)
    {
        RevalLogProductDJ prod = new RevalLogProductDJ();

        prod.Nomor = d.Nomor;

        //prod.Operator = d.Operator;
        prod.Operator = App.UserNama;
        prod.OperatorTgl = DateTime.Now;
        //prod.Approval = d.Approval;
        prod.Approval = App.UserNama;
        prod.ApprovalTgl = DateTime.Now;
        prod.ImgPicture = d.ImgPicture;

        db.RevalLogProductDJ.InsertOnSubmit(prod);
        db.SubmitChanges();

        int ProdID = Db.SingleInteger("SELECT TOP 1 ID FROM RevalLogProductDJ ORDER BY ID DESC", Db.Cnn_DB);

        var cek1 = db.RevalLogProductDJ_CharProduct.SingleOrDefault(p => p.ID == ProdID);

        if (cek1 == null)
        {
            RevalLogProductDJ_CharProduct pd = new RevalLogProductDJ_CharProduct();
            pd.ID = ProdID;
            pd.ProductItem = d.StockProductDJ_CharProduct.ProductItem;
            pd.ProductCategory = d.StockProductDJ_CharProduct.ProductCategory;
            pd.ProductLevel = d.StockProductDJ_CharProduct.ProductLevel;
            pd.StoneDist = d.StockProductDJ_CharProduct.StoneDist;
            pd.FrameColor = d.StockProductDJ_CharProduct.FrameColor;
            pd.FrameFinishing = d.StockProductDJ_CharProduct.FrameFinishing;
            pd.FrameMaterial = d.StockProductDJ_CharProduct.FrameMaterial;
            pd.ProcessCons = d.StockProductDJ_CharProduct.ProcessCons;
            pd.GrossWeight = d.StockProductDJ_CharProduct.GrossWeight;
            pd.StoneCarat = d.StockProductDJ_CharProduct.StoneCarat;
            pd.StoneWeight = d.StockProductDJ_CharProduct.StoneWeight;
            pd.NettoWeight = d.StockProductDJ_CharProduct.NettoWeight;
            pd.KadarLogam = d.StockProductDJ_CharProduct.KadarLogam;
            pd.KadarTukaran = d.StockProductDJ_CharProduct.KadarTukaran;
            pd.DimensiD = d.StockProductDJ_CharProduct.DimensiD;
            pd.DimensiL = d.StockProductDJ_CharProduct.DimensiL;
            pd.DimensiP = d.StockProductDJ_CharProduct.DimensiP;
            pd.DimensiR = d.StockProductDJ_CharProduct.DimensiR;
            pd.StoneQty = d.StockProductDJ_CharProduct.StoneQty;

            db.RevalLogProductDJ_CharProduct.InsertOnSubmit(pd);
            db.SubmitChanges();
        }

        var cek2 = db.RevalLogProductDJ_Costing.SingleOrDefault(p => p.ID == ProdID);

        if (cek2 == null)
        {

            RevalLogProductDJ_Costing cost = new RevalLogProductDJ_Costing();
            cost.ID = ProdID;
            cost.UnitPriceOld = d.StockProductDJ_Costing.UnitPrice;
            cost.FrameOld = d.StockProductDJ_Costing.Frame;
            cost.MountingNOOld = d.StockProductDJ_Costing.MountingNO;
            cost.MountingROOld = d.StockProductDJ_Costing.MountingRO;
            cost.Pembagi = d.StockProductDJ_Costing.Pembagi.Value; /// no old
            cost.SettingOld = d.StockProductDJ_Costing.Setting;
            cost.FinishingOld = d.StockProductDJ_Costing.Finishing;
            cost.OngkosLainCOGSOld = d.StockProductDJ_Costing.OngkosLainCOGS;
            cost.OngkosLainOld = d.StockProductDJ_Costing.OngkosLain;
            cost.TotalRateOld = d.StockProductDJ_Costing.TotalRate.Value;
            cost.TotalRupiahOld = d.StockProductDJ_Costing.TotalRupiah.Value;
            cost.TotalBiayaOld = d.StockProductDJ_Costing.TotalBiaya;

            db.RevalLogProductDJ_Costing.InsertOnSubmit(cost);
            db.SubmitChanges();
        }

        var cek3 = db.RevalLogProductDJ_CostingProduct.SingleOrDefault(p => p.ID == ProdID);

        if (cek3 == null)
        {

            RevalLogProductDJ_CostingProduct cp = new RevalLogProductDJ_CostingProduct();
            cp.ID = ProdID;
            cp.GoldRateOld = d.StockProductDJ_CostingProduct.GoldRate;
            cp.Netto = d.StockProductDJ_CostingProduct.Netto; /// no old
            cp.Kadar = d.StockProductDJ_CostingProduct.Kadar; /// no old
            cp.TotalProductOld = d.StockProductDJ_CostingProduct.TotalProduct;
            cp.TotalStone1AOld = d.StockProductDJ_CostingProduct.TotalStone1A;
            cp.TotalStone1BOld = d.StockProductDJ_CostingProduct.TotalStone1B;
            cp.TotalStone2Old = d.StockProductDJ_CostingProduct.TotalStone2;
            cp.TotalStone3Old = d.StockProductDJ_CostingProduct.TotalStone3;
            cp.TotalStone4Old = d.StockProductDJ_CostingProduct.TotalStone4;
            cp.TotalStone5Old = d.StockProductDJ_CostingProduct.TotalStone5;
            cp.TotalOld = d.StockProductDJ_CostingProduct.Total;
            cp.EfektifGoldOld = d.StockProductDJ_CostingProduct.EfektifGold;

            db.RevalLogProductDJ_CostingProduct.InsertOnSubmit(cp);
            db.SubmitChanges();
        }

        var l_fns = from p in db.StockProductDJ_Finishing
                    where p.IDForm == d.ID
                    select p;
        if (l_fns != null)
        {
            foreach (var r in l_fns)
            {
                RevalLogProductDJ_Finishing fns = new RevalLogProductDJ_Finishing();
                fns.IDForm = ProdID; /// no old
                fns.IDFinishing = r.IDFinishing; /// no old
                fns.BiayaOld = r.Biaya;
                fns.BiayaModalOld = r.BiayaModal;
                fns.EfektifOld = r.Efektif;
                db.RevalLogProductDJ_Finishing.InsertOnSubmit(fns);
                db.SubmitChanges();
            }
        }

        var cek4 = db.RevalLogProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ProdID);

        if (cek4 == null)
        {

            RevalLogProductDJ_PricingBiaya by = new RevalLogProductDJ_PricingBiaya();
            by.ID = ProdID;
            by.NamaBrand = d.StockProductDJ_PricingBiaya.NamaBrand; /// no old
            by.NamaItem = d.StockProductDJ_PricingBiaya.NamaItem; /// no old
            by.NamaCons = d.StockProductDJ_PricingBiaya.NamaCons; /// no old
            by.MountingOld = d.StockProductDJ_PricingBiaya.Mounting;
            by.MountingNOOld = d.StockProductDJ_PricingBiaya.MountingNO;
            by.MountingROOld = d.StockProductDJ_PricingBiaya.MountingRO;
            by.SettingOld = d.StockProductDJ_PricingBiaya.Setting;
            by.FinishingOld = d.StockProductDJ_PricingBiaya.Finishing;
            by.TotalRupiahOld = d.StockProductDJ_PricingBiaya.TotalRupiah;
            by.TotalRateOld = d.StockProductDJ_PricingBiaya.TotalRate;
            by.TotalOld = d.StockProductDJ_PricingBiaya.Total;
            by.EfektifRateOld = d.StockProductDJ_PricingBiaya.EfektifRate;
            by.EfektifMountingOld = d.StockProductDJ_PricingBiaya.EfektifMounting;
            by.EfektifSettingOld = d.StockProductDJ_PricingBiaya.EfektifSetting;

            db.RevalLogProductDJ_PricingBiaya.InsertOnSubmit(by);
            db.SubmitChanges();
        }

        var cek5 = db.RevalLogProductDJ_PricingMU.SingleOrDefault(p => p.ID == ProdID);

        if (cek5 == null)
        {

            RevalLogProductDJ_PricingMU mu = new RevalLogProductDJ_PricingMU();
            mu.ID = ProdID;
            mu.NamaCategory = d.StockProductDJ_PricingMU.NamaCategory;
            mu.NamaLevel = d.StockProductDJ_PricingMU.NamaLevel;
            mu.NamaDist = d.StockProductDJ_PricingMU.NamaDist;
            mu.ModalUSDOld = d.StockProductDJ_PricingMU.ModalUSD;
            mu.ModalRateOld = d.StockProductDJ_PricingMU.ModalRate;
            mu.ModalRupiahOld = d.StockProductDJ_PricingMU.ModalRupiah;
            mu.MUOld = d.StockProductDJ_PricingMU.MU;
            mu.HargaUSDOld = d.StockProductDJ_PricingMU.HargaUSD;
            mu.HargaRupiahOld = d.StockProductDJ_PricingMU.HargaRupiah;
            mu.HargaRupiahRoundOld = d.StockProductDJ_PricingMU.HargaRupiahRound;
            mu.EfektifRateOld = d.StockProductDJ_PricingMU.EfektifRate;

            db.RevalLogProductDJ_PricingMU.InsertOnSubmit(mu);
            db.SubmitChanges();
        }

        var cek6 = db.RevalLogProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ProdID);

        if (cek6 == null)
        {

            RevalLogProductDJ_PricingProduct prc = new RevalLogProductDJ_PricingProduct();
            prc.ID = ProdID;
            prc.GoldRateOld = d.StockProductDJ_PricingProduct.GoldRate;
            prc.Netto = d.StockProductDJ_PricingProduct.Netto;
            prc.Kadar = d.StockProductDJ_PricingProduct.Kadar;
            prc.TotalProductOld = d.StockProductDJ_PricingProduct.TotalProduct;
            prc.TotalStone1AOld = d.StockProductDJ_PricingProduct.TotalStone1A;
            prc.TotalStone1BOld = d.StockProductDJ_PricingProduct.TotalStone1B;
            prc.TotalStone2Old = d.StockProductDJ_PricingProduct.TotalStone2;
            prc.TotalStone3Old = d.StockProductDJ_PricingProduct.TotalStone3;
            prc.TotalStone4Old = d.StockProductDJ_PricingProduct.TotalStone4;
            prc.TotalStone5Old = d.StockProductDJ_PricingProduct.TotalStone5;
            prc.TotalOld = d.StockProductDJ_PricingProduct.Total;
            prc.EfektifGoldOld = d.StockProductDJ_PricingProduct.EfektifGold;

            db.RevalLogProductDJ_PricingProduct.InsertOnSubmit(prc);
            db.SubmitChanges();
        }

        var cek7 = db.RevalLogProductDJ_PricingSegmen.SingleOrDefault(p => p.ID == ProdID);

        if (cek7 == null)
        {

            RevalLogProductDJ_PricingSegmen sg = new RevalLogProductDJ_PricingSegmen();
            sg.ID = ProdID;
            sg.ProductSegmen = d.StockProductDJ_PricingSegmen.ProductSegmen;
            sg.NamaCategory = d.StockProductDJ_PricingSegmen.NamaCategory;
            sg.NamaLevel = d.StockProductDJ_PricingSegmen.NamaLevel;
            sg.NamaDist = d.StockProductDJ_PricingSegmen.NamaDist;
            sg.SegmenSistemOld = d.StockProductDJ_PricingSegmen.SegmenSistem;
            sg.SegmenMinimumOld = d.StockProductDJ_PricingSegmen.SegmenMinimum;
            sg.TotalCarat = d.StockProductDJ_PricingSegmen.TotalCarat; /// no old
            sg.HargaUSDOld = d.StockProductDJ_PricingSegmen.HargaUSD;
            sg.EfektifSegmenOld = d.StockProductDJ_PricingSegmen.EfektifSegmen;

            db.RevalLogProductDJ_PricingSegmen.InsertOnSubmit(sg);
            db.SubmitChanges();
        }

        var s1A = from p in db.StockProductDJ_Stone1A
                  where p.IDForm == d.ID
                  select p;
        if (s1A != null)
        {
            foreach (var r in s1A)
            {
                RevalLogProductDJ_Stone1A stone = new RevalLogProductDJ_Stone1A();
                stone.IDForm = ProdID;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.HargaSatuanOld = r.HargaSatuan;
                stone.HargaTotalOld = r.HargaTotal;
                stone.HargaSatuanMOld = r.HargaSatuanM;
                stone.HargaTotalMOld = r.HargaTotalM;
                stone.EfektifOld = r.Efektif;
                stone.SettingOld = r.Setting;
                db.RevalLogProductDJ_Stone1A.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }

        var s1B = from p in db.StockProductDJ_Stone1B
                  where p.IDForm == d.ID && p.StatusTerpasang == 1
                  select p;
        if (s1B != null)
        {
            foreach (var r in s1B)
            {
                RevalLogProductDJ_Stone1B stone = new RevalLogProductDJ_Stone1B();
                stone.IDForm = ProdID;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.HargaSatuanOld = r.HargaSatuan;
                stone.HargaTotalOld = r.HargaTotal;
                stone.GIA = r.GIA;
                stone.StatusTerpasang = r.StatusTerpasang; /// no old
                stone.HargaInputHOld = r.HargaInputH;
                stone.HargaInputPOld = r.HargaInputP;
                stone.HargaSatuanMOld = r.HargaSatuanM;
                stone.HargaTotalMOld = r.HargaTotalM;
                stone.EfektifOld = r.Efektif;
                stone.SettingOld = r.Setting;
                db.RevalLogProductDJ_Stone1B.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }

        var s2 = from p in db.StockProductDJ_Stone2
                 where p.IDForm == d.ID
                 select p;
        if (s2 != null)
        {
            foreach (var r in s2)
            {
                RevalLogProductDJ_Stone2 stone = new RevalLogProductDJ_Stone2();
                stone.IDForm = ProdID;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.HargaSatuanOld = r.HargaSatuan;
                stone.HargaTotalOld = r.HargaTotal;
                stone.HargaSatuanMOld = r.HargaSatuanM;
                stone.HargaTotalMOld = r.HargaTotalM;
                stone.EfektifOld = r.Efektif;
                stone.SettingOld = r.Setting;
                db.RevalLogProductDJ_Stone2.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }

        var s3 = from p in db.StockProductDJ_Stone3
                 where p.IDForm == d.ID
                 select p;
        if (s3 != null)
        {
            foreach (var r in s3)
            {
                RevalLogProductDJ_Stone3 stone = new RevalLogProductDJ_Stone3();
                stone.IDForm = ProdID;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.HargaSatuanOld = r.HargaSatuan;
                stone.HargaTotalOld = r.HargaTotal;
                stone.HargaSatuanMOld = r.HargaSatuanM;
                stone.HargaTotalMOld = r.HargaTotalM;
                stone.EfektifOld = r.Efektif;
                stone.SettingOld = r.Setting;
                db.RevalLogProductDJ_Stone3.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }

        var s4 = from p in db.StockProductDJ_Stone4
                 where p.IDForm == d.ID
                 select p;
        if (s4 != null)
        {
            foreach (var r in s4)
            {
                RevalLogProductDJ_Stone4 stone = new RevalLogProductDJ_Stone4();
                stone.IDForm = ProdID;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.HargaSatuanOld = r.HargaSatuan;
                stone.HargaTotalOld = r.HargaTotal;
                stone.HargaSatuanMOld = r.HargaSatuanM;
                stone.HargaTotalMOld = r.HargaTotalM;
                stone.EfektifOld = r.Efektif;
                stone.SettingOld = r.Setting;
                db.RevalLogProductDJ_Stone4.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }


        var s5 = from p in db.StockProductDJ_Stone5
                 where p.IDForm == d.ID
                 select p;
        if (s5 != null)
        {
            foreach (var r in s5)
            {
                RevalLogProductDJ_Stone5 stone = new RevalLogProductDJ_Stone5();
                stone.IDForm = ProdID;
                stone.IDStone = r.IDStone;
                stone.TotalButir = r.TotalButir;
                stone.TotalCarat = r.TotalCarat;
                stone.CaratButir = r.CaratButir;
                stone.DimensiL = r.DimensiL;
                stone.DimensiP = r.DimensiP;
                stone.DimensiT = r.DimensiT;
                stone.HargaSatuanOld = r.HargaSatuan;
                stone.HargaTotalOld = r.HargaTotal;
                stone.HargaSatuanMOld = r.HargaSatuanM;
                stone.HargaTotalMOld = r.HargaTotalM;
                stone.EfektifOld = r.Efektif;
                stone.SettingOld = r.Setting;
                db.RevalLogProductDJ_Stone5.InsertOnSubmit(stone);
                db.SubmitChanges();
            }
        }

        return ProdID;
    }

    protected void deleteProcess(RevalProductDJ d)
    {
        if (d.RevalProductDJ_CharDesign != null)
            db.RevalProductDJ_CharDesign.DeleteOnSubmit(
                d.RevalProductDJ_CharDesign);

        if (d.RevalProductDJ_CharProduct != null)
            db.RevalProductDJ_CharProduct.DeleteOnSubmit(
                d.RevalProductDJ_CharProduct);

        if (d.RevalProductDJ_Costing != null)
            db.RevalProductDJ_Costing.DeleteOnSubmit(
                d.RevalProductDJ_Costing);

        if (d.RevalProductDJ_CostingProduct != null)
            db.RevalProductDJ_CostingProduct.DeleteOnSubmit(
                d.RevalProductDJ_CostingProduct);

        var s1A = db.RevalProductDJ_Stone1A.Where(p => p.IDForm == d.ID);
        if (s1A != null)
        {
            db.RevalProductDJ_Stone1A.DeleteAllOnSubmit(s1A);
            db.SubmitChanges();
        }

        var s1B = db.RevalProductDJ_Stone1B.Where(p => p.IDForm == d.ID);
        if (s1B != null)
        {
            db.RevalProductDJ_Stone1B.DeleteAllOnSubmit(s1B);
            db.SubmitChanges();
        }

        var s2 = db.RevalProductDJ_Stone2.Where(p => p.IDForm == d.ID);
        if (s2 != null)
        {
            db.RevalProductDJ_Stone2.DeleteAllOnSubmit(s2);
            db.SubmitChanges();
        }

        var s3 = db.RevalProductDJ_Stone3.Where(p => p.IDForm == d.ID);
        if (s3 != null)
        {
            db.RevalProductDJ_Stone3.DeleteAllOnSubmit(s3);
            db.SubmitChanges();
        }

        var s4 = db.RevalProductDJ_Stone4.Where(p => p.IDForm == d.ID);
        if (s4 != null)
        {
            db.RevalProductDJ_Stone4.DeleteAllOnSubmit(s4);
            db.SubmitChanges();
        }

        var s5 = db.RevalProductDJ_Stone5.Where(p => p.IDForm == d.ID);
        if (s5 != null)
        {
            db.RevalProductDJ_Stone5.DeleteAllOnSubmit(s5);
            db.SubmitChanges();
        }

        var fn = db.RevalProductDJ_Finishing.Where(p => p.IDForm == d.ID);
        if (fn != null)
        {
            db.RevalProductDJ_Finishing.DeleteAllOnSubmit(fn);
            db.SubmitChanges();
        }

        if (d.RevalProductDJ_PricingBiaya != null)
            db.RevalProductDJ_PricingBiaya.DeleteOnSubmit(
                d.RevalProductDJ_PricingBiaya);

        if (d.RevalProductDJ_PricingMU != null)
            db.RevalProductDJ_PricingMU.DeleteOnSubmit(
                d.RevalProductDJ_PricingMU);

        if (d.RevalProductDJ_PricingProduct != null)
            db.RevalProductDJ_PricingProduct.DeleteOnSubmit(
                d.RevalProductDJ_PricingProduct);

        if (d.RevalProductDJ_PricingSegmen != null)
            db.RevalProductDJ_PricingSegmen.DeleteOnSubmit(
                d.RevalProductDJ_PricingSegmen);

        if (d != null)
            db.RevalProductDJ.DeleteOnSubmit(d);

        if (d.RevalItemDJ != null)
            db.RevalItemDJ.DeleteOnSubmit(d.RevalItemDJ);

        //Commit
        db.SubmitChanges();
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 0 || Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            string t = "All";
            string v = "0";
            ListItem item = new ListItem(t, v);
            lokasi.Items.Add(item);
            DLC.LocationList(lokasi, Tipe);
        }
        lokasi.Focus();
    }
}