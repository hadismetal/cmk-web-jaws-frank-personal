using System;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;

/// <summary>
/// Customer System
/// </summary>
public partial class Customer
{

    public static string Customer_Create_Dbcmk_Stamps(string mode, int id, string nama, string alamat, string telp, string email, DateTime tgllahir, string cabang, string kodecustomer, string statusmembership, Int32 stampsstatus, Int32 stampspoint, Int32 xStampsID = 0)
    {
        bool status = false;
        int status_flag = 0;
        string msg = "";

        nama = nama.Trim().Replace("'", " ").ToUpper();
        alamat = alamat.Trim().Replace("'", " ").ToUpper();
        telp = telp.Trim().Replace("'", " ").ToUpper();
        email = email.Trim().Replace("'", " ").ToUpper();
        statusmembership = statusmembership.Trim().Replace("'", " ").ToUpper();

        string agama = "00";
        string kota = "";

        using (Map_DB db = new Map_DB())
        using (Map_CS cs = new Map_CS())
        {

            string newkodecust = kodecustomer;

            try
            {
                Db.SingleString("DECLARE	@return_value int," +
                        "@m_kode varchar(20)" +
                        "SELECT	@m_kode = N''" +
                        "EXEC @return_value = sp_savecustomerstamps " +
                        "@m_kode = N'" + newkodecust + "'," +
                        "@m_group = @m_kode," +
                        "@m_nama = N'" + nama + "'," +
                        "@m_alamat = N'" + alamat + "'," +
                        "@m_kota = N'" + kota + "'," +
                        "@m_telepon1 = N'" + telp + "'," +
                        "@m_telepon2 = N''," +
                        "@m_fax = N''," +
                        "@m_email = N'" + email + "'," +
                        "@m_npwp = N''," +
                        "@m_status = N'00'," +
                        "@m_tmplahir = N''," +
                        "@m_tgllahir = N'" + tgllahir.ToString("yyyy-MM-dd") + "'," +
                        "@m_agama = N'" + agama + "'," +
                        "@m_cabang = N'" + cabang + "'," +
                        "@m_kodesales = N''," +
                        "@m_referensi = @m_kode," +
                        "@m_pinbb = N''," +
                        "@m_tanggal = N'" + tgllahir.Day.ToString() + "'," +
                        "@m_bulan = N'" + tgllahir.Month.ToString() + "'," +
                        "@m_tahun = N'" + tgllahir.Year.ToString() + "'," +
                        "@m_ktp = N''," +
                        "@m_validktp = N''," +
                        "@m_email2 = N''," +
    					"@m_stampsstatusmembership = " + stampsstatus.ToString() + ","+ //Tambahan Andri insert status member
                        "@m_stampspoint = " + stampspoint.ToString() + "," + //Tambahan Andri insert point crm
                        "@loginid = N''," +
    					"@m_stampsuserid = " + xStampsID.ToString() + " " +
                        "SELECT	@m_kode as N'@m_kode'" +
                        "SELECT	'Return Value' = @return_value", Db.Cnn_CS);

                status_flag = 1;

            }
            catch (Exception) { }

            if (status_flag == 1 && newkodecust != "")
            {
                if (mode == "Sales")
                {
                    status = Customer.saveLink_SalesOrder(id, newkodecust, telp, email, statusmembership);
                    if (status) { status_flag = 2; } else { status_flag = 3; }
                }
                else if (mode == "TradeIn")
                {
                    status = Customer.saveLink_TradeIn(id, newkodecust, telp, email, statusmembership);
                    if (status) { status_flag = 2; } else { status_flag = 3; }
                }
                else if (mode == "Resell")
                {
                    status = Customer.saveLink_Resell(id, newkodecust, telp, email, statusmembership);
                    if (status) { status_flag = 2; } else { status_flag = 3; }
                }
                else if (mode == "Repair")
                {
                    status = Customer.saveLink_DocRepair(id, newkodecust, telp, email, statusmembership);
                    if (status) { status_flag = 2; } else { status_flag = 3; }
                }
                else if (mode == "PaymentDP")
                {
                    status = Customer.saveLink_SalesReceiptDPPO(id, newkodecust, telp, email, statusmembership);
                    if (status) { status_flag = 2; } else { status_flag = 3; }
                }
                else if (mode == "Titipan")
                {
                    status = Customer.saveLink_DocTitipan(id, newkodecust, telp, email, statusmembership);
                    if (status) { status_flag = 2; } else { status_flag = 3; }
                }
            }

            if (status_flag == 0) { msg = "<div class='alert alert-danger'>Error Code #0x001. Terjadi kesalahan teknis. Pembuatan customer baru gagal dilakukan. Silahkan hubungi Admin untuk bantuan lebih lanjut.</span>"; }
            if (status_flag == 1)
            {
                msg = "<div class='alert alert-warning'>Error Code #0x002. Terjadi kesalahan teknis. Pembuatan customer baru terbentuk di database pusat namun gagal terbentuk di JAWS. Silahkan menggunakan kode customer baru "
                    + (newkodecust != "" ? (Customer.TD_HyperLink(newkodecust, mode, id)) : "")
                    + " untuk melanjutkan transaksi (tanpa meregister form customer baru).</span>";
            }
            if (status_flag == 2) { msg = ""; }
            if (status_flag == 3)
            {
                msg = "<div class='alert alert-warning'>Error Code #0x003. Terjadi kesalahan teknis. Pembuatan customer baru terbentuk di database pusat namun gagal terbentuk di JAWS. Silahkan menggunakan kode customer baru "
                    + (newkodecust != "" ? (Customer.TD_HyperLink(newkodecust, mode, id)) : "")
                    + " untuk melanjutkan transaksi (tanpa meregister form customer baru).</span>";
            }

            return msg;
            }
}

