using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web.UI.WebControls;

public partial class Page_Register_Approver : System.Web.UI.Page
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
        // App.Protect();
        // App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            List<Karyawan> karyawans = GetDataMaster();
            RepeaterKaryawan.DataSource = karyawans;
            RepeaterKaryawan.DataBind(); // This will bind the data to the Repeater control
        }

        if (IsPostBack)
        {
            OnSubmit_SearchKaryawan();
        }
    }


    public void RepeaterKaryawan_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Karyawan karyawan = (Karyawan)e.Item.DataItem;
            int IsDisabled = karyawan.IsDisabled;
            int IsApprover = karyawan.IsApprover;

            Button buttonDisableEnable = (Button)e.Item.FindControl("ButtonDataBoundStatus");
            Button buttonApproverMengetahui = (Button)e.Item.FindControl("ButtonDataBoundApprover");

            if (IsDisabled == 0)
            {
                buttonDisableEnable.Text = "Disable";
                buttonDisableEnable.CommandName = "Disable";
                buttonDisableEnable.CssClass = "focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900";
            }
            else
            {
                buttonDisableEnable.Text = "Enable";
                buttonDisableEnable.CommandName = "Enable";
                buttonDisableEnable.CssClass = "focus:outline-none text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800";
            }

            if (IsApprover == 1)
            {
                buttonApproverMengetahui.Text = "Mengetahui";
                buttonApproverMengetahui.CommandName = "Mengetahui";
                buttonApproverMengetahui.CssClass = "py-2.5 px-5 me-2 mb-2 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700";
            }
            else
            {
                buttonApproverMengetahui.Text = "Approver";
                buttonApproverMengetahui.CommandName = "Approver";
                buttonApproverMengetahui.CssClass = "text-white bg-gray-800 hover:bg-gray-900 focus:outline-none focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-gray-800 dark:hover:bg-gray-700 dark:focus:ring-gray-700 dark:border-gray-700";
            }


            buttonApproverMengetahui.CommandArgument = karyawan.NIK;
            buttonDisableEnable.CommandArgument = karyawan.NIK;
        }
    }


    public void OnSubmit_SearchKaryawan(int IsRefetch = 0)
    {
        string NIK = InputNIK.Value;
        GetKaryawanResponse response;
        if (IsRefetch == 0)
        {
            response = GetKaryawan(NIK);
        }
        else
        {
            response = GetKaryawan(Session["NIK"].ToString());
        }

        if (response.MESSAGE_TYPE == "ERROR")
        {
            element_errorMessage.Visible = true;
            element_contentKaryawan.Visible = false;
            element_errorMessage.InnerText = response.MESSAGE;
            return;
        }

        element_errorMessage.Visible = false;
        element_contentKaryawan.Visible = true;


        Session["NIK"] = response.DATA.NIK;
        element_name.InnerHtml = response.DATA.Nama + " (" + response.DATA.NIK + ") ";
        element_divisi.InnerHtml = response.DATA.Divisi;
        element_email.InnerHtml = response.DATA.Email;
        element_subdivisi.InnerHtml = response.DATA.Subdivisi;
        element_jabatan.InnerHtml = response.DATA.Jabatan;
        element_image.Src = response.DATA.Image;
    }

    public void OnSubmit_RegisterKaryawan(object sender, EventArgs e)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_IN))
        {
            using (SqlCommand command = new SqlCommand("sp_registerMasterWhitelist", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("NIK", SqlDbType.VarChar) { Value = Session["NIK"].ToString() });
                command.Parameters.Add(new SqlParameter("IsApprover", SqlDbType.Int) { Value = element_checkboxApprover.Checked ? 1 : 0 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                GetInsertResponse response = Helper.GetDataFromDataTable<GetInsertResponse>(ds.Tables[0])[0];

                if (response.MESSAGE_TYPE == "ERROR")
                {
                    element_errorMessageRegister.Visible = true;
                    element_errorMessageRegister.InnerHtml = response.MESSAGE;
                    OnSubmit_SearchKaryawan(1);
                }
                else
                {
                    Response.Redirect("index.aspx");
                }
            }
        }
    }

    protected GetKaryawanResponse GetKaryawan(string NIK)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Db.Cnn_CS))
            {
                using (SqlCommand command = new SqlCommand("sp_getDataKaryawanByNik", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("NIK", SqlDbType.VarChar) { Value = NIK });

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    List<Karyawan> karyawans = Helper.GetDataFromDataTable<Karyawan>(ds.Tables[0]);


                    if (karyawans.Count > 0)
                    {
                        return new GetKaryawanResponse
                        {
                            DATA = karyawans[0],
                            MESSAGE = "SUCCESS",
                            MESSAGE_TYPE = "SUCCESS"
                        };
                    }
                    else
                    {
                        return new GetKaryawanResponse
                        {
                            DATA = new Karyawan(),
                            MESSAGE = "Karyawan tidak ditemukan.",
                            MESSAGE_TYPE = "ERROR"
                        };
                    }


                }
            }
        }
        catch (Exception error)
        {
            return new GetKaryawanResponse
            {
                DATA = new Karyawan(),
                MESSAGE = error.Message.ToString(),
                MESSAGE_TYPE = "ERROR"
            };
        }
    }

    protected void DisableEnableMaster(object sender, CommandEventArgs e)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_IN))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("UPDATE StockProductWhitelist_Approval_Master SET IsDisabled = @Status WHERE NIK = @NIK", connection))
            {
                string CommandName = e.CommandName.ToString();
                string Status = CommandName == "Disable" ? "1" : "0";
                string NIK = e.CommandArgument.ToString();

                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("Status", SqlDbType.VarChar) { Value = Status });
                command.Parameters.Add(new SqlParameter("NIK", SqlDbType.VarChar) { Value = NIK });
                command.ExecuteNonQuery();
                Response.Redirect("index.aspx");
            }
        }
    }

    protected void ApproverMengetahuiMaster(object sender, CommandEventArgs e)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_IN))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("UPDATE StockProductWhitelist_Approval_Master SET IsApprover = @Status WHERE NIK = @NIK", connection))
            {
                string CommandName = e.CommandName.ToString();
                string Status = CommandName == "Approver" ? "1" : "0";
                string NIK = e.CommandArgument.ToString();

                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("Status", SqlDbType.VarChar) { Value = Status });
                command.Parameters.Add(new SqlParameter("NIK", SqlDbType.VarChar) { Value = NIK });
                command.ExecuteNonQuery();
                Response.Redirect("index.aspx");
            }
        }
    }

    public List<Karyawan> GetDataMaster()
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_IN))
        {
            using (SqlCommand command = new SqlCommand("SELECT * FROM StockProductWhitelist_Approval_Master", connection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return Helper.GetDataFromDataTable<Karyawan>(ds.Tables[0]);
            }
        }
    }

    public class GetInsertResponse
    {
        public string MESSAGE { get; set; }
        public string MESSAGE_TYPE { get; set; }
    }

    public class GetKaryawanResponse
    {
        public string MESSAGE { get; set; }
        public string MESSAGE_TYPE { get; set; }
        public Karyawan DATA { get; set; }
    }

    [Serializable]
    public class Karyawan
    {
        public string NIK { get; set; }
        public string Nama { get; set; }
        public string Jabatan { get; set; }
        public string Divisi { get; set; }
        public string Subdivisi { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public int IsApprover { get; set; }
        public int IsDisabled { get; set; }
    }
}