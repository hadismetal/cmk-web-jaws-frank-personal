using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

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
    protected int cabang { get { return App.QueryInt(this, "location"); } }
    private int tipe = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            string Lokasi = "";
            var x = d.First();

            if (x.TipeLokasi == 1)
                Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
            else
                Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + x.IDLokasi, Db.Cnn_DB);

            outlet.Text = Lokasi;
        }
        else if (cabang > 0)
        {
            string Lokasi = string.Empty;
            if (tipe == 1)
                Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + cabang, Db.Cnn_DB);
            else
                Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + cabang, Db.Cnn_DB);

            outlet.Text = Lokasi;
        }
        else
            outlet.Text = "ALL";
        printtime.Text = DateTime.Now.ToString();
        //Generate
        fillTable(sender, e);
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;
        tb.Visible = true;

        string spname = "sp_summary_souvenir";
        string query = " exec " + spname
                     + " " + Request.QueryString["locationType"] + ", "
                     + "'" + reverseDate(Request.QueryString["dari"], "from") + "', "
                     + "'" + reverseDate(Request.QueryString["sampai"], "to") + "', "
                     + Request.QueryString["location"] + ", "
                     + Request.QueryString["product"] + ", 1, "
                     + Request.QueryString["brand"];

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);


        if (dt.Rows.Count > 0)
        {
            TableRow tr;
            TableCell tc;
            DataRow last = dt.Rows[0];
            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();
                if (dr["Description"].ToString() == "Total")
                {
                    last = dr;
                    continue;
                }

                tc = new TableCell();
                tc.Text = dr["Description"].ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Kode"].ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Stock"].ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Receiving"].ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Incoming"].ToString();
                //tc.Style.Add("vo", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Outgoing"].ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Sales"].ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Ongoing"].ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["StockReal"].ToString();
                tr.Cells.Add(tc);

                tb.Rows.Add(tr);



            }

            tr = new TableRow();
            tc = new TableCell();
            tc.Text = last["Description"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.ColumnSpan = 2;
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Stock"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Receiving"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Incoming"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            //tc.Style.Add("vo", "right");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Outgoing"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Sales"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Ongoing"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["StockReal"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tb.Rows.Add(tr);


        }
    }
    private string reverseDate(string date, string param)
    {

        string newDate = string.Empty;
        try
        {
            string[] arr = date.Split('/');
            newDate = arr[2] + "-" + arr[0] + "-" + arr[1];
            if (param.Equals("from"))
            {
                newDate = arr[2] + "-" + arr[0] + "-01";
            }
        }
        catch
        {
            string[] arr = date.Split('-');
            newDate = arr[2] + "-" + arr[1] + "-" + arr[0];
            if (param.Equals("from"))
            {
                newDate = arr[2] + "-" + arr[1] + "-01";
            }
        }
        return newDate;
    }
}