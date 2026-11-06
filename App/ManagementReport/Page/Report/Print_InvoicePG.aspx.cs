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
        var d = db.SalesOrderPG.SingleOrDefault(p => p.ID == myID);
        if (d == null)
            Response.Redirect("Sales.aspx");
        else
        {
            string Cabang = "";
            if (d.SalesOrder.TipeLokasi == 1)
                Cabang = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + d.SalesOrder.IDLokasi,Db.Cnn_DB);
            else
                Cabang = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + d.SalesOrder.IDLokasi, Db.Cnn_DB);

            cabang.Text = Cabang;
            tgl.Text = Str.Tgl(d.SalesOrder.Tgl);
            terbilang.Text = Str.Terbilang(d.TotalBayar.Value);
            customer.Text = d.SalesOrder.DataCustomer.Nama;
            customer2.Text = d.SalesOrder.DataCustomer.Nama;
            adminsales.Text = d.SalesOrder.DataSales.Nama;
        }
        string Item = "";
        
            decimal Discount = 0m;
            Discount = d.DiscountNominal.Value + d.DiscountProgramNominal.Value + d.DiscountGift.Value + d.DiscountRound.Value;
            Item += "<div style=\"padding-top:10px; width:1200px;\">";
            Item += "<div style=\"width:180px; float:left;\">" + d.StockProductPG.Nomor + "</div>";
            Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(d.StockProductPG.CharProductItem.Nama) + " BERAT " + Convert.ToDecimal(d.StockProductPG.GrossWeight).ToString("N2") + " KADAR " + Math.Floor(d.StockProductPG.KadarLogam.Value).ToString("N0") + "% WARNA " + Str.CapUpper(d.StockProductPG.CharFrameColor.Nama) + "</div>";
            Item += "<div style=\"width:60px; float:left;\">1</div>";
            Item += "<div style=\"width:180px; float:left;\">" + d.HargaRupiah.Value.ToString("N0") + "</div>";
            Item += "<div style=\"width:200px; float:left;\">" + Discount.ToString("N0") + "</div>";
            Item += "<div style=\"width:200px; float: left; text-align: right;\">" + d.TotalBayar.Value.ToString("N0") + "</div>";
            Item += "</div>";
            Item += "<div style=\"clear: both;\"></div>";
        
        itemlist.Text = Item;

        string[] ASP = new string[10];
        ASP = DLC.getASPPGParameter();

        me1.Text = ASP[1];
        me2.Text = ASP[1];
        me_rumus1.Text = ASP[2];
        /*t1.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[1]) / 12)).ToString();
        t2.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[1]) / 12)).ToString();*/
		t1.Text = ASP[1];
        t2.Text = ASP[1];

    }
}