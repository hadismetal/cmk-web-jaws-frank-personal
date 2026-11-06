using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Mounting_CharDesign02 : System.Web.UI.Page
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
    protected void DataError() { Response.Redirect("Mounting.aspx"); }
    protected void LinkError() { Response.Redirect("Mounting_CharDesign01.aspx?id=" + myID); }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/Mounting_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            if (d.Proto != null && d.Proto != 0)
            {
                var proto = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                if (proto != null)
                {
                    naming.Text = proto.ProductOrder.ProductOrder_CharDesign.Naming;
                    setitem.Checked = (proto.ProductOrder.ProductOrder_CharDesign.SetItem == true) ? true : false;
                    special.Checked = (proto.ProductOrder.ProductOrder_CharDesign.Special == true) ? true : false;
                    grafir.Text = proto.ProductOrder.ProductOrder_CharDesign.Grafir;

                    //DLC
                    DLC.CharProcessFinishingFromSPK(cb, Convert.ToInt32(proto.IDProductOrder));
                }
            }
            else
            {
                DLC.CharProcessFinishing(cb);
            }
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            var product = d.StockReceiveMounting_CharProduct;

            var x = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);

            //Process
            saveData(d);
            saveFinishing(d);

            //Recal Weight
            //Lib.RecalWeight(db, d, product);

            //Redirect
            Response.Redirect("Mounting_Costing.aspx?id=" + myID);
        }
    }

    //Data
    protected void saveData(StockReceiveMounting ms)
    {
        var d = ms.StockReceiveMounting_CharDesign;
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
    protected void saveFinishing(StockReceiveMounting d)
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
    protected void insertProcess(StockReceiveMounting ms, int myIDFinishing)
    {
        var d = ms.StockReceiveMounting_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d == null)
        {
            StockReceiveMounting_Finishing n = new StockReceiveMounting_Finishing();

            n.IDForm = ms.ID;
            n.IDFinishing = myIDFinishing;

            //Commit
            db.StockReceiveMounting_Finishing.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected void deleteProcess(StockReceiveMounting ms, int myIDFinishing)
    {
        var d = ms.StockReceiveMounting_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d != null)
        {
            //Commit
            db.StockReceiveMounting_Finishing.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
}