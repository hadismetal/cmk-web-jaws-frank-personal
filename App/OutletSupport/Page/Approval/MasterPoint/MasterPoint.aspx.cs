using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_UpdateProcess_MU : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }

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
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        var rs = from p in db.PointRedeem
                 where p.ApprovalTgl == null
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
            td.Text = Convert.ToDecimal(r.Point).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(r.PointPending);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Rupiah).ToString("N2");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.RupiahPending);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Operator(r);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Process-ID
            process_id.Items.Add(r.ID.ToString());
        }
    }

    protected string TD_Operator(PointRedeem r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

        return x;
    }

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

                    var d = db.PointRedeem.SingleOrDefault(
                        p =>
                        p.ID == myID &&
                        p.ApprovalTgl == null
                        );

                    if (d != null)
                    {
                        d.Point = d.PointPending;
                        d.Rupiah = d.RupiahPending;
                        d.PointPending = null;
                        d.RupiahPending = null;

                        d.ApprovalTgl = DateTime.Now;
                        d.Approval = App.UserNama;
                    }

                    //Commit

                    db.SubmitChanges();
                }
            }

            catch (Exception ex) { }
            index++;
        }

        //Redirect
        App.Reload(this);
    }

}