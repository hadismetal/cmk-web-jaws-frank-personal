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
        // detailPaymentControlR.SetDetailPayment("SO", idResult.ID,  0, paid);


        if (!IsPostBack)
        {
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetDetailVouocher(int IDDocRepair)
    {
        return true;    
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetDetailInvoice(int IDDocRepair) {
        DataSet ds = ExecuteDocRepair(IDDocRepair);
        DataSet ds1 = ExecuteDocRepairInstruction(IDDocRepair);

        DataTable dt = ds.Tables[0];
        DataTable dt1 = ds1.Tables[0];
        DataTable dt2 = ds1.Tables[1];

        decimal totalBayar = 0m;
        decimal beforeTax = 0m;
        decimal Tax = 0m;
        decimal nominalDiscount = 0m;

        List<object> Data = new List<object>();
        foreach (DataRow dr in dt1.Rows)
        {
            totalBayar += Convert.ToDecimal(dr["BiayaJual"]);
        }
        foreach (DataRow diskon in dt2.Rows)
        {
            nominalDiscount += Convert.ToDecimal(diskon["NominalDiscount"]);            
        }
        if (nominalDiscount > 0) { totalBayar -= nominalDiscount; }

        foreach (DataRow dr in dt.Rows)
        {
            beforeTax = totalBayar / Convert.ToDecimal(dr["PPN"]);
            Tax = totalBayar - (totalBayar / Convert.ToDecimal(dr["PPN"]));
            Data.Add(new
            {
                NomorRepair = (string)dr["NomorRepair"],
                TglRepair = dr["TglRepair"].ToString(),
                NomorInvoice = (string)dr["NomorInvoice"],
                NomorSO = (string)dr["NomorSO"],
                SalesNama = (string)dr["SalesNama"],
                CustomerNama = (string)dr["CustomerNama"],
                NoCustomer = (string)dr["NoCustomer"],
                AddrAlamat = (string)dr["AddrAlamat"],
                AfterTax = totalBayar,
                BeforeTax = beforeTax,
                Tax = Tax,
                NamaItem = (string)dr["NamaItem"],
                TglNow = dr["TglNow"].ToString(),
                Store = (string)dr["Store"],
                IsUsePPN = Convert.ToInt32(dr["IsUsePPN"])
            });
        }
        return Data[0];
    }

    public static DataSet ExecuteDocRepair(int IDDocRepair)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getDetailInvoiceRepair", connection))
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
}