    public static string Customer_Create_Dbcmk(string mode, int id, string nama, string alamat, string kota, string telp, string email, DateTime tgllahir, string agama, string cabang)
    {
        bool status = false;
        int status_flag = 0;
        string msg = "";

        nama = nama.Trim().Replace("'", " ").ToUpper();
        alamat = alamat.Trim().Replace("'", " ").ToUpper();
        kota = kota.Trim().Replace("'", " ").ToUpper();
        telp = telp.Trim().Replace("'", " ").ToUpper();
        email = email.Trim().Replace("'", " ").ToUpper();
        agama = agama.Trim().Replace("'", " ").ToUpper();
        cabang = cabang.Trim().Replace("'", " ").ToUpper();

        using (Map_DB db = new Map_DB())
        using (Map_CS cs = new Map_CS())
        {

            string newkodecust = "";

            try
            {
                newkodecust = Db.SingleString("DECLARE	@return_value int," +
                        "@m_kode varchar(20)" +
                        "SELECT	@m_kode = N''" +
                        "EXEC @return_value = sp_savecustomer " +
                        "@m_kode = @m_kode OUTPUT," +
                        "@m_group = @m_kode," +
                        "@m_nama = N'" + nama + "'," +
                        "@m_alamat = N'" + alamat + "'," +
                        "@m_kota = N'" + kota + "'," +
                        "@m_telepon1 = N'" + telp + "'," +
                        "@m_telepon2 = N''," +
                        "@m_fax = N''," +
                        "@m_email = N'" + email + "'," +
                        "@m_npwp = N''," +
                        "@m_status = N'00'," +
                        "@m_tmplahir = N''," +
                        "@m_tgllahir = N'" + tgllahir.ToString("yyyy-MM-dd") + "'," +
                        "@m_agama = N'" + agama + "'," +
                        "@m_cabang = N'" + cabang + "'," +
                        "@m_kodesales = N''," +
                        "@m_referensi = @m_kode," +
                        "@m_pinbb = N''," +
                        "@m_tanggal = N'" + tgllahir.Day.ToString() + "'," +
                        "@m_bulan = N'" + tgllahir.Month.ToString() + "'," +
                        "@m_tahun = N'" + tgllahir.Year.ToString() + "'," +
                        "@m_ktp = N''," +
                        "@m_validktp = N''," +
                        "@m_email2 = N''," +
                        "@loginid = N''" +
                        "SELECT	@m_kode as N'@m_kode'" +
                        "SELECT	'Return Value' = @return_value", Db.Cnn_CS);

                status_flag = 1;

            }
            catch (Exception) { }

            if (status_flag == 1 && newkodecust != "")
            {
                if (mode == "Sales")
                {
                    status = Customer.saveLink_SalesOrder(id, newkodecust);
                    if (status) { status_flag = 2; } else { status_flag = 3; }
                }
                else if (mode == "TradeIn")
                {
                    status = Customer.saveLink_TradeIn(id, newkodecust);
                    if (status) { status_flag = 2; } else { status_flag = 3; }
                }
                else if (mode == "Resell")
                {
                    status = Customer.saveLink_Resell(id, newkodecust);
                    if (status) { status_flag = 2; } else { status_flag = 3; }
                }
                else if (mode == "Repair")
                {
                    status = Customer.saveLink_DocRepair(id, newkodecust);
                    if (status) { status_flag = 2; } else { status_flag = 3; }
                }
                else if (mode == "PaymentDP")
                {
                    status = Customer.saveLink_SalesReceiptDPPO(id, newkodecust);
                    if (status) { status_flag = 2; } else { status_flag = 3; }
                }
                else if (mode == "Titipan")
                {
                    status = Customer.saveLink_DocTitipan(id, newkodecust);
                    if (status) { status_flag = 2; } else { status_flag = 3; }
                }
            }

            if (status_flag == 0) { msg = "<div class='alert alert-danger'>Error Code #0x001. Terjadi kesalahan teknis. Pembuatan customer baru gagal dilakukan. Silahkan hubungi Admin untuk bantuan lebih lanjut.</span>"; }
            if (status_flag == 1)
            {
                msg = "<div class='alert alert-warning'>Error Code #0x002. Terjadi kesalahan teknis. Pembuatan customer baru terbentuk di database pusat namun gagal terbentuk di JAWS. Silahkan menggunakan kode customer baru "
                    + (newkodecust != "" ? (Customer.TD_HyperLink(newkodecust, mode, id)) : "")
                    + " untuk melanjutkan transaksi (tanpa meregister form customer baru).</span>";
            }
            if (status_flag == 2) { msg = ""; }
            if (status_flag == 3)
            {
                msg = "<div class='alert alert-warning'>Error Code #0x003. Terjadi kesalahan teknis. Pembuatan customer baru terbentuk di database pusat namun gagal terbentuk di JAWS. Silahkan menggunakan kode customer baru "
                    + (newkodecust != "" ? (Customer.TD_HyperLink(newkodecust, mode, id)) : "")
                    + " untuk melanjutkan transaksi (tanpa meregister form customer baru).</span>";
            }

            return msg;
            }
}

