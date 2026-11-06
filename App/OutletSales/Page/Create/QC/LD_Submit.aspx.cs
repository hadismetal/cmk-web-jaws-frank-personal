using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;

public partial class Page_Approval_LD_PricingSubmit : System.Web.UI.Page
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
    decimal GT_HargaM = 0;

    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
			/*Ddl.CompanyBrand(brand);
			Ddl.Parcel104(cla);
			Ddl.Parcel101(shape);
			Ddl.Parcel103(col);
			Ddl.Parcel106(cut);*/
			
			/*Ddl.DropdownColor(col);
			Ddl.DropdownShape(shape);
			Ddl.DropdownClarity(cla);
			Ddl.DropdownCutting(cut);
			Ddl.DropdownBrand(brand);*/
			
			DataTable dt1 = Db.Rs("SELECT * FROM ClarityCharacteristicLD",Db.Cnn_DB);
			DataTable dt2 = Db.Rs("SELECT * FROM PositionLD",Db.Cnn_DB);
			DataTable dt3 = Db.Rs("SELECT * FROM SizeLD",Db.Cnn_DB);
			listclarity.Value = JsonConvert.SerializeObject(dt1);
			listposition.Value = JsonConvert.SerializeObject(dt2);
			listsize.Value = JsonConvert.SerializeObject(dt3);
			
			fillForm();
        }
    }

    //Form
    protected void fillForm()
    {
        var d = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == myID);
		var so = db.SalesOrderLD.SingleOrDefault(p => p.IDProduct == myID && p.SalesOrder.StatusPembayaran == true && p.SalesOrder.StatusVoid == false);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerText = d.Nomor;
			header_butir.InnerText = d.TotalButir.ToString("N0");
            header_carat.InnerText = d.TotalCarat.ToString("N3");
			header_gia.InnerText = d.GIA;
			
			if(so != null)
			{
				header_invoice.InnerText = so.Invoice;
				header_cust.InnerText = so.SalesOrder.CustomerNama;
				if(so.SalesOrder.TipeLokasi == 1)
					header_storeasal.InnerText = Db.SingleString("SELECT TOP 1 UPPER(Nama) [Nama] FROM LocOutlet WHERE ID = " + so.SalesOrder.IDLokasi, Db.Cnn_DB);
				else if(so.SalesOrder.TipeLokasi == 2)
					header_storeasal.InnerText = Db.SingleString("SELECT TOP 1 UPPER(Nama) [Nama] FROM LocExhibition WHERE ID =" + so.SalesOrder.IDLokasi, Db.Cnn_DB);
				header_hg_beli.InnerText = "IDR " + Str.Integer((Convert.ToInt32(so.TotalBayar)));
			}
			
			string pqc = "";
			using (Map_AC w = new Map_AC())
			{
				var user = w.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
				if (user != null)
					pqc = user.Nama + " (" + user.UserNumber +")"; 
			}
			
			petugasqc.Text = pqc;

			//Detail
			shape.Text = d.Stone1B.Parcel101.Nama.ToString();
			cla.Text = d.Stone1B.Parcel104.Nama.ToString();
			col.Text = d.Stone1B.Parcel103.Nama.ToString();
			cut.Text = d.Stone1B.Parcel105.Nama.ToString();
			brand.Text = d.Stone1B.Parcel106.Nama.ToString();
			item.Text = "LOOSE DIAMOND";
			val.Text = "0";
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
		string msg = validateFillLD();
		if(string.IsNullOrEmpty(msg)){
			var d = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == myID);
			var so = db.SalesOrderLD.SingleOrDefault(p => p.IDProduct == myID && p.SalesOrder.StatusPembayaran == true && p.SalesOrder.StatusVoid == false);
			if (d == null) App.DataNull(this);
			else
			{

				DocQCLD x = new DocQCLD();

				x.QCNama = petugasqc.Text;
				x.Keterangan = keterangan.Text;
				x.Nomor = Document.Generate_Nomor_QCLD(x);
				x.IDProduct = d.ID;
				x.Blemish = "";//blemish.Text;
				x.JumlahButir = "1";//;jlhbutir.Text;
				x.Sertifikat = sertifikat.Text;
				x.NonCZ = "";//noncz.Text;
				x.CZ = "";//cz.Text;
				x.Logo = "";//logo.Text;
				x.KadarEmas = "";//kadaremas.Text;
				x.Merk = "";//merk.Text;
				x.FisikKeseluruhan = "";//fisik.Text;
				x.NetWeight = "";
				x.WarnaEmas = "";//framecolor.Text;
				x.OperatorNama = App.UserNama;
				x.OperatorTgl = DateTime.Now;
				x.Draft = false;
				x.DraftDate = null;

				db.DocQCLD.InsertOnSubmit(x);
				db.SubmitChanges();
				
				int hargajual = 0;
				if(so != null)
				{
					hargajual = Convert.ToInt32(so.TotalBayar ?? 0m);
				}
				
				Db.Execute("UPDATE DocQCLD SET Item = '" + item.Text + "', HargaJualCustomer = '" + hargajual/*String.Format("{0:0.00}", val.Text.Trim().Replace(",","."))*/ + "', Clarity = '" + cla.Text + "', Shape = '" + shape.Text + "', Colour = '" + col.Text + "', Cutting = '" + cut.Text + "', Carat = " + String.Format("{0:0.000}", d.TotalCarat.ToString("N3").Trim().Replace(",",".")) + ", Brand = '" + brand.Text + "', Extension = '" + finalextension.Value + "' WHERE ID = " + x.ID,Db.Cnn_DB);
				UpdateQCLDBySM(x.ID,isanycertificate.SelectedItem.ToString(),isanyinvoice.SelectedItem.ToString(),laserincription.Text,measurement.Text,kondisibatu.SelectedItem.ToString());
				
				//Redirect
				//App.ReloadClose(this);
				Response.Redirect("LD_Detail.aspx?id=" + x.ID);
			}
			lblwarning.Visible = false;
            lblwarning.InnerText = "";
		}
		else
		{
			lblwarning.Visible = true;
            lblwarning.InnerText = msg;
		}
    }
	
	protected void btnSearchNIK_Click(object sender, EventArgs e)
    {
		lblwarning.Visible = false;
        string nik = petugasqcsearch.Text;
        DataTable dt = new DataTable();
        dt = Db.Rs("SELECT TOP 5 m_nik, m_nama FROM dbhrd.dbo.mskaryawan WHERE m_nik = '"+nik+"' ", Db.Cnn_CS);

        int rowcount = dt.Rows.Count;
        if(rowcount == 1)
        {
            petugasqc.Text = dt.Rows[0]["m_nama"].ToString().ToUpper() + " (" + dt.Rows[0]["m_nik"].ToString() + ")";
            lblerrorpetugasqc.InnerText = "";
            lblerrorpetugasqc.Visible = false;
        }
        else if(rowcount == 0)
        {
            petugasqc.Text = "";
            lblerrorpetugasqc.InnerText = "NIK tidak ditemukan.";
            lblerrorpetugasqc.Visible = true;
        }
        else
        {
            petugasqc.Text = "";
            lblerrorpetugasqc.InnerText = "NIK duplikasi.";
            lblerrorpetugasqc.Visible = true;
        }
		dt.Dispose();
		
    }
	
	protected string validateFillLD()
    {
        string message = "";
        if (string.IsNullOrEmpty(petugasqc.Text))
        {
            message = "Petugas QC wajib diinput.";
        }
		
		/*string caratactual = String.Format("{0:0.000}", weight.Text.Trim());
		decimal carat1 = 0m;
		bool isnumber = decimal.TryParse(caratactual, out carat1);
		if (!isnumber)
		{
			message = "Berat harus berupa angka / nominal.";
		}*/
		
		string hargajualz = String.Format("{0:0.00}", val.Text.Trim());
		decimal hargajual1 = 0m;
		bool isnumber1 = decimal.TryParse(hargajualz, out hargajual1);
		if (!isnumber1)
		{
			message = "Value harus berupa angka / nominal.";
		}
		/*if(finalextension.Value != "")
		{
			message = finalextension.Value;
		}*/
        return message; 
    }
	
	protected void UpdateQCLDBySM(int id, string sertifikatbysm, string invoicebysm, string laserincriptionbysm, string measurementbysm, string kondisibatubysm)
	{
		string query = "UPDATE DocQCLD SET CheckSertifikatBySM = '" + sertifikatbysm + "', CheckInvoiceBySM = '" + invoicebysm + "', CheckLaserIncriptionBySM = '" + laserincriptionbysm + "', CheckMeasurementBySM = '" + measurementbysm + "', CheckKondisiBatu = '" + kondisibatubysm + "' WHERE ID = " + id;
		Db.Execute(query,Db.Cnn_DB);
	}
}