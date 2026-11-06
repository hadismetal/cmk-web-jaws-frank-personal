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
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=PG_Export.xls");
            
            fillTable1(sender, e);
        }
    }

    //Display Table
    protected void fillTable1(object sender, EventArgs e)        
    {        
        DateTime start = Convert.ToDateTime(Request.QueryString["dari"]);
        DateTime end = Convert.ToDateTime(Request.QueryString["sampai"]);

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
        int productLevel = Convert.ToInt32(Request.QueryString["productlevel"]== "" ? 0 : Convert.ToInt32(Request.QueryString["productlevel"])) ;
        int goldModel = Convert.ToInt32(Request.QueryString["model"]== "" ? 0 : Convert.ToInt32(Request.QueryString["model"])) ;
        int Fcolor = Convert.ToInt32(Request.QueryString["framecolor"]== "" ? 0 : Convert.ToInt32(Request.QueryString["framecolor"])) ;
        int item = Convert.ToInt32(Request.QueryString["productitem"]== "" ? 0 : Convert.ToInt32(Request.QueryString["productitem"])) ;

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
                cmd.Parameters.AddWithValue("@Brand", Convert.ToInt32(Request.QueryString["brand"]== "" ? 0 : Convert.ToInt32(Request.QueryString["brand"])));
                cmd.Parameters.AddWithValue("@item", item);
                cmd.Parameters.AddWithValue("@level", productLevel);
                cmd.Parameters.AddWithValue("@model", goldModel);
                cmd.Parameters.AddWithValue("@Fcolor", Fcolor);
                cmd.Parameters.AddWithValue("@SupplierAsal", Convert.ToInt32(Request.QueryString["supplier"] == "" ? 0 : Convert.ToInt32(Request.QueryString["supplier"])));
                cmd.Parameters.AddWithValue("@SupplierTujuan", Convert.ToInt32(Request.QueryString["suppliertujuan"]== "" ? 0 : Convert.ToInt32(Request.QueryString["suppliertujuan"])));
                cmd.Parameters.AddWithValue("@keyword", Request.QueryString["kw"] ?? string.Empty);

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
                    td1.ColumnSpan = 14;
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "<b>" + TotalQty.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "<b>" + TotalWeight.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "<b>" + TotalBeratResell.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "<b>" + TotalOngkos.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "<b>" + TotalHarga.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "<b>" + TotalTotal.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "<b>" + TotalHPPM.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
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
    
    protected void fillTable2(object sender, EventArgs e)    
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

        string query = @"
                         select c.ImgPicture, a.NoDokumen, c.Nomor as PLU, c.KodeBarang, CONVERT(varchar(12), a.Tgl, 110) Tgl, CONVERT(varchar(12), a.ApprovalTgl, 110) ApprovalTgl, a.SupplierNama as SupplierAsal, 1 as Qty,
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
                         			Select top 1 CAST(ISNULL(BeratTimbang, 0)AS numeric(9,2)) BeratTimbang, bx.IDProduct 
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
            start = Convert.ToDateTime(Request.QueryString["dari"]);
            end = Convert.ToDateTime(Request.QueryString["sampai"]);        
            int index = 1;            
            
            int level = Request.QueryString["productlevel"] != "" && Request.QueryString["productlevel"] != "0" ? Convert.ToInt32(Request.QueryString["productlevel"]):0;            
            int model = Request.QueryString["model"] != "" && Request.QueryString["model"] != "0" ? Convert.ToInt32(Request.QueryString["model"]):0;            
            int fcolor = Request.QueryString["framecolor"] != "" && Request.QueryString["framecolor"] != "0" ? Convert.ToInt32(Request.QueryString["framecolor"]):0;            
            int item = Request.QueryString["productitem"] != "" && Request.QueryString["productitem"] != "0" ? Convert.ToInt32(Request.QueryString["productitem"]):0;            
            int Supplier = Request.QueryString["supplier"] != "" && Request.QueryString["supplier"] !="0" ? Convert.ToInt32(Request.QueryString["supplier"]) : 0;
            int SupplierTujuan = Request.QueryString["suppliertujuan"] != "" && Request.QueryString["suppliertujuan"]!="0" ? Convert.ToInt32(Request.QueryString["suppliertujuan"]) : 0;
            int brand = Request.QueryString["brand"] != "" && Request.QueryString["brand"] != "0" ? Convert.ToInt32(Request.QueryString["brand"]) : 0;
            string keyword = Request.QueryString["kw"] ?? string.Empty;
            
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand(query, conn))            
                {
                    cmd.Parameters.AddWithValue("@StartDate", start);
                    cmd.Parameters.AddWithValue("@EndDate", end.AddDays(1));
                    cmd.Parameters.AddWithValue("@Brand", brand);
                    cmd.Parameters.AddWithValue("@item", item);
                    cmd.Parameters.AddWithValue("@level", level);
                    cmd.Parameters.AddWithValue("@model", model);
                    cmd.Parameters.AddWithValue("@Fcolor", fcolor);
                    cmd.Parameters.AddWithValue("@SupplierAsal", Supplier);
                    cmd.Parameters.AddWithValue("@SupplierTujuan", SupplierTujuan);
                    cmd.Parameters.AddWithValue("@keyword", keyword);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    DataTable dt = ds.Tables[0];
                    foreach (DataRow rslt in dt.Rows)
                    {
                        //if (!Response.IsClientConnected) break;
                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = index.ToString() + ".";
                        tr.Cells.Add(td);

                        
                        td = new TableCell();                        
                        if (rslt["ImgPicture"] != DBNull.Value)
                        {
                            try
                            {
                                string imageUrl = "https://frank.cmk.co.id/Images.aspx?path=" + rslt["ImgPicture"].ToString();
                                using (System.Net.WebClient wc = new System.Net.WebClient())
                                {
                                    try
                                    {
                                        byte[] imageBytes = wc.DownloadData(imageUrl);
                                        string base64Output = Convert.ToBase64String(imageBytes);
                                        td.Text = "<img src='data:image/png;base64,"+base64Output+"' style='width:100px;height:100px;' />";
                                    }
                                    catch (Exception ex)
                                    {
                                        td.Text = "Error loading image: " + ex.Message;
                                    }
                                }
                                // td.Text = base64Output;
                            }
                            catch (Exception ex)
                            {
                                td.Text = ex.Message;
                            }
                        }
                        else
                        {
                            td.Text = "No Image";
                        }
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class=\"text-sm\">" + rslt["NoDokumen"].ToString() + "</span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class=\"text-sm\">" + rslt["PLU"].ToString() + "</span>";
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
                        td.Text = Convert.ToDecimal(rslt["KadarLogam"]).ToString("N2").Replace(".", "").Replace(",", ".") + "%";
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
                        td.Text = Convert.ToDecimal(rslt["Qty"]).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(rslt["GrossWeight"]).ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(rslt["BeratResell"]).ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(rslt["MountingR"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(rslt["Harga"]).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = (Convert.ToDecimal(rslt["Harga"]) + Convert.ToDecimal(rslt["MountingR"])).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = (Convert.ToDecimal(rslt["TotalHarga24"]) / Convert.ToDecimal(rslt["TotalRate"])).ToString("N2").Replace(".", "").Replace(",", ".");
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


        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            start = Convert.ToDateTime(Request.QueryString["dari"]);
            end = Convert.ToDateTime(Request.QueryString["sampai"]);
            rs = rs.Where(p => p.StockReturnPG.Tgl >= start && p.StockReturnPG.Tgl < end.AddDays(1));
        }

        if (rs.Count() > 0)
        {
            int index = 1;
            if (Request.QueryString["productlevel"] != "" && Request.QueryString["productlevel"] != "0")
                rs = rs.Where(p => p.StockProductPG.ProductLevel == Convert.ToInt32(Request.QueryString["productlevel"]));
            if (Request.QueryString["model"] != "" && Request.QueryString["model"] != "0")
                rs = rs.Where(p => p.StockProductPG.Model == Convert.ToInt32(Request.QueryString["model"]));
            if (Request.QueryString["framecolor"] != "" && Request.QueryString["framecolor"] != "0")
                rs = rs.Where(p => p.StockProductPG.FrameColor == Convert.ToInt32(Request.QueryString["framecolor"]));
            if (Request.QueryString["productitem"] != "" && Request.QueryString["productitem"] != "0")
                rs = rs.Where(p => p.StockProductPG.ProductItem == Convert.ToInt32(Request.QueryString["productitem"]));

            var pg = from p in rs
                     select new
                     {
                         Nomor = p.Nomor,
                         Tgl = p.StockReturnPG.Tgl,
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

            if (Request.QueryString["kw"] != "")
                pg = pg.Where(p => p.ProductItem.Contains(Request.QueryString["kw"]) || p.ProductLevel.Contains(Request.QueryString["kw"]) || p.Model.Contains(Request.QueryString["kw"]) || p.FrameColor.Contains(Request.QueryString["kw"]));

            string Supplier = Request.QueryString["supplier"];
            if (Supplier != "")
                rs = rs.Where(p => p.StockProductPG.IDSupplier.Equals(Supplier));

            string SupplierTujuan = Request.QueryString["suppliertujuan"];
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
                td.Text = rslt.KadarLogam.Value.ToString("N2").Replace(".", "").Replace(",", ".") + "%";
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
                td.Text = rslt.Qty.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.Weight.Value.ToString("N2").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (rslt.BeratResell != 0m) ? rslt.BeratResell.ToString("N2").Replace(".", "").Replace(",", ".") : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.Ongkos.Value.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.Harga).ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (Convert.ToDecimal(rslt.Harga.Value) + rslt.Ongkos.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (Convert.ToDecimal(rslt.TotalHarga24.Value / rslt.TotalRate.Value)).ToString("N2").Replace(".", "").Replace(",", ".");
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
                td.Text = "<b>" + TotalQty.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalWeight.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalBeratResell.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalOngkos.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHarga.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalTotal.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHPPM.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }
}