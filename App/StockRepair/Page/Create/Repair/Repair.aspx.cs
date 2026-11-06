using System;
using System.Linq;

public partial class Page_Register_Repair_Form : System.Web.UI.Page
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

    string ID = "";

    protected int myID { get { return App.QueryInt(this, "id"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int myIDLokasi { get { return App.QueryInt(this, "idlokasi"); } }
    protected int status { get { return App.QueryInt(this, "status"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            keterangan.Focus();

            //Form
            fillForm();

            //Form-Default
            tgl.Text = Str.Tgl(DateTime.Today);
            //tglselesai.Text = Str.Tgl(DateTime.Today.AddMonths(1));
            tglselesai.Text = Str.Tgl(DateTime.Today);
        }
    }

    //Form
    protected void fillForm()
    {
        if (status != 1)
        {

            DocRepair n = new DocRepair();

            //Location
            n.IDLokasi = myIDLokasi;
            n.TipeLokasi = tipe;

            //Status
            n.Draft = true;
            n.DraftDate = DateTime.Now;

            //User-Info
            n.Operator = App.UserNama;
            n.OperatorTgl = DateTime.Now;
            n.StatusRepair = 0;
            n.Sumber = 1;

            //Commit
            db.DocRepair.InsertOnSubmit(n);
            db.SubmitChanges();

            string qstr = "?id=" + n.ID;
            search_plu.HRef += qstr;
            ID = n.ID.ToString();
        }
        else
        {
            var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
            if (d == null) { }
            else
            {
                string Brand = "";
                if (d.TipeLokasi == 1)
                {
                    var k = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                    Brand = k.Nama;
                }
                else if (d.TipeLokasi == 2)
                {
                    var k = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                    Brand = k.Nama;
                }

                //warehouse.InnerHtml = Brand;

                if (d.IDProduct != null)
                    plu.Text = d.StockProductDJ.Nomor;
                if (d.IDProductTitipan != null)
                    plu.Text = d.StockProductDJCustomer.Nomor;
            }
            string qstr = "?id=" + myID;
            search_plu.HRef += qstr;
            ID = myID.ToString();
        }
    }

    //Save
    protected bool valid
    {
        get
        {
            bool x = true;
            if (Fv.isDateTime(tgl, tgl_ERR) == false)
                x = false;
            if (Fv.isDateTime(tglselesai, tglselesai_ERR) == false)
                x = false;
            return x;
        }
    }
    protected bool validPLU
    {
        get
        {
            if (plu.Text == "")
                return false;
            else
                return true;
        }
    }
    protected void save_Click(object sender, EventArgs e)
    {
        if (valid && validPLU)
        {
            var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
            if (d != null)
            {
                d.Keterangan = Str.CapFirst(keterangan.Text);
                d.Tgl = Convert.ToDateTime(tgl.Text);
                d.TglSelesai = Convert.ToDateTime(tglselesai.Text);
                //d.Nomor = Generate_Nomor(d);
                d.Nomor = "";

                db.SubmitChanges();
            }

            db.ExecuteCommand(
                "UPDATE DocRepair SET Flag = 0 WHERE ID = {0}", myID
            );

            Response.Redirect("Repair_Data.aspx?id=" + myID);
        }
    }


}