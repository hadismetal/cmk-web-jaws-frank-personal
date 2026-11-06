using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_GJ_CharDesign02 : System.Web.UI.Page
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
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            naming.Focus();

            //DLC
            //DLC.CharProcessFinishing(cb);

            //Form
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("GJ.aspx"); }
    protected void LinkError() { Response.Redirect("GJ_CharDesign01.aspx?id=" + myID); }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveGJ_CharDesign.SingleOrDefault(p => p.ID == myID && p.StockReceiveGJ.Approval==null);
        if (d == null)
        {
            var x = db.StockReceiveGJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
            if (x == null) DataError();
            header_nomor.InnerHtml =
                "<a href=\"../History/GJ_File.aspx?id=" + x.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + x.Nomor + "</a>";
        }
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/GJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.StockReceiveGJ.Nomor + "</a>";


            naming.Text = d.Naming;
            setitem.Checked = (d.SetItem == true) ? true : false;
            special.Checked = (d.Special == true) ? true : false;
            grafir.Text = d.Grafir;



            DLC.CharProcessFinishingFromDesign(cb,d.ID);
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveGJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
        if (d == null) DataError();
        else
        {
            var product = d.StockReceiveGJ_CharProduct;

            var x = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);

            //Process
            saveData(d);
            saveFinishing(d);
            /*if (d.Proto != null && d.Proto != 0)
                moveStone(d, x);*/

            //Recal Weight
            //Lib.RecalWeight(db, d, product);

            //Redirect
            Response.Redirect("GJ_Costing.aspx?id=" + myID);
        }
    }

    //Data
    protected void saveData(StockReceiveGJ ms)
    {
        var d = ms.StockReceiveGJ_CharDesign;
        if (d == null) LinkError();
        else
        {
            d.Naming = Str.CapTitle(naming.Text);
            d.Grafir = Str.CapUpper(grafir.Text);

            d.SetItem = setitem.Checked;
            d.Special = special.Checked;

            //Commit
            db.SubmitChanges();
        }
    }

    //Finishing
    protected void saveFinishing(StockReceiveGJ d)
    {
        var rs = from p in cb.Items.Cast<ListItem>()
                 select p;

        foreach (var r in rs)
        {
            int myIDFinishing = Convert.ToInt32(r.Value);
            if (r.Selected)
                insertProcess(d, myIDFinishing);
            else
                deleteProcess(d, myIDFinishing);
        }
    }
    protected void insertProcess(StockReceiveGJ ms, int myIDFinishing)
    {
        var d = ms.StockReceiveGJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d == null)
        {
            StockReceiveGJ_Finishing n = new StockReceiveGJ_Finishing();

            n.IDForm = ms.ID;
            n.IDFinishing = myIDFinishing;

            //Commit
            db.StockReceiveGJ_Finishing.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected void deleteProcess(StockReceiveGJ ms, int myIDFinishing)
    {
        var d = ms.StockReceiveGJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d != null)
        {
            //Commit
            db.StockReceiveGJ_Finishing.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
}