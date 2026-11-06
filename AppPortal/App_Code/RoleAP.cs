public class RoleAP {
	//Display
	public static bool Open(string Module) { return AC.Role(Module); }
	public static bool OpenLog(string Module) { return AC.Role(Module, "Manager"); }

	//Input / Delete
	public static bool Delete(string Module) { return AC.Role(Module, "Manager"); }
	public static bool Create(string Module) { return AC.Role(Module, "Operator"); }

	//Download
	public static bool Download(string Module) { return AC.Role(Module, "DownloadFile"); }
}