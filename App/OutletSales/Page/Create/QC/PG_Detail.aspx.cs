using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_QC_PG_Detail : System.Web.UI.Page
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
		
		var data = db.DocQCPG.SingleOrDefault(p => p.ID == myID).StockProductPG.StockActualPG;
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
		lit_nomor_repair.Text = db.DocQCPG.SingleOrDefault(p => p.ID == myID).Nomor;

        fillForm();
    }

    protected void fillForm()
    {
        var q = db.DocQCPG.SingleOrDefault(p => p.ID == myID);
        if (q.StockProductPG == null) App.DataNull(this);
        else
        {
			try
			{
				//Header
				nodocqc.InnerText = q.Nomor;
				tgldocqc.InnerText = Convert.ToDateTime(q.OperatorTgl).ToString("dd-MM-yyyy");
				plu.InnerText = q.StockProductPG.Nomor;
				netto.InnerText = Convert.ToDecimal(q.StockProductPG.NettoWeight).ToString("N2") + " gram";
				frame.InnerText = q.StockProductPG.CharFrameColor.Nama + " (" + q.StockProductPG.CharFrameColor.NamaKode + ")";
				kadar.InnerText = Convert.ToDecimal(q.StockProductPG.KadarLogam).ToString("N0") + " %";
				petugasqc.InnerText = q.QCNama.ToUpper();
				logo.InnerText = q.Logo;
				kadaremas.InnerText = q.KadarLogam;
				fisik.InnerText = q.FisikKeseluruhan;
				netweight.InnerText = Convert.ToDecimal(q.NetWeight).ToString("N2") + " gram";
				kadarkaratimeter.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(KadarKaratimeter,'') [KadarKaratimeter] FROM DocQCPG WHERE ID = " + myID, Db.Cnn_DB);
				framecoloractual.InnerText = q.WarnaEmas;
				keterangan.InnerText = q.Keterangan;
				checkedby.InnerText = q.QCNama.ToUpper();
			}
			catch
			{
				//Header
				nodocqc.InnerText = q.Nomor;
				tgldocqc.InnerText = Convert.ToDateTime(q.OperatorTgl).ToString("dd-MM-yyyy");
				plu.InnerText = q.StockProductPG.Nomor;
				netto.InnerText = Convert.ToDecimal(q.StockProductPG.NettoWeight).ToString("N2") + " gram";
				frame.InnerText = q.StockProductPG.CharFrameColor.Nama + " (" + q.StockProductPG.CharFrameColor.NamaKode + ")";
				kadar.InnerText = Convert.ToDecimal(q.StockProductPG.KadarLogam).ToString("N0") + " %";
				petugasqc.InnerText = q.QCNama.ToUpper();
				logo.InnerText = q.Logo;
				kadaremas.InnerText = q.KadarLogam;
				fisik.InnerText = q.FisikKeseluruhan;
				netweight.InnerText = q.NetWeight;
				kadarkaratimeter.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(KadarKaratimeter,'') [KadarKaratimeter] FROM DocQCPG WHERE ID = " + myID, Db.Cnn_DB);
				framecoloractual.InnerText = q.WarnaEmas;
				keterangan.InnerText = q.Keterangan;
				checkedby.InnerText = q.QCNama.ToUpper();
			}
        }
    }
}