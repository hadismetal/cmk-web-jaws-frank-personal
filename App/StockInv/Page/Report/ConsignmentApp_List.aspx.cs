using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
// using Newtonsoft.Json;


public partial class Page_Approval_ConsignmentApp_List : System.Web.UI.Page
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
    // public Dictionary<string, object> respon { get; private set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);
        // respon = new Dictionary<string, object>();
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
            
            fillFilter();
            if (App.Refresh(this))fillTable1(sender, e);
            //Set();
        }
    }

    //Display Table

    protected void fillFilter(){  
        if (App.Refresh(this))
        {          
            datepicker.Text = Request.QueryString["datepicker"];
            datepicker2.Text = Request.QueryString["datepicker2"];
            ddl_ProductLevel.SelectedValue = Request.QueryString["ddl_ProductLevel"];            
            ddl_ProductItem.SelectedValue = Request.QueryString["ddl_ProductItem"];
            ddl_Model.SelectedValue = Request.QueryString["ddl_Model"];
            ddl_RangkaWarna.SelectedValue = Request.QueryString["ddl_RangkaWarna"];
            ddl_Supplier.SelectedValue = Request.QueryString["ddl_Supplier"];
            
        }
    }

    protected void btn_display_Click(object sender, EventArgs e)    
    {
            fillTable(sender,e);
    }

    protected void btn_display_Header(object sender, EventArgs e){
        string href = App.RefreshURL(this);
            
        href += "&datepicker=" + datepicker.Text;
        href += "&datepicker2=" + datepicker2.Text;
        href += "&ddl_ProductLevel=" + ddl_ProductLevel.Text;        
        href += "&ddl_ProductItem=" + ddl_ProductItem.Text;
        href += "&ddl_Model=" + ddl_Model.Text;
        href += "&ddl_RangkaWarna=" + ddl_RangkaWarna.Text;
        href += "&ddl_Supplier=" + ddl_Supplier.Text;                       

        Response.Redirect(href);
    }


    protected void fillTable(object sender,EventArgs e){
        DateTime dateStart = DateTime.Parse(datepicker.Text);
        DateTime dateEnd = DateTime.Parse(datepicker2.Text);
        tableHeader.Style.Add("display","none");
        tableDetail.Attributes.Remove("style");


        string supplierSJ = SupplierSJ.Value;
        string queryIsUserMerchOrIT = "SELECT COUNT('') [Total] FROM UserAccount A JOIN [DB_PROD_MYAPPS].[dbhrd].[dbo].[mskaryawan] B ON A.UserNumber COLLATE SQL_Latin1_General_CP1_CI_AS = B.m_nik COLLATE SQL_Latin1_General_CP1_CI_AS WHERE B.m_departemen IN ('95') AND A.ID = " + App.UserID;
        int isUserMerchOrIT = Db.SingleInteger(queryIsUserMerchOrIT, Db.Cnn_AC);
        var query = "EXEC dbo.sp_getStockConsignment "+ ddl_ProductItem.SelectedValue +","+ ddl_ProductLevel.SelectedValue +","+ ddl_Model.SelectedValue +","+ ddl_RangkaWarna.SelectedValue +","+ ddl_Supplier.SelectedValue +",'"+ dateStart.ToString("yyyy-MM-dd") +"','"+dateEnd.ToString("yyyy-MM-dd")+"',"+App.UserID+","+isUserMerchOrIT+",'detail','"+supplierSJ+"','Report'";
        DataSet ds = QuerySPDataSet(query, Db.Cnn_DB);
        // DataTable rs1 = Exec(Db.Cnn_DB,query,parameters);        
        int index = 1;    
         
        var counter = 1;
        
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "console.log(`"+query+"`)", true);       

         try
         {
            foreach (DataRow rss in ds.Tables[0].Rows) {    

                TableRow tr = new TableRow();
				TableCell td;
    
				td = new TableCell();
                td.Text = "<p>" + counter.ToString() + ".</p>";
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);				

                td = new TableCell();
                td.Text =(string)rss["SupplierSJ"];
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text =(string)rss["KodeBarang"];
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text ="<input runat='server' ClientIDMode='Static' type='hidden' name='PLU' id='PLU"+counter+"' value='"+(string)rss["PLU"]+"'>"+(string)rss["PLU"];
                td.Attributes.CssStyle.Add("text-align", "center");                                 
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text =(string)rss["model"];
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text =(string)rss["item"];
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text =(string)rss["warna"];
                td.Attributes.CssStyle.Add("text-align", "justify"); 
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text =Convert.ToDecimal(rss["GrossWeight"]).ToString("N2");
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text =Convert.ToDecimal(rss["DimensiR"]).ToString("N0");
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text =(string)rss["tanggalPengajuan"];
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);                
                string label;
                if ((int)rss["Status1"] == 0){
                    label = "<span class='label label-warning'>No approval yet</span>";
                }
                else if ((int)rss["Status1"] == 1){
                    label = "<span class='label label-success' >Approved by "+ (string)rss["approver"]+" </span>";
                }else{
                    label = "<a type='button' onclick='openModalR1("+counter+")' data-plu='"+(string)rss["PLU"]+"' data-ket='"+ Regex.Replace((string)rss["Keterangan"] , @"[^a-zA-Z0-9\s]", "")+"' id='btn_btn"+counter+"'><span class='label label-danger'>Reject By "+ (string)rss["approver"] +" on "+ (string)rss["tanggalApprove"] +" </span></a>";
                }

                td = new TableCell();
                td.Text = label;
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);                
                               

                tb.Rows.Add(tr);
                counter++;
            }            
            
         } catch (Exception ex) {
             ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "console.log([`"+ query+"`])", true);                
             Response.Write(ex.Message.ToString());
        } 
    }
    

    protected void fillTable1(object sender,EventArgs e){
          DateTime dateStart = DateTime.Parse(datepicker.Text);
        DateTime dateEnd = DateTime.Parse(datepicker2.Text);
        
         string queryIsUserMerchOrIT = "SELECT COUNT('') [Total] FROM UserAccount A JOIN [DB_PROD_MYAPPS].[dbhrd].[dbo].[mskaryawan] B ON A.UserNumber COLLATE SQL_Latin1_General_CP1_CI_AS = B.m_nik COLLATE SQL_Latin1_General_CP1_CI_AS WHERE B.m_departemen IN ('95') AND A.ID = " + App.UserID;
        int isUserMerchOrIT = Db.SingleInteger(queryIsUserMerchOrIT, Db.Cnn_AC);
        var query = "EXEC dbo.sp_getStockConsignment "+ ddl_ProductItem.SelectedValue +","+ ddl_ProductLevel.SelectedValue +","+ ddl_Model.SelectedValue +","+ ddl_RangkaWarna.SelectedValue +","+ ddl_Supplier.SelectedValue +",'"+ dateStart.ToString("yyyy-MM-dd") +"','"+dateEnd.ToString("yyyy-MM-dd")+"',"+App.UserID+","+isUserMerchOrIT+",'header','','Report'";
        DataSet ds = QuerySPDataSet(query, Db.Cnn_DB);        
        int index = 1;    
        // DataTable dtMember = new DataTable("AdditionalMember");
        // DataRow drMember;
        // dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("id"), new DataColumn("check"), new DataColumn("SJSupp"), new DataColumn("kodebarang"), new DataColumn("PLU"),  new DataColumn("model"), new DataColumn("product_item"),new DataColumn("color"), new DataColumn("berat"),new DataColumn("dimensi"),new DataColumn("tgl") });
        var counter = 1;        
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "console.log(`"+query+"`)", true);        

        
         try
         {
            foreach (DataRow rss in ds.Tables[0].Rows) {    

                TableRow tr = new TableRow();
				TableCell td;
    
				td = new TableCell();
                td.Text = "<p>" + counter.ToString() + ".</p>";
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);
				                                

                td = new TableCell();
                td.Text =TD_Hyperlink((string)rss["SupplierSJ"]);
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);
                
                td = new TableCell();
                td.Text =Convert.ToDecimal(rss["totalQuantity"]).ToString("N0");
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text =Convert.ToDecimal(rss["totalBerat"]).ToString("N2");
                td.Attributes.CssStyle.Add("text-align", "center"); 
                tr.Cells.Add(td);

                // td = new TableCell();
                // td.Text =(string)rss["tanggalPengajuan"];
                // td.Attributes.CssStyle.Add("text-align", "center"); 
                // tr.Cells.Add(td);                               

                tbh.Rows.Add(tr);
                counter++;
            }            
            
         } catch (Exception ex) {
             ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "console.log([`"+ query+"`])", true);                
             Response.Write(ex.Message.ToString());
        }  
    }
    // protected void btn_submit_Click(object sender, EventArgs e)
    // {

    //     Response.Write("asd");
    //     for (var i = 0; i < EmasGridView.Rows.Count; i++)
    //     {
    //         var gridViewRowTemp = EmasGridView.Rows[i];

    //         string PLU = ((Literal)gridViewRowTemp.Cells[1].FindControl("PLU")).Text;
    //         bool Check = ((CheckBox)gridViewRowTemp.Cells[1].FindControl("check")).Checked;

    //         // var rs = from p in db.StockReceivePG
    //         //         where p.NoTTB == "" + NoTTB + "" && p.Approval == null
    //         //         select p;

    //         // if (rs != null)
    //         // {
    //         //     foreach (var r in rs)
    //         //     {

    //         //         if (Check)
    //         //         {
    //         //             r.Approval = App.UserNama;
    //         //             r.ApprovalTgl = DateTime.Now;

    //         //             db.SubmitChanges();

    //         //             StockProductPG x = new StockProductPG();

    //         //             x.NomorSertifikat = r.NomorSertifikat;
    //         //             x.IDBrand = r.LocWarehouse.CompanyBrand.ID;
    //         //             x.KodeBarang = r.KodeBarang;
    //         //             x.NamaBarang = r.NamaBarang;
    //         //             x.Tgl = r.Tgl;
    //         //             x.TglTJT = r.TglTJT;
    //         //             x.NoTTB = r.NoTTB;
    //         //             x.IDWarehouse = r.IDWarehouse;
    //         //             x.Nomor = r.Nomor;
    //         //             x.SupplierNama = r.SupplierNama;
    //         //             x.SupplierSJ = r.SupplierSJ;
    //         //             x.Keterangan = (r.Keterangan != null) ? r.Keterangan : "";
    //         //             x.Draft = r.Draft;
    //         //             x.DraftDate = r.DraftDate;
    //         //             x.Operator = r.Operator;
    //         //             x.OperatorTgl = r.OperatorTgl;
    //         //             x.Approval = r.Approval;
    //         //             x.ApprovalTgl = r.ApprovalTgl;
    //         //             x.CatatanManager = r.CatatanManager;
    //         //             x.ImgPicture = r.ImgPicture;
    //         //             x.ProductLevel = r.ProductLevel;
    //         //             x.Model = r.Model;
    //         //             x.ProductItem = r.ProductItem;
    //         //             x.FrameColor = r.FrameColor;
    //         //             x.TargetAge = r.TargetAge;
    //         //             x.KadarLogam = r.KadarLogam;
    //         //             x.KadarTukaranJual = r.KadarTukaranJual;
    //         //             x.KadarTukaranBeli = r.KadarTukaranBeli;
    //         //             x.GrossWeight = r.GrossWeight;
    //         //             x.NettoWeight = r.NettoWeight;
    //         //             x.MountingM = r.MountingM;
    //         //             x.MountingR = r.MountingR;
    //         //             x.MountingMUSD = r.MountingMUSD;
    //         //             x.MountingRUSD = r.MountingRUSD;
    //         //             x.GoldRate = r.GoldRate;
    //         //             x.EfektifGold = r.EfektifGold;
    //         //             x.Harga = r.Harga;
    //         //             x.HargaUSD = r.HargaUSD;
    //         //             x.TotalHarga = r.TotalHarga;
    //         //             x.TotalHargaJual = r.TotalHargaJual;
    //         //             x.TotalHargaUSD = r.TotalHargaUSD;
    //         //             x.TotalHargaJualUSD = r.TotalHargaJualUSD;
    //         //             x.TotalHarga24 = r.TotalHarga24;
    //         //             x.TotalRate = r.TotalRate;
    //         //             x.HumanStock = r.HumanStock;
    //         //             x.HumanStockNama = r.HumanStockNama;
    //         //             x.EfektifTotalRate = r.EfektifTotalRate;
    //         //             x.IDSupplier = r.IDSupplier;
    //         //             x.FixRate = r.FixRate;
    //         //             x.StatusLebur = false;
    //         //             x.ReturnProduct = false;
    //         //             x.StatusPenjualan = 0;
    //         //             x.DimensiR = (r.DimensiR.HasValue ? r.DimensiR.Value : 0m);

    //         //             db.StockProductPG.InsertOnSubmit(x);
    //         //             db.SubmitChanges();

    //         //             int LastID = x.ID;
    //         //             Db.Execute("UPDATE A SET NVProduct = NVReceive, KMProduct = KMReceive, Consignment = A.Cons FROM (SELECT A.Consignment as Cons, B.Consignment, A.NomorValidasi as NVReceive, B.NomorValidasi as NVProduct, A.KodeMaterial as KMReceive, B.KodeMaterial as KMProduct FROM StockReceivePG A JOIN StockProductPG B ON A.Nomor = B.Nomor AND A.ApprovalTgl = B.ApprovalTgl WHERE B.ID = "+ LastID +") A", Db.Cnn_DB);

    //         //             // Ledger 
    //         //             StockLedgerPG l = new StockLedgerPG();

    //         //             // Value (See Library DB - Ledger for Value and Remark)
    //         //             decimal Value = 1; // 1 : in , -1 : out
    //         //             int Remark = 4; // See Library DB - Ledger
    //         //             string Keterangan = x.Nomor; // Commonly filled by document number or id 
    //         //             if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

    //         //             //Input
    //         //             l.IDProduct = x.ID;
    //         //             l.TipeLokasi = 0;
    //         //             l.IDLokasi = r.IDWarehouse;
    //         //             l.NamaLokasi = r.LocWarehouse.Nama;
    //         //             l.Value = Value;
    //         //             l.Remark = Remark;
    //         //             l.Keterangan = Keterangan;

    //         //             //User-Info
    //         //             l.Operator = App.UserNama;
    //         //             l.OperatorTgl = DateTime.Now;

    //         //             //Commit
    //         //             db.StockLedgerPG.InsertOnSubmit(l);
    //         //             db.SubmitChanges();

    //         //             StockActualPG y = new StockActualPG();

    //         //             y.IDProduct = x.ID;
    //         //             y.IDBrand = r.LocWarehouse.CompanyBrand.ID;
    //         //             y.IDLokasi = r.IDWarehouse;
    //         //             y.NamaLokasi = r.LocWarehouse.Nama;
    //         //             y.TipeLokasi = 0;
    //         //             y.Nomor = r.Nomor;
    //         //             y.Keterangan = (r.Keterangan != null) ? r.Keterangan : "";
    //         //             y.InHand = 1;
    //         //             y.Operator = r.Operator;
    //         //             y.Substorage = 1;
    //         //             y.OperatorTgl = r.OperatorTgl.Value;

    //         //             db.StockActualPG.InsertOnSubmit(y);
    //         //             db.SubmitChanges();
						
	// 		// 			// Lib.InsertPlantToPlantToSAPStaging("", "", "DOPG", false, r.Nomor, "");
    //         //         }
    //         //     }
    //         // }        
    //     }
    //     // Response.Redirect("PG.aspx?d=1");
    // }

    protected string TD_Hyperlink(string SupplierSJ)
    {
        string x = string.Empty;

        x += "<a href='#' onclick='detail(event)' data-supp='"+SupplierSJ+"'>" + SupplierSJ + "</a>";

        return x;
    }
    protected DataSet QuerySPDataSet(string query, string connectionString)
	{
		DataSet ds = new DataSet();
		using (SqlConnection conn = new SqlConnection(connectionString))
		{
			conn.Open();
			SqlTransaction trans;
			trans = conn.BeginTransaction();
			try
			{
				SqlDataAdapter sda = new SqlDataAdapter(query, connectionString);
				sda.SelectCommand.CommandTimeout = 3000;
				sda.Fill(ds);
				sda.Dispose();
				trans.Commit();
				conn.Close();
			}
			catch(Exception ex)
			{
				string ErrorMessage = ex.Message.ToString();
				trans.Rollback();
				conn.Close();
			}
		}
		return ds;
	}
}