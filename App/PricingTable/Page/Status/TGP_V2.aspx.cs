using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Linq.Dynamic;
using System.ServiceModel.Configuration;
using System.Web.UI.WebControls;

public partial class Page_Status_TG_V2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            //DLC
            DLC.StatusPricing(status);

            //Generate
            fillFilter();
            fillTable(sender, e);
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&status=" + status.SelectedIndex;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            status.SelectedIndex = Convert.ToInt32(Request.QueryString["status"]);
        }
    }

    protected DataTable Source()
    {
        string query = "EXEC [DB_PROD_MYAPPS].dbcmk.dbo.sp_tgp_getDataActual";
        DataTable dt = Rs(query, Db.Cnn_CS, null, CommandType.Text);
        return dt;
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DataTable dtsource = Source();
        string query = "SELECT * FROM PricingtableTGP WHERE 1 = 1";
        List<SqlParameter> parameters = new List<SqlParameter>();
        if (status.SelectedIndex > 0) 
        { 
            query += " AND StatusPricing = @STATUS";
            parameters.Add(new SqlParameter("@STATUS", SqlDbType.Int) { Value = status.SelectedValue });
        }
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);

        // Perform Left Join
        var rs = from a in dt.AsEnumerable()
                  join b in dtsource.AsEnumerable()
                  on a.Field<Guid>("MyAppsID") equals b.Field<Guid>("IdGoldContent")
                  into gj
                  from subB in gj.DefaultIfEmpty()
                  select new
                  {
                      MyAppsID = subB == null || subB.IsNull("IdGoldContent") ? Guid.Empty : subB.Field<Guid>("IdGoldContent"),
                      MyAppsName = subB == null || subB.IsNull("MyApps") ? string.Empty : subB.Field<string>("MyApps"),
                      MyAppsSell = subB == null || subB.IsNull("Sell") ? 0 : subB.Field<decimal>("Sell"),
                      MyAppsBuy = subB == null || subB.IsNull("Buy") ? 0 : subB.Field<decimal>("Buy"),
                      ID = (a["ID"] == DBNull.Value) ? 0 : a.Field<int>("ID"),
                      Nama = (a["Nama"] == DBNull.Value) ? string.Empty : a.Field<string>("Nama"),
                      Keterangan = (a["Keterangan"] == DBNull.Value) ? string.Empty : a.Field<string>("Keterangan"),
                      SetupFlag = (a["SetupFlag"] == DBNull.Value) ? false : a.Field<bool>("SetupFlag"),
                      StatusPricing = (a["StatusPricing"] == DBNull.Value) ? -1 : a.Field<int>("StatusPricing"),
                      HargaJual = (a["HargaJual"] == DBNull.Value) ? 0 : a.Field<decimal>("HargaJual"),
                      HargaBeli = (a["HargaBeli"] == DBNull.Value) ? 0 : a.Field<decimal>("HargaBeli"),
                      HargaJualPending = (a["HargaJualPending"] == DBNull.Value) ? 0 : a.Field<decimal>("HargaJualPending"),
                      HargaBeliPending = (a["HargaBeliPending"] == DBNull.Value) ? 0 : a.Field<decimal>("HargaBeliPending"),
                      MutasiJual = (a["MutasiJual"] == DBNull.Value) ? 0 : a.Field<decimal>("MutasiJual"),
                      MutasiBeli = (a["MutasiBeli"] == DBNull.Value) ? 0 : a.Field<decimal>("MutasiBeli"),
                      MutasiPersenJual = (a["MutasiPersenJual"] == DBNull.Value) ? 0 : a.Field<decimal>("MutasiPersenJual"),
                      MutasiPersenBeli = (a["MutasiPersenBeli"] == DBNull.Value) ? 0 : a.Field<decimal>("MutasiPersenBeli"),
                      Operator = (a["Operator"] == DBNull.Value) ? string.Empty : a.Field<string>("Operator"),
                      OperatorTgl = (a["OperatorTgl"] == DBNull.Value) ? string.Empty : a.Field<DateTime>("OperatorTgl").ToString("dd-MM-yyyy hh:mm"),
                      Approval = (a["Approval"] == DBNull.Value) ? string.Empty : a.Field<string>("Approval"),
                      ApprovalTgl = (a["ApprovalTgl"] == DBNull.Value) ? string.Empty : a.Field<DateTime>("ApprovalTgl").ToString("dd-MM-yyyy hh:mm"),
                  };

        // Perform Left Join
        var rsj = from a in dtsource.AsEnumerable()
                join subB in dt.AsEnumerable()
                on a.Field<Guid>("IdGoldContent") equals subB.Field<Guid>("MyAppsID") 
                //into gj
                //from subB in gj.DefaultIfEmpty()
                select new
                {
                    MyAppsID = a.Field<Guid>("IdGoldContent"),
                    MyAppsName = a.Field<string>("MyApps"),
                    MyAppsSell = a.Field<decimal>("Sell"),
                    MyAppsBuy = a.Field<decimal>("Buy"),
                    ID = (subB["ID"] == DBNull.Value) ? 0 : subB.Field<int>("ID"),
                    Nama = (subB["Nama"] == DBNull.Value) ? string.Empty : subB.Field<string>("Nama"),
                    Keterangan = (subB["Keterangan"] == DBNull.Value) ? string.Empty : subB.Field<string>("Keterangan"),
                    SetupFlag = (subB["SetupFlag"] == DBNull.Value) ? false : subB.Field<bool>("SetupFlag"),
                    StatusPricing = (subB["StatusPricing"] == DBNull.Value) ? -1 : subB.Field<int>("StatusPricing"),
                    HargaJual = (subB["HargaJual"] == DBNull.Value) ? 0 : subB.Field<decimal>("HargaJual"),
                    HargaBeli = (subB["HargaBeli"] == DBNull.Value) ? 0 : subB.Field<decimal>("HargaBeli"),
                    HargaJualPending = (subB["HargaJualPending"] == DBNull.Value) ? 0 : subB.Field<decimal>("HargaJualPending"),
                    HargaBeliPending = (subB["HargaBeliPending"] == DBNull.Value) ? 0 : subB.Field<decimal>("HargaBeliPending"),
                    MutasiJual = (subB["MutasiJual"] == DBNull.Value) ? 0 : subB.Field<decimal>("MutasiJual"),
                    MutasiBeli = (subB["MutasiBeli"] == DBNull.Value) ? 0 : subB.Field<decimal>("MutasiBeli"),
                    MutasiPersenJual = (subB["MutasiPersenJual"] == DBNull.Value) ? 0 : subB.Field<decimal>("MutasiPersenJual"),
                    MutasiPersenBeli = (subB["MutasiPersenBeli"] == DBNull.Value) ? 0 : subB.Field<decimal>("MutasiPersenBeli"),
                    Operator = (subB["Operator"] == DBNull.Value) ? string.Empty : subB.Field<string>("Operator"),
                    OperatorTgl = (subB["OperatorTgl"] == DBNull.Value) ? string.Empty : subB.Field<DateTime>("OperatorTgl").ToString("dd-MM-yyyy hh:mm"),
                    Approval = (subB["Approval"] == DBNull.Value) ? string.Empty : subB.Field<string>("Approval"),
                    ApprovalTgl = (subB["ApprovalTgl"] == DBNull.Value) ? string.Empty : subB.Field<DateTime>("ApprovalTgl").ToString("dd-MM-yyyy hh:mm"),
                };

        //RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(r.ID, r.MyAppsID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Nama, r.Keterangan);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Approval(r.Approval, r.ApprovalTgl);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Harga(r.HargaJual.ToString("N2"));
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Harga(r.HargaBeli.ToString("N2"));
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Harga(r.MyAppsSell.ToString("N2"));
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Harga(r.MyAppsBuy.ToString("N2"));
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Status(r.HargaJual,r.HargaBeli, r.MyAppsSell, r.MyAppsBuy, r.MyAppsID);
            td.CssClass = "text-left";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }

    protected string TD_HyperLink(int ID, Guid? MyAppsID)
    {
        string x = string.Empty;
        if (ID > 0)
        {
            x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../StatusLog/TGP_V2.aspx?id=" + ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  Detail";
            x += "</a>";
        }
        else
        {
            x += "<i class=\"icon-exclamation-sign text-red text-md\"></i> ";
        }
        return x;
    }

    protected string TD_Status(int ID, Guid? MyAppsID)
    {
        string x = string.Empty;
        return x;
    }

    protected string TD_Nama(string Nama, string Keterangan)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (Keterangan != "")
            x += "<p class=\"text-muted\">" + Str.HtmlBreak(Keterangan) + "</p>";

        return x;
    }

    protected string TD_Approval(string Approval, string ApprovalTgl)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + Approval + "</strong></p>";
        x += "<p class=\"text-sm\">" + ApprovalTgl + "</p>";

        return x;
    }

    protected string TD_Harga(string Harga)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += Harga;
        x += "</p>";

        return x;
    }

    protected string TD_Status(decimal HargaJual, decimal HargaBeli, decimal HargaJualMyapps, decimal HargaBeliMyapps, Guid ID)
    {
        string x = string.Empty;
        if(ID == Guid.Empty)
        {
            x += "<p class=\"text-danger text-lg\">";
            x += "Tgp Terdaftar Di Jaws tapi tidak di Myapps";
            x += "</p>";
        }
        else
        {
            if (HargaJual <= 0 || HargaBeli <= 0) 
            {
                x += "<p class=\"text-danger text-lg\">";
                x += "Tgp Jaws harus lebih besar dari 0";
                x += "</p>";
            }
            else if (HargaJual == HargaJualMyapps && HargaBeli == HargaBeliMyapps)
            {
                x += "<p class=\"text-success text-lg\">";
                x += "Tgp Jaws sama dengan Tgp Myapps";
                x += "</p>";
            }
            else
            {
                x += "<p class=\"text-danger text-lg\">";
                x += "Tgp Jaws berbeda dengan Tgp Myapps";
                x += "</p>";
            }
        }
        return x;
    }

    protected DataTable Rs(string query, string connection, List<SqlParameter> parameters, CommandType cmdtype)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = cmdtype;
                if (parameters != null)
                {
                    foreach (SqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                cmd.CommandTimeout = 300;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader); // Not streaming, but async-safe here
                }
            }
        }
        return dt;
    }
}