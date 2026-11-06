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
using System.Globalization;



public partial class DOKRM_index : System.Web.UI.Page
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

        decimal paid = Db.SingleDecimal("SELECT B.TotalBayar FROM DocRepairResult A JOIN SalesReceipt B ON A.ID = B.IDRepair", Db.Cnn_DB);
        // var DocRepairResult = from p in db.DocRepairResult where p.IDWorkOrder == myID select p;
        // var idDocRepairResult = DocRepairResult.Max()
        var idResult = db.DocRepairResult.OrderByDescending(u => u.ID).FirstOrDefault(u => u.IDWorkOrder == myID);
        detailPaymentControlR.SetDetailPayment("SO", idResult.ID, 0, paid);


        if (!IsPostBack)
        {
        }
    }

    static string UppercaseWords(string text)
    {


        return char.ToUpper(text[0]) +
            ((text.Length > 1) ? text.Substring(1).ToLower() : string.Empty);

    }



    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetDetailInvoice(int IDDocRepair)
    {
        DataSet ds = ExecuteDocRepair(IDDocRepair, 1);
        DataSet ds1 = ExecuteDocRepairInstruction(IDDocRepair);
        DataSet ds2 = ExecuteDocRepair(IDDocRepair, 2);


        DataTable dt = ds.Tables[0];
        DataTable dt1 = ds1.Tables[0];
        DataTable dt3 = ds1.Tables[1];
        DataTable dt2 = ds2.Tables[0];

        decimal totalBayar = 0m;        
        decimal beforeTax = 0m;
        decimal Tax = 0m;
        decimal nominalDiscount = 0m;

        List<DataInstruksi> docRepairInstruksi = new List<DataInstruksi>();
        
        foreach (DataRow dr in dt1.Rows)
        {
            totalBayar = totalBayar + Convert.ToDecimal(dr["BiayaJual"]);    
        }
        foreach (DataRow diskon in dt3.Rows)
        {
            nominalDiscount += Convert.ToDecimal(diskon["NominalDiscount"]);            
        }
        if (nominalDiscount > 0) { totalBayar -= nominalDiscount; }

        foreach (DataRow dr in dt.Rows)
        {
            beforeTax = totalBayar / Convert.ToDecimal(dr["PPN"]);
            Tax = totalBayar - (totalBayar / Convert.ToDecimal(dr["PPN"]));
            docRepairInstruksi.Add(new DataInstruksi
            {
                NomorRepair = (string)dr["NomorRepair"],
                TglRepair = dr["TglRepair"].ToString(),
                NomorInvoice = (string)dr["NomorInvoice"],
                NomorSO = (string)dr["NomorSO"],
                SalesNama = (string)dr["SalesNama"],
                CustomerNama = (string)dr["CustomerNama"],
                NoCustomer = (string)dr["NoCustomer"],
                AddrAlamat = (string)dr["AddrAlamat"],
                AfterTax = totalBayar, // or Convert.ToDecimal(...) in dt2
                BeforeTax = beforeTax,
                Tax = Tax,
                NamaItem = (string)dr["NamaItem"],
                TglNow = dr["TglNow"].ToString(),
                Store = (string)dr["Store"],
                IsUsePPN = Convert.ToInt32(dr["IsUsePPN"])
            });

        }

        foreach (DataRow dr1 in dt2.Rows)
        {
            if (Convert.ToDecimal(dr1["AfterTax"]) > 0)
            {
                docRepairInstruksi.Add (new DataInstruksi
                {
                    NomorRepair = (string)dr1["NomorRepair"],
                    TglRepair = dr1["TglRepair"].ToString(),
                    NomorInvoice = (string)dr1["NomorInvoice"],
                    NomorSO = (string)dr1["NomorSO"],
                    SalesNama = (string)dr1["SalesNama"],
                    CustomerNama = (string)dr1["CustomerNama"],
                    NoCustomer = (string)dr1["NoCustomer"],
                    AddrAlamat = (string)dr1["AddrAlamat"],
                    AfterTax = Convert.ToDecimal(dr1["AfterTax"]),
                    BeforeTax = Convert.ToDecimal(dr1["BeforeTax"]),
                    Tax = Convert.ToDecimal(dr1["Tax"]),
                    NamaItem = (string)dr1["NamaItem"],
                    TglNow = dr1["TglNow"].ToString(),
                    Store = (string)dr1["Store"],
                    IsUsePPN = Convert.ToInt32(dr1["IsUsePPN"])
                });
            }
        }

        return docRepairInstruksi;
    }

    public static DataSet ExecuteDocRepair(int IDDocRepair, int Tipe)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getDetailInvoiceRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.Int) { Value = IDDocRepair });
                command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = Tipe });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }
    public static DataSet ExecuteDocRepairInstruction(int IDDocRepair)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getDetailPackageRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.Int) { Value = IDDocRepair });                

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }


    public class DataInstruksi
    {
        public string NomorRepair { get; set; }
        public string TglRepair { get; set; }
        public string NomorInvoice { get; set; }
        public string SalesNama { get; set; }
        public string CustomerNama { get; set; }
        public string NomorSO { get; set; }
        public string NoCustomer { get; set; }
        public string AddrAlamat { get; set; }
        public decimal AfterTax { get; set; }
        public decimal BeforeTax { get; set; }
        public decimal Tax { get; set; }
        public string NamaItem { get; set; }
        public string TglNow { get; set; }
        public string Store { get; set; }
        public int IsUsePPN { get; set; }
    }
}