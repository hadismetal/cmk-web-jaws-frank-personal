public class Role {
	public static bool Manager { get { return AC.RoleSession("Manager"); } }
	public static bool Operator { get { return AC.RoleSession("Operator"); } }
	public static bool DownloadFile { get { return AC.RoleSession("DownloadFile"); } }
	public static bool DownloadExcel { get { return AC.RoleSession("DownloadExcel"); } }

    public partial class DL
    {
        public static string SubStorage(int input)
        {
            string x = string.Empty;
            switch (input)
            {
                case 1: x = "Approve"; break;
                case 2: x = "Reject"; break;
                case 3: x = "Outstanding"; break;
                case 4: x = "Pending Approval"; break;           
            }
            return x;
        }
    }
}