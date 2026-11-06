using System;
using System.Linq;
using System.Web.UI.WebControls;

public class Lib {
	//Recalculate

    public static void RecalWeight(Map_DB db, ProductOrder d, ProductOrder_CharProduct product)
    {
        //Master
        decimal totalCarat = 0;

        //Stone
        var rs_1A = from p in d.ProductOrder_Stone1A select p.TotalCarat;
        foreach (var r in rs_1A) totalCarat += r;

        var rs_1B = from p in d.ProductOrder_Stone1B where p.StatusTerpasang == 1 select p.TotalCarat;
        foreach (var r in rs_1B) totalCarat += r;

        var rs_2 = from p in d.ProductOrder_Stone2 select p.TotalCarat;
        foreach (var r in rs_2) totalCarat += r;

        var rs_3 = from p in d.ProductOrder_Stone3 select p.TotalCarat;
        foreach (var r in rs_3) totalCarat += r;

        var rs_4 = from p in d.ProductOrder_Stone4 select p.TotalCarat;
        foreach (var r in rs_4) totalCarat += r;

        var rs_5 = from p in d.ProductOrder_Stone5 select p.TotalCarat;
        foreach (var r in rs_5) totalCarat += r;

        //Save to Database
        product.StoneCarat = totalCarat;
        product.StoneWeight = totalCarat * (decimal)0.2;
        product.NettoWeight = product.GrossWeight - product.StoneWeight;

        //Commit
        db.SubmitChanges();
    }

    public static void Designer(DropDownList c)
    {
        using (Map_DB db = new Map_DB())
        using (Map_AC ac = new Map_AC())
        {
            var activeUser = from p in ac.UserAccount where p.Status == 1 orderby p.Nama select p.ID;
            var rs = db.DataHuman.Where(x => !x.Disable);
            foreach (var r in rs)
            {
                if (!activeUser.Any(x => x.Equals(r.IDUser))) continue;
                string t = r.Nama + " (" + r.DataHumanGroup.Nama + ")";
                string v = r.ID.ToString();
                c.Items.Add(new ListItem(t, v));
            }
            }
}
}