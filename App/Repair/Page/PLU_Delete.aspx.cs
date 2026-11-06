using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_DJ : System.Web.UI.Page {
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

		if (!IsPostBack) {

			
		}
	}

    protected void deletePLU(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipe.SelectedValue);
        if (kw.Text != "")
        {
            if (Tipe == 1)
            {
                var u = from p in db.StockProductDJ
                        where p.Nomor.Equals(kw.Text) && p.Legacy == 1
                        select p;
                if (u.Count() > 0)
                {
                    foreach (var d in u)
                    {
                        foreach (var r in d.DocQCDJ)
                        {
                            Db.Execute("DELETE FROM DocQCDJ_Stone1A WHERE IDForm=" + r.ID, Db.Cnn_DB);
                            Db.Execute("DELETE FROM DocQCDJ_Stone1B WHERE IDForm=" + r.ID, Db.Cnn_DB);
                            Db.Execute("DELETE FROM DocQCDJ_Stone2 WHERE IDForm=" + r.ID, Db.Cnn_DB);
                            Db.Execute("DELETE FROM DocQCDJ_Stone3 WHERE IDForm=" + r.ID, Db.Cnn_DB);
                            Db.Execute("DELETE FROM DocQCDJ_Stone4 WHERE IDForm=" + r.ID, Db.Cnn_DB);
                            Db.Execute("DELETE FROM DocQCDJ_Stone5 WHERE IDForm=" + r.ID, Db.Cnn_DB);
                            Db.Execute("DELETE FROM ResellDJ WHERE IDDocQC=" + r.ID, Db.Cnn_DB);
                            Db.Execute("DELETE FROM DocQCDJ WHERE ID=" + r.ID, Db.Cnn_DB);
                        }

                        Db.Execute("DELETE FROM StockProductDJ_CharDesign WHERE ID=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_CharProduct WHERE ID=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_Costing WHERE ID=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_CostingProduct WHERE ID=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_Finishing WHERE IDForm=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_PricingBiaya WHERE ID=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_PricingMU WHERE ID=" + d.ID, Db.Cnn_DB);

                        Db.Execute("DELETE FROM StockProductDJ_PricingProduct WHERE ID=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_PricingSegmen WHERE ID=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_Stone1A WHERE IDForm=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_Stone1B WHERE IDForm=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_Stone2 WHERE IDForm=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_Stone3 WHERE IDForm=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_Stone4 WHERE IDForm=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockProductDJ_Stone5 WHERE IDForm=" + d.ID, Db.Cnn_DB);

                        

                        Db.Execute("DELETE FROM ResellDJ WHERE IDProduct=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockActualDJ WHERE IDProduct=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM SalesOrderDJ WHERE IDProduct=" + d.ID, Db.Cnn_DB);

                        db.StockProductDJ.DeleteOnSubmit(d);
                        db.SubmitChanges();
                    }
                    Response.Redirect("PLU_Delete.aspx?d=1");
                }
            }
            else if (Tipe == 2)
            {
                var u = from p in db.StockProductPG
                        where p.Nomor.Equals(kw.Text) && p.Legacy == 1
                        select p;
                if (u.Count() > 0)
                {
                    foreach (var d in u)
                    {
                        foreach (var r in d.DocQCPG)
                        {
                            Db.Execute("DELETE FROM ResellPG WHERE IDDocQC=" + r.ID, Db.Cnn_DB);
                            Db.Execute("DELETE FROM DocQCPG WHERE ID=" + r.ID, Db.Cnn_DB);

                            

                            
                        }
                        Db.Execute("DELETE FROM ResellPG WHERE IDProduct=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockActualPG WHERE IDProduct=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM SalesOrderPG WHERE IDProduct=" + d.ID, Db.Cnn_DB);

                        db.StockProductPG.DeleteOnSubmit(d);
                        db.SubmitChanges();
                    }
                    Response.Redirect("PLU_Delete.aspx?d=1");
                }
            }
            else if (Tipe == 3)
            {
                var u = from p in db.StockProductLD_Stone1B
                        where p.Nomor.Equals(kw.Text)
                        select p;
                if (u.Count() > 0)
                {
                    foreach (var d in u)
                    {
                        foreach (var r in d.DocQCLD)
                        {
                            Db.Execute("DELETE FROM ResellLD WHERE IDDocQC=" + r.ID, Db.Cnn_DB);
                            Db.Execute("DELETE FROM DocQCLD WHERE ID=" + r.ID, Db.Cnn_DB);




                        }
                        Db.Execute("DELETE FROM ResellLD WHERE IDProduct=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM StockActualLD_Stone1B WHERE IDProduct=" + d.ID, Db.Cnn_DB);
                        Db.Execute("DELETE FROM SalesOrderLD WHERE IDProduct=" + d.ID, Db.Cnn_DB);

                        db.StockProductLD_Stone1B.DeleteOnSubmit(d);
                        db.SubmitChanges();
                    }
                    Response.Redirect("PLU_Delete.aspx?d=1");
                }
            }
        }
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            
            kw.Text = Request.QueryString["kw"];
        }
    }
}