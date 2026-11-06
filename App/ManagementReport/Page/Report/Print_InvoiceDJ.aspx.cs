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
        var d = db.SalesOrderDJ.SingleOrDefault(p => p.ID == myID);
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
            Item += "<div style=\"width:180px; float:left;\">" + d.StockProductDJ.Nomor + "</div>";
            Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama) + " BERAT " + Convert.ToDecimal(d.StockProductDJ.StockProductDJ_CharProduct.NettoWeight).ToString("N2") + " KADAR " + Math.Floor(d.StockProductDJ.StockProductDJ_CharProduct.KadarLogam).ToString("N0") + "% WARNA " + Str.CapUpper(d.StockProductDJ.StockProductDJ_CharProduct.CharFrameColor.Nama) + "</div>";
            Item += "<div style=\"width:60px; float:left;\">1</div>";
            Item += "<div style=\"width:180px; float:left;\">" + d.HargaRupiah.Value.ToString("N0") + "</div>";
            Item += "<div style=\"width:200px; float:left;\">" + Discount.ToString("N0") + "</div>";
            Item += "<div style=\"width:200px; float: left; text-align: right;\">" + d.TotalBayar.Value.ToString("N0") + "</div>";
            Item += "</div>";
            Item += "<div style=\"clear: both;\"></div>";
        
        itemlist.Text = Item;

        bool Solitaire = false;

        if(d.StockProductDJ.StockProductDJ_Stone1B.Count() == 1)
            Solitaire = true;

        string[] ASP = new string[20];
        ASP = DLC.getASPDJParameter(d.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.ID, Solitaire);
        if (ASP[0] == "")
            asp.Visible = false;
        else
        {
            me1.Text = ASP[1];
            me2.Text = ASP[1];
            me_rumus1.Text = ASP[2];
            ti1.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[4])/12)).ToString();
            ti_rumus1.Text = ASP[5];
            ti2.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[7]) / 12)).ToString();
            ti_rumus2.Text = ASP[8];
            ti3.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[9]) / 12)).ToString();
            r1.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[13]) / 12)).ToString();
            r1_rumus.Text = ASP[14];
            r2.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[15]) / 12)).ToString();
        }
    }
}