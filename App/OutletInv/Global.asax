<%@ Application Language="C#" %>

<script RunAt="server">
    void Application_Start(object sender, EventArgs e) { ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None; }
    void Application_Error(object sender, EventArgs e) { Common.ErrorReporting(); JAWSMailSystem.EmailTheException(Server.GetLastError()); }
</script>
