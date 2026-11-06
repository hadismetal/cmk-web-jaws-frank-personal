using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page
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
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);

            datepicker.Text = Str.Tgl(DateDrv.AwalBulan());
            datepicker2.Text = Str.Tgl(DateTime.Now);

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        err.InnerHtml = "PLU Tidak ditemukan.";
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.DocRepairResult
                 where p.IDProduct != null && !p.Draft.Value && p.Sumber == 1
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end.AddDays(1));
        }

        if (kw.Text != "")
        {
            rs = rs.Where(p => p.Nomor.Contains(kw.Text) || p.StockProductDJ.Nomor.Contains(kw.Text));
        }
        
        
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == ProductCategory);

        rs = rs.OrderBy(p => p.Tgl).ThenBy(p => p.Nomor);

        if (rs != null)
        {
            string gw1 = "";
            string gw2 = "";
            string gw3 = "";
            string nw1 = "";
            string nw2 = "";
            string nw3 = "";
            string tb1 = "";
            string tb2 = "";
            string tb3 = "";
            string tc1 = "";
            string tc2 = "";
            string tc3 = "";

            int index = 1;
            foreach (var r in rs)
            {
                try
                {
                    gw1 = Convert.ToDecimal(r.DocRepairResult_CharProduct.GrossWeightOld.Value).ToString("N2");
                    gw2 = Convert.ToDecimal(r.DocRepair.DocRepair_CharProduct.GrossWeight.Value).ToString("N2");
                    gw3 = Convert.ToDecimal(r.DocRepairResult_CharProduct.GrossWeight.Value).ToString("N2");

                    nw1 = Convert.ToDecimal(r.DocRepairResult_CharProduct.NettoWeightOld.Value).ToString("N2");
                    nw2 = Convert.ToDecimal(r.DocRepair.DocRepair_CharProduct.NettoWeight.Value).ToString("N2");
                    nw3 = Convert.ToDecimal(r.DocRepairResult_CharProduct.NettoWeight.Value).ToString("N2");

                    tb1 = Convert.ToDecimal(r.DocRepairResult_CharProduct.StoneQtyOld.Value).ToString("N0");
                    tb2 = Convert.ToDecimal(r.DocRepair.DocRepair_CharProduct.StoneQty.Value).ToString("N0");
                    tb3 = Convert.ToDecimal(r.DocRepairResult_CharProduct.StoneQty.Value).ToString("N0");

                    tc1 = Convert.ToDecimal(r.DocRepairResult_CharProduct.StoneCaratOld.Value).ToString("N3");
                    tc2 = Convert.ToDecimal(r.DocRepair.DocRepair_CharProduct.StoneCarat.Value).ToString("N3");
                    tc3 = Convert.ToDecimal(r.DocRepairResult_CharProduct.StoneCarat.Value).ToString("N3");
                }
                catch (Exception ex) { }

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
				td = new TableCell();
				if(r.StatusRepair != 2){
					td.Text = TD_HyperlinkPrint(r.DocRepair, 1)
							+ "&nbsp;&nbsp;&nbsp;"
							+ TD_HyperlinkPrint(r.DocRepair, 2)
					;
				}
				else{
					td.Text = "";
				}
				
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.DocRepair.Nomor;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Hyperlink_Invoice(r);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
                td = new TableCell();
                td.Text = TD_Hyperlink_PLU(r);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductLevel.NamaKode;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.Tgl);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Operator;
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                // Gross
                td = new TableCell();
                td.Text = gw1;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = gw2;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = gw3;
                tr.Cells.Add(td);

                // Netto
                td = new TableCell();
                td.Text = nw1;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = nw2;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = nw3;
                tr.Cells.Add(td);

                // Stone Qty
                td = new TableCell();
                td.Text = tb1;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = tb2;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = tb3;
                tr.Cells.Add(td);

                // Stone Carat
                td = new TableCell();
                td.Text = tc1;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = tc2;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = tc3;
                tr.Cells.Add(td);
				
				td = new TableCell();
				if(r.StatusRepair != 2)
					td.Text = "<p style='color:blue'>Ongoing</p>";
				else
					td.Text = "<p style='color:green'>Finish</p>";
				tr.Cells.Add(td);
				
				td = new TableCell();
				if(r.StatusRepair != 2)
					td.Text = TD_HyperlinkUpdate(r.DocRepair);
				else
					td.Text = "";
				tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }
        }
    }

    protected string TD_Hyperlink_PLU(DocRepairResult d)
    {
        string x = string.Empty;

        x += "<a target='_blank' href='Repair_Item.aspx?id=" + d.StockProductDJ.ID + "'>" + d.StockProductDJ.Nomor + "</a>";

        return x;
    }

    protected string TD_Hyperlink_Invoice(DocRepairResult d)
    {
        string x = string.Empty;

        x += "<a target='_blank' href='Repair_Detail.aspx?id=" + d.ID + "'>" + d.Nomor + "</a>";

        return x;
    }
	protected string TD_HyperlinkPrint(DocRepair d, int mode = 1)
    {

        string x = string.Empty;

        if (mode == 1)
        {
            x += "<a class=\"btn btn-warning\" target='_blank' href='Repair_Print.aspx?id=" + d.ID + "'>";
            x += "  <i class=\"icon-print\"></i> ";
            x += "  ";
            x += "</a>";
        }
        else if (mode == 2)
        {
            x += "<a class=\"btn btn-info\" target='_blank' href='Repair_Print2.aspx?id=" + d.ID + "'>";
            x += "  <i class=\"icon-print\"></i> ";
            x += "  ";
            x += "</a>";
        }

        return x;
    }
	protected string TD_HyperlinkUpdate(DocRepair d)
	{
		string x = string.Empty;

        x += "<a target='_blank' class='btn btn-warning text-center' href='../Update/Repair/Repair_Data.aspx?id=" + d.ID + "'>Update</a>";

        return x;
	}
}