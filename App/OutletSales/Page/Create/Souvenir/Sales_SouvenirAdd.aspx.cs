using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_ItemAdd : System.Web.UI.Page
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
    App gp = new App();
    protected int myID { get { return App.QueryInt(this); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            kw.Focus(); //Keyword

            // DLC.POSListItem(myID, ddl_item);
            //Form
            fillForm();

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("Sales.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/Sales_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            //Cancel-Link
            cancel.HRef += "?id=" + myID;
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;
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

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        fillSouvenir();
        fillSouvenirDJ();
        fillSouvenirGJ();
        fillSouvenirPG();
        fillSouvenirLD();
    }
    protected void fillSouvenir()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);

        if (d != null)
        {
            var rs = from p in db.StockActualSouvenir
                     where p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.InHand > 0
						&& p.Souvenir.Draft == false && p.Souvenir.Disable == false
						&& !db.SalesOrderSouvenir.Any(so => so.IDForm == myID && so.IDProduct == p.IDProduct)
                     select p;

            if (kw.Text != "")
            {
                rs = from p in rs
                     where p.Souvenir.Nama.Contains(kw.Text)
                     || p.Souvenir.Kode.Contains(kw.Text)
                     select p;
            }

            int index = 1;

            //if (rs.Count() == 1)
            //{
            //    Response.Redirect("Sales_SouvenirAddSave.aspx?id=" + myID + "&item=1&iditem=" + rs.First().IDProduct);
            //}
            //else
            //{
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r.IDProduct, 0);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Souvenir.Kode;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Souvenir.Nama;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
                // }
            }
        }
    }

    protected void fillSouvenirDJ()
	{
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
		if (d != null)
		{
			string error_blacklist = validasi_BlackList(kw.Text.Trim());

			if (string.IsNullOrEmpty(error_blacklist))
			{
				/*var rs = from p in db.Bundle_ItemPasangan
						 join q in db.StockProductDJ on p.IDProduct equals q.ID
						 where p.TipeProduct == 1 && p.BundleItem.Draft == false && p.BundleItem.ApprovalNama != null
						 && p.BundleItem.StatusActive == 1
						 && q.StatusPenjualan == 0 && q.ReturnProduct == false 
					&& q.StockActualDJ.TipeLokasi == d.TipeLokasi 
					&&
					 q.StockActualDJ.IDLokasi == d.IDLokasi  
					 && q.StockActualDJ.Substorage == 1 
					 //&& !(from z in db.Bundle_ItemMaster
					 //     select z.IDForm).Contains(p.IDForm)
					 select new { p, q };*/

				//rs = from p in rs
				//     where !(from z in db.Bundle_ItemMaster
				//             select z.IDForm).Contains(p.IDForm)
				//     select p;

				string sql = @"
			SELECT
				p.ID,
				p.IDForm,
				p.IDProduct,
				p.TipeProduct,
				q.ID AS IDStockProduct,
				q.Nomor,
				cpi.Nama [Item]
			FROM Bundle_ItemPasangan p
			INNER JOIN BundleItem bi
				ON p.IDForm = bi.ID
			INNER JOIN StockProductDJ q
				ON p.IDProduct = q.ID
			INNER JOIN StockActualDJ sa
				ON q.ID = sa.IDProduct
			INNER JOIN StockProductDJ_CharProduct cp
				ON q.ID = cp.ID
			INNER JOIN CharProductItem cpi
				ON cp.ProductItem = cpi.ID
			WHERE p.TipeProduct = 1
				AND bi.Draft = 0
				AND bi.ApprovalNama IS NOT NULL
				AND bi.StatusActive = 1
				AND q.StatusPenjualan = 0
				AND q.ReturnProduct = 0
				AND sa.TipeLokasi = @TipeLokasi
				AND sa.IDLokasi = @IDLokasi
				AND sa.Substorage = 1
				AND NOT EXISTS
				(
					SELECT 1
					FROM StockProductDJ b
					WHERE b.Nomor = q.Nomor
					  AND b.StatusPenjualan = 2
				)
				AND NOT EXISTS
				(
					SELECT 1
					FROM jaws_integration.dbo.StockProductBlackList b
					WHERE b.PLU = q.Nomor
					  AND b.STATUS = 1
					  AND b.PRODUCT_TYPE = 'DJ'
				)";

				List<SqlParameter> parameters = new List<SqlParameter>();
				if (kw.Text != "")
				{
					sql += " AND q.Nomor LIKE '%' + @Keyword + '%'";

					parameters.Add(new SqlParameter("@TipeLokasi", d.TipeLokasi));
					parameters.Add(new SqlParameter("@IDLokasi", d.IDLokasi));
					parameters.Add(new SqlParameter("@Keyword", kw.Text));
				}
				else
				{
					parameters.Add(new SqlParameter("@TipeLokasi", d.TipeLokasi));
					parameters.Add(new SqlParameter("@IDLokasi", d.IDLokasi));
				}

				DataTable dt = Rs(sql, Db.Cnn_DB, parameters, CommandType.Text);


				/*if (kw.Text != "")
					rs = rs.Where(q => q.q.Nomor.Contains(kw.Text));
				*/
				int index = 1;

				//if (rs.Count() == 1)
				//{
				//    Response.Redirect("Sales_SouvenirAddSave.aspx?id=" + myID + "&item=1&iditem=" + rs.First().p.ID);
				//}
				//else
				//{
				foreach (DataRow r in dt.Rows)
				{
					TableRow tr = new TableRow();
					TableCell td;

					td = new TableCell();
					td.Text = TD_HyperLink(Convert.ToInt32(r["IDStockProduct"]), 1);
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = "<p>" + index.ToString() + ".</p>";
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = r["Nomor"].ToString();
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = r["Item"].ToString();
					tr.Cells.Add(td);

					tb.Rows.Add(tr);

					index++;
					//}
				}
				lit_Message.InnerText = "";
			}
			else
			{
				lit_Message.InnerText = "PLU Tidak dapat Dijual/Trade In. Silakan lakukan pemesanan melalui Pre Order (PO)";
			}
		}
	}

    protected void fillSouvenirGJ()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);

        if (d != null)
        {
            var rs = from p in db.Bundle_ItemPasangan
                     join q in db.StockProductGJ on p.IDProduct equals q.ID
                     where p.TipeProduct == 2 && p.BundleItem.Draft == false && p.BundleItem.ApprovalNama != null
                     && p.BundleItem.StatusActive == 1
                     && q.StatusPenjualan == 0 && q.ReturnProduct == false &&
                     q.StockActualGJ.IDLokasi == d.IDLokasi && q.StockActualGJ.TipeLokasi == d.TipeLokasi && q.StockActualGJ.Substorage == 1
                     //&& !(from z in db.Bundle_ItemMaster
                     //     select z.IDForm).Contains(p.IDForm)
                     select new { p, q };

            //rs = from p in rs
            //     where !(from z in db.Bundle_ItemMaster
            //             select z.IDForm).Contains(p.IDForm)
            //     select p;

            if (kw.Text != "")
                rs = rs.Where(q => q.q.Nomor.Contains(kw.Text));

            int index = 1;

            //if (rs.Count() == 1)
            //{
            //    Response.Redirect("Sales_ItemAddSave.aspx?id=" + myID + "&item=3&iditem=" + rs.First().p.ID);
            //}
            //else
            //{
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r.q.ID, 2);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.q.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.q.StockProductGJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
                // }
            }
        }
    }

    protected void fillSouvenirPG()
	{
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);

		if (d != null)
		{
			string error_blacklist = validasi_BlackList(kw.Text.Trim());
			if (string.IsNullOrEmpty(error_blacklist))
			{
				/*var rs = from p in db.Bundle_ItemPasangan
						 join q in db.StockProductPG on p.IDProduct equals q.ID
						 where p.TipeProduct == 3 && p.BundleItem.Draft == false && p.BundleItem.ApprovalNama != null
						 && p.BundleItem.StatusActive == 1
						 && q.StatusPenjualan == 0 && q.ReturnProduct == false &&
						 q.StockActualPG.IDLokasi == d.IDLokasi && q.StockActualPG.TipeLokasi == d.TipeLokasi && q.StockActualPG.Substorage == 1
						 //&& !(from z in db.Bundle_ItemMaster
						 //     select z.IDForm).Contains(p.IDForm)
						 select new { p, q };*/

				//rs = from p in rs
				//     where !(from z in db.Bundle_ItemMaster
				//             select z.IDForm).Contains(p.IDForm)
				//     select p;

				string sql = @"
			SELECT
				p.ID,
				p.IDForm,
				p.IDProduct,
				p.TipeProduct,
				q.ID AS IDStockProduct,
				q.Nomor,
				cpi.Nama
			FROM Bundle_ItemPasangan p
			INNER JOIN BundleItem bi
				ON p.IDForm = bi.ID
			INNER JOIN StockProductPG q
				ON p.IDProduct = q.ID
			INNER JOIN StockActualPG sa
				ON q.ID = sa.IDProduct
			INNER JOIN CharProductItem cpi
				ON q.ProductItem = cpi.ID
			WHERE p.TipeProduct = 3
				AND bi.Draft = 0
				AND bi.ApprovalNama IS NOT NULL
				AND bi.StatusActive = 1
				AND q.StatusPenjualan = 0
				AND q.ReturnProduct = 0
				AND sa.IDLokasi = @IDLokasi
				AND sa.TipeLokasi = @TipeLokasi
				AND sa.Substorage = 1
				AND NOT EXISTS
				(
					SELECT 1
					FROM jaws_integration.dbo.StockProductBlackList b
					WHERE b.PLU = q.Nomor
					  AND b.Status = 1
				)
			";

				List<SqlParameter> parameters = new List<SqlParameter>();
				if (kw.Text != "")
				{
					sql += " AND q.Nomor LIKE '%' + @Keyword + '%'";

					parameters.Add(new SqlParameter("@TipeLokasi", d.TipeLokasi));
					parameters.Add(new SqlParameter("@IDLokasi", d.IDLokasi));
					parameters.Add(new SqlParameter("@Keyword", kw.Text));
				}
				else
				{
					parameters.Add(new SqlParameter("@TipeLokasi", d.TipeLokasi));
					parameters.Add(new SqlParameter("@IDLokasi", d.IDLokasi));
				}

				DataTable dt = Rs(sql, Db.Cnn_DB, parameters, CommandType.Text);

				/*if (kw.Text != "")
					rs = rs.Where(q => q.q.Nomor.Contains(kw.Text));*/

				int index = 1;

				//if (rs.Count() == 1)
				//{
				//    Response.Redirect("Sales_ItemAddSave.aspx?id=" + myID + "&item=3&iditem=" + rs.First().p.ID);
				//}
				//else
				//{
				foreach (DataRow r in dt.Rows)
				{
					TableRow tr = new TableRow();
					TableCell td;

					td = new TableCell();
					td.Text = TD_HyperLink((int)r["IDStockProduct"], 3);
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = "<p>" + index.ToString() + ".</p>";
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = (string)r["Nomor"];
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = (string)r["Nama"];
					tr.Cells.Add(td);

					tb.Rows.Add(tr);

					index++;
					// }
				}
				lit_Message.InnerText = "";
			}
			else
			{
				lit_Message.InnerText = "PLU Tidak dapat Dijual/Trade In. Silakan lakukan pemesanan melalui Pre Order (PO)";
			}
		}
	}

    protected void fillSouvenirLD()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d != null)
        {
            var rs = from p in db.Bundle_ItemPasangan
                     join q in db.StockProductLD_Stone1B on p.IDProduct equals q.ID
                     where p.TipeProduct == 4 && p.BundleItem.Draft == false && p.BundleItem.ApprovalNama != null
                     && p.BundleItem.StatusActive == 1
                     && q.StatusPenjualan == 0 && q.ReturnProduct == false &&
                     q.StockActualLD_Stone1B.IDLokasi == d.IDLokasi && q.StockActualLD_Stone1B.TipeLokasi == d.TipeLokasi && q.StockActualLD_Stone1B.Substorage == 1
                     //&& !(from z in db.Bundle_ItemMaster
                     //     select z.IDForm).Contains(p.IDForm)
                     select new { p, q };

            //rs = from p in rs
            //     where !(from z in db.Bundle_ItemMaster
            //             select z.IDForm).Contains(p.IDForm)
            //     select p;

            if (kw.Text != "")
                rs = rs.Where(q => q.q.Nomor.Contains(kw.Text));

            int index = 1;

            //if (rs.Count() == 1)
            //{
            //    Response.Redirect("Sales_ItemAddSave.aspx?id=" + myID + "&item=4&iditem=" + rs.First().p.ID);
            //}
            //else
            //{
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r.q.ID, 4);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.q.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "-";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
                // }
            }
        }
    }

    //Display Column
    protected string TD_HyperLink(int ID, int itemindex)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "Sales_SouvenirAddSave.aspx";
        href += "?id=" + myID;
        href += "&item=" + itemindex;
        href += "&iditem=" + ID;


        x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
	
	protected string validasi_BlackList(string nomor)
	{
		string query = "DECLARE @ERROR VARCHAR(100) = ''; " +
			"IF EXISTS (SELECT 1 FROM StockProductBlackList WHERE PLU = @PLU AND STATUS = 1) " +
			"BEGIN " +
			"   SET @ERROR = 'This product is blacklisted.'; " +
			"END " +
			"SELECT @ERROR AS ErrorMessage;";
		List<SqlParameter> parameters = new List<SqlParameter>
		{
			new SqlParameter("@PLU", SqlDbType.VarChar) { Value = nomor }
		};
		DataTable dt = Rs(query, Db.Cnn_IN, parameters, CommandType.Text);
		return dt.Rows[0][0].ToString();
	}
	
	protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
	{
		DataTable rs = new DataTable();
		using (SqlConnection sqlCnn = new SqlConnection(CnnString))
		{
			sqlCnn.Open();
			using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
			{
				cmd.CommandType = cmdType;
				if (param != null)
				{
					foreach (var x in param)
					{
						cmd.Parameters.Add(x);
					}
				}
				using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
				{
					sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
					sqlAdapter.Fill(rs);
				}
			}
		}
		return rs;
	}
}