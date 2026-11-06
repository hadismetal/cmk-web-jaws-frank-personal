using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_DJ : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
        App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
            Ddl.Filter(productitem,"CharProductItem");
            Ddl.Filter(productcategory,"CharProductCategory");
            Ddl.Filter(productlevel,"CharProductLevel");

            //datepicker.Text = Str.Tgl(DateTime.Now);
            //datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable1(sender, e);
		}
	}

    //Display Table
    protected void fillTable1(object sender, EventArgs e)
    {        
                
        // Make sure the controls are DropDownList        
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);        
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);        
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);                

        string startRowValue = startROW.InnerHtml;        
        int pageStart = int.Parse(startRowValue);        
        
        int pageSize=10;        
        

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))                
        {
        string query = @"create table #tempLoc(
				        	IDLokasi int,
				        	NamaLokasi varchar(100),
				        	TipeLokasi varchar(20)
				        )			

				        insert into #tempLoc
				        select ID,Nama,0 from LocWarehouse

				        insert into #tempLoc
				        select ID,Nama,1 from LocOutlet

				        insert into #tempLoc
				        select ID,Nama,2 from LocExhibition

				        select a.IDProductLD,ca.Nama,b.Nomor,f.Nomor,ISNULL(e.NamaLokasi,d.NamaLokasi) as Lokasi
				        from StockProductDJLD a 
				        JOIN StockProductDJ b on b.ID=a.IDProductDJ
				        JOIN StockProductDJ_CharProduct c on c.ID=b.ID
				        JOIN CharProductItem ca on ca.ID=c.ProductItem
				        JOIN StockActualDJ d on d.IDProduct=a.IDProductDJ
				        left JOIN #tempLoc e on e.IDLokasi=d.IDLokasi and e.TipeLokasi=d.TipeLokasi
				        JOIN StockProductLD_Stone1B f on f.id=a.IDProductLD
				        where b.ReturnProduct=0 and StatusLebur=0 
				        and a.IDProductDJ NOT IN (select a.IDProduct from SalesOrderDJ a JOIN SalesOrder b on b.ID=a.IDForm where b.StatusVoid=0 and a.StatusResell=0)				 
                        and ('@kw' = '' or (b.Nomor like '%'+@kw+'%' or f.Nomor like '%'+@kw+'%')) and (@productlevel=0 or c.ProductLevel=@productlevel) 
                        and (@prodcategory=0 or c.ProductCategory=@prodcategory) and (@proditem=0 or c.ProductItem=@proditem)
				        ORDER BY b.Nomor ASC                        
                        OFFSET "+ (pageStart <= 0?1:pageStart -1) * pageSize +@" ROWS
                        FETCH NEXT "+ pageSize +@" ROWS ONLY
    
				        drop table #tempLoc";                        

                    string query1 = @"

				        select COUNT(*) as totalPages
				        from StockProductDJLD a 
				        JOIN StockProductDJ b on b.ID=a.IDProductDJ
				        JOIN StockProductDJ_CharProduct c on c.ID=b.ID
				        JOIN CharProductItem ca on ca.ID=c.ProductItem
				        JOIN StockActualDJ d on d.IDProduct=a.IDProductDJ				        
				        JOIN StockProductLD_Stone1B f on f.id=a.IDProductLD
				        where b.ReturnProduct=0 and StatusLebur=0 
				        and a.IDProductDJ NOT IN (select a.IDProduct from SalesOrderDJ a JOIN SalesOrder b on b.ID=a.IDForm where b.StatusVoid=0 and a.StatusResell=0)				 
                        and ('@kw' = '' or (b.Nomor like '%'+@kw+'%' or f.Nomor like '%'+@kw+'%')) and (@productlevel=0 or c.ProductLevel=@productlevel) 
                        and (@prodcategory=0 or c.ProductCategory=@prodcategory) and (@proditem=0 or c.ProductItem=@proditem)";                        

            using (SqlCommand cmd = new SqlCommand(query1, connection))
            {
                cmd.CommandTimeout = 1;                
                cmd.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = kw.Text });
                cmd.Parameters.Add(new SqlParameter("@productlevel", SqlDbType.Int) { Value = ProductLevel });
                cmd.Parameters.Add(new SqlParameter("@prodcategory", SqlDbType.Int) { Value = ProductCategory });
                cmd.Parameters.Add(new SqlParameter("@proditem", SqlDbType.Int) { Value = ProductItem });

                connection.Open();                
                int totalRecord=0;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                totalRecord = reader.Read() ? Convert.ToInt32(reader["totalPages"]) : 0;
            }
                connection.Close(); 
                BindData(pageStart,pageSize,totalRecord);             
            }    
            using (SqlCommand command = new SqlCommand(query, connection))            
            {
                command.CommandTimeout = 1;                
                command.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = kw.Text });
                command.Parameters.Add(new SqlParameter("@productlevel", SqlDbType.Int) { Value = ProductLevel });
                command.Parameters.Add(new SqlParameter("@prodcategory", SqlDbType.Int) { Value = ProductCategory });
                command.Parameters.Add(new SqlParameter("@proditem", SqlDbType.Int) { Value = ProductItem });
                


                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);                 
                pageStart = pageStart <= 0 ? 1 : pageStart; 
                int no = ((pageStart-1) * pageSize) + 1;        
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count == 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;
                        td = new TableCell();
                        td.Text = "No data available";
                        td.ColumnSpan = 12;
                        td.Style.Add("text-align", "center");
                        tr.Cells.Add(td);
                        tb.Rows.Add(tr);
                        return;
                    }
                 foreach (DataRow row in dt.Rows)
                {
                    
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_Hyperlink(Convert.ToInt32(row[0]));
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = no.ToString();                    
                    tr.Cells.Add(td);

                    td = new TableCell();                    
                    td.Text = Convert.ToString(row[1]);
                    tr.Cells.Add(td);

                    td = new TableCell();                    
                    td.Text = Convert.ToString(row[2]);
                    tr.Cells.Add(td);

                    td = new TableCell();                    
                    td.Text = Convert.ToString(row[3]);
                    tr.Cells.Add(td);

                    td = new TableCell();                    
                    td.Text = Convert.ToString(row[4]);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                    no++;
                }
            
            }        
            }
       
        }

    }
   

   private void BindData(int page,int pageSize,int totalRecord)
    {     
        int totalRecords = totalRecord; // Fetch total records from DB
        int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);        

        GeneratePagination(page, totalPages);
    }  

    private void GeneratePagination(int currentPage, int totalPages)
    {        
        string paginationHtml = "";

        // Previous Button            
        // paginationHtml += $"<li class='{(currentPage == 1 ? "disabled" : "")}'> <a href='{(currentPage > 1 ? url + (currentPage - 1) : "#")}'>&laquo;</a> </li>";
        paginationHtml += "<a href='DJLD.aspx?refreshPage=1&kw="+kw.Text+"&productitem="+productitem.SelectedValue+"&productcategory="+productcategory.SelectedValue+"&productlevel="+productlevel.SelectedValue+"&startROW="+(int.Parse(startROW.InnerHtml)-1)+"' class='btn btn-default' "+(currentPage == 1 ? "disabled" : "")+" runat='server'>Prev</a>";

        // Page Numbers
        int maxDisplay = 10;
        int startPage = Math.Max(1, currentPage - maxDisplay / 2);
        int endPage = Math.Min(totalPages, startPage + maxDisplay - 1);

        if (startPage > 1)
        {
            paginationHtml += "<a href='DJLD.aspx?refreshPage=1&kw=" + kw.Text + "&productitem=" + productitem.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productlevel=" + productlevel.SelectedValue + "&startROW=1' class='btn btn-default' runat='server'>1</a>";
            if (startPage > 2)
            {
            paginationHtml += "<span readonly class='btn btn-default'>...</span>";
            }
        }

        for (int i = startPage; i <= endPage; i++)
        {
            paginationHtml += "<a href='DJLD.aspx?refreshPage=1&kw=" + kw.Text + "&productitem=" + productitem.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productlevel=" + productlevel.SelectedValue + "&startROW=" + i + "' class='btn btn-default' " + (currentPage == i ? "disabled" : "") + " runat='server'>" + i + "</a>";
        }

        if (endPage < totalPages)
        {
            if (endPage < totalPages - 1)
            {
            paginationHtml += "<span readonly class='btn btn-default'>...</span>";
            }
            paginationHtml += "<a href='DJLD.aspx?refreshPage=1&kw=" + kw.Text + "&productitem=" + productitem.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productlevel=" + productlevel.SelectedValue + "&startROW=" + totalPages + "' class='btn btn-default' runat='server'>" + totalPages + "</a>";
        }

        // Next Button
        paginationHtml += "<a href='DJLD.aspx?refreshPage=1&kw="+kw.Text+"&productitem="+productitem.SelectedValue+"&productcategory="+productcategory.SelectedValue+"&productlevel="+productlevel.SelectedValue+"&startROW="+(int.Parse(startROW.InnerHtml)+1)+"' class='btn btn-default' "+((currentPage == totalPages || currentPage >totalPages) ? "disabled" : "")+" runat='server'>Next</a>";

        paginationGRP.InnerHtml= paginationHtml;
    }
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.StockProductDJLD
                 where p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusLebur == false
                           && !(from s in db.SalesOrderDJ
                               where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
                               select s.IDProduct).Contains(p.IDProductDJ.Value)
                 select p;

        rs = rs.OrderByDescending(p => p.StockProductDJ.ApprovalTgl);

        if(kw.Text!="")
            rs = rs.Where(p=>p.StockProductDJ.Nomor==""+kw.Text+"" || p.StockProductLD_Stone1B.Nomor == "" + kw.Text + "");
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == ProductCategory);

        var rsg = rs.GroupBy(p => p.StockProductDJ.ID);

        if (rs != null)
        {
            int index = 1;
            foreach (var r in rsg)
            {
                var ld = from p in db.StockProductDJLD
                         where p.IDProductDJ == r.Key && p.Draft == false
                         select p;

                foreach (var j in ld)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_Hyperlink(j.ID);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = index++.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = j.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = j.StockProductDJ.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = j.StockProductLD_Stone1B.Nomor;
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    var x = db.StockActualDJ.SingleOrDefault(p => p.IDProduct == j.StockProductDJ.ID);
                    if (x.TipeLokasi == 0)
                    {
                        Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                    }
                    else if (x.TipeLokasi == 1)
                    {
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                    }
                    else if (x.TipeLokasi == 2)
                    {
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                    }

                    td = new TableCell();
                    td.Text = Lokasi;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }

                

            }

            if (rs.Count() > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
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
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            startROW.InnerHtml = Request.QueryString["startROW"];            
            //datepicker.Text = Request.QueryString["dari"];
            //datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&tipe=" + tipe;
        href += "&startROW=1";
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected string TD_Hyperlink(int ID)
    {
        string x = string.Empty;

        x += "<div class=\"btn-group\">";
        x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + ID + "')\">";
        x += "      <i class=\"icon-trash\"></i> ";
        x += "      Delete";
        x += "  </a>";
        x += "</div>";

        return x;
    }
}