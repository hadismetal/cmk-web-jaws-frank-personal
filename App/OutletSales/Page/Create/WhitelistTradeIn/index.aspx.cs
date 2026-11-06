using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.IO;
using System.Text;


public partial class Page_Register_WhitelistTradeIn : System.Web.UI.Page
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
            GetDefaultLocation(1);
            GetReason();
        }

    }


    public void OnChange_TipeLokasi(object sender, EventArgs e)
    {
        int TipeLokasi = Convert.ToInt32(ddl_tipelokasi.SelectedValue);
        GetDefaultLocation(TipeLokasi);

        ddl_tipelokasi.SelectedValue = TipeLokasi.ToString();
    }

    private static string RenameFile(string originalFileName)
    {
        string dateTimePart = DateTime.Now.ToString("ddMMyyyy");
        string extension = Path.GetExtension(originalFileName);
        string guidPart = Guid.NewGuid().ToString("N");
        string newFileName = guidPart + "_" + dateTimePart + extension;

        return newFileName;
    }

    protected void GetReason()
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("SELECT * FROM StockProductWhitelist_Reason", connection))
            {
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                List<ResponseDataTypeLocation> Reasons = Helper.GetDataFromDataTable<ResponseDataTypeLocation>(ds.Tables[0]);

                ddl_reason.Items.Clear();

                foreach (ResponseDataTypeLocation Reason in Reasons)
                {
                    ddl_reason.Items.Add(new ListItem(Reason.Nama, Reason.ID.ToString()));
                }
            }
        }
    }

    protected void GetDefaultLocation(int TipeLokasi)
    {
        int UserID = App.UserID;

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getDefaultLocationUserLogin", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.VarChar) { Value = UserID });
                command.Parameters.Add(new SqlParameter("@IsReport", SqlDbType.VarChar) { Value = 0 });
                command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = TipeLokasi });
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                ddl_tipelokasi.Items.Clear();
                ddl_lokasi.Items.Clear();

                if (ds.Tables.Count > 1)
                {
                    List<ResponseDataTypeLocation> TypeLocations = Helper.GetDataFromDataTable<ResponseDataTypeLocation>(ds.Tables[1]);
                    List<ResponseDataLocation> Locations = Helper.GetDataFromDataTable<ResponseDataLocation>(ds.Tables[2]);

                    foreach (ResponseDataTypeLocation TypeLocation in TypeLocations)
                    {
                        ddl_tipelokasi.Items.Add(new ListItem(TypeLocation.Nama, TypeLocation.ID.ToString()));
                    }

                    foreach (ResponseDataLocation Location in Locations)
                    {
                        ddl_lokasi.Items.Add(new ListItem(Location.NamaOutlet, Location.IDLokasi.ToString()));
                    }
                }
                else
                {
                    label_message.Text = "Lokasi tidak ditemukan dalam mapping. Silahkan hubungi Tim IT.";
                    label_message.Attributes["style"] = "width: 100%; margin-bottom: 20px; background-color: #dc3545; color: white; padding: 8px; 15px; border-radius: 5px;";
                }

            }
        }
    }

    protected ResponseUploadFiles uploadFiles()
    {
        List<string> validExtension = new List<string> { ".pdf", ".png", ".jpeg", ".jpg" };


        int isFailed = 0; // 0 = no files is uploaded, 1 = there is upload files but failed, 2 = there is upload files and success
        int isInvalidExtension = 0; // 0 = valid files uploaded, 1 = invalid files uploaded

        List<string> filesUploaded = new List<string>();
        if (imagePicture.HasFile)
        {
            foreach (HttpPostedFile image in imagePicture.PostedFiles)
            {
                string extension = Path.GetExtension(image.FileName);

                if (!validExtension.Contains(extension.ToLower()))
                {
                    isInvalidExtension = 1;
                }

                if (isInvalidExtension == 1)
                {
                    break;
                }

                try
                {
                    if (image != null && image.InputStream != null && image.FileName != "" && image.ContentLength > 0)
                    {
                        string path = App.ImagesDir + "WhitelistTradein\\";
                        if (Upload.CreateDir(path))
                        {
                            string fileName = Path.GetFileName(image.FileName);
                            string newFileName = RenameFile(fileName);
                            image.SaveAs(path + newFileName);
                            filesUploaded.Add(path + newFileName);
                            isFailed = 2;
                        }
                    }
                }
                catch (Exception error)
                {
                    label_message_image.Text = error.Message.ToString();
                    label_message_image.Attributes["style"] = "width: 100%; margin-bottom: 20px; background-color: #dc3545; color: white; padding: 8px; 15px; border-radius: 5px;";
                    isFailed = 1;
                    break;
                }
            }
        }


        if (isFailed == 1 || isInvalidExtension == 1)
        {
            // Delete files if there is invalid extension or invalid upload.
            foreach (string file in filesUploaded)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }

            if (isInvalidExtension == 1)
            {
                label_message_image.Text = String.Join(", ", validExtension) + " are the accepted files.";
                label_message_image.Attributes["style"] = "width: 100%; margin-bottom: 20px; background-color: #dc3545; color: white; padding: 8px; 15px; border-radius: 5px;";
            }

            return
                new
                ResponseUploadFiles
                {
                    FILE_UPLOADED = filesUploaded,
                    IS_INVALID_EXTENSION = isInvalidExtension,
                    IS_UPLOADED = isFailed
                };
        }

        return
            new
            ResponseUploadFiles
            {
                FILE_UPLOADED = filesUploaded,
                IS_INVALID_EXTENSION = isInvalidExtension,
                IS_UPLOADED = isFailed
            };
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        int UserID = App.UserID;
        string UserNama = App.UserNama;
        string TipeProduct = ddl_tipeproduk.SelectedValue.ToString().ToUpper();
        string NomorPLU = txt_plu.Text.Trim();
        string Keterangan = txt_keterangan.Text.Trim();

        if (String.IsNullOrEmpty(ddl_tipelokasi.SelectedValue))
        {
            label_message.Text = "Tipe Lokasi tidak boleh kosong.";
            label_message.Attributes["style"] = "width: 100%; margin-bottom: 20px; background-color: #dc3545; color: white; padding: 8px; 15px; border-radius: 5px;";
            return;
        }
        else
        {
            if (String.IsNullOrEmpty(ddl_lokasi.SelectedValue))
            {
                label_message.Text = "Lokasi tidak boleh kosong.";
                label_message.Attributes["style"] = "width: 100%; margin-bottom: 20px; background-color: #dc3545; color: white; padding: 8px; 15px; border-radius: 5px;";
                return;
            }
        }

        ResponseUploadFiles responseUploadFiles = uploadFiles();

        if (responseUploadFiles.IS_INVALID_EXTENSION == 1 || responseUploadFiles.IS_INVALID_EXTENSION == 1)
        {
            return;
        }

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("sp_registerWhitelistTradein", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar) { Value = UserID });
                command.Parameters.Add(new SqlParameter("@UserNama", SqlDbType.VarChar) { Value = UserNama });
                command.Parameters.Add(new SqlParameter("@PLU", SqlDbType.VarChar) { Value = NomorPLU });
                command.Parameters.Add(new SqlParameter("@TipeProduct", SqlDbType.VarChar) { Value = TipeProduct });
                command.Parameters.Add(new SqlParameter("@Keterangan", SqlDbType.VarChar) { Value = Keterangan });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = Convert.ToInt32(ddl_tipelokasi.SelectedValue) });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = Convert.ToInt32(ddl_lokasi.SelectedValue) });
                command.Parameters.Add(new SqlParameter("@IDReason", SqlDbType.Int) { Value = Convert.ToInt32(ddl_reason.SelectedValue) });
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    string MessageType = dt.Rows[0]["MESSAGE_TYPE"].ToString();
                    string Message = dt.Rows[0]["MESSAGE"].ToString();
                    label_message.Text = Message;

                    if (MessageType == "ERROR")
                    {
                        label_message.Attributes["style"] = "width: 100%; margin-bottom: 20px; background-color: #dc3545; color: white; padding: 8px; 15px; border-radius: 5px;";

                        // Delete files if there is invalid response coming from databases
                        foreach (string file in responseUploadFiles.FILE_UPLOADED)
                        {
                            if (File.Exists(file))
                            {
                                File.Delete(file);
                            }
                        }
                    }
                    else
                    {
                        int ID = (int)dt.Rows[0]["ID"];

                        txt_plu.Text = "";
                        txt_keterangan.Text = "";
                        label_message.Attributes["style"] = "width: 100%; margin-bottom: 20px; background-color: #198754; color: white; padding: 8px; 15px; border-radius: 5px;";
                        label_message_image.Text = "";
                        label_message_image.Attributes["style"] = "";
                        using (SqlCommand commandInsertFiles = new SqlCommand("INSERT INTO StockProductWhitelist_Files (IDForm, FileName) VALUES (@IDForm, @FileName)", connection))
                        {
                            commandInsertFiles.CommandType = CommandType.Text;
                            foreach (string file in responseUploadFiles.FILE_UPLOADED)
                            {
                                commandInsertFiles.Parameters.Add(new SqlParameter("@IDForm", SqlDbType.Int) { Value = ID });
                                commandInsertFiles.Parameters.Add(new SqlParameter("@FileName", SqlDbType.VarChar) { Value = file });
                                commandInsertFiles.ExecuteNonQuery();
                                commandInsertFiles.Parameters.Clear();
                            }
                        }
                    }
                }
            }
        }
    }
}

public class ResponseMessage
{
    public string MESSAGE { get; set; }
    public string MESSAGE_TYPE { get; set; }
}

public class ResponseUploadFiles
{
    public int IS_UPLOADED { get; set; }
    public int IS_INVALID_EXTENSION { get; set; }
    public List<string> FILE_UPLOADED { get; set; }
}
public class ResponseDataTypeLocation
{
    public int ID { get; set; }
    public string Nama { get; set; }
}

public class ResponseDataLocation
{
    public int IDLokasi { get; set; }
    public string NamaOutlet { get; set; }
    public string KodeOutlet { get; set; }
}