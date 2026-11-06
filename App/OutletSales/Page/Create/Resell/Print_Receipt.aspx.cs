using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Fill();
            
        }
    }
    protected void Fill()
    {
        var d = db.Resell.SingleOrDefault(p => p.ID == myID);
        if (d == null)
            Response.Redirect("Sales.aspx");
        else
        {
            string Cabang = "";
            if (d.TipeLokasi == 1)
                Cabang = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + d.IDLokasi,Db.Cnn_DB);
            else
                Cabang = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);

            var u = db.TradeIn.SingleOrDefault(p => p.IDResell == d.ID);

            cabang.Text = Cabang;
            tgl.Text = Str.Tgl(d.Tgl);
            terbilang.Text = Str.Terbilang(d.TotalHarga.Value);
            total.Text = Str.Number0(d.TotalHarga.Value);
            nominal.Text = Str.Number0(d.TotalHarga.Value);
            customer.Text = d.DataCustomer.Nama;
            customer2.Text = "_________________________";
            if (u != null)
                adminsales.Text = u.SalesNama;
            else if (d.IDSales.HasValue)
                adminsales.Text = d.SalesNama;
            else
                adminsales.Text = App.UserNama;
        }
        string Item = "";
        foreach (var r in d.ResellDJ)
        {
            Item += "<div style=\"padding-top:10px; width:1200px;\">";
            Item += "<div style=\"width:250px; float:left;\">" + r.Resell.Nomor + " / " + r.StockProductDJ.Nomor + "</div>";
            //Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama) + " BERAT " + r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight + " KADAR " + Math.Floor(r.StockProductDJ.StockProductDJ_CharProduct.KadarLogam).ToString("N0") + "% WARNA " + Str.CapUpper(r.StockProductDJ.StockProductDJ_CharProduct.CharFrameColor.Nama) + "</div>";
            Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama) + "</div>";
            Item += "<div style=\"width:60px; float:left;\">1</div>";
            Item += "<div style=\"width:180px; float:left;\">" + r.HargaRupiah.Value.ToString("N0") + "</div>";
            Item += "<div style=\"width:200px; float: left; text-align: right;\">" + r.HargaRupiah.Value.ToString("N0") + "</div>";
            Item += "</div>";
            Item += "<div style=\"clear: both;\"></div>";
        }
        foreach (var r in d.ResellPG)
        {
			decimal beratisian = Db.SingleDecimal("SELECT TOP 1 ISNULL(BeratTimbang,0) FROM ResellPG WHERE ID = " + r.ID.ToString() + " ", Db.Cnn_DB);
            Item += "<div style=\"padding-top:10px; width:1200px;\">";
            Item += "<div style=\"width:250px; float:left;\">" + r.Resell.Nomor + " / " + r.StockProductPG.Nomor + "</div>";
            Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductPG.CharProductItem.Nama) + " BERAT " + ((beratisian != 0m) ? beratisian : r.StockProductPG.GrossWeight) + " KADAR " + Math.Floor(r.StockProductPG.KadarLogam.Value).ToString("N0") + "% WARNA " + Str.CapUpper(r.StockProductPG.CharFrameColor.Nama) + "</div>";
            Item += "<div style=\"width:60px; float:left;\">1</div>";
            Item += "<div style=\"width:180px; float:left;\">" + r.HargaRupiah.Value.ToString("N0") + "</div>";
            Item += "<div style=\"width:200px; float: left; text-align: right;\">" + r.HargaRupiah.Value.ToString("N0") + "</div>";
            Item += "</div>";
            Item += "<div style=\"clear: both;\"></div>";
        }
        foreach (var r in d.ResellGJ)
        {
            Item += "<div style=\"padding-top:10px; width:1200px;\">";
            Item += "<div style=\"width:250px; float:left;\">" + r.Resell.Nomor + " / " +r.StockProductGJ.Nomor + "</div>";
            Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama) + " BERAT " + r.StockProductGJ.StockProductGJ_CharProduct.NettoWeight + " KADAR " + Math.Floor(r.StockProductGJ.StockProductGJ_CharProduct.KadarLogam).ToString("N0") + "% WARNA " + Str.CapUpper(r.StockProductGJ.StockProductGJ_CharProduct.CharFrameColor.Nama) + "</div>";
            Item += "<div style=\"width:60px; float:left;\">1</div>";
            Item += "<div style=\"width:180px; float:left;\">" + r.HargaRupiah.Value.ToString("N0") + "</div>";
            Item += "<div style=\"width:200px; float: left; text-align: right;\">" + r.HargaRupiah.Value.ToString("N0") + "</div>";
            Item += "</div>";
            Item += "<div style=\"clear: both;\"></div>";
        }
        foreach (var r in d.ResellLD)
        {
            Item += "<div style=\"padding-top:10px; width:1200px;\">";
            Item += "<div style=\"width:250px; float:left;\">" + r.Resell.Nomor + " / " + r.StockProductLD_Stone1B.Nomor + "</div>";
            Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductLD_Stone1B.TotalCarat.ToString("N2")) + " Carat</div>";
            Item += "<div style=\"width:60px; float:left;\">1</div>";
            Item += "<div style=\"width:180px; float:left;\">" + r.HargaRupiah.Value.ToString("N0") + "</div>";
            Item += "<div style=\"width:200px; float: left; text-align: right;\">" + r.HargaRupiah.Value.ToString("N0") + "</div>";
            Item += "</div>";
            Item += "<div style=\"clear: both;\"></div>";
        }
        itemlist.Text = Item;
    }
}