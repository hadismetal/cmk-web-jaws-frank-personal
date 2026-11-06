using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using ExcelDataReader;

public partial class Page_Update_Process_1B : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);
	}
	protected void uploadData(object sender, EventArgs e)
	{
        if (Path.GetExtension(FileUpload1.PostedFile.FileName) == ".xlsx")
        {
            IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(FileUpload1.PostedFile.InputStream);

            DataSet ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            if (ds.Tables.Count > 0)
            {
                save_data(ds.Tables[0]);
            }
        }
        else
        {
            error.Text = "File tidak sesuai, gunakan file extensi .xlsx";
            error.Visible = true;
        }
    }

    protected void save_data(DataTable tb)
    {
        try
        {
            foreach (DataRow row in tb.Rows)
            {
                var d = db.StoneCosting1B.SingleOrDefault(p => p.Stone1B.Nama == row["Stone"].ToString() && !p.Stone1B.Disable);

                if (d == null) continue;
                else
                {
                    d.PendingH = Convert.ToDecimal(row["Harga_USD"]);
                    d.PendingP = Convert.ToDecimal(row["Persen"]);

                    //Rumus Rapaport
                    d.HargaPending = d.PendingH + (d.PendingH * (d.PendingP / 100));

                    //Mutasi
                    d.Mutasi = d.Harga.HasValue ? d.HargaPending - d.Harga : d.HargaPending;
                    d.MutasiPersen = d.Harga != 0 ? (d.Mutasi / d.Harga) * 100 : null;

                    //Status
                    d.Stone1B.StatusCosting = 2;

                    //User-Info
                    d.Operator = App.UserNama;
                    d.OperatorTgl = DateTime.Now;

                    //Commit
                    db.SubmitChanges();

                    //Redirect

                }
            }

            Response.Write("<script language=javascript>this.window.opener = null;window.open('','_self'); window.close();   </script>");
        }
        catch
        {
            error.Text = "Format template/data tidak sesuai";
            error.Visible = true;
        }

    }

    protected void cancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=javascript>this.window.opener = null;window.open('','_self'); window.close();   </script>");
    }
}