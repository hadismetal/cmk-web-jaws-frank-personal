using System;
using System.Linq;
using System.Web.UI.WebControls;
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

        cbl_Product.Items.Clear();

        var rsH = d.StockOutgoingSouvenir.SingleOrDefault(p => p.ID == id);
        //brand.InnerText = rsH.Company.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        //inputproduct.Text = "13/11/0018";
        //inputproduct.Focus();
        FillSouvenir();

    }
    //InputProduct

    protected void btn_inputproduct_Click1(object sender, EventArgs e)
    {
        // var rsH = d.StockOutgoingSouvenir.SingleOrDefault(p => p.ID == id);
        // int vB = rsH.IDBrand.Value;
        // int vF = rsH.TipeAsal;

        lit_Message.Text = "";
        //string input = inputproduct.Text.Trim();
        String input = ddl_ProductCode.SelectedValue;
        string qtyo = qty.Text == "" ? "0" : qty.Text.Trim();
        
        decimal qto = Convert.ToDecimal(qtyo);

            try
            {
                // var rs = d.StockActualSouvenir.SingleOrDefault(p => p.Souvenir.Kode == input && p.TipeLokasi == vF && p.InHand >= 1 && p.IDLokasi == rsH.IDAsal && p.Souvenir.Disable == false);
                string getData = @"select b.Kode, b.Nama, a.InHand,b.ImgPicture,b.ID
                           from StockActualSouvenir a
                           JOIN Souvenir b on b.ID=a.IDProduct
                           CROSS APPLY (SELECT IDAsal,TipeAsal FROM StockOutgoingSouvenir WHERE ID = @id) c
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
            
                            string err_msg = valid(input, cbl_Product);
                            if (err_msg == "")
                            {
                                string v = data.Rows[0]["ID"].ToString();
            
                                // string tu = PackList(Convert.ToString(data.Rows[0]["ImgPicture"]), Convert.ToString(data.Rows[0]["Nama"]), input, qin, qtyo, data.Rows[0]["ID"].ToString());
                                string t = ProductList_Souvenir(
                                            Convert.ToString(data.Rows[0]["Nama"]), qty.Text, Convert.ToInt32(data.Rows[0]["ID"]).ToString());
                                            
                                cbl_Product.Items.Add(new ListItem(t, v));
                                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                                qty.Text = "";
            
                            }
                            else
                            {
                                // error message
                                lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
                            }                            
            
                            save.Visible = cbl_Product.Items.Count > 0 ? true : false;
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
        var rsH = d.StockOutgoingSouvenir.SingleOrDefault(p => p.ID == id);
        int vB = rsH.IDBrand.Value;
        int vF = rsH.TipeAsal;

        lit_Message.Text = "";
        //string input = inputproduct.Text.Trim();
        String input = ddl_ProductCode.SelectedValue;

        try
        {
            var rs = d.StockActualSouvenir.SingleOrDefault(p => p.Souvenir.Kode == input && p.TipeLokasi == vF && p.InHand >= 1 && p.IDLokasi == rsH.IDAsal);

            string err_msg = valid(input, cbl_Product);
            if (err_msg == "")
            {
                string v = rs.Souvenir.ID.ToString();
                string t = ProductList_Souvenir(
                        rs.Souvenir.Nama, qty.Text, rs.Souvenir.ID.ToString());
                cbl_Product.Items.Add(new ListItem(t, v));
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                //upl.Update();

                //inputproduct.Text = "";
                qty.Text = "";
                //inputproduct.Focus();
            }
            else
            {
                // error message
                lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
            }

            save.Visible = cbl_Product.Items.Count > 0 ? true : false;
        }
        catch
        {
            // error message
            lit_Message.Text = "<span style='color:red;'>Stock yang anda pilih tidak ada! Silahkan cek Stock anda</span>";
        }
    }
    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.Souvenir.SingleOrDefault(p => p.Kode == "" + input + "");

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
        
        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            var rsH = d.StockOutgoingSouvenir.SingleOrDefault(p => p.ID == id);
            int vB = rsH.IDBrand.Value;
            int vF = rsH.TipeAsal;

            decimal qty = 0m;
            if (cbl_Product.Items[i].Selected)
            {
                qty = Convert.ToDecimal(Request.Form["qty_" + cbl_Product.Items[i].Value]);

                var rs = d.StockActualSouvenir.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product.Items[i].Value) && p.TipeLokasi == vF && p.IDLokasi == rsH.IDAsal);

                StockOutgoingSouvenir_Product n = new StockOutgoingSouvenir_Product()
                {
                    //Input
                    IDForm = id,
                    IDProduct = Convert.ToInt32(cbl_Product.Items[i].Value),
                    Qty = qty
                    //SubStorage = rs.Substorage

                };
                d.StockOutgoingSouvenir_Product.InsertOnSubmit(n);

                //Commit
                d.SubmitChanges();
            }
        }
        Db.Execute("exec sp_eliminate_duplicate_souvenir " + id.ToString(), Db.Cnn_DB);

        

        //Redirect
        Response.Redirect("Product_Finish.aspx?id=" + id);
        
    }

