using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_PG : System.Web.UI.Page
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
    protected int ReturnID { get { return App.QueryInt(this, "id"); } }
    protected int ItemID { get { return App.QueryInt(this, "item"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            Set();

        }
    }

    protected void Set()
    {
        int totalDataReturn = 0;
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            // string checkFormReturnPG = @"select ID from StockReturnPG where ID = @ID and Draft = 1";
            // using (SqlCommand command = new SqlCommand(checkFormReturnPG, connection)){                
            //     command.CommandTimeout = 1;
            //     command.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar) { Value = ReturnID });

            //     connection.Open();
            //     SqlDataAdapter adapter = new SqlDataAdapter(command);
            //     DataSet ds = new DataSet();
            //     adapter.Fill(ds);
			// 	DataTable dt = ds.Tables[0];                
            //     connection.Close();

            //     if (dt.Rows.Count == 0)
            //     {
            //         Response.Redirect("PG.aspx?d=1");
            //     }                
            // }
            

            string DataPG = @"
                            select c.ID as IDProduct,c.Nomor as PLU, c.KodeBarang, IIF((c.Consignment = 0 OR c.Consignment IS NULL), 'Non-Consignment','Consignment') StatusConsignment,
                            ca.Nama as Item, cb.Nama as ProductLevel,c.KadarLogam, c.MountingR as Ongkos, c.TotalHarga as HargaJual
                            , c.TotalHarga24 as HargaM, c.GrossWeight as Weight, c.ImgPicture
                            from StockReturnPG a 
                            JOIN StockReturnPG_Product b on b.IDForm = a.ID
                            JOIN StockProductPG c on c.ID=b.IDProduct
                            JOIN CharProductItem ca	on ca.ID=C.ProductItem
                            JOIN CharGoldLevel cb on cb.ID = c.ProductLevel
                            WHERE a.ID = @IDReturnPG
                            ";
            
            using (SqlCommand command = new SqlCommand(DataPG, connection)){                
                command.CommandTimeout = 1;
                command.Parameters.Add(new SqlParameter("@IDReturnPG", SqlDbType.VarChar) { Value = ReturnID });

                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                connection.Close();

				DataTable dt = ds.Tables[0];

                int index = 1;
                Response.Write(dt.Rows.Count);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = r["PLU"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["KodeBarang"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["StatusConsignment"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["Item"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["ProductLevel"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["KadarLogam"]).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["Ongkos"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["HargaJual"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["HargaM"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["Weight"]).ToString("N2");
                        tr.Cells.Add(td);

                        HiddenField hf = new HiddenField();
                        hf.ID = "hf" + index.ToString();
                        hf.Value = r["IDProduct"].ToString();
                        hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                        td = new TableCell();
                        td.Controls.Add(hf);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);

                        index++;
                    }            
                }
            }
            
        }
    }
    // protected void Set()
    // {
    //     var d = db.StockReturnPG.SingleOrDefault(p => p.ID == ReturnID);
    //     if (d == null)
    //         Response.Redirect("PG.aspx");
    //     else
    //     {

    //         var rs = from p in db.StockReturnPG_Product
    //                  where p.IDForm == d.ID
    //                  select p;
    //         rs = rs.OrderByDescending(p => p.ID);

    //         if (rs != null)
    //         {
    //             int index = 1;
    //             foreach (var r in rs)
    //             {
    //                 TableRow tr = new TableRow();
    //                 TableCell td;

    //                 td = new TableCell();
    //                 td.Text = r.Nomor;
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = r.StockProductPG.CharProductItem.Nama;
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(r.StockProductPG.MountingR).ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(r.StockProductPG.TotalHarga).ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(r.StockProductPG.TotalHarga24).ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(r.StockProductPG.GrossWeight).ToString("N2");
    //                 tr.Cells.Add(td);

    //                 HiddenField hf = new HiddenField();
    //                 hf.ID = "hf" + index.ToString();
    //                 hf.Value = r.IDProduct.ToString();
    //                 hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

    //                 td = new TableCell();
    //                 td.Controls.Add(hf);
    //                 tr.Cells.Add(td);

    //                 tb.Rows.Add(tr);

    //                 index++;
    //             }
    //         }
    //     }
    // }

    protected void Approve(object sender, EventArgs e)
    {

        var rs = db.StockReturnPG.SingleOrDefault(p=>p.ID == ReturnID);
        if(rs!=null)
        {
            var det = from p in db.StockReturnPG_Product
                      where p.IDForm == ReturnID
                      select p;
            foreach (var r in det)
            {
                var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProduct);
                if (x != null)
                {
                    x.ReturnProduct = true;
                    db.SubmitChanges();

                    StockLedgerPG l = new StockLedgerPG();

                    // Value (See Library DB - Ledger for Value and Remark)
                    decimal Value = -1; // 1 : in , -1 : out
                    int Remark = 9; // See Library DB - Ledger
                    string Keterangan = rs.NoDokumen; // Commonly filled by document number or id 
                    if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

                    string Lokasi = "";
                    if (x.StockActualPG.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + x.StockActualPG.IDLokasi, Db.Cnn_DB);
                    else if (x.StockActualPG.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + x.StockActualPG.IDLokasi, Db.Cnn_DB);

                    //Input
                    l.IDProduct = x.ID;
                    l.TipeLokasi = x.StockActualPG.TipeLokasi;
                    l.IDLokasi = x.StockActualPG.IDLokasi;
                    l.NamaLokasi = Lokasi;
                    l.Value = Value;
                    l.Remark = Remark;
                    l.Keterangan = Keterangan;

                    //User-Info
                    l.Operator = App.UserNama;
                    l.OperatorTgl = DateTime.Now;

                    //Commit
                    db.StockLedgerPG.InsertOnSubmit(l);
                    db.SubmitChanges();
                }
            }
            rs.Approval = App.UserNama;
            rs.ApprovalTgl = DateTime.Now;
            db.SubmitChanges();
        }

        Response.Redirect("PG.aspx?d=1");
    }
}