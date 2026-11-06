﻿using System;
using System.Linq;

public partial class Menu : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
			App.MenuPath(a1, "/Page/Register/");
			App.MenuPath(a2, "/Page/Update/");
			App.MenuPath(a3, "/Page/Approval/");
            App.MenuPath(a5, "/Page/Report/");
            App.MenuPath(a6, "/Page/Summary/");

            consignment();

            //Role Protection
            a1.Visible = Role.Operator;
            a2.Visible = Role.Operator;
			a3.Visible = Role.Manager;
            //a5.Visible = Role.Manager;
			a5.Visible = (Role.DownloadExcel || Role.DownloadFile);
			
            if (Lib.isTimeBetween(System.DateTime.Now, new TimeSpan(8, 40, 0), new TimeSpan(9, 10, 0))
             || Lib.isTimeBetween(System.DateTime.Now, new TimeSpan(12, 10, 0), new TimeSpan(12, 40, 0))
             || Lib.isTimeBetween(System.DateTime.Now, new TimeSpan(16, 10, 0), new TimeSpan(16, 40, 0))
                )
            {
                // a80.HRef = "Page/Summary.aspx";
                // a80.Style.Add("color", "red");
            }

        }
    }
    protected void consignment()
    {
        using (Map_AC ac = new Map_AC())
        {
            UserAccount user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            if (user != null)
            {
                string jabatan = user.Keterangan.ToString();
                if(jabatan.StartsWith("IT"))
                {
                    konsimenjaja.Visible = true;                	
                }
                else
                {
                    konsimenjaja.Visible = false;                	
                }
            }
            else 
            {
                konsimenjaja.Visible = false;                
            }
        }  
    }
}