using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using OfficeOpenXml;

public partial class Page_DJ : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            generateExcel();
        }
    }

    private void generateExcel()
    {
        using (ExcelPackage pck = new ExcelPackage())
        {
            //Create the worksheet
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");

            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
            DataTable dt = new DataTable();

            dt.Columns.Add("Stone");
            dt.Columns.Add("Harga_USD");
            dt.Columns.Add("Persen");

            ws.Cells.LoadFromDataTable(dt, true);
            //Write it back to the client
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;  filename=Template_Update_Rapaport.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
    }

}