    public static bool saveLink_SalesOrder(int id, string kodecust, string telp = "", string email = "", string membershipstatus = "")
    {
        bool xreturn = false;
        using (Map_DB db = new Map_DB())
        using (Map_CS cs = new Map_CS())
        {

            var data = db.SalesOrder.Single(p => p.ID == id && p.Draft);
            var dx = db.DataCustomer.Where(p => p.NoCustomer.Equals(kodecust));

            if (dx.Count() == 0)
            {
                //// create data customer
                //// SBU Brand
                int IDBrand = 1;
                if (data.TipeLokasi == 1)
                {
                    var Brand = db.LocOutlet.SingleOrDefault(p => p.ID == data.IDLokasi);
                    IDBrand = Brand.IDBrand;
                }
                else if (data.TipeLokasi == 2)
                {
                    var Brand = db.LocExhibition.SingleOrDefault(p => p.ID == data.IDLokasi);
                    IDBrand = Brand.IDBrand;
                }

                var cust = cs.Mscustomer.SingleOrDefault(p => p.M_kode.Equals(kodecust));

                string namacust = (cust.M_nama != null) ? cust.M_nama : dx.First().Nama;

                DataCustomer n = new DataCustomer();
                n.Nama = namacust;
                n.NoCustomer = kodecust;
                n.IDBrand = IDBrand;
                n.Keterangan = "";
                n.Tgl = System.DateTime.Now;
                n.AddrNoTelp = telp;
                n.AddrEmail = email;
                n.NoMember = membershipstatus;
                db.DataCustomer.InsertOnSubmit(n);
                db.SubmitChanges();

                data.IDCustomer = n.ID;
                data.CustomerNama = n.Nama;
                db.SubmitChanges();
                xreturn = true;
            }
            else if (dx.Count() == 1)
            {
                dx.ToList().ForEach(c => c.AddrNoTelp = telp);
                dx.ToList().ForEach(c => c.AddrEmail = email);
                dx.ToList().ForEach(c => c.NoMember = membershipstatus);
                db.SubmitChanges();

                //// update data transaction
                data.IDCustomer = dx.First().ID;
                data.CustomerNama = dx.First().Nama;
                db.SubmitChanges();
                xreturn = true;
            }
            else
            {
                dx.ToList().ForEach(c => c.AddrNoTelp = telp);
                dx.ToList().ForEach(c => c.AddrEmail = email);
                dx.ToList().ForEach(c => c.NoMember = membershipstatus);
                db.SubmitChanges();

                //// kondisi data customer lebih dari 1
                try
                {
                    //// execute sp delete duplicate customer non-transaction
                    string SPname = "sp_audit_customer_delete_multiple";

                    Db.Execute("EXEC " + SPname + " "
                        + "@nomor_customer = N'" + kodecust + "'"
                        , Db.Cnn_DB);

                    //// update data transaction
                    data.IDCustomer = dx.First().ID;
                    data.CustomerNama = dx.First().Nama;
                    db.SubmitChanges();
                    xreturn = true;
                }
                catch (Exception)
                {
                    xreturn = false;
                }

            }

            return xreturn;

                }
}
    
