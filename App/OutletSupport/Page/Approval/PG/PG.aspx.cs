using System;
using System.Linq;
using System.Linq.Dynamic;
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
            //Role Protection
            App.TableRole(tb, Role.Operator);

            //Generate

        }
        fillTable(sender, e);
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        var rs = from p in db.PromoPG
                 orderby p.DraftDate
                 where p.Draft == false && p.ApprovalTgl == null
                 select p;

        //NO-RESULT
        zero.Visible = rs.Count() == 0;
        form_area.Visible = !zero.Visible;

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(r);
            tr.Cells.Add(td);

            CheckBox cb = new CheckBox();
            cb.ID = "cb_po" + index.ToString();
            cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            HiddenField hf = new HiddenField();
            hf.ID = "hf" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(cb);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (r.StatusASP.Value == 1)
                    //? "<span style='font-size:20px; color:red;' class='glyphicon glyphicon glyphicon-remove' aria-hidden='true'></span>"
                    ? ""
                    : "<span style='font-size:20px; color:green;' class='glyphicon glyphicon glyphicon-ok' aria-hidden='true'></span>"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (r.StatusDiscMembership.Value == true)
                    //? "<span style='font-size:20px; color:red;' class='glyphicon glyphicon glyphicon-remove' aria-hidden='true'></span>"
                    ? "" 
                    : "<span style='font-size:20px; color:green;' class='glyphicon glyphicon glyphicon-ok' aria-hidden='true'></span>"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (r.StatusDiscNormal.Value == true)
                    //? "<span style='font-size:20px; color:red;' class='glyphicon glyphicon glyphicon-remove' aria-hidden='true'></span>"
                    ? ""
                    : "<span style='font-size:20px; color:green;' class='glyphicon glyphicon glyphicon-ok' aria-hidden='true'></span>"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Operator(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TglStart(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TglExpire(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DL.PromoStatus(r.Tipe.Value);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_DiscountPersen(r);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_DiscountNominal(r);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Point(r);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_PromoHarga(r);
            td.CssClass = "text-right";
            tr.Cells.Add(td);


            tb.Rows.Add(tr);

            //Process-ID
            process_id.Items.Add(r.ID.ToString());
        }
    }

    protected string TD_HyperLink(PromoPG r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"PG_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-pencil\"></i> ";
        x += "  Detail";
        x += "</a>";

        return x;
    }

    protected string TD_Nama(PromoPG r)
    {
        string x = string.Empty;

        x += "<p><strong>" + r.Nama + "</strong></p>";

        return x;
    }

    protected string TD_Operator(PromoPG r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.OperatorNama + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

        return x;
    }

    protected string TD_DiscountPersen(PromoPG r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += (r.DiscountPersen.HasValue) ? Str.Number2(r.DiscountPersen.Value) : "0";
        x += "</p>";

        return x;
    }

    protected string TD_DiscountNominal(PromoPG r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += (r.DiscountNominal.HasValue) ? Str.Number0(r.DiscountNominal.Value) : "0";
        x += "</p>";

        return x;
    }
    protected string TD_Point(PromoPG r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += (r.PoinReward.HasValue) ? Str.Number0(r.PoinReward.Value) : "0";
        x += "</p>";

        return x;
    }
    protected string TD_PromoHarga(PromoPG r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += (r.PromoHarga.HasValue) ? Str.Number0(r.PromoHarga.Value) : "0";
        x += "</p>";

        return x;
    }
    protected string TD_TglStart(PromoPG r)
    {

        string x = string.Empty;

        x += "<p>" + Str.FullDate(r.TglStart) + "</p>";

        return x;
    }

    protected string TD_TglExpire(PromoPG r)
    {

        string x = string.Empty;

        x += "<p>" + Str.FullDate(r.TglExpire) + "</p>";

        return x;
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        //var rs = from p in process_id.Items.Cast<ListItem>()
        //		 select p.Value;

        //foreach (var r in rs) {
        CheckBox cb = new CheckBox();
        string hf = "";
        int index = 1;
        foreach (TableRow row in tb.Rows)
        {

            try
            {
                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                cb = (CheckBox)row.FindControl("cb_po" + index.ToString());
                if (cb.Checked == true)
                {
                    int myID = Convert.ToInt32(hf);

                    //Response.Redirect(myID.ToString());

                    var d = db.PromoPG.SingleOrDefault(
                        p =>
                        p.ID == myID &&
                        p.Draft == false &&
                        p.ApprovalTgl == null
                        );

                    if (d != null)
                    {

                        d.ApprovalTgl = DateTime.Now;
                        d.ApprovalNama = App.UserNama;

                        //Commit

                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) { }
            index++;
        }

        //Redirect
        App.Reload(this);
    }
}