using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_DJ_SPK_DataPOSave : System.Web.UI.Page
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
    protected int idpo { get { return App.QueryInt(this, "idpo"); } }
    protected int mytipe { get { return App.QueryInt(this, "tipe"); } }
    protected void LinkError() { Response.Redirect("SPK_DataPO.aspx?id=" + myID); }
    protected void DataError() { Response.Redirect("SPK.aspx"); }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            saveLink(d);

            //Redirect
            Response.Redirect("SPK_Data01.aspx?id=" + myID + "&status=1&tipe=" + mytipe + "");
        }
    }

    protected void saveLink(ProductOrder data)
    {
        int IDBrand = 1;
        IDBrand = data.IDBrand.Value;

        var d = (from p in db.SalesReceiptDPPO where p.ID == idpo select p).SingleOrDefault();

        //if (d == null)
        //{
        //    var cust = cs.Mscustomer.SingleOrDefault(p => p.M_kode.Equals(myIDCust));

        //    DataCustomer n = new DataCustomer();
        //    n.Nama = cust.M_nama;
        //    n.NoCustomer = cust.M_kode;
        //    n.IDBrand = IDBrand;
        //    n.Keterangan = "";
        //    n.Tgl = System.DateTime.Now;
        //    db.DataCustomer.InsertOnSubmit(n);
        //    db.SubmitChanges();

        //    data.RequestCustomer = n.ID;
        //    data.RequestCustomerNama = n.Nama;
        //    db.SubmitChanges();
        //}
        //else
        if (d != null)
        {
            data.NoPO = d.NomorDP;
            data.RequestCustomer = Convert.ToInt32(d.IDCustomer);
            data.RequestCustomerNama = d.CustomerNama;
            if (d.TipeLokasi == 1)
            {
                // data.RequestOutlet = d.IDLokasi;
                var lokasi = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                if (lokasi != null)
                {
                    data.RequestOutlet = lokasi.ID;
                    data.RequestOutletNama = lokasi.Nama;
                    data.RequestExhibition = 0;
                    data.RequestExhibitionNama = "";
                    db.SubmitChanges();
                }
            }
            else if (d.TipeLokasi == 2)
            {
                var lokasi = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                if (lokasi != null)
                {
                    data.RequestOutlet = 0;
                    data.RequestOutletNama = "";
                    data.RequestExhibition = lokasi.ID;
                    data.RequestExhibitionNama = lokasi.Nama;
                    db.SubmitChanges();
                }
            }            
        }
    }


}