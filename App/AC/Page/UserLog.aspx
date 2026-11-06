<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="UserLog.aspx.cs" inherits="Page_UserLog" %>

<%@ register src="MenuFile.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	User Account
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	<asp:literal id="h1_st" runat="server" />
</asp:content>
<asp:content contentplaceholderid="h1_id" runat="server">
	ID:<asp:literal id="h1_id" runat="server" />
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Periode :
			</div>
			<asp:dropdownlist id="bulan" runat="server" cssclass="form-control" width="150" />
			<asp:dropdownlist id="tahun" runat="server" cssclass="form-control" width="100" />
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
			</div>
		</div>
	</form>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-report">
			<asp:tableheaderrow>
				<asp:tableheadercell width="50">Tgl.</asp:tableheadercell>
				<asp:tableheadercell width="100">Hari</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

