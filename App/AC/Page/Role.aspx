<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Role.aspx.cs" inherits="Page_Role" %>

<asp:content contentplaceholderid="h1" runat="server">
	Role Settings
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Module Group :
			</div>
			<asp:dropdownlist id="group" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
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
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="90" />
				<asp:tableheadercell width="250">Module</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-center">Manager</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-center">Operator</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-center">Visitor</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-center">Total</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

