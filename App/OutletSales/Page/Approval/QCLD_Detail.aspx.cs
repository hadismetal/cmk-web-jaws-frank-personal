using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class Page_Report_QCLD_Detail : System.Web.UI.Page
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
	
    protected void Page_Load(object sender, EventArgs e)
    {
		App.Protect();
        App.ProtectRole(Role.Manager);
		
		if (!Page.IsPostBack)
        {
			var data = db.DocQCLD.SingleOrDefault(p => p.ID == myID).StockProductLD_Stone1B.StockActualLD_Stone1B;
			var so = db.SalesOrderLD.SingleOrDefault(p => p.IDProduct == data.IDProduct && p.SalesOrder.StatusPembayaran == true && p.SalesOrder.StatusVoid == false);
			var locationbrand_outlet = db.LocOutlet;
			var locationbrand_exhibition = db.LocExhibition;

			string CB = ""; // mondial, franknco, missmondial, thepalace
			string Alamat = "";
			if (data.TipeLokasi == 1) 
			{ 
				CB = locationbrand_outlet.SingleOrDefault(p => p.ID == data.IDLokasi).CompanyBrand.Nama; 
				Alamat = locationbrand_outlet.SingleOrDefault(p => p.ID == data.IDLokasi).AddrAlamat;
			}
			else if (data.TipeLokasi == 2) 
			{ 
				CB = locationbrand_exhibition.SingleOrDefault(p => p.ID == data.IDLokasi).CompanyBrand.Nama; 
				Alamat = locationbrand_exhibition.SingleOrDefault(p => p.ID ==data.IDLokasi).AddrAlamat;
			}

			CB = CB.ToLower();

			if (CB == "mondial") { img_logo_web.Src = "/Images/web/logo_web_mondial.jpg"; }
			else if (CB == "miss mondial") { img_logo_web.Src = "/Images/web/logo_web_missmondial.jpg"; }
			else if (CB == "the palace") { img_logo_web.Src = "/Images/web/logo_web_thepalace.jpg"; }
			else if (CB == "frank & co") { img_logo_web.Src = "/Images/web/logo_web_franknco.jpg"; }
			else { img_logo_web.Src = "/Images/web/logo_web_franknco.jpg"; }
			
			cabang.Text = Alamat;
			lit_nomor_repair.Text = db.DocQCLD.SingleOrDefault(p => p.ID == myID).Nomor;
			
			if(so != null)
			{
				invoice.InnerText = so.Invoice;
				cust.InnerText = so.SalesOrder.CustomerNama;
				if(so.SalesOrder.TipeLokasi == 1)
					storeasal.InnerText = Db.SingleString("SELECT TOP 1 UPPER(Nama) [Nama] FROM LocOutlet WHERE ID = " + so.SalesOrder.IDLokasi, Db.Cnn_DB);
				else if(so.SalesOrder.TipeLokasi == 2)
					storeasal.InnerText = Db.SingleString("SELECT TOP 1 UPPER(Nama) [Nama] FROM LocExhibition WHERE ID =" + so.SalesOrder.IDLokasi, Db.Cnn_DB);
				hg_beli.InnerText = "IDR " + (Convert.ToInt64(so.TotalBayar)).ToString("N0");
			}
			fillForm();
		}	
    }
	
	protected void fillForm()
    {
		var q = db.DocQCLD.SingleOrDefault(p => p.ID == myID);
        if (q.StockProductLD_Stone1B == null) App.DataNull(this);
        else
        {
			nodocqc.InnerText = q.Nomor;
			tgldocqc.InnerText = Convert.ToDateTime(q.OperatorTgl).ToString("dd-MM-yyyy");
			plu.InnerText = q.StockProductLD_Stone1B.Nomor;
			gia.InnerText = q.StockProductLD_Stone1B.GIA;
			petugasqc.InnerText = q.QCNama.ToUpper();
			sertifikat.InnerText = q.Sertifikat;
			valbeli.InnerText = Db.SingleDecimal("SELECT TOP 1 ISNULL(HargaJualCustomer,0) [HargaJualCustomer] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB).ToString("N0");
			item.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(Item,'') [Item] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			weight.InnerText = Db.SingleDecimal("SELECT TOP 1 ISNULL(Carat,0) [Carat] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB).ToString("N3");
			shape.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(Shape,'') [Shape] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			cla.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(Clarity,'') [Clarity] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			col.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(Colour,'') [Color] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			cut.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(Cutting,'') [Cutting] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			brand.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(Brand,'') [Brand] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			isanycertificate.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(CheckSertifikatBySM,'') [CheckSertifikatBySM] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			isanyinvoice.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(CheckInvoiceBySM,'') [CheckInvoiceBySM] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			laserincription.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(CheckLaserIncriptionBySM,'') [CheckLaserIncriptionBySM] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			measurement.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(CheckMeasurementBySM,'') [CheckMeasurementBySM] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			kondisibatu.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(CheckKondisiBatu,'') [CheckKondisiBatu] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			keterangan.InnerText = q.Keterangan;
			createdby.InnerText = q.QCNama.ToUpper();
		}

        string nik = string.Empty;
        string jabatan = string.Empty;
        using (Map_AC ac = new Map_AC())
        {
            var user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            if (user != null)
            {
                nik = user.UserNumber;
                jabatan = user.Keterangan;
            }
        }

        DataTable dt = new DataTable();
        if (nik != "")
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@NIK", SqlDbType.VarChar, 50){ Value = nik }
            };

            dt = Rs("SELECT * FROM DataProcurement WHERE NIK = @NIK", Db.Cnn_DB, parameters);
        }

        int rowcount = dt.Rows.Count;

        string approval = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalProcurement,'') [ApprovalProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
        string approval2 = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalProcurementFinal,'') [ApprovalProcurementFinal] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
		string approval3 = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalSM,'') [ApprovalSM] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
		string approval4 = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalAM,'') [ApprovalAM] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
		string keteranganproc = Db.SingleString("SELECT TOP 1 ISNULL(KeteranganProcurement,'') [KeteranganProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
		decimal hargaproc = Db.SingleDecimal("SELECT TOP 1 CAST(ISNULL(HargaProcurement,0) AS MONEY) [KeteranganProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
		
		string usernumber = Db.SingleString("SELECT TOP 1 ISNULL(UserNumber,'') [UserNumber] FROM UserAccount WHERE ID = " + App.UserID, Db.Cnn_AC);
		int countproc = Db.SingleInteger("SELECT COUNT('') [Total] FROM DataProcurement WHERE Disable = 0 AND NIK = '" + usernumber + "'", Db.Cnn_DB);
		int countproc2 = Db.SingleInteger("SELECT COUNT('') [Total] FROM DataProcurement WHERE Disable = 0 AND Role = 'Supervisor' AND NIK = '" + usernumber + "'", Db.Cnn_DB);
		
		string ekstension = Db.SingleString("SELECT TOP 1 ISNULL(Extension,'[]') [Extension] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
		try
		{
			List<ExtensionLD> test = JsonConvert.DeserializeObject<List<ExtensionLD>>(ekstension);
			fillExtensions(test);
		}
		catch{}
		
		if(approval3 == "")
		{
			approvesmby.InnerText = "______________________";
		}
		else
		{
			approvesmby.InnerText = approval3;
		}
		if(approval4 == "")
		{
			approveamby.InnerText = "______________________";
		}
		else
		{
			approveamby.InnerText = approval4;
		}
		
		if(approval != "")
		{
			divprint2.Visible = true;
			divprint.Visible = true;
			divapprove.Visible = false;
			panelapproved.Visible = true;
			panelnotapproved.Visible = false;
			divqcproc.Visible = true;
			lblwarning.Visible = false;
			divfinalprice.Visible = true;
			btnEdit.Visible = true;
			approveprocurementby.InnerText = approval;
			
			val.InnerText = Db.SingleDecimal("SELECT TOP 1 CAST(ISNULL(HargaProcurement,0) AS MONEY) [HargaProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB).ToString("N2");
			
			hargajualcust_1.Text = "$ " + Convert.ToDecimal(hargaproc).ToString("N2");
			keteranganprocurement_1.Text = keteranganproc;
			operatorprocurement.Text = approval;
			
			if(approval2 != "")
			{
				divprint2.Visible = false;
				divprint.Visible = false;
				divapprove.Visible = false;
				btnApprove.Visible = false;
				btnApproveFinal.Visible = false;
				btnEdit.Visible = false;
				lblwarning.Visible = true;
				approveprocurementfinalby.InnerText = approval2;
				lblwarning.InnerText = "Need Approval Merchandise Director. Please Contact Merchandise Director.";
			}
			else
			{
				if(countproc2 > 0)
				{
					DataTable tbl = Db.Rs("SELECT TOP 1 ISNULL(ApprovalSMTgl,'') [ApprovalSMTgl], ISNULL(ApprovalAMTgl,'') [ApprovalAMTgl], ISNULL(ApprovalProcurementTgl,'') [ApprovalProcurementTgl], ISNULL(ApprovalProcurement,'') [ApprovalProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);

					DateTime approvaltgl = DateTime.Now;
                    DateTime approvaltgl3 = Convert.ToDateTime(tbl.Rows[0]["ApprovalSMTgl"]);
                    DateTime approvaltgl4 = Convert.ToDateTime(tbl.Rows[0]["ApprovalAMTgl"]);
					DateTime approvaltgl5 = Convert.ToDateTime(tbl.Rows[0]["ApprovalProcurementTgl"]);
					string approvalprc = tbl.Rows[0]["ApprovalProcurement"].ToString();

                    approvaltgl = approvaltgl5;
                    tbl.Dispose();

					int countdays = (DateTime.Now.Date - approvaltgl.Date).Days;
                    if (approvaltgl == Convert.ToDateTime("1900-01-01 00:00:00.000"))
                        countdays = 0;

                    if (countdays <= 7)
					{
						divprint2.Visible = false;
						divprint.Visible = false;
						divapprove.Visible = true;
						btnApprove.Visible = false;
						//btnApprove.InnerText = "Submit New Price";
						btnApproveFinal.Visible = true;
						btnEdit.Visible = true;
						panelnotapproved.Visible = true;
						panelapproved.Visible = false;
						hargajualcust.Text = hargaproc.ToString("N2");
						keteranganprocurement.Text = keteranganproc;
						approveprocurementfinalby.InnerText = "______________________";
					}
					else
					{
                        divprint2.Visible = false;
                        divprint.Visible = false;
                        divapprove.Visible = false;
                        btnApprove.Visible = false;
                        btnApproveFinal.Visible = false;
                        btnEdit.Visible = false;
                        lblwarning.Visible = true;
                        approveprocurementfinalby.InnerText = "______________________";
						//lblwarning.InnerText = countdays.ToString();
                        lblwarning.InnerText = "QC above 7 days before approve procurement is expired. Please create new QC.";
                        if (rowcount > 0)
                        {
                            val.InnerText = Db.SingleDecimal("SELECT TOP 1 CAST(ISNULL(HargaProcurement,0) AS MONEY) [HargaProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB).ToString("N2");
                            divfinalprice.Visible = true;
                        }
                        else
                        {
                            divfinalprice.Visible = false;
                        }
                    }
				}
				else
				{
                    DataTable tbl = Db.Rs("SELECT TOP 1 ISNULL(ApprovalSMTgl,'') [ApprovalSMTgl], ISNULL(ApprovalAMTgl,'') [ApprovalAMTgl], ISNULL(ApprovalProcurementTgl,'') [ApprovalProcurementTgl], ISNULL(ApprovalProcurement,'') [ApprovalProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);

                    DateTime approvaltgl = DateTime.Now;
                    DateTime approvaltgl3 = Convert.ToDateTime(tbl.Rows[0]["ApprovalSMTgl"]);
                    DateTime approvaltgl4 = Convert.ToDateTime(tbl.Rows[0]["ApprovalAMTgl"]);
                    DateTime approvaltgl5 = Convert.ToDateTime(tbl.Rows[0]["ApprovalProcurementTgl"]);
                    string approvalprc = tbl.Rows[0]["ApprovalProcurement"].ToString();

                    approvaltgl = approvaltgl5;
                    tbl.Dispose();

                    int countdays = (DateTime.Now.Date - approvaltgl.Date).Days;
                    if (approvaltgl == Convert.ToDateTime("1900-01-01 00:00:00.000"))
                        countdays = 0;

					if (countdays <= 7)
					{
                        lblwarning.InnerText = "Need Approval Head Of Procurement. Please Contact Head Of Procurement.";
                    }
					else
					{
                        lblwarning.InnerText = "QC above 7 days before approve procurement is expired. Please create new QC.";
                    }

                    divprint2.Visible = false;
					divprint.Visible = false;
					divapprove.Visible = false;
					btnApprove.Visible = false;
					btnApproveFinal.Visible = false;
					btnEdit.Visible = false;
					lblwarning.Visible = true;
					approveprocurementfinalby.InnerText = "______________________";
					
				}
			}
			
		}
		else
		{
			if(countproc > 0)
			{
                DataTable tbl = Db.Rs("SELECT TOP 1 ISNULL(ApprovalSMTgl,'') [ApprovalSMTgl], ISNULL(ApprovalAMTgl,'') [ApprovalAMTgl], ISNULL(ApprovalProcurementTgl,'') [ApprovalProcurementTgl], ISNULL(ApprovalProcurement,'') [ApprovalProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);

                DateTime approvaltgl = DateTime.Now;
                DateTime approvaltgl3 = Convert.ToDateTime(tbl.Rows[0]["ApprovalSMTgl"]);
                DateTime approvaltgl4 = Convert.ToDateTime(tbl.Rows[0]["ApprovalAMTgl"]);
                DateTime approvaltgl5 = Convert.ToDateTime(tbl.Rows[0]["ApprovalProcurementTgl"]);
                string approvalprc = tbl.Rows[0]["ApprovalProcurement"].ToString();

                approvaltgl = approvaltgl5;
                tbl.Dispose();

                int countdays = (DateTime.Now.Date - approvaltgl.Date).Days;
				if (approvaltgl == Convert.ToDateTime("1900-01-01 00:00:00.000"))
					countdays = 0;

                if (countdays <= 7)
				{
					divprint2.Visible = false;
					divprint.Visible = false;
					divapprove.Visible = true;
					panelapproved.Visible = false;
					panelnotapproved.Visible = true;
					btnApproveFinal.Visible = false;
					divqcproc.Visible = true;
					lblwarning.Visible = false;
					divfinalprice.Visible = false;
					btnEdit.Visible = false;
					approveprocurementby.InnerText = "______________________";
				}
				else
				{
                    divprint2.Visible = false;
                    divprint.Visible = false;
                    divapprove.Visible = false;
                    panelapproved.Visible = false;
                    panelnotapproved.Visible = false;
                    btnApproveFinal.Visible = false;
                    divqcproc.Visible = false;
                    lblwarning.Visible = true;
                    divfinalprice.Visible = false;
                    btnEdit.Visible = false;
                    approveprocurementby.InnerText = "______________________";
                    lblwarning.InnerText = "QC above 7 days before approve procurement is expired. Please create new QC.";
                    if (rowcount > 0)
                    {
                        val.InnerText = Db.SingleDecimal("SELECT TOP 1 CAST(ISNULL(HargaProcurement,0) AS MONEY) [HargaProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB).ToString("N2");
                        divfinalprice.Visible = true;
                    }
                    else
                    {
                        divfinalprice.Visible = false;
                    }
                }
			}
			else
			{
				divprint2.Visible = false;
				divprint.Visible = false;
				divapprove.Visible = false;
				panelapproved.Visible = false;
				panelnotapproved.Visible = false;
				btnApproveFinal.Visible = false;
				divqcproc.Visible = false;
				lblwarning.Visible = true;
				divfinalprice.Visible = false;
				btnEdit.Visible = false;
				approveprocurementby.InnerText = "______________________";
				lblwarning.InnerText = "Need Approval. Please Contact Procurement.";
			}
		}
	}
	protected void btnApprove_Click(object sender, EventArgs e)
	{
		Db.Execute("EXEC sp_approval_qcld " + myID + ", '"+ hargajualcust.Text.Replace(".","").Replace(",",".") +"', '"+ keteranganprocurement.Text +"', '"+App.UserNama+"','', 'PROC' " , Db.Cnn_DB);
		//keteranganprocurement.Text = "EXEC sp_approval_qcld " + myID + ", '"+ hargajualcust.Text.Replace(".","").Replace(",",".") +"', '"+ keteranganprocurement.Text +"', '"+App.UserNama+"','', 'PROC' ";
		
		fillForm();
	}
	protected void btnApproveFinal_Click(object sender, EventArgs e)
	{
		Db.Execute("EXEC sp_approval_qcld " + myID + ", '"+ hargajualcust.Text.Replace(".","").Replace(",",".") +"', '"+ keteranganprocurement.Text +"', '"+App.UserNama+"', '"+App.UserNama+"', 'PROC' " , Db.Cnn_DB);
		
		string url = "https://frank.cmk.co.id/OutletSales/Page/Approval/QCLDFinal_Detail2.aspx?encrypt=" + EncryptDecrypt2.EncryptString(myID.ToString());
		
		Db.Execute("UPDATE DocQCLD SET UrlApprovalFinal = '" + url + "', SentMailApproveFinal = 1 WHERE ID = " + myID + " EXEC sp_sendmailapprovalqcld " + myID, Db.Cnn_DB);
		
		fillForm();
	}
	protected void btnEdit_Click(object sender, EventArgs e)
	{
		divqcproc.Visible = true;
		btnEdit.Visible = false;
		divapprove.Visible = true;
		divprint.Visible = false;
		panelapproved.Visible = false;
		panelnotapproved.Visible = true;
	}
	
	public class ExtensionLD
	{
		public string clarity { get;set; }
		public string position { get;set; }
		public string number { get;set; }
		public string size { get;set; }
	}
	
	protected void fillExtensions(List<ExtensionLD> test)
	{
		int index = 1;
		tbextension.Dispose();
		tbextension.Rows.Clear();
		
		TableHeaderRow thr = new TableHeaderRow();
		TableHeaderCell thd = new TableHeaderCell();
		thd.Text = "No.";
		thr.Cells.Add(thd);
		
		thd = new TableHeaderCell();
		thd.Text = "Clarity Characteristic";
		thr.Cells.Add(thd);
		
		thd = new TableHeaderCell();
		thd.Text = "Size";
		thr.Cells.Add(thd);
		
		thd = new TableHeaderCell();
		thd.Text = "Number";
		thr.Cells.Add(thd);
		
		thd = new TableHeaderCell();
		thd.Text = "Position";
		thr.Cells.Add(thd);
		
		tbextension.Rows.Add(thr);
		
		foreach(var x in test)
		{
			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = index.ToString();
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = x.clarity;
			tr.Cells.Add(td);
			
			td = new TableCell();
			td.Text = x.position;
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = x.number;
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = x.size;
			tr.Cells.Add(td);
			
			tbextension.Rows.Add(tr);
			index++;
		}
	}

    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = CommandType.Text;
                foreach (var x in param)
                {
                    cmd.Parameters.Add(x);
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }
}