using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;

public partial class KodeKaretCleansingHome : System.Web.UI.Page
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

            fillData();
        }
    }

    protected void btnDisplay_Click(object sender, EventArgs e)
    {

    }

    protected void fillData()
    {
        var d = db.KodeKaretCleansing.Where(p => p.OperatorTgl >= System.DateTime.Now.AddMonths(-3));

        d = d.OrderByDescending(p => p.OperatorTgl);

        if (d.Count() == 0) { App.TableNoResult(tbl); }
        else
        {
            int index = 1;
            foreach (var r in d)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_Hyperlink(r.ID);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = index++ + ".";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                if (r.Sumber == 0)
                {
                    td.Text = "Manual";
                }
                else if (r.Sumber == 1)
                {
                    td.Text = "SPK";
                }
                else if (r.Sumber == 2)
                {
                    td.Text = "Outsource";
                }
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharProductItem.Nama + " (" + r.CharProductItem.NamaKode + ")";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "";
                //td.Style.Add("text-align", "center");
                //tr.Cells.Add(td);

                td = new TableCell();
                if (r.DocStatus == 0)
                {
                    td.Text = "Draft";
                }
                else if (r.DocStatus == 1)
                {
                    td.Text = "Waiting for Approval";
                }
                else if (r.DocStatus == 2)
                {
                    td.Text = "Approved";
                }
                else if (r.DocStatus == 3)
                {
                    td.Text = "Rejected";
                }
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Operator + "<br />" + Str.FullDateJam(r.OperatorTgl);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (r.Approval != null) ? (r.Approval + "<br />" + Str.FullDateJam(r.ApprovalTgl)) : "";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                tbl.Rows.Add(tr);

            }




        }
    }


    protected string TD_Hyperlink(int id)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-xs btn-info\" href=\"KodeKaretCleansingAddDetail.aspx?id=" + id + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "";
        x += "</a>";

        return x;
    }


}