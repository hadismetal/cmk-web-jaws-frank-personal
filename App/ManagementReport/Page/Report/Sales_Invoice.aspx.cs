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
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null)
            Response.Redirect("Sales.aspx");
        else
        {
            string Cabang = "";
            if (d.TipeLokasi == 1)
                Cabang = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + d.IDLokasi,Db.Cnn_DB);
            else
                Cabang = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);

            cabang.Text = Cabang;
            tgl.Text = Str.Tgl(d.Tgl);
            terbilang.Text = Str.Terbilang(d.TotalBayar.Value);
            total.Text = Str.Number0(d.TotalRupiah);
            discount.Text = Str.Number0(d.DiscountNominal.Value);
            discountprogram.Text = Str.Number0(d.DiscountProgramNominal.Value);
            discountgift.Text = Str.Number0(d.DiscountGift.Value);
            discountround.Text = Str.Number0(d.DiscountRound.Value);
            nominal.Text = Str.Number0(d.TotalBayar.Value);
            customer.Text = d.DataCustomer.Nama;
            customer2.Text = d.DataCustomer.Nama;
            adminsales.Text = d.DataSales.Nama;
        }
    }
}