using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

public partial class Page_Report_WhitelistTradeIn : System.Web.UI.Page
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
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Dari.Text = DateTime.Today.ToString("yyyy-MM-dd"); // Format for HTML5 date input
            Hingga.Text = DateTime.Today.ToString("yyyy-MM-dd"); // Format for HTML5 date input

            Session["Dari"] = Dari.Text.Trim();
            Session["Hingga"] = Hingga.Text.Trim();
            DefaultFormValue(sender, e);
        }
    }

    protected void OnClickFormSearch(object sender, EventArgs e)
    {
        int TipeLokasi = Convert.ToInt32(tipe_lokasi.SelectedValue);
        int IDLokasi = Convert.ToInt32(outlet.SelectedValue);
        int Status = Convert.ToInt32(status.SelectedValue);
        string FromDate = Dari.Text.Trim();
        string ToDate = Hingga.Text.Trim();

        DateTime FromDateTime = Convert.ToDateTime(Dari.Text);
        DateTime ToDateTime = Convert.ToDateTime(Hingga.Text);
        int result = DateTime.Compare(FromDateTime, ToDateTime);

        Session["Dari"] = FromDate;
        Session["Hingga"] = ToDate;
        Session["IDLokasi"] = IDLokasi;
        Session["TipeLokasi"] = TipeLokasi;
        Session["Status"] = Status;


        if (result > 0)
        {
            error_message.Text = "Tanggal tidak valid.";
            error_message.Visible = true;
        }
        else
        {
            error_message.Text = "";
            error_message.Visible = false;
            var whitelists = GetDataWhitelist(IDLokasi, TipeLokasi, FromDate, ToDate, Status);
            rptTransactions.DataSource = whitelists;
            rptTransactions.DataBind();
        }
    }

    protected void Tipe_Lokasi_Change(object sender, EventArgs e)
    {
        int TipeLokasi = Convert.ToInt32(tipe_lokasi.SelectedValue);
        Dictionary<string, object> defaultLocation = GetDefaultLocation(TipeLokasi);
        var whitelists = GetDataWhitelist(
            Convert.ToInt32(Session["IDLokasi"]),
            Convert.ToInt32(Session["TipeLokasi"]),
            Session["Dari"].ToString(),
            Session["Hingga"].ToString(),
            Convert.ToInt32(Session["Status"])
        );
        rptTransactions.DataSource = whitelists;
        rptTransactions.DataBind();
    }

    protected List<ResponseWhitelist> GetDataWhitelist(int IDLokasi, int TipeLokasi, string TanggalStart = "", string TanggalEnd = "", int Status = 0)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getWhitelistTradein_report", connection))
            {
                NumberFormatInfo nfi = new NumberFormatInfo { NumberGroupSeparator = ".", NumberDecimalDigits = 0 };

                if (TanggalStart == "" && TanggalEnd == "")
                {
                    TanggalStart = DateTime.Today.ToString("yyyy-MM-dd");
                    TanggalEnd = DateTime.Today.ToString("yyyy-MM-dd");
                }

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = TipeLokasi });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@From", SqlDbType.VarChar) { Value = TanggalStart });
                command.Parameters.Add(new SqlParameter("@To", SqlDbType.VarChar) { Value = TanggalEnd });
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar) { Value = Status });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                List<ResponseWhitelist> listWhitelist = new List<ResponseWhitelist>();


                if (ds.Tables.Count > 0)
                {
                    DataTable dtHeader = ds.Tables[0];
                    DataTable dtApprover = ds.Tables[1];
                    DataTable dtFile = ds.Tables[2];
                    List<ResponseHeader> listHeader = Helper.GetDataFromDataTable<ResponseHeader>(dtHeader);
                    List<ResponseApprover> listApprover = Helper.GetDataFromDataTable<ResponseApprover>(dtApprover);
                    List<ResponseFile> listFile = Helper.GetDataFromDataTable<ResponseFile>(dtFile);

                    foreach (ResponseHeader header in listHeader)
                    {
                        var approvers = listApprover.Where(a => a.IDParent == header.IDParent).ToList();
                        var files = listFile.Where(a => a.IDParent == header.IDParent)
                            .Select(a => new ResponseFile
                            {
                                IDParent = a.IDParent,
                                Extension = Path.GetExtension(a.FileName),
                                FileName = a.FileName
                            })
                            .ToList();

                        header.Nominal = header.Nominal.ToString(nfi);

                        listWhitelist.Add(new ResponseWhitelist
                        {
                            Header = header,
                            Approvers = approvers,
                            Files = files
                        });
                    }
                }

                return listWhitelist;
            }
        }
    }

    protected Dictionary<string, object> GetDefaultLocation(int Tipe = 9)
    {
        try
        {

            using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
            {
                using (SqlCommand command = new SqlCommand("sp_getDefaultLocationUserLogin", connection))
                {
                    int IDUser = App.UserID;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = IDUser });
                    command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = Tipe });

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    DataTable DtErrorMessage = ds.Tables[0];

                    string MessageType = "";
                    string Message = "";
                    foreach (DataRow row in DtErrorMessage.Rows)
                    {
                        MessageType = row["MESSAGE_TYPE"].ToString();
                        Message = row["MESSAGE"].ToString();
                    }

                    if (MessageType == "ERROR")
                    {
                        return new Dictionary<string, object>
                        {
                            { "MESSAGE", "ERROR" },
                            { "ERROR_MESSAGE", Message }
                        };
                    }
                    else
                    {
                        DataTable DtTipeLokasi = ds.Tables[1];
                        DataTable DtLokasi = ds.Tables[2];

                        int TipeLokasi = Convert.ToInt32(DtTipeLokasi.Rows[0]["ID"]);
                        int IDLokasi = Convert.ToInt32(DtLokasi.Rows[0]["IDLokasi"]);

                        tipe_lokasi.Items.Clear();
                        outlet.Items.Clear();

                        foreach (DataRow dr in DtTipeLokasi.Rows)
                        {
                            tipe_lokasi.Items.Add(new ListItem(dr["Nama"].ToString(), dr["ID"].ToString()));
                        }

                        tipe_lokasi.SelectedValue = Tipe.ToString();

                        foreach (DataRow dr in DtLokasi.Rows)
                        {
                            outlet.Items.Add(new ListItem(dr["NamaOutlet"].ToString(), dr["IDLokasi"].ToString()));
                        }

                        return new Dictionary<string, object>
                            {
                                { "MESSAGE", "SUCCESS" },
                                { "IDLokasi", IDLokasi },
                                { "TipeLokasi", TipeLokasi }
                            };
                    }
                }
            }
        }
        catch (Exception Error)
        {
            return new Dictionary<string, object>
            {
                { "MESSAGE", "ERROR" },
                { "ERROR_MESSAGE", Error.Message.ToString()
}
            };
        }
    }

    protected void DefaultFormValue(object sender, EventArgs e)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            try
            {
                Dictionary<string, object> defaultLocation = GetDefaultLocation();

                if (defaultLocation["MESSAGE"].ToString() == "ERROR")
                {
                    error_message.Text = defaultLocation["ERROR_MESSAGE"].ToString();
                }
                else
                {
                    Session["TipeLokasi"] = Convert.ToInt32(defaultLocation["TipeLokasi"]);
                    Session["IDLokasi"] = Convert.ToInt32(defaultLocation["IDLokasi"]);
                    Session["Dari"] = Dari.Text.Trim();
                    Session["Hingga"] = Hingga.Text.Trim();
                    Session["Status"] = 0;
                    var whitelists = GetDataWhitelist(Convert.ToInt32(defaultLocation["IDLokasi"]), Convert.ToInt32(defaultLocation["TipeLokasi"]));

                    rptTransactions.DataSource = whitelists;
                    rptTransactions.DataBind();
                }
            }
            catch (Exception Error)
            {
                throw Error;
            }

        }
    }

    public class ResponseHeader
    {
        public int IDParent { get; set; }
        public string PLU { get; set; }
        public string StorePengaju { get; set; }
        public string BrandPengaju { get; set; }
        public string NomorSalesOrder { get; set; }
        public string Invoice { get; set; }
        public string TanggalSalesOrder { get; set; }
        public string Brand { get; set; }
        public string Store { get; set; }
        public string Customer { get; set; }
        public string Nominal { get; set; }
        public string Keterangan { get; set; }
        public string TanggalPengajuan { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public string Reason { get; set; }
    }

    public class ResponseApprover
    {
        public int IDParent { get; set; }
        public string Approval { get; set; }
        public string NIKApproval { get; set; }
        public string Jabatan { get; set; }
        public string ApprovalTgl { get; set; }
        public string Keterangan { get; set; }
    }

    public class ResponseFile
    {
        public int IDParent { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }

    public class ResponseWhitelist
    {
        public ResponseHeader Header { get; set; }
        public List<ResponseApprover> Approvers { get; set; }
        public List<ResponseFile> Files { get; set; }
    }
}