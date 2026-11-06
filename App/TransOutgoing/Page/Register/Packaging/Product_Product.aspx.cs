using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


public partial class Product_Product : System.Web.UI.Page
{
    private Map_AC _db;

    private Map_AC db
    {
        get
        {
            if (_db == null)
                _db = new Map_AC();
            return _db;
        }
    }

    private Map_DB _d;

    private Map_DB d
    {
        get
        {
            if (_d == null)
                _d = new Map_DB();
            return _d;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (d != null)
            d.Dispose();
        base.Dispose();
    }

    protected int id { get { return App.QueryInt(this); } } /// id trans out

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            //Form-Default
            Fill();
            
        }
    }
    //Fill
    protected void Fill()
    {
        save.Visible = false;

        cbprod.Items.Clear();

        var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id);
        //brand.InnerText = rsH.Company.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        //inputproduct.Text = "13/11/0018";
        //inputproduct.Focus();
        FillPackaging();

    }
    //InputProduct
    protected void btn_inputproduct_Click1(object sender, EventArgs e)
    {
        // var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id);
        // int vB = rsH.IDBrand.Value;
        // int vF = rsH.TipeAsal;

        lit_Message.Text = "";
        //string input = inputproduct.Text.Trim();
        String input = ddl_ProductCode.SelectedValue;
        string qtyo = qty.Text == "" ? "0" : qty.Text.Trim();
        
        decimal qto = Convert.ToDecimal(qtyo);

            try
            {
                // var rs = d.StockActualPackaging.SingleOrDefault(p => p.Packaging.Kode == input && p.TipeLokasi == vF && p.InHand >= 1 && p.IDLokasi == rsH.IDAsal && p.Packaging.Disable == false);
                string getData = @"select b.Kode, b.Nama, a.InHand,b.ImgPicture,b.ID
                           from StockActualPackaging a
                           JOIN Packaging b on b.ID=a.IDProduct
                           CROSS APPLY (SELECT IDAsal,TipeAsal FROM StockOutgoingPackaging WHERE ID = @id) c
                           where a.IDLokasi = c.IDAsal and b.disable=0 and b.Draft=0
                           and b.Kode = @input and InHand >= 1
                           and TipeLokasi = c.TipeAsal ";
                using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
                {
                    using (SqlCommand command = new SqlCommand(getData, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@id", id));
                        command.Parameters.Add(new SqlParameter("@input", input));
                        connection.Open();
                        SqlDataAdapter adapter1 = new SqlDataAdapter(command);
                        DataSet ds1 = new DataSet();
                        adapter1.Fill(ds1);
                        connection.Close();
                        
                        DataTable data = ds1.Tables[0];         
                        
                        if (data.Rows.Count == 0)                        
                        {
                            lit_Message.Text = "<span style='color:red;'>Product not found in this place !!!</span>";
                        }
                        else if (Convert.ToInt32(data.Rows[0]["InHand"]) < qto)
                        {
                            lit_Message.Text = "<span style='color:red;'>Product out of stock !!!</span>";
                        }
                        else
                        {
                            
                            int qi1 = Convert.ToInt32(data.Rows[0]["InHand"]);
                            string qin = qi1.ToString();
            
                            string err_msg = valid(input, cbprod);
                            if (err_msg == "")
                            {
                                string v = data.Rows[0]["ID"].ToString();
            
                                string tu = PackList(Convert.ToString(data.Rows[0]["ImgPicture"]), Convert.ToString(data.Rows[0]["Nama"]), input, qin, qtyo, data.Rows[0]["ID"].ToString());
                                cbprod.Items.Add(new ListItem(tu, v));
                                cbprod.Items[cbprod.Items.Count - 1].Selected = true;
                                qty.Text = "";
            
                            }
                            else
                            {
                                // error message
                                lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                            }                            
            
                            save.Visible = cbprod.Items.Count > 0 ? true : false;
                        }
                                                                        
                    }
                }
            
            }
            catch
            {
                // error message
                lit_Message.Text = "<span style='color:red;'>Invalid Product Code !!!</span>";
            }
       
        }
    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id);
        int vB = rsH.IDBrand.Value;
        int vF = rsH.TipeAsal;

        lit_Message.Text = "";
        //string input = inputproduct.Text.Trim();
        String input = ddl_ProductCode.SelectedValue;
        string qtyo = qty.Text == "" ? "0" : qty.Text.Trim();
        
        decimal qto = Convert.ToDecimal(qtyo);

            try
            {
                var rs = d.StockActualPackaging.SingleOrDefault(p => p.Packaging.Kode == input && p.TipeLokasi == vF && p.InHand >= 1 && p.IDLokasi == rsH.IDAsal && p.Packaging.Disable == false);

            if (rs == null)
            {
                lit_Message.Text = "<span style='color:red;'>Product not found in this place !!!</span>";
            }
            else if(rs.InHand < qto)
            {
                lit_Message.Text = "<span style='color:red;'>Product out of stock !!!</span>";
            }
            else 
            {

                int qi1 = Convert.ToInt32(rs.InHand);
                string qin = qi1.ToString();

                string err_msg = valid(input, cbprod);
                if (err_msg == "")
                {
                    string v = rs.Packaging.ID.ToString();

                    string tu = PackList(rs.Packaging.ImgPicture, rs.Packaging.Nama, input, qin, qtyo, rs.Packaging.ID.ToString());
                    cbprod.Items.Add(new ListItem(tu, v));
                    cbprod.Items[cbprod.Items.Count - 1].Selected = true;
                    qty.Text = "";

                }
                else
                {
                    // error message
                    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                }

                save.Visible = cbprod.Items.Count > 0 ? true : false;
            }
            }
            catch
            {
                // error message
                lit_Message.Text = "<span style='color:red;'>Invalid Product Code !!!</span>";
            }
       
        }

    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.Packaging.SingleOrDefault(p => p.Kode == "" + input + "" && p.Disable == false);

        if (rs1 == null) { message = "Invalid Product"; }
        else
        {
            // 2. Check Apakah Product Item sudah pernah terinput
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + input + " sudah terinput"; }
            }
        }

        if (!Fv.isInt(qty)) message = "Please input quantity";

        // 3. Check Apakah product valid status substoragenya untuk dikirim ke gudang

        return message;
    }

    protected void save_Click(object sender, EventArgs e)    
    {
        //code awal
        for (int i = 0; i < cbprod.Items.Count; i++)
        {
            var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id);
            int vB = rsH.IDBrand.Value;
            int vF = rsH.TipeAsal;
          
            if (cbprod.Items[i].Selected)
            {
                decimal qty = Convert.ToDecimal(Request.Form["qty_" + cbprod.Items[i].Value]);

                var rs = d.StockActualPackaging.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbprod.Items[i].Value)
                                                                 && p.TipeLokasi == vF && p.IDLokasi == rsH.IDAsal);

                StockOutgoingPackaging_Product n = new StockOutgoingPackaging_Product()
                {
                    //Input
                    IDForm = id,
                    IDProduct = Convert.ToInt32(cbprod.Items[i].Value),
                    Qty = qty
                    //SubStorage = rs.Substorage
                };
                d.StockOutgoingPackaging_Product.InsertOnSubmit(n);

                //Commit
                d.SubmitChanges();
            }
        }
        Db.Execute("exec sp_eliminate_duplicate_packaging " + id.ToString(), Db.Cnn_DB);

        //Redirect
        Response.Redirect("Product_Finish.aspx?id=" + id);        
    }

    protected void save_Click1(object sender, EventArgs e)    
    {
        int CounterSuccess = cbprod.Items.Count;
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();

            //code awal        
            for (int i = 0; i < cbprod.Items.Count; i++)
            {
                // var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id);
                // int vB = rsH.IDBrand.Value;
                // int vF = rsH.TipeAsal;

                if (cbprod.Items[i].Selected)
                {
                    decimal qty = Convert.ToDecimal(Request.Form["qty_" + cbprod.Items[i].Value]);

                    string query = @"                                                                                    
                        INSERT INTO StockOutgoingPackaging_Product (IDForm, IDProduct, Qty)
                        SELECT ID, @IDProduct, @Qty
                        FROM StockOutgoingPackaging WHERE ID = @id;                            
                    ";

                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.CommandTimeout = 10;
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@Qty", qty);
                            command.Parameters.AddWithValue("@IDProduct", Convert.ToInt32(cbprod.Items[i].Value));

                            command.ExecuteNonQuery();
                        }
                    }
                    catch (System.Exception)
                    {
                        CounterSuccess -= 1;
                    }
                }
            }
            connection.Close();
        }

        Db.Execute("exec sp_eliminate_duplicate_packaging " + id.ToString(), Db.Cnn_DB);

        //Redirect
        if (CounterSuccess == cbprod.Items.Count)
        {
            Response.Redirect("Product_Finish.aspx?id=" + id);        
        }
        else
        {
            lit_Message.Text = "<span style='color:red;'>No product selected or all products failed to save.</span>";
        }
    }



    protected void FillPackaging()
    {
        //var rs = from p in d.Packaging
        //         where !p.Draft && !p.Disable //&& ((p.Nama.Contains(query)) || p.Kode.Contains(query))
                    
        //         select new { id = p.Kode, label = p.Nama };

        // var p1 = (from p in d.StockOutgoingPackaging
        //          where p.ID == id
        //          select p).SingleOrDefault();
        
        // var qr = (from t1 in d.StockActualPackaging
        //           join t2 in d.Packaging on t1.IDProduct equals t2.ID
        //           where t1.IDLokasi == p1.IDAsal && t2.Disable == false && t2.Draft == false
        //           select new { id = t2.Kode, label = t2.Nama });


        string getData = @"select b.Kode, b.Nama, a.InHand
                           from StockActualPackaging a
                           JOIN Packaging b on b.ID=a.IDProduct
                           CROSS APPLY (SELECT IDAsal,TipeAsal FROM StockOutgoingPackaging WHERE ID = @id) c
                           where a.IDLokasi = c.IDAsal and b.disable=0 and b.Draft=0
                           and TipeLokasi = c.TipeAsal ";
                           
            ddl_ProductCode.Items.Clear();

            using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
            {
                using (SqlCommand command = new SqlCommand(getData, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", id));
                    connection.Open();
                    SqlDataAdapter adapter1 = new SqlDataAdapter(command);
                    DataSet ds1 = new DataSet();
                    adapter1.Fill(ds1);
                    connection.Close();
                    
                    DataTable data = ds1.Tables[0];         
                    
                    foreach (DataRow r in data.Rows)
                    {
                        string t = Convert.ToString(r["Nama"]) + " (" + Convert.ToString(r["Kode"])+ ") (Qty : "+Convert.ToDecimal(r["InHand"]).ToString("N0")+")";
                        string v = Convert.ToString(r["Kode"]);                        
                        ListItem item1 = new ListItem(t, v);

                        ddl_ProductCode.Items.Add(item1);
                    }
                }
            }
    
        // foreach (var r in qr)
        // {
        //     string t = r.label + " (" + r.id + ")";
        //     string v = r.id;
        //     ListItem item = new ListItem(t, v);

        //     ddl_ProductCode.Items.Add(item);
        // }
    }

    private string PackList(string pic, string nama, string kode, string qtyin, string qtyout, string id)
    {
        string x;
        x = "<div style='width: 850px;'>"
            + "<div style='padding:5px; display: table-cell;'>"
            + "<div style='float: left; width: 150px;'><img src='/Images.aspx?path=" + pic + "' style='max-width:95px; max-height:85px' /></div>"
            + "<div style='float: left; width: 195px;'>"+  nama + "</div>"
            + "<div style='float: left; width: 200px;'>" + kode+ "</div>"
            + "<div style='float: left; width: 150px;'>" + qtyin + "</div>"
            + "<div style='float: left; width: 130px;'><input size=\"6\" class=\"span2\" type=\"text\" name=\"qty_" + id + "\" value=\"" + qtyout + "\" /></div>"
            + "</div>"
            + "</div>"
            + "<div style='clear: both; font-size:1px; width:5px;'>&nbsp;</div>"
            ;
        return x;
    }
}