    public static bool saveLink_TradeIn(int id, string kodecust, string telp = "", string email = "", string membershipstatus = "")
    {
        bool xreturn = false;
        using (Map_DB db = new Map_DB())
        using (Map_CS cs = new Map_CS())
        {

            var data = db.TradeIn.Single(p => p.ID == id && p.Draft);
            var dx = db.DataCustomer.Where(p => p.NoCustomer.Equals(kodecust));

            if (dx.Count() == 0)
            {
                //// create data customer
                //// SBU Brand
                int IDBrand = 1;
                if (data.TipeLokasi == 1)
                {
                    var Brand = db.LocOutlet.SingleOrDefault(p => p.ID == data.IDLokasi);
                    IDBrand = Brand.IDBrand;
                }
                else if (data.TipeLokasi == 2)
                {
                    var Brand = db.LocExhibition.SingleOrDefault(p => p.ID == data.IDLokasi);
                    IDBrand = Brand.IDBrand;
                }

                var cust = cs.Mscustomer.SingleOrDefault(p => p.M_kode.Equals(kodecust));

                string namacust = (cust.M_nama != null) ? cust.M_nama : dx.First().Nama;

                DataCustomer n = new DataCustomer();
                n.Nama = namacust;
                n.NoCustomer = kodecust;
                n.IDBrand = IDBrand;
                n.Keterangan = "";
                n.Tgl = System.DateTime.Now;
                n.AddrNoTelp = telp;
                n.AddrEmail = email;
                n.NoMember = membershipstatus;
                db.DataCustomer.InsertOnSubmit(n);
                db.SubmitChanges();

                data.IDCustomer = n.ID;
                data.CustomerNama = n.Nama;
                db.SubmitChanges();
                xreturn = true;
            }
            else if (dx.Count() == 1)
            {
                dx.ToList().ForEach(c => c.AddrNoTelp = telp);
                dx.ToList().ForEach(c => c.AddrEmail = email);
                dx.ToList().ForEach(c => c.NoMember = membershipstatus);
                db.SubmitChanges();

                //// update data transaction
                data.IDCustomer = dx.First().ID;
                data.CustomerNama = dx.First().Nama;
                db.SubmitChanges();
                xreturn = true;
            }
            else
            {
                //// kondisi data customer lebih dari 1
                try
                {
                    dx.ToList().ForEach(c => c.AddrNoTelp = telp);
                    dx.ToList().ForEach(c => c.AddrEmail = email);
                    dx.ToList().ForEach(c => c.NoMember = membershipstatus);
                    db.SubmitChanges();

                    //// execute sp delete duplicate customer non-transaction
                    string SPname = "sp_audit_customer_delete_multiple";

                    Db.Execute("EXEC " + SPname + " "
                        + "@nomor_customer = N'" + kodecust + "'"
                        , Db.Cnn_DB);

                    //// update data transaction
                    data.IDCustomer = dx.First().ID;
                    data.CustomerNama = dx.First().Nama;
                    db.SubmitChanges();
                    xreturn = true;
                }
                catch (Exception)
                {
                    xreturn = false;
                }

            }

            return xreturn;

                }
}

