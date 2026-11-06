using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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
        fillTable(sender, e);
	}

    protected void fillTable(object sender, EventArgs e)
    {
        fillBulk();
        fillDJ();
        fillGJ();
        fillPG();
        fillLD();
    }
    protected void fillBulk()
    {
        var rs = from p in db.StockAudit
                 where p.StatusFinish == false

                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.TipeLokasi == d.First().TipeLokasi && p.IDLokasi == d.First().IDLokasi);
        }

        rs = rs.OrderByDescending(p => p.OperatorTgl);

        if (rs != null)
        {
            int index = 1;

            var _locWarehouseDict = db.LocWarehouse.ToDictionary(p => p.ID);

            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);

            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);

            foreach (var r in rs)
            {


                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ID.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(r.TipeLokasi);
                tr.Cells.Add(td);

                string NamaLokasi = "ALL";
                try
                {
                    if (Convert.ToInt32(r.IDLokasi) == 0)
                        NamaLokasi = "ALL";
                    else
                    {
                        if (r.TipeLokasi == 0)
                        {
                            var w = (_locWarehouseDict.ContainsKey(r.IDLokasi) ? _locWarehouseDict[r.IDLokasi] : null);
                            NamaLokasi = w.Nama;
                        }
                        if (r.TipeLokasi == 1)
                        {
                            var o = (_locOutletDict.ContainsKey(r.IDLokasi) ? _locOutletDict[r.IDLokasi] : null);
                            NamaLokasi = o.Nama;
                        }
                        if (r.TipeLokasi == 2)
                        {
                            var exh = (_locExhibitionDict.ContainsKey(r.IDLokasi) ? _locExhibitionDict[r.IDLokasi] : null);
                            NamaLokasi = exh.Nama;
                        }
                    }
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = NamaLokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Operator;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(r.TotalItemAudit);
                tr.Cells.Add(td);

                int jlh = 0;
                jlh += r.StockAudit_Detail.First().StockAuditDJ.StockAuditDJDetail.Count();
                jlh += r.StockAudit_Detail.First().StockAuditGJ.StockAuditGJDetail.Count();
                jlh += r.StockAudit_Detail.First().StockAuditPG.StockAuditPGDetail.Count();
                jlh += r.StockAudit_Detail.First().StockAuditLD.StockAuditLDDetail.Count();

                td = new TableCell();
                td.Text = Str.Number0(jlh);
                tr.Cells.Add(td);

                tb5.Rows.Add(tr);

                index++;

            }


        }
    }
    protected void fillDJ()
    {
        var rs = from p in db.StockAuditDJ
                 where p.StatusFinish == false

                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.TipeLokasi == d.First().TipeLokasi && p.IDLokasi == d.First().IDLokasi);
        }

        rs = rs.OrderByDescending(p => p.OperatorTgl);

        if (rs != null)
        {
            int index = 1;

            var _locWarehouseDict = db.LocWarehouse.ToDictionary(p => p.ID);

            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);

            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);

            foreach (var r in rs)
            {


                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ID.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(r.TipeLokasi);
                tr.Cells.Add(td);

                string NamaLokasi = "ALL";
                try
                {
                    if (Convert.ToInt32(r.IDLokasi) == 0)
                        NamaLokasi = "ALL";
                    else
                    {
                        if (r.TipeLokasi == 0)
                        {
                            var w = (_locWarehouseDict.ContainsKey(r.IDLokasi) ? _locWarehouseDict[r.IDLokasi] : null);
                            NamaLokasi = w.Nama;
                        }
                        if (r.TipeLokasi == 1)
                        {
                            var o = (_locOutletDict.ContainsKey(r.IDLokasi) ? _locOutletDict[r.IDLokasi] : null);
                            NamaLokasi = o.Nama;
                        }
                        if (r.TipeLokasi == 2)
                        {
                            var exh = (_locExhibitionDict.ContainsKey(r.IDLokasi) ? _locExhibitionDict[r.IDLokasi] : null);
                            NamaLokasi = exh.Nama;
                        }
                    }
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = NamaLokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Operator;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(r.TotalItemAudit);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(r.StockAuditDJDetail.Count());
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;

            }


        }
    }
    protected void fillGJ()
    {
        var rs = from p in db.StockAuditGJ
                 where p.StatusFinish == false

                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.TipeLokasi == d.First().TipeLokasi && p.IDLokasi == d.First().IDLokasi);
        }

        rs = rs.OrderByDescending(p => p.OperatorTgl);

        if (rs != null)
        {
            int index = 1;

            var _locWarehouseDict = db.LocWarehouse.ToDictionary(p => p.ID);

            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);

            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);

            foreach (var r in rs)
            {


                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ID.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(r.TipeLokasi);
                tr.Cells.Add(td);

                string NamaLokasi = "ALL";
                try
                {
                    if (Convert.ToInt32(r.IDLokasi) == 0)
                        NamaLokasi = "ALL";
                    else
                    {
                        if (r.TipeLokasi == 0)
                        {
                            var w = (_locWarehouseDict.ContainsKey(r.IDLokasi) ? _locWarehouseDict[r.IDLokasi] : null);
                            NamaLokasi = w.Nama;
                        }
                        if (r.TipeLokasi == 1)
                        {
                            var o = (_locOutletDict.ContainsKey(r.IDLokasi) ? _locOutletDict[r.IDLokasi] : null);
                            NamaLokasi = o.Nama;
                        }
                        if (r.TipeLokasi == 2)
                        {
                            var exh = (_locExhibitionDict.ContainsKey(r.IDLokasi) ? _locExhibitionDict[r.IDLokasi] : null);
                            NamaLokasi = exh.Nama;
                        }
                    }
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = NamaLokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Operator;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(r.TotalItemAudit);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(r.StockAuditGJDetail.Count());
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                index++;

            }
        }
    }
    protected void fillPG()
    {
        var rs = from p in db.StockAuditPG
                 where p.StatusFinish == false

                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.TipeLokasi == d.First().TipeLokasi && p.IDLokasi == d.First().IDLokasi);
        }

        rs = rs.OrderByDescending(p => p.OperatorTgl);

        if (rs != null)
        {
            int index = 1;

            var _locWarehouseDict = db.LocWarehouse.ToDictionary(p => p.ID);

            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);

            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);

            foreach (var r in rs)
            {


                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ID.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(r.TipeLokasi);
                tr.Cells.Add(td);

                string NamaLokasi = "ALL";
                try
                {
                    if (Convert.ToInt32(r.IDLokasi) == 0)
                        NamaLokasi = "ALL";
                    else
                    {
                        if (r.TipeLokasi == 0)
                        {
                            var w = (_locWarehouseDict.ContainsKey(r.IDLokasi) ? _locWarehouseDict[r.IDLokasi] : null);
                            NamaLokasi = w.Nama;
                        }
                        if (r.TipeLokasi == 1)
                        {
                            var o = (_locOutletDict.ContainsKey(r.IDLokasi) ? _locOutletDict[r.IDLokasi] : null);
                            NamaLokasi = o.Nama;
                        }
                        if (r.TipeLokasi == 2)
                        {
                            var exh = (_locExhibitionDict.ContainsKey(r.IDLokasi) ? _locExhibitionDict[r.IDLokasi] : null);
                            NamaLokasi = exh.Nama;
                        }
                    }
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = NamaLokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Operator;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(r.TotalItemAudit);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(r.StockAuditPGDetail.Count());
                tr.Cells.Add(td);

                tb3.Rows.Add(tr);

                index++;

            }
        }
    }
    protected void fillLD()
    {
        var rs = from p in db.StockAuditLD
                 where p.StatusFinish == false

                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.TipeLokasi == d.First().TipeLokasi && p.IDLokasi == d.First().IDLokasi);
        }

        rs = rs.OrderByDescending(p => p.OperatorTgl);

        if (rs != null)
        {
            int index = 1;

            var _locWarehouseDict = db.LocWarehouse.ToDictionary(p => p.ID);

            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);

            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);

            foreach (var r in rs)
            {


                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ID.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(r.TipeLokasi);
                tr.Cells.Add(td);

                string NamaLokasi = "ALL";
                try
                {
                    if (Convert.ToInt32(r.IDLokasi) == 0)
                        NamaLokasi = "ALL";
                    else
                    {
                        if (r.TipeLokasi == 0)
                        {
                            var w = (_locWarehouseDict.ContainsKey(r.IDLokasi) ? _locWarehouseDict[r.IDLokasi] : null);
                            NamaLokasi = w.Nama;
                        }
                        if (r.TipeLokasi == 1)
                        {
                            var o = (_locOutletDict.ContainsKey(r.IDLokasi) ? _locOutletDict[r.IDLokasi] : null);
                            NamaLokasi = o.Nama;
                        }
                        if (r.TipeLokasi == 2)
                        {
                            var exh = (_locExhibitionDict.ContainsKey(r.IDLokasi) ? _locExhibitionDict[r.IDLokasi] : null);
                            NamaLokasi = exh.Nama;
                        }
                    }
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = NamaLokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Operator;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(r.TotalItemAudit);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(r.StockAuditLDDetail.Count());
                tr.Cells.Add(td);

                tb4.Rows.Add(tr);

                index++;

            }
        }
    }
    protected string TD_HyperLink(StockAudit r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Register/Bulk_Opname.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        //x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"Bulk_Delete.aspx?id=" + r.ID + "\">";
        //x += "  <i class=\"icon-trash\"></i> ";
        //x += "  Delete";
        //x += "</a>";

        return x;
    }
    protected string TD_HyperLink(StockAuditDJ r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Register/DJ_Opname.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        //x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"DJ_Delete.aspx?id=" + r.ID + "\">";
        //x += "  <i class=\"icon-trash\"></i> ";
        //x += "  Delete";
        //x += "</a>";

        return x;
    }
    protected string TD_HyperLink(StockAuditGJ r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Register/GJ_Opname.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        //x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"GJ_Delete.aspx?id=" + r.ID + "\">";
        //x += "  <i class=\"icon-trash\"></i> ";
        //x += "  Delete";
        //x += "</a>";

        return x;
    }
    protected string TD_HyperLink(StockAuditPG r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Register/PG_Opname.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        //x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"PG_Delete.aspx?id=" + r.ID + "\">";
        //x += "  <i class=\"icon-trash\"></i> ";
        //x += "  Delete";
        //x += "</a>";

        return x;
    }
    protected string TD_HyperLink(StockAuditLD r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Register/LD_Opname.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        //x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"LD_Delete.aspx?id=" + r.ID + "\">";
        //x += "  <i class=\"icon-trash\"></i> ";
        //x += "  Delete";
        //x += "</a>";

        return x;
    }
}