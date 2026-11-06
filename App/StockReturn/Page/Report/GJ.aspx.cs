using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_GJ : System.Web.UI.Page
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
    protected int ReturnID { get { return App.QueryInt(this, "id"); } }
    protected int ItemID { get { return App.QueryInt(this, "item"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            DLC.StockKeeperDJ(sk);
            DLC.DDLSupplier(supplier, 0);
            DLC.DDLSupplierTujuan(suppliertujuan, 1);
            fillFilter();
            if (App.Refresh(this)) Set();
        }
    }

    protected void Set()
    {
        DateTime start;
        DateTime end;

        int Qty = 0;
        decimal netweight = 0m;
        decimal frameprice = 0m;
        decimal totalbutir = 0m;
        decimal totalcarat = 0m;
        decimal totalharga = 0m;
        decimal no = 0m;
        decimal ro = 0m;
        decimal finishing = 0m;
        decimal stonesetting = 0m;
        decimal hargausd = 0m;
        decimal hargarp = 0m;
        decimal totaljual = 0m;

        var rs = from p in db.StockReturnGJ_Product
                 where p.StockReturnGJ.Approval != null
                 select p;

        if (kw.Text != "")
        {
            rs = from p in rs
                 where p.Nomor.Contains(kw.Text) || p.StockProductGJ.ProtoNomor.Contains(kw.Text) || p.StockProductGJ.Nomor.Contains(kw.Text)
                 select p;
        }

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.StockReturnGJ.ApprovalTgl >= start && p.StockReturnGJ.ApprovalTgl < end.AddDays(1));
        }

        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == ProductCategory);
        string Supplier = supplier.SelectedValue;
        if (Supplier != "")
            rs = rs.Where(p => p.StockProductGJ.SupplierNama.Equals(Supplier));
        string SupplierTujuan = suppliertujuan.SelectedValue;
        if (SupplierTujuan != "")
            rs = rs.Where(p => p.StockReturnGJ.SupplierNama.Equals(SupplierTujuan));
        if (sk.SelectedValue != "0") rs = rs.Where(p => p.StockProductGJ.HumanStockNama == "" + sk.SelectedValue + "");

        rs = rs.OrderByDescending(p => p.StockReturnGJ.ApprovalTgl);

        if (rs != null)
        {
            int index = 1;
            foreach (var r in rs)
            {
                /*TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ.ProtoNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ.StockProductGJ_PricingMU.ModalRupiah).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ.StockProductGJ_CharProduct.GrossWeight).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ.StockProductGJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ.StockProductGJ_CharProduct.StoneQty).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ.StockProductGJ_CharProduct.StoneWeight).ToString("N3");
                tr.Cells.Add(td);

                HiddenField hf = new HiddenField();
                hf.ID = "hf" + index.ToString();
                hf.Value = r.IDProduct.ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                td.Controls.Add(hf);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);*/

                decimal AllM = 0m;
                decimal Rate = 0m;

                Rate = r.StockProductGJ.StockProductGJ_PricingBiaya.TotalRate;

                var y = r.StockProductGJ.StockProductGJ_CostingProduct;
                var z = r.StockProductGJ.StockProductGJ_Costing;

                AllM = (y.Total + z.TotalBiaya);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.StockReturnGJ.NoDokumen;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.StockReturnGJ.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.StockReturnGJ.ApprovalTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockReturnGJ.SupplierNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ.ProtoNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ.StockProductGJ_CharProduct.CharProductCategory.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ.StockProductGJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ.StockProductGJ_CharProduct.NettoWeight.ToString("N2");
                tr.Cells.Add(td);

                netweight += r.StockProductGJ.StockProductGJ_CharProduct.NettoWeight;

                td = new TableCell();
                td.Text = r.StockProductGJ.StockProductGJ_CostingProduct.TotalProduct.ToString("N2");
                tr.Cells.Add(td);

                frameprice += r.StockProductGJ.StockProductGJ_CostingProduct.TotalProduct;

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ.StockProductGJ_Costing.MountingNO).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ.StockProductGJ_Costing.MountingRO.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ.StockProductGJ_Finishing.Sum(x => x.BiayaModal)).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ.StockProductGJ_Costing.Setting).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                //td.Text = r.StockProductGJ_PricingMU.ModalUSD.ToString("N2");
                td.Text = AllM.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                //td.Text = Convert.ToDecimal(r.StockProductGJ_PricingMU.ModalRupiah).ToString("N2");
                td.Text = (AllM * Rate).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                //td.Text = Convert.ToDecimal(r.StockProductGJ_PricingMU.ModalRupiah).ToString("N2");
                td.Text = (r.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(r.StockReturnGJ.Keterangan);
                tr.Cells.Add(td);

                no += Convert.ToDecimal(r.StockProductGJ.StockProductGJ_Costing.MountingNO);
                ro += r.StockProductGJ.StockProductGJ_Costing.MountingRO;
                finishing += Convert.ToDecimal(r.StockProductGJ.StockProductGJ_Finishing.Sum(x => x.BiayaModal));
                stonesetting += Convert.ToDecimal(r.StockProductGJ.StockProductGJ_Costing.Setting);
                hargausd += AllM;
                hargarp += (AllM * Rate);
                totaljual += r.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value;

                tb.Rows.Add(tr);

                Qty++;
                index++;
            }
            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + Qty.ToString() + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + netweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + frameprice.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + totalharga.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + no.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + ro.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + finishing.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + hargausd.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + hargarp.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + totaljual.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);

        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            sk.Text = Request.QueryString["sk"];
            supplier.SelectedValue = Request.QueryString["supplier"];
            suppliertujuan.SelectedValue = Request.QueryString["suppliertujuan"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&sk=" + sk.Text;
        href += "&supplier=" + supplier.Text;
        href += "&suppliertujuan=" + suppliertujuan.Text;
        // href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "GJ_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&sk=" + sk.Text;
        href += "&supplier=" + supplier.Text;
        href += "&suppliertujuan=" + suppliertujuan.Text;
        // href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "GJ_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&sk=" + sk.Text;
        href += "&supplier=" + supplier.Text;
        href += "&suppliertujuan=" + suppliertujuan.Text;
        // href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
}