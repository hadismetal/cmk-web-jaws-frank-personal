using System;
using System.Linq;

public partial class Page_Register_Sales_Form : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            keterangan.Focus();

            //Form
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("Sales.aspx"); }

    //Form
    protected void fillForm()
    {

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            so.InnerHtml = d.Nomor;
        }

    }
    protected void save_Click(object sender, EventArgs e)
    {
        //var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        //if(d!=null)
        //{
        //    d.KeteranganVoid = keterangan.Text;
        //    db.SubmitChanges();
        //}
        //Response.Redirect("Sales_Void.aspx?id=" + myID);

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d != null)
        {
            string txtKeterangan = keterangan.Text.Trim();
            txtKeterangan += " (Void by " + App.UserNama + ", " + Str.FullDateJam(System.DateTime.Now) + ")";

            d.KeteranganVoid = txtKeterangan;
            db.SubmitChanges();

            // void sms courtesy
            try
            {
                var cust = d.DataCustomer.NoCustomer.ToString().Trim();

                Db.Execute("EXEC sp_sms_courtesy "
                    + "@so_no = N'" + d.Nomor + "'"
                    + "," + "@so_id = " + d.ID + ""
                    + "," + "@so_void_flag = 1"
                    + "," + "@so_void_document = N'" + d.Nomor + "'"
                    + "," + "@so_void_nocustomer = N'" + cust + "'"
                    + "," + "@so_void_description = N'" + txtKeterangan + "'"
                , Db.Cnn_DB);
            }
            catch { }

        }
        Response.Redirect("Sales_Void.aspx?id=" + myID);
    }


}