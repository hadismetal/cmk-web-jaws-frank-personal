using System;
using System.Data.SqlClient;
using System.Linq;
using System.Data;

public partial class Menu : System.Web.UI.UserControl
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
		if (!IsPostBack)
		{
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
			App.MenuPath(a1, "/Page/Create/");
			App.MenuPath(a2, "/Page/Report/");


			string queryIsUserIT = "SELECT COUNT(*) [Count] FROM frank_ac.dbo.UserAccount WHERE (Keterangan LIKE '%IT DEV%' OR Keterangan LIKE '%IT OP%' OR Keterangan LIKE '%BUSINESS AN%' OR Keterangan LIKE '%BUSINESS PR%' OR Keterangan LIKE '%QUALITY ASSURANCE%') AND Status = 1 AND ID = " + App.UserID;
			int rs = Db.SingleInteger(queryIsUserIT, Db.Cnn_DB);

			string queryISservicePoint = @"select count(*) [Count]
                FROM RepairMasterLocationProvince a
                JOIN LocOutlet b on b.Kode=a.Kode
                WHERE B.ID IN (SELECT IDLokasi FROM DataAdminSales a JOIN frank_ac.dbo.UserAccount b on b.UserNumber=LTRIM(RTRIM(a.Keterangan)) AND b.ID = " + App.UserID + ")";
			int ISSC = Db.SingleInteger(queryISservicePoint, Db.Cnn_DB);
		
			//Role Protection
			a1.Visible = Role.Operator;
			a2.Visible = Role.Operator;
			a5.Visible = ISSC > 0 || rs > 0;
			a3.Visible = Role.Manager;
			FilterPLUSetting();
			FilterMenuJR();
			FilterMenuSM();
			FilterMenuAM();
			FilterMenuGM();
			FilterAll(rs);			


		}
	}

	protected void FilterPLUSetting()
	{

		string queryGetDataJabatan = @"
			select top 1 a.ID
			from frank_ac.dbo.UserAccount a
			JOIN [DB_PROD_MYAPPS].dbhrd.dbo.mskaryawan b on b.m_nik COLLATE SQL_Latin1_General_CP1_CI_AS = a.UserNumber COLLATE SQL_Latin1_General_CP1_CI_AS
			JOIN [DB_PROD_MYAPPS].dbhrd.dbo.mssubdivisinew c on c.m_idsubdiv=b.m_subdivisinew
			where ((b.m_departemen = 01 and b.m_subdivisinew=68 /*and b.m_jabatan like '%MANAGER%'*/ AND b.m_tglkeluar = '1900-01-01 00:00:00.000') or (b.m_departemen = 95))
			AND a.ID = @IDUser 
		";
		using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
		{
			using (SqlCommand command = new SqlCommand(queryGetDataJabatan, connection))
			{
				command.CommandType = CommandType.Text;
				command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = App.UserID });

				SqlDataAdapter adapter = new SqlDataAdapter(command);
				DataSet ds = new DataSet();
				adapter.Fill(ds);
				DataTable dt = ds.Tables[0];
				int RowCount = dt.Rows.Count;
				if (RowCount > 0)
				{
					RG9.Visible = true;
				}
				else
				{
					RG9.Visible = false;
				}
			}
		}
	}
	protected void FilterMenuJR()
	{
		string queryCheckJR = @"SELECT * FROM FRANK_AC.DBO.UserAccount A
			JOIN DataAdminSales B ON B.Keterangan = a.UserNumber
			where Status=1 and 
			(A.Keterangan like '%JEWELLERY REPRESENTATIVE%' OR A.Keterangan like '%ADMINISTRATIVE EXECUTIVE')
			AND A.ID = " + App.UserID;
		int rsJR;
		try{
			rsJR = Db.SingleInteger(queryCheckJR, Db.Cnn_DB);
		}catch{
			rsJR = 0;
		}
		if (rsJR > 0 )
		{
			A11.Visible = true;A12.Visible = true;A13.Visible = true;			
			RG1.Visible = true;RG8.Visible = true;RG3.Visible = true;
			RG4.Visible = true;RG2.Visible = true; // RG6.Visible = true;
			RG7.Visible = true;
			RG5.Visible = false;RG9.Visible = false;RE3.Visible = false;
			AP1.Visible = false; AP2.Visible = false; AP3.Visible = false; AP4.Visible = false;
		}
	}
	protected void FilterMenuSM()
	{
		string queryCheckJR = @"SELECT * FROM FRANK_AC.DBO.UserAccount A
			JOIN DataAdminSales B ON B.Keterangan = a.UserNumber
			where Status=1 and 
			(A.Keterangan like '%STORE MANAGER%' OR A.Keterangan like '%JEWELLERY COORDINATOR')
			AND A.ID = " + App.UserID;
		int rsJR;
		try{
			rsJR = Db.SingleInteger(queryCheckJR, Db.Cnn_DB);
		}catch{
			rsJR = 0;
		}
		if (rsJR > 0 )
		{
			A11.Visible = false; A12.Visible = false; A13.Visible = false;
			RG1.Visible = false; RG8.Visible = false; RG3.Visible = false;
			RG4.Visible = false; RG2.Visible = false; // RG6.Visible = false;
			RG7.Visible = false;
			RG5.Visible = false; RG9.Visible = false; RE3.Visible = false;
			AP1.Visible = true; AP2.Visible = true; AP3.Visible = true; AP4.Visible = true;
		}
	}		
	protected void FilterMenuAM()
	{
		string queryCheckJR = @"SELECT * FROM FRANK_AC.DBO.UserAccount A
			JOIN DataAdminSales B ON B.Keterangan = a.UserNumber
			where Status=1 and 
			(A.Keterangan like '%AREA MANAGER%')
			AND A.ID = " + App.UserID;
		int rsJR;
		try{
			rsJR = Db.SingleInteger(queryCheckJR, Db.Cnn_DB);
		}catch{
			rsJR = 0;
		}
		if (rsJR > 0 )
		{
			A11.Visible = false; A12.Visible = false; A13.Visible = false;
			RG1.Visible = false; RG8.Visible = false; RG3.Visible = false;
			RG4.Visible = false; RG2.Visible = false; RG6.Visible = false;
			RG7.Visible = false; RG5.Visible = false; RG9.Visible = false;
			AP1.Visible = true; AP2.Visible = true; AP3.Visible = true; AP4.Visible = true;
		}
	}
	protected void FilterMenuGM()
	{
		string queryCheckJR = @"SELECT * FROM FRANK_AC.DBO.UserAccount A		
		where Status=1 and 
		(A.Keterangan like '%GENERAL MANAGER%')
		AND A.ID = " + App.UserID;
		int rsJR;
		try{
			rsJR = Db.SingleInteger(queryCheckJR, Db.Cnn_DB);
		}catch{
			rsJR = 0;
		}
		if (rsJR > 0 )
		{
			A11.Visible = false; A12.Visible = false; A13.Visible = false;
			RG1.Visible = false; RG8.Visible = false; RG3.Visible = false;
			RG4.Visible = false; RG2.Visible = false; RG6.Visible = false;
			RG7.Visible = false; RG5.Visible = false; RG9.Visible = false;
			AP1.Visible = true; AP2.Visible = true; AP3.Visible = true; AP4.Visible = true;
		}
	}
	protected void FilterAll(int rs)
	{
		if (rs > 0)
		{
			A11.Visible = true; A12.Visible = true; A13.Visible = true;
			RG1.Visible = true; RG8.Visible = true; RG3.Visible = true;
			RG4.Visible = true; RG2.Visible = true; RG6.Visible = true;
			RG7.Visible = true;
			RG5.Visible = true; RG9.Visible = true; RE3.Visible = true;
			AP1.Visible = true; AP2.Visible = true; AP3.Visible = true; AP4.Visible = true;
		}
	}
}