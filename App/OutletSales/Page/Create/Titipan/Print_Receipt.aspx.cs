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
        var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID);
        if (d == null)
            Response.Redirect("Titipan.aspx");
        else
        {            
            string Cabang = "";
            //if (d.TipeLokasi == 1)
            //    Cabang = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + d.IDLokasi,Db.Cnn_DB);
            //else
            //    Cabang = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);

            string AlamatCabang = "";
            if (d.TipeLokasi == 1)
            {
                Cabang = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
                AlamatCabang = Db.SingleString("SELECT AddrAlamat FROM LocOutlet WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
            }
            else
            {
                Cabang = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
                AlamatCabang = Db.SingleString("SELECT AddrAlamat FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
            }


            cabang.Text = Str.HtmlBreak(AlamatCabang);            
            tgl.Text = Str.Tgl(d.Tgl);
            //terbilang.Text = Str.Terbilang(d.TotalHarga.Value);
            customer.Text = d.DataCustomer.Nama;
            customer2.Text = d.DataCustomer.Nama;
            nomor.InnerText = d.Nomor;
            sales.Text = d.SalesNama;
            sales2.Text = d.SalesNama;
        }
        string Item = "";
        foreach (var r in d.DocTitipan_Product)
        {
            string DescItem = "";
            string Img = "";
            if (r.IDProduct.HasValue)
            {
                DescItem = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                DescItem += " " + r.StockProductDJ.StockProductDJ_CharProduct.GrossWeight.ToString("N2") + " gr";
                DescItem += " " + r.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value.ToString("N0") + " butir";
                Img = r.StockProductDJ.ImgPicture != null ? r.StockProductDJ.ImgPicture : r.ImgPicture;
            }
            else
            {
                DescItem = r.StockProductDJCustomer.StockProductDJCustomer_CharProduct.CharProductItem.Nama;
                DescItem += " " + r.StockProductDJCustomer.StockProductDJCustomer_CharProduct.GrossWeight.ToString("N2") + " gr";
                DescItem += " " + r.StockProductDJCustomer.StockProductDJCustomer_CharProduct.StoneQty.Value.ToString("N0") + " butir";
                Img = r.StockProductDJCustomer.ImgPicture != null ? r.StockProductDJCustomer.ImgPicture : r.ImgPicture;
            }
            
            
            Item += "<div style=\"padding-top:10px; width:1200px;\">";
            Item += "<div style=\"width:260px; float:left;\">" + (r.IDProduct.HasValue ? r.StockProductDJ.Nomor : r.StockProductDJCustomer.Nomor) + "</div>";
            Item += "<div style=\"width:720px; float:left;\">" + Str.CapUpper((r.IDProduct.HasValue ? r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama : r.StockProductDJCustomer.StockProductDJCustomer_CharProduct.CharProductItem.Nama)) + "<br /><em>" + r.Keterangan + "</em></div>";
            Item += "<div style=\"width:200px; float: left;\"><img src='/Images.aspx?path=" + Img + "' style='width: 100px;' border='0' /></div>";
            Item += "</div>";
            Item += "<div style=\"clear: both;\"></div>";
        }
        
        itemlist.Text = Item;
        keterangan.Text = d.Keterangan;

        
    }
}