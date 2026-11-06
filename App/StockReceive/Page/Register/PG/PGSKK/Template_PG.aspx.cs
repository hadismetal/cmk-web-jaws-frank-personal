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

            ws.Cells[2, 1].Value = "No. Surat Jalan :";
            ws.Cells[2, 1, 2, 2].Merge = true;

            ws.Cells[4, 1].Value = "Segmen";
            ws.Cells[4, 2].Value = "SKU";
            ws.Cells[4, 3].Value = "ProductID";
            ws.Cells[4, 4].Value = "Item";
            ws.Cells[4, 5].Value = "KadarEmas";
            ws.Cells[4, 6].Value = "KadarTukaranJual";
            ws.Cells[4, 7].Value = "KadarTukaranBeli";
            ws.Cells[4, 8].Value = "Size";
            ws.Cells[4, 9].Value = "Color";
            ws.Cells[4, 10].Value = "Berat";
            ws.Cells[4, 11].Value = "Qty";
            //Write it back to the client
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;  filename=Template_Update_PG_SKK.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
    }

}