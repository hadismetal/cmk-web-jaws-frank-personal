public class Role
{
	public static bool Manager { get { return AC.RoleSession("Manager"); } }
	public static bool Operator { get { return AC.RoleSession("Operator"); } }
	public static bool DownloadFile { get { return AC.RoleSession("DownloadFile"); } }
	public static bool DownloadExcel { get { return AC.RoleSession("DownloadExcel"); } }
}