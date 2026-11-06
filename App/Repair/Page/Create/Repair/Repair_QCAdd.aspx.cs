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
        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        //href += "&brand=" + brand.SelectedValue;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            //brand.SelectedValue = Request.QueryString["brand"];
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);

        if (d != null)
        {
            //var rs = (from p in db.StockProductDJ
            //          where p.Draft == false && p.ReturnProduct == false
            //          && (from z in db.StockActualDJ
            //              where z.TipeLokasi == d.TipeLokasi
            //                 && z.IDLokasi == d.IDLokasi
            //              select z.IDProduct).Contains(p.ID)
            //          && (from z in db.SalesOrderDJ
            //              where z.SalesOrder.IDCustomer == d.IDCustomer
            //              select z.IDProduct).Contains(p.ID)
            //          select new
            //          {
            //              Nomor = p.Nomor,
            //              ID = p.ID,
            //              HargaRupiahRound = p.StockProductDJ_PricingMU.HargaRupiahRound.Value,
            //              ProductItem = p.StockProductDJ_CharProduct.CharProductItem.Nama,
            //              Tipe = 1,
            //              ImgPicture = p.ImgPicture
            //          }).Union(
            //         from p in db.StockProductDJCustomer
            //         where p.Draft == false
            //         && (from z in db.DocTitipan_Product
            //             where z.DocTitipan.IDCustomer == d.IDCustomer 
            //                && z.DocTitipan.TipeLokasi == d.TipeLokasi 
            //                && z.DocTitipan.IDLokasi == d.IDLokasi
            //                && z.DocTitipan.IDSales == d.IDSales
            //             select z.IDProductTitipan).Contains(p.ID)
            //         select new
            //         {
            //             Nomor = p.Nomor,
            //             ID = p.ID,
            //             HargaRupiahRound = 0m,
            //             ProductItem = p.StockProductDJCustomer_CharProduct.CharProductItem.Nama,
            //             Tipe = 2,
            //             ImgPicture = p.ImgPicture
            //         });

            var rs = (from p in db.DocQCDJ
                      where p.Draft == false && p.Nomor.Contains(kw.Text.Trim())
                      select new
                      {
                          Nomor = p.Nomor,
                          ID = p.ID,
                          PLU = p.StockProductDJ.Nomor,
                          Operator = p.OperatorNama,
                          OperatorTgl = p.OperatorTgl,
                          ImgPicture = p.StockProductDJ.ImgPicture,
                          Tipe = 1
                      }).Union(
                      from p in db.DocQCDJCustomer
                      where p.Draft == false && p.Nomor.Contains(kw.Text.Trim())
                      select new
                      {
                          Nomor = p.Nomor,
                          ID = p.ID,
                          PLU = p.StockProductDJCustomer.Nomor,
                          Operator = p.OperatorNama,
                          OperatorTgl = p.OperatorTgl,
                          ImgPicture = p.StockProductDJCustomer.ImgPicture,
                          Tipe = 2
                      }).Where(p => p.PLU == (d.IDProduct != null ? d.StockProductDJ.Nomor : d.StockProductDJCustomer.Nomor)).OrderByDescending(p => p.OperatorTgl);

            //if (kw.Text != "")
            //    rs = rs.Where(p => p.Nomor.Contains(kw.Text));

            int index = 1;

            if (rs != null)
            {
                //if (rs.Count() > 0)
                //{
                    foreach (var r in rs)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(r.ID, r.Tipe);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index.ToString() + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.Nomor;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.PLU;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.Operator;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Str.FullDate(r.OperatorTgl);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Img(r.ImgPicture);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);

                        index++;
                    }
                //}
            }
            else
            {
                lit_message.Text = "Data QC tidak ditemukan. Silahkan buat dokumen QC terhadap PLU bersangkutan terlebih dahulu.";
            }
        }
    }
    protected string TD_HyperLink(int ID, int i)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "Repair_QCAddSave.aspx";
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