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

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Fill();
        }
    }
    protected void Error(string Message)
    {

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
        decimal totalGross = 0;
        decimal totalNett = 0;
        int totalQty = 0;
        int totalButir = 0;
        decimal totalCarat = 0;

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

                    td = new TableCell();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    tr.Cells.Add(td);

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
                    string netto = Convert.ToDecimal(row["NettoWeight"]).ToString("N2");
                    td.Text = netto;
                    tr.Cells.Add(td);

                    totalGross += Convert.ToDecimal(row["GrossWeight"]);
                    totalNett += Convert.ToDecimal(netto);
                    totalQty++;
                }

                td = new TableCell();
                td.Text = row["Parcel"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["TotalButir"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(row["TotalCarat"]).ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["GIA"].ToString();
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
                spl = row["SPL"].ToString();

                totalButir += Convert.ToInt32(row["TotalButir"]);
                totalCarat += Convert.ToDecimal(row["TotalCarat"]);
            }
            
            TableRow rows = new TableRow();
            TableCell cell;

            cell = new TableCell();
            cell.ColumnSpan = 4;
            cell.Text = "Total";
            cell.Style.Add("text-align", "center");
            cell.Style.Add("font-weight", "bold");
            rows.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = totalQty.ToString();
            cell.Style.Add("font-weight", "bold");
            rows.Cells.Add(cell);

            cell = new TableCell();
            rows.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = totalGross.ToString("N2");
            cell.Style.Add("font-weight", "bold");
            rows.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = totalNett.ToString("N2");
            cell.Style.Add("font-weight", "bold");
            rows.Cells.Add(cell);

            cell = new TableCell();
            rows.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = totalButir.ToString();
            cell.Style.Add("font-weight", "bold");
            rows.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = totalCarat.ToString("N3");
            cell.Style.Add("font-weight", "bold");
            rows.Cells.Add(cell);

            tb.Rows.Add(rows);
        }
    }
}