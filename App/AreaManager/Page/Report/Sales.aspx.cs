using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;

public partial class Page_Summary : System.Web.UI.Page {
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

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

            FillLokasi(sender, e);

            //Generate
            fillFilter();

            //if (App.Refresh(this)) fillTable(sender, e); 
			if (App.Refresh(this)) fillDataTable(sender, e);			
        }
    }
	
	
    public void fillDataTable(object sender, EventArgs e)
    {
        DateTime tempDari = DateTime.ParseExact(dari.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        string stgl1 = tempDari.ToString("yyyy-MM-dd");

        DateTime tempSampai = DateTime.ParseExact(sampai.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        string stgl2 = tempSampai.ToString("yyyy-MM-dd");

        string stipelokasi = "1";
        string sidlokasi = lokasi.SelectedValue;
        string sgroupby = groupby.SelectedValue;
        string stipeproduct = tipeproduct.SelectedValue;

        string query = " exec sp_sales_summary "
                     + "'" + stgl1 + "'"
                     + ",'" + stgl2 + "'"
                     + ",'" + stipelokasi + "'"
                     + ",'" + sidlokasi + "'"
                     + ",'" + stipeproduct + "'"
                     + ",'" + sgroupby + "'"
                     ;

        DataTable dt = new DataTable();
        dt = Db.Rs(" exec sp_sales_summary "
                     + "'" + stgl1 + "'"
                     + ",'" + stgl2 + "'"
                     + ",'" + stipelokasi + "'"
                     + ",'" + sidlokasi + "'"
                     + ",'" + stipeproduct + "'"
                     + ",'" + sgroupby + "'"
                     , System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        if (dt.Rows.Count > 0)
        {
            int index = 1;
            int indexColumn = 1;
            string result = "";

            decimal qty1 = 0m;
            decimal val1 = 0m;
            decimal qty2 = 0m;
            decimal val2 = 0m;
            decimal qty3 = 0m;
            decimal val3 = 0m;
            decimal qty4 = 0m;
            decimal val4 = 0m;
            decimal qty5 = 0m;
            decimal val5 = 0m;
            decimal qty6 = 0m;
            decimal val6 = 0m;
            decimal qty7 = 0m;
            decimal val7 = 0m;
            decimal qty8 = 0m;
            decimal val8 = 0m;

            TableRow tr;
            TableCell td;

            // On all tables' rows
            foreach (DataRow dtRow in dt.Rows)
            {
                tr = new TableRow();

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                indexColumn = 1;

                // On all tables' columns
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.Substring(0, 3) == "qty" || dc.ColumnName.Substring(0, 3) == "val")
                    {
                        result = Convert.ToDecimal(dtRow[dc]).ToString("N0");
                    }
                    else
                    {
                        result = dtRow[dc].ToString();
                    }

                    if (indexColumn == 2) { qty1 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 3) { val1 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 4) { qty2 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 5) { val2 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 6) { qty3 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 7) { val3 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 8) { qty4 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 9) { val4 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 10) { qty5 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 11) { val5 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 12) { qty6 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 13) { val6 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 14) { qty7 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 15) { val7 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 16) { qty8 += Convert.ToDecimal(dtRow[dc]); }
                    else if (indexColumn == 17) { val8 += Convert.ToDecimal(dtRow[dc]); }

                    td = new TableCell();
                    td.Text = result;
                    tr.Cells.Add(td);

                    indexColumn++;
                }

                tb3.Rows.Add(tr);
                index++;
            }

            /// Grand Total
            tr = new TableRow();

            td = new TableCell();
            td.Text = "<b>" + "Total" + "</b>";
            td.HorizontalAlign = HorizontalAlign.Right;
            td.ColumnSpan = 2;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty1.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val1.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty2.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val2.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty3.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val3.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty4.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val4.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty5.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val5.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty6.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val6.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty7.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val7.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + qty8.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + val8.ToString("N0") + "</b>";
            tr.Cells.Add(td);

            tb3.Rows.Add(tr);

        }

    }
	
	

    private decimal p_ForceConvertToDecimal(object objValue)
    {
        decimal decReturn = decimal.Zero;

        try
        {
            decReturn = Convert.ToDecimal(objValue);
        }
        catch
        {
            //  do nothing.
            //  return 0;
        }

        return decReturn;
    }


	
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            basic.SelectedValue = Request.QueryString["basic"];
            //kw.Text = Request.QueryString["kw"];
            groupby.SelectedValue = Request.QueryString["groupby"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            lokasi.Text = Request.QueryString["lokasi"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&groupby=" + groupby.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&lokasi=" + lokasi.Text;
        href += "&basic=" + basic.Text;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "Sales_Export.aspx?id=1";

        //Customize-Qstr
        href += "&groupby=" + groupby.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&lokasi=" + lokasi.Text;
        href += "&basic=" + basic.Text;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "Sales_Print.aspx?id=1";

        //Customize-Qstr
        href += "&groupby=" + groupby.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&lokasi=" + lokasi.Text;
        href += "&basic=" + basic.Text;
        Response.Redirect(href);
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = 1;
        if (Tipe == 1 || Tipe == 2)
        {
            var am = from p in db.DataAreaManager
                     where p.IDUser == App.UserID
                     select p;
            if (am.Count() > 0)
            {
                DLC.LocationListAM(lokasi, App.UserID);
            }
        }
        lokasi.Focus();
    }
}