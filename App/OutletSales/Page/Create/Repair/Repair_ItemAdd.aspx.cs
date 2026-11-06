using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_DataAgent : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            kw.Focus(); //Keyword

            //DLC.CompanyBrand(brand);

            //Form
            fillForm();

            //Generate
            fillFilter();
            //if (App.Refresh(this)) 

            //Cancel Link
            var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
            cancel.HRef += "?idlokasi=" + d.IDLokasi + "&tipe=" + d.TipeLokasi;

        }
        fillTable(sender, e);
    }

    //Exception
    protected void DataError() { Response.Redirect("Repair.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
        if (d == null) DataError();
        else
        {
            //Header
            
            //Hidden-ID
            //idbrand.Text = d.DataRepair.IDBrand.ToString();
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);

        if (!String.IsNullOrEmpty(inputoldcust.Text))
        {
            var dz = db.DataCustomer.FirstOrDefault(p => p.NoCustomer == inputoldcust.Text);
            if (dz != null)
            {
                Db.Execute("Exec sp_insert_kode_customer_lama '" + d.ID.ToString() + "','" + inputoldcust.Text + "','RP' ", Db.Cnn_DB);
            }
        }

        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        //href += "&brand=" + brand.SelectedValue;
		
		//OldCust
		href += "&coc=" + inputoldcust.Text;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            //brand.SelectedValue = Request.QueryString["brand"];
			inputoldcust.Text = Request.QueryString["coc"];
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        //var ua = db.DataCustomer.Count(p => p.NoCustomer == inputoldcust.Text);
        var oldcodecust = Db.SingleInteger("Exec sp_select_kode_customer_lama '" + myID + "','RP' ", Db.Cnn_DB);
		int idcustcheck = 0;
		if(oldcodecust != -1)
			idcustcheck = db.DataCustomer.FirstOrDefault(a => a.NoCustomer == inputoldcust.Text).ID;

        if (d != null)
        {
            // barang jaws : berada di lokasi toko tersebut dan sudah pernah terjual atas nama customer yang sama
            // barang non-jaws : titipan di toko tersebut dengan JPJR dan customer yang sama
            var rs = (from p in db.StockProductDJ
                      where p.Draft == false && p.ReturnProduct == false
                      && (from z in db.StockActualDJ
                          select z.IDProduct).Contains(p.ID)
                      && (from z in db.SalesOrderDJ
                          where (oldcodecust != -1) ? (z.SalesOrder.IDCustomer == d.IDCustomer || z.SalesOrder.IDCustomer == idcustcheck) :
                          z.SalesOrder.IDCustomer == d.IDCustomer
                          select z.IDProduct).Contains(p.ID)
                      select new
                      {
                          Nomor = p.Nomor,
                          ID = p.ID,
                          HargaRupiahRound = p.StockProductDJ_PricingMU.HargaRupiahRound.Value,
                          ProductItem = p.StockProductDJ_CharProduct.CharProductItem.Nama,
                          Tipe = 1,
                          ImgPicture = p.ImgPicture
                      }).Union(
                     from p in db.StockProductDJCustomer
                     where p.Draft == false
                     && (from z in db.DocTitipan_Product
						where (oldcodecust != -1) ? (z.DocTitipan.IDCustomer == d.IDCustomer || z.DocTitipan.IDCustomer == idcustcheck) :
                        z.DocTitipan.IDCustomer == d.IDCustomer
                         select z.IDProductTitipan).Contains(p.ID)
                     select new
                     {
                         Nomor = p.Nomor,
                         ID = p.ID,
                         HargaRupiahRound = 0m,
                         ProductItem = p.StockProductDJCustomer_CharProduct.CharProductItem.Nama,
                         Tipe = 2,
                         ImgPicture = p.ImgPicture
                     });

            if (kw.Text != "")
                rs = rs.Where(p => p.Nomor == kw.Text);

            int index = 1;

            //if (rs.Count() == 1)
            //{
            //    Response.Redirect("Repair_ItemAddSave.aspx?id=" + myID + "&item=" + rs.First().ID);
            //}
            //else
            //{
            if (rs.Count() > 0)
            {
                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.ID,r.Tipe);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.ProductItem;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.HargaRupiahRound).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Img(r.ImgPicture);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    index++;
                }
            }
        }
    }
    protected string TD_HyperLink(int ID, int i)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "Repair_ItemAddSave.aspx";
        href += "?id=" + myID;
        href += "&item=" + ID;
        href += "&itemindex=" + i;

        x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }

    protected string TD_Img(string Img)
    {
        string x = string.Empty;
        string href = string.Empty;

        x += "<img src=\"/Images.aspx?path=" + Img + "\" width=\"150\" />";

        return x;
    }
}