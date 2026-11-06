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
            //Set();

        }
        Set();
    }

    protected void Set()
    {
        int totalDataReturn = 0;
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string checkFormReturnPG = @"select ID from StockReturnPG where ID = @ID and Draft = 1
            
                                        SELECT ImgPicture FROM StockProductPG WHERE ID = @ItemID";
            using (SqlCommand command = new SqlCommand(checkFormReturnPG, connection))
            {
                command.CommandTimeout = 1;
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar) { Value = ReturnID });
                command.Parameters.Add(new SqlParameter("@ItemID", SqlDbType.VarChar) { Value = ItemID });

                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                DataTable Images = ds.Tables[1];
                connection.Close();

                if (dt.Rows.Count == 0)
                {
                    Response.Redirect("PG.aspx?d=1");
                }
                
                if (Images.Rows.Count > 0)
                {
                    Upload.Image(imgpicture, Images.Rows[0]["ImgPicture"].ToString());
                }
            }
            

            string DataPG = @"
                            select b.ID as IDDETAIL,c.ID as IDProduct,c.Nomor as PLU, c.KodeBarang, IIF((c.Consignment = 0 OR c.Consignment IS NULL), 'Non-Consignment','Consignment') StatusConsignment,
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

                        string x = "";

                        x += "<div class=\"btn-group\">";
                        x += "  <a class=\"btn btn-danger\" href=\"PG_Delete.aspx?id=" + r["IDDETAIL"].ToString() + "\">";
                        x += "      <i class=\"icon-trash\"></i> ";
                        x += "      Delete";
                        x += "  </a>";
                        x += "</div>";

                        td = new TableCell();
                        td.Text = x;
                        tr.Cells.Add(td);

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
        // var d = db.StockReturnPG.SingleOrDefault(p => p.ID == ReturnID);
        // if (d == null)
        //     Response.Redirect("PG.aspx");
        // else
        // {
        //     if (ItemID > 0)
        //     {
        //         var x = db.StockProductPG.SingleOrDefault(p => p.ID == ItemID);
        //         if (x != null)
        //             Upload.Image(imgpicture, x.ImgPicture);
        //     }

        //     var rs = from p in db.StockReturnPG_Product
        //              where p.IDForm == d.ID
        //              select p;
        //     rs = rs.OrderByDescending(p => p.ID);

        //     if (rs != null)
        //     {
                
        //     }
        // }
    }
    
    
    // protected void Set()
    // {
    //     var d = db.StockReturnPG.SingleOrDefault(p => p.ID == ReturnID);
    //     if (d == null)
    //         Response.Redirect("PG.aspx");
    //     else
    //     {
    //         if (ItemID > 0)
    //         {
    //             var x = db.StockProductPG.SingleOrDefault(p => p.ID == ItemID);
    //             if (x != null)
    //                 Upload.Image(imgpicture, x.ImgPicture);
    //         }

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

    //                 string x = "";

    //                 x += "<div class=\"btn-group\">";
    //                 x += "  <a class=\"btn btn-danger\" href=\"PG_Delete.aspx?id=" + r.ID.ToString() + "\">";
    //                 x += "      <i class=\"icon-trash\"></i> ";
    //                 x += "      Delete";
    //                 x += "  </a>";
    //                 x += "</div>";

    //                 td = new TableCell();
    //                 td.Text = x;
    //                 tr.Cells.Add(td);

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

    protected void Add(object sender, EventArgs e)
    {
        int index = 1;

        TextBox txt = new TextBox();
        CheckBox cb_img = new CheckBox();
        CheckBox cb_bd = new CheckBox();
        string hf = "";
        foreach (TableRow row in tb.Rows)
        {

            try
            {
                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("tb_" + index.ToString());
                cb_img = (CheckBox)row.FindControl("cb_img" + index.ToString());
                cb_bd = (CheckBox)row.FindControl("cb_bd" + index.ToString());
                var d = db.StockReturnPG_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.IDForm == ReturnID);
                if (d != null)
                {

                    db.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }

        string plu = kw.Text;
        if (plu != "")
        {
            var rt = db.StockReturnPG.SingleOrDefault(p=>p.ID == ReturnID);
            var d = db.StockProductPG.SingleOrDefault(p => p.Nomor == "" + plu + "" && p.StockActualPG.TipeLokasi == 0 && p.StockActualPG.IDLokasi == rt.IDWarehouse && p.ReturnProduct == false);
            if (d == null)
            {
                err.InnerText = "PLU Tidak ditemukan.";
                err.Visible = true;
            }

            else
            {

                Upload.Image(imgpicture, d.ImgPicture);
                err.Visible = false;

                //2. Validasi cek di dokumen yang aktif tidak boleh ada double PLU yang sama
                var val2 = db.StockReturnPG_Product.SingleOrDefault(p => p.IDForm == ReturnID && p.StockReturnPG.Draft == true && p.Nomor == "" + plu + "");

                if (val2 != null)
                {
                    err.InnerText = " PLU SUDAH TERINPUT , TOLONG CEK LAGI PLU YANG DI INPUT ";
                    err.Visible = true;
                    return;
                }

                //3. Validasi cek di dokumen yang sudah pernah di approve / draft
                var val3 = db.StockReturnPG_Product.SingleOrDefault(p => p.Nomor == "" + plu + "" && p.StockReturnPG.Draft == false);

                if (val3 != null)
                {
                    var doc = val3.StockReturnPG.NoDokumen.ToString();
                    err.InnerText = " PLU SUDAH PERNAH DI INPUT DI DOKUMEN" + "   " + doc;
                    err.Visible = true;
                    return;
                }

                var val14 = db.StockOutgoingPG_Product.SingleOrDefault(p => p.IDProduct == d.ID && p.StockOutgoingPG.Approval == null);
                if (val14 != null)
                {
                    err.InnerText = " PLU SEDANG DI INPUT DI DOKUMEN OUTGOING";
                    err.Visible = true;
                    return;
                }

                var cekProduct = db.StockReturnPG_Product.FirstOrDefault(a => a.IDForm == ReturnID && a.Nomor == plu);
                
                var KadarBarometer = (from a in db.StockReturnPG_Product
                                join b in db.StockProductPG on a.Nomor equals b.Nomor
                                where a.IDForm == ReturnID
                                select new 
                                {
                                a.Nomor,
                                b.KadarLogam
                                })
                                .FirstOrDefault();
                var getInputedProductData = db.StockProductPG.FirstOrDefault(a => a.Nomor == plu);

                if (cekProduct != null)
                {
                    err.InnerText = " Duplikat PLU, Silahkan pilih PLU lain";
                    err.Visible = true;
                    return;
                }

                if (KadarBarometer != null)
                {
                    if(KadarBarometer.KadarLogam != getInputedProductData.KadarLogam ){
                        err.InnerText = "Kadar Logam Harus Sama, Silahkan pilih PLU lain";
                        err.Visible = true;
                        return;
                    }
                }
                    // if(KadarBarometer != null){
                    //     if(KadarBarometer.KadarLogam != getInputedProductData.KadarLogam ){
                    //         err.InnerText = "Kadar Logam Harus Sama, Silahkan pilih PLU lain";
                    //         err.Visible = true;
                    //         return;
                    //     }
                    // }else{
                StockReturnPG_Product item = new StockReturnPG_Product();
                    item.IDForm = ReturnID;
                    item.Nomor = d.Nomor;
                    item.IDProduct = d.ID;

                    db.StockReturnPG_Product.InsertOnSubmit(item);
                    db.SubmitChanges();

                    Upload.Image(imgpicture, d.ImgPicture);
                    err.Visible = false;

                    //var x = db.StockReturnPG_Product.SingleOrDefault(p => p.AuditID == AuditID && p.IDProduct == d.ID);
                    //if (x == null)
                    //{
                
                    //}
                    Response.Redirect("PG_List.aspx?id=" + ReturnID + "&item=" + d.ID.ToString());
                    // }
                
                
            }
        }
    }

    protected void Finish(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
            {
                string UpdatedStockReturnPG = @"
                if exists (select * from StockReturnPG where ID = @ID)
                BEGIN
                    UPDATE A SET Draft = 0 FROM StockReturnPG A WHERE ID = @ID 
                END 
                ";
                using (SqlCommand command = new SqlCommand(UpdatedStockReturnPG, connection)){
                    command.CommandTimeout = 1;
                    command.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar) { Value = ReturnID });
                    connection.Open();                                  
                    command.ExecuteReader();
                    connection.Close(); 
                }
            }
            Response.Redirect("PG.aspx?d=1");
        }
        catch (Exception ex)
        {      
            Response.Write(ex.Message);      
        }
        // var rs = db.StockReturnPG.SingleOrDefault(p=>p.ID == ReturnID);
        // if(rs!=null)
        // {
        //     rs.Draft = false;
        //     db.SubmitChanges();
        // }

    }
}