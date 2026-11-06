using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Page_Approval_Product : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            edc.Enabled = false;
            jenisKartu.Enabled = false;
            program.Enabled = false;
            getData();

        }
    }

    private void getData()
    {
        var rs = db.MDRRate.Where(x => x.ID.Equals(myID)).ToList().FirstOrDefault();

        edc.Text = DLC.EDC(rs.IDBank);
        jenisKartu.Text = DLC.JenisKartu(rs.IDJenisKartuKredit);
        program.Text = DLC.Program(rs.IDProgram);
        rate.Text = rs.MDRRate1.ToString("N2");

    }

    //Display Table

    protected void save_ServerClick(object sender, EventArgs e)
    {
        var rs = db.MDRRate.SingleOrDefault(x => x.ID.Equals(myID));

        rs.MDRRate1 = Convert.ToDecimal(rate.Text);

        db.SubmitChanges();

        App.Reload(this, myID);
    }
}