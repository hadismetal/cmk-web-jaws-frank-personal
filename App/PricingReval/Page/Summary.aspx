<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Summary.aspx.cs" inherits="Page_Summary" %>

<asp:content contentplaceholderid="h1" runat="server">
	Summary
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <div class="row">
        <div class="col-xs-12 col-sm-6">
            <div id="content_legacy_ERR">
		        <i class="icon-warning-sign icon-5x pull-left text-red"></i>
		        <h4 style="color:red;">Announcement</h4>
		        <p class="text-muted">
			        Proses revaluasi harga (bulk), baru dapat dilakukan pada jam 22.00 - 08.00
		        </p>
		        <div class="clearfix"></div>
	        </div>
        </div>
        <div class="col-xs-12 col-sm-6">

        </div>
    </div>
</asp:content>

