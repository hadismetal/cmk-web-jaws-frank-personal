using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class DJ : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Packaging_Detail_Outgoing.xls");
            fillTable(sender, e);

        }
    }


       //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;

        int tF = 0; // type from
        int tT = 0; // type to

        int brand = Convert.ToInt32(Request.QueryString["brand"]);
        int lokasiFrom = Convert.ToInt32(Request.QueryString["lokasifrom"]);
        int lokasiTo = Convert.ToInt32(Request.QueryString["lokasito"]);
        int status = Convert.ToInt32(Request.QueryString["status"]);
        int tipeLokasiFrom = Convert.ToInt32(Request.QueryString["tipelokasifrom"]);
        int tipeLokasiTo = Convert.ToInt32(Request.QueryString["tipelokasito"]);
        int packaging = Convert.ToInt32(Request.QueryString["packaging"]);
        string dateFrom = Request.QueryString["pf"];
        string dateTo = Request.QueryString["pt"];
		
        string spname = "sp_detailOutgoingPackaging_export";
        string query = " exec " + spname
                     + "'" + dateFrom + "', "
                     + "'" + dateTo + "', "
                     + packaging + ", "
                     + lokasiFrom + ", "
                     + lokasiTo + ", "
                     + tipeLokasiFrom + ", "
                     + tipeLokasiTo + ", "
                     + status + ", "
                     + brand;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);


        int index = 1;
        if (dt.Rows.Count == 0) { App.TableNoResult(tb); }
        else
        {
            foreach (DataRow dr in dt.Rows)
            {

                

                TableRow tr = new TableRow();
                TableCell td;
                
                td = new TableCell();
                if(!String.IsNullOrEmpty(dr["Nomor"].ToString())) {             
                    td.Text = "<p>" + index++ + ".</p>";
                }else{
                    td.Text = "<p></p>";
                }
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Nomor"].ToString(); 
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Tgl"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Location"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Tujuan"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Packaging"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["QtyKirim"]).ToString("N2");
                tr.Cells.Add(td);
                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["QtyTerima"]).ToString("N2");
                tr.Cells.Add(td);
                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["SelisihTerima"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Status"].ToString();
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                
            }
        }

        tb.Visible = true;
        bool isValidFromTo = true;
    }

    protected string TD_Nama(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        /*else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }*/

        return x;
    }
}