    public static bool saveLink_Resell(int id, string kodecust, string telp = "", string email = "", string membershipstatus = "")
    {
        bool xreturn = false;
        using (Map_DB db = new Map_DB())
        using (Map_CS cs = new Map_CS())
        {

            var data = db.Resell.Single(p => p.ID == id && p.Draft);
            var dx = db.DataCustomer.Where(p => p.NoCustomer.Equals(kodecust));

            if (dx.Count() == 0)
            {
                //// create data customer
                //// SBU Brand
                int IDBrand = 1;
                if (data.TipeLokasi == 1)
                {
                    var Brand = db.LocOutlet.SingleOrDefault(p => p.ID == data.IDLokasi);
                    IDBrand = Brand.IDBrand;
                }
                else if (data.TipeLokasi == 2)
                {
                    var Brand = db.LocExhibition.SingleOrDefault(p => p.ID == data.IDLokasi);
                    IDBrand = Brand.IDBrand;
                }

                var cust = cs.Mscustomer.SingleOrDefault(p => p.M_kode.Equals(kodecust));

                string namacust = (cust.M_nama != null) ? cust.M_nama : dx.First().Nama;

                DataCustomer n = new DataCustomer();
                n.Nama = namacust;
                n.NoCustomer = kodecust;
                n.IDBrand = IDBrand;
                n.Keterangan = "";
                n.Tgl = System.DateTime.Now;
                n.AddrNoTelp = telp;
                n.AddrEmail = email;
                n.NoMember = membershipstatus;
                db.DataCustomer.InsertOnSubmit(n);
                db.SubmitChanges();

                data.IDCustomer = n.ID;
                data.CustomerNama = n.Nama;
                db.SubmitChanges();
                xreturn = true;
            }
            else if (dx.Count() == 1)
            {
                dx.ToList().ForEach(c => c.AddrNoTelp = telp);
                dx.ToList().ForEach(c => c.AddrEmail = email);
                dx.ToList().ForEach(c => c.NoMember = membershipstatus);
                db.SubmitChanges();

                //// update data transaction
                data.IDCustomer = dx.First().ID;
                data.CustomerNama = dx.First().Nama;
                db.SubmitChanges();
                xreturn = true;
            }
            else
            {
                //// kondisi data customer lebih dari 1
                try
                {
                    dx.ToList().ForEach(c => c.AddrNoTelp = telp);
                    dx.ToList().ForEach(c => c.AddrEmail = email);
                    dx.ToList().ForEach(c => c.NoMember = membershipstatus);
                    db.SubmitChanges();

                    //// execute sp delete duplicate customer non-transaction
                    string SPname = "sp_audit_customer_delete_multiple";

                    Db.Execute("EXEC " + SPname + " "
                        + "@nomor_customer = N'" + kodecust + "'"
                        , Db.Cnn_DB);

                    //// update data transaction
                    data.IDCustomer = dx.First().ID;
                    data.CustomerNama = dx.First().Nama;
                    db.SubmitChanges();
                    xreturn = true;
                }
                catch (Exception)
                {
                    xreturn = false;
                }

            }

            return xreturn;

                }
}

