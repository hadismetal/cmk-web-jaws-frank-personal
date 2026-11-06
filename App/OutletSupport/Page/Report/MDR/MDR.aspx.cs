using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Page_Approval_Product : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {

            fillTable(sender, e);

        }
        else { 
            
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        var rs = from p in db.MDRRate
                 
                 //where p.Draft == false && p.ApprovalTgl == null && p.StatusActive == 1
                 select p;

        //rs = from p in rs
        //     where !(from z in db.Bundle_ItemMaster
        //             select z.IDForm).Contains(p.ID)
        //     select p;

        //NO-RESULT
        if(rs.Count() == 0)
            App.TableNoResult(tb);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DLC.EDC(r.IDBank);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DLC.JenisKartu(r.IDJenisKartuKredit);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DLC.Program(r.IDProgram);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.MDRRate1.ToString("N2");
            tr.Cells.Add(td);


            td = new TableCell();
            td.Style["text-align"] = "center";
            td.Text = TD_HyperLink(r.ID);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
            index++;
        }
    }

    protected string TD_HyperLink(int r)
    {
        string x = string.Empty;

        x += "<div class=\"btn-group\">";
        x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"EditMDR.aspx?id=" + r + "\">";
        x += "      <i class=\"icon-edit\"></i> ";
        x += "      Edit";
        x += "  </a>";
        x += "</div>";

        return x;
    }
}