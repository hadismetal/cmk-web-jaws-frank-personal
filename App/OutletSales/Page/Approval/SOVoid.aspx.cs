using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Configuration;


public partial class Page_Approval_SOVoid : System.Web.UI.Page
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

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (cs != null)
            cs.Dispose();
        base.Dispose();
    }
    string Module = "pos"; // pos, acc
    // public Dictionary<string, object> respon { get; private set; }
    protected int myID { get { return App.QueryInt(this, "id"); } }
    public class objStatusStamps
    {
        public string error_message { get; set; }
        public int status_code { get; set; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);
        // respon = new Dictionary<string, object>();
        if (!IsPostBack)
        {
            //Generate
            DateTime now = DateTime.Now;
            start.Text = now.ToString("dd-MM-yyyy");
            end.Text = now.ToString("dd-MM-yyyy");
            Location(loccode);
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }
    protected void DataError() { Response.Redirect("SOVoid.aspx"); }

    protected string getLocationUser()
    {
        string result = "";
        int xUserID = App.UserID;
        int xisAM = 0;
        string listStore = "";
        //check area manager
        var am = from p in db.DataAreaManager where p.IDUser == App.UserID select p;
        int amcount = am.Count();
        if (amcount > 0)
        {
            int i = 0;
            foreach (var x in am)
            {
                int j = 0;
                var lokasi = x.DataAreaManager_Lokasi
                    .GroupBy(p => new { p.IDForm, p.TipeLokasi, p.IDLokasi })
                    .Select(c => new { c.Key.IDForm, c.Key.TipeLokasi, c.Key.IDLokasi });
                foreach (var y in lokasi)
                {
                    //listStore = "";
                    if (y.TipeLokasi == 1)
                        listStore += db.LocOutlet.Single(p => p.ID == y.IDLokasi).Kode;
                    else
                        listStore += db.LocExhibition.Single(p => p.ID == y.IDLokasi).Kode;
                    j++;
                    listStore += ",";
                }
                i++;
            }
            xisAM = 1;
        }

        if (xisAM == 0)
        {
            //check admin sales
            var ad = from p in db.DataAdminSales where p.IDUser == App.UserID select p;
            if (ad.Count() > 0)
            {
                int k = 0;
                foreach (var x in ad)
                {
                    listStore = "";
                    if (x.TipeLokasi == 1)
                        listStore = db.LocOutlet.Single(p => p.ID == x.IDLokasi).Kode;
                    else
                        listStore = db.LocExhibition.Single(p => p.ID == x.IDLokasi).Kode;
                    k++;
                    listStore += ",";
                }
            }
        }

        result = (listStore.Length > 0) ? listStore.Remove(listStore.Length - 1) : listStore;

        return result;
    }

    protected void Location(DropDownList ddl)
    {
        string keyloc = getLocationUser();

        string query = "SELECT A.* FROM (" +
            "SELECT Kode, UPPER(Nama) [Nama] FROM LocOutlet WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',',')))) " +
            "UNION ALL " +
            "SELECT Kode, UPPER(Nama) [Nama] FROM LocExhibition WHERE Draft = 0 AND Disable = 0 AND ('" + keyloc + "' = '' OR ('" + keyloc + "' <> '' AND Kode IN (SELECT splitdata FROM dbo.fx_split_string('" + keyloc + "',',')))) " +
            ") A";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem("All", ""));
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["Kode"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }
    //Display Table

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            loccode.SelectedValue = Request.QueryString["location"];
            status.SelectedValue = Request.QueryString["status"];
            start.Text = Request.QueryString["start"];
            end.Text = Request.QueryString["end"];
            startROW.InnerHtml = Request.QueryString["startROW"];


        }
    }


    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);
        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&location=" + loccode.SelectedValue;
        href += "&status=" + status.SelectedValue;
        href += "&start=" + start.Text;
        href += "&end=" + end.Text;
        href += "&startROW=" + startROW.InnerHtml;
        Response.Redirect(href);
    }

    [WebMethod]
    public static string Approve(int IDAppr)
    {
        try
        {
            string status_SAP = "";
            string status_STAMPS = "";
            int code = 0;
            string msg = "";
            string title = "";


            int IDSO = 0;
            string keterangan = "";
            string Operator = "";

            Literal testingg = new Literal();

            testingg.Text = "";  // Set the Literal's text to an empty string        

            string getKeterangan = "select a.*,b.Nama Operator from ApprovalSOVoid a JOIN frank_ac.dbo.UserAccount b on b.ID=a.Created_by where a.ID=@ID";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(getKeterangan, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", IDAppr);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Loop through the results
                        while (reader.Read())
                        {
                            IDSO = Convert.ToInt32(reader["IDSO"]);
                            keterangan = (string)reader["KeteranganVoid"];
                            Operator = (string)reader["Operator"];
                        }
                    }
                    conn.Close();
                }
            }

            using (Map_DB db = new Map_DB()) // Create a local instance
            {

                var d = db.SalesOrder.SingleOrDefault(p => p.ID == IDSO && p.TglVoid == null);
                string kodelokasi = "";
                if (d.TipeLokasi == 1)
                    kodelokasi = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi).Kode;
                else if (d.TipeLokasi == 2)
                    kodelokasi = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi).Kode;


                if (d != null)
                {
                    //validation and sap void   
                    /*try
                    {
                        if (SAP.xActivate)
                        {

                            status_SAP += ((status_SAP == "") ? SAPM.postVoidSalesOrderToSAP(d) : "");

                            if (d.TradeIn.HasValue)
                            {
                                var t = db.TradeIn.SingleOrDefault(p => p.IDSalesOrder.Value == d.ID);
                                var s = db.Resell.SingleOrDefault(p => p.ID == t.IDResell);

                                status_SAP += ((status_SAP == "") ? SAPM.postVoidBuybackToSAP(s) : "");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status_SAP = "Terjadi kesalahan teknis. Silahkan hubungi IT Administrator. (Error message : " + ex.Message + ")";
                    }*/

                    // Validation and STAMPS Posting
                    try
                    {
                        if (STAMPS.xActivate)
                        {
                            // void all redeem voucher discount / payment stamps

                            bool voidx = false;
                            bool stampsNotActivate = false;

                            // Belum Pelunasan
                            // void for draft data
                            if ((d.StatusVoid == false && d.StatusPembayaran == false))
                            {
                                string txtKeterangan = keterangan.Trim();
                                txtKeterangan += " (Void by " + Operator + ", " + Str.FullDateJam(System.DateTime.Now) + ")";

                                voidx = STAMPSM.VoidAllVoucherByPLU(d, 0, 0);
                                if (!(d.StatusVoid == false && d.StatusPembayaran == false && d.Draft == true))
                                {
                                    voidPayment(d);
                                }

                                d.KeteranganVoid = txtKeterangan;
                                d.TglVoid = DateTime.Today;
                                d.StatusVoid = true;
                                d.Unpaid = d.TotalBayar;
                                d.Paid = 0;
                                db.SubmitChanges();
								
								voidSalesOrder(d, txtKeterangan);
								
                                code = 200;
                                msg = "berhasil void1";
                            }
                            else
                            {
                                //void stamps point too
                                if (d.Unpaid != 0)
                                {
                                    status_STAMPS += ((status_STAMPS == "") ? STAMPSM.CancelTransaction(testingg, d) : "");
                                }
                                else if (d.Unpaid == 0)

                                {
                                    status_STAMPS += ((status_STAMPS == "") ? STAMPSM.CancelTransaction(testingg, d) : "");
                                }

                                status_SAP = ""; // di bypass untuk SAP                                               
                                if (status_SAP == "" && status_STAMPS == "")
                                {
                                    string txtKeterangan = keterangan.Trim();
                                    txtKeterangan += " (Void by " + Operator + ", " + Str.FullDateJam(System.DateTime.Now) + ")";

                                    if (!(d.StatusVoid == false && d.StatusPembayaran == false && d.Draft == true))
                                    {
                                        voidPayment(d);
                                    }
                                    voidx = STAMPSM.VoidAllVoucherByPLU(d, 0, 0);

                                    d.KeteranganVoid = txtKeterangan;
                                    d.TglVoid = DateTime.Today;
                                    d.StatusVoid = true;
                                    d.Unpaid = d.TotalBayar;
                                    d.Paid = 0;
                                    db.SubmitChanges();
									
									voidSalesOrder(d, txtKeterangan);
									
                                    code = 200;
                                    msg = "berhasil void2";
                                }
                                else if ((status_STAMPS.Contains("STAMPS Error : Transaction is already canceled.") || status_STAMPS == "STAMPS Error : Transaction is already canceled. <br /> Segera hubungi Tim CRM.") && d.StatusVoid == false)
                                {
                                    string txtKeterangan = keterangan.Trim();
                                    txtKeterangan += " (Void by " + Operator + ", " + Str.FullDateJam(System.DateTime.Now) + ")";

                                    if (!(d.StatusVoid == false && d.StatusPembayaran == false && d.Draft == true))
                                    {
                                        voidPayment(d);
                                    }

                                    voidx = STAMPSM.VoidAllVoucherByPLU(d, 0, 0);
                                    d.KeteranganVoid = txtKeterangan;
                                    d.TglVoid = DateTime.Today;
                                    d.StatusVoid = true;
                                    d.Unpaid = d.TotalBayar;
                                    d.Paid = 0;
                                    db.SubmitChanges();
									
									voidSalesOrder(d, txtKeterangan);
									
                                    code = 200;
                                    msg = "berhasil void3";
                                }
                                else
                                {
                                    if (status_SAP != "")
                                    {
                                        status_SAP = " SAP Error : " + status_SAP + " . ";
                                    }

                                    if (status_STAMPS == "STAMPS Error : Transaction is already canceled.")
                                    {
                                        string txtKeterangan = keterangan.Trim();
                                        txtKeterangan += " (Void by " + Operator + ", " + Str.FullDateJam(System.DateTime.Now) + ")";

                                        if (!(d.StatusVoid == false && d.StatusPembayaran == false && d.Draft == true))
                                        {
                                            voidPayment(d);
                                        }

                                        voidx = STAMPSM.VoidAllVoucherByPLU(d, 0, 0);
                                        d.KeteranganVoid = txtKeterangan;
                                        d.TglVoid = DateTime.Today;
                                        d.StatusVoid = true;
                                        d.Unpaid = d.TotalBayar;
                                        d.Paid = 0;
                                        db.SubmitChanges();
										
										voidSalesOrder(d, txtKeterangan);
										
                                        code = 200;
                                        msg = "berhasil void3";
                                    }
                                    else
                                    {
                                        code = 500;
                                        msg = status_STAMPS;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        status_STAMPS = "Terjadi kesalahan teknis. Silahkan hubungi IT Support. (STAMPS Error message : "
                            + ex.Message
                            + ex.StackTrace
                            + ex.InnerException
                            + ")";
                        code = 500;
                        msg = status_STAMPS;
                        // msg = status_STAMPS;
                    }

                }
                if (status_STAMPS == "" || status_STAMPS.Contains("STAMPS Error : Transaction is already canceled.") || status_STAMPS == "STAMPS Error : Transaction is already canceled. <br /> Segera hubungi Tim CRM.")
                {
                    try
                    {
                        string Approve = "update a set Approval=@Operator,ApprovalTgl=GETDATE() FROM (select * from ApprovalSOVoid where ID=@ID)a";
                        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(Approve, conn))
                            {
                                cmd.Parameters.AddWithValue("@Operator", App.UserNama);
                                cmd.Parameters.AddWithValue("@ID", IDAppr);
                                conn.Open();
                                cmd.ExecuteReader();
                                conn.Close();
                            }
                        }
                        code = 200;
                        title = "success";
                        msg = "berhasil void4 ";
                    }
                    catch (Exception ex)
                    {
                        code = 500;
                        title = "error";
                        msg = ex.Message.ToString();
                    }
                }
                else
                {
                    try
                    {
                        string Reject = "update a set Rejected=@Operator,RejectedTgl=GETDATE(),KeteranganReject=@Keterangan FROM (select * from ApprovalSOVoid where ID=@ID)a";
                        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(Reject, conn))
                            {
                                cmd.Parameters.AddWithValue("@Operator", App.UserNama);
                                cmd.Parameters.AddWithValue("@ID", IDAppr);
                                cmd.Parameters.AddWithValue("@Keterangan", status_STAMPS);
                                conn.Open();
                                cmd.ExecuteReader();
                                conn.Close();
                            }
                        }
                        code = 400;
                        title = "berhasil";
                        msg = status_STAMPS;
                    }
                    catch (Exception ex)
                    {
                        code = 500;
                        title = "error";
                        msg = ex.Message.ToString();
                    }
                }

            }
            var respon = new Dictionary<string, object>()
            {
                { "code", code},
                { "title", title},
                { "msg", msg }
            };

            return new JavaScriptSerializer().Serialize(respon);
        }
        catch (Exception ex)
        {
            var respon = new Dictionary<string, object>()
            {
                { "code", "500"},
                { "title", "Error"},
                { "msg", ex.Message.ToString()}
            };

            return new JavaScriptSerializer().Serialize(respon);
        }

    }

    [WebMethod]
    public static string Reject(int IDAppr, string ketVoid, string noso)
    {
        int code = 0;
        string title = "";
        string msg = "";

        try
        {
            string Reject = "update a set Rejected=@Operator,RejectedTgl=GETDATE(),KeteranganReject=@Keterangan FROM (select * from ApprovalSOVoid where ID=@ID)a";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Reject, conn))
                {
                    cmd.Parameters.AddWithValue("@Operator", App.UserNama);
                    cmd.Parameters.AddWithValue("@ID", IDAppr);
                    cmd.Parameters.AddWithValue("@Keterangan", ketVoid);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }

            code = 200;
            title = "berhasil";
            msg = "Pengajuan void dengan nomor SO  " + noso + " berhasil di reject.";

        }
        catch (Exception ex)
        {
            code = 500;
            title = "gagal";
            msg = ex.Message.ToString();
        }


        var respon = new Dictionary<string, object>()
        {
            { "code", code},
            { "title", title},
            { "msg", msg}
        };

        return new JavaScriptSerializer().Serialize(respon);
    }
    public class ItemModel
    {
        public string keterangan { get; set; }
        public string PLU { get; set; }
    }


    protected void fillTable(object sender, EventArgs e)
    {
        string keyword = ChangeStringWildCardCharacterSQL(kw.Text);
        string[] starttgl = start.Text.Split('-');
        string[] endtgl = end.Text.Split('-');


        string started = ChangeStringWildCardCharacterSQL(starttgl[2] + "-" + starttgl[1] + "-" + starttgl[0]);
        string ended = ChangeStringWildCardCharacterSQL(endtgl[2] + "-" + endtgl[1] + "-" + endtgl[0]);
        string statusddl = ChangeStringWildCardCharacterSQL(status.SelectedValue);

        string xKeyloc = "";
        string startRowValue = startROW.InnerHtml;
        int pageStart = int.Parse(startRowValue);


        int pageSize = 10;

        if (loccode.SelectedValue == "")
            xKeyloc = getLocationUser();
        else
            xKeyloc = loccode.SelectedValue;

        string query = @"                    
                        select * from (
                            select a.ID,a.IDSO,a.KeteranganVoid,ISNULL(a.Approval,'') Approval,a.ApprovalTgl,ISNULL(a.Rejected,'') Rejected,a.RejectedTgl
                            ,ISNULL(a.KeteranganReject,'') KeteranganReject
                            ,b.Nomor AS NomorSO,b.CustomerNama,b.IDCustomer,b.SalesNama,b.Tgl,aa.Nama Operator,Convert(varchar(12),a.Created_at,113) tanggalVoid
                            ,IIF(b.TipeLokasi=2,bb.Nama,ba.Nama) Store ,IIF(b.TipeLokasi=2,bb.Kode,ba.Kode) Kode
                            ,b.IDLokasi
                            from ApprovalSOVoid a                        
                            JOIN SalesOrder b on b.ID=a.IDSO
                            LEFT JOIN LocOutlet ba on ba.ID=b.IDLokasi
                            LEFT JOIN LocExhibition bb on bb.ID=b.IDLokasi
                            JOIN frank_ac.dbo.UserAccount aa on aa.ID=A.Created_by
                        )ax
                        WHERE (@loc = '' OR (@loc <> '' AND ax.Kode in(SELECT splitdata FROM dbo.fx_split_string(@loc,','))))
                        and (@keyword ='' or ax.NomorSO LIKE '%'+@keyword+'%')
                        and ((@status = 0 ) 
                        OR (@status = 1 and ax.ApprovalTgl IS NULL and ax.RejectedTgl IS NULL) 
                        OR (@status = 2 and ax.ApprovalTgl IS NOT NULL and ax.RejectedTgl IS NULL) 
                        OR (@status = 3 and ax.RejectedTgl IS NOT NULL and ax.ApprovalTgl IS NULL))
                        and convert(date,ax.tanggalVoid) between @tgl1 and @tgl2
                        ORDER BY ax.ID ASC
                        OFFSET " + (pageStart <= 0 ? 1 : pageStart - 1) * pageSize + @" ROWS
                        FETCH NEXT " + pageSize + @" ROWS ONLY 
                        ";
        string query1 = @"select count(*) totalPages from (
                            select a.ID,a.IDSO,a.KeteranganVoid,ISNULL(a.Approval,'') Approval,a.ApprovalTgl,ISNULL(a.Rejected,'') Rejected,a.RejectedTgl
                            ,ISNULL(a.KeteranganReject,'') KeteranganReject
                            ,b.Nomor AS NomorSO,b.CustomerNama,b.IDCustomer,b.SalesNama,b.Tgl,aa.Nama Operator,Convert(varchar(12),a.Created_at,113) tanggalVoid
                            ,IIF(b.TipeLokasi=2,bb.Nama,ba.Nama) Store ,IIF(b.TipeLokasi=2,bb.Kode,ba.Kode) Kode
                            ,b.IDLokasi
                            from ApprovalSOVoid a                        
                            JOIN SalesOrder b on b.ID=a.IDSO
                            LEFT JOIN LocOutlet ba on ba.ID=b.IDLokasi
                            LEFT JOIN LocExhibition bb on bb.ID=b.IDLokasi
                            JOIN frank_ac.dbo.UserAccount aa on aa.ID=A.Created_by
                        )ax
                        WHERE (@loc = '' OR (@loc <> '' AND ax.Kode in(SELECT splitdata FROM dbo.fx_split_string(@loc,','))))
                        and (@keyword ='' or ax.NomorSO LIKE '%'+@keyword+'%')
                        and ((@status = 0 ) 
                        OR (@status = 1 and ax.ApprovalTgl IS NULL and ax.RejectedTgl IS NULL) 
                        OR (@status = 2 and ax.ApprovalTgl IS NOT NULL and ax.RejectedTgl IS NULL) 
                        OR (@status = 3 and ax.RejectedTgl IS NOT NULL and ax.ApprovalTgl IS NULL))
                        and convert(date,ax.tanggalVoid) between @tgl1 and @tgl2";
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query1, conn))
            {
                cmd.Parameters.AddWithValue("@loc", xKeyloc);
                cmd.Parameters.AddWithValue("@keyword", keyword);
                cmd.Parameters.AddWithValue("@status", statusddl);
                cmd.Parameters.AddWithValue("@tgl1", started);
                cmd.Parameters.AddWithValue("@tgl2", ended);
                conn.Open();
                int totalRecord = 0;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    totalRecord = reader.Read() ? Convert.ToInt32(reader["totalPages"]) : 0;
                }
                conn.Close();
                BindData(pageStart, pageSize, totalRecord);
            }
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@loc", xKeyloc);
                cmd.Parameters.AddWithValue("@keyword", keyword);
                cmd.Parameters.AddWithValue("@status", statusddl);
                cmd.Parameters.AddWithValue("@tgl1", started);
                cmd.Parameters.AddWithValue("@tgl2", ended);
                conn.Open();
                string Status = "";
                string PICA = "";
                string PICR = "";
                string btnappr = "";
                DateTime formattedDate = DateTime.Today;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    try
                    {
                        int index = 1;
                        pageStart = pageStart <= 0 ? 1 : pageStart;
                        var counter = ((pageStart - 1) * pageSize) + 1;
                        while (reader.Read())
                        {
                            TableRow tr = new TableRow();
                            TableCell td;
                            int IDSO = Convert.ToInt32(reader["IDSO"]);
                            int IDAPPR = Convert.ToInt32(reader["ID"]);
                            string nomorSO = Convert.ToString(reader["NomorSO"]);
                            DateTime tglSO = Convert.ToDateTime(reader["Tgl"]);

                            if (reader["Approval"].ToString() != "")
                            {
                                Status = "<span class='label label-success'>Approved</span>";
                                PICA = (string)reader["Approval"];
                                PICR = "";
                            }
                            else if (reader["Rejected"].ToString() != "")
                            {
                                Status = "<span class='label label-danger'>Rejected</span>";
                                PICA = "";
                                PICR = (string)reader["Rejected"];
                            }
                            else
                            {
                                Status = "<span class='label label-warning'>Not Approve Yet</span>";
                                if (tglSO >= formattedDate)
                                {
                                    btnappr = @"<div id='bungkusanBTN" + IDAPPR + @"' ClientIDMode='Static'>
                                            
                                                <div id='btnWrap' ClientIDMode='Static'>
                                                    <button runat='server' id='btnAppr' ClientIDMode='Static' onclick='approveSelected(" + IDAPPR + @"); return false;' class='btn btn-success button-loading btn-default btn-sm'>Approve</button>
                                                    <button runat='server' id='btnReject" + IDAPPR + "' data-noso='" + nomorSO + "' ClientIDMode='Static' onclick='openModalR(" + IDAPPR + @"); return false;' class='btn btn-danger button-loading btn-default btn-sm'>Reject</button>
                                                </div>
                                            </div>";
                                }
                                PICA = "";
                                PICR = "";
                            }


                            td = new TableCell();
                            td.Text = btnappr;
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);


                            td = new TableCell();
                            td.Text = "<p>" + counter.ToString() + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["NomorSO"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["Store"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["KeteranganVoid"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["SalesNama"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["CustomerNama"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["Operator"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Status;
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = PICA;
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = reader["ApprovalTgl"] == DBNull.Value ? "" : Convert.ToDateTime(reader["ApprovalTgl"]).ToString("dd-MM-yyyy hh:mm tt");
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = PICR;
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = reader["RejectedTgl"] == DBNull.Value ? "" : Convert.ToDateTime(reader["RejectedTgl"]).ToString("dd-MM-yyyy hh:mm tt");
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["KeteranganReject"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);


                            tb.Rows.Add(tr);
                            counter++;
                        }
                    }
                    catch (System.Exception)
                    {

                        throw;
                    }
                }
                conn.Close();
            }
        }
    }
    protected void fillTable1(object sender, EventArgs e)
    {
        string keyword = ChangeStringWildCardCharacterSQL(kw.Text);
        string[] starttgl = start.Text.Split('-');
        string[] endtgl = end.Text.Split('-');


        string started = ChangeStringWildCardCharacterSQL(starttgl[2] + "-" + starttgl[1] + "-" + starttgl[0]);
        string ended = ChangeStringWildCardCharacterSQL(endtgl[2] + "-" + endtgl[1] + "-" + endtgl[0]);
        string statusddl = ChangeStringWildCardCharacterSQL(status.SelectedValue);

        string xKeyloc = "";
        string startRowValue = Request.Form["startROW"];
        int pageStart = 1;
        if (!string.IsNullOrEmpty(startRowValue))
        {
            pageStart = int.Parse(startRowValue);
        }


        int pageSize = 10;

        if (loccode.SelectedValue == "")
            xKeyloc = getLocationUser();
        else
            xKeyloc = loccode.SelectedValue;

        string query = @"                    
                        select * from (
                            select a.ID,a.IDSO,a.KeteranganVoid,ISNULL(a.Approval,'') Approval,a.ApprovalTgl,ISNULL(a.Rejected,'') Rejected,a.RejectedTgl
                            ,ISNULL(a.KeteranganReject,'') KeteranganReject
                            ,b.Nomor AS NomorSO,b.CustomerNama,b.IDCustomer,b.SalesNama,b.Tgl,aa.Nama Operator,Convert(varchar(12),a.Created_at,113) tanggalVoid
                            ,IIF(b.TipeLokasi=2,bb.Nama,ba.Nama) Store ,IIF(b.TipeLokasi=2,bb.Kode,ba.Kode) Kode
                            ,b.IDLokasi
                            from ApprovalSOVoid a                        
                            JOIN SalesOrder b on b.ID=a.IDSO
                            LEFT JOIN LocOutlet ba on ba.ID=b.IDLokasi
                            LEFT JOIN LocExhibition bb on bb.ID=b.IDLokasi
                            JOIN frank_ac.dbo.UserAccount aa on aa.ID=A.Created_by
                        )ax
                        WHERE (@loc = '' OR (@loc <> '' AND ax.Kode in(SELECT splitdata FROM dbo.fx_split_string(@loc,','))))
                        and (@keyword ='' or ax.NomorSO LIKE '%'+@keyword+'%')
                        and ((@status = 0 ) 
                        OR (@status = 1 and ax.ApprovalTgl IS NULL and ax.RejectedTgl IS NULL) 
                        OR (@status = 2 and ax.ApprovalTgl IS NOT NULL and ax.RejectedTgl IS NULL) 
                        OR (@status = 3 and ax.RejectedTgl IS NOT NULL and ax.ApprovalTgl IS NULL))
                        and convert(date,ax.tanggalVoid) between @tgl1 and @tgl2
                        ORDER BY ax.ID ASC
                        OFFSET " + (pageStart - 1) * pageSize + @" ROWS
                        FETCH NEXT " + pageSize + @" ROWS ONLY 
                        ";
        string query1 = @"select count(*) totalPages from (
                            select a.ID,a.IDSO,a.KeteranganVoid,ISNULL(a.Approval,'') Approval,a.ApprovalTgl,ISNULL(a.Rejected,'') Rejected,a.RejectedTgl
                            ,ISNULL(a.KeteranganReject,'') KeteranganReject
                            ,b.Nomor AS NomorSO,b.CustomerNama,b.IDCustomer,b.SalesNama,b.Tgl,aa.Nama Operator,Convert(varchar(12),a.Created_at,113) tanggalVoid
                            ,IIF(b.TipeLokasi=2,bb.Nama,ba.Nama) Store ,IIF(b.TipeLokasi=2,bb.Kode,ba.Kode) Kode
                            ,b.IDLokasi
                            from ApprovalSOVoid a                        
                            JOIN SalesOrder b on b.ID=a.IDSO
                            LEFT JOIN LocOutlet ba on ba.ID=b.IDLokasi
                            LEFT JOIN LocExhibition bb on bb.ID=b.IDLokasi
                            JOIN frank_ac.dbo.UserAccount aa on aa.ID=A.Created_by
                        )ax
                        WHERE (@loc = '' OR (@loc <> '' AND ax.Kode in(SELECT splitdata FROM dbo.fx_split_string(@loc,','))))
                        and (@keyword ='' or ax.NomorSO LIKE '%'+@keyword+'%')
                        and ((@status = 0 ) 
                        OR (@status = 1 and ax.ApprovalTgl IS NULL and ax.RejectedTgl IS NULL) 
                        OR (@status = 2 and ax.ApprovalTgl IS NOT NULL and ax.RejectedTgl IS NULL) 
                        OR (@status = 3 and ax.RejectedTgl IS NOT NULL and ax.ApprovalTgl IS NULL))
                        and convert(date,ax.tanggalVoid) between @tgl1 and @tgl2";
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query1, conn))
            {
                cmd.Parameters.AddWithValue("@loc", xKeyloc);
                cmd.Parameters.AddWithValue("@keyword", keyword);
                cmd.Parameters.AddWithValue("@status", statusddl);
                cmd.Parameters.AddWithValue("@tgl1", started);
                cmd.Parameters.AddWithValue("@tgl2", ended);
                conn.Open();
                int totalRecord = 0;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    totalRecord = reader.Read() ? Convert.ToInt32(reader["totalPages"]) : 0;
                }
                conn.Close();
                BindData(pageStart, pageSize, totalRecord);
            }
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@loc", xKeyloc);
                cmd.Parameters.AddWithValue("@keyword", keyword);
                cmd.Parameters.AddWithValue("@status", statusddl);
                cmd.Parameters.AddWithValue("@tgl1", started);
                cmd.Parameters.AddWithValue("@tgl2", ended);
                conn.Open();
                string Status = "";
                string PICA = "";
                string PICR = "";
                string btnappr = "";
                DateTime formattedDate = DateTime.Today;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    try
                    {
                        int index = 1;
                        var counter = 1;
                        while (reader.Read())
                        {
                            TableRow tr = new TableRow();
                            TableCell td;
                            int IDSO = Convert.ToInt32(reader["IDSO"]);
                            int IDAPPR = Convert.ToInt32(reader["ID"]);
                            string nomorSO = Convert.ToString(reader["NomorSO"]);
                            DateTime tglSO = Convert.ToDateTime(reader["Tgl"]);

                            if ((string)reader["Approval"] != "")
                            {
                                Status = "<span class='label label-success'>Approved</span>";
                                PICA = (string)reader["Approval"];
                                PICR = "";
                            }
                            else if ((string)reader["Rejected"] != "")
                            {
                                Status = "<span class='label label-danger'>Rejected</span>";
                                PICA = "";
                                PICR = (string)reader["Rejected"];
                            }
                            else
                            {
                                Status = "<span class='label label-warning'>Not Approve Yet</span>";
                                if (tglSO >= formattedDate)
                                {
                                    btnappr = @"<div id='bungkusanBTN" + IDAPPR + @"' ClientIDMode='Static'>
                                            
                                                <div id='btnWrap' ClientIDMode='Static'>
                                                    <button runat='server' id='btnAppr' ClientIDMode='Static' onclick='approveSelected(" + IDAPPR + @"); return false;' class='btn btn-success button-loading btn-default btn-sm'>Approve</button>
                                                    <button runat='server' id='btnReject" + IDAPPR + "' data-noso='" + nomorSO + "' ClientIDMode='Static' onclick='openModalR(" + IDAPPR + @"); return false;' class='btn btn-danger button-loading btn-default btn-sm'>Reject</button>
                                                </div>
                                            </div>";
                                }
                                PICA = "";
                                PICR = "";
                            }


                            td = new TableCell();
                            td.Text = btnappr;
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);


                            td = new TableCell();
                            td.Text = "<p>" + counter.ToString() + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["NomorSO"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["Store"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["KeteranganVoid"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["SalesNama"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["CustomerNama"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["Operator"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Status;
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = PICA;
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = reader["ApprovalTgl"] == DBNull.Value ? "" : Convert.ToDateTime(reader["ApprovalTgl"]).ToString("dd-MM-yyyy hh:mm tt");
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = PICR;
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = reader["RejectedTgl"] == DBNull.Value ? "" : Convert.ToDateTime(reader["RejectedTgl"]).ToString("dd-MM-yyyy hh:mm tt");
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "<p>" + (string)reader["KeteranganReject"] + ".</p>";
                            td.Attributes.CssStyle.Add("text-align", "center");
                            tr.Cells.Add(td);


                            tb.Rows.Add(tr);
                            counter++;
                        }
                    }
                    catch (System.Exception)
                    {

                        throw;
                    }
                }
                conn.Close();
            }
        }
    }

    private void BindData(int page, int pageSize, int totalRecord)
    {
        int totalRecords = totalRecord; // Fetch total records from DB
        int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        GeneratePagination(page, totalPages);
    }

    private void GeneratePagination(int currentPage, int totalPages)
    {
        string paginationHtml = "";

        // Previous Button            
        // paginationHtml += $"<li class='{(currentPage == 1 ? "disabled" : "")}'> <a href='{(currentPage > 1 ? url + (currentPage - 1) : "#")}'>&laquo;</a> </li>";
        paginationHtml += "<a href='SOVoid.aspx?refreshPage=1&kw=" + kw.Text + "&location=" + loccode.SelectedValue + "&status=" + status.SelectedValue + "0&start=" + start.Text + "&end=" + end.Text + "&startROW=" + (int.Parse(startROW.InnerHtml) - 1) + "' class='btn btn-default' " + (currentPage == 1 ? "disabled" : "") + " runat='server'>Prev</a>";

        // Page Numbers
        for (int i = 1; i <= totalPages; i++)
        {
            paginationHtml += "<a href='SOVoid.aspx?refreshPage=1&kw=" + kw.Text + "&location=" + loccode.SelectedValue + "&status=" + status.SelectedValue + "0&start=" + start.Text + "&end=" + end.Text + "&startROW=" + i + "' class='btn btn-default' " + (currentPage == i ? "disabled" : "") + " runat='server'>" + i + "</a>";
        }

        // Next Button
        paginationHtml += "<a href='SOVoid.aspx?refreshPage=1&kw=" + kw.Text + "&location=" + loccode.SelectedValue + "&status=" + status.SelectedValue + "0&start=" + start.Text + "&end=" + end.Text + "&startROW=" + (int.Parse(startROW.InnerHtml) + 1) + "' class='btn btn-default' " + ((currentPage == totalPages || currentPage > totalPages) ? "disabled" : "") + " runat='server'>Next</a>";

        paginationGRP.InnerHtml = paginationHtml;
    }

    protected string ChangeStringWildCardCharacterSQL(string input = "")
    {
        string x = "";
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i].ToString() == "'")
                x += input[i] + "'";
            else
                x += input[i];
        }
        return x;
    }
    protected string TD_Hyperlink(string SupplierSJ)
    {
        string x = string.Empty;

        x += "<a href='#' onclick='detail(event)' data-supp='" + SupplierSJ + "'>" + SupplierSJ + "</a>";

        return x;
    }

    protected static void voidPayment(SalesOrder d)
    {
        string result = "";

        result = Payment.voidSalesOrderPaymentAll(d);
    }

    protected static void voidSalesOrder(SalesOrder d, string VoidDescription)
    {
        var dj = d.SalesOrderDJ;
        var pg = d.SalesOrderPG;
        var gj = d.SalesOrderGJ;
        var ld = d.SalesOrderLD;
        var pc = d.SalesOrderPackaging;
        var sv = d.SalesOrderSouvenir;
        var vc = d.SalesVoucher;
        using (Map_CS cs = new Map_CS()) // Create a local instance
        {
            foreach (var r in vc)
            {
                string voucherNumber = r.NomorVoucher;

                var x = cs.T_voucher.SingleOrDefault(p => p.M_nomor == voucherNumber);

                if (x != null)
                {
                    x.M_klaimdoc = "";
                    x.M_tanggal2 = null;
                    x.M_store2 = "";

                    cs.SubmitChanges();
                }
            }
        }

        using (Map_DB db = new Map_DB()) // Create a local instance
        {
            foreach (var r in dj)
            {
                var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProduct);
                if (x != null)
                {
                    Ledger.createLedgerVoidSalesOrderDJ(d, r);

                    x.StatusPenjualan = 0;
                    x.StockActualDJ.InHand = 1;
                    x.StockActualDJ.Substorage = 1;
                    db.SubmitChanges();
                }
            }

            foreach (var r in pg)
            {
                var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProduct);
                if (x != null)
                {
                    Ledger.createLedgerVoidSalesOrderPG(d, r);

                    x.StatusPenjualan = 0;
                    x.StockActualPG.InHand = 1;
                    x.StockActualPG.Substorage = 1;
                    db.SubmitChanges();
                }
            }

            foreach (var r in gj)
            {
                var x = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProduct);
                if (x != null)
                {
                    Ledger.createLedgerVoidSalesOrderGJ(d, r);

                    x.StatusPenjualan = 0;
                    x.StockActualGJ.InHand = 1;
                    x.StockActualGJ.Substorage = 1;
                    db.SubmitChanges();
                }
            }

            foreach (var r in ld)
            {
                var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProduct);
                if (x != null)
                {
                    Ledger.createLedgerVoidSalesOrderLD(d, r);

                    x.StatusPenjualan = 0;
                    x.StockActualLD_Stone1B.InHand = 1;
                    x.StockActualLD_Stone1B.Substorage = 1;
                    db.SubmitChanges();
                }
            }

            foreach (var r in pc)
            {
                var x = db.StockActualPackaging.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeLokasi == d.TipeLokasi && p.IDLokasi == d.IDLokasi);
                if (x != null)
                {
                    if (r.Qty != null)
                    {
                        x.InHand = x.InHand + r.Qty.Value;
                        db.SubmitChanges();
                    }

                }
            }

            foreach (var r in sv)
            {
                if (r.TipeProduct == 0 || r.TipeProduct == null)
                {
                    var x = db.StockActualSouvenir.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeLokasi == d.TipeLokasi && p.IDLokasi == d.IDLokasi);
                    if (x != null)
                    {
                        if (r.Qty != null)
                        {
                            x.InHand = x.InHand + r.Qty.Value;
                            db.SubmitChanges();
                        }
                    }
                }
                else if (r.TipeProduct == 1)
                {
                    var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                    if (x != null)
                    {
                        x.StatusPenjualan = 0;
                        x.StockActualDJ.InHand = 1;
                        db.SubmitChanges();
                    }
                }
                else if (r.TipeProduct == 2)
                {
                    var x = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                    if (x != null)
                    {
                        x.StatusPenjualan = 0;
                        x.StockActualGJ.InHand = 1;
                        db.SubmitChanges();
                    }
                }
                else if (r.TipeProduct == 3)
                {
                    var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                    if (x != null)
                    {
                        x.StatusPenjualan = 0;
                        x.StockActualPG.InHand = 1;
                        db.SubmitChanges();
                    }
                }
                else if (r.TipeProduct == 4)
                {
                    var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                    if (x != null)
                    {
                        x.StatusPenjualan = 0;
                        x.StockActualLD_Stone1B.InHand = 1;
                        db.SubmitChanges();
                    }
                }
            }

            int tradein_resell_id = 0;
            int salesorder_id = 0;

            try
            {
                var tradein = db.TradeIn.SingleOrDefault(p => p.IDSalesOrder == d.ID);

                var resell = db.Resell.SingleOrDefault(p => p.ID == tradein.IDResell);

                foreach (var r in resell.ResellDJ)
                {
                    Ledger.createLedgerVoidTradeInDJ(d, r);

                    var x = db.SalesOrderDJ.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                    x.StatusResell = 0;
                    r.StockProductDJ.StatusPenjualan = 1;
                    r.StockProductDJ.StockActualDJ.InHand = 0;
                    r.StockProductDJ.StockActualDJ.Substorage = 1;
                    db.SubmitChanges();
                }

                foreach (var r in resell.ResellPG)
                {
                    Ledger.createLedgerVoidTradeInPG(d, r);

                    var x = db.SalesOrderPG.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                    x.StatusResell = 0;
                    r.StockProductPG.StatusPenjualan = 1;
                    r.StockProductPG.StockActualPG.InHand = 0;
                    r.StockProductPG.StockActualPG.Substorage = 1;
                    db.SubmitChanges();
                }

                foreach (var r in resell.ResellGJ)
                {
                    Ledger.createLedgerVoidTradeInGJ(d, r);

                    var x = db.SalesOrderGJ.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                    x.StatusResell = 0;
                    r.StockProductGJ.StatusPenjualan = 1;
                    r.StockProductGJ.StockActualGJ.InHand = 0;
                    r.StockProductGJ.StockActualGJ.Substorage = 1;
                    db.SubmitChanges();
                }

                foreach (var r in resell.ResellLD)
                {
                    Ledger.createLedgerVoidTradeInLD(d, r);

                    var x = db.SalesOrderLD.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                    x.StatusResell = 0;
                    r.StockProductLD_Stone1B.StatusPenjualan = 1;
                    r.StockProductLD_Stone1B.StockActualLD_Stone1B.InHand = 0;
                    r.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage = 1;
                    db.SubmitChanges();
                }

                tradein_resell_id = tradein.IDResell.Value;

            }
            catch (Exception) { }

            try
            {
                salesorder_id = d.ID;

                if (tradein_resell_id > 0)
                {
                    Db.Execute("DELETE FROM ResellDJ WHERE IDForm = " + tradein_resell_id.ToString(), Db.Cnn_DB);
                    Db.Execute("DELETE FROM ResellPG WHERE IDForm = " + tradein_resell_id.ToString(), Db.Cnn_DB);
                    Db.Execute("DELETE FROM ResellGJ WHERE IDForm = " + tradein_resell_id.ToString(), Db.Cnn_DB);
                    Db.Execute("DELETE FROM ResellLD WHERE IDForm = " + tradein_resell_id.ToString(), Db.Cnn_DB);
                }

                if (salesorder_id > 0)
                {
                    Db.Execute("DELETE FROM TradeIn WHERE IDSalesOrder = " + salesorder_id.ToString(), Db.Cnn_DB);
                    Db.Execute("DELETE FROM SalesOrderPackaging WHERE IDForm = " + salesorder_id.ToString(), Db.Cnn_DB);

                    string NamaKodeSBU = "";
                    string TransactionType = "so";
                    string TransactionNumber = d.Nomor;

                    if (d.TipeLokasi == 0)
                    {
                        NamaKodeSBU = db.LocWarehouse.SingleOrDefault(p => p.ID == d.IDLokasi).CompanyBrand.NamaKode;
                    }
                    else if (d.TipeLokasi == 1)
                    {
                        NamaKodeSBU = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi).CompanyBrand.NamaKode;
                    }
                    else if (d.TipeLokasi == 2)
                    {
                        NamaKodeSBU = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi).CompanyBrand.NamaKode;
                    }

                    Db.Execute("EXEC sp_repost_all_void_transaction 0 "
                        + ", '" + NamaKodeSBU + "'"
                        + ", '" + TransactionType + "'"
                        + ", '" + TransactionNumber + "'"
                        , Db.Cnn_CS);
                }
            }
            catch (Exception) { }

        }
    }


    protected void voidgiftcard_Click(object sender, EventArgs e)
    {
        string result = "";

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            var x = db.SalesReceiptDetail.Where(p => p.SalesReceipt.SalesOrder.ID == d.ID
                && p.IDPaymentType == 9
                && p.SalesReceipt.SalesOrder.StatusVoid == true
                && p.GC_VoidVerification == null
                && p.GC_VoidVerificationTgl == null
                && p.Nominal > 0
                );

            if (x.Count() > 0)
            {
                foreach (var r in x)
                {
                    result += "PAY # " + r.ID.ToString() + " - " + Payment.returnGiftCard(d, r);
                }

            }

            if (!Payment.showVoidPaymentStatusGiftCard(d))
            {
                Response.Redirect("SalesOrderReport_FormVoid.aspx?id=" + myID);
            }
            // else
            // {
            //     lit_warning_payment_gc_response.Text = "<br /><br />" + result;
            // }
        }

    }

    protected DataSet QuerySPDataSet(string query, string connectionString)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            SqlTransaction trans;
            trans = conn.BeginTransaction();
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(query, connectionString);
                sda.SelectCommand.CommandTimeout = 3000;
                sda.Fill(ds);
                sda.Dispose();
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message.ToString();
                trans.Rollback();
                conn.Close();
            }
        }
        return ds;
    }
}