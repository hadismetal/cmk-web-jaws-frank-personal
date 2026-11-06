using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.Text;
using System.Reflection;
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

public partial class TestWeb : System.Web.UI.Page
{
    private Map_AP _ap;

    private Map_AP ap
    {
        get
        {
            if (_ap == null)
                _ap = new Map_AP();
            return _ap;
        }
    }

    public override void Dispose()
    {
        if (ap != null)
            ap.Dispose();
        base.Dispose();
    }
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //protected DataTable TabelTest()
    //{
    //    var dt = from p in ac.UserAccount select p;
    //    return ToDataTable(dt);
    //}
    //private string DataTableJson()
    //{
    //    DataTable dt = TabelTest();
    //    return (DataTableToJson(dt));
    //}
    private string DataTableToJson(DataTable table)
    {
        var JSONString = new StringBuilder();
        if (table.Rows.Count > 0)
        {
            JSONString.Append("[");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                JSONString.Append("{");
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (j < table.Columns.Count - 1)
                    {
                        JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                    }
                    else if (j == table.Columns.Count - 1)
                    {
                        JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                    }
                }
                if (i == table.Rows.Count - 1)
                {
                    JSONString.Append("}");
                }
                else
                {
                    JSONString.Append("},");
                }
            }
            JSONString.Append("]");
        }
        return JSONString.ToString();
    }

    public DataTable ToDataTable<T>(/*this IEnumerable<T> items*/)
    {
        var tb = new DataTable(typeof(T).Name);
        PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in props)
        {
            tb.Columns.Add(prop.Name, prop.PropertyType);
        }

        //foreach (var item in items)
        //{
        //    var values = new object[props.Length];
        //    for (var i = 0; i < props.Length; i++)
        //    {
        //        values[i] = props[i].GetValue(item, null);
        //    }

        //    tb.Rows.Add(values);
        //}
        return tb;
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            textSearch.Text = Request.QueryString["kw"];

        }
    }

    protected void fillTable(object sender, EventArgs e)
    {
        //var attach = from p in ap.Attach
        //             join q in ap.AttachBinary on p.ID equals q.ID
        //             select new { Post = p, Qust = q };
        var attach = from p in ap.Attach
                     select new { Post = p };
        if (textSearch.Text != "")
            attach = from p in ap.Attach
                     where (p.Tgl.ToString() + p.DataTable + p.Author + p.NamaFile).Contains(textSearch.Text)
                     select new { Post = p };
        int index = 1;
        HeaderID.Style.Add("display", "none");
        foreach (var r in attach)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = r.Post.ID.ToString();
            td.Style.Add("display", "none");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = index.ToString();
            index++;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Post.DataTable;
            tr.Cells.Add(td);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Post.Tgl.ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Post.Author;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Post.NamaFile;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<button class='btn btn-primary text-center' type='button' id='btnUpdate' data-id='" + r.Post.ID + "' data-toggle='modal' data-target='#myModalUpdate' onclick='onclickbutton(this);' >Update</button> ";
            td.Text = td.Text + "<button class='btn btn-warning text-center' type='button' id='btnDelete' data-id='"+r.Post.ID+ "' data-action='update' data-toggle='modal' data-target='#myModalDelete' onclick='onclickbutton(this);' >Delete</button> ";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);
        //string
        href += "&kw=" + textSearch.Text;

        Response.Redirect(href);
    }
    protected void btnOKUpdate_ServerClick(object sender, EventArgs e)
    {
        var d = ap.Attach.SingleOrDefault(p => p.ID == int.Parse(idTable.Value));

        //DataTable
        d.DataTable = txtDataTableUpdate.Value;

        //Tanggal
        d.Tgl = DateTime.Now;

        //Author
        d.Author = txtAuthorUpdate.Value;

        //NamaFile
        d.NamaFile = txtNamaFileUpdate.Value;
        

        //Commit
        ap.SubmitChanges();

        Response.Redirect(App.RefreshURL(this));
    }
    
    protected void btnOKAdd_ServerClick(object sender, EventArgs e)
    {
        Attach n = new Attach();
        //DataID
        n.DataID = new Random().Next(20,30);

        //Module
        n.Module = "DataCustomer";

        //DataTable
        n.DataTable = txtDataTableAdd.Value;

        //Tanggal
        n.Tgl = DateTime.Now;

        //Author
        n.Author = txtAuthorAdd.Value;

        //NamaFile
        n.NamaFile = txtNamaFileAdd.Value;

        //ContentType
        n.ContentType = "image";

        //ContentLength
        n.ContentLength = new Random().Next(10000, 20000);
        
        //Commit
        ap.Attach.InsertOnSubmit(n);
        ap.SubmitChanges();

        Response.Redirect(App.RefreshURL(this));
    }
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        var d = ap.Attach.SingleOrDefault(p => p.ID == int.Parse(idTable1.Value));

        ap.Attach.DeleteOnSubmit(d);
        ap.SubmitChanges();

        Response.Redirect(App.RefreshURL(this));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        refreshPage(sender, e);
    }
}