    public static bool saveLink_DocRepair(int id, string kodecust, string telp = "", string email = "", string membershipstatus = "")
    {
        bool xreturn = false;
        using (Map_DB db = new Map_DB())
        using (Map_CS cs = new Map_CS())
        {

            var data = db.DocRepair.Single(p => p.ID == id && p.Draft.Value);
            var dx = db.DataCustomer.Where(p => p.NoCustomer.Equals(kodecust));

            if (dx.Count() == 0)
            {
                //// create data customer
                //// SBU Brand
                int IDBrand = 1;
                if (data.TipeLokasi == 1)
                {
                    var Brand = db.LocOutlet.SingleOrDefault(p => p.ID == data.IDLokasi);
                    IDBrand = Brand.IDBrand;
                }
                else if (data.TipeLokasi == 2)
                {
                    var Brand = db.LocExhibition.SingleOrDefault(p => p.ID == data.IDLokasi);
                    IDBrand = Brand.IDBrand;
                }

                var cust = cs.Mscustomer.SingleOrDefault(p => p.M_kode.Equals(kodecust));

                string namacust = (cust.M_nama != null) ? cust.M_nama : dx.First().Nama;

                DataCustomer n = new DataCustomer();
                n.Nama = namacust;
                n.NoCustomer = kodecust;
                n.IDBrand = IDBrand;
                n.Keterangan = "";
                n.Tgl = System.DateTime.Now;
                n.AddrNoTelp = telp;
                n.AddrEmail = email;
                n.NoMember = membershipstatus;
                db.DataCustomer.InsertOnSubmit(n);
                db.SubmitChanges();

                data.IDCustomer = n.ID;
                data.CustomerNama = n.Nama;
                db.SubmitChanges();
                xreturn = true;
            }
            else if (dx.Count() == 1)
            {
                dx.ToList().ForEach(c => c.AddrNoTelp = telp);
                dx.ToList().ForEach(c => c.AddrEmail = email);
                dx.ToList().ForEach(c => c.NoMember = membershipstatus);
                db.SubmitChanges();

                //// update data transaction
                data.IDCustomer = dx.First().ID;
                data.CustomerNama = dx.First().Nama;
                db.SubmitChanges();
                xreturn = true;
            }
            else
            {
                //// kondisi data customer lebih dari 1
                try
                {
                    dx.ToList().ForEach(c => c.AddrNoTelp = telp);
                    dx.ToList().ForEach(c => c.AddrEmail = email);
                    dx.ToList().ForEach(c => c.NoMember = membershipstatus);
                    db.SubmitChanges();

                    //// execute sp delete duplicate customer non-transaction
                    string SPname = "sp_audit_customer_delete_multiple";

                    Db.Execute("EXEC " + SPname + " "
                        + "@nomor_customer = N'" + kodecust + "'"
                        , Db.Cnn_DB);

                    //// update data transaction
                    data.IDCustomer = dx.First().ID;
                    data.CustomerNama = dx.First().Nama;
                    db.SubmitChanges();
                    xreturn = true;
                }
                catch (Exception)
                {
                    xreturn = false;
                }

            }

            return xreturn;

                }
}