protected void save_Click1(object sender, EventArgs e)    
    {
        int CounterSuccess = cbl_Product.Items.Count;
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();

            //code awal        
            for (int i = 0; i < cbl_Product.Items.Count; i++)
            {
                // var rsH = d.StockOutgoingSouvenir.SingleOrDefault(p => p.ID == id);
                // int vB = rsH.IDBrand.Value;
                // int vF = rsH.TipeAsal;

                if (cbl_Product.Items[i].Selected)
                {
                    decimal qty = Convert.ToDecimal(Request.Form["qty_" + cbl_Product.Items[i].Value]);

                    string query = @"                                                                                    
                        INSERT INTO StockOutgoingSouvenir_Product (IDForm, IDProduct, Qty)
                        SELECT ID, @IDProduct, @Qty
                        FROM StockOutgoingSouvenir WHERE ID = @id;                            
                    ";

                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.CommandTimeout = 10;
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@Qty", qty);
                            command.Parameters.AddWithValue("@IDProduct", Convert.ToInt32(cbl_Product.Items[i].Value));

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

        Db.Execute("exec sp_eliminate_duplicate_Souvenir " + id.ToString(), Db.Cnn_DB);

        //Redirect
        if (CounterSuccess == cbl_Product.Items.Count)
        {
            Response.Redirect("Product_Finish.aspx?id=" + id);        
        }
        else
        {
            lit_Message.Text = "<span style='color:red;'>No product selected or all products failed to save.</span>";
        }
    }


    /*

    
     * */

    protected void FillSouvenir()
    {

        string getData = @"select b.Kode, b.Nama, a.InHand
                           from StockActualSouvenir a
                           JOIN Souvenir b on b.ID=a.IDProduct
                           CROSS APPLY (SELECT IDAsal,TipeAsal FROM StockOutgoingSouvenir WHERE ID = @id) c
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
                    string t = Convert.ToString(r["Nama"]) + " (" + Convert.ToString(r["Kode"]) + ") (Qty : " + Convert.ToDecimal(r["InHand"]).ToString("N0") + ")";
                    string v = Convert.ToString(r["Kode"]);
                    ListItem item1 = new ListItem(t, v);

                    ddl_ProductCode.Items.Add(item1);
                }
            }
        }
        // var rs = from p in d.Souvenir
        //          where !p.Draft && !p.Disable //&& ((p.Nama.Contains(query)) || p.Kode.Contains(query))

        //          select new { id = p.Kode, label = p.Nama };

        // ddl_ProductCode.Items.Clear();
        // foreach (var r in rs)
        // {
        //     string t = r.label + " (" + r.id + ")";
        //     string v = r.id;
        //     ListItem item = new ListItem(t, v);

        //     ddl_ProductCode.Items.Add(item);
        // }
    }

    private string ProductList_Souvenir(string KodeLabel, string qty, string id)
    {
        string x;
        x = "<div class=\"form-inline\">"
            + "<div style='padding:5px;'>"
            + "<div style='border-bottom:solid 1px black;'>" + KodeLabel + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Qty : <input size=\"6\" class=\"span2\" type=\"text\" name=\"qty_" + id + "\" value=\"" + qty + "\" /></div>"
            + "</div>"
            + "</div>"
            ;
        return x;
    }

}