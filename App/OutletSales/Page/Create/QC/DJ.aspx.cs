using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Page_Approval_DJ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        string query = @"SELECT TOP 20 A.ID, A.Nomor,
            A4.Nama [Item], A5.NamaKode [Kategory], A6.NamaKode [Level], 
            ISNULL(A.ImgPicture,'') ImgPicture, ISNULL(ProtoNomor,'') ProtoNomor, 
            ISNULL(ProductOrderNomor,'') ProductOrderNomor, ISNULL(SupplierNomor,'') SupplierNomor
            FROM StockProductDJ A
            JOIN StockProductDJ_CharProduct A3 ON A.ID = A3.ID
            JOIN CharProductItem A4 ON A3.ProductItem = A4.ID
            JOIN CharProductCategory A5 ON A3.Productcategory = A5.ID
            JOIN CharProductLevel A6 ON A3.ProductLevel = A6.ID
            JOIN SalesOrderDJ B ON A.ID = B.IDProduct
            JOIN SalesOrder C ON B.IDForm = C.ID
            WHERE A.Draft = 0 AND C.StatusPembayaran = 1 AND C.StatusVoid = 0
            AND A.Nomor IS NOT NULL
            AND A.Nomor <> ''
            AND A.Nomor LIKE '%' + @Keyword + '%'";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@Keyword", SqlDbType.VarChar){ Value = kw.Text.Trim() }
        };
        DataTable rs = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);

        //RESULT
        int index = 1;
        if (rs.Rows.Count > 0)
        {
            foreach (DataRow r in rs.Rows)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(Convert.ToInt32(r["ID"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Image(r["ImgPicture"].ToString());
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nomor(r["Nomor"].ToString());
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Product01(r["Item"].ToString());
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Product02(r["Kategory"].ToString());
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Product03(r["Level"].ToString());
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }
    protected void Sub_Header(string WarehouseNama)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += WarehouseNama;

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected string TD_HyperLink(int ID)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"DJ_Submit.aspx?id=" + ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }
    protected string TD_Nomor(string Nomor)
    {
        string x = string.Empty;

        x += "<p class=\"text-md text-info\">" + Nomor + "</p>";

        return x;
    }
    protected string TD_Data(string ProtoNomor, string ProductOrderNomor, string SupplierNomor)
    {
        string x = string.Empty;

        if (ProtoNomor != "" && ProtoNomor != null)
            x += "<p class=\"text-sm\">" + ProtoNomor + "</p>";

        if (ProductOrderNomor != "" && ProductOrderNomor != null)
            x += "<p class=\"text-sm\">" + ProductOrderNomor + "</p>";

        if (SupplierNomor != "" && SupplierNomor != null)
            x += "<p class=\"text-sm\">" + SupplierNomor + "</p>";

        return x;
    }
    protected string TD_Product01(string Item)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += Item;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(string Kategori)
    {
        string x = string.Empty;
        x += Kategori;
        return x;
    }
    protected string TD_Product03(string Level)
    {
        string x = string.Empty;
        x += Level;

        return x;
    }
    protected string TD_Image(string ImgPicture)
    {
        string urlImage = ConfigurationManager.AppSettings["ImageAPI"];
        string x = string.Empty;

        x += "<img src=" + urlImage + ImgPicture + " width=\"100\" />";

        return x;
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
        }
    }

    protected static DataTable Rs(string query, string connection, List<SqlParameter> parameters, CommandType cmdtype)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connection))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = cmdtype;
                if (parameters != null)
                {
                    foreach (SqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                {
                    dt.Load(reader);
                }
            }
        }
        return dt;
    }
}