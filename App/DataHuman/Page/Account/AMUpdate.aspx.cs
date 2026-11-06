using System;
using System.Linq;

public partial class Page_Account_AMUpdate : System.Web.UI.Page {
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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			nama.Focus();

			

			//Form
			fillForm();

			//Action
			save.Visible = Role.Operator;
			App.ConfirmDelete(del, Role.Manager);
		}
	}
	protected void fillForm() {
        DLC.User(username);
        
		var d = db.DataAreaManager.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
            if (d.IDUser != null)
                username.SelectedValue = d.IDUser.ToString();

			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);

			//Account
			nama.Text = d.Nama;
			keterangan.Text = d.Keterangan;
			tgl.Text = Str.Tgl(d.Tgl);

			//Disable
			if (d.Draft) div_disable.Visible = false;
			else {
				div_draft.Visible = false;
				disable.Checked = d.Disable;
			}

            string Script = "<script>";
            Script += "$(document).ready(function(){";

            foreach(var r in d.DataAreaManager_Lokasi)
            {
                string Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi ,Db.Cnn_DB);
                Script += "$('#outletlist').append('<option value=\"" + r.IDLokasi.ToString() + "\">" + Lokasi + "</option>');";
            }
            Script += "$('#outletlist option').prop('selected', true);";
            Script += "});";
            Script += "</script>";
            scripting.Text = Script;
		}
	}

	//Save
	protected bool valid {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.DataAreaManager.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
				//Master
                d.IDUser = Convert.ToInt32(username.SelectedValue);

				//Account
				d.Nama = Str.CapTitle(nama.Text);
				d.Keterangan = Str.CapFirst(keterangan.Text);
				d.Tgl = Convert.ToDateTime(tgl.Text);
				d.Disable = disable.Checked;

				//Commit
				db.SubmitChanges();

                db.DataAreaManager_Lokasi.DeleteAllOnSubmit(d.DataAreaManager_Lokasi);
                db.SubmitChanges();

                string itemlist = Request.Form["outletlist"];
                string[] item = itemlist.Split(',');
                if (item.Count() > 0)
                {
                    for (int i = 0; i < item.Count(); i++)
                    {
                        DataAreaManager_Lokasi l = new DataAreaManager_Lokasi();
                        l.IDForm = d.ID;
                        l.IDLokasi = Convert.ToInt32(item[i]);
                        l.TipeLokasi = 1;
                        db.DataAreaManager_Lokasi.InsertOnSubmit(l);
                        db.SubmitChanges();
                    }
                }

				//Redirect
				App.Reload(this, myID);
			}
		}
	}
	protected void del_Click(object sender, EventArgs e) {
		var d = db.DataAreaManager.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			try {
				//Commit
                db.DataAreaManager_Lokasi.DeleteAllOnSubmit(d.DataAreaManager_Lokasi);
				db.DataAreaManager.DeleteOnSubmit(d);
				db.SubmitChanges();

				//Appendix
				AP.Delete("DataAreaManager", myID);

				//Redirect
				App.ReloadClose(this);
			}
			catch { nodel.Visible = true; }
		}
	}
}