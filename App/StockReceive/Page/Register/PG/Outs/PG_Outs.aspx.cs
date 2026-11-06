using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;
using System.Text;

public partial class Page_Register_PG : System.Web.UI.Page
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
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static dynamic GetProductInfo()
    {
        try
        {
            string[] tables = { "CharGoldLevel", "CharFrameColor", "CharProductItem", "CharGoldModel", "CharTargetAge" };
            Dictionary<string, string> mapTableToSelect = new Dictionary<string, string>
            {
                { "CharGoldLevel", "SelectGoldLevel" },
                { "CharFrameColor", "SelectFrameColor" },
                { "CharProductItem", "SelectProductItem" },
                { "CharGoldModel", "SelectGoldModel" },
                { "CharTargetAge", "SelectTargetAge" }
            };

            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();

            using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
            {
                foreach (string table in tables)
                {
                    string query = table == "CharGoldModel" ? "SELECT ID, Nama, NamaKode FROM CharGoldModel WHERE ID IN (1,2,3)" : "SELECT ID, Nama, NamaKode FROM " + table;
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        keyValuePairs.Add(mapTableToSelect[table], Helper.GetDataFromDataTable<IProductInfo>(ds.Tables[0]));
                    }
                }
            }

            return new Dictionary<string, object>
            {
                { "MESSAGE_TYPE", "SUCCESS" },
                { "MESSAGE", "" },
                { "DATA", keyValuePairs }
            };
        }
        catch (Exception error)
        {
            return new Dictionary<string, string>
            {
                { "MESSAGE_TYPE", "ERROR" },
                { "MESSAGE", error.Message.ToString() }
            };
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static dynamic GetProductModel(int IsFixedRate)
    {
        try
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();

            using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
            {
                string query = IsFixedRate == 0 ? "SELECT ID, Nama, NamaKode FROM CharGoldModel WHERE ID IN (1,2,3)" : "SELECT ID, Nama, NamaKode FROM CharGoldModel WHERE ID IN (4,5,6)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    return new Dictionary<string, object>
                    {
                        { "MESSAGE_TYPE", "SUCCESS" },
                        { "MESSAGE", "" },
                        { "DATA", Helper.GetDataFromDataTable<IProductInfo>(ds.Tables[0]) }
                    };
                }

            }


        }
        catch (Exception error)
        {
            return new Dictionary<string, string>
            {
                { "MESSAGE_TYPE", "ERROR" },
                { "MESSAGE", error.Message.ToString() }
            };
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static dynamic GetGoldRate()
    {
        try
        {
            string query = "SELECT TOP 1 CAST(Harga as DECIMAL(18,2)) FROM PricingTableTG WHERE ID = 41";
            decimal harga = Db.SingleDecimal(query, Db.Cnn_DB);

            return new Dictionary<string, object>
            {
                { "MESSAGE_TYPE", "SUCCESS" },
                { "MESSAGE", "" },
                { "DATA", harga }
            };
        }
        catch (Exception error)
        {
            return new Dictionary<string, string>
            {
                { "MESSAGE_TYPE", "ERROR" },
                { "MESSAGE", error.Message.ToString() }
            };
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static dynamic GetWarehouse()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
            {
                using (SqlCommand command = new SqlCommand("SELECT A.ID, A.IDGroup, A.Nama, B.NamaKode FROM LocWarehouse A JOIN LocWarehouseGroup B ON A.IDGroup = B.ID AND A.Disable = 0 AND B.Disable = 0", connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    return new Dictionary<string, object>
                    {
                        { "MESSAGE_TYPE", "SUCCESS" },
                        { "MESSAGE", "" },
                        { "DATA", Helper.GetDataFromDataTable<IWarehouse>(ds.Tables[0])}
                    };
                }
            }
        }
        catch (Exception error)
        {
            return new Dictionary<string, string>
            {
                { "MESSAGE_TYPE", "ERROR" },
                { "MESSAGE", error.Message.ToString() }
            };
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static dynamic GetSupplier()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
            {
                using (SqlCommand command = new SqlCommand("SELECT A.ID, A.IDGroup, A.Nama, B.Nama [NamaSupplierGroup] FROM DataSupplier A JOIN DataSupplierGroup B ON A.IDGroup = B.ID AND A.Disable = 0 AND B.Disable = 0 ORDER BY A.Nama", connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    return new Dictionary<string, object>
                    {
                        { "MESSAGE_TYPE", "SUCCESS" },
                        { "MESSAGE", "" },
                        { "DATA", Helper.GetDataFromDataTable<ISupplier>(ds.Tables[0])}
                    };
                }
            }
        }
        catch (Exception error)
        {
            return new Dictionary<string, string>
            {
                { "MESSAGE_TYPE", "ERROR" },
                { "MESSAGE", error.Message.ToString() }
            };
        }
    }

    [WebMethod]
    public static dynamic GeneratePLU(IStockReceiveGenerate stock, object UDT_PGOuts_PLU)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_generateStockReceivePGOuts", connection))
            {
                try
                {
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(UDT_PGOuts_PLU);
                    DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                    command.CommandType = CommandType.StoredProcedure;
                    Helper.AddParametersFromObject<IStockReceiveGenerate>(command, stock);
                    command.Parameters.Add(new SqlParameter("Operator", SqlDbType.VarChar) { Value = App.UserNama });
                    command.Parameters.Add(new SqlParameter("UDT_GeneratedPLU", SqlDbType.Structured) { Value = pDt });
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    if (ds.Tables.Count == 1)
                    {
                        return new Dictionary<string, object>
                        {
                            { "MESSAGE",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Message>(ds.Tables[0])[0] }
                        };
                    }
                    else
                    {
                        return new Dictionary<string, object>
                        {
                            { "MESSAGE",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Message>(ds.Tables[0])[0] },
                            { "DATA",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Data>(ds.Tables[1]) },
                            { "ADDITIONAL", Helper.GetDataFromDataTable<IStockReceiveGenerate_Print>(ds.Tables[2]) }
                        };
                    }
                }
                catch (Exception error)
                {
                    return error.Message.ToString();
                }
            }
        }
    }


    [WebMethod]
    public static dynamic UpdateGeneratedPLU(IStockReceiveUpdateProductAndDetail stock, object UDT_PGOuts_PLU)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_updateGeneratedStockReceivePGOuts", connection))
            {
                try
                {
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(UDT_PGOuts_PLU);
                    DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                    command.CommandType = CommandType.StoredProcedure;
                    Helper.AddParametersFromObject<IStockReceiveUpdateProductAndDetail>(command, stock);
                    command.Parameters.Add(new SqlParameter("Operator", SqlDbType.VarChar) { Value = App.UserNama });
                    command.Parameters.Add(new SqlParameter("UDT_GeneratedPLU", SqlDbType.Structured) { Value = pDt });
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    if (ds.Tables.Count == 1)
                    {
                        return new Dictionary<string, object>
                        {
                            { "MESSAGE",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Message>(ds.Tables[0])[0] }
                        };
                    }
                    else
                    {
                        return new Dictionary<string, object>
                        {
                            { "MESSAGE",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Message>(ds.Tables[0])[0] },
                            { "DATA",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Data>(ds.Tables[1]) },
                            { "ADDITIONAL", Helper.GetDataFromDataTable<IStockReceiveGenerate_Print>(ds.Tables[2]) }
                        };
                    }
                }
                catch (Exception error)
                {
                    return error.Message.ToString();
                }
            }
        }
    }

    [WebMethod]
    public static dynamic DeleteGeneratedPLU(object UDT_PGOuts_PLU_Group, object UDT_PGOuts_PLU_All, int IsSingle = 0, string PLU = "")
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_deleteGeneratedStockReceivePGOuts", connection))
            {
                try
                {
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(UDT_PGOuts_PLU_Group);
                    DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                    string json2 = Newtonsoft.Json.JsonConvert.SerializeObject(UDT_PGOuts_PLU_All);
                    DataTable pDt2 = JsonConvert.DeserializeObject<DataTable>(json2);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("UDT_GeneratedPLU_Group", SqlDbType.Structured) { Value = pDt });
                    command.Parameters.Add(new SqlParameter("UDT_GeneratedPLU_All", SqlDbType.Structured) { Value = pDt2 });
                    command.Parameters.Add(new SqlParameter("IsSingle", SqlDbType.Int) { Value = IsSingle });
                    command.Parameters.Add(new SqlParameter("PLU", SqlDbType.VarChar) { Value = PLU });
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    if (ds.Tables.Count == 1)
                    {
                        return new Dictionary<string, object>
                        {
                            { "MESSAGE",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Message>(ds.Tables[0])[0] }
                        };
                    }
                    else
                    {
                        return new Dictionary<string, object>
                        {
                            { "MESSAGE",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Message>(ds.Tables[0])[0] },
                            { "DATA",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Data>(ds.Tables[1]) },
                            { "ADDITIONAL",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Print>(ds.Tables[2]) },
                        };
                    }
                }
                catch (Exception error)
                {
                    return error.Message.ToString();
                }
            }
        }
    }

    [WebMethod]
    public static dynamic GenerateOrEditPLU(IStockReceiveUpdateOrDeleteProduct stock, object UDT_PGOuts_PLU)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_generateOrEditStockReceivePGOuts", connection))
            {
                try
                {
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(UDT_PGOuts_PLU);
                    DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                    command.CommandType = CommandType.StoredProcedure;
                    Helper.AddParametersFromObject<IStockReceiveUpdateOrDeleteProduct>(command, stock);
                    command.Parameters.Add(new SqlParameter("Operator", SqlDbType.VarChar) { Value = App.UserNama });
                    command.Parameters.Add(new SqlParameter("UDT_GeneratedPLU", SqlDbType.Structured) { Value = pDt });
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    if (ds.Tables.Count == 1)
                    {
                        return new Dictionary<string, object>
                        {
                            { "MESSAGE",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Message>(ds.Tables[0])[0] }
                        };
                    }
                    else
                    {
                        return new Dictionary<string, object>
                        {
                            { "MESSAGE",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Message>(ds.Tables[0])[0] },
                            { "DATA",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Data>(ds.Tables[1]) },
                            { "ADDITIONAL", Helper.GetDataFromDataTable<IStockReceiveGenerate_Print>(ds.Tables[2]) }
                        };
                    }
                }
                catch (Exception error)
                {
                    return error.Message.ToString();
                }
            }
        }
    }

    [WebMethod]
    public static dynamic PostingToStockProduct(object UDT_PGOuts_PLU)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_postingStockReceivePGOutsToStockProduct", connection))
            {
                try
                {
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(UDT_PGOuts_PLU);
                    DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("UDT_GeneratedPLU", SqlDbType.Structured) { Value = pDt });
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    if (ds.Tables.Count == 1)
                    {
                        return new Dictionary<string, object>
                        {
                            { "MESSAGE",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Message>(ds.Tables[0])[0] }
                        };
                    }
                    else
                    {
                        return new Dictionary<string, object>
                        {
                            { "MESSAGE",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Message>(ds.Tables[0])[0] },
                            { "DATA",  Helper.GetDataFromDataTable<IStockReceiveGenerate_Url>(ds.Tables[1])[0] }
                        };
                    }
                }
                catch (Exception error)
                {
                    return error.Message.ToString();
                }
            }
        }
    }

    public class IBarcode
    {
        public string PLU { get; set; }
        public string ITEM { get; set; }
        public string GROSSWEIGHT { get; set; }
        public string HARGA { get; set; }
        public string KADAR { get; set; }
        public string KODE { get; set; }
        public string COLOR { get; set; }
        public string DIMENSI { get; set; }
        public string MATERIAL { get; set; }
        public string RINGSIZE { get; set; }
    }

    public class IStockReceiveGenerate_Message
    {
        public string ERROR_MESSAGE { get; set; }
        public string MESSAGE_TYPE { get; set; }
        public string ERROR_NUMBER { get; set; }
    }

    public class IStockReceiveGenerate_Data
    {
        public int ID { get; set; }
        public string PLU { get; set; }
        public decimal Weight { get; set; }
        public decimal Harga24 { get; set; }
        public decimal Ongkos { get; set; }
        public decimal TotalHarga24 { get; set; }
        public decimal TotalHargaJual { get; set; }
        public decimal TotalBerat24 { get; set; }
        public decimal HPPM { get; set; }
        public decimal WeightGroup { get; set; }
        public string URL { get; set; }
    }

    public class IStockReceiveGenerate_Url
    {
        public string URL { get; set; }
    }
    public class IStockReceiveGenerate_Print
    {
        public decimal WeightGroup { get; set; }
        public int IDPRINT { get; set; }
        public string KODEBRAND { get; set; }
    }
    public class IStockReceiveGenerate
    {
        public int IDWarehouse { get; set; }
        public string Tanggal { get; set; }
        public string TanggalTJT { get; set; }
        public int IDSupplier { get; set; }
        public string NamaSupplier { get; set; }
        public string NamaSJSupplier { get; set; }
        public decimal GoldRate { get; set; }
        public int IsFixedRate { get; set; }
        public decimal RingSize { get; set; }
        public string Keterangan { get; set; }
        public string StockKeeper { get; set; }
        public int ProductLevel { get; set; }
        public int ProductModel { get; set; }
        public int ProductItem { get; set; }
        public int FrameColor { get; set; }
        public int TargetAge { get; set; }
        public decimal KadarLogam { get; set; }
        public decimal KadarTukaranJual { get; set; }
        public decimal KadarTukaranBeli { get; set; }
        public string KodeBarang { get; set; }
        public string NamaBarang { get; set; }
        public string KodeMaterial { get; set; }
        public string NomorValidasi { get; set; }
        public int Qty { get; set; }
        public decimal Weight { get; set; }
        public string NoCertificate { get; set; }
    }

    public class IStockReceiveUpdateOrDeleteProduct
    {
        public int Qty { get; set; }
        public decimal Weight { get; set; }
        public string NoCertificate { get; set; }
    }

    public class IStockReceiveUpdateProductAndDetail
    {
        public int IDWarehouse { get; set; }
        public string Tanggal { get; set; }
        public string TanggalTJT { get; set; }
        public int IDSupplier { get; set; }
        public string NamaSupplier { get; set; }
        public string NamaSJSupplier { get; set; }
        public decimal GoldRate { get; set; }
        public int IsFixedRate { get; set; }
        public decimal RingSize { get; set; }
        public string Keterangan { get; set; }
        public string StockKeeper { get; set; }
        public int ProductLevel { get; set; }
        public int ProductModel { get; set; }
        public int ProductItem { get; set; }
        public int FrameColor { get; set; }
        public int TargetAge { get; set; }
        public decimal KadarLogam { get; set; }
        public decimal KadarTukaranJual { get; set; }
        public decimal KadarTukaranBeli { get; set; }
        public string KodeBarang { get; set; }
        public string NamaBarang { get; set; }
        public string KodeMaterial { get; set; }
        public string NomorValidasi { get; set; }
    }

    public class IProductInfo
    {
        public int ID { get; set; }
        public string Nama { get; set; }
        public string NamaKode { get; set; }
    }

    public class IWarehouse
    {
        public int ID { get; set; }
        public int IDGroup { get; set; }
        public string Nama { get; set; }
        public string NamaKode { get; set; }
    }

    public class ISupplier
    {
        public int ID { get; set; }
        public int IDGroup { get; set; }
        public string Nama { get; set; }
        public string NamaSupplierGroup { get; set; }
    }
}