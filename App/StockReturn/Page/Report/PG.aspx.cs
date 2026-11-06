using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_Report_PG_List : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            Ddl.Filter(productitem,"CharProductItem");
            Ddl.Filter(productlevel,"CharGoldLevel");
            Ddl.Filter(framecolor,"CharFrameColor");
            Ddl.Filter(brand,"CompanyBrand");
            Ddl.Filter(model,"CharGoldModel");
            // DLC.CharGoldModelAllFilter(model);
            Ddl.Supplier(supplier);
            Ddl.Supplier(suppliertujuan);

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable1(sender, e);
        }
    }

    //Display Table
    
    protected void fillTable1(object sender, EventArgs e)        
    {
        DateTime start = Convert.ToDateTime(datepicker.Text);
        DateTime end = Convert.ToDateTime(datepicker2.Text);

        int index = 1;
        decimal TotalHarga = 0m;
        decimal TotalBerat = 0m;
        decimal Harga = 0m;
        decimal OngkosR = 0m;
        decimal Berat = 0m;
        decimal Qty = 0m;
        decimal KadarLogam = 0m;
        decimal KadarTukaranJual = 0m;
        decimal KadarTukaranBeli = 0m;
        decimal TotalBerat24 = 0m;
        decimal TotalHarga24 = 0m;
        decimal TotalQty = 0m;
        decimal TotalWeight = 0m;
        decimal TotalOngkos = 0m;
        decimal TotalTotal = 0m;
        decimal TotalHPPM = 0m;
        decimal TotalBeratResell = 0m;
        int productLevel = Convert.ToInt32(productlevel.SelectedValue);
        int goldModel = Convert.ToInt32(model.SelectedValue);
        int Fcolor = Convert.ToInt32(framecolor.SelectedValue);
        int item = Convert.ToInt32(productitem.SelectedValue);

        string query = @"                         
                         select c.ImgPicture, a.NoDokumen, c.Nomor as PLU, c.KodeBarang, FORMAT(CONVERT(DATETIME, a.Tgl, 113), 'dddd, dd MMMM yyyy', 'id-ID') Tgl, FORMAT(CONVERT(DATETIME, a.ApprovalTgl, 113), 'dddd, dd MMMM yyyy', 'id-ID') ApprovalTgl, a.SupplierNama as SupplierAsal, 1 as Qty,
                         ca.Nama as Item, cb.Nama as GoldLevel, cc.Nama as GoldModel, cd.Nama as FrameColor,
                         c.KadarLogam, c.GrossWeight, c.MountingR, c.TotalHarga, c.NettoWeight, c.TotalHarga24,
                         c.Harga, c.SupplierNama as KodeSupplier, c.TotalRate, a.Keterangan, CAST(ISNULL(c.DimensiR,0) as numeric(9,2)) as DimensiR,
                         cast(ISNULL(d.BeratTimbang,0) as numeric(9,2)) as BeratResell
                         from StockReturnPG a
                         JOIN StockReturnPG_Product b on b.IDForm=a.ID
                         JOIN StockProductPG c on c.ID=b.IDProduct
                         JOIN CharProductItem ca on ca.ID=c.ProductItem
                         JOIN CharGoldLevel cb on cb.ID=c.ProductLevel
                         JOIN CharGoldModel cc on cc.ID=c.Model
                         JOIN CharFrameColor cd on cd.ID=c.FrameColor
                         LEFT JOIN (
                         			Select CAST(ISNULL(BeratTimbang, 0)AS numeric(9,2)) BeratTimbang, bx.IDProduct 
                         			from Resell ax 
                         			JOIN ResellPG bx on ax.ID=bx.IDForm AND ISNULL(StatusVoid,0) = 0 
                         			)  d on d.IDProduct = c.ID
                         WHERE a.Approval is not null and a.Draft=0
                         and CAST(a.Tgl as date) between @StartDate and @EndDate
                         and (@Brand = 0 or c.IDBrand=@Brand) and (@item = 0 or c.ProductItem=@item)
                         and (@level=0 or c.ProductLevel=@level) and (@model=0 or c.Model=@model)
                         and (@Fcolor=0 or c.FrameColor=@Fcolor) and (@SupplierAsal=0 or c.IDSupplier=@SupplierAsal)
                         and (@SupplierTujuan=0 or a.Supplier = @SupplierTujuan)
                         and (@keyword = '' or ca.Nama like '%' + @keyword + '%' or cb.Nama like '%' + @keyword + '%' or cc.Nama like '%' + @keyword + '%' or cd.Nama like '%' + @keyword + '%'
                                or a.NoDokumen like '%' + @keyword + '%' or c.Nomor like '%' + @keyword + '%')
                         order by a.ApprovalTgl desc";
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
        try
        {
            using(SqlCommand cmd = new SqlCommand(query, conn))            
            {
                cmd.Parameters.AddWithValue("@StartDate", start);
                cmd.Parameters.AddWithValue("@EndDate", end.AddDays(1));
                cmd.Parameters.AddWithValue("@Brand", brand.SelectedValue);
                cmd.Parameters.AddWithValue("@item", item);
                cmd.Parameters.AddWithValue("@level", productLevel);
                cmd.Parameters.AddWithValue("@model", goldModel);
                cmd.Parameters.AddWithValue("@Fcolor", Fcolor);
                cmd.Parameters.AddWithValue("@SupplierAsal", supplier.SelectedValue);
                cmd.Parameters.AddWithValue("@SupplierTujuan", suppliertujuan.SelectedValue);
                cmd.Parameters.AddWithValue("@keyword", kw.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                {
                    App.TableNoResult(tb);                                                
                } 

                foreach (DataRow rslt in dt.Rows)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = index.ToString() + ".";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    Image imgpicture = new Image();
                    imgpicture.CssClass = "img-thumbnail";
                    imgpicture.Width = 200;
                    UploadS.Image(imgpicture, rslt["ImgPicture"].ToString());
                    td.Controls.Add(imgpicture);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<span class=\"text-sm\">" + rslt["NoDokumen"].ToString() + "</span>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<span class=\"text-sm\">" + rslt["PLU"].ToString()+ "</span>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<span class=\"text-sm\">" + rslt["KodeBarang"].ToString() + "</span>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<span class=\"text-sm\">" + rslt["Tgl"].ToString() + "</span>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<span class=\"text-sm\">" + rslt["ApprovalTgl"].ToString() + "</span>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rslt["SupplierAsal"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rslt["Item"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rslt["GoldLevel"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rslt["GoldModel"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(rslt["KadarLogam"]).ToString("N2") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rslt["FrameColor"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(rslt["DimensiR"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rslt["KodeSupplier"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(rslt["Qty"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(rslt["GrossWeight"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(rslt["BeratResell"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(rslt["MountingR"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(rslt["Harga"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (Convert.ToDecimal(rslt["Harga"]) + Convert.ToDecimal(rslt["MountingR"])).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (Convert.ToDecimal(rslt["TotalHarga24"]) / Convert.ToDecimal(rslt["TotalRate"])).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.HtmlBreak(rslt["Keterangan"].ToString());
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    TotalQty += Convert.ToDecimal(rslt["Qty"]);                    
                    TotalWeight += Convert.ToDecimal(rslt["GrossWeight"]);
                    TotalBeratResell += Convert.ToDecimal(rslt["BeratResell"]);
                    TotalOngkos += Convert.ToDecimal(rslt["MountingR"]);
                    TotalHarga += Convert.ToDecimal(rslt["Harga"]);
                    TotalBerat += Convert.ToDecimal(rslt["NettoWeight"]);
                    TotalTotal += (Convert.ToDecimal(rslt["Harga"]) + Convert.ToDecimal(rslt["MountingR"]));
                    TotalHPPM += Convert.ToDecimal(Convert.ToDecimal(rslt["TotalHarga24"]) / Convert.ToDecimal(rslt["TotalRate"]));
                    index++;
                }                    

                TableRow tr1 = new TableRow();
                TableCell td1;


                td1 = new TableCell();
                td1.Text = "<b>Total</b>";
                td1.ColumnSpan = 15;                  
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + TotalWeight.ToString("N2") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + TotalBeratResell.ToString("N2") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + TotalOngkos.ToString("N0") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + TotalHarga.ToString("N0") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + TotalTotal.ToString("N0") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + TotalHPPM.ToString("N2") + "</b>";
                tr1.Cells.Add(td1);

                tb.Rows.Add(tr1);
            
            }
        }
        catch (Exception ex)
        {
            
           Response.Write(ex.Message);
            // App.Alert(this, ex.Message);            
        }
        }                    
    }        
    
    protected void fillTable(object sender, EventArgs e)    
    {
        DateTime start;
        DateTime end;

        decimal TotalHarga = 0m;
        decimal TotalBerat = 0m;
        decimal Harga = 0m;
        decimal OngkosR = 0m;
        decimal Berat = 0m;
        decimal Qty = 0m;
        decimal KadarLogam = 0m;
        decimal KadarTukaranJual = 0m;
        decimal KadarTukaranBeli = 0m;
        decimal TotalBerat24 = 0m;
        decimal TotalHarga24 = 0m;
        decimal TotalQty = 0m;
        decimal TotalWeight = 0m;
        decimal TotalOngkos = 0m;
        decimal TotalTotal = 0m;
        decimal TotalHPPM = 0m;
        decimal TotalBeratResell = 0m;

        var rs = from p in db.StockReturnPG_Product
                 where p.StockReturnPG.Draft == false && p.StockReturnPG.Approval != null
                 select p;

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.StockReturnPG.Tgl >= start && p.StockReturnPG.Tgl < end.AddDays(1));
        }

        if (rs.Count() > 0)
        {
            int index = 1;
            if (productlevel.SelectedValue != "" && productlevel.SelectedValue != "0")
                rs = rs.Where(p => p.StockProductPG.ProductLevel == Convert.ToInt32(productlevel.SelectedValue));
            if (model.SelectedValue != "" && model.SelectedValue != "0")
                rs = rs.Where(p => p.StockProductPG.Model == Convert.ToInt32(model.SelectedValue));
            if (framecolor.SelectedValue != "" && framecolor.SelectedValue != "0")
                rs = rs.Where(p => p.StockProductPG.FrameColor == Convert.ToInt32(framecolor.SelectedValue));
            if (productitem.SelectedValue != "" && productitem.SelectedValue != "0")
                rs = rs.Where(p => p.StockProductPG.ProductItem == Convert.ToInt32(productitem.SelectedValue));

            var pg = from p in rs
                     select new
                     {
                         Nomor = p.Nomor,
                         Tgl = p.StockReturnPG.Tgl,
                         ImgPicture = p.StockProductPG.ImgPicture,
                         ApprovalTgl = p.StockReturnPG.ApprovalTgl,
                         SupplierNama = p.StockReturnPG.SupplierNama,
                         Qty = 1,
                         ProductItem = p.StockProductPG.CharProductItem.Nama,
                         ProductLevel = p.StockProductPG.CharGoldLevel.Nama,
                         Model = p.StockProductPG.CharGoldModel.Nama,
                         KadarLogam = p.StockProductPG.KadarLogam,
                         FrameColor = p.StockProductPG.CharFrameColor.Nama,
                         Weight = p.StockProductPG.GrossWeight,
                         Ongkos = p.StockProductPG.MountingR,
                         TotalHarga = p.StockProductPG.TotalHarga,
                         TotalBerat24 = p.StockProductPG.NettoWeight,
                         TotalHarga24 = p.StockProductPG.TotalHarga24,
                         Harga = p.StockProductPG.Harga,
                         KodeSupplier = p.StockProductPG.SupplierNama,
                         TotalRate = p.StockProductPG.TotalRate,
                         NoDokumen = p.StockReturnPG.NoDokumen,
                         Keterangan = p.StockReturnPG.Keterangan,
                         DimensiR = Db.SingleDecimal("SELECT TOP 1 ISNULL(DimensiR,0.00) FROM StockProductPG WHERE ID = " + p.StockProductPG.ID + " ", Db.Cnn_DB),
                         KodeBarang = p.StockProductPG.KodeBarang,
                         BeratResell = Db.SingleDecimal("SELECT TOP 1 ISNULL(BeratTimbang,0.00) FROM ResellPG A JOIN Resell B ON A.IDForm = B.ID WHERE ISNULL(B.StatusVoid, 0) = '0' AND IDProduct = " + p.StockProductPG.ID + " ", Db.Cnn_DB)
                     };

            if (kw.Text != "")
                pg = pg.Where(p => p.ProductItem.Contains(kw.Text) || p.ProductLevel.Contains(kw.Text) || p.Model.Contains(kw.Text) || p.FrameColor.Contains(kw.Text));

            if (supplier.SelectedValue != "")
            {
                int Supplier = Convert.ToInt32(supplier.SelectedValue);
                if (Supplier > 0)
                    rs = rs.Where(p => p.StockProductPG.IDSupplier.Equals(Supplier));
            }
            string SupplierTujuan = suppliertujuan.SelectedValue;
            if (SupplierTujuan != "")
                rs = rs.Where(p => p.StockReturnPG.SupplierNama.Equals(SupplierTujuan));

            pg = pg.OrderByDescending(p => p.ApprovalTgl);

            string status, status2 = "";

            foreach (var rslt in pg)
            {
                //if (!Response.IsClientConnected) break;
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                Image imgpicture = new Image();
                imgpicture.CssClass = "img-thumbnail";
                imgpicture.Width = 200;
                UploadS.Image(imgpicture, rslt.ImgPicture);
                td.Controls.Add(imgpicture);
                tr.Cells.Add(td);
            
                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + rslt.NoDokumen + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + rslt.Nomor + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + rslt.KodeBarang + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + Str.FullDate(rslt.Tgl) + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + Str.FullDate(rslt.ApprovalTgl) + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.SupplierNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.ProductItem;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.ProductLevel;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.Model;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.KadarLogam.Value.ToString("N2") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.FrameColor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.DimensiR.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.KodeSupplier;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.Qty.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.Weight.Value.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (rslt.BeratResell != 0m) ? rslt.BeratResell.ToString("N2") : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.Ongkos.Value.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.Harga).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (Convert.ToDecimal(rslt.Harga.Value) + rslt.Ongkos.Value).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (Convert.ToDecimal(rslt.TotalHarga24.Value / rslt.TotalRate.Value)).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(rslt.Keterangan);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalQty += rslt.Qty;
                TotalWeight += Convert.ToDecimal(rslt.Weight);
                TotalBeratResell += rslt.BeratResell;
                TotalOngkos += Convert.ToDecimal(rslt.Ongkos);
                TotalHarga += Convert.ToDecimal(rslt.Harga);
                TotalBerat += Convert.ToDecimal(rslt.TotalBerat24);
                TotalTotal += (Convert.ToDecimal(rslt.Harga) + Convert.ToDecimal(rslt.Ongkos));
                TotalHPPM += Convert.ToDecimal(rslt.TotalHarga24.Value / rslt.TotalRate.Value);                
                index++;
            }
            if (pg.Count() > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalBeratResell.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalOngkos.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHarga.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalTotal.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHPPM.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }
    
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            framecolor.SelectedValue = Request.QueryString["framecolor"];
            brand.SelectedValue = Request.QueryString["brand"];
            model.SelectedValue = Request.QueryString["model"];
            supplier.SelectedValue = Request.QueryString["supplier"];
            suppliertujuan.SelectedValue = Request.QueryString["suppliertujuan"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&brand=" + brand.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;
        href += "&supplier=" + supplier.Text;
        href += "&suppliertujuan=" + suppliertujuan.Text;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "PG_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&brand=" + brand.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;
        href += "&supplier=" + supplier.Text;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "PG_Export.aspx?id=1";

        href += "&kw=" + kw.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&brand=" + brand.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;
        href += "&supplier=" + supplier.Text;

        Response.Redirect(href);
    }
}