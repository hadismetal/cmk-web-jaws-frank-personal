using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_ItemAdd : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this,"tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

            //DLC.POSListItem(myID, ddl_item);

			//Form
			fillForm();

			//Generate
			fillFilter();
            fillTable(sender, e);
			//if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Sales.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
            
			//Cancel-Link
			cancel.HRef += "?id=" + myID;
		}
	}



    //protected void changePLU(object sender, EventArgs e)
    //{

    //    int itemindex = 1;
    //    int PLU = 0;
    //    string[] data = (ddl_item.SelectedValue).Split(',');
    //    if (data[1] != "")
    //    {
    //        itemindex = Convert.ToInt32(data[0]);
    //        PLU = Convert.ToInt32(data[1]);
    //    }

    //    string href = App.RefreshURL(this);

    //    //Page-Qstr
    //    href += "&id=" + myID;
    //    href += "&pitem=" + ddl_item.SelectedValue;
    //    href += "&kw=" + kw.Text;

    //    Response.Redirect(href);
    //}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;

		href += "&kw=" + kw.Text;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			
			kw.Text = Request.QueryString["kw"];
            //ddl_item.SelectedValue = Request.QueryString["pitem"];
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
        //fillPackaging();
        fillPackagingChk();
	}

    protected void fillPackagingChk()
    {
        var d = db.SalesOrder.SingleOrDefault(p=>p.ID == myID);

        if (d != null)
        {
            var rs = from p in db.StockActualPackaging
                     where p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.InHand > 0
                        && !(from z in db.SalesOrderPackaging
                             where z.IDForm == myID
                                && z.IDProduct != null
                                && z.IDProductDJ == null
                                && z.IDProductPG == null
                                && z.IDProductGJ == null
                                && z.IDProductLD == null
                             select z.IDProduct).Contains(p.IDProduct)
                        && !p.Packaging.Disable
                        && !p.Packaging.Draft
                     select p;

            if (kw.Text != "")
            {
                rs = from p in rs
                     where p.Packaging.Nama.Contains(kw.Text)
                     || p.Packaging.Kode.Contains(kw.Text)
                     select p;
            }

            rs = rs.OrderBy(p => p.Packaging.Nama);

            // fill chk
            if (rs.Count() == 1)
            {
                string v = rs.First().Packaging.ID.ToString();
                string t = PackagingCheckBoxList(rs.First());
                cbl_packaging.Items.Add(new ListItem(t, v));
                cbl_packaging.Items[cbl_packaging.Items.Count - 1].Selected = true;
            }
            else
            {
                foreach (var r in rs)
                {
                    string v = r.Packaging.ID.ToString();
                    string t = PackagingCheckBoxList(r);
                    cbl_packaging.Items.Add(new ListItem(t, v));
                }
            }

        }
    }
    public static string PackagingCheckBoxList(StockActualPackaging sap)
    {
        string x;
        //x = "<div style='width: 300px;'>"
        //    + "<div style='padding:5px; display: table-cell;'>"
        //    + "<div style='float: left; width: 150px;'>" + sap.Packaging.Kode + "</div>"
        //    + "<div style='float: left; width: 150px;'>" + sap.Packaging.Nama + "<div style='clear:both;'></div></div>"
        //    + "</div>"
        //    + "</div>"
        //    + "<div style='clear: both; font-size:1px; height: 1px;'>&nbsp;</div>"
        //    ;

        x = sap.Packaging.Nama + " - " + sap.Packaging.Kode + " - " + " (" + Str.Number0(sap.InHand) + ")";
        return x;
    }
	protected void fillPackaging() 
    {
        //var d = db.SalesOrder.SingleOrDefault(p=>p.ID == myID);

        //if(d!=null)
        //{
        //    var rs = from p in db.StockActualPackaging
        //             where p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.InHand > 0
        //                && !(from z in db.SalesOrderPackaging 
        //                     where z.IDForm == myID 
        //                        && z.IDProduct != null 
        //                        && z.IDProductDJ == null
        //                        && z.IDProductPG == null
        //                        && z.IDProductGJ == null
        //                        && z.IDProductLD == null
        //                     select z.IDProduct).Contains(p.IDProduct)
        //             select p;
                
        //    if (kw.Text != "")
        //    {
        //        rs = from p in rs
        //             where p.Packaging.Nama.Contains(kw.Text)
        //             || p.Packaging.Kode.Contains(kw.Text)
        //             select p;
        //    }

        //    int index = 1;

        //    int itemindex = 1;
        //    int PLU = 0;
        //    //string[] data = (ddl_item.SelectedValue).Split(',');
        //    //if (data[1] != "")
        //    //{
        //    //    itemindex = Convert.ToInt32(data[0]);
        //    //    PLU = Convert.ToInt32(data[1]);
        //    //}

        //    if (rs.Count() == 1)
        //    {
        //        Response.Redirect("Sales_PackagingAddSave.aspx?id=" + myID + "&item="+itemindex+"&iditem=" + rs.First().IDProduct + "&plu=" + PLU);
        //    }
        //    else
        //    {
                

        //        foreach (var r in rs)
        //        {
        //            TableRow tr = new TableRow();
        //            TableCell td;

        //            td = new TableCell();
        //            td.Text = TD_HyperLink(r.IDProduct, itemindex, PLU);
        //            tr.Cells.Add(td);

        //            td = new TableCell();
        //            td.Text = "<p>" + index.ToString() + ".</p>";
        //            tr.Cells.Add(td);

        //            td = new TableCell();
        //            td.Text = r.Packaging.Kode;
        //            tr.Cells.Add(td);

        //            td = new TableCell();
        //            td.Text = r.Packaging.Nama;
        //            tr.Cells.Add(td);
                    
        //            td = new TableCell();
        //            td.Text = r.InHand.ToString("N0");
        //            tr.Cells.Add(td);

        //            tb.Rows.Add(tr);

        //            index++;
        //        }
        //    }
        //}
	}

	//Display Column
	protected string TD_HyperLink(int ID, int itemindex, int PLU) {
		string x = string.Empty;
		string href = string.Empty;

        itemindex = 1;
        PLU = 0;
        //string[] data = (ddl_item.SelectedValue).Split(',');
        //if (data[1] != "")
        //{
        //    itemindex = Convert.ToInt32(data[0]);
        //    PLU = Convert.ToInt32(data[1]);
        //}

		href += "Sales_PackagingAddSave.aspx";
		href += "?id=" + myID;
		href += "&item=" + itemindex;
		href += "&iditem=" + ID;
        href += "&plu=" + PLU;

		x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  ";
		x += "</a>";

		return x;
	}

    protected void save_Click(object sender, EventArgs e)
    {
        CheckBoxList cbl = cbl_packaging;

        foreach (ListItem item in cbl.Items)
        {
            if (item.Selected)
            {
                int id_Product = Convert.ToInt32(item.Value);

                var stock = from p in db.StockReceivePackaging_Product
                            where p.IDProduct == id_Product
                            orderby p.StockReceivePackaging.OperatorTgl
                            select p;
                decimal Modal = 0m;
                try
                {
                    Modal = stock.First().Harga.Value;
                }
                catch (Exception ex) { }

                SalesOrderPackaging n = new SalesOrderPackaging();

                n.IDForm = myID;
                n.IDProduct = id_Product;
                n.ModalRupiah = Modal;
                n.TotalRupiah = 0m;
                n.Qty = 1m;

                db.SalesOrderPackaging.InsertOnSubmit(n);
                db.SubmitChanges();

                n.Invoice = Document.Generate_Nomor_SalesOrderPackaging(n);
                db.SubmitChanges();
            }
        }

        db.SubmitChanges();

        //Redirect
        Response.Redirect("Sales_Data.aspx?id=" + myID + "#div_packaging");

    }


}