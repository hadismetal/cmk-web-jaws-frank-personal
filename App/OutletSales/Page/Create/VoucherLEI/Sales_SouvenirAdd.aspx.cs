using System;
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

            //DLC.POSListItem(myID, ddl_item);

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
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/Sales_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

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
        //fillSouvenirDJ();
        //fillSouvenirGJ();
        //fillSouvenirLD();
        //fillSouvenirPG();
    }
    protected void fillSouvenir()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);

        if (d != null)
        {
            var rs = from p in db.StockActualSouvenir
                     where p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.InHand > 0
                        && p.Souvenir.Draft == false && p.Souvenir.Disable == false
                        && !(from z in db.SalesOrderSouvenir
                          where z.IDForm == myID
                             && z.IDProduct != null
                             && z.IDProductDJ == null
                             && z.IDProductPG == null
                             && z.IDProductGJ == null
                             && z.IDProductLD == null
                          select z.IDProduct).Contains(p.IDProduct)
                     select p;

            var so = from p in db.Bundle_ItemPasangan

                     select p;

            if (kw.Text != "")
            {
                rs = from p in rs
                     where p.Souvenir.Nama.Contains(kw.Text)
                     || p.Souvenir.Kode.Contains(kw.Text)
                     select p;
            }

            int index = 1;

            int itemindex = 1;
            int PLU = 0;
            //string[] data = (ddl_item.SelectedValue).Split(',');
            //if (data[1] != "")
            //{
            //    itemindex = Convert.ToInt32(data[0]);
            //    PLU = Convert.ToInt32(data[1]);
            //}

            //if (rs.Count() == 1)
            //{
            //    Response.Redirect("Sales_SouvenirAddSave.aspx?id=" + myID + "&item=1&iditem=" + rs.First().IDProduct + "&plu=" + PLU);
            //}
            //else
            //{

                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.IDProduct, 0, PLU, itemindex);
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

                    td = new TableCell();
                    td.Text = r.InHand.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    index++;
                }
           // }
        }
    }

    protected void fillSouvenirDJ()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);

        var item = from p in db.SalesOrderDJ
                   where p.IDForm == myID
                   select p;

        int i = 0;
        int[] it = new int[100];

        foreach (var r in item)
        {
            it[i] = r.IDProduct;
            i++;
        }
        
        if (d != null)
        {
            //var rs = from p in db.Bundle_ItemPasangan
            //         join q in db.StockProductDJ on p.IDProduct equals q.ID
            //         join r in db.Bundle_ItemMaster on p.IDForm equals r.IDForm
            //         where r.TipeProduct == 1 && p.BundleItem.Draft == false
            //         && p.BundleItem.StatusActive == 1
            //              && q.StatusPenjualan == 0 && q.ReturnProduct == false
            //              && q.StockActualDJ.TipeLokasi == d.TipeLokasi
            //              && q.StockActualDJ.IDLokasi == d.IDLokasi
            //              && q.StockActualDJ.Substorage == 1
            //              && it.Contains(r.IDProduct.Value)
            //         && (from z in db.Bundle_ItemMaster
            //             select z.IDForm).Contains(p.IDForm)

            //         select new { p, q };

            var rs = from p in db.Bundle_ItemPasangan
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
                     select new { p, q };

            if (kw.Text != "")
                rs = rs.Where(q => q.q.Nomor.Contains(kw.Text));

            int index = 1;

            int itemindex = 1;
            int PLU = 0;
            //string[] data = (ddl_item.SelectedValue).Split(',');
            //if (data[1] != "")
            //{
            //    itemindex = Convert.ToInt32(data[0]);
            //    PLU = Convert.ToInt32(data[1]);
            //}
            //rs = from p in rs
            //     where (from r in db.Bundle_ItemMaster select r.IDProduct).Contains(PLU)
            //     select p;


            //if (rs.Count() == 1)
            //{
            //    Response.Redirect("Sales_SouvenirAddSave.aspx?id=" + myID + "&item=1&iditem=" + rs.First().p.ID);
            //}
            //else
            //{
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r.q.ID, 1, PLU,itemindex);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.q.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.q.StockProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
                //}
            }
        }
    }

    protected void fillSouvenirGJ()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);

        var item = from p in db.SalesOrderGJ
                   where p.IDForm == myID
                   select p;

        int i = 0;
        int[] it = new int[100];

        foreach (var r in item)
        {
            it[i] = r.IDProduct;
            i++;
        }
        
        if (d != null)
        {
            /*var rs = from p in db.Bundle_ItemMaster
                     where it.Contains(p.IDProduct.Value)
                     select p;*/

            //var rs = from p in db.Bundle_ItemPasangan
            //         join q in db.StockProductGJ on p.IDProduct equals q.ID
            //         join s in db.Bundle_ItemMaster on p.IDForm equals s.IDForm
            //         where s.TipeProduct == 2 && p.BundleItem.Draft == false
            //         && p.BundleItem.StatusActive == 1
            //         && q.StatusPenjualan == 0 && q.ReturnProduct == false
            //         && q.StockActualGJ.IDLokasi == d.IDLokasi && q.StockActualGJ.TipeLokasi == d.TipeLokasi && q.StockActualGJ.Substorage == 1
            //         && it.Contains(s.IDProduct.Value)
            //         && (from z in db.Bundle_ItemMaster
            //              select z.IDForm).Contains(p.IDForm)
            //         select new { p, q };

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
            int itemindex = 1;
            int PLU = 0;
            //string[] data = (ddl_item.SelectedValue).Split(',');
            //if (data[1] != "")
            //{
            //    itemindex = Convert.ToInt32(data[0]);
            //    PLU = Convert.ToInt32(data[1]);
            //}

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
                td.Text = TD_HyperLink(r.q.ID, 2, PLU,itemindex);
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

        var item = from p in db.SalesOrderPG
                   where p.IDForm == myID
                   select p;

        int i = 0;
        int[] it = new int[100];

        foreach (var r in item)
        {
            it[i] = r.IDProduct;
            i++;
        }

        if (d != null)
        {
            //var rs = from p in db.Bundle_ItemPasangan
            //         join q in db.StockProductPG on p.IDProduct equals q.ID
            //         join r in db.Bundle_ItemMaster on p.IDForm equals r.IDForm
            //         where r.TipeProduct == 3 && p.BundleItem.Draft == false
            //         && p.BundleItem.StatusActive == 1
            //         && q.StatusPenjualan == 0 && q.ReturnProduct == false &&
            //         q.StockActualPG.IDLokasi == d.IDLokasi && q.StockActualPG.TipeLokasi == d.TipeLokasi && q.StockActualPG.Substorage == 1
            //         && it.Contains(r.IDProduct.Value)
            //         && (from z in db.Bundle_ItemMaster
            //              select z.IDForm).Contains(p.IDForm)
            //         select new { p, q };

            var rs = from p in db.Bundle_ItemPasangan
                     join q in db.StockProductPG on p.IDProduct equals q.ID
                     where p.TipeProduct == 3 && p.BundleItem.Draft == false && p.BundleItem.ApprovalNama != null
                     && p.BundleItem.StatusActive == 1
                     && q.StatusPenjualan == 0 && q.ReturnProduct == false &&
                     q.StockActualPG.IDLokasi == d.IDLokasi && q.StockActualPG.TipeLokasi == d.TipeLokasi && q.StockActualPG.Substorage == 1
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
            int itemindex = 1;
            int PLU = 0;
            //string[] data = (ddl_item.SelectedValue).Split(',');
            //if (data[1] != "")
            //{
            //    itemindex = Convert.ToInt32(data[0]);
            //    PLU = Convert.ToInt32(data[1]);
            //}

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
                td.Text = TD_HyperLink(r.q.ID, 3, PLU,itemindex);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.q.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.q.CharProductItem.Nama;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
                // }
            }
        }
    }

    protected void fillSouvenirLD()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);

        var item = from p in db.SalesOrderPG
                   where p.IDForm == myID
                   select p;

        int i = 0;
        int[] it = new int[100];

        foreach (var r in item)
        {
            it[i] = r.IDProduct;
            i++;
        }

        if (d != null)
        {
            //var rs = from p in db.Bundle_ItemPasangan
            //         join q in db.StockProductLD_Stone1B on p.IDProduct equals q.ID
            //         join r in db.Bundle_ItemMaster on p.IDForm equals r.IDForm
            //         where r.TipeProduct == 4 && p.BundleItem.Draft == false
            //         && p.BundleItem.StatusActive == 1
            //         && q.StatusPenjualan == 0 && q.ReturnProduct == false &&
            //         q.StockActualLD_Stone1B.IDLokasi == d.IDLokasi && q.StockActualLD_Stone1B.TipeLokasi == d.TipeLokasi && q.StockActualLD_Stone1B.Substorage == 1
            //         && it.Contains(r.IDProduct.Value)
            //         && (from z in db.Bundle_ItemMaster
            //              select z.IDForm).Contains(p.IDForm)
            //         select new { p, q };

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
            int itemindex = 1;
            int PLU = 0;
            //string[] data = (ddl_item.SelectedValue).Split(',');
            //if (data[1] != "")
            //{
            //    itemindex = Convert.ToInt32(data[0]);
            //    PLU = Convert.ToInt32(data[1]);
            //}

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
                td.Text = TD_HyperLink(r.q.ID, 3, PLU,itemindex);
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
    protected string TD_HyperLink(int ID, int itemindex, int PLU, int itemindexSouvenir)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "Sales_SouvenirAddSave.aspx";
        href += "?id=" + myID;
        href += "&item=" + itemindex;
        href += "&itemsouvenir=" + itemindexSouvenir;
        href += "&iditem=" + ID;
        href += "&plu=" + PLU;

        x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
}