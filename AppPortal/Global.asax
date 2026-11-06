<%@ Application Language="C#" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Net.Mail" %>
<%@ Import Namespace="System.Net.Configuration" %>
<%@ Import Namespace="System.Configuration" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e) { ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None; }
    void Application_Error(object sender, EventArgs e)
    {
        //Common.ErrorReporting(); 

        Exception ex = Server.GetLastError();
        //Exception ex = Server.GetLastError().GetBaseException();
        EmailTheException(ex);
        
    }

    protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
    {
        HttpContext.Current.Response.Headers.Remove("X-Powered-By");
        HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
        HttpContext.Current.Response.Headers.Remove("Server"); // Optionally remove the Server header too
    }

    private void EmailTheException(Exception ex)
    {
        string sUrl = "";
        string sPage = "";

        string ErrorMessage = "";
        //string Source = "";
        string StackTrace = "";
        string ExceptionType = "";
        
        // Code that runs when an unhandled error occurs
        if (HttpContext.Current != null)
        {
            sUrl = HttpContext.Current.Request.Url.ToString();
            //sPage = HttpContext.Current.Handler as System.Web.UI.Page;
        }

        ErrorMessage = ex.Message;
        StackTrace = ex.StackTrace;
        ExceptionType = ex.GetType().FullName;

        string TextBody = "";
        TextBody += "<b>Url</b> : <br />" + sUrl + "<br /><br />";
        TextBody += "<b>ErrorMessage</b> : <br />" + ErrorMessage + "<br /><br />";
        //TextBody += "<b>StackTrace</b> : <br />" + StackTrace + "<br /><br />";
        //TextBody += "<b>ExceptionType</b> : <br />" + ExceptionType + "<br /><br />";

        string mail1 = System.Configuration.ConfigurationManager.AppSettings["EmailAdmin1"];
        string mail2 = System.Configuration.ConfigurationManager.AppSettings["EmailAdmin2"];
        string mail3 = System.Configuration.ConfigurationManager.AppSettings["EmailAdmin3"];

        if (mail1 != "" && mail2 != "" && mail3 != "")
        {
            string name1 = System.Configuration.ConfigurationManager.AppSettings["NameAdmin1"];
            string name2 = System.Configuration.ConfigurationManager.AppSettings["NameAdmin2"];
            string name3 = System.Configuration.ConfigurationManager.AppSettings["NameAdmin3"];

            try
            {
                var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                string username = smtpSection.Network.UserName;
                string password = smtpSection.Network.Password;

                var smtp = new System.Net.Mail.SmtpClient();
                var host = smtp.Host;
                var ssl = smtp.EnableSsl;
                var port = smtp.Port;

                MailMessage message = new MailMessage();
                message.From = new MailAddress(username, "JAWS Mail System");
                if (mail1 != "") { message.To.Add(new MailAddress(mail1, name1)); }
                if (mail2 != "") { message.To.Add(new MailAddress(mail2, name2)); }
                if (mail3 != "") { message.To.Add(new MailAddress(mail3, name3)); }
                message.Subject = "[JAWS] Error";
                message.IsBodyHtml = true;
                message.Body = TextBody;

                //var credential = new System.Net.Configuration.SmtpSection().Network;
                //var username = credential.UserName;
                //var password = credential.Password;

                var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password)
                    ,
                    EnableSsl = true

                };
                client.Send(message);

            }
            catch (SmtpException smtpEx)
            {
                
            }

        }
    }
</script>
