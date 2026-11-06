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
            header_nomor.InnerHtml =
                "<a href=\"../History/Repair_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

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
            if (kw.Text.Length > 2)
            {
                var rs = (from p in db.StockProductDJ
                          where p.Draft == false && p.ReturnProduct == false

                          select new
                          {
                              Nomor = p.Nomor,
                              ID = p.ID,
                              HargaRupiahRound = p.StockProductDJ_PricingMU.HargaRupiahRound.Value,
                              ProductItem = p.StockProductDJ_CharProduct.CharProductItem.Nama,
                              Tipe = 1,
                              ImgPicture = p.ImgPicture
                          });

                if (kw.Text != "")
                    rs = rs.Where(p => p.Nomor.Contains(kw.Text));

                int index = 1;

                if (rs.Count() == 1)
                {
                    Response.Redirect("Repair_ItemAddSave.aspx?id=" + myID + "&item=" + rs.First().ID + "&itemindex=1");
                }
                else
                {
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