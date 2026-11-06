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
using System.Net;
using System.IO;
using System.Web;
using System.Configuration;
using System.Web.Script.Serialization;

public partial class Page_Register_Titipan_Form : System.Web.UI.Page {
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

    string ID = "";

    protected int myID { get { return App.QueryInt(this, "id"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected int myIDLokasi { get { return App.QueryInt(this, "idlokasi"); } }
    protected int status { get { return App.QueryInt(this, "status"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			keterangan.Focus();

			//Form
			fillForm();

            if (myID > 0)
            {
                customerSearch(myID);
            }

            //Form-Default
            tgl.Text = Str.Tgl(DateTime.Today);
            tgl.Enabled = false;
		}
	}

    protected void customerSearch(int xID)
    {
        search_customer.HRef = (STAMPS.xActivate) ? "/OutletSales/Page/Create/DataCustomerStamps.aspx" : search_customer.HRef;
        search_customer.HRef += "?id=" + xID;
        search_customer.HRef += (STAMPS.xActivate) ? "&mod=TITIPAN" : "";
    }

    //Exception
    protected void DataError() { Response.Redirect("Titipan.aspx"); }

	//Form
	protected void fillForm() {
        if (status != 1)
        {
            var d = db.LocOutlet.SingleOrDefault(p => p.ID == myIDLokasi && !p.Disable);
            if (d == null) DataError();
            else
            {
                warehouse.InnerHtml = d.Nama;


                DocTitipan n = new DocTitipan();

                //Location
                n.IDLokasi = myIDLokasi;
                n.TipeLokasi = tipe;

                //Status
                n.Draft = true;
                n.DraftDate = DateTime.Now;

                //User-Info
                n.Operator = App.UserNama;
                n.OperatorTgl = DateTime.Now;

                n.Keterangan = keterangan.Text;

                //Commit
                db.DocTitipan.InsertOnSubmit(n);
                db.SubmitChanges();

                string qstr = "?id=" + n.ID;
                customerSearch(n.ID);
                search_agent.HRef += qstr;
                ID = n.ID.ToString();
                

            }
        }
        else
        {
            var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
            if (d == null) DataError();
            else
            {
                string Brand = "";
                if (d.TipeLokasi == 1)
                {
                    var k = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                    Brand = k.Nama;
                }
                else if (d.TipeLokasi == 2)
                {
                    var k = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                    Brand = k.Nama;
                }

                warehouse.InnerHtml = Brand;
                if(d.IDCustomer !=null)
                    customer.Text = d.DataCustomer.Nama;
                if (d.IDSales != null)
                    agent.Text = d.DataSales.Nama;
            }
            string qstr = "?id=" + myID;
            search_customer.HRef += qstr;
            search_agent.HRef += qstr;
            ID = myID.ToString();
        }
	}

	//Save
	protected bool valid {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
    protected bool validCustomer
    {
        get
        {
            if (customer.Text == "")
                return false;
            else
                return true;
        }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (valid && validCustomer) {
            var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID);
            if (d != null)
            {
                d.Keterangan = Str.CapFirst(keterangan.Text);
                //d.Tgl = Convert.ToDateTime(tgl.Text);
                d.Tgl = System.DateTime.Now;
                //d.Nomor = Generate_Nomor(d);
                d.Nomor = "";

                db.SubmitChanges();
            }

            Response.Redirect("Titipan_Data.aspx?id=" + myID);
		}
	}
    [WebMethod]
    public static List<Message> SaveData(int IDDoc, string Keterangan, int TipeTitipan)
    {
        var list = new List<Message>();
        string operatorName = HttpContext.Current.User.Identity.Name;
        DateTime tanggalAwal = DateTime.Now;
        

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            string insertData = @"
            IF Exists(select 1 from DocTitipan where ID = @IDDoc)
            BEGIN
                UPDATE DocTitipan
                SET Keterangan = @Keterangan, IDMasterTipeTitipan = @TipeTitipan, Tgl = CAST(GETDATE() AS DATE), Nomor = ''
                WHERE ID = @IDDoc
            END
         ";
            using (SqlCommand command = new SqlCommand(insertData, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDDoc", SqlDbType.Int) { Value = IDDoc });
                command.Parameters.Add(new SqlParameter("@Keterangan", SqlDbType.VarChar) { Value = Str.CapFirst(Keterangan) });
                command.Parameters.Add(new SqlParameter("@TipeTitipan", SqlDbType.Int) { Value = TipeTitipan });

                try                {
                    command.ExecuteNonQuery();
                    list.Add(new Message
                    {
                        StatusCode = 200,
                        MessageText = "Data berhasil disimpan"                        
                    });
                }
                catch (Exception ex)
                {                    
                    list.Add(new Message
                    {
                        StatusCode = 500,
                        MessageText = ex.Message
                    });                    
                }                
            }            
            connection.Close();        
        }
        return list;
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<DataTipeTitipan> GetTipeTitipan()
    {
        var list = new List<DataTipeTitipan>();
        DataSet ds = new DataSet();
        using(SqlConnection  connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            string query = @"SELECT ID, Nama FROM MasterTipeTitipan WHERE Disable = 0";
            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);
                DataTable dataTipeTitip = ds.Tables[0];                

                foreach (DataRow row in dataTipeTitip.Rows)
                {
                    list.Add(new DataTipeTitipan
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Nama = Convert.ToString(row["Nama"])
                    });
                }                
            }
            connection.Close();
        }
        return list;
        
    }

    public class DataTipeTitipan
    {
        public int ID { get; set; }
        public string Nama { get; set; }
    }
    public class Message
    {
        public int StatusCode { get; set; }
        public string MessageText { get; set; }
    }

    
}