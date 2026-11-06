using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
public partial class DJ_Print : System.Web.UI.Page
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
    protected int IDSPL { get { return Convert.ToInt32(Request.QueryString["idr"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            try
            {
                if (IDSPL > 0)
                {
                    Void(IDSPL);
                }
            }
            catch { }
            Fill();
        }
    }
    protected void Error(string Message)
    {

    }

    protected void Void(int ID)
    {
        Db.Execute("EXEC sp_deleteWorkOrderLebur " + ID, Db.Cnn_DB); // Outgoing

        string href = "DJ_WorkOrderDetail.aspx?d=1&id=" + myID;

        Response.Redirect(href);
    }

    protected void Fill()
    {
        //Form-Default
        FillHeader();
        FillTable();
    }

    //Fill
    protected void FillHeader()
    {
        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }
    protected void FillTable()
    {
        string spl = string.Empty;
        tb.Visible = true;

        //SELECT
        var dt = Db.Rs("EXEC sp_getWorkOrderLebur " + myID, Db.Cnn_DB); // Outgoing

        // = rs.OrderBy(p => p.StockProductDJ.Nomor);

        int index = 1;

        if(dt.Rows.Count > 0)
        {
            nomor.InnerText = dt.Rows[0]["WorkOrder"].ToString();
            operatortgl.InnerText = dt.Rows[0]["OperatorTgl"].ToString();
            @operator.InnerText = dt.Rows[0]["Operator"].ToString();
            
            foreach(DataRow row in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td;

                if (spl.Equals(row["SPL"].ToString()))
                {
                    td = new TableCell();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);

                    if (row["Frame"].ToString() == "LD")
                    {
                        td = new TableCell();
                        td.Text = row["PLU"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["Item"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["Frame"].ToString();
                        tr.Cells.Add(td);
                    }
                    else
                    {
                        td = new TableCell();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        tr.Cells.Add(td);
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);
                }
                else
                {
                    td = new TableCell();
                    td.Text = TD_HyperLink(Convert.ToInt32(row["ID"]));
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["SPL"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["PLU"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Item"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Frame"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "1";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["KadarLogam"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["GrossWeight"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["NettoWeight"]).ToString("N2");
                    tr.Cells.Add(td);
                }

                td = new TableCell();
                td.Text = row["Parcel"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["TotalButir"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["TotalCarat"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["GIA"].ToString();
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
                spl = row["SPL"].ToString();
            }
        }
        else {
            VoidAll();
        }
    }

    protected void print_ServerClick(object sender, EventArgs e)
    {
        string href = "DJ_PrintWO.aspx?id=" + myID;

        Response.Redirect(href);
    }

    protected string TD_HyperLink(int ID)
    {
        string x = string.Empty;


        x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"DJ_WorkOrderDetail.aspx?id=" + myID + "&idr=" + ID + "\">";
        x += "  <i class=\"icon-trash\"></i> ";
        x += "  Void";
        x += "</a>";

        return x;
    }

    protected void btnVoid_ServerClick(object sender, EventArgs e)
    {
        VoidAll();
    }

    private void VoidAll()
    {
        Db.Execute("EXEC sp_deleteWorkOrderLebur " + myID + ", 'ALL'", Db.Cnn_DB); 

        string href = "DJ_WorkOrder.aspx?d=1";

        Response.Redirect(href);
    }
}