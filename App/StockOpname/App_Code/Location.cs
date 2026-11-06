using System;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Web.UI.WebControls;

/// <summary>
/// Customer System
/// </summary>
public partial class Location
{
    public static void ddlLocation(DropDownList c, int mode, string role
        , int locationAE_Tipe = 0, int locationAE_ID = 0
        , int[] locationAM = null
        , bool isAllLocationEnabled = false
        )
    {
        /// mode list :
        /// 1 = ALL (W, O, E)
        /// 2 = POS ONLY (O, E)
        /// 3 = WAREHOUSE ONLY (W)
        /// 4 = STORE ONLY (O)
        /// 5 = EXHIBITION ONLY (E)
        /// 

        /// role list : AE / AM / -

        using (Map_DB db = new Map_DB())
        {

            c.Items.Clear();

            if(isAllLocationEnabled)
            {
                c.Items.Add(new ListItem("...", "9"));
            }

            /// Warehouse
            if (new[] { 1, 3 }.Contains(mode))
            {
                var rs1 = from p in db.LocWarehouse where !p.Draft && !p.Disable orderby p.Nama select p;
                foreach (var r in rs1)
                {
                    string t = r.Nama + " (W)";
                    string v = "0" + r.ID.ToString();
                    c.Items.Add(new ListItem(t, v));
                }
            }

            // Store
            if (new[] { 1, 2, 4 }.Contains(mode))
            {
                if(role == "AE" && locationAE_Tipe == 1)
                {
                    var rs2 = from p in db.LocOutlet where !p.Draft && !p.Disable && p.ID == locationAE_ID orderby p.Nama select p;
                    foreach (var r in rs2)
                    {
                        string t = r.Kode + " - " + r.Nama + " (O)";
                        string v = "1" + r.ID.ToString();
                        c.Items.Add(new ListItem(t, v));
                    }
                }
                else if (role == "AM")
                {
                    var rs2 = from p in db.LocOutlet where !p.Draft && !p.Disable && locationAM.Contains(p.ID) orderby p.Nama select p;
                    foreach (var r in rs2)
                    {
                        string t = r.Kode + " - " + r.Nama + " (O)";
                        string v = "1" + r.ID.ToString();
                        c.Items.Add(new ListItem(t, v));
                    }
                }
                else
                {
                    var rs2 = from p in db.LocOutlet where !p.Draft && !p.Disable orderby p.Nama select p;
                    foreach (var r in rs2)
                    {
                        string t = r.Kode + " - " + r.Nama + " (O)";
                        string v = "1" + r.ID.ToString();
                        c.Items.Add(new ListItem(t, v));
                    }
                }
            }

            // Exhibition
            if (new[] { 1, 2, 5 }.Contains(mode))
            {
                if (role == "AE" && locationAE_Tipe == 2)
                {
                    var rs3 = from p in db.LocExhibition where !p.Draft && !p.Disable && p.ID == locationAE_ID orderby p.Nama select p;
                    foreach (var r in rs3)
                    {
                        string t = r.Kode + " - " + r.Nama + " (E)";
                        string v = "2" + r.ID.ToString();
                        c.Items.Add(new ListItem(t, v));
                    }
                }
                else
                {
                    var rs3 = from p in db.LocExhibition where !p.Draft && !p.Disable orderby p.Nama select p;
                    foreach (var r in rs3)
                    {
                        string t = r.Kode + " - " + r.Nama + " (E)";
                        string v = "2" + r.ID.ToString();
                        c.Items.Add(new ListItem(t, v));
                    }
                }
            
            }
        
                }
}

}