    public static bool saveLink_SalesReceiptDPPO(int id, string kodecust, string telp = "", string email = "", string membershipstatus = "")
    {
        bool xreturn = false;
        using (Map_DB db = new Map_DB())
        using (Map_CS cs = new Map_CS())
        {

            var data = db.SalesReceiptDPPO.Single(p => p.ID == id && p.Draft.Value);
            var dx = db.DataCustomer.Where(p => p.NoCustomer.Equals(kodecust));

            if (dx.Count() == 0)
            {
                //// create data customer
                //// SBU Brand
                int IDBrand = 1;
                if (data.TipeLokasi == 1)
                {
                    var Brand = db.LocOutlet.SingleOrDefault(p => p.ID == data.IDLokasi);
                    IDBrand = Brand.IDBrand;
                }
                else if (data.TipeLokasi == 2)
                {
                    var Brand = db.LocExhibition.SingleOrDefault(p => p.ID == data.IDLokasi);
                    IDBrand = Brand.IDBrand;
                }

                var cust = cs.Mscustomer.SingleOrDefault(p => p.M_kode.Equals(kodecust));

                string namacust = (cust.M_nama != null) ? cust.M_nama : dx.First().Nama;

                DataCustomer n = new DataCustomer();
                n.Nama = namacust;
                n.NoCustomer = kodecust;
                n.IDBrand = IDBrand;
                n.Keterangan = "";
                n.Tgl = System.DateTime.Now;
                n.AddrNoTelp = telp;
                n.AddrEmail = email;
                n.NoMember = membershipstatus;
                db.DataCustomer.InsertOnSubmit(n);
                db.SubmitChanges();

                data.IDCustomer = n.ID;
                data.CustomerNama = n.Nama;
                db.SubmitChanges();
                xreturn = true;
            }
            else if (dx.Count() == 1)
            {
                dx.ToList().ForEach(c => c.AddrNoTelp = telp);
                dx.ToList().ForEach(c => c.AddrEmail = email);
                dx.ToList().ForEach(c => c.NoMember = membershipstatus);
                db.SubmitChanges();

                //// update data transaction
                data.IDCustomer = dx.First().ID;
                data.CustomerNama = dx.First().Nama;
                db.SubmitChanges();
                xreturn = true;
            }
            else
            {
                //// kondisi data customer lebih dari 1
                try
                {
                    dx.ToList().ForEach(c => c.AddrNoTelp = telp);
                    dx.ToList().ForEach(c => c.AddrEmail = email);
                    dx.ToList().ForEach(c => c.NoMember = membershipstatus);
                    db.SubmitChanges();

                    //// execute sp delete duplicate customer non-transaction
                    string SPname = "sp_audit_customer_delete_multiple";

                    Db.Execute("EXEC " + SPname + " "
                        + "@nomor_customer = N'" + kodecust + "'"
                        , Db.Cnn_DB);

                    //// update data transaction
                    data.IDCustomer = dx.First().ID;
                    data.CustomerNama = dx.First().Nama;
                    db.SubmitChanges();
                    xreturn = true;
                }
                catch (Exception)
                {
                    xreturn = false;
                }

            }

            return xreturn;

                }
}

    public static bool saveLink_DocTitipan(int id, string kodecust, string telp = "", string email = "", string membershipstatus = "")
    {
        bool xreturn = false;
        using (Map_DB db = new Map_DB())
        using (Map_CS cs = new Map_CS())
        {

            var data = db.DocTitipan.Single(p => p.ID == id && p.Draft.Value);
            var dx = db.DataCustomer.Where(p => p.NoCustomer.Equals(kodecust));

            if (dx.Count() == 0)
            {
                //// create data customer
                //// SBU Brand
                int IDBrand = 1;
                if (data.TipeLokasi == 1)
                {
                    var Brand = db.LocOutlet.SingleOrDefault(p => p.ID == data.IDLokasi);
                    IDBrand = Brand.IDBrand;
                }
                else if (data.TipeLokasi == 2)
                {
                    var Brand = db.LocExhibition.SingleOrDefault(p => p.ID == data.IDLokasi);
                    IDBrand = Brand.IDBrand;
                }

                var cust = cs.Mscustomer.SingleOrDefault(p => p.M_kode.Equals(kodecust));

                string namacust = (cust.M_nama != null) ? cust.M_nama : dx.First().Nama;

                DataCustomer n = new DataCustomer();
                n.Nama = namacust;
                n.NoCustomer = kodecust;
                n.IDBrand = IDBrand;
                n.Keterangan = "";
                n.Tgl = System.DateTime.Now;
                n.AddrNoTelp = telp;
                n.AddrEmail = email;
                n.NoMember = membershipstatus;
                db.DataCustomer.InsertOnSubmit(n);
                db.SubmitChanges();

                data.IDCustomer = n.ID;
                data.CustomerNama = n.Nama;
                db.SubmitChanges();
                xreturn = true;
            }
            else if (dx.Count() == 1)
            {
                dx.ToList().ForEach(c => c.AddrNoTelp = telp);
                dx.ToList().ForEach(c => c.AddrEmail = email);
                dx.ToList().ForEach(c => c.NoMember = membershipstatus);
                db.SubmitChanges();

                //// update data transaction
                data.IDCustomer = dx.First().ID;
                data.CustomerNama = dx.First().Nama;
                db.SubmitChanges();
                xreturn = true;
            }
            else
            {
                //// kondisi data customer lebih dari 1
                try
                {
                    dx.ToList().ForEach(c => c.AddrNoTelp = telp);
                    dx.ToList().ForEach(c => c.AddrEmail = email);
                    dx.ToList().ForEach(c => c.NoMember = membershipstatus);
                    db.SubmitChanges();

                    //// execute sp delete duplicate customer non-transaction
                    string SPname = "sp_audit_customer_delete_multiple";

                    Db.Execute("EXEC " + SPname + " "
                        + "@nomor_customer = N'" + kodecust + "'"
                        , Db.Cnn_DB);

                    //// update data transaction
                    data.IDCustomer = dx.First().ID;
                    data.CustomerNama = dx.First().Nama;
                    db.SubmitChanges();
                    xreturn = true;
                }
                catch (Exception)
                {
                    xreturn = false;
                }

            }

            return xreturn;

                }
}

