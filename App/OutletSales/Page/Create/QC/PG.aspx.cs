using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Page_Approval_PG : System.Web.UI.Page
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
            B.Nama [Item], C.Nama [Color], D.Nama [Level], 
            ISNULL(A.ImgPicture,'') ImgPicture 
            FROM StockProductPG A
            JOIN CharProductItem B ON A.ProductItem = B.ID
            JOIN CharFrameColor C ON A.FrameColor = C.ID
            JOIN CharGoldLevel D ON A.ProductLevel = D.ID
            JOIN SalesOrderPG E ON A.ID = E.IDProduct
            JOIN SalesOrder F ON E.IDForm = F.ID
            WHERE A.Draft = 0
            AND A.Nomor IS NOT NULL
            AND A.Nomor <> ''
            AND A.Nomor LIKE '%' + @Keyword + '%'
            ";
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
                td.Text = TD_Product02(r["Color"].ToString());
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

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"PG_Submit.aspx?id=" + ID + "\">";
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
    protected string TD_Product01(string Item)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += Item;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(string FrameColor)
    {
        string x = string.Empty;
        x += FrameColor;
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