using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Catalogue_LD_Detail : System.Web.UI.Page
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
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("LD.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == myID
                //&& p.ReturnProduct == false
                );
        if (d == null) DataError();
        else
        {
            header_nomor.InnerText = d.Nomor;
            designnomor.InnerText = d.Nomor;
            productitem.InnerText = d.CharProductItem.Nama + " (" + d.CharProductItem.NamaKode + ")";
            totalbutir.InnerText = d.TotalButir.ToString();
            totalcarat.InnerText = d.TotalCarat.ToString();
            caratbutir.InnerText = d.CaratButir.ToString();
            gia.InnerText = d.GIA;

            shape.InnerText = d.Stone1B.Parcel101.Nama;
            size.InnerText = d.Stone1B.Parcel102.Nama;
            colour.InnerText = d.Stone1B.Parcel103.Nama;
            clarity.InnerText = d.Stone1B.Parcel104.Nama;
            cutting.InnerText = d.Stone1B.Parcel105.Nama;
            brand.InnerText = d.Stone1B.Parcel106.Nama;
            category.InnerText = d.Stone1B.Parcel107.Nama;

            var us = from p in db.PricingTableUS
                     where p.ID == CV.US_LD_Rate
                     select p;

            decimal Harga = Math.Floor((d.Stone1B.StonePricing1B.Harga.Value * d.TotalCarat) * us.First().Harga.Value / 1000) * 1000;
            harga.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(Harga).ToString("N0") + "</p>";
        }
    }
}