    protected static string TD_HyperLink(string kodecust, string mode, int id)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"" + mode + "_DataCustomerSave.aspx?id=" + id + "&kdcust=" + kodecust + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }

    public static bool isValidHPNumber(string x, int mode = 1)
    {
        bool status = true;

        x = x.Trim();

        if (status && (x == "")) { status = false; } else { status = true; }
        if (status && (x.Length < 4)) { status = false; } else { status = true; }
        if (status && mode == 2 && (x.Length < 9 || x.Length > 13)) { status = false; } else { status = true; }
        if (x.Length > 4 && mode == 2)
        {
            if (status && (x.Substring(0, 4) == "+628" || x.Substring(0, 3) == "628" || x.Substring(0, 2) == "08")) { status = true; } else { status = false; }
        }
        if (status && (x.All(char.IsDigit))) { status = true; } else { status = false; }

        return status;
    }

    public static bool isValidEmail(string x, int mode = 1)
    {
        bool status = true;

        x = x.Trim();

        if (status && (x == "") && mode == 1) { status = false; } else { status = true; }
        if (status && (x.Length < 5 || x.Length > 100)) { status = false; } else { status = true; }
        try
        {
            status = (isValidEmailMainFunction(x));

            //var addr = new System.Net.Mail.MailAddress(x);
            //status = addr.Address == x;
        }
        catch
        {
            status = false;
        }

        return status;
    }

    public static string reformatHPNumber(string x)
    {
        if (isValidHPNumber(x))
        {
            if (x.Length > 2)
            {
                if (x.Substring(0, 2) == "08")
                {
                    x = "628" + x.Substring(2);
                }
            }

            if (x.Length > 4)
            {
                if (x.Substring(0, 4) == "+628")
                {
                    x = "628" + x.Substring(4);
                }
            }
        }
        return x;
    }

    public static bool isValidEmailMainFunction(string strIn)
    {
        bool invalid = false;
        invalid = false;
        if (String.IsNullOrEmpty(strIn))
            return false;

        // Use IdnMapping class to convert Unicode domain names.
        try
        {
            strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }

        if (invalid)
            return false;

        // Return true if strIn is in valid e-mail format.
        try
        {
            return Regex.IsMatch(strIn,
                  @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                  RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private static string DomainMapper(Match match)
    {
        bool invalid = false;
        // IdnMapping class with default property values.
        IdnMapping idn = new IdnMapping();

        string domainName = match.Groups[2].Value;
        try
        {
            domainName = idn.GetAscii(domainName);
        }
        catch (ArgumentException)
        {
            invalid = true;
        }
        return match.Groups[1].Value + domainName;
    }
}

public class Stamps
{
    public List<StampsMembership> membership { get; set; }
    public List<StampsUser> user { get; set; }
}

public class StampsMembership
{
    public string tags { get; set; }
    public string status { get; set; }
    public string status_text { get; set; }
    public string stamps { get; set; }
    public string balance { get; set; }
    public string is_blocked { get; set; }
    public string referral_code { get; set; }
    public string start_date { get; set; }
    public string created { get; set; }
}
public class StampsUser
{

    public string id { get; set; }
    public string name { get; set; }
    public string gender { get; set; }
    public string address { get; set; }
    public string is_active { get; set; }
    public string email { get; set; }
    public string picture_url { get; set; }
    public string birthday { get; set; }
    public string phone { get; set; }
    public string protected_redemption { get; set; }
    public string has_incorrect_email { get; set; }
    public string member_ids { get; set; }
}