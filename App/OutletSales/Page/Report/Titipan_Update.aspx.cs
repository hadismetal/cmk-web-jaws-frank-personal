using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_Register_Sales_Detail : System.Web.UI.Page {
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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			

            DL.StatusTitipanList(statustitipan);

            fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Sales.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID);
		if (d == null) DataError();
		else {
            string tglPengembalian = "";
			//Header
            header_nomor.InnerHtml =
				"<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Trans. Form
            string Brand = "";
            if(d.TipeLokasi == 1)
                Brand = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi,Db.Cnn_DB);
            else if(d.TipeLokasi == 2)
                Brand = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi,Db.Cnn_DB);

            tglPengembalian = Db.SingleString("SELECT ISNULL(FORMAT(tglPengembalian, 'dddd, dd MMMM yyyy', 'id-ID'),'-') as TanggalPengembalian FROM DocTitipan WHERE ID=" + myID, Db.Cnn_DB);

            brand.InnerHtml = Brand;
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
            customer.InnerHtml = d.DataCustomer.Nama;
			tgl.InnerHtml = Str.Tgl(d.Tgl);
            tglBalikin.InnerHtml = tglPengembalian;

            statustitipan.SelectedValue = d.StatusTitipan.Value.ToString();
            if (d.StatusTitipan == 5)
            {
                titipan.Visible = false;
            }            

			
			//Stone
            fillItem();

		}
	}

	//Stone
    protected void fillItem()
    {
        var rs = from p in db.DocTitipan_Product
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.DocTitipan.Nomor;
            tr.Cells.Add(td);

            string ProductItem = "";
            string PLU = "";

            if (r.IDProduct.HasValue)
            {
                ProductItem = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                PLU = r.StockProductDJ.Nomor;
            }
            else if (r.IDProductTitipan.HasValue)
            {
                ProductItem = r.StockProductDJCustomer.StockProductDJCustomer_CharProduct.CharProductItem.Nama;
                PLU = r.StockProductDJCustomer.Nomor;
            }

            td = new TableCell();
            td.Text = ProductItem;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = PLU;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            index++;

        }
    }

    
    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        return x;
    }
    protected string TD_Nama(string Nama, string GIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

        return x;
    }
    protected void Update_Click(object sender, EventArgs e)
    {
        var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID && p.StatusTitipan !=5 && p.StatusTitipan.HasValue);
        if (d == null) DataError();
        else
        {

            if (Convert.ToInt32(statustitipan.SelectedValue) == 5)
            {
                using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
                {
                    connection.Open();
                    string query = @"UPDATE DocTitipan SET StatusTitipan=@Status, OperatorPengembalian = @UserID, TglPengembalian = GETDATE() WHERE ID=@ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter("@ID", d.ID));
                    command.Parameters.Add(new SqlParameter("@UserID", App.UserNama));
                    command.Parameters.Add(new SqlParameter("@Status", Convert.ToInt32(statustitipan.SelectedValue)));

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                d.StatusTitipan = Convert.ToInt32(statustitipan.SelectedValue);
                db.SubmitChanges();                
            }
            

        }
        App.ReloadClose(this);
    }

}