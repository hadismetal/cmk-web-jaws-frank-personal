using System.Linq;
using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Globalization;
using System.Data.SqlClient;

public partial class Page_Report_CalcPricingStone_index : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) 
        {	
            kw.Focus(); //Keyword		
            Ddl.DropdownShapeCalc(parcel01);
			Ddl.DropdownSize(parcel02);
			Ddl.DropdownColorCalc(parcel03);
			// DLC.Parcel103(parcel03);
			Ddl.DropdownClarityCalc(parcel04);
			// DLC.Parcel104(parcel04);
			Ddl.DropdownCuttingCalc(parcel05);
			Ddl.DropdownBrandStoneCalc(parcel06);						
			Ddl.DropdownCatCalc(parcel07);						
			Ddl.CompanyBrand2(BrandC);						
        fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}

	}


    protected void fillFilter() {
		if (App.Refresh(this)) {
			
            kw.Text = Request.QueryString["kw"];
			parcel01.SelectedValue = Request.QueryString["parcel01"];
			parcel02.SelectedValue = Request.QueryString["parcel02"];
			parcel03.SelectedValue = Request.QueryString["parcel03"];
			parcel04.SelectedValue = Request.QueryString["parcel04"];
			parcel05.SelectedValue = Request.QueryString["parcel05"];
			parcel06.SelectedValue = Request.QueryString["parcel06"];			
			parcel07.SelectedValue = Request.QueryString["parcel07"];			
			BrandC.SelectedValue = Request.QueryString["BrandC"];			
			order.SelectedValue = Request.QueryString["order"];			
			orderby.SelectedValue = Request.QueryString["orderby"];			


		}
	}
    protected void fillTable(object sender, EventArgs e) {
        int Shape=0;
        int Carat=0;
        int Colour=0;
        int Clarity=0;
        int Cutting=0;
        int Brand=0;
        int Category=0;
        string Brand1="";
        decimal CaratVal ;
        string textFromTextBox = kw.Text.Trim(); // Assuming kw is your TextBox
        

        if (parcel01.SelectedIndex > 0)
        {
            Shape = Convert.ToInt32(parcel01.SelectedValue);
        }
        if (parcel02.SelectedIndex > 0)
        {
            Carat = Convert.ToInt32(parcel02.SelectedValue);
        }
        if (parcel03.SelectedIndex > 0)
        {
            Colour = Convert.ToInt32(parcel03.SelectedValue);
        }
        if (parcel04.SelectedIndex > 0)
        {
            Clarity = Convert.ToInt32(parcel04.SelectedValue);
        }
        if (parcel05.SelectedIndex > 0)
        {
            Cutting = Convert.ToInt32(parcel05.SelectedValue);
        }
        if (parcel06.SelectedIndex > 0)
        {
            Brand = Convert.ToInt32(parcel06.SelectedValue);
        }
        if (parcel07.SelectedIndex > 0)
        {
            Category = Convert.ToInt32(parcel07.SelectedValue);
        }
        if (textFromTextBox == " ")
        {
            textFromTextBox="0";
        }
        // string query1 ="";
        // string query="";        
        
        // if (BrandC.SelectedValue == "F"){                        
        //     query = "EXEC dbo.sp_CalcPricingStone_hadis "+ Shape +","+ Carat +","+ Colour +","+ Clarity +","+ Cutting +","+ Brand +",'"+ order.SelectedValue +"','"+ orderby.SelectedValue +"','01','"+ textFromTextBox.Replace(",",".") +"'";		
        //     query1 = "EXEC dbo.sp_CalcPricingStone_hadis "+ Shape +","+ Carat +","+ Colour +","+ Clarity +","+ Cutting +","+ Brand +",'"+ order.SelectedValue +"','"+ orderby.SelectedValue +"','99','"+ textFromTextBox.Replace(",",".") +"'";
        // }else if (BrandC.SelectedValue == "M"){            
        //     query = "EXEC mondial_db_real.dbo.sp_CalcPricingStone_hadis "+ Shape +","+ Carat +","+ Colour +","+ Clarity +","+ Cutting +","+ Brand +",'"+ order.SelectedValue +"','"+ orderby.SelectedValue +"','01','"+ textFromTextBox.Replace(",",".") +"'";		
        //     query1 = "EXEC mondial_db_real.dbo.sp_CalcPricingStone_hadis "+ Shape +","+ Carat +","+ Colour +","+ Clarity +","+ Cutting +","+ Brand +",'"+ order.SelectedValue +"','"+ orderby.SelectedValue +"','99','"+ textFromTextBox.Replace(",",".") +"'";		
        // }else if (BrandC.SelectedValue == "P"){
        //     query = "EXEC palace_db_real.dbo.sp_CalcPricingStone_hadis "+ Shape +","+ Carat +","+ Colour +","+ Clarity +","+ Cutting +","+ Brand +",'"+ order.SelectedValue +"','"+ orderby.SelectedValue +"','01','"+ textFromTextBox.Replace(",",".") +"'";		
        //     query1 = "EXEC palace_db_real.dbo.sp_CalcPricingStone_hadis "+ Shape +","+ Carat +","+ Colour +","+ Clarity +","+ Cutting +","+ Brand +",'"+ order.SelectedValue +"','"+ orderby.SelectedValue +"','99','"+ textFromTextBox.Replace(",",".") +"'";		
        // }
        

		 string database = string.Empty;
        switch (BrandC.SelectedValue)
        {
            case "P": database = "palace_db_real.dbo"; break;
            case "M": database = "mondial_db_real.dbo"; break;
            case "F": database = "dbo"; break;
        }

		string query = BuildQuery(database);
        string query1 = BuildQuery(database);
        var parameters = new Dictionary<string, object>
        {
            {"@Shape", Shape},
            {"@Carat", Carat},
            {"@Colour", Colour},
            {"@Clarity", Clarity},
            {"@Cutting", Cutting},
            {"@Brand", Brand},
            {"@Order", order.SelectedValue},
            {"@OrderBy", orderby.SelectedValue},
            {"@RekapBy", "01"},
            {"@caratbutir", textFromTextBox.Replace(",", ".")},
            {"@Category", Category}
        };
        DataTable rs1 = Exec(Db.Cnn_DB,query,parameters);
        // DataTable rs1 = Db.Rs(query, Db.Cnn_DB);                
        
        int index = 1;
        try
        {
            foreach (DataRow rss in rs1.Rows)            
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" +  (string)rss["PLU"]+ "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" +  (string)rss["Kategori"]+ "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + (string)rss["GIA"] + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + (string)rss["Status"] + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + (string)rss["NamaLokasi"] + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + (string)rss["Shape"] + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + (string)rss["Size"] + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + (string)rss["Colour"] + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + (string)rss["Clarity"] + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + (string)rss["StoneBrand"] + "</p>";
                tr.Cells.Add(td);                

                int butir = Convert.ToInt32(rss["TotalButir"]);

                td = new TableCell();
                td.Text = butir.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ((decimal)rss["CaratButir"]).ToString("N3"); 
                tr.Cells.Add(td);

                string x = string.Empty;

			    x += "<p class=\"text-info\">" + ((decimal)rss["InputH"]).ToString("N2") + "</p>";
			    x += "<p class=\"text-info\">" + ((decimal)rss["InputP"]).ToString("N2") + "</p>";

                decimal InputH = (decimal)rss["InputH"];
                decimal InputP = ((decimal)rss["InputP"] / 100);
                decimal totCarat = (decimal)rss["CaratButir"];
                decimal persenan1 = (InputH * InputP);
                decimal persenan = (InputH +persenan1);
                decimal value = (persenan * totCarat);

                td = new TableCell();
                td.Text = x; 
                tr.Cells.Add(td);

                td = new TableCell();
                // td.Text = "<p class=\"text-success\">" + (InputP).ToString("N2") + "</p>";                
                td.Text = "<p class=\"text-success\">" + ((decimal)value).ToString("N2") + "</p>";                
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

            }
        
        parameters["@RekapBy"] = "99";
        DataTable rs2 = Exec(Db.Cnn_DB,query1,parameters);
        var sssss = "EXEC dbo.sp_CalcPricingStone_hadis "+ Shape +","+ Carat +","+ Colour +","+ Clarity +","+ Cutting +","+ Brand +",'"+ order.SelectedValue +"','"+ orderby.SelectedValue +"','99','"+ textFromTextBox.Replace(",",".") +"'";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "console.log([`"+ sssss +"`])", true);                
        // DataTable rs2 = Db.Rs(query1, Db.Cnn_DB);     
        int index2 = 1;
        foreach (DataRow rss1 in rs2.Rows)        
            {
                TableRow tr1 = new TableRow();
                
                TableCell td1;

                td1 = new TableCell();
                td1.Text = "<p>" + index2++.ToString() + "</p>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<p>" +  (string)rss1["Kategori"]+ "</p>";
                td1.HorizontalAlign = HorizontalAlign.Center; 
                tr1.Cells.Add(td1);
            
                string x = string.Empty;
                string y = string.Empty;

			    x += "<p class=\"text-info\">" + ((decimal)rss1["InputH"]).ToString("N2") + "</p>";
			    y += "<p class=\"text-info\">" + ((decimal)rss1["InputP"]).ToString("N2") + "</p>";


                td1 = new TableCell();
                td1.Text = x; 
                td1.HorizontalAlign = HorizontalAlign.Center; 
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = y; 
                td1.HorizontalAlign = HorizontalAlign.Center; 
                tr1.Cells.Add(td1);

                decimal InputH = (decimal)rss1["InputH"];
                decimal InputP = ((decimal)rss1["InputP"] / 100);
                decimal persenan2 = (InputH*InputP);
                decimal persenan = (InputH+persenan2);

                decimal persenan1 = persenan;
                // float persenan1 = float.Parse(persenan.ToString());
                decimal floatValue = Convert.ToDecimal(textFromTextBox);
                // float floatValue = Convert.ToDecimal(textFromTextBox, CultureInfo.InvariantCulture);

                td1 = new TableCell();
                td1.Text = "<p class=\"text-success\">" + ((decimal)persenan).ToString("N2") + "</p>";                
                td1.HorizontalAlign = HorizontalAlign.Center; 
                tr1.Cells.Add(td1);

                decimal value = (persenan * floatValue);

                td1 = new TableCell();                
                td1.Text = "<p class=\"text-success\">" + (value).ToString("N2") + "</p>";                
                // td1.Text = "<p class=\"text-success\">" + ((decimal)value).ToString("N2") + "</p>";                
                td1.HorizontalAlign = HorizontalAlign.Center; 
                tr1.Cells.Add(td1);

                tb1.Rows.Add(tr1);

            }

        }
        catch (System.Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "console.log([`"+ ex.Message +"`])", true);                
        }
        
	}
    private string BuildQuery(string database)
    {        
        return string.Format("{0}.sp_CalcPricingStone_hadis ",database);
    }
    
    protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
        href += "&kw=" + kw.Text;		
		href += "&parcel01=" + parcel01.SelectedValue;
		href += "&parcel02=" + parcel02.SelectedValue;
		href += "&parcel03=" + parcel03.SelectedValue;
		href += "&parcel04=" + parcel04.SelectedValue;
		href += "&parcel05=" + parcel05.SelectedValue;
		href += "&parcel06=" + parcel06.SelectedValue;		
		href += "&parcel07=" + parcel07.SelectedValue;		
        href += "&BrandC=" + BrandC.SelectedValue;		
        href += "&order=" + order.SelectedValue;
		href += "&orderby=" + orderby.SelectedValue;

		Response.Redirect(href);
	}
    private DataTable Exec(string connectionString, string storedProcedure, Dictionary<string, object> parameters)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable result = new DataTable();
                adapter.Fill(result);
                return result;
            }
        }
    }
}