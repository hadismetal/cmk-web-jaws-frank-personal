<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="RoleUserAdd.aspx.cs" inherits="Page_RoleUserAdd" %>

<asp:content contentplaceholderid="h1" runat="server">
	Role Settings
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Add User
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div class="row">
		<div class="col-md-3">
			<p class="text-nowrap">
				<span class="text-warning" id="header_group" runat="server" />
				<br />
				<span class="text-lg" id="header_module" runat="server" />
			</p>
		</div>
	</div>
	<hr />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
				<a href="javascript:closeWindow();" class="btn btn-default">
					<i class="icon-remove"></i>
					Cancel
				</a>
			</div>
		</div>
	</form>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="90" />
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Nama</asp:tableheadercell>
				<asp:tableheadercell width="250">Alamat Email</asp:tableheadercell>
				<asp:tableheadercell width="250">Keterangan</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

