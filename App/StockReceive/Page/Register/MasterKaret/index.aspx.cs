using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Web;
using System.IO;
using System.Reflection;
using System.Text;

public partial class MasterKaret : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static ISelectOption GetSelectOption()
    {
        var getItem = Db.Rs("SELECT DISTINCT Item [Value] FROM MasterKaretToMaterial WHERE NULLIF(Item, '') IS NOT NULL", Db.Cnn_IN);
        var getSatuan = Db.Rs("SELECT DISTINCT Satuan [Value] FROM MasterKaretToMaterial WHERE NULLIF(Satuan, '') IS NOT NULL", Db.Cnn_IN);
        var getDist = Db.Rs("SELECT DISTINCT Dist [Value] FROM MasterKaretToMaterial WHERE NULLIF(Dist, '') IS NOT NULL", Db.Cnn_IN);
        var getColor = Db.Rs("SELECT DISTINCT Color [Value] FROM MasterKaretToMaterial WHERE NULLIF(Color, '') IS NOT NULL", Db.Cnn_IN);


        List<IString> Item = Helper.GetDataFromDataTable<IString>(getItem);
        List<IString> Satuan = Helper.GetDataFromDataTable<IString>(getSatuan);
        List<IString> Dist = Helper.GetDataFromDataTable<IString>(getDist);
        List<IString> Color = Helper.GetDataFromDataTable<IString>(getColor);

        return new ISelectOption
        {
            Item = Item,
            Satuan = Satuan,
            Dist = Dist,
            Color = Color
        };
    }

    [WebMethod]
    public static object InsertMasterMaterialExcel(List<IMasterMaterialExcel> input)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Db.Cnn_IN))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_validateMasterMaterial", connection))
                {

                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(input);
                    DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@MasterMaterial", SqlDbType.Structured) { Value = pDt });
                    command.Parameters.Add(new SqlParameter("@Command", SqlDbType.Int) { Value = 1 });

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    return new Dictionary<string, object>
                    {
                        { "Status", "success"},
                        { "Message", "Successfully insert the data!" },
                        { "DataAdded", Helper.GetDataFromDataTable<IMasterMaterial>(ds.Tables[0]) }
                    };
                }
            }
        }
        catch (Exception e)
        {
            return new Dictionary<string, string>
            {
                { "Status", "error"},
                { "Message", e.Message.ToString() }
            };
        }

    }

    [WebMethod]
    public static object ValidateMasterMaterial(List<IMasterMaterialExcel> input)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_IN))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("sp_validateMasterMaterial", connection))
            {

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(input);
                DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@MasterMaterial", SqlDbType.Structured) { Value = pDt });
                command.Parameters.Add(new SqlParameter("@Command", SqlDbType.Int) { Value = 0 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable messageTable = ds.Tables[0];
                List<IValidateDuplicateValues> DuplicateValues = new List<IValidateDuplicateValues>();

                if (ds.Tables.Count > 1)
                {
                    DuplicateValues = Helper.GetDataFromDataTable<IValidateDuplicateValues>(ds.Tables[1]);
                }

                List<IValidateMasterMessage> messages = Helper.GetDataFromDataTable<IValidateMasterMessage>(messageTable);

                string Message = "";
                string TypeMessage = "";
                if (messages.Count > 0)
                {
                    Message = messages[0].Message;
                    TypeMessage = messages[0].TypeMessage;
                }

                return new
                {
                    Message = Message,
                    TypeMessage = TypeMessage,
                    DuplicateValues = DuplicateValues
                };
            }
        }
    }

    [WebMethod]
    public static object InsertMasterMaterial(IInputInsertMaster input)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Db.Cnn_IN))
            {
                connection.Open();

                List<IErrorMessage> ErrorMessage = new List<IErrorMessage>();
                string[] columns = { "FinalSAP", "KodeKaretJaws" };

                Dictionary<string, string> columnToInputMapping = new Dictionary<string, string>
                    {
                        { "FinalSAP", "InputKodeSAP" },
                        { "KodeKaretJaws", "InputKodeKaretJaws" }
                    };


                foreach (var column in columns)
                {
                    using (SqlCommand command = new SqlCommand("SELECT COUNT(1) FROM MasterKaretToMaterial WHERE " + column + " = @InputNotes", connection))
                    {

                        PropertyInfo propertyInfo = typeof(IInputInsertMaster).GetProperty(columnToInputMapping[column]);
                        string values = propertyInfo.GetValue(input).ToString();

                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@InputNotes", SqlDbType.VarChar) { Value = values });
                        int recordCount = (int)command.ExecuteScalar();

                        // If the record does exist, return an error
                        if (recordCount > 0)
                        {
                            ErrorMessage.Add(new IErrorMessage
                            {
                                InputColumn = columnToInputMapping[column],
                                Column = column,
                                Value = values,
                                Message = "Inserted data has duplicate values!"

                            });
                        }
                    }
                }

                if (ErrorMessage.Count > 0)
                {
                    return new Dictionary<string, object>
                    {
                        { "Status", "error" },
                        { "Message", "ERROR: Duplicate Values!" },
                        { "MessageProperties", ErrorMessage }
                    };
                }

                using (SqlCommand command = new SqlCommand("" +
                    "INSERT INTO MasterKaretToMaterial (KodeKaret, KodeKaretJaws, FinalSAP, Item, Satuan, Dist, Color, KetUploadSAP, IsUploadedToSAP) VALUES " +
                     "(@InputKodeKaret, @InputKodeKaretJaws, @InputKodeSAP, @InputItem, @InputSatuan, @InputDist, @InputColor, @InputNotes, @InputStatus)"
                     , connection))

                {

                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@InputKodeKaret", SqlDbType.VarChar) { Value = input.InputKodeKaret });
                    command.Parameters.Add(new SqlParameter("@InputKodeKaretJaws", SqlDbType.VarChar) { Value = input.InputKodeKaretJaws });
                    command.Parameters.Add(new SqlParameter("@InputKodeSAP", SqlDbType.VarChar) { Value = input.InputKodeSAP });
                    command.Parameters.Add(new SqlParameter("@InputItem", SqlDbType.VarChar) { Value = input.InputItem });
                    command.Parameters.Add(new SqlParameter("@InputSatuan", SqlDbType.VarChar) { Value = input.InputSatuan });
                    command.Parameters.Add(new SqlParameter("@InputDist", SqlDbType.VarChar) { Value = input.InputDist });
                    command.Parameters.Add(new SqlParameter("@InputColor", SqlDbType.VarChar) { Value = input.InputColor });
                    command.Parameters.Add(new SqlParameter("@InputNotes", SqlDbType.VarChar) { Value = input.InputNotes });
                    command.Parameters.Add(new SqlParameter("@InputStatus", SqlDbType.Int) { Value = input.InputStatus });

                    // Execute the insert command and return success message if rows are affected
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return new Dictionary<string, string>
                        {
                            { "Status", "success"},
                            { "Message", "Successfully insert the data!" }
                        };
                    }
                    else
                    {
                        return new Dictionary<string, string>
                        {
                            { "Status", "error"},
                            { "Message", "ERROR: No rows affected. Data is failed to insert!" }
                        };
                    }
                }
            }
        }
        catch (Exception error)
        {
            return new Dictionary<string, string>
            {
                { "Message", error.StackTrace.ToString()}
            };
        }
    }


    [WebMethod]
    public static string UpdateNotesMasterMaterial(string InputNotes, int ID, string ColumnName)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Db.Cnn_IN))
            {

                connection.Open();
                // Create a dictionary with keys of type string and values of any type
                string[] columns = { "KodeKaretJaws", "FinalSAP" };

                if (Array.Exists(columns, column => column == ColumnName))
                {
                    using (SqlCommand command = new SqlCommand("SELECT COUNT(1) FROM MasterKaretToMaterial WHERE " + ColumnName + " = @InputNotes", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@InputNotes", SqlDbType.VarChar) { Value = InputNotes });


                        int recordCount = (int)command.ExecuteScalar();

                        // If the record does exist, return an error
                        if (recordCount > 0)
                        {
                            return "ERROR: Can't update data due to duplicate values!";
                        }
                    }
                }


                using (SqlCommand command = new SqlCommand("" +
                    "UPDATE MasterKaretToMaterial SET " + ColumnName + " = @InputNotes WHERE ID = @ID"
                     , connection))

                {

                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@InputNotes", SqlDbType.VarChar) { Value = InputNotes });
                    command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = ID });

                    // Execute the insert command and return success message if rows are affected
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return "success";
                    }
                    else
                    {
                        return "ERROR: No rows affected. Data is failed to update!";
                    }
                }
            }
        }
        catch (Exception error)
        {
            return error.Message.ToString();
        }
    }


    [WebMethod]
    public static string UpdateStatusMasterMaterial(string InputStatus, int ID)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Db.Cnn_IN))
            {
                using (SqlCommand command = new SqlCommand("" +
                    "UPDATE MasterKaretToMaterial SET IsUploadedToSAP = @InputStatus WHERE ID = @ID"
                     , connection))

                {

                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@InputStatus", SqlDbType.VarChar) { Value = InputStatus });
                    command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = ID });

                    // Open connection
                    connection.Open();

                    // Execute the insert command and return success message if rows are affected
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return "success";
                    }
                    else
                    {
                        return "ERROR: No rows affected. Data is failed to insert!";
                    }
                }
            }
        }
        catch (Exception error)
        {
            return error.Message.ToString();
        }
    }

    [WebMethod]
    public static Dictionary<string, object> GetMasterMaterial(IArgsFilterGetMaster filter)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Db.Cnn_IN))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("" +
                    "SELECT * FROM MasterKaretToMaterial WHERE " +
                     "(@Keywords = '' OR KodeKaretJAWS = @Keywords OR FinalSAP = @Keywords)" +
                        " AND " +
                        "(@Item = 'ALL' OR Item = @Item)" +
                        " AND " +
                        "(@Satuan = 'ALL' OR Satuan = @Satuan)" +
                        " AND " +
                        "(@Dist = 'ALL' OR Dist = @Dist)" +
                        " AND" +
                        "(@Color = 'ALL' OR Color = @Color)" +
                        " AND " +
                        "(@Status = -1 OR IsUploadedToSAP = @Status)"
                        , connection))

                {

                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@Keywords", SqlDbType.VarChar) { Value = filter.Keywords });
                    command.Parameters.Add(new SqlParameter("@Item", SqlDbType.VarChar) { Value = filter.Item });
                    command.Parameters.Add(new SqlParameter("@Satuan", SqlDbType.VarChar) { Value = filter.Satuan });
                    command.Parameters.Add(new SqlParameter("@Dist", SqlDbType.VarChar) { Value = filter.Dist });
                    command.Parameters.Add(new SqlParameter("@Color", SqlDbType.VarChar) { Value = filter.Color });
                    command.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar) { Value = filter.Status });

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dataTable = ds.Tables[0];

                    List<IMasterMaterial> masterMaterials = Helper.GetDataFromDataTable<IMasterMaterial>(dataTable);
                    return new Dictionary<string, object>
                    {
                        {"Message", "Successfully fetch the data" },
                        {"MessageType", "SUCCESS" },
                        {"Data", masterMaterials}
                    };
                }
            }
        }
        catch (Exception e)
        {
            return new Dictionary<string, object>
            {
                {"Message", e.Message.ToString() },
                {"MessageType", "ERROR"},
                {"StackTrace", e.StackTrace.ToString() }
            };
        }

    }

    public class IString
    {
        public string Value { get; set; }
    }

    public class ISelectOption
    {
        public List<IString> Item { get; set; }
        public List<IString> Satuan { get; set; }
        public List<IString> Dist { get; set; }
        public List<IString> Color { get; set; }
    }

    public class IInputInsertMaster
    {
        public string InputKodeKaret { get; set; }
        public string InputKodeKaretJaws { get; set; }
        public string InputKodeSAP { get; set; }
        public string InputItem { get; set; }
        public string InputSatuan { get; set; }
        public string InputDist { get; set; }
        public string InputColor { get; set; }
        public string InputNotes { get; set; }
        public int InputStatus { get; set; }
    }

    public class IArgsFilterGetMaster
    {
        public string Keywords { get; set; }
        public string Item { get; set; }
        public string Satuan { get; set; }
        public string Dist { get; set; }
        public string Color { get; set; }
        public string Status { get; set; }
    }

    public class IMasterMaterial
    {
        public string KodeKaret { get; set; }
        public string KodeKaretJaws { get; set; }
        public string FinalSAP { get; set; }
        public string Item { get; set; }
        public string Satuan { get; set; }
        public string Dist { get; set; }
        public string Color { get; set; }
        public string KetUploadSAP { get; set; }
        public int IsUploadedToSAP { get; set; }
        public int ID { get; set; }
    }

    public class IMasterMaterialExcel
    {
        public string KodeKaret { get; set; }
        public string KodeKaretJaws { get; set; }
        public string FinalSAP { get; set; }
        public string Item { get; set; }
        public string Satuan { get; set; }
        public string Dist { get; set; }
        public string Color { get; set; }
        public string KetUploadSAP { get; set; }
        public string IsUploadedToSAP { get; set; }
    }

    public class IErrorMessage
    {
        public string Message { get; set; }
        public string Column { get; set; }
        public string InputColumn { get; set; }
        public string Value { get; set; }
    }

    public class IValidateMasterMessage
    {
        public string Message { get; set; }
        public string TypeMessage { get; set; }
    }

    public class IValidateDuplicateValues
    {
        public string KodeKaretJaws { get; set; }
        public string FinalSAP { get; set; }
    }
}