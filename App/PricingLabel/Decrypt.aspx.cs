using System;
using System.Linq;

public partial class _Default : System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            
		}
	}
    protected void Decrypt(object sender, EventArgs e)
    {
        EncryptDecrypt ec = new EncryptDecrypt();

        Response.Write(ec.Decrypt(from.Text));
    }
}