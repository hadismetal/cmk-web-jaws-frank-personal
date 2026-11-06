using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page
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
    protected int myReceiptID { get { return App.QueryInt(this, "sid"); } }

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
        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
        if (d == null)
            Response.Redirect("PaymentDP.aspx");
        else
        {

            string Cabang = "";
            if (d.TipeLokasi == 1)
                Cabang = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
            else
                Cabang = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);

            cabang.Text = Cabang;
            tgl.Text = Str.Tgl(d.Tgl);
            terbilang.Text = Str.Terbilang((d.TotalBayar.Value));
            total.Text = Str.Number0(d.TotalBayar.Value);
            nominal.Text = Str.Number0(d.TotalBayar.Value);
            customer.Text = d.DataCustomer.Nama;
            customer2.Text = d.DataCustomer.Nama;
            kode_customer.Text = d.DataCustomer.NoCustomer;
            adminsales.Text = d.SalesNama;

            detailPaymentControl.SetDetailPayment(d.ID);
			
			int freegwpcount = Db.SingleInteger("SELECT COUNT('') FROM LetterWR WHERE IDSalesReceiptDPPO = " + myID.ToString(), Db.Cnn_DB);
            if (freegwpcount == 1)
            {
                freegwptext.Visible = true;
                freegwptext.InnerHtml = freegwptext.InnerHtml + " : " + Db.SingleString("SELECT TOP 1 Letter FROM LetterWR WHERE IDSalesReceiptDPPO = " + myID.ToString(), Db.Cnn_DB);
            }
            else
            {
                freegwptext.Visible = false;
                freegwptext.InnerHtml = "";
            }

            string Item = "";
            int index = 1;
            decimal DP = 0m;

            string ket = string.Empty;

            if((d.Keterangan != null && d.Keterangan != "" ))
            {
                ket = "<div style=\"width:500px; float:left;\">" + d.Keterangan + "</div>";
            }
            else
            {
                ket = "<div style=\"width:500px; float:left;\">&nbsp;</div>";
            }

            
                
                Item += "<div style=\"padding-top:10px; width:1200px;\">";
                Item += "<div style=\"width:180px; float:left;\">" + d.NomorDP + "</div>";
                Item += "<div style=\"width:500px; float:left;\">" + ket + "</div>";
                Item += "<div style=\"width:60px; float:left;\">1</div>";
                Item += "<div style=\"width:260px; float:left;\">" + d.TotalBayar.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:120px; float: left; text-align: right;\">" + d.TotalBayar.Value.ToString("N0") + "</div>";
                Item += "</div>";
                Item += "<div style=\"clear: both;\"></div>";
                index++;
            
            itemlist.Text = Item;



            // if (d.DataSales.CompanyBrand.NamaKode == "F")
            //     frank_term.Visible = true;
            // else if (d.DataSales.CompanyBrand.NamaKode == "M")
            //     mondial.Visible = true;
            // else if (d.DataSales.CompanyBrand.NamaKode == "S")
            //     missmondial.Visible = true;
            // else if (d.DataSales.CompanyBrand.NamaKode == "P")
            //     thepalace.Visible = true;


            //  show_Catatan(d);

            //if (d.DataSales.CompanyBrand.NamaKode == "F")
            //    //frank_term.Visible = true;
            //div_note.Visible = true;
        }
    }

  




}