<%@ Application Language="C#" %>

<script RunAt="server">
    void Application_Start(object sender, EventArgs e)
    {
        ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        StampsBacklogWorker.Start();
    }
    void Application_Error(object sender, EventArgs e)
    {
        // Common.ErrorReporting();
        // Error email intentionally disabled for OutletSales (Sales Order finalization
        // is prone to error bursts under load; synchronous SmtpClient.Send in
        // JAWSMailSystem.EmailTheException made it worse). Scope is this module only —
        // each App/* folder has its own Global.asax/IIS application.
        LogUnhandledError(Server.GetLastError());
    }

    // Tidak ada pola logging exception generic (SP/tabel) yang sudah dipakai di codebase ini
    // sebelum perubahan ini (sudah dicek: tidak ada "_log_"/ErrorLog table di App_Code atau
    // catch block manapun di Page code). Jadi dibuat baru, minimal, self-provisioning (bikin
    // tabelnya sendiri kalau belum ada) supaya tidak butuh migrasi DB manual terpisah.
    private static bool _appErrorLogTableEnsured = false;
    private static readonly object _appErrorLogLock = new object();

    void EnsureAppErrorLogTable()
    {
        if (_appErrorLogTableEnsured) return;
        lock (_appErrorLogLock)
        {
            if (_appErrorLogTableEnsured) return;
            using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(Db.Cnn_DB))
            using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(
                @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AppErrorLog')
                  CREATE TABLE dbo.AppErrorLog (
                      ID INT IDENTITY(1,1) PRIMARY KEY,
                      ErrorTime DATETIME NOT NULL,
                      RequestPath VARCHAR(500) NULL,
                      ExceptionMessage VARCHAR(MAX) NULL,
                      StackTrace VARCHAR(MAX) NULL
                  )", cnn))
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
            }
            _appErrorLogTableEnsured = true;
        }
    }

    void LogUnhandledError(Exception ex)
    {
        try
        {
            if (ex == null) return;
            Exception real = ex.GetBaseException() ?? ex;

            string path = "";
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    path = HttpContext.Current.Request.Path;
            }
            catch { /* Request bisa null/unavailable di sebagian error lifecycle */ }

            EnsureAppErrorLogTable();

            using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(Db.Cnn_DB))
            using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(
                "INSERT INTO dbo.AppErrorLog (ErrorTime, RequestPath, ExceptionMessage, StackTrace) VALUES (GETDATE(), @RequestPath, @ExceptionMessage, @StackTrace)", cnn))
            {
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RequestPath", System.Data.SqlDbType.VarChar, 500)).Value = (object)path ?? DBNull.Value;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ExceptionMessage", System.Data.SqlDbType.VarChar, -1)).Value = (object)real.Message ?? DBNull.Value;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@StackTrace", System.Data.SqlDbType.VarChar, -1)).Value = (object)real.StackTrace ?? DBNull.Value;
                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch
        {
            // Logging sendiri tidak boleh sampai menjatuhkan Application_Error — kalau gagal
            // (DB down, permission, dll), exception ini dibiarkan hilang saja.
        }
    }
</script>
