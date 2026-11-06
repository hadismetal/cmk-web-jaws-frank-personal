public class Log {
	public static void AccessSignIn(int Aktivitas) { Access(App.UserID, App.UserNama, Aktivitas); }
	public static void Access(int UserID, string UserNama, int Aktivitas) {
		using (Map_AC db = new Map_AC())
		{
			AccessLog n = new AccessLog();

			n.Tgl = System.DateTime.Now;
			n.UserID = UserID;
			n.UserNama = UserNama;
			n.Aktivitas = Aktivitas;
			n.IPAddr = App.IPAddr;

			//Commit
			db.AccessLog.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
}