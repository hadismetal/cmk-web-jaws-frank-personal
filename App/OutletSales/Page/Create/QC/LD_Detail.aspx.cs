using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class Page_Create_QC_LD_Detail : System.Web.UI.Page
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
			val.InnerText = Db.SingleDecimal("SELECT TOP 1 CAST(ISNULL(HargaJualCustomer,0) AS MONEY) [HargaJualCustomer] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB).ToString("N0");
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
			checkedby.InnerText = q.QCNama.ToUpper();
			
			string ekstension = Db.SingleString("SELECT TOP 1 ISNULL(Extension,'[]') [Extension] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			string approvalsmam = Db.SingleString("SELECT TOP 1 ISNULL(ISNULL(ApprovalSM,ApprovalAM),'') [Approval] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			string approvalproc1 = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalProcurement,'') [ApprovalProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			string approvalproc2 = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalProcurementFinal,'') [ApprovalProcurementFinal] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			string approvalgm = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalFinal,'') [ApprovalProcurementFinal] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
			if(approvalsmam == "")
			{
				approveby.InnerText = "";
			}
			else
			{
				approveby.InnerText = approvalsmam;
			}
			if(approvalproc1 == "")
			{
				approveprocby.InnerText = "";
			}
			else
			{
				approveprocby.InnerText = approvalproc1;
			}
			if(approvalproc2 == "")
			{
				approveprocfinalby.InnerText = "";
			}
			else
			{
				approveprocfinalby.InnerText = approvalproc2;
			}
			if(approvalgm == "")
			{
				approvefinalby.InnerText = "";
			}
			else
			{
				approvefinalby.InnerText = approvalgm;
			}
			try
			{
				List<ExtensionLD> test = JsonConvert.DeserializeObject<List<ExtensionLD>>(ekstension);
				fillExtensions(test);
			}
			catch{}
		}
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