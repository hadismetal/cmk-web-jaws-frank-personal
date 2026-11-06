using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using System.Collections.Generic;

public partial class QCLDFinal_Detail2 : System.Web.UI.Page
{
	protected int myID { get { return Convert.ToInt32(EncryptDecrypt2.DecryptString(Request.QueryString["encrypt"])); } }
	//protected string encrypt { get { return Request.QueryString["encrypt"]; } }
	
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
		//try
		//{
			//App.Protect();
			//App.ProtectRole(Role.Manager);
			
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
				hg_beli.InnerText = "IDR " + Str.Integer((Convert.ToInt32(so.TotalBayar)));
			}
			fillForm();
		//}
		//catch
		//{
			//Response.Redirect("/OutletSales/Page/Summary.aspx");
		//}
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
			valbeli.InnerText = Db.SingleInteger("SELECT TOP 1 CAST(ISNULL(HargaJualCustomer,0) AS INT) [HargaJualCustomer] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB).ToString("N0");
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
		}
		
		
		string approvalmerdir = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalFinal,'') [ApprovalFinal] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
		string ekstension = Db.SingleString("SELECT TOP 1 ISNULL(Extension,'[]') [Extension] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
		try
		{
			List<ExtensionLD> test = JsonConvert.DeserializeObject<List<ExtensionLD>>(ekstension);
			fillExtensions(test);
		}
		catch{}
		
		if(approvalmerdir != "")
		{
			btnApprove.Visible = false;
			lblwarning.InnerText = "Form Resell LD Has been Approved by Merchandise Director (" + approvalmerdir + ").";
		}
		else
		{
			btnApprove.Visible = true;
			lblwarning.InnerText = "";
		}
	}
	protected void btnApprove_Click(object sender, EventArgs e)
	{
		Db.Execute("EXEC sp_approval_qcld " + myID + ", '0', '', 'TANYA ALISSIA','','FINAL' " , Db.Cnn_DB);
		fillForm();
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
}