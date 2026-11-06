using System;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using System.Web;

public partial class Menu : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
			App.MenuPath(a1, "/Page/Register/");
			App.MenuPath(a2, "/Page/Report/");


			string queryIsUserIT = "SELECT COUNT(*) [Count] FROM frank_ac.dbo.UserAccount WHERE (Keterangan LIKE '%IT DEV%' OR Keterangan LIKE '%IT OP%' OR Keterangan LIKE '%BUSINESS AN%' OR Keterangan LIKE '%BUSINESS PR%' OR Keterangan LIKE '%QUALITY ASSURANCE%') AND Status = 1 AND ID = " + App.UserID;
			int rs = Db.SingleInteger(queryIsUserIT, Db.Cnn_AC);
			string GetJabatanSementara = @"SELECT ISNULL(LTRIM(RTRIM(TipeJabatan)),'') TipeJabatan FROM MasterJabatanPenggantiOpname WHERE CAST(GETDATE() AS DATE) = EfektifDate AND IDUser = " + App.UserID;
			string rss = Db.SingleString(GetJabatanSementara, Db.Cnn_DB) ?? "";   // default value

			// var c1 = new HttpCookie("OperatorName", App.UserNama);
			// c1.HttpOnly = true;
			// c1.Secure = true; // if HTTPS
			
			// var c2 = new HttpCookie("UserID", App.UserID.ToString());
			// c2.HttpOnly = true;
			// c2.Secure = true; // if HTTPS

			// Response.Cookies.Add(c1);
			// Response.Cookies.Add(c2);



			// //Role Protection
			// a1.Visible = Role.Operator;
			// a2.Visible = Role.Operator;
			// a5.Visible = !Role.Operator || rs > 0;
			// a3.Visible = Role.Manager || rs > 0;
			// FilterPLUSetting();			
			
			FilterMenuJR(rss);
			FilterMenuSM();
			
				
	
		}
	}

	protected void FilterMenuJR(string rss)
	{
		string queryCheckJR = @"SELECT COUNT(*) FROM frank_ac.DBO.UserAccount A
			JOIN DataAdminSales B ON B.Keterangan = a.UserNumber
			where Status=1 and 
			(A.Keterangan like '%JEWELLERY REPRESENTATIVE%' OR A.Keterangan like '%ADMINISTRATIVE EXECUTIVE' OR A.Keterangan like '%GOLD REPRESENTATIVE')
			AND A.ID = " + App.UserID;
		int rsJR;
		try{
			rsJR = Db.SingleInteger(queryCheckJR, Db.Cnn_DB);
		}catch{
			rsJR = 0;
		}
		if (rsJR > 0 )
		{			
			a3.Visible = rss.ToUpper() == "SM" ? true : false;
			RG2.Visible = false;
			RG3.Visible = false;
			RE1.Visible = false;
			RE3.Visible = false;
		}
	}
	protected void FilterMenuSM()
	{
		string queryCheckJR = @"SELECT * FROM frank_ac.DBO.UserAccount A
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
		}
	}		
	// protected void FilterMenuAM()
	// {
	// 	string queryCheckJR = @"SELECT * FROM frank_ac.DBO.UserAccount A
	// 		JOIN DataAdminSales B ON B.Keterangan = a.UserNumber
	// 		where Status=1 and 
	// 		(A.Keterangan like '%AREA MANAGER%')
	// 		AND A.ID = " + App.UserID;
	// 	int rsJR;
	// 	try{
	// 		rsJR = Db.SingleInteger(queryCheckJR, Db.Cnn_DB);
	// 	}catch{
	// 		rsJR = 0;
	// 	}
	// 	if (rsJR > 0 )
	// 	{
	// 		A11.Visible = false; A12.Visible = false; A13.Visible = false;
	// 		RG1.Visible = false; RG8.Visible = false; RG3.Visible = false;
	// 		RG4.Visible = false; RG2.Visible = false; RG6.Visible = false;
	// 		RG7.Visible = false; RG5.Visible = false; RG9.Visible = false;
	// 		AP1.Visible = true; AP2.Visible = true; AP3.Visible = true; AP4.Visible = true;
	// 	}
	// }
	// protected void FilterMenuGM()
	// {
	// 	string queryCheckJR = @"SELECT * FROM frank_ac.DBO.UserAccount A		
	// 	where Status=1 and 
	// 	(A.Keterangan like '%GENERAL MANAGER%')
	// 	AND A.ID = " + App.UserID;
	// 	int rsJR;
	// 	try{
	// 		rsJR = Db.SingleInteger(queryCheckJR, Db.Cnn_DB);
	// 	}catch{
	// 		rsJR = 0;
	// 	}
	// 	if (rsJR > 0 )
	// 	{
	// 		A11.Visible = false; A12.Visible = false; A13.Visible = false;
	// 		RG1.Visible = false; RG8.Visible = false; RG3.Visible = false;
	// 		RG4.Visible = false; RG2.Visible = false; RG6.Visible = false;
	// 		RG7.Visible = false; RG5.Visible = false; RG9.Visible = false;
	// 		AP1.Visible = true; AP2.Visible = true; AP3.Visible = true; AP4.Visible = true;
	// 	}
	// }
	// protected void FilterAll(int rs)
	// {
	// 	if (rs > 0)
	// 	{
	// 		A11.Visible = true; A12.Visible = true; A13.Visible = true;
	// 		RG1.Visible = true; RG8.Visible = true; RG3.Visible = true;
	// 		RG4.Visible = true; RG2.Visible = true; RG6.Visible = true;
	// 		RG7.Visible = true;
	// 		RG5.Visible = true; RG9.Visible = true; RE3.Visible = true;
	// 		AP1.Visible = true; AP2.Visible = true; AP3.Visible = true; AP4.Visible = true;
	// 	}
	// }
}