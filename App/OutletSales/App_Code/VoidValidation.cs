using System;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

/// <summary>
/// Customer System
/// </summary>
public partial class VoidValidation
{

    public static bool validVoidSalesOrderRole(SalesOrder r, string Module = "pos")
    {
        bool result = false;

        if (((Module == "acc" && r.Tgl < DateTime.Today))
            && r.StatusVoid == false
        )
        {
            result = true;
        }

        if (((Module == "pos" && r.Tgl >= DateTime.Today))
            && r.StatusVoid == false
        )
        {
            result = true;
        }

        return result;
    }
	
	public static bool validVoidSalesOrderRole(int id, string Module = "pos")
	{
		using (Map_DB db = new Map_DB())
		{

			SalesOrder r = db.SalesOrder.SingleOrDefault(p => p.ID == id);
			bool result = false;

			if (((Module == "acc" && r.Tgl < DateTime.Today))
				&& r.StatusVoid == false
			)
			{
				result = true;
			}

			if (((Module == "pos" && r.Tgl >= DateTime.Today))
				&& r.StatusVoid == false
			)
			{
				result = true;
			}

			return result;
			}
	}

    public static bool validVoidSalesOrderNoResell(SalesOrder r)
    {
        using (Map_DB db = new Map_DB())
        {

            bool result = false;

            var dj = r.SalesOrderDJ.Select(p => p.IDProduct);
            var pg = r.SalesOrderPG.Select(p => p.IDProduct);
            var gj = r.SalesOrderGJ.Select(p => p.IDProduct);
            var ld = r.SalesOrderLD.Select(p => p.IDProduct);

            var r_dj = from p in db.ResellDJ where dj.Contains(p.IDProduct) && p.Resell.Draft == false select p;
            var r_pg = from p in db.ResellPG where pg.Contains(p.IDProduct) && p.Resell.Draft == false select p;
            var r_gj = from p in db.ResellGJ where gj.Contains(p.IDProduct) && p.Resell.Draft == false select p;
            var r_ld = from p in db.ResellLD where ld.Contains(p.IDProduct) && p.Resell.Draft == false select p;

            if (r_dj.Count() == 0 && r_pg.Count() == 0 && r_gj.Count() == 0 && r_ld.Count() == 0)
            {
                result = true;
            }

            return result;
            }
    }

    public static bool validVoidSalesOrderNoVoucherExpire(SalesOrder r)
    {
        using (Map_DB db = new Map_DB())
        using (Map_CS cs = new Map_CS())
        {

            bool result = false;

            var s = db.SalesReceiptDetail.Where(p => p.SalesReceipt.SalesOrder.ID == r.ID).Select(p => p.CCNumber).ToList();

            var d = from p in cs.T_voucher where s.Contains(p.M_nomor) && p.M_expiredate < System.DateTime.Now select p;

            if (d.Count() == 0) { result = true; }

            return result;
            }
    }

    public static bool validVoidSalesOrderNoMoreThanOneYear(SalesOrder r)
    {
        bool result = false;

        if (r.Tgl >= System.DateTime.Now.AddYears(-1)) { result = true; }

        return result;
    }

    public static bool validVoidSalesOrderPayment(SalesOrder r)
    {
        using (Map_DB db = new Map_DB())
        {

            bool result = false;

            var sr = db.SalesReceiptDetail.Where(p => p.SalesReceipt.SalesOrder == r);

            if(sr.Sum(p => p.Nominal) == 0)
            {
                result = true;
            }
        
            return result;
            }
    }

}
