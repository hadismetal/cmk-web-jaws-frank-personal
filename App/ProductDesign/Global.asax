<%@ application language="C#" %>

<script runat="server">
	void Application_Start(object sender, EventArgs e) { ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None; }
	void Application_Error(object sender, EventArgs e) { Common.ErrorReporting(); }
</script>
