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
        var d = db.SalesOrderRepair.SingleOrDefault(p => p.ID == myID);
        if (d == null)
            Response.Redirect("Repair.aspx");
        else
        {
            string Cabang = "";
            if (d.TipeLokasi == 1)
                Cabang = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + d.IDLokasi,Db.Cnn_DB);
            else
                Cabang = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);

            cabang.Text = Cabang;
            tgl.Text = Str.Tgl(DateTime.Today);
            terbilang.Text = Str.Terbilang(d.TotalBayar.Value);
            total.Text = Str.Number0(d.TotalBayar);
            nominal.Text = Str.Number0(d.Paid.Value);
            sisa.Text = Str.Number0(d.Unpaid.Value);
            customer.Text = d.DataCustomer.Nama;
            customer2.Text = d.DataCustomer.Nama;
            adminsales.Text = App.UserNama;
        }
        string Item = "";
        
            decimal Discount = 0m;
            Discount = d.DiscountNominal.Value + d.DiscountProgramNominal.Value + d.DiscountGift.Value + d.DiscountRound.Value;
            Item += "<div style=\"padding-top:10px; width:1200px;\">";
            Item += "<div style=\"width:180px; float:left;\">" + d.StockProductDJ.Nomor + "</div>";
            Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama) + " BERAT " + d.StockProductDJ.StockProductDJ_CharProduct.NettoWeight + " KADAR " + Math.Floor(d.StockProductDJ.StockProductDJ_CharProduct.KadarLogam).ToString("N0") + "% WARNA " + Str.CapUpper(d.StockProductDJ.StockProductDJ_CharProduct.CharFrameColor.Nama) + "<br /><em>" + d.DocRepairResult.Keterangan + "</em></div>";
            Item += "<div style=\"width:60px; float:left;\">1</div>";
            Item += "<div style=\"width:180px; float:left;\">" + d.HargaRupiah.Value.ToString("N0") + "</div>";
            Item += "<div style=\"width:200px; float:left;\">" + Discount.ToString("N0") + "</div>";
            Item += "<div style=\"width:200px; float: left; text-align: right;\">" + d.TotalBayar.Value.ToString("N0") + "</div>";
            Item += "</div>";
            Item += "<div style=\"clear: both;\"></div>";
        
        itemlist.Text = Item;
    }
}