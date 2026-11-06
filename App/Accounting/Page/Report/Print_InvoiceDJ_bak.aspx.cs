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
            tgl.Text = Str.Tgl(d.SalesOrder.OperatorTgl);
            terbilang.Text = Str.Terbilang(d.TotalBayar.Value);
            customer.Text = d.SalesOrder.DataCustomer.Nama;
            customer2.Text = d.SalesOrder.DataCustomer.Nama;
            adminsales.Text = d.SalesOrder.SalesNama;
            brand.Text = d.SalesOrder.DataSales.CompanyBrand.Nama;
            brand2.Text = d.SalesOrder.DataSales.CompanyBrand.Nama;
            brand3.Text = d.SalesOrder.DataSales.CompanyBrand.Nama;
            brand4.Text = d.SalesOrder.DataSales.CompanyBrand.Nama;
        }
        string Item = "";
        
            decimal Discount = 0m;
            Discount = d.DiscountNominal.Value + d.DiscountProgramNominal.Value + d.DiscountGift.Value + d.DiscountRound.Value;
            Item += "<div style=\"padding-top:10px; width:1200px;\">";
            Item += "<div style=\"width:180px; float:left;\">" + d.StockProductDJ.Nomor + "</div>";
            Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama) + "</div>";
            Item += "<div style=\"width:60px; float:left;\">1</div>";
            Item += "<div style=\"width:180px; float:left;\">" + d.TotalBayar.Value.ToString("N0") + "</div>";
            //Item += "<div style=\"width:200px; float:left;\">" + d.DiscountGift.Value.ToString("N0") + "</div>";
            Item += "<div style=\"width:200px; float:left;\">&nbsp;</div>";
            Item += "<div style=\"width:200px; float: left; text-align: right;\">" + d.TotalBayar.Value.ToString("N0") + "</div>";
            Item += "</div>";
            Item += "<div style=\"clear: both;\"></div>";

            int itemDJ = 0;
            int itemPG = 0;
            itemDJ = d.SalesOrder.SalesOrderDJ.Count();
            itemPG = d.SalesOrder.SalesOrderPG.Count();
            
            var s = from p in db.SalesOrderPackaging
                    where p.IDForm == d.IDForm && p.IDProductDJ == d.IDProduct && p.IDProductDJ.HasValue
                    select p;

            var t = from p in db.SalesOrderSouvenir
                    where p.IDForm == d.IDForm && p.IDProductDJ == d.IDProduct && p.IDProductDJ.HasValue
                    select p;
            foreach (var r in s)
            {
                Discount = 0m;
                Item += "<div style=\"padding-top:10px; width:1200px;\">";
                Item += "<div style=\"width:180px; float:left;\">" + r.Packaging.Kode + "</div>";
                Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.Packaging.Nama) + "</div>";
                Item += "<div style=\"width:60px; float:left;\">" + Str.Number0(r.Qty) + "</div>";
                Item += "<div style=\"width:180px; float:left;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                //Item += "<div style=\"width:200px; float:left;\">" + Discount.ToString("N0") + "</div>";
                Item += "<div style=\"width:200px; float:left;\">&nbsp;</div>";
                Item += "<div style=\"width:200px; float: left; text-align: right;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                Item += "</div>";
                Item += "<div style=\"clear: both;\"></div>";
            }

            foreach (var r in t)
            {
                Discount = 0m;
                Item += "<div style=\"padding-top:10px; width:1200px;\">";
                Item += "<div style=\"width:180px; float:left;\">" + r.Souvenir.Kode + "</div>";
                Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.Souvenir.Nama) + "</div>";
                Item += "<div style=\"width:60px; float:left;\">" + Str.Number0(r.Qty) + "</div>";
                Item += "<div style=\"width:180px; float:left;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                //Item += "<div style=\"width:200px; float:left;\">" + Discount.ToString("N0") + "</div>";
                Item += "<div style=\"width:200px; float:left;\">&nbsp;</div>";
                Item += "<div style=\"width:200px; float: left; text-align: right;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                Item += "</div>";
                Item += "<div style=\"clear: both;\"></div>";
            }
        
        itemlist.Text = Item;

        string[] ASP = new string[20];

        bool Solitaire = false;

        if (d.StockProductDJ.StockProductDJ_CharProduct.CharStoneDist.NamaKode == "SOL")
        {
            if (d.StockProductDJ.StockProductDJ_Stone1A.Max().CaratButir < 0.9m || d.StockProductDJ.StockProductDJ_Stone1B.Max().CaratButir < 0.9m
                || d.StockProductDJ.StockProductDJ_Stone2.Max().CaratButir < 0.9m || d.StockProductDJ.StockProductDJ_Stone3.Max().CaratButir < 0.9m
                || d.StockProductDJ.StockProductDJ_Stone4.Max().CaratButir < 0.9m || d.StockProductDJ.StockProductDJ_Stone5.Max().CaratButir < 0.9m)
                Solitaire = true;
        }

        //ASP = DLC.getASPDJParameter(d.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.ID, Solitaire);
        ASP = DLC.getASPDJParameter(d.StockProductDJ, Solitaire);
        if (ASP[0] == "")
        {
            asp.Visible = false;
            asp2.Visible = true;
        }
        else
        {
            me1.Text = ASP[1];
            me2.Text = ASP[1];
            me_rumus1.Text = ASP[2];
            ti1.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[4]) / 12)).ToString();
            ti_rumus1.Text = ASP[5];
            ti2.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[7]) / 12)).ToString();
            ti_rumus2.Text = ASP[8];
            ti3.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[9]) / 12)).ToString();
            r1.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[13]) / 12)).ToString();
            r1_rumus.Text = ASP[14];
            r2.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[15]) / 12)).ToString();

            /*me1.Text = d.MEPeriodeMax.ToString();
            me2.Text = d.MEPeriodeMax.ToString();
            ti1.Text = Math.Floor(Convert.ToDecimal(d.TI1_PeriodeMax.Value / 12)).ToString();
            ti_rumus1.Text = d.TI1_Rumus.Value.ToString("N2");
            ti2.Text = Math.Floor(Convert.ToDecimal(d.TI2_PeriodeMax.Value / 12)).ToString();
            ti_rumus2.Text = d.TI2_Rumus.Value.ToString("N2");
            ti3.Text = Math.Floor(Convert.ToDecimal(d.TI3_PeriodeMin.Value / 12)).ToString();
            r1.Text = Math.Floor(Convert.ToDecimal(d.R1_PeriodeMax / 12)).ToString();
            r1_rumus.Text = d.R1_Rumus.Value.ToString("N2");
            r2.Text = Math.Floor(Convert.ToDecimal(d.R2_PeriodeMin.Value / 12)).ToString();*/
